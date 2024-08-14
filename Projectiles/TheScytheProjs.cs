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
    public class TheScytheProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("One Shot Scythe Spinner");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 9999999;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item71 with { Volume = 1f, Pitch = 0.5f }, Projectile.Center);

        }
        bool parry;
        int parrytime = 90;
        int parrycooldown = 90;
        public override void AI()
        {           
            Player player = Main.player[Projectile.owner];
            // 25 frame parry, 90 frame cooldown
            if (parrytime > 0 && player.releaseUseTile) //don't count down if player is still holding rmb
                parrytime--;

            if (parrytime == 1) //sound when can parry again
            {
                SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing with { Volume = 1f, Pitch = 0.5f, MaxInstances = 1 }, Projectile.Center);
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(player.Center.X, player.Center.Y),
                }, player.whoAmI);
            }

            if (player.controlUseTile && !parry && parrytime <= 0) //add parry buff
            {
                player.AddBuff(ModContent.BuffType<ReflectedBuff>(), 25);
                parrytime = parrycooldown + 25;
                parry = true;
            }
            if (!player.HasBuff(ModContent.BuffType<ReflectedBuff>()))
                parry = false;

            if (parry)
                Projectile.scale = 1.1f;
            else
                Projectile.scale = 1;

            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                Projectile.soundDelay = 25;
            }

            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage); //update damage

            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.dead)
                {
                    Projectile.Kill();
                    player.ClearBuff(ModContent.BuffType<ReflectedBuff>());
                }
            }
            if (Projectile.owner == Main.myPlayer)
            {
                if (Main.MouseWorld.X >= player.Center.X)
                {
                    Projectile.velocity.X = 1;
                    player.direction = 1;
                }
                else if (Main.MouseWorld.X < player.Center.X)
                {
                    Projectile.velocity.X = -1;
                    player.direction = -1;

                }
            }
            Projectile.Center = player.MountedCenter;
            Projectile.position.X += player.width / 2 * player.direction;
            Projectile.position.Y += player.height / 10;

            Projectile.spriteDirection = player.direction;
            if (parry)
                Projectile.rotation += MathHelper.ToRadians(12.5f) * player.direction; //this is the projectile rotation/spinning speed
            else
                Projectile.rotation += MathHelper.ToRadians(8.5f) * player.direction; //this is the projectile rotation/spinning speed

            //Projectile.rotation += 0.15f * player.direction; //this is the projectile rotation/spinning speed

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = Projectile.rotation;

            //-------------------------------------------------------------Sound-------------------------------------------------------
            if (!parry)
                Projectile.soundDelay--;
            else
                Projectile.soundDelay -= 3;

            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item1 with { Volume = 2f, Pitch = -0.5f, MaxInstances = 5 }, Projectile.Center);
                Projectile.soundDelay = 30;
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
            hitbox.Width = 180;
            hitbox.Height = 180;
            hitbox.X -= (hitbox.Width - Projectile.width) / 2;
            hitbox.Y -= (hitbox.Height - Projectile.height) / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool PreDraw(ref Color lightColor)
        {        
            Main.instance.LoadProjectile(Projectile.type);

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/TheScytheProj_Trail");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
             for (int k = 0; k < Projectile.oldPos.Length; k++)
             {
                 Vector2 drawPos = (Projectile.position - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                 Color color = Projectile.GetAlpha(Color.Violet) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                 Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
             }

            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //_________________________________________________________________________________________________________________
    public class TheScytheProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("One Shot Scythe Thrown");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            Projectile.CloneDefaults(106);
            AIType = 106;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[2]++;
            //Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage); //update damage

            if (Projectile.position.X >= player.position.X)
            {
                Projectile.spriteDirection = 1;
            }
            else
            {
                Projectile.spriteDirection = -1;
            }                      
            Projectile.width = 34;
            Projectile.height = 34;
            DrawOffsetX = -12;
            DrawOriginOffsetY = -12;
            Projectile.light = 0;
            Projectile.penetrate = -1;
           
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
            hitbox.Width = 45;
            hitbox.Height = 45;
            hitbox.X -= (hitbox.Width - Projectile.width) / 2;
            hitbox.Y -= (hitbox.Height - Projectile.height) / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item71 with { Volume = 1f, Pitch = 0.5f }, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 27);
                dust.scale = 1.5f;
                dust.noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 27);
                dust.scale = 1f;
                dust.noGravity = true;
            }
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            //Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/TheScytheProj2_Trail");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(-12f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.Violet) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}