using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework.Graphics;


namespace StormDiversMod.Projectiles
{
    public class SoulsProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Damaging Soul");
            Main.projFrames[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 24;
            Projectile.light = 0.5f;
            Projectile.tileCollide = false;
           
            Projectile.scale = 1.5f;
            DrawOffsetX = -0;
            DrawOriginOffsetY = -0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        int dusttype;
        public override void AI()
        {

            AnimateProjectile();
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (Projectile.ai[1] == 0) //Fright
            {
                dusttype = 170;
                DrawOffsetX = 4;

            }
            else if (Projectile.ai[1] == 1) //Sight
            {
                dusttype = 110;
                DrawOffsetX = 4;

            }
            else if (Projectile.ai[1] == 2) //Might
            {
                dusttype = 56;
                DrawOffsetX = 2;

            }

            var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dusttype);
            dust.velocity *= 0.5f;
            dust.noGravity = true;

            if (Projectile.timeLeft >= 24)
            {

                for (int i = 0; i < 15; i++)
                {

                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dusttype);
                    dust2.velocity *= 2;
                    dust2.noGravity = true;

                }

            }
        }
      
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Projectile.ai[1] == 0)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dusttype, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                    dust.scale = 1.25f;
                    dust.noGravity = true;
                }
                else if (Projectile.ai[1] == 1)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dusttype, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                    dust.scale = 0.75f;
                    dust.noGravity = true;

                }
                else if (Projectile.ai[1] == 2)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dusttype, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                    dust.noGravity = true;

                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dusttype);
                dust.velocity *= 2;
                dust.noGravity = true;

            }
            //SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 0.25f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Projectile.Center);
        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;

            if (Projectile.ai[1] == 0) //Fright frames 0-3
            {
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            else if (Projectile.ai[1] == 1) //Sight frames 4-7
            {
                if (Projectile.frame <= 3 || Projectile.frame >= 8)
                {
                    Projectile.frame = 4;
                }
            }
            else if (Projectile.ai[1] == 2) //Might frames 8-11
            {
                if (Projectile.frame <= 7 || Projectile.frame >= 12)
                {
                    Projectile.frame = 8;
                }
            }

            if (Projectile.frameCounter >= 8) //Advacned 1 frame every 8 frames
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
        public override bool PreDraw(ref Color lightColor) //trail
        {
            Main.instance.LoadProjectile(Projectile.type);
            //Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/BetsyFlameProj_Trail");
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                    color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
    }
}