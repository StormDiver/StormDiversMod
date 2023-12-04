using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace StormDiversMod.Projectiles
{
    public class CrackedDaggerProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shattered Dagger");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 40;
            Projectile.light = 0.5f;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1f;
            DrawOffsetX = -0;
            DrawOriginOffsetY = -0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 15; i++)
            {
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 248);
                dust2.noGravity = true;
            }
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
          
            var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 248);
            dust.velocity *= 0.5f;
            dust.scale = 0.75f;
            dust.noGravity = true;

        }
      
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 248, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                dust.scale = 1.25f;
                dust.noGravity = true;

            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 248);
                dust.noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 0.5f, Pitch = -0.2f }, Projectile.Center);

        }

        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 100;
            return color;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(2f, 0);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
    }
}