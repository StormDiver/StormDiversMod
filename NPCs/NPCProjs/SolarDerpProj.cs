using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StormDiversMod.NPCs.NPCProjs
{
    public class SolarDerpProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Solar Fireball");
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;
            
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 4;

            
            Projectile.tileCollide = true;
            Projectile.scale = 1f;

          

            Projectile.aiStyle = 14;
           // aiType = ProjectileID.Meteor1;


        }
        
       
        
        public override void AI()
        {
            if (Main.rand.NextFloat() < 0.6f)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 153, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }
            Dust dust2;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position2 = Projectile.Center;
            dust2 = Terraria.Dust.NewDustPerfect(position2, 174, new Vector2(0f, 0f), 0, new Color(255, 100, 0), 1f);
            dust2.noGravity = true;
            Projectile.rotation += (float)Projectile.direction * -0.2f;

            AnimateProjectile();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            
            
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Buffs.UltraBurnDebuff>(), 180);


        }

        public override void OnKill(int timeLeft)
        {

            Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
            //Main.PlaySound(SoundID.Item10, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 244);
                //Main.PlaySound(SoundID.NPCHit, (int)Projectile.Center.X, (int)Projectile.Center.Y, 3);
            }

        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 2; // Will reset to the first frame if you've gone through them all.
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
