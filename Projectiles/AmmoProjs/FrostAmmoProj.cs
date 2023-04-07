using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles.AmmoProjs
{
    public class FrostBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;

            Projectile.aiStyle = 1;
            Projectile.light = 0.4f;
            
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 2;
            
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            AIType = ProjectileID.Bullet;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
        }
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
           
            //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
           
            {
                float speedX = Projectile.velocity.X;
                float speedY = Projectile.velocity.Y;
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
                
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X * 10, perturbedSpeed.Y * 10), ModContent.ProjectileType<FrostBulletProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            Projectile.Kill();
            return false;
        }
        // int dusttime = 10;
        public override void AI()
        {
            
            {
                //Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
            }
        }



        public override void Kill(int timeLeft)
        {

            //Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
            }

        }

        public override Color? GetAlpha(Color lightColor)
        {



            return Color.White;
    
        }

    }

    //_____________________________________________________________________________________________________________________________________________________
    public class FrostBulletProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;

            Projectile.aiStyle = 1;
            Projectile.light = 0.4f;

            Projectile.friendly = true;
            Projectile.timeLeft = 28;
            Projectile.penetrate = 2;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.tileCollide = false;

            Projectile.DamageType = DamageClass.Ranged;

            AIType = ProjectileID.Bullet;


        }
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
           
            return false;
        }
        // int dusttime = 10;
        public override void AI()
        {

            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
            }
        }

        public override void Kill(int timeLeft)
        {

            //Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
            }

        }
        public override Color? GetAlpha(Color lightColor)
        {



            return Color.White;

        }


    }

    //_____________________________________________________________________________________________________________________________________________________

    public class FrostArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.aiStyle = 1;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.arrow = true;
            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.arrow = true;
            AIType = ProjectileID.WoodenArrowFriendly;
            //Creates no immunity frames
           

            DrawOffsetX = -4;
            DrawOriginOffsetY = 0;
        }
       
        public override void AI()
        {
           
               /* int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 187, 0f, 0f, 100, default, 0.7f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;*/
        }
        
       
       

        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

            for (int i = 0; i < 10; i++)
            {

                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 92, 0f, 0f, 0, new Color(255, 255, 255), 0.7f)];
                dust.noGravity = true;

            }

            int numberProjectiles = 2 + Main.rand.Next(2); //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {

                float speedX = Main.rand.NextFloat(-5f, 5f);
                float speedY = Main.rand.NextFloat(-5f, 5f);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ProjectileID.CrystalShard, (int)(Projectile.damage * 0.33f), 0, Projectile.owner);
            }

        }
       
    }

}
