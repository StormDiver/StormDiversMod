using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;


namespace StormDiversMod.NPCs.NPCProjs
{
    public class GladiatorMiniBossProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fallen Warrior Sword");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;

        }
        public override void SetDefaults()
        {

            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 120;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
        }

        int speedup = 0;
        public override void AI()
        {
            if (speedup == 1)
            {
                for (int i = 0; i < 10; i++)
                {

                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 57);
                    dust2.noGravity = true;
                    dust2.velocity *= 3;

                }
            }

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.height, Projectile.width, 57, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
            dust.noGravity = true;

            speedup++;
            if (speedup <= 80)
            {
                Projectile.velocity.X *= 1.04f;
                Projectile.velocity.Y *= 1.04f;

            }

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {

        }

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void Kill(int timeLeft)
        {

            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 57);
                dust.noGravity = true;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }

}
