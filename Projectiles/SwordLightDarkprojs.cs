using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.GameContent.Drawing;
using Terraria.DataStructures;
using StormDiversMod.Common;
using ReLogic.Content;

using static Terraria.Main;
using System.Collections.Generic;

namespace StormDiversMod.Projectiles
{
 
    //___________________________________________________________________________________________
    public class SwordLightProj : ModProjectile //unused
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Light Essence");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.light = 0.3f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.alpha = 150;
            //Projectile.usesLocalNPCImmunity = true;
            //Projectile.localNPCHitCooldown = 10;
            //drawOffsetX = 2;
            //drawOriginOffsetY = -10;

        }
        int dusttime;
        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            dusttime++;
            if (dusttime == 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Projectile.Center.X + Main.rand.Next(5, 5), Projectile.Center.Y + Main.rand.Next(5, 5)),
                    }, player.whoAmI);
                }
            }
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            //Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);
            Projectile.spriteDirection = Projectile.direction;

            for (int i = 0; i < 5; i++)
            {
                float X = Projectile.Center.X - Projectile.velocity.X / 5f * (float)i;
                float Y = Projectile.Center.Y - Projectile.velocity.Y / 5f * (float)i;

                int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 264, 0, 0, 100, default, 1f);

                Main.dust[dust].position.X = X;
                Main.dust[dust].position.Y = Y;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.25f;
            }

            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 250;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
                {
                    if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                    {
                        Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            move = newMove;
                            distance = distanceTo;
                            target = true;
                        }
                    }
                }
            }
            if (target)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 11;
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.extraUpdates = 1;
            }
            else
                Projectile.extraUpdates = 0;

        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 12f)
            {
                vector *= 12f / magnitude;
            }

        }
    
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];
            for (int i = 0; i < 10; i++)
            {
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 264, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f, 130, default, 1f);
                dust2.noGravity = true;
            }
            for (int i = 0; i < 3; i++)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(target.Center.X + Main.rand.Next(-target.width / 3, target.width / 3), target.Center.Y + Main.rand.Next(-target.height / 3, target.height / 3)),

                }, player.whoAmI);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath7 with {Volume = 0.5f}, Projectile.Center);
            for (int i = 0; i < 25; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 264, 0, 0, 130, default, 1f);
                dust.noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.Lavender;
            color.A = 150;
            return color;
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
    }
    //___________________________________________________________________________________________
    public class SwordDarkProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dark Essence");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.light = 0f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 3;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            //drawOffsetX = 2;
            //drawOriginOffsetY = -10;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            //Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);
            Projectile.spriteDirection = Projectile.direction;
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 109, Projectile.velocity.X * -0.2f, Projectile.velocity.Y * -0.2f, 0, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
            Main.dust[dust].noGravity = true; //this make so the dust has no gravity

            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 125;
                Projectile.height = 125;
                Projectile.Center = Projectile.position;

                //Projectile.knockBack = 6;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            target.AddBuff(ModContent.BuffType<DarkShardDebuff>(), 180);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            return false;

        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath6 with{Volume = 1.5f, Pitch = -0.5f}, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionDarkProj>(), 0, 0, Projectile.owner);
            //Main.projectile[proj].scale = 1;

            for (int i = 0; i < 30; i++)
            {

                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 109, 0f, 0f, 0, default, 2f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 3;

            }
            var player = Main.player[Projectile.owner];

            for (int i = 0; i < 3; i++)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningHit, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

                }, player.whoAmI);
            }

        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft > 3)
            {
                return Color.White;
            }
            else
            {
                return null;
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
    }
    public class SwordLightProjspin : ModProjectile //used
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Light Essence");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.light = 0.3f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 200;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.alpha = 150;
            //Projectile.usesLocalNPCImmunity = true;
            //Projectile.localNPCHitCooldown = 10;
            //drawOffsetX = 2;
            //drawOriginOffsetY = -10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            var player = Main.player[Projectile.owner];
            /*for (int i = 0; i < 6; i++)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(Projectile.Center.X + Main.rand.Next(5, 5), Projectile.Center.Y + Main.rand.Next(5, 5)),
                }, player.whoAmI);
            }*/
        }
        Vector2 dustposition;
        double degrees;
        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            Projectile.ai[2]++;
            if (Projectile.ai[2] >= 2) //delay dust
            {
                degrees += 15 * Projectile.direction; //The degrees
                for (int i = 0; i < 2; i++)
                {
                    double rad = degrees * (Math.PI / 180); //Convert degrees to radians
                    double dist = 15; //Distance away from the player

                    dustposition.X = Projectile.Center.X - (int)(Math.Cos(rad) * dist);
                    dustposition.Y = Projectile.Center.Y - (int)(Math.Sin(rad) * dist);

                    for (int j = 0; j < 5; j++)
                    {
                        float X = dustposition.X - Projectile.velocity.X / 5f * (float)j;
                        float Y = dustposition.Y - Projectile.velocity.Y / 5f * (float)j;

                        int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 264, 0, 0, 100, default, 1.5f);

                        Main.dust[dust].position.X = X;
                        Main.dust[dust].position.Y = Y;
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 0.2f;
                    }

                    /*ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(dustposition.X + Main.rand.Next(5, 5), dustposition.Y + Main.rand.Next(5, 5)),
                    }, player.whoAmI);*/

                    degrees += 180; //for dust on other side
                }
            }

            Projectile.rotation += 0.1745f * Projectile.direction; //convert degrees to rad via converter (not needed, no visible sprite)

            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 250;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
                {
                    if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                    {
                        Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            move = newMove;
                            distance = distanceTo;
                            target = true;
                        }
                    }
                }
            }
            if (target)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 11;
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.extraUpdates = 1;
            }
            else
                Projectile.extraUpdates = 0;

        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 12f)
            {
                vector *= 12f / magnitude;
            }

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];
            for (int i = 0; i < 10; i++)
            {
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 264, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f, 130, default, 1f);
                dust2.noGravity = true;

            }
            for (int i = 0; i < 3; i++)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(target.Center.X + Main.rand.Next(-target.width / 3, target.width / 3), target.Center.Y + Main.rand.Next(-target.height / 3, target.height / 3)),

                }, player.whoAmI);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath7 with { Volume = 1f }, Projectile.Center);
            for (int i = 0; i < 50; i++) //Orange particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -3.5f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 264, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 2f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.Lavender;
            color.A = 150;
            return color;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            /*Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }*/
            return true;
        }
    }

    public class SwordLightDarkBlade : ModProjectile //sword effect
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Light Essence");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(158);

            Projectile.light = 0.3f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
           
            //Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            //Projectile.alpha = 150;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            //AI:
            //0: Direction,
            //1: Animation time,
            //2: light or dark
            var player = Main.player[Projectile.owner];

            if (Projectile.ai[0] == 1) //facing right
            {
                Projectile.rotation = MathHelper.ToRadians(25 - 135); //25 to 215 (remove 135 degrees due to sprite angle)
                if (player.gravDir == -1) //when upside down add 40 (fits better than 50) 
                    Projectile.rotation += MathHelper.ToRadians(40);
            }
            else //facing left
            {
                Projectile.rotation += MathHelper.ToRadians(155 - 45); //155 to -25 (remove 45 degrees due to sprite angle)
                if (player.gravDir == -1) //when upside down remove 40
                    Projectile.rotation -= MathHelper.ToRadians(40);
            }

            Projectile.ai[0] *= player.gravDir; //flip rotation direction if upside down

            Projectile.timeLeft = (int)Projectile.ai[1]; //Item useanimation is the time left

            Projectile.spriteDirection = player.direction * (int)player.gravDir; //anti grav needs sprite flipped

            //Main.NewText("Dust: " + MathHelper.ToDegrees(Projectile.rotation), 220, 63, 139);

            //Add player rotation to sprite rotation (e.g when in mounts)
            Projectile.rotation = Projectile.rotation + player.fullRotation;
        }
        float rotationspeed;
        public override void AI()
        {
            Projectile.localAI[0]++; //delay trail effect
            Projectile.frame = (int)Projectile.ai[2]; //frame 0 for light, 2 for dark

            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI; //appear in front of player but behind hand
            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter);
            //Projectile.position.X += player.width / 3 * player.direction;
            //Projectile.position.Y += player.height / 10;

            //angle of 190
            rotationspeed = 190 / Projectile.ai[1]; //190 swing, divide by the time to live (usetime) to get the speed

            if (Projectile.ai[0] == 1) //clockwise for right/ left-upsidedown
                Projectile.rotation += MathHelper.ToRadians(rotationspeed);
            else //Anti clockwise for left/ right-upsidedown
                Projectile.rotation -= MathHelper.ToRadians(rotationspeed);

            /*if (Projectile.timeLeft == 1)
                Main.NewText("Dust: " + MathHelper.ToDegrees(Projectile.rotation), 220, 63, 139);*/
        }
        public override void PostDraw(Color lightColor) //glowmask for animated
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/SwordLightDarkBlade_Glow");
            //Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
        public override bool PreDraw(ref Color lightColor) //trail
        {
            Main.instance.LoadProjectile(Projectile.type);

            //Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/SwordLightDarkBlade_Trail");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            if (Projectile.localAI[0] > 5) //otherwise visual glitch oocurs
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = (Projectile.position - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                        color, Projectile.oldRot[k], drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
           
            return null;
        }
    }
}