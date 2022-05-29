using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace StormDiversMod.NPCs.NPCProjs
{
    public class MeteorDropperProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor Fragment");
            Main.projFrames[Projectile.type] = 4;

        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.light = 0.3f;
            Projectile.friendly = false;
            Projectile.hostile = true;
            
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 1;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
           
        }
       
        public override void AI()
        {
  
            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {

                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                dust2.noGravity = true;
                dust2.scale = 1.5f;
                dust2.velocity *= 2;

            }

            AnimateProjectile();

        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
         }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            

            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                dust.noGravity = true;
                dust.scale = 1.5f;
            }
            return true;
        }
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6) 
            {
                Projectile.frame++;
                Projectile.frame %= 4; 
                Projectile.frameCounter = 0;
            }
        }
        public override void Kill(int timeLeft)
        {


        }
        

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        
    }
    
}