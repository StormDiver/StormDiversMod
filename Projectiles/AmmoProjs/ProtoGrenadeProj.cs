using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;


namespace StormDiversMod.Projectiles.AmmoProjs
{

    public class ProtoGrenadeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Proto Grenade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;


           
            Projectile.friendly = true;

            Projectile.aiStyle = 2;

            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.timeLeft = 200;

        }

        int shrapnel = 0;
        public override void AI()
        {
            shrapnel++;
            if (shrapnel == 25)
            {
                if (Main.rand.Next(10) == 0)
                {
                    int numberProjectiles = 4 + Main.rand.Next(2); //This defines how many projectiles to shot.
                    for (int i = 0; i < numberProjectiles; i++)
                    {

                        float speedX = Main.rand.NextFloat(-6f, 6f);
                        float speedY = Main.rand.NextFloat(-6f, 6f);

                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<ProtoGrenadeProj2>(), (int)(Projectile.damage * 0.5), 0, Projectile.owner);
                    }
                    Projectile.Kill();
                }
            }
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 60;
                Projectile.height = 60;
                Projectile.Center = Projectile.position;
                

                Projectile.knockBack = 3f;
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                
                
            }
            else
            {
                
                if (Main.rand.NextBool())
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                    
                    dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                   
                }
            }
            }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionGenericProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1.1f;

            for (int i = 0; i < 30; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, -4f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;

                dust.scale = 1.5f;
                dust.fadeIn = 1.5f;

            }
            for (int i = 0; i < 20; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -2.5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 2f;
                dust.velocity *= 1.5f;

            }
           
        }

    }
    //_____________________________________________________________________________________________________________________________________________
    public class ProtoGrenadeProj2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharpnel");
        }
        public override void SetDefaults()
        {

            Projectile.width = 9;
            Projectile.height = 9;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
        }

        public override void AI()
        {
            if (Main.rand.NextBool())
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;

               

            }
            Projectile.rotation += (float)Projectile.direction * -0.2f;
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
                SoundEngine.PlaySound(SoundID.Tink, Projectile.Center);

                for (int i = 0; i < 5; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width = 10, Projectile.height = 10, 1);
                }

            }
        }
    }
}