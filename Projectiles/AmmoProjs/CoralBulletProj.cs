using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles.AmmoProjs
{
    
    public class CoralBulletProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Bullet");

        }
        public override void SetDefaults()
        {

            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = 1;
            //aiType = ProjectileID.WoodenArrowFriendly;
            Projectile.ignoreWater = true;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            AIType = ProjectileID.Bullet;

        }
        public override void AI()
        {
          

            if (Main.rand.Next(4) == 0)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 33, 0f, 0f, 0, new Color(255, 255, 255), 0.75f);
                dust.noGravity = true;

            }
          

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Wet, 300);

        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Wet, 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.Item85, Projectile.Center);
                for (int i = 0; i < 5; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 25);
                    dust.scale = 0.7f;
                }

            }
           
        }
        
    }
    //________________________________________________________
   
}
