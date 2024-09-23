using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Common;
using StormDiversMod.Buffs;
using Terraria.GameContent.Drawing;
using Terraria.GameContent;

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
    //__________________________________________________________________________________________________________________
    public class IceStaff2Proj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Giant Icicle");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.friendly = true;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 180;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.78f;

            if (Main.rand.Next(3) == 0)
            {
                var dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 0, new Color(255, 255, 255), 0.75f)];

                var dust2 = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 284, 0f, 0f, 0, new Color(255, 255, 255), 1.25f)];
                dust2.noGravity = true;
            }
            //dust.noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 180);
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
    //__________________________________________________________________________________________________________________
    public class IceStaff2Proj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Giant Spinning Icicle");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.friendly = true;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.rotation += 0.4f * Projectile.direction;

            if (Main.rand.Next(1) == 0)
            {
                var dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 0, new Color(255, 255, 255), 0.75f)];

                var dust2 = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 284, 0f, 0f, 0, new Color(255, 255, 255), 1.25f)];
                dust2.noGravity = true;
            }
            //dust.noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 180);
            }
            Projectile.velocity *= 0.66f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 1f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 1f;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            for (int i = 0; i < 25; i++)
            {
                var dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 0, new Color(255, 255, 255), 1.5f)];
                dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 284, 0f, 0f, 0, new Color(255, 255, 255), 1.25f)];
                dust.velocity *= 1.5f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
    }
}