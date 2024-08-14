using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

using System.Linq;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using StormDiversMod.Projectiles.SentryProjs;
using Terraria.GameContent.Drawing;

namespace StormDiversMod.Projectiles
{
    public class DeathsListProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nine Lives Soul");
            //Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.knockBack = 0;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 99999999;
            Projectile.tileCollide = false;
            Projectile.MaxUpdates = 1;
        }
        float degrees;
        bool scaleup;
        //int shoottime;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        bool escaped;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.ai[0] == 1 && Projectile.ai[1] != 9)//no effect when just escaping without being collected
            {
                for (int i = 0; i < 1; i++)
                {
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningHit, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

                    }, player.whoAmI);
                }
            }

            Projectile.ai[0]++;
            if (Projectile.ai[1] <= 8)
            degrees = Projectile.ai[1] * 40;

            //ai[0] 9 escapes immedietaly, used for when dropped item expires 

            if (player.GetModPlayer<EquipmentEffects>().ninelives <= Projectile.ai[1] || player.dead)
            {
                if (!escaped)//mark soul as escaped
                {
                    Projectile.timeLeft = 300;

                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.StellarTune, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

                    }, player.whoAmI);

                    SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1f, Pitch = 0.5f, MaxInstances = 2, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Projectile.Center);
                    escaped = true;
                }     
                //Projectile.Kill();
            }

            if (!escaped)//prevent escaped souls returning
            {
                Projectile.rotation = player.velocity.X / 40;

                //Factors for calculations
                double deg = (degrees + 90); //The degrees, you can multiply Projectile.ai[1] to make it orbit faster, may be choppy depending on the value
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 75; //Distance away from the player

                Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
                Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
            }
            
            if (escaped)//fly up to freedom
            {
                if (Projectile.velocity.Y >= -15)
                {
                    Projectile.velocity.Y -= 0.2f;
                }
                Projectile.rotation = 0;
            }
            if (Main.rand.Next(10) == 0)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                dust.noGravity = true;
                dust.velocity *= 0.75f;
                dust.scale = 0.75f;
            }
            if (scaleup)
            {
                Projectile.scale += 0.01f;
            }
            else
            {
                Projectile.scale -= 0.01f;
            }
            if (Projectile.scale >= 1f)
            {
                scaleup = false;
            }
            if (Projectile.scale <= 0.75f)
            {
                scaleup = true;
            }
            // AnimateProjectile();
        }
        public override bool? CanDamage()
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 10; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                    dust.noGravity = true;
                }
            }
        }
        Texture2D texture;
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/DeathsListProj");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 150;
            return color;
        }
    }

}