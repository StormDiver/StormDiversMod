using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;


namespace StormDiversMod.Projectiles.Minions
{


	public class SantankMinionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mini Santank Minion Buff");
			//Description.SetDefault("A mini santank will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SantankMinionProj>()] > 0)
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
	public class SantankMinionProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mini Santank Minion");
			Main.projFrames[Projectile.type] = 8;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

		public sealed override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 40;
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

            Projectile.light = 0.5f;

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
		float speed = 25f;
		float inertia = 25f;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];


			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<SantankMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<SantankMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 48f;

			// If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
			// The index is Projectile.minionPos
			float minionPositionOffsetX = (25 + Projectile.minionPos * 25) * -player.direction;
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

							if ((lineOfSight || closeThroughWall) && npcDistance < 1500)
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
				if (Projectile.position.X < targetNPC.X)
					Projectile.spriteDirection = 1;
				else
					Projectile.spriteDirection = -1;

				if (Collision.CanHit(Projectile.Center, 0, 0, targetNPC, 0, 0))
				{
					Projectile.ai[1]++; //Shoottime
					Projectile.ai[2]++; //Missile time
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

				if (Vector2.Distance(Projectile.Center, targetNPC) < 300f && Collision.CanHit(Projectile.Center, 0, 0, targetNPC, 0, 0)) //Slow down if close
				{
					shooting = true;
					speed *= 0.5f;
					inertia = 8f;
				}
				else
				{
					shooting = false;
                    speed = 22f;
                    inertia = 25f;
                }
				if (Vector2.Distance(Projectile.Center, targetCenter) < 650f)
				{
					if (Projectile.ai[1] >= 15)
					{
                        SoundEngine.PlaySound(SoundID.Item11 with { Volume = 0.5f, Pitch = 0f }, Projectile.Center);

                        if (!Main.dedServ)
						{
							Vector2 velocity = Vector2.Normalize(new Vector2(targetNPC.X, targetNPC.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)).RotatedByRandom(MathHelper.ToRadians(1)) * 15;

							int ProjID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + 10 * Projectile.spriteDirection, Projectile.Center.Y ), new Vector2(velocity.X, velocity.Y), 
								ModContent.ProjectileType<SantankMinionProj3>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
							Main.projectile[ProjID].DamageType = DamageClass.Summon;

							/*for (int i = 0; i < 10; i++)
							{
								var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
								dust.scale = 0.75f;
								dust.noGravity = true;

							}*/
						}
						Projectile.ai[1] = 0;
					}

					if (Projectile.ai[2] >= 90)
					{
						if (!Main.dedServ)
						{
							Vector2 perturbedSpeed = new Vector2(0, -5).RotatedByRandom(MathHelper.ToRadians(15));
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X - 10 * Projectile.spriteDirection, Projectile.Center.Y - 5), 
								new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<SantankMinionProj2>(), (int)(Projectile.damage * 1.5f), Projectile.knockBack * 2, Projectile.owner);
							SoundEngine.PlaySound(SoundID.Item92 with { Volume = 0.5f, Pitch = 0f }, Projectile.Center);

							for (int i = 0; i < 25; i++)
							{
								int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X - 10 * Projectile.direction, Projectile.Center.Y - 5), 0, 0, 31, 0, -3, 100, default, 0.5f);
								Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
								Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
								Main.dust[dustIndex].noGravity = true;
							}
						}
						Projectile.ai[2] = 0;

					}
				}
			}
			else
			{
				if (Projectile.position.X < player.position.X)
                    Projectile.spriteDirection = 1;
				else
                    Projectile.spriteDirection = -1;


                shooting = false;

				// Minion doesn't have a target: return to player and idle
				if (distanceToIdlePosition > 200)
				{
					// Speed up the minion if it's away from the player
					speed = 20f;
					inertia = 25f;

                }
                else
				{
					// Slow down the minion if closer to the player
					speed = 10f;
					inertia = 60f;

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

			Projectile.frameCounter++;

			if (shooting)
			{
				if (Projectile.frameCounter >= 5)
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;

				}
				if (Projectile.frame <= 3 || Projectile.frame >= 8) // frame 4-7 when shooting
				{
					Projectile.frame = 4;
				}
			}
			else
			{
				if (Projectile.frameCounter >= 6)
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;

				}
				if (Projectile.frame >= 4) // frame 0-3 when not shooting
				{
					Projectile.frame = 0;
				}
			}
            // Some visuals here
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.2f);

                if (Main.rand.Next(4) == 0)
                {
                    var dust3 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 3, Projectile.Bottom.Y - 2), 6, 10, 6, 0, 10);
                    dust3.noGravity = true;
                }
            }
        }
		public override void OnKill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				if (!Main.dedServ)
				{
					SoundEngine.PlaySound(SoundID.NPCDeath14 with { Volume = 0.75f }, Projectile.Center);

					for (int i = 0; i < 25; i++)
					{
						var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
						dust.scale = 1;
						dust.velocity *= 2;
					}
					for (int i = 0; i < 30; i++)
					{

						int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 31, 0f, 0f, 0, default, 1f);
						Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
						Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
						Main.dust[dustIndex].noGravity = true;
					}
				}
			}
		}
        public override void PostDraw(Color lightColor) //glowmask for animated
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/Minions/SantankMinionProj_Glow");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f - 2, Projectile.height * 0.5f);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
    }
	//__________________________________________________________________________________________________________
	public class SantankMinionProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mini Homing missile");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;

			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;

			Projectile.friendly = true;

			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.extraUpdates = 0;
			Projectile.timeLeft = 180;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.extraUpdates = 1;
			//Projectile.ArmorPenetration = 24;
		}
		int timer;
        Vector2 newMove;

        public override void AI()
		{
			var player = Main.player[Projectile.owner];

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

			timer++;
			if (timer > 5)
			{
				for (int i = 0; i < 5; i++)
				{
					float X = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
					float Y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;


					int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 6, 0, 0, 100, default, 1f);
					Main.dust[dust].position.X = X;
					Main.dust[dust].position.Y = Y;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0f;
				}
			}
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				Projectile.tileCollide = false;
				// Set to transparent. This projectile technically lives as  transparent for about 3 frames
				Projectile.alpha = 255;
				// change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
				Projectile.position = Projectile.Center;

				Projectile.width = 100;
				Projectile.height = 100;
				Projectile.Center = Projectile.position;

				Projectile.knockBack = 1.5f;
				Projectile.velocity.X = 0;
				Projectile.velocity.Y = 0;

			}
			if (timer > 25)
			{
				if (Projectile.localAI[0] == 0f)
				{
					AdjustMagnitude(ref Projectile.velocity);
					Projectile.localAI[0] = 1f;
				}
				Vector2 move = Vector2.Zero;
				float distance = 750;
				bool target = false;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
					{
						if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
						{
                            if (player.HasMinionAttackTargetNPC)
                            {
                                newMove = Main.npc[player.MinionAttackTargetNPC].Center - Projectile.Center;
                            }
                            else
                            {
                                newMove = Main.npc[k].Center - Projectile.Center;
                            }
                            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
							if (distanceTo < distance)
							{
								move = newMove;
								distance = distanceTo;
								target = true;
							}
						}
					}
				}
				if (target)
				{
					AdjustMagnitude(ref move);
					Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
					AdjustMagnitude(ref Projectile.velocity);
				}
			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
			{
				Projectile.frame++;
				Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
				Projectile.frameCounter = 0;
			}
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			if (timer > 25)
			{
				float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
				if (magnitude > 15f)
				{
					vector *= 15f / magnitude;
				}
			}
		}
		public override bool? CanDamage()
		{
			if (timer < 30)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (timer < 45)
			{
				Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X * 0.8f;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
				}
			}
			if (Projectile.timeLeft > 3 && timer >= 45)
			{
				Projectile.timeLeft = 3;
			}
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{

			if (Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3;
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.5f, Pitch = 0f }, Projectile.Center);

			int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionGenericProj>(), 0, 0, Projectile.owner);
			Main.projectile[proj].scale = 0.9f;

			for (int i = 0; i < 15; i++) //Orange particles
			{
				Vector2 perturbedSpeed = new Vector2(0, -4.5f).RotatedByRandom(MathHelper.ToRadians(360));

				var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
				dust.noGravity = true;
				dust.scale = 2f;

			}
			for (int i = 0; i < 25; i++) //Grey dust circle
			{
				Vector2 perturbedSpeed = new Vector2(0, -4f).RotatedByRandom(MathHelper.ToRadians(360));
				var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

				//dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
				dust.noGravity = true;
				dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
				dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
			}
		}
	}
	//________________________________________________
	public class SantankMinionProj3 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            //DisplayName.SetDefault("Mini Santank Bullet");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;

			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }

        public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			//AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 2;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 300;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
            Projectile.ArmorPenetration = 12;
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++) //Orange particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -1f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 0.75f;
            }
        }

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
    }
}