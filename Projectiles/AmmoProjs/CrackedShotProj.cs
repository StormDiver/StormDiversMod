using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles.AmmoProjs
{
    public class CrackedShotProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cracked Bullet");

        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;

            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            AIType = ProjectileID.Bullet;
            Projectile.aiStyle = 1;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.extraUpdates = 1;
            Projectile.light = 0.3f;
        }
        //int split = 0;
        public override void OnSpawn(IEntitySource source)
        {
            if (Main.rand.Next(3) == 0)
            {
                SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

                float numberProjectiles = 4;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    float speedX = Projectile.velocity.X;
                    float speedY = Projectile.velocity.Y;
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
                    float scale = 1f - (Main.rand.NextFloat() * .6f);
                    perturbedSpeed = perturbedSpeed * scale;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<CrackedShotProj2>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack / 2, Projectile.owner);
                }
                Projectile.Kill();
            }
        }
        public override void AI()
        {
            /*Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);*/
            Projectile.spriteDirection = Projectile.direction;
            /*split++;
            if (split >= 15 && split < 25)
            {
               
            }*/
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            for (int i = 0; i < 5; i++)
            {
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 248);
                dust2.noGravity = true;
                dust2.scale = 0.75f;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //___________________________________________________________________________________________
    public class CrackedShotProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cracked Bullet Shard");
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;

            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 200;
            AIType = ProjectileID.Bullet;
            Projectile.aiStyle = 1;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.extraUpdates = 1;
            Projectile.light = 0.3f;
            Projectile.ArmorPenetration = 10;
        }

        public override void AI()
        {
          

            Projectile.spriteDirection = Projectile.direction;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 248);
                dust2.noGravity = true;
                dust2.scale = 0.75f;
            }

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
}