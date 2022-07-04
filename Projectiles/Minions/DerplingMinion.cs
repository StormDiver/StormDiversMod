using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Items.Pets;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Projectiles.Minions
{
    public class DerplingMinionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Derpling Minion");
            Description.SetDefault("A buffed baby Derpling will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<DerplingMinionProj>()] > 0)
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
    

    public class DerplingMinionProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Derpling Minion");
            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[Projectile.type] = 8;
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            // These below are needed for a minion
            // Denotes that this Projectile is a pet or minion
            Main.projPet[Projectile.type] = true;
            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

        public sealed override void SetDefaults()
        {
            // Makes the minion go through tiles freely
            Projectile.tileCollide = false;
            // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.friendly = true;
            // Only determines the damage type
            Projectile.minion = true;
            // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.minionSlots = 1f;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.penetrate = -1;
            //Projectile.DamageType = DamageClass.Summon;
            Projectile.DamageType = DamageClass.Summon;
            //Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.width = 32;
            Projectile.height = 24;
            Projectile.Opacity = 1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = -4;
			//Projectile.aiStyle = 2;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
		bool grounded;
		bool jump = false;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{
				player.ClearBuff(BuffType<DerplingMinionBuff>());
			}
			if (player.HasBuff(BuffType<DerplingMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			Vector2 idlePosition = player.Center;

			// If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
			// The index is Projectile.minionPos
			float minionPositionOffsetX = (45 + Projectile.minionPos * 40) * -player.direction;
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
			// Set wheter they should fly or hop along the ground
			if (player.velocity.Y == 0 && !player.mount.Active && (Projectile.Center.Y < player.Bottom.Y) && vectorToIdlePosition.Y < 250 && (vectorToIdlePosition.X < 250 || vectorToIdlePosition.X > -250) && Collision.CanHitLine(Projectile.Center, 0, 0, player.position, 0, 0))
			{
				grounded = true;
				Projectile.tileCollide = true;
			}
			else
			{
				grounded = false;
				Projectile.tileCollide = false;

			}
			if (player.velocity.Y == 0 & !grounded && Projectile.Center.Y > player.Bottom.Y) //gravity
			{
				if (Projectile.velocity.Y > -5)
				{
					Projectile.velocity.Y -= 0.5f;
				}
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
					if (!grounded)
					{


						if (Projectile.position.Y < other.position.Y) Projectile.velocity.Y -= overlapVelocity;
						else Projectile.velocity.Y += overlapVelocity;

					}

				}
				if (grounded) //Stop moving left or right when in place
				{
					if (vectorToIdlePosition.X <= 10 && vectorToIdlePosition.X >= -10)
					{
						Projectile.velocity.X = 0f;
					}
				}

			}

			// Starting search distance
			Vector2 targetCenter = Projectile.position;
			bool foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				// Reasonable distance away so it doesn't target across multiple screens
				if (between < 2000f)
				{
					targetCenter = npc.Center + new Vector2(0, -0);

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
							bool closeThroughWall = npcDistance < 250f;
							bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

							if ((lineOfSight || closeThroughWall) && npcDistance < 1250)
							{
								targetCenter = npc.Center + new Vector2(0, 0);

								foundTarget = true;
							}
						}
					}
				}
			}


			Projectile.friendly = foundTarget;


			// Default movement parameters (here for attacking)
			float speed = 15f;
			float inertia = 20f;
			if (Projectile.ai[0] <= 6)
			{
				Projectile.ai[0] += 1; //Fix issue where minion would not moveif summoned near an enemy, bonus of making it attack enemies as soon as it's summoned
			}
			if (foundTarget)
			{
				if (grounded && Projectile.velocity.Y == 0)
				{
					Projectile.velocity.Y = -7.5f; //Jump when on the ground and attacking
				}
				grounded = false; //Attacking always puts them in flight mode because :pain:

				// Minion has a target: attack (here, fly towards the enemy)
				if (Vector2.Distance(Projectile.Center, targetCenter) > 30f || Projectile.ai[0] <= 5)
				{
					// The immediate range around the target (so it doesn't latch onto it when close)
					Vector2 direction = targetCenter - Projectile.Center;
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;

				}


			}
			else
			{
				// Minion doesn't have a target: return to player and idle
				if (distanceToIdlePosition > 300f)
				{
					// Speed up the minion if it's away from the player
					speed = 12f;
					inertia = 60f;
				}
				else
				{
					// Slow down the minion if closer to the player
					speed = 4f;
					if (!grounded)
					{
						inertia = 80f;
					}
					else
					{
						inertia = 30;
					}
				}
				if (distanceToIdlePosition > 30f)
				{
					// The immediate range around the player (when it passively floats about)

					// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					if (!grounded) //Movement when flying
					{
						Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
					}
					if (grounded) //Code for when hopping along the ground
					{
						Projectile.velocity.X = (Projectile.velocity.X * (inertia - 1) + vectorToIdlePosition.X) / inertia;


						if (Projectile.velocity.X != 0 && jump && Projectile.velocity.Y == 0) //Start jump
						{
							Projectile.velocity.Y = -6;
							jump = true;
						}
						if (jump) //if jump do countdown to stop jump
						{
							//Projectile.velocity.X *= 1.1f;
							Projectile.ai[1]++;
						}
						if (!jump) //When not jumping gravity
						{
							if (Projectile.velocity.Y <= 8)
							{
								Projectile.velocity.Y += 0.3f;
							}
						}
						if (Projectile.ai[1] > 20) //After 20 frames start falling
						{
							jump = false;

						}
						if (Projectile.velocity.Y == 0) //Reset cooldown
						{
							Projectile.ai[1] = 0;
							//Projectile.velocity.X *= 0;
						}


					}

				}

				else if (Projectile.velocity == Vector2.Zero)
				{
					// If there is a case where it's not moving at all, give it a little "poke"
					if (!grounded)
					{
						Projectile.velocity.X = -0.15f;
						Projectile.velocity.Y = -0.05f;
					}
				}
			}

			//Roatation and sprite direction
			if (!grounded)
			{
				Projectile.rotation = Projectile.velocity.X * 0.05f; //In the air tilt towards flying direction
			}
			else
			{
				Projectile.rotation = 0; //On ground always upright
			}
			if (Projectile.velocity != Vector2.Zero)
			{
				Projectile.spriteDirection = -Projectile.direction; // If not moving just face the same direction as the player
			}
			else
            {
				Projectile.spriteDirection = -player.direction; //If moving face the direction it's moving
            }

			Projectile.frameCounter++;
			if (!grounded)
			{
				if (Projectile.frameCounter >= 6) //Flying
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;
					
				}
				if (Projectile.frame <= 3 || Projectile.frame >= 8)
				{
					Projectile.frame = 4;
				}
			}
			else if (grounded) //On ground
            {
				if (Projectile.velocity.Y == 0)
				{
					if (Projectile.frameCounter >= 16)
					{
						Projectile.frameCounter = 0;
						Projectile.frame++;
						
					}
					if (Projectile.frame >= 2)
					{
						Projectile.frame = 0;
					}
				}
                else //Jumping
                {	
					if (Projectile.velocity.Y < 0)
                    {
						Projectile.frame = 2;
                    }
					else if (Projectile.velocity.Y > 0)
                    {
						Projectile.frame = 3;
                    }
				
				}
			}

	
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			jump = true;
			return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 2; i++)
                {


                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68);
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                if (!Main.dedServ)
                {
                    SoundEngine.PlaySound(SoundID.NPCHit22, Projectile.Center);
                    for (int i = 0; i < 15; i++)
                    {


                        var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68);
                    }
                }
            }
        }

    }
}