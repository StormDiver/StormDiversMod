using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;


namespace StormDiversMod.NPCs.NPCProjs
{
   
    public class HellSoulEnemyProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Red Soul");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            //Projectile.light = 0.4f;

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = 1;

            Projectile.tileCollide = false;
            Projectile.scale = 1.1f;
            Projectile.alpha = 200;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 900;
            //aiType = ProjectileID.LostSoulHostile;
            Projectile.aiStyle = -1;
            // Projectile.CloneDefaults(452);
            //aiType = 452;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }


        readonly int homerandom = Main.rand.Next(180, 300);
        public override void AI()
        {
            var dust3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0);
            dust3.noGravity = true;
            Projectile.ai[0]++;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.ai[0] <= homerandom)
            {
                Projectile.velocity.X *= 0.98f;
                Projectile.velocity.Y *= 0.98f;
            }

            if (Projectile.ai[0] == homerandom)
            {
                for (int i = 0; i < 20; i++)
                {

                    int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5, 0f, 0f, 50, default, 1f);
                    Main.dust[dust2].noGravity = true;

                }
            }
            if (Projectile.ai[0] == homerandom)
            {
                for (int i = 0; i < 200; i++)
                {
                    Player target = Main.player[i];

                    if (Vector2.Distance(target.Center, Projectile.Center) < 2000f && target.active)
                    {
                        float projspeed = 2.5f;
                        Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;

                        Projectile.velocity.X = velocity.X;
                        Projectile.velocity.Y = velocity.Y;
                    }
                }
            }

        }
     
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Buffs.SuperBurnDebuff>(), 300);

            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {

                int dust2 = Dust.NewDust(Projectile.Center - Projectile.velocity, Projectile.width, Projectile.height, 5, 0f, 0f, 50, default, 1f);

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
            Color color = Color.White;
            color.A = 200;
            return color;
        }

    }
}
