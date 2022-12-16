using StormDiversMod.Basefiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using System;
using StormDiversMod.Items.Pets;
using Microsoft.Xna.Framework.Graphics;


namespace StormDiversMod.Projectiles.Petprojs
{

   
    public class GoldDerpieBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Golden Derpie");
            Description.SetDefault("So shiny and bouncy!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<EquipmentEffects>().goldDerpie = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<GoldDerpie>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {

                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), new Vector2(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2)), new Vector2(0, 0), ProjectileType<GoldDerpie>(), 0, 0, player.whoAmI);

            }
        }
    }
    public class GoldDerpie : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Golden Derpie");
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.EyeSpring);
            //AIType = ProjectileID.EyeSpring;
            Projectile.aiStyle = -1;
            Projectile.width = 32;
            Projectile.height = 24;
            DrawOffsetX = -0;
            DrawOriginOffsetY = -4;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            //player.eyeSpring = false; 
            return true;
        }
        bool grounded;
        bool jump = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            EquipmentEffects modPlayer = player.GetModPlayer<EquipmentEffects>();
            if (player.dead)
            {
                modPlayer.goldDerpie = false;
            }
            if (modPlayer.goldDerpie)
            {
                Projectile.timeLeft = 2;
            }


            Vector2 idlePosition = player.Center;

            idlePosition.X += 50 * player.direction; // Go behind the player

            // Teleport to player if distance is too big
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1000f)
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
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (grounded) //Stop moving left or right when in place
                {
                    if (vectorToIdlePosition.X <= 10 && vectorToIdlePosition.X >= -10)
                    {
                        Projectile.velocity.X = 0f;
                    }
                }
            }
            float speed;
            float inertia;

            if (distanceToIdlePosition > 200f)
            {
                // Speed up the pet if it's away from the player

                speed = 20f;
                inertia = 60f;
            }
            else
            {
                // Slow down the pet if closer to the player
                if (!grounded)
                {
                    speed = 5f;
                    inertia = 80f;
                }
                else
                {
                    speed = 4f;
                    inertia = 1; //had to set this low
                }
            }

            if (distanceToIdlePosition > 10f)
            {

                // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
                vectorToIdlePosition.Normalize();
                vectorToIdlePosition *= speed;
                if (!grounded) //Movement when flying
                {
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
                if (grounded) //Code for when hopping along the ground
                {
                    //Main.NewText("AGGRHHHHHH!!!!!::::::" + vectorToIdlePosition.X, 204, 101, 22);

                    
                     Projectile.velocity.X = (Projectile.velocity.X * (inertia - 1) + vectorToIdlePosition.X) / inertia;
                    

                    if (Projectile.velocity.X != 0 && !jump && Projectile.velocity.Y == 0) //Start jump
                    {
                        Projectile.velocity.Y = -7.5f;
                        jump = true;
                    }
                    if (jump) //if jump do countdown to stop jump
                    {
                        Projectile.ai[1]++;
                    }
                    if (!jump) //When not jumping gravity
                    {
                        if (Projectile.velocity.Y <= 8)
                        {
                            Projectile.velocity.Y += 0.3f;
                        }
                    }
                    if (Projectile.ai[1] > 5) //After 5 frames start falling
                    {
                        jump = false;

                    }
                    if (Projectile.velocity.Y == 0) //Reset cooldown
                    {
                        Projectile.ai[1] = 0;
                        Projectile.velocity.X *= 0;

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
                if (Projectile.frameCounter >= 2) //Flying
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;

                }
                if (Projectile.frame <= 3 || Projectile.frame >= 8) //4-7
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
                    if (Projectile.frame >= 2) //0-1
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
     

            if (!Main.dedServ)
            {
                if (Main.rand.Next(15) == 0)
                {
                    {
                        var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 124);

                        dust.scale = 0.5f;
                        dust.velocity *= 0;

                    }
                }
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //jump = false;
            return false;
        }
        public override void Kill(int timeLeft)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 30; i++) //Dust post-teleport
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 124);
                    dust.noGravity = true;

                }
            }
        }
    }
}

