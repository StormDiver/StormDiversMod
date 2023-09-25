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
    public class DualArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dual Arrow");
           
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
           
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged; 
            Projectile.timeLeft = 2000;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.aiStyle = 1;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
           
            DrawOffsetX = -2;
            DrawOriginOffsetY = -0;
        }
        int split = 0;


        public override void AI()
        {
            /*Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);*/
            Projectile.spriteDirection = Projectile.direction;
            
            split++;
            
            if (split == 40)
            {
                if (Main.rand.Next(3) == 0)
                {

                    SoundEngine.PlaySound(SoundID.Item5 with{Volume = 1f, Pitch = -0.5f}, Projectile.Center);
                    for (int i = 0; i < 10; i++)
                    {

                         
                        var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 129);
                        dust2.noGravity = true;
                    }

                   
                    //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                   
                    for (int i = 0; i < 2; i++)
                    {
                        float speedX = Projectile.velocity.X;
                        float speedY = Projectile.velocity.Y;
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10)); // This defines the projectiles random spread . 10 degree spread.
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ProjectileID.WoodenArrowFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                    Projectile.Kill();

                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            
        }

       

        public override void OnKill(int timeLeft)
        {



            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 129);
                dust2.noGravity = true;
            }
        }
       
    }
   
}