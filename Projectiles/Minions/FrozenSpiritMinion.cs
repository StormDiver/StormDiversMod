using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Projectiles.Minions
{


	public class FrozenSpiritMinionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Frozen Spirit Minion");
			//Description.SetDefault("A Frozen Spirit minion will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<FrozenSpiritMinionProj>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}


	//_______________________________________________________________________
	public class FrozenSpiritMinionProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Frozen Spirit Minion");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			// Makes the minion go through tiles freely
			Projectile.tileCollide = false;

			// These below are needed for a minion weapon
			// Only controls if it deals damage to enemies on contact (more on that later)
			Projectile.friendly = true;
			// Only determines the damage type
			Projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.minionSlots = 1f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Summon;

		}

		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return false;
		}
		int xpos = 0;
		bool moveright = true;


		public override void AI()
		{
			Player player = Main.player[Projectile.owner];


			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<FrozenSpiritMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<FrozenSpiritMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 80f; // above the center of the player)

			// If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
			// The index is Projectile.minionPos
			float minionPositionOffsetX = (10 + Projectile.minionPos * 20) * -player.direction;
			idlePosition.X += minionPositionOffsetX; // Go behind the player


			// Teleport to player if distance is too big
			Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
			{
				// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the Projectile,
				// and then set netUpdate to true
				Projectile.position = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}

			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.04f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
				{
					if (Projectile.position.X < other.position.X) Projectile.velocity.X -= overlapVelocity;
					else Projectile.velocity.X += overlapVelocity;

					if (Projectile.position.Y < other.position.Y) Projectile.velocity.Y -= overlapVelocity;
					else Projectile.velocity.Y += overlapVelocity;
				}
			}

			// Starting search distance
			Vector2 targetCenter = Projectile.position; //Flowing above enemy
			Vector2 targetNPC = Projectile.position; //Shooting at enemy
			bool foundTarget = false;

			if (xpos > 250)
			{
				moveright = false;
			}
			if (xpos < -250)
			{
				moveright = true;
			}

			if (moveright)
			{
				xpos += 3;
			}
			else
			{
				xpos -= 3;
			}
			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				// Reasonable distance away so it doesn't target across multiple screens
				if (between < 2000f)
				{
					targetCenter = npc.Center + new Vector2(xpos, Main.rand.Next(-150, -40));
					targetNPC = npc.Center + new Vector2(0, 0);

					foundTarget = true;
				}
			}
			if (!foundTarget)
			{
				

				// This code is required either way, used for finding a target
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy())
					{
						float npcDistance = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > npcDistance;

						if (closest || !foundTarget)
						{
							// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
							// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
							bool closeThroughWall = npcDistance < 50f;
							bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

							if ((lineOfSight || closeThroughWall) && npcDistance < 750)
							{
								targetCenter = npc.Center + new Vector2(xpos, Main.rand.Next(-150, -40));
								targetNPC = npc.Center + new Vector2(0, 0);

								foundTarget = true;
							}
						}
					}
				}
			}


			Projectile.friendly = foundTarget;


			// Default movement parameters (here for attacking)
			float speed = 11f;
			float inertia = 65f;
			if (Projectile.ai[0] <= 6)
			{
				Projectile.ai[0] += 1; //Fix issue where minion would not moveif summoned near an enemy, bonus of making it attack enemies as soon as it's summoned
			}
			if (foundTarget)
			{
				if (Collision.CanHit(Projectile.Center, 0, 0, targetNPC, 0, 0))
				{
					Projectile.ai[1]++;
					
				}
				// Minion has a target: attack (here, fly towards the enemy)
				if (Vector2.Distance(Projectile.Center, targetCenter) > 50f || Projectile.ai[0] <= 5)
				{
					// The immediate range around the target (so it doesn't latch onto it when close)
					Vector2 direction = targetCenter - Projectile.Center;
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;				
				}

				if (Projectile.ai[1] > 40 && Vector2.Distance(Projectile.Center, targetCenter) < 550f)
				{
					if (!Main.dedServ)
					{
						if (Main.rand.Next(4) == 0)
						{
							var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
							dust.scale = 2;
							dust.noGravity = true;
						}

						if (Projectile.ai[1] > 60)
						{
                            float projspeed = 12;
                            Vector2 velocity = Vector2.Normalize(new Vector2(targetNPC.X, targetNPC.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                           
							float numberProjectiles = 3;
							float rotation = MathHelper.ToRadians(10);
							for (int j = 0; j < numberProjectiles; j++)
							{

								Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), 
									new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<FrozenSpiritMinionProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
							}
							Projectile.velocity.X = velocity.X * -0.18f;
							Projectile.velocity.Y = velocity.Y * -0.18f;

							SoundEngine.PlaySound(SoundID.Item30 with{Volume = 0.75f, Pitch = 0.5f}, Projectile.Center);

							for (int i = 0; i < 25; i++)
							{
								Vector2 perturbedSpeed = new Vector2(0, -2).RotatedByRandom(MathHelper.ToRadians(360));

								int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 111, perturbedSpeed.X, perturbedSpeed.Y, 0, default, 1f);
								Main.dust[dust2].noGravity = true;
							}
							Projectile.ai[1] = 0;

						}
					}
				}
			}
			else
			{
				// Minion doesn't have a target: return to player and idle
				if (distanceToIdlePosition > 300f)
				{
					// Speed up the minion if it's away from the player
					speed = 12f;
					inertia = 40f;
				}
				else
				{
					// Slow down the minion if closer to the player
					speed = 5f;
					inertia = 70f;
				}
				if (distanceToIdlePosition > 10f)
				{
					// The immediate range around the player (when it passively floats about)

					// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else if (Projectile.velocity == Vector2.Zero)
				{
					// If there is a case where it's not moving at all, give it a little "poke"
					Projectile.velocity.X = -0.15f;
					Projectile.velocity.Y = -0.05f;
				}
			}


			// So it will lean slightly towards the direction it's moving
			Projectile.rotation = Projectile.velocity.X * 0.06f;

			// This is a simple "loop through all frames from top to bottom" animation
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 8)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}

			// Some visuals here
			if (!Main.dedServ)
			{
				
					Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.78f);
				
				if (Main.rand.Next(8) == 0)
				{

					var dust2 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.Top.Y), Projectile.width, Projectile.height / 2, 135, 0, -2);
					dust2.scale = 1f;
					dust2.noGravity = true;

				}
				if (Main.rand.Next(6) == 0)
				{

					var dust3 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 8, Projectile.Bottom.Y - 10), 16, 6, 135, 0, 5);
					dust3.noGravity = true;

				}
			}
		}
		public override void Kill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				if (!Main.dedServ)
				{
					SoundEngine.PlaySound(SoundID.NPCDeath7 with { Volume = 0.75f}, Projectile.Center);

					for (int i = 0; i < 25; i++)
					{
						var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
						dust.scale = 1;
						dust.velocity *= 2;
					}
				}
			}
		}
	}
	//__________________________________________________________________________________________________________

	public class FrozenSpiritMinionProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Frozen Spirit Shard");
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;

			Projectile.friendly = true;
			Projectile.scale = 1f;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = 1;
			Projectile.aiStyle = 0;

			Projectile.timeLeft = 90;

			DrawOffsetX = -2;
			DrawOriginOffsetY = -2;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.78f;

			if (Main.rand.Next(5) == 0)
			{
				var dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 0, new Color(255, 255, 255), 0.75f)];

			}
			//dust.noGravity = true;


		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.Next(2) == 0)
			{
				target.AddBuff(ModContent.BuffType<Buffs.SuperFrostBurn>(), 150);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}
		public override void Kill(int timeLeft)
		{

			SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

			for (int i = 0; i < 5; i++)
			{

				var dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 0, new Color(255, 255, 255), 1f)];

				dust.velocity *= 2;
			}

		}
		public override Color? GetAlpha(Color lightColor)
		{

			Color color = Color.LightBlue;
			color.A = 150;
			return color;

		}
	}

}