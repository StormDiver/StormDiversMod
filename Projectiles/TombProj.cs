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
    
    public class TombProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TombStone");

        }
        public override void SetDefaults()
        {

            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 2;
            //aiType = ProjectileID.WoodenArrowFriendly;
            Projectile.ignoreWater = true;
            DrawOffsetX = 0;
            DrawOriginOffsetY = -2;
        }
        int rotate;
        public override void AI()
        {
            rotate += 1;
            Projectile.rotation = rotate * 0.2f;

            if (Main.rand.Next(3) == 0)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 1, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;

            }
            if (Main.rand.Next(3) == 0)
            {
                Dust dust2;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust2 = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 2, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust2.noGravity = true;

            }
            /* Dust dust2;
             // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
             Vector2 position2 = Projectile.Center;
             dust2 = Terraria.Dust.NewDustPerfect(position2, 45, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
             dust2.noGravity = true;*/


        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
       
            Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(target.Center.X - 11, target.Top.Y - 15), new Vector2(0, -3), ModContent.ProjectileType<GhostProj>(), (int)(Projectile.damage * 1f), 1, Projectile.owner);
            for (int i = 0; i < 15; i++)
            {


                var dust = Dust.NewDustDirect(target.position, target.width, target.height, 31);

                dust.noGravity = true;

            }

        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            //target.AddBuff(BuffID.Wet, 300);
        }

        int reflect = 4;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.Center + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            Projectile.damage = (Projectile.damage * 9) / 10;
            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();

            }

            {

                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.9f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
                }
            }
            if (reflect >= 1)
            {
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 62, 0.5f, 1.5f);
            }
            for (int i = 0; i < 5; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 1);
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 2);

            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 62);
                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 1);
                    dust.scale = 0.8f;
                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 2);
                    dust2.scale = 0.8f;

                }

            }

        }
        
    }
    //___________________________
    public class GhostProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Ghost");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {

            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;

            Projectile.scale = 1f;
            Projectile.CloneDefaults(189);
            AIType = 189;

            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        int damagetime = 0;
        public override bool? CanDamage()
        {
            if (damagetime <= 25)
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
            damagetime++;
            Projectile.width = 22;
            Projectile.height = 22;

            AnimateProjectile();

           
        }
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.6f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.6f;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
           
            SoundEngine.PlaySound(SoundID.NPCKilled, (int)Projectile.Center.X, (int)Projectile.Center.Y, 6, 0.5f, 1);
            
            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {


                for (int i = 0; i < 7; i++)
                {

                    
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);

                    dust.noGravity = true;

                }

            }
        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 5; // Will reset to the first frame if you've gone through them all.
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
