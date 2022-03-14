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
    public class DesertBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Bullet");
          
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            AIType = ProjectileID.Bullet;
            Projectile.aiStyle = 1;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.extraUpdates = 1;
            Projectile.light = 0.3f;
        }
        int split = 0;
        public override void AI()
        {
            /*Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);*/
            Projectile.spriteDirection = Projectile.direction;


            split++;

            if (split == 18)
            {

                if (Main.rand.Next(2) == 0)
                {

                    for (int i = 0; i < 10; i++)
                    {

                         
                        var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 10);
                        dust2.noGravity = true;
                    }

                    float numberProjectiles = 2;
                    float rotation = MathHelper.ToRadians(4);
                    //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        float speedX = Projectile.velocity.X;
                        float speedY = Projectile.velocity.Y;
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) ;
                        Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<DesertBulletProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                    Projectile.Kill();

                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {

                 
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 54);
                dust2.noGravity = true;
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {


            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

        }
        public override Color? GetAlpha(Color lightColor)
        {



            return Color.White;

        }

    }
    //___________________________________________________________________________________________
    public class DesertBulletProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Bullet");
            
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
           
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 200;
            AIType = ProjectileID.Bullet;
            Projectile.aiStyle = 1;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.extraUpdates = 1;
            Projectile.light = 0.3f;
        }

        public override void AI()
        {
            /*Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);*/
            Projectile.spriteDirection = Projectile.direction;



        }
      

     

        public override void Kill(int timeLeft)
        {


            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 54);
                dust2.noGravity = true;
            }

        }
        public override Color? GetAlpha(Color lightColor)
        {



            return Color.White;

        }

    }
    //____________________________________________________________________________
    public class DesertArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.aiStyle = 1;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 2;
            Projectile.arrow = true;
            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.arrow = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            DrawOffsetX = -4;
            DrawOriginOffsetY = 0;
        }
        int spinspeed = 0;
        

        int reflect = 3;
        bool spin = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();
            }
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);


                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 1.1f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 1.1f;
                }
                spin = true;
            }
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
            return false;
        }

        public override void AI()
        {


            if (spin)
            {
               
                spinspeed++;
                Projectile.rotation = (0.4f * spinspeed) * Projectile.direction;
                Projectile.penetrate = -1;
             
                DrawOriginOffsetY = -16;
              


                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 10, 0f, 0f, 100, default, 0.7f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }



        }

        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 54);
                dust2.noGravity = true;
            }
        }

    }
    
}