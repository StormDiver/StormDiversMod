using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using StormDiversMod.Common;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

using static Terraria.Main;


using Terraria.GameContent.Drawing;
using System.Collections.Generic;
using Mono.Cecil;

namespace StormDiversMod.Projectiles
{
    
    public abstract class SwordAura : ModProjectile
    {
        public override string Texture => "StormDiversMod/Projectiles/SwordAura";
        public virtual void AuraDefaults()
        {

        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(16);

            Projectile.aiStyle = 190;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.usesOwnerMeleeHitCD = true;
            Projectile.ownerHitCheckDistance = 300f;
            AuraDefaults();
        }
        public override bool PreAI()
        {
            SwingAI();
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {

            float coneLength = 95 * Projectile.scale;
            float maximumAngle = MathF.PI / 2.5f;
            float coneRotation = Projectile.rotation - 0.6f * Projectile.direction;
            return IntersectsConeFastInaccurate(targetHitbox, Projectile.Center, coneLength, coneRotation, maximumAngle) && Projectile.localAI[0] > 1;
        }

        public static bool IntersectsConeFastInaccurate(Rectangle targetRect, Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle)
        {
            Vector2 point = coneCenter + coneRotation.ToRotationVector2() * coneLength;
            Vector2 spinningpoint = targetRect.ClosestPointInRect(point) - coneCenter;
            float num = spinningpoint.RotatedBy(0f - coneRotation).ToRotation();
            if (num < 0f - maximumAngle || num > maximumAngle)
            {
                return false;
            }
            return spinningpoint.Length() < coneLength;
        }

        protected float scaleIncrease = 0f;
        protected Color frontColor = Color.White;
        protected Color middleColor = Color.White;
        protected Color backColor = Color.White;
        protected bool once = false;

        public override bool PreDraw(ref Color lightColor)
        {
            Color[] palette = new Color[] { frontColor, middleColor, backColor };
            SwordAura.DrawProj_BladeAura(Projectile, palette);
            return false;
        }

        public void SwingAI()
        {
            //if (Projectile.localAI[0] == 0f)
            //{
            //    SoundEngine.PlaySound(SoundID.Item60 with { Volume = 0.65f }, Projectile.position);
            //}
            Projectile.localAI[0] += 1f;
            Player player = Main.player[Projectile.owner];
            float progress = Projectile.localAI[0] / Projectile.ai[1];
            float whichSide = Projectile.ai[0];
            float velToRot = Projectile.velocity.ToRotation();
            float realRotation = MathF.PI * whichSide * progress + velToRot + whichSide * MathF.PI + player.fullRotation;
            Projectile.rotation = realRotation;
            float baseScale = 1f;

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = (baseScale + progress * scaleIncrease) * Projectile.ai[2];
            float rotationWithDeviation = Projectile.rotation + Main.rand.NextFloatDirection() * (MathF.PI / 2f) * 0.7f;
            Vector2 edgePosition = Projectile.Center + rotationWithDeviation.ToRotationVector2() * 85f * Projectile.scale;
            Vector2 vector8 = (rotationWithDeviation + Projectile.ai[0] * (MathF.PI / 2f)).ToRotationVector2();
            Lighting.AddLight(Projectile.Center, middleColor.ToVector3());
            Projectile.scale *= 1;
            if (progress > 1)
            {
                Projectile.Kill();
            }
        }
        public static void DrawProj_BladeAura(Projectile proj, Color[] colorArray)
        {
            Vector2 vector = proj.Center - screenPosition;
            Asset<Texture2D> texture = TextureAssets.Projectile[proj.type];
            Rectangle rectangle = texture.Frame(1, 4);
            Vector2 origin = rectangle.Size() / 2f;
            float num = proj.scale * 1.1f;
            SpriteEffects effects = ((!(proj.ai[0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None);
            float progress = proj.localAI[0] / proj.ai[1];
            float remappedProgress = Utils.Remap(progress, 0f, 0.6f, 0f, 1f) * Utils.Remap(progress, 0.6f, 1f, 1f, 0f);
            //remappedProgress = progress;
            //Main.NewText($"{remappedProgress}, {Utils.Remap(progress, 0f, 0.8f, 0f, 1f)}, {Utils.Remap(progress, 0.8f, 1f, 1f, 0f)}");
            float num4 = 0.975f;
            float lightingMultiplier = Lighting.GetColor(proj.Center.ToTileCoordinates()).ToVector3().Length() / MathF.Sqrt(3);
            lightingMultiplier = 0.5f + lightingMultiplier * 0.5f;
            lightingMultiplier = Utils.Remap(lightingMultiplier, 0.2f, 1f, 0f, 1f);
            Color blue = colorArray[2];//new Color(45, 124, 205);//blue
            Color lime = colorArray[0];//new Color(181, 230, 29);//yellowlime
            Color green = colorArray[1];//new Color(34, 177, 76);//green
            Color whiteOverlay = Color.White * remappedProgress * 0.5f;
            whiteOverlay.A = (byte)((float)(int)whiteOverlay.A * (1f - lightingMultiplier));
            Color color5 = whiteOverlay * lightingMultiplier * 0.5f;
            color5.G = (byte)((float)(int)color5.G * lightingMultiplier);
            color5.B = (byte)((float)(int)color5.R * (0.25f + lightingMultiplier * 0.75f));
            spriteBatch.Draw(texture.Value, vector, rectangle, blue * lightingMultiplier * remappedProgress, proj.rotation + proj.ai[0] * (MathF.PI / 4f) * -1f * (1f - progress), origin, num * 0.95f, effects, 0f);
            spriteBatch.Draw(texture.Value, vector, rectangle, color5 * 0.15f, proj.rotation + proj.ai[0] * 0.01f, origin, num, effects, 0f);
            spriteBatch.Draw(texture.Value, vector, rectangle, green * lightingMultiplier * remappedProgress * 0.3f, proj.rotation, origin, num, effects, 0f);
            spriteBatch.Draw(texture.Value, vector, rectangle, lime * lightingMultiplier * remappedProgress * 0.5f, proj.rotation, origin, num * num4, effects, 0f);
            spriteBatch.Draw(texture.Value, vector, texture.Frame(1, 4, 0, 3), Color.White * 0.6f * remappedProgress, proj.rotation + proj.ai[0] * 0.01f, origin, num, effects, 0f);
            spriteBatch.Draw(texture.Value, vector, texture.Frame(1, 4, 0, 3), Color.White * 0.5f * remappedProgress, proj.rotation + proj.ai[0] * -0.05f, origin, num * 0.8f, effects, 0f);
            spriteBatch.Draw(texture.Value, vector, texture.Frame(1, 4, 0, 3), Color.White * 0.4f * remappedProgress, proj.rotation + proj.ai[0] * -0.1f, origin, num * 0.6f, effects, 0f);
            for (float num6 = 0f; num6 < 12f; num6 += 1f)
            {
                float num7 = proj.rotation + proj.ai[0] * (num6 - 2f) * (MathF.PI * -2f) * 0.025f + Utils.Remap(progress, 0f, 1f, 0f, MathF.PI / 4f) * proj.ai[0];
                Vector2 drawpos = vector + num7.ToRotationVector2() * ((float)texture.Width() * 0.5f - 6f) * num;
                float num8 = num6 / 12f;
                DrawPrettyStarSparkle(proj.Opacity, SpriteEffects.None, drawpos, new Color(255, 255, 255, 0) * remappedProgress * num8, green, progress, 0f, 0.5f, 0.5f, 1f, num7, new Vector2(0f, Utils.Remap(progress, 0f, 1f, 3f, 0f)) * num, Vector2.One * num);
            }
            Vector2 drawpos2 = vector + (proj.rotation + Utils.Remap(progress, 0f, 1f, 0f, MathF.PI / 4f) * proj.ai[0]).ToRotationVector2() * ((float)texture.Width() * 0.5f - 4f) * num;
            DrawPrettyStarSparkle(proj.Opacity, SpriteEffects.None, drawpos2, new Color(255, 255, 255, 0) * remappedProgress * 0.5f, green, progress, 0f, 0.5f, 0.5f, 1f, 0f, new Vector2(2f, Utils.Remap(progress, 0f, 1f, 4f, 1f)) * num, Vector2.One * num * 1.5f);
        }
        public static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D texture = TextureAssets.Extra[98].Value;
            Color bigShineColor = shineColor * opacity * 0.5f;
            bigShineColor.A = 0;
            Vector2 origin = texture.Size() / 2f;
            Color smallShineColor = drawColor * 0.5f;
            float brightness = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 vector = new Vector2(fatness.X * 0.5f, scale.X) * brightness;
            Vector2 vector2 = new Vector2(fatness.Y * 0.5f, scale.Y) * brightness;
            bigShineColor *= brightness;
            smallShineColor *= brightness;
            Main.EntitySpriteDraw(texture, drawpos, null, bigShineColor, MathHelper.PiOver2 + rotation, origin, vector, dir, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, bigShineColor, rotation, origin, vector2, dir, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, smallShineColor, MathHelper.PiOver2 + rotation, origin, vector * 0.6f, dir, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, smallShineColor, rotation, origin, vector2 * 0.6f, dir, 0);
        }
    }
    
    public class LightDarkAuraLight : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.2f;
            frontColor = Color.LightGray;
            middleColor = Color.Silver;
            backColor = Color.Violet;
            //backColor = new Color(192, 176, 138);
        }
        int hits;
        public override bool? CanDamage()
        {
            if (hits < 4)
                return true;
            else
                return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];
            for (int i = 0; i < 3; i++)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(target.Center.X + Main.rand.Next(-target.width / 3, target.width / 3), target.Center.Y + Main.rand.Next(-target.height / 3, target.height / 3)),

                }, player.whoAmI);
            }
            hits++;
        }
    }
    public class LightDarkAuraDark : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.6f;
            frontColor = Color.DarkViolet;
            middleColor = Color.Indigo;
            backColor = Color.Black;
            //backColor = new Color(192, 176, 138);
        }
        int hits;
        public override bool? CanDamage()
        {
            if (hits < 4)
                return true;
            else
                return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];
            for (int i = 0; i < 3; i++)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningSmall, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(target.Center.X + Main.rand.Next(-target.width / 3, target.width / 3), target.Center.Y + Main.rand.Next(-target.height / 3, target.height / 3)),

                }, player.whoAmI);
            }
            if (hits < 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(target.Center.X, target.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.SwordDarkProj>(), Projectile.damage / 2, 0, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item73, player.Center);

            }
            hits++;
        }
    }
    public class SoulAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.15f;
            frontColor = Color.Aqua;
            middleColor = Color.DeepSkyBlue;
            backColor = Color.SlateBlue;
            //backColor = new Color(192, 176, 138);
        }

        int hits;
        public override bool? CanDamage()
        {
            if (hits < 4)
                return true;
            else
                return false;
        }
        public override void AI()
        {
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            /*for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(target.position, target.width, target.height, 173, 0, 0);
                dust.scale = 2;
            }*/
            var player = Main.player[Projectile.owner];
            for (int i = 0; i < 3; i++)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.PrincessWeapon, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(target.Center.X + Main.rand.Next(-target.width / 3, target.width / 3), target.Center.Y + Main.rand.Next(-target.height / 3, target.height / 3)),

                }, player.whoAmI);
            }
            target.AddBuff(ModContent.BuffType<Buffs.HellSoulFireDebuff>(), 480);
            hits++;

        }
    }
    public class DerpAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.25f;
            frontColor = Color.DeepSkyBlue;
            middleColor = Color.DodgerBlue;
            backColor = Color.Blue;
            //backColor = new Color(192, 176, 138);
        }
        int hits;
        public override bool? CanDamage()
        {
            if (hits < 5)
                return true;
            else
                return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(target.Center, 0, 0, 68);
                dust.noGravity = true;
            }
            hits++;
        }
    }
    public class AsteroidAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 1f;
            frontColor = Color.SlateBlue;
            middleColor = Color.MediumPurple;
            backColor = Color.Indigo;
            //backColor = new Color(192, 176, 138);
        }
        int hits;
        public override bool? CanDamage()
        {
            if (hits < 6)
                return true;
            else
                return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.NightsEdge, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(target.Center.X + Main.rand.Next(-target.width / 3, target.width / 3), target.Center.Y + Main.rand.Next(-target.height / 3, target.height / 3)),

            }, player.whoAmI);
            hits++;
        }
    }
    public class RoseAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.9f;
            frontColor = Color.Crimson;
            middleColor = Color.DarkRed;
            backColor = Color.Black;
            //backColor = new Color(192, 176, 138);
        }
        int hits;
        public override bool? CanDamage()
        {
            if (hits < 6)
                return true;
            else
                return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];
            /*ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.TrueExcalibur, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(target.Center.X, target.Center.Y), 
                
            }, player.whoAmI);*/
            for (int i = 0; i < 15; i++)
            {
                var dust = Dust.NewDustDirect(target.Center, 0, 0, 218);
                dust.scale = 1.25f;
                dust.velocity *= 2f;
                dust.noGravity = true;
            }
            hits++;
        }
    }
    public class BloodAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = -0.6f;
            frontColor = Color.Red;
            middleColor = Color.Crimson;
            backColor = Color.DarkRed;
        }
        int hits;
        public override bool? CanDamage()
        {
            if (hits < 3)
                return true;
            else
                return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];
            for (int i = 0; i < 15; i++)
            {
                var dust = Dust.NewDustDirect(target.Center, 0, 0, 115);
                dust.scale = 1.25f;
                dust.velocity *= 2f;
                dust.noGravity = true;
            }
            hits++;
        }
    }
}