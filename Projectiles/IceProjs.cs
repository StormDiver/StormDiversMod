using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;

namespace StormDiversMod.Projectiles
{
    public class IceGrenadeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ice Grenade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;


            Projectile.light = 0.1f;
            Projectile.friendly = true;

            Projectile.CloneDefaults(30);
            AIType = 30;

            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.timeLeft = 180;

        }


        public override void AI()
        {
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 100;
                Projectile.height = 100;
                Projectile.Center = Projectile.position;

                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                Projectile.knockBack = 3f;

            }
            else
            {

                if (Main.rand.NextBool())
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 187, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;

                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            if (Main.rand.Next(1) == 0) // the chance
            {
                target.AddBuff(BuffID.Frostburn, 600);

            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionFrostProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1.25f;

            for (int i = 0; i < 30; i++) //frost dust
            {
                Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 156, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 1.5f;

            }

            for (int i = 0; i < 30; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                dust.noGravity = true;
                dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;

            }
        }
    }
    //__________________________________________________________________________________________________________________
    public class IceStaffProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Icicle");

        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.friendly = true;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            
            Projectile.aiStyle = 2;
        
            Projectile.timeLeft = 120;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.78f;

            if (Main.rand.Next(5) == 0)
            {
                var dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 0, new Color(255, 255, 255), 0.75f)];

                var dust2 = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 284, 0f, 0f, 0, new Color(255, 255, 255), 1f)];

            }
            //dust.noGravity = true;


        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.Next(5) == 0)
            {
                target.AddBuff(BuffID.Frostburn, 180);
            }
        }

    

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

            for (int i = 0; i < 5; i++)
            {

                var dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 284, 0f, 0f, 0, new Color(255, 255, 255), 1f)];

                dust.velocity *= 2;
            }

        }
    }

}