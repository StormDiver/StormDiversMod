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


	public class AncientMinionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arid Spirit Minion");
			Description.SetDefault("An Arid Spirit minion will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<AncientMinionProj>()] > 0)
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
	public class AncientMinionProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arid Spirit Minion");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
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

			Projectile.alpha = 75;
			
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
		bool shooting = false;
		float speed = 10f;
		float inertia = 75f;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];


			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<AncientMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<AncientMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 48f;

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
			Vector2 targetCenter = Projectile.position;
			bool foundTarget = false;
			Vector2 targetNPC = Projectile.position; //Shooting at enemy

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				// Reasonable distance away so it doesn't target across multiple screens
				if (between < 2000f)
				{
					
					targetCenter = npc.Center + new Vector2(0, 0);
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
								targetCenter = npc.Center + new Vector2(0, 0);
								targetNPC = npc.Center + new Vector2(0, 0);

								foundTarget = true;
							}
						}
					}
				}
			}


			Projectile.friendly = foundTarget;


			// Default movement parameters (here for attacking)
			
			if (Projectile.ai[0] <= 6)
			{
				Projectile.ai[0] += 1; //Fix issue where minion would not move if summoned near an enemy, bonus of making it attack enemies as soon as it's summoned
			}
			if (foundTarget)
			{
				if (Collision.CanHit(Projectile.Center, 0, 0, targetNPC, 0, 0))
				{
					Projectile.ai[1]++; //Shoottime
				}
				// Minion has a target: attack (here, fly towards the enemy)
				if (Vector2.Distance(Projectile.Center, targetCenter) > 30f || Projectile.ai[0] <= 5)
				{
					// The immediate range around the target (so it doesn't latch onto it when close)
					Vector2 direction = targetCenter - Projectile.Center;
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;


				}
				//Getting the shooting trajectory
				float shootToX = targetNPC.X - Projectile.Center.X;
				float shootToY = targetNPC.Y - Projectile.Center.Y;
				float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				if (Vector2.Distance(Projectile.Center, targetNPC) < 200f && Collision.CanHit(Projectile.Center, 0, 0, targetNPC, 0, 0)) //Slow down if close
				{
					shooting = true;
					speed *= 0.5f;
					inertia = 8f;
				}
				else
                {
					shooting = false;
					speed = 13f;
					inertia = 50f;
				}

				if (Projectile.ai[1] >= 20 && Vector2.Distance(Projectile.Center, targetCenter) < 250f)
				{
					SoundEngine.PlaySound(SoundID.Item34 with { Volume = 1f, Pitch = 0.5f }, Projectile.Center);

					if (!Main.dedServ)
					{
						distance = 1.6f / distance;

						//Multiplying the shoot trajectory with distance times a multiplier if you so choose to
						shootToX *= distance * 4f;
						shootToY *= distance * 4f;

						Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(shootToX, shootToY), ModContent.ProjectileType<AncientMinionProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

						for (int i = 0; i < 10; i++)
						{
							var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
							dust.scale = 0.75f;
							dust.noGravity = true;
						
						}
					}
				
					Projectile.ai[1] = 0;

				}
			}
			else
			{
				shooting = false;

				// Minion doesn't have a target: return to player and idle
				if (distanceToIdlePosition > 300f)
				{
					// Speed up the minion if it's away from the player
					speed = 12f;
					inertia = 50f;
				}
				else
				{
					// Slow down the minion if closer to the player
					speed = 4f;
					inertia = 80f;
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
			Projectile.rotation = Projectile.velocity.X * 0.05f;

			
			if (shooting)
			{
				Projectile.frameCounter++;
				if (Projectile.frameCounter >= 8)
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;

				}
				if (Projectile.frame <= 0 || Projectile.frame >= 4) // frae 1-3 when shooting
				{
					Projectile.frame = 1;
				}
			}
			else
			{

				Projectile.frame = 0; //no animation when not

			}
			// Some visuals here
			if (!Main.dedServ)
			{
				Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.78f);
				
				if (Main.rand.Next(6) == 0)
				{

					var dust3 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 8, Projectile.Bottom.Y - 10), 16, 5, 138, 0, 4);
					dust3.noGravity = true;
					dust3.scale = 1;

				}
				if (Main.rand.Next(8) == 0)
				{
					var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
					dust.scale = 0.75f;
					dust.noGravity = true;

				}
			}
		}
		public override void Kill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				if (!Main.dedServ)
				{
					SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);

					for (int i = 0; i < 25; i++)
					{
						var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
						dust.scale = 0.75f;
						dust.noGravity = true;
						
					}
				}
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{

			Color color = Color.Orange;
			color.A = 100;
			return color;
		}
	}
	//__________________________________________________________________________________________________________

	public class AncientMinionProj2 : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Spirit Minion Projectile");
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 60;
			Projectile.light = 0.4f;
			Projectile.scale = 1f;
			Projectile.aiStyle = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.extraUpdates = 1;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors


			if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 138, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);
				Main.dust[dust].noGravity = true; //this make so the dust has no gravity
				Main.dust[dust].velocity *= 0.5f;
				int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
			}


			return;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.damage = (Projectile.damage * 9) / 10;


			target.AddBuff(BuffID.OnFire, 180);


		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{

		}
		public override void OnHitPvp(Player target, int damage, bool crit)

		{
			target.AddBuff(BuffID.OnFire, 180);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}
	}
}