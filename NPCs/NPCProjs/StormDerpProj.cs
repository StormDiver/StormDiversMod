using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace StormDiversMod.NPCs.NPCProjs
{
    public class StormDerpProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vortex Spike");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 14;

            Projectile.aiStyle = 1;
            Projectile.light = 0.4f;
            
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 400;
            Projectile.penetrate = 4;

            
            Projectile.tileCollide = true;
            Projectile.scale = 1.2f;
            
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Spike;
            

        }
        
       
        
        public override void AI()
        {
            //Projectile.velocity.Y = 1f;
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.Center;
            dust = Terraria.Dust.NewDustPerfect(position, 229, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;

            AnimateProjectile();
            

        }
        

        public override void Kill(int timeLeft)
        {

            Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
           
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 206);
            }

        }

        

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 6; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
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
