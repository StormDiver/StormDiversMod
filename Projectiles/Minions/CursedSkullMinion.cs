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
	public class CursedSkullMinionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
            //DisplayName.SetDefault("Magical Feather Minion");
            //Description.SetDefault("A Magical Feather minion will fight for you");
            Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<CursedSkullMinionProj>()] > 0)
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
	public class CursedSkullMinionProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Cursed Skull Minion");
			Main.projFrames[Projectile.type] = 6;
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
			DrawOriginOffsetY = -4;

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
		bool shooting;
		int xpos = 0;

        float speed = 8f;
        float inertia = 50f;
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];


			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<CursedSkullMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<CursedSkullMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 30f; // above the center of the player)

			// If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
			// The index is Projectile.minionPos
			float minionPositionOffsetX = (25 + Projectile.minionPos * 40) * -player.direction;
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

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				// Reasonable distance away so it doesn't target across multiple screens
				if (between < 2000f)
				{
					targetCenter = npc.Center + new Vector2(xpos, Main.rand.Next(-75, -25));
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
							bool closeThroughWall = npcDistance < 20f;
							bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

							if ((lineOfSight || closeThroughWall) && npcDistance < 750)
							{
								targetCenter = npc.Center + new Vector2(xpos, Main.rand.Next(-50, 0));
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
				Projectile.ai[0] += 1; //Fix issue where minion would not moveif summoned near an enemy, bonus of making it attack enemies as soon as it's summoned
			}
			if (foundTarget)
			{
                speed = 12f;
                inertia = 15f;

                if (Projectile.position.X < targetNPC.X)
                    xpos = -100;
                else
                    xpos = 100;

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

                if (Projectile.ai[1] > 40 && Vector2.Distance(Projectile.Center, targetCenter) < 250f)
				{
					if (!Main.dedServ)
					{
                        shooting = true;

						for (int i = 0; i < 3; i++)
						{
							var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113);
							dust.scale = 0.75f;
							dust.noGravity = true;
						}
						if (Projectile.ai[1] > 60)
						{
                            Vector2 velocity = Vector2.Normalize(new Vector2(targetNPC.X, targetNPC.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * 12;
                           
							float rotation = MathHelper.ToRadians(8);
							
								Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(0);
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), 
									new Vector2(perturbedSpeed.X + Projectile.velocity.X, perturbedSpeed.Y - 4), ModContent.ProjectileType<CursedSkullMinionProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

							SoundEngine.PlaySound(SoundID.Item8 with{Volume = 0.75f, Pitch = 0f}, Projectile.Center);

							for (int i = 0; i < 25; i++)
							{
								Vector2 perturbedSpeed2 = new Vector2(0, -2).RotatedByRandom(MathHelper.ToRadians(360));

								int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 113, perturbedSpeed2.X, perturbedSpeed2.Y, 0, default, 1f);
								Main.dust[dust2].noGravity = true;
							}
							Projectile.ai[1] = 0;

						}
					}
				}
                if (shooting)
                    Projectile.rotation = Projectile.velocity.X * 0.06f;
                else
                    Projectile.rotation += 0.4f;
            }
			else
			{
				shooting = false;

                // Minion doesn't have a target: return to player and idle
                if (distanceToIdlePosition > 200f)
				{
                    Projectile.rotation += 0.4f;

                    // Speed up the minion if it's away from the player
                    speed = 12f;
					inertia = 20f;
				}
				else
				{
                    Projectile.rotation = Projectile.velocity.X * 0.06f;
                    // Slow down the minion if closer to the player
                    speed = 5f;
					inertia = 40f;
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

			Projectile.frameCounter++;

            if (shooting) //frames 2-5 when firing
            {
                if (Projectile.frameCounter >= 8)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame <= 1)
                {
                    Projectile.frame = 2;
                }
                if (Projectile.frame >= 6)
                {
                    shooting = false;
                }
            }
            if (!shooting)//frames 0-1 when idle
            {
                
                if (Projectile.frameCounter >= 8)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 1 || Projectile.frame < 0)
                {
                    Projectile.frame = 0;
                }
            }

            // Some visuals here
            if (!Main.dedServ)
			{
				
				Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.33f);
				
				if (Main.rand.Next(6) == 0)
				{
					var dust2 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.Center.Y), Projectile.width, Projectile.height / 2, 113, 0, 3);
					dust2.scale = 1f;
					dust2.noGravity = true;

				}
			}
		}
		public override void OnKill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				if (!Main.dedServ)
				{
					SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1.5f}, Projectile.Center);

					for (int i = 0; i < 25; i++)
					{
						var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113);
						dust.scale = 1.5f;
						dust.velocity *= 2;
                        dust.noGravity = true;

                    }
                }
			}
		}
	}
    //___________________________________________________________________________________________
    public class CursedSkullMinionProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cursed Spinning Bones");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.light = 0f;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 180;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 0.75f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }
        public override void AI()
        {
            Projectile.rotation += 0.25f * Projectile.direction;
            var player = Main.player[Projectile.owner];
			Projectile.velocity.Y += 0.5f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9 / 10);

            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113);
                dust.noGravity = true;

            }
            SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.Center);
        }
		int bouncetime = 3;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.66f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.66f;
            }
			bouncetime--;

			if (bouncetime <= 0)
				Projectile.Kill();
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            //Main.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113);
				dust.noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
    }

}