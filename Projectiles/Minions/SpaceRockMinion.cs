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
using Terraria.GameContent;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Projectiles.Minions
{
    public class SpaceRockMinionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Minion");
            //Description.SetDefault("A mini Asteroid will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<SpaceRockMinionProj>()] > 0)
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
    

    public class SpaceRockMinionProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Minion");
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.friendly = true;
            // Only determines the damage type
            Projectile.minion = true;
            // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.minionSlots = 1f;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        int dustspeed;

        public override bool? CanCutTiles()
        {
            return false;
        }

     
        public override void AI()
        {

            Player player = Main.player[Projectile.owner];
            // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<SpaceRockMinionBuff>());
            }
            if (player.HasBuff(BuffType<SpaceRockMinionBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            Vector2 idlePosition = player.Center;
            idlePosition.Y -= 0f; // On player

            // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
            // The index is Projectile.minionPos
            float minionPositionOffsetX = (25 + Projectile.minionPos * 20) * -player.direction;
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
            float overlapVelocity = 0.01f;
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
                            bool closeThroughWall = npcDistance < 500f;
                            bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

                            if ((lineOfSight || closeThroughWall) && npcDistance < 1000)
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
            float speed = 30f;
            float inertia = 5f;
            if (Projectile.ai[0] <= 6)
            {
                Projectile.ai[0] += 1; //Fix issue where minion would not move if summoned near an enemy, bonus of making it attack enemies as soon as it's summoned
            }
            if (foundTarget)
            {
                
                // Minion has a target: attack (here, fly towards the enemy)
                if (Vector2.Distance(Projectile.Center, targetCenter) > 200f || Projectile.ai[0] <= 5)
                {
                    // The immediate range around the target (so it doesn't latch onto it when close)
                    Vector2 direction = targetCenter - Projectile.Center;
                    direction.Normalize();
                    direction *= speed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                }
                else
                {                 
                        Projectile.velocity *= 1.03f; //Small little boost so that it doesn't slow down                  
                }
            }
            else
            {

                // Minion doesn't have a target: return to player and idle
                if (distanceToIdlePosition > 300f)
                {
                    // Speed up the minion if it's away from the player
                    speed = 20f;
                    inertia = 20f;
                }
                else
                {
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



            Projectile.ai[1]++;
            Projectile.localAI[0]++;

            if (Projectile.velocity.X > 7 || Projectile.velocity.X < -7 || Projectile.velocity.Y > 7 || Projectile.velocity.Y < -7) //When moving fast, ie. charging, face direction moving and have tail
            {
                dustspeed = 1;

                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

                Projectile.frameCounter++;
               
                if (Projectile.frameCounter >= 8)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                    
                }
                if (Projectile.frame <= 3 || Projectile.frame >= 8)
                {
                    Projectile.frame = 4;
                }
            }
            else //when moving slow, spin and have no tail
            {
                dustspeed = 10;

                Projectile.rotation = Projectile.localAI[0] * 0.5f;
               
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 8)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                }
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            if (!Main.dedServ)
            {

                if (Main.rand.Next(dustspeed) == 0)     //this defines how many dust to spawn
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                        dust2.noGravity = true;
                        dust2.scale = 1.5f;
                        dust2.velocity *= 1;
                    }
                }
            }           
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            if (!Main.dedServ)
            {
                for (int i = 0; i < 20; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 130, default, 1.5f);
                    dust.noGravity = true;
                }
                
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 0, 0, 0, 130, default, 1f);
                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 130, default, 1f);
                }
                SoundEngine.PlaySound(SoundID.Item62 with{Volume = 0.5f, Pitch = 0.2f}, Projectile.Center);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
       
       
    }
    
}