using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace StormDiversMod.Projectiles.AmmoProjs
{
    public class BouncyArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bouncy Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = 1;
            
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            
            
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.arrow = true;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            if (reflect <= 0)
            {
                Projectile.Kill();
            }

        }
        int reflect = 5;
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;

            reflect--;
           
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X *0.8f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y *0.8f;
                }
            }
            SoundEngine.PlaySound(SoundID.Item56, Projectile.Center);
            return false;
        }
      
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            reflect--;

            Projectile.damage = (Projectile.damage * 8) / 10;

            Projectile.velocity.X = Projectile.velocity.X * -0.6f;

            Projectile.velocity.Y = Projectile.velocity.Y * -0.6f;
            SoundEngine.PlaySound(SoundID.Item56, Projectile.Center);
        }
       
        public override void OnKill(int timeLeft)
        {
            
             //int item = Main.rand.NextBool(5) ? Item.NewItem(Projectile.getRect(), ModContent.ItemType<Ammo.BouncyArrow>()) : 0;
             Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
             SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 3; i++)
            {

                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 100, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }

        }
    }
   
}
