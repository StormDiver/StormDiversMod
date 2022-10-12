using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using StormDiversMod;
using static Terraria.ModLoader.ModContent;


namespace StormDiversMod.Projectiles
{
    public class ThrowingSilverKniveProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver Throwing Knive");

        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = 2;


            Projectile.friendly = true;
            Projectile.timeLeft = 400;
            Projectile.penetrate = 3;

            Projectile.tileCollide = true;

           Projectile.DamageType = DamageClass.Ranged;

            DrawOffsetX = 0;
            DrawOriginOffsetY = -0;

        }


        int spinspeed = 0;

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (spin)
            {

                spinspeed++;
                Projectile.rotation = (0.4f * spinspeed) * Projectile.direction;
               
                DrawOriginOffsetY = -8;
      
                
            }

        }
        int reflect = 2;
        bool spin = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();
            }
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);


                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.8f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
                }
                spin = true;
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9 / 10);
        }


        public override void Kill(int timeLeft)
        {
            //int item = Main.rand.NextBool(5) ? Item.NewItem(Projectile.getRect(), ModContent.ItemType<Weapons.MetalSilverKnive>()) : 0;

            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            for (int i = 0; i < 5; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 30);
            }
        }



    }
    public class ThrowingTungstenKniveProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tungsten Throwing Knive");
            
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = 2;


            Projectile.friendly = true;
            Projectile.timeLeft = 400;
            Projectile.penetrate = 3;

            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Ranged;

            DrawOffsetX = 0;
            DrawOriginOffsetY = -0;
        }

        int spinspeed = 0;

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (spin)
            {

                spinspeed++;
                Projectile.rotation = (0.4f * spinspeed) * Projectile.direction;

                DrawOriginOffsetY = -8;


            }

        }
        int reflect = 2;
        bool spin = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();
            }
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);


                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.8f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
                }
                spin = true;
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9 / 10);
        }
        

        public override void Kill(int timeLeft)
        {
            //int item = Main.rand.NextBool(5) ? Item.NewItem(Projectile.getRect(), ModContent.ItemType<Weapons.MetalTungstenBullet>()) : 0;

            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            for (int i = 0; i < 5; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 30);
            }
        }



    }
    public class ThrowingKnifeBouncyProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bouncy Knife");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = 2;


            Projectile.friendly = true;
            Projectile.timeLeft = 400;
            Projectile.penetrate = 5;

            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Ranged;

            DrawOffsetX = 0;
            DrawOriginOffsetY = -0;

        }


        int spinspeed = 0;

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (spin)
            {

                spinspeed++;
                Projectile.rotation = (0.4f * spinspeed) * Projectile.direction;

                DrawOriginOffsetY = -8;


            }

        }
        int reflect = 5;
        bool spin = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();
            }
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);


                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.9f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
                }
                spin = true;
            }
            SoundEngine.PlaySound(SoundID.Item56, Projectile.Center);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            Projectile.damage = (Projectile.damage * 8) / 10;

            Projectile.velocity.X = Projectile.velocity.X * -0.8f;

            Projectile.velocity.Y = Projectile.velocity.Y * -0.8f;
            SoundEngine.PlaySound(SoundID.Item56, Projectile.Center);

        }



        public override void Kill(int timeLeft)
        {
            //int item = Main.rand.NextBool(5) ? Item.NewItem(Projectile.getRect(), ModContent.ItemType<Weapons.MetalSilverKnive>()) : 0;

            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            for (int i = 0; i < 5; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 30);
            }
        }

    }
}
