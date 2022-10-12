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
    public class ChaosArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chaos Arrow");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.aiStyle = 0;
            Projectile.light = 0.3f;
            Projectile.friendly = true;
            Projectile.timeLeft = 450;
            Projectile.penetrate = -1;
            Projectile.arrow = true;
            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.arrow = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            //DrawOffsetX = +13;
            DrawOriginOffsetY = 0;
        }
        //int spinspeed = 0;

        int reflect = 6;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();
            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 1.2f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 1.2f;
            }

            for (int i = 0; i < 10; i++)
            {
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 248);
                dust2.noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item56, Projectile.Center);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            reflect--;

            Projectile.damage = (Projectile.damage * 9) / 10;

            Projectile.velocity.X = -Projectile.velocity.X * 1.2f;

            Projectile.velocity.Y = -Projectile.velocity.Y * 1.2f;

            for (int i = 0; i < 10; i++)
            {
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 248);
                dust2.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item56, Projectile.Center);
        }
        public override void AI()
        {
            //spinspeed++;
            //Projectile.rotation = (0.4f * spinspeed) * Projectile.direction;
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;


            if (Main.rand.Next(5) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 248, 0f, 0f, 100, default, 0.7f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
            if (reflect <= 0)
            {
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {

            for (int i = 0; i < 10; i++)
            {
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 248);
                dust2.noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0, 0);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
    }
}