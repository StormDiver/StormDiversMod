using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{
    
    public class WebProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Web");

        }
        public override void SetDefaults()
        {

            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 14;
            //aiType = ProjectileID.WoodenArrowFriendly;
            Projectile.ignoreWater = true;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }
        int rotate;
        bool stick;
        public override void AI()
        {
            if (!stick)
            {
                rotate += 1;
                Projectile.rotation = rotate * 0.2f;
                if (Main.rand.Next(3) == 0)
                {
                    Dust dust;
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Vector2 position = Projectile.position;
                    dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                    dust.noGravity = true;

                }
            }
            
            
            /* Dust dust2;
             // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
             Vector2 position2 = Projectile.Center;
             dust2 = Terraria.Dust.NewDustPerfect(position2, 45, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
             dust2.noGravity = true;*/
             if (stick)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                Projectile.knockBack = 0;

            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //stick = true;
            Projectile.damage = (Projectile.damage * 9) / 10;

        }
      
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.Center + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            if (!stick)
            {
                Projectile.penetrate = 5;
                for (int i = 0; i < 5; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                    SoundEngine.PlaySound(SoundID.NPCDeath1 with{Volume = 0.5f, Pitch = 0.2f}, Projectile.Center);


                }
            }
            stick = true;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.NPCDeath1 with{Volume = 0.5f, Pitch = 0.2f }, Projectile.Center);
                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                   
                   

                }

            }

        }
        
    }
   
}
