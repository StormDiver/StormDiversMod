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
   
    //_______________________________________________________________________________________
    public class MagicMushProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magical Mushroom");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            //Projectile.aiStyle = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.light = 0.2f;
        }
        int reflect = 4;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            reflect--;
       
            {
                
                    Collision.HitTiles(Projectile.Center + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

                    if (Projectile.velocity.X != oldVelocity.X)
                    {
                        Projectile.velocity.X = -oldVelocity.X * 1;
                    }
                    if (Projectile.velocity.Y != oldVelocity.Y)
                    {
                        Projectile.velocity.Y = -oldVelocity.Y * 1f;
                    }

                if (reflect == 0)
                {
                    Projectile.Kill();
                }
                return false;
            }
        }
        public override void AI()
        {
            Projectile.rotation += (float)Projectile.direction * -0.4f;

            if (Main.rand.Next(3) == 0)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 113, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;
                dust.scale = 0.8f;
            }
            Projectile.spriteDirection = Projectile.direction;

           
        }
    
        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
          
            for (int i = 0; i < 10; i++)
            {

                Vector2 vel = new Vector2(Main.rand.NextFloat(20, 20), Main.rand.NextFloat(-20, -20));
                var dust = Dust.NewDustDirect(target.position, target.width, Projectile.height, 113);
                dust.velocity *= 0.5f;

            }
            Projectile.damage = (Projectile.damage * 9) / 10;

        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113);
                    dust.velocity *= 0.1f;
                }
                for (int i = 0; i < 15; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 50;
            return color;

        }
    }
    //_______________________________________________________________________________________
    public class MagicMushArmourProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magical Mushroom");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 16;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.light = 0.2f;
        }
        bool spawntime;
        public override void AI()
        {
            if (!spawntime)
            {
                for (int i = 0; i < 10; i++)
                {

                    Vector2 vel = new Vector2(Main.rand.NextFloat(20, 20), Main.rand.NextFloat(-20, -20));
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113);
                    dust.velocity *= 0.5f;

                }
                SoundEngine.PlaySound(SoundID.Item, Projectile.position, 8);

                spawntime = true;

            }
            Projectile.rotation += (float)Projectile.direction * -0.4f;

            if (Main.rand.Next(3) == 0)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 113, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;
                dust.scale = 0.8f;
            }
            Projectile.spriteDirection = Projectile.direction;


        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            for (int i = 0; i < 10; i++)
            {

                Vector2 vel = new Vector2(Main.rand.NextFloat(20, 20), Main.rand.NextFloat(-20, -20));
                var dust = Dust.NewDustDirect(target.position, target.width, Projectile.height, 113);
                dust.velocity *= 0.25f;

            }
            Projectile.damage = (Projectile.damage * 9) / 10;
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

   
                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 113);
                    dust.velocity *= 0.1f;
                }
                for (int i = 0; i < 15; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 50;
            return color;

        }
    }
}
