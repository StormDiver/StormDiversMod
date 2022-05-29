using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{

    public class StickyBombProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiky Bomb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;


           
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = 2;

            Projectile.penetrate = 5;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 3600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -1;
            DrawOriginOffsetY = -1;
        }
        bool stick; //wheter bomb has hit a tile
        bool boomed; //when it is exploding
        int boomtime = 0; //How long until you can actually detonate
        bool unstick; //Bool for if you unstick the bombs
        
        public override bool? CanDamage()
        {
            if (unstick)
            {
                return true;
            }
            if (!boomed)
            {
                return false;

            }
            else
            {
                return true;
            }
        }
        
        public override void AI()
        {
            boomtime++;
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                boomed = true;

                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 200;
                Projectile.height = 200;
                Projectile.Center = Projectile.position;


                Projectile.knockBack = 3f;
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                //Projectile.hostile = true;


            }
            else
            {

                if (!stick)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;

                    dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;

                }
            }

            if (stick && !unstick)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                Projectile.knockBack = 0;

            }
            var player = Main.player[Projectile.owner];

            if ((player.controlUseTile && !player.controlDown && player.HeldItem.type == ModContent.ItemType<Items.Weapons.StickyLauncher>() && boomtime > 30) || player.dead) //will go BOOM
            {
                if (Projectile.timeLeft > 3)
                {
                    Projectile.timeLeft = 3;
                }
            }
            if ((player.controlUseTile && player.controlDown && !unstick && stick)) //will go BOOM
            {
                SoundEngine.PlaySound(SoundID.Item108, Projectile.Center);
                Projectile.velocity.Y = -2;
                unstick = true;
            }

        }
            
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            damage /= 4;
            target.velocity.Y = -15;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!unstick)
            {
                Collision.HitTiles(Projectile.Center + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            }
            if (!stick)
            {
                Projectile.penetrate = -1;
                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);


                }
                SoundEngine.PlaySound(SoundID.NPCHit1 with{Volume = 0.5f, Pitch = 0.2f}, Projectile.Center);

            }
            stick = true;
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (unstick)
            {
                if (Projectile.timeLeft > 3)
                {
                    Projectile.timeLeft = 3;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);



            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X- 10, Projectile.Center.Y - 10), 20, 20, 6, 0f, 0f, 100, default, 2f);
                Main.dust[dustIndex].noGravity = false;
                Main.dust[dustIndex].velocity *= 4f;
                int dustIndex2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y - 10), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 3f);
                Main.dust[dustIndex2].noGravity = true;
                Main.dust[dustIndex2].velocity *= 2;

            }
            for (int i = 0; i < 40; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.Center;
                dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 3f;
                dust.velocity *= 5;

            }
            for (int i = 0; i < 50; i++)
            {

                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default, 1f);
                Main.dust[dustIndex].scale = 0.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }

    }
   
}