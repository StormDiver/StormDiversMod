using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace StormDiversMod.Projectiles
{
    public class ExplosionGenericProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion Generic");
            Main.projFrames[Projectile.type] = 7;

        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = 25;
            DrawOriginOffsetY = 25;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
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
    public class ExplosionDarkProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion Dark");
            Main.projFrames[Projectile.type] = 7;

        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = 25;
            DrawOriginOffsetY = 25;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
    }
    public class ExplosionFrostProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion Frost");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = 25;
            DrawOriginOffsetY = 25;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 100;
            return color;
        }
    }
    public class ExplosionShroomiteProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion Shroomite");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = 25;
            DrawOriginOffsetY = 25;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 100;
            return color;
        }
    }
    public class ExplosionVortexProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion Vortex");
            Main.projFrames[Projectile.type] = 7;

        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = 25;
            DrawOriginOffsetY = 25;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 100;
            return color;
        }
    }
    public class ExplosionHellSoulProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion Soul");
            Main.projFrames[Projectile.type] = 7;

        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = 25;
            DrawOriginOffsetY = 25;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 100;
            return color;
        }
    }
    public class ExplosionPainProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion Pain");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = 25;
            DrawOriginOffsetY = 25;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.Pink;
            color.A = 255;
            return color;
        }
    }
    public class ExplosionAridProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion Arid");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = 25;
            DrawOriginOffsetY = 25;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 255;
            return color;
        }
    }
}
