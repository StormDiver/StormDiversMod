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
        int projsleft = 17; //1 extra
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
            if (player.itemAnimation == 1 && player.HeldItem.type == ModContent.ItemType<Items.Weapons.StickyLauncher>())
            {
                projsleft -= 1; 
            }
            if (projsleft == 0 && Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
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



            for (int i = 0; i < 50; i++) //Flame particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -10f).RotatedByRandom(MathHelper.ToRadians(360));

                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 2.5f);
                Main.dust[dustIndex].noGravity = true;
               

            }
            for (int i = 0; i < 50; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -2.5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 3f;
                dust.velocity *= 2.5f;

            }
            for (int i = 0; i < 50; i++) //Grey dust fade
            {

                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default, 1f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }

    }
   
}