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
	public class HarpyMinionBuff : ModBuff
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
			if (player.ownedProjectileCounts[ModContent.ProjectileType<HarpyMinionProj>()] > 0)
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
	public class HarpyMinionProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Frozen Spirit Minion");
			Main.projFrames[Projectile.type] = 8;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 46;
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
		bool shooting;

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];


			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<HarpyMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<HarpyMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 80f; // above the center of the player)

			// If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
			// The index is Projectile.minionPos
			float minionPositionOffsetX = (15 + Projectile.minionPos * 40) * -player.direction;
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

			if (xpos > 150)
			{
				moveright = false;
			}
			if (xpos < -150)
			{
				moveright = true;
			}

			if (moveright)
			{
				xpos += 1;
			}
			else
			{
				xpos -= 1;
			}
			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				// Reasonable distance away so it doesn't target across multiple screens
				if (between < 2000f)
				{
					targetCenter = npc.Center + new Vector2(xpos, Main.rand.Next(-200, -100));
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

							if ((lineOfSight || closeThroughWall) && npcDistance < 700)
							{
								targetCenter = npc.Center + new Vector2(xpos, Main.rand.Next(-200, -100));
								targetNPC = npc.Center + new Vector2(0, 0);
								foundTarget = true;
							}
						}
					}
				}
			}

			Projectile.friendly = foundTarget;

			// Default movement parameters (here for attacking)
			float speed = 8f;
			float inertia = 25f;
			if (Projectile.ai[0] <= 6)
			{
				Projectile.ai[0] += 1; //Fix issue where minion would not moveif summoned near an enemy, bonus of making it attack enemies as soon as it's summoned
			}
			if (foundTarget)
			{
                Projectile.rotation = ((targetNPC - Projectile.Center) / 360).ToRotation() + 1.5708f;//Look at the enemy

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

				if (Projectile.ai[1] > 100 && Vector2.Distance(Projectile.Center, targetCenter) < 350f)
				{
					if (!Main.dedServ)
					{
                        for (int i = 0; i < 2; i++)
                        {
                            var dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 10, Projectile.Center.Y - 10), 20, 20, 202);
							dust.scale = 1;
							dust.noGravity = true;
						}
						Projectile.velocity *= 0;
						if (Projectile.ai[1] > 120)
						{
							shooting = true;
                            float projspeed = 12;
                            Vector2 velocity = Vector2.Normalize(new Vector2(targetNPC.X, targetNPC.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                           
							float rotation = MathHelper.ToRadians(8);
							
								Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(0);
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), 
									new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<HarpyMinionProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
							
							Projectile.velocity.X = velocity.X * -0.18f;
							Projectile.velocity.Y = velocity.Y * -0.18f;

							SoundEngine.PlaySound(SoundID.Item8 with{Volume = 0.75f, Pitch = 0f}, Projectile.Center);

							for (int i = 0; i < 25; i++)
							{
								Vector2 perturbedSpeed2 = new Vector2(0, -2).RotatedByRandom(MathHelper.ToRadians(360));

								int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 202, perturbedSpeed2.X, perturbedSpeed2.Y, 0, default, 1f);
								Main.dust[dust2].noGravity = true;
							}
							Projectile.ai[1] = 0;

						}
					}
				}
			}
			else
			{
				shooting = false;
                // So it will lean slightly towards the direction it's moving
                Projectile.rotation = Projectile.velocity.X * 0.06f;

                // Minion doesn't have a target: return to player and idle
                if (distanceToIdlePosition > 300f)
				{
					// Speed up the minion if it's away from the player
					speed = 8f;
					inertia = 25f;
				}
				else
				{
					// Slow down the minion if closer to the player
					speed = 5f;
					inertia = 50f;
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

            if (shooting) //frames 4-7 when firing
            {
                if (Projectile.frameCounter >= 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame <= 3)
                {
                    Projectile.frame = 4;
                }
                if (Projectile.frame >= 8)
                {
                    shooting = false;
                }
            }
            if (!shooting)//frames 0-3 when idle
            {
                
                if (Projectile.frameCounter >= 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 3 || Projectile.frame < 0)
                {
                    Projectile.frame = 0;
                }
            }

            // Some visuals here
            if (!Main.dedServ)
			{				
				if (Main.rand.Next(8) == 0)
				{

					var dust2 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.Top.Y), Projectile.width, Projectile.height / 2, 202, 0, -2);
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
					SoundEngine.PlaySound(SoundID.NPCHit11 with { Volume = 1.5f}, Projectile.Center);

					for (int i = 0; i < 25; i++)
					{
						var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 202);
						dust.scale = 1.5f;
						dust.velocity *= 2;
                        dust.noGravity = true;

                    }
                }
			}
		}
	}
    //___________________________________________________________________________________________
    public class HarpyMinionProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Big Harpy Feather");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.light = 0f;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 300;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            DrawOffsetX = 0;
            DrawOriginOffsetY = -5;
        }
        Vector2 newMove;

        public override void AI()
        {
            Projectile.rotation += 0.5f;
            var player = Main.player[Projectile.owner];

            if (Main.rand.Next(2) == 0) // the chance
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 202, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;
            }

            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 650;
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
                Projectile.velocity = (9f * Projectile.velocity + move) / 9f;
                AdjustMagnitude(ref Projectile.velocity);
            }
        }

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 7f)
			{
				vector *= 7f / magnitude;
			}
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9 / 10);

            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 202);
            }
            SoundEngine.PlaySound(SoundID.NPCHit11, Projectile.Center);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.8f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            //Main.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 202);
            }
            SoundEngine.PlaySound(SoundID.NPCHit11, Projectile.Center);
        }

    }
}