using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.DataStructures;

namespace StormDiversMod.NPCs.NPCProjs
{
    public class DerpMimicProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Shadow");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;

                        
            Projectile.friendly = false;
            Projectile.hostile = true;
            
            Projectile.penetrate = 1;
            
            
            Projectile.tileCollide = false;
            Projectile.scale = 1f;


            Projectile.extraUpdates = 0;
            
           
            Projectile.timeLeft = 300;
            //aiType = ProjectileID.LostSoulHostile;
            Projectile.aiStyle = 0;
            // Projectile.CloneDefaults(452);
            //aiType = 452;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Main.masterMode)
                Projectile.damage /= 6;
            else if (Main.expertMode && !Main.masterMode)
                Projectile.damage /= 4;
            else
                Projectile.damage /= 2;
        }
        public override void AI()
        {
            Projectile.ai[0]++;

            if (Projectile.ai[0] <= 20)
            {
                Projectile.velocity.X *= 1.04f;
                Projectile.velocity.Y *= 1.04f;

            }

            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 109, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f, 0, new Color(255, 255, 255), 1f)];
            dust.noGravity = true;
            dust.scale = 1.5f;

            if (Projectile.velocity.X < 0)
                Projectile.spriteDirection = -1;
            else
                Projectile.spriteDirection = 1;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            for (int i = 0; i < 15; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 109, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 130, default, 1.2f);
                dust.noGravity = true;
            }
            for (int i = 0; i < 3; i++)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningHit, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(target.Center.X, target.Center.Y),

                }, target.whoAmI);
            }
            target.AddBuff(BuffID.Obstructed, 60);
        }
        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.NPCDeath6 with{Volume = 0.75f}, Projectile.Center);

            for (int i = 0; i < 100; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 109);
                dust.scale = 2;
                dust.velocity *= 2;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.DarkSlateGray;
            color.A = 255;
            return color;
        }
    }
}
