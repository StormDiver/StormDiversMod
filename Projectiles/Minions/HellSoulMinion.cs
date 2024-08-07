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


	public class HellSoulMinionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
            //DisplayName.SetDefault("Soul Flame Minion");
            //Description.SetDefault("A Soul Flame minion will fight for you");
            Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<HellSoulMinionProj>()] > 0)
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
	public class HellSoulMinionProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Soul Flame Minion");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

		public sealed override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
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
        float speed = 18f;
        float inertia = 20f;
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];


			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<HellSoulMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<HellSoulMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

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
					targetCenter = npc.Center + new Vector2(0, -120);
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
								targetCenter = npc.Center + new Vector2(0, -120);
								targetNPC = npc.Center + new Vector2(0, 0);

								foundTarget = true;
							}
						}
					}
				}
			}


			Projectile.friendly = foundTarget;


			// Default movement parameters (here for attacking)
			//float speed = 18f;
			//float inertia = 20f;
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
				if (Vector2.Distance(Projectile.Center, targetCenter) > 30 || Projectile.ai[0] <= 5)
				{
					// The immediate range around the target (so it doesn't latch onto it when close)
					Vector2 direction = targetCenter - Projectile.Center;
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;

				}
				
				if (Projectile.ai[1] > 45 && Vector2.Distance(Projectile.Center, targetCenter) < 450f)
				{
					if (!Main.dedServ)
					{					
                        //float projspeed = 0.1f;
                        //Vector2 velocity = Vector2.Normalize(new Vector2(targetNPC.X, targetNPC.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;

                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Main.rand.NextFloat(-3, 3), -3), ModContent.ProjectileType<HellSoulMinionProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);

                        for (int i = 0; i < 10; i++)
						{
							var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173);
							dust.scale = 1.5f;
							dust.velocity *= 2;
						}
					}
					Projectile.ai[1] = 0;
				}
                if (Vector2.Distance(Projectile.Center, targetNPC) < 150f && Collision.CanHit(Projectile.Center, 0, 0, targetNPC, 0, 0)) //Slow down if close
                {
                    speed *= 0.5f;
                    inertia = 10f;
                }
                else
                {
                    speed = 18f;
                    inertia = 18f;
                }
            }
			else
			{
				// Minion doesn't have a target: return to player and idle
				if (distanceToIdlePosition > 300f)
				{
					// Speed up the minion if it's away from the player
					speed = 18;
					inertia = 18;
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


			// So it will lean slightly towards the direction it's moving
			Projectile.rotation = Projectile.velocity.X * 0.05f;

			// This is a simple "loop through all frames from top to bottom" animation
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				
			}
			if (Projectile.frame >= Main.projFrames[Projectile.type])
			{
				Projectile.frame = 0;
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

					var dust3 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 8, Projectile.Bottom.Y - 10), 16, 6, 173, 0, 5);
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
					SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);

					for (int i = 0; i < 25; i++)
					{
						var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173);
						dust.scale = 1;
						dust.velocity *= 2;
					}
				}
			}
		}
        public override void PostDraw(Color lightColor) //glowmask for animated
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/Minions/HellSoulMinionProj_Glow");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
        public override bool PreDraw(ref Color lightColor) //trail
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                    color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
    }
	//__________________________________________________________________________________________________________

	public class HellSoulMinionProj2 : ModProjectile
	{

		public override void SetStaticDefaults()
		{
            //DisplayName.SetDefault("Soul Flame Minion Projectile");
            Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 240;
			Projectile.light = 0.4f;
			Projectile.scale = 1f;
			Projectile.aiStyle = 0;
			//Projectile.extraUpdates = 1;
			Projectile.DamageType = DamageClass.Summon;
		}
        Vector2 newMove;

		public override void AI()
		{
			Projectile.ai[2]++;
			//Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Projectile.rotation = Projectile.velocity.X * 0.02f;

			AnimateProjectile();
			Dust dust;
			// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
			Vector2 position = Projectile.position;
			dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f, 0, new Color(255, 255, 255), 1f)];
			dust.noGravity = true;
			dust.scale = 1f;

			Player player = Main.player[Projectile.owner];
			Vector2 move = Vector2.Zero;
			float distance = 750;
			bool target = false;
			if (Projectile.ai[2] > 20)
			{
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
				Projectile.velocity = (10 * Projectile.velocity + move) / 10f;
				AdjustMagnitude(ref Projectile.velocity);
			}
		}
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {

            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 14f)
            {
                vector *= 15f / magnitude;
            }

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{

			target.AddBuff(ModContent.BuffType<Buffs.HellSoulFireDebuff>(), 120);

		}
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<Buffs.HellSoulFireDebuff>(), 120);
		}
		public override void OnKill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{

				SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);

				for (int i = 0; i < 10; i++)
				{
					var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173);
					dust.scale = 1;
					dust.velocity *= 2;
				}

			}
		}

		public void AnimateProjectile() // Call this every frame, for example in the AI method.
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
			{
				Projectile.frame++;
				Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
				Projectile.frameCounter = 0;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			Color color = Color.White;
			color.A = 150;
			return color;

		}
        public override bool PreDraw(ref Color lightColor) //trail
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                    color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
    }
}