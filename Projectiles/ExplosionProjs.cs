﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using StormDiversMod.Common;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace StormDiversMod.Projectiles
{
    public class ExplosionEffect : ModProjectile //test, didn't look so good
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Explosion Generic");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
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
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
        }
        Color color;
        public override Color? GetAlpha(Color lightColor)
        {
            switch (Projectile.ai[0]) //Ai 0 determines colour
            {
                case 0:
                    color = Color.Orange; //Generic
                    break;
                case 1:
                    color = Color.SkyBlue; //Frost
                    break;
                case 2:
                    color = Color.Goldenrod; //Arid
                    break;
                case 3:
                    color = Color.LimeGreen; //Cholophyte
                    break;
                case 4:
                    color = Color.Indigo; //Dark
                    break;
                case 5:
                    color = Color.DeepPink; //Pain
                    break;
                case 6:
                    color = Color.RoyalBlue; //Shroom
                    break;
                case 7:
                    color = Color.Aquamarine; //Vortex
                    break;
                case 8:
                    color = Color.DarkOrchid; //Hellsoul?
                    break;
            }
            color.A = 150;
            return color;
        }
    }
    public class ExplosionGenericProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Explosion Generic");
            Main.projFrames[Projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -49;
            DrawOriginOffsetY = -49;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
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
            //DisplayName.SetDefault("Explosion Dark");
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
            //DisplayName.SetDefault("Explosion Frost");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -49;
            DrawOriginOffsetY = -49;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
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
            //DisplayName.SetDefault("Explosion Shroomite");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -49;
            DrawOriginOffsetY = -49;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
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
            //DisplayName.SetDefault("Explosion Vortex");
            Main.projFrames[Projectile.type] = 7;

        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -49;
            DrawOriginOffsetY = -49;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
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
            //DisplayName.SetDefault("Explosion Soul");
            Main.projFrames[Projectile.type] = 7;

        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -49;
            DrawOriginOffsetY = -49;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
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
            //DisplayName.SetDefault("Explosion Pain");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -49;
            DrawOriginOffsetY = -49;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.Pink;
            color.A = 255;
            return color;
        }
    }
    public class ExplosionPainNofaceProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Explosion Painless");
            Main.projFrames[Projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -49;
            DrawOriginOffsetY = -49;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
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
            //DisplayName.SetDefault("Explosion Arid");
            Main.projFrames[Projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -49;
            DrawOriginOffsetY = -49;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 255;
            return color;
        }
    }

    public class ExplosionChloroProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Explosion Chlorophyte");
            Main.projFrames[Projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -49;
            DrawOriginOffsetY = -49;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 150;
            return color;
        }
    }
    public class ExplosionCompactProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Explosion Compact");
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -26;
            DrawOriginOffsetY = -26;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 150;
            return color;
        }
    }
    public class ExplosionSporeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Explosion Spore");
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -26;
            DrawOriginOffsetY = -26;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 150;
            return color;
        }
    }
}
