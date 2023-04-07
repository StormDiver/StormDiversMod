using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StormDiversMod.Projectiles.ToolsProjs   
{
   
    public class AircanProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Air Blast");
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 40;
            Projectile.extraUpdates = 1;
            //Projectile.knockBack = 8f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
           
            if (Projectile.ai[0] > 0f)  //this defines where the flames starts
            {
                if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                {

                    
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Vector2 position = Projectile.position;
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, 12, 31, 0f, 0f, 150, default, 100f);
                    
                    Main.dust[dustIndex].scale = 0.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 203, 0f, 0f, 150);
                 
                    dust2.noGravity = true;


                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            return;
        }
        
       
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
    
}