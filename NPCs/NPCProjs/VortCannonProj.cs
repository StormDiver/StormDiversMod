using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace StormDiversMod.NPCs.NPCProjs
{
    public class VortCannonProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vortexian Rocket");
            
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            
            Projectile.light = 0.4f;
            
            Projectile.friendly = false;
            Projectile.hostile = true;
            
            Projectile.penetrate = 1;
            
            
            Projectile.tileCollide = true;
            Projectile.scale = 1.1f;

            
            Projectile.extraUpdates = 1;
            
           
            Projectile.timeLeft = 600;
            //aiType = ProjectileID.LostSoulHostile;
            Projectile.aiStyle = -1;
            // Projectile.CloneDefaults(452);
            //aiType = 452;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }
        
       
        
        
        public override void AI()
        {
            
          
            
          
            for (int i = 0; i < 10; i++)
            {
                float x2 = Projectile.Center.X - Projectile.velocity.X / 20f * (float)i;
                float y2 = Projectile.Center.Y - Projectile.velocity.Y / 20f * (float)i;
                int j = Dust.NewDust(new Vector2(x2, y2), 1, 1, 229);
                //Main.dust[num165].alpha = alpha;
                Main.dust[j].position.X = x2;
                Main.dust[j].position.Y = y2;
                Main.dust[j].velocity *= 0.3f;
                Main.dust[j].noGravity = true;
                Main.dust[j].scale = 0.8f;
            }
            if (Projectile.timeLeft <= 3)
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                // Set to transparent. This Projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original Projectile center. This makes the Projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 100;
                Projectile.height = 100;
                Projectile.Center = Projectile.position;


                Projectile.knockBack = 6f;

            }
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            
         
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {

            Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
            
            for (int i = 0; i < 30; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 229, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 2f;
                dust.fadeIn = 1f;

            }
            for (int i = 0; i < 50; i++) //Green particle circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -6f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 229, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;

                dust.scale = 1.5f;
                dust.fadeIn = 1.5f;

            }
        }

      
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }
    }
}
