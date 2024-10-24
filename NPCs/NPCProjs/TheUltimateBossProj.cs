using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.DataStructures;
using StormDiversMod.Common;
using StormDiversMod.Projectiles;
using Terraria.GameContent.Drawing;
using Steamworks;
using System.Collections.Generic;
using StormDiversMod.NPCs.Boss;

namespace StormDiversMod.NPCs.NPCProjs
{
    public class TheUltimateBossProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Skull of Pain");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.light = 0f;
            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 300;
           
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 1;

            Projectile.timeLeft = 300;

            //DrawOriginOffsetY = -2;
        }
        public override bool? CanDamage()
        {  
                return true;
        }
        Vector2 playerpos; //set pos on spawn
        Vector2 projpos;
        Vector2 projspeed; //set speed on spawn
        float linewidth = 6;
        public override void OnSpawn(IEntitySource source)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

            }, Main.myPlayer);
            /*for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 115, 0f, 0f, 50, default, 0.5f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }*/
            for (int i = 0; i < 1; i++)
            {
                playerpos = Main.player[i].Center;
            }
            if (Projectile.ai[0] == 2)
                Projectile.timeLeft = 120;

            projpos = Projectile.Center;
            projspeed = Projectile.velocity * 650;
            base.OnSpawn(source);
        }
        public override void AI()
        {
            if (linewidth > 0.1f)
            {
                linewidth -= 0.1f;
            }
            //Projectile.ai[0]
            //0 = Normal Shot (Attacks 1, 3, 7, and 9)
            //1 = Horizontal (Attack 5) (different line telegraph)
            Projectile.ai[1]++;

            if (Projectile.ai[1] < 60)
            {
                Projectile.velocity *= 1.04f;
            }
          
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            Projectile.spriteDirection = Projectile.direction;
            
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 115, Projectile.velocity.X * -0.2f, Projectile.velocity.Y * -0.2f, 50, default, 1f);
                Main.dust[dust].noGravity = true;
            

            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;

                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 110;
                Projectile.height = 110;
                Projectile.Center = Projectile.position;

                Projectile.knockBack = 6;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1.5f, Pitch = -0.5f, MaxInstances = 1 }, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<TheUltimateBossProjExplode>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 0.5f;

            for (int i = 0; i < 50; i++) //Pink particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -1.5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 115, perturbedSpeed.X, perturbedSpeed.Y, 50);
                dust.noGravity = true;
                dust.scale = 1.5f;

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
            if (linewidth > 0.1f)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    if (Projectile.ai[0] is 0) //regular attack
                        Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(projpos.X + projspeed.X, projpos.Y + projspeed.Y), Color.Red, Color.Transparent, linewidth);

                    else if (Projectile.ai[0] is 1) //horizonal attack
                    {
                        if (Projectile.velocity.X > 0)
                            Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(projpos.X + 1500, projpos.Y), Color.Red, Color.Transparent, linewidth);
                        else
                            Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(projpos.X - 1500, projpos.Y), Color.Red, Color.Transparent, linewidth);
                    }
                }
            }
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
    }

    //_____________________________________________________________________________________
    public class TheUltimateBossProjGravity : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heavy Skull of Pain");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.light = 0f;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.aiStyle = 1;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 300;
           
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 1;

            Projectile.timeLeft = 300;

            //DrawOriginOffsetY = -2;
        }
        public override bool? CanDamage()
        {
                return true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

            }, Main.myPlayer);

            base.OnSpawn(source);
        }
        public override void AI()
        {
            Projectile.ai[1]++;


            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;

                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 110;
                Projectile.height = 110;
                Projectile.Center = Projectile.position;

                Projectile.knockBack = 6;
            }

            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 115, Projectile.velocity.X * -0.2f, Projectile.velocity.Y * -0.2f, 50, default, 1f);
            Main.dust[dust].noGravity = true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1.5f, Pitch = -0.5f, MaxInstances = -1 }, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<TheUltimateBossProjExplode>(), 0, 0, Projectile.owner);

            for (int i = 0; i < 30; i++) //Pink particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -10f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 115, perturbedSpeed.X, perturbedSpeed.Y, 50);
                dust.noGravity = true;
                dust.scale = 1.5f;

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
            if (Projectile.velocity.Y > 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Vector2 velocity = Projectile.velocity * 35;

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(0);
                    Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Projectile.Center.X - 1 + (perturbedSpeed.X), Projectile.Center.Y - 3 + (perturbedSpeed.Y)), Color.Red, Color.Transparent, 5);
                }

            }
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
    }
    //____________________________________________________________________________________________
    public class TheUltimateBossProjCharge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Charging Skull of Pain");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.light = 0f;
            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 210;

            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 1;
            //DrawOriginOffsetY = -2;
        }
        public override bool? CanDamage()
        {
            if (Projectile.ai[1] < 90)
                return false;
            else
                return true;
        }
        Vector2 projspeed;
        Vector2 playerpos; //set pos on spawn
        Vector2 projpos;
        public override void OnSpawn(IEntitySource source)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

            }, Main.myPlayer);
            base.OnSpawn(source);
        }
        float projshootspeed;
        float linewidth = 6;
        int chargetime = 90;
        public override void AI()
        {
            Projectile.ai[1]++;

            //Projectile.spriteDirection = Projectile.direction;

            if (Projectile.ai[1] < chargetime)
            {
                if (Main.LocalPlayer.Center.X < Projectile.Center.X)
                {
                    Projectile.spriteDirection = -1;
                    Projectile.rotation = ((Main.LocalPlayer.Center - Projectile.Center) / 360).ToRotation() + MathHelper.ToRadians(180);//Look at the player
                }
                else
                {
                    Projectile.spriteDirection = 1;
                    Projectile.rotation = ((Main.LocalPlayer.Center - Projectile.Center) / 360).ToRotation() + MathHelper.ToRadians(0);//Look at the player

                }
            }
            for (int i = 0; i < 1; i++)
            {
                Player player = Main.player[i];

                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y));
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                if (Main.rand.Next(2) == 0)
                {
                    Dust dust;
                    dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 115, new Vector2(perturbedSpeed.X * -5, perturbedSpeed.Y * -5), 0, new Color(255, 255, 255), 1.25f);
                    dust.noGravity = true;
                }
            }
            if (Main.getGoodWorld && Main.masterMode)
                projshootspeed = 1.4f;
            else if (Main.masterMode)
                projshootspeed = 1.3f;
            else if (Main.expertMode && !Main.masterMode)
                projshootspeed = 1.2f;
            else
                projshootspeed = 1.1f;
            if (Projectile.ai[1] == chargetime)
            {
                SoundEngine.PlaySound(SoundID.Item42, Projectile.position);

                for (int i = 0; i < 1; i++)
                {
                    Player player = Main.player[i];
                    Vector2 velocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(player.Center.X + (player.velocity.X / 5), player.Center.Y + (player.velocity.Y / 5))) * -projshootspeed;
                    //Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<TheUltimateBossProj>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 2, 0);
                    //Projectile.Kill();
                    //Set the velocities to the shoot values
                    Projectile.velocity.X = velocity.X;
                    Projectile.velocity.Y = velocity.Y;
                }

                projspeed = Projectile.velocity * 950;
                for (int i = 0; i < 1; i++)
                {
                    playerpos = Main.player[i].Center;
                }
                projpos = Projectile.Center;
            }
            if (Projectile.ai[1] > chargetime && Projectile.ai[1] <= chargetime + 60)
            {
                Projectile.velocity *= 1.04f;
            }
            if (Projectile.ai[1] > chargetime)
            {
                if (linewidth > 0.1f)
                linewidth -= 0.1f;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1.5f, Pitch = -0.5f, MaxInstances = 1 }, Projectile.Center);

            for (int i = 0; i < 50; i++) //Pink particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -1.5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 115, perturbedSpeed.X, perturbedSpeed.Y, 50);
                dust.noGravity = true;
                dust.scale = 1.5f;
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
            if (linewidth > 0.1f && Projectile.ai[1] > chargetime)
            {
                Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(playerpos.X, playerpos.Y), Color.Red, Color.Transparent, linewidth);
            }
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
    }
    //____________________________________________________________________________________________
    public class TheUltimateBossProjHome : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Homing Skull of Pain");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.light = 0f;
            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 300;

            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 1;

            Projectile.timeLeft = 450;
            DrawOffsetX = -2;
            //DrawOriginOffsetY = -2;
        }
        public override bool? CanDamage()
        {
            if (Projectile.timeLeft >= 360 - 15) //15 frames after starting to move, deal damaging for homing 
                return false;
            else
                return true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

            }, Main.myPlayer);

            base.OnSpawn(source);
        }
        //For homing attack
        float speed;
        float inertia;
        float distanceToIdlePosition; //distance to player
        public override void AI()
        {
            Projectile.ai[1]++;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f + MathHelper.ToRadians(180);

            if (Projectile.timeLeft > 360)// appear for 1.5 seconds without moving
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 115, Projectile.velocity.X * -0.2f, Projectile.velocity.Y * -0.2f, 50, default, 1.5f);
                Main.dust[dust].noGravity = true;
                Projectile.hide = true;
            }
            else
                Projectile.hide = false;

            if (Projectile.timeLeft >= 180 && Projectile.timeLeft <= 360) //home in for 2 seconds, then move straight for 3
            {
                speed = 14f;
                inertia = 90;

                //Player target = Main.player;
                Vector2 idlePosition = Main.LocalPlayer.Center;
                Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
                distanceToIdlePosition = vectorToIdlePosition.Length();

                //if (Collision.CanHit(Projectile.Center, 0, 0, Main.MouseWorld, 0, 0))
                {
                    if (distanceToIdlePosition > 50f)
                    {
                        vectorToIdlePosition.Normalize();
                        vectorToIdlePosition *= speed;
                    }
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1.5f, Pitch = -0.5f, MaxInstances = 1 }, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<TheUltimateBossProjExplode>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 0.5f;

            for (int i = 0; i < 50; i++) //Pink particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -1.5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 115, perturbedSpeed.X, perturbedSpeed.Y, 50);
                dust.noGravity = true;
                dust.scale = 1.5f;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
    }
    //____________________________________________________________________
    public class TheUltimateBossProjShard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shard of Pain");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.light = 0f;
            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 1;

            Projectile.timeLeft = 450;

            //DrawOriginOffsetY = -2;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        Vector2 projspeed;
        Vector2 projpos;

        float linewidth = 3;
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 115, 0f, 0f, 50, default, 1f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 3;

            }
            if (Projectile.ai[2] == 0) //Burst has slower velocity, so increase line range
            projspeed = Projectile.velocity * 350;
            else
                projspeed = Projectile.velocity * 1000;

            projpos = Projectile.Center;

            base.OnSpawn(source);
        }
        public override void AI()
        {
            if (linewidth > 0.1f)
            {
                linewidth -= 0.1f;
            }
            Projectile.ai[1]++;
            if (Projectile.ai[1] < 50)
            {
                Projectile.velocity *= 1.04f;
            }

            //Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.rotation += MathHelper.ToRadians(20 * Projectile.direction);
            if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.TheUltimateBoss>())) //remove all proejctiles if boss is dead
            {
                //Projectile.Kill();
            }

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 115, 0f, 0f, 50, default, 1f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 1.5f, Pitch = 0.5f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Projectile.Center);
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
            if (linewidth > 0.1f)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Utils.DrawLine(Main.spriteBatch, new Vector2(projpos.X, projpos.Y), new Vector2(projpos.X + projspeed.X, projpos.Y + projspeed.Y), Color.Red, Color.Transparent, linewidth);
                }
            }
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            /*for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }*/

            return true;
        }
    }
    //____________________________________________________________
    public class TheUltimateBossProjLarge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Giant Skull of Pain");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.light = 0f;
            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 9999;

            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 1;

            //DrawOriginOffsetY = -2;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        NPC target; //target boss?

        float linewidth = 10;
        public override void OnSpawn(IEntitySource source)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

            }, Main.myPlayer);
           
            base.OnSpawn(source);

            if (NPC.CountNPCS(ModContent.NPCType<TheUltimateBoss>()) > 0)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].type == ModContent.NPCType<TheUltimateBoss>() && Main.npc[i].active)
                    {
                        target = Main.npc[i];
                    }
                }
            }
            else target = null;
        }
        float dashvelocity;
        int dashtime;
        int dashcount;
        int dashlimit;
        public override void AI()
        {
            if (Main.getGoodWorld && Main.masterMode)
            {
                dashtime = 0; // 90 total = 900 > 450 frames (7.5 seconds)
                dashlimit = 10;
            }
            else if (Main.masterMode)
            {
                dashtime = 10; //100 total = 900 > 450 frames (7.5 seconds)
                dashlimit = 9;
            }
            else if (Main.expertMode && !Main.masterMode)
            {
                dashtime = 20; // 110 total = 880 > 440 frames (7.33 seconds)
                dashlimit = 8;
            }
            else
            {
                dashtime = 30; // 120 total = 840 > 420 frames (7 seconds)
                dashlimit = 7;
            }

            if (linewidth > 0.1f)
            {
                linewidth -= 0.2f;
            }
            Projectile.ai[2]++;
            if (Projectile.ai[2] >= (90 + dashtime) && (dashcount < dashlimit))
            {
                Projectile.ai[1]++;
                if (Projectile.ai[1] == 1) //start dash
                {
                    linewidth = 10;
                    for (int i = 0; i < 1; i++)
                    {
                        Player target = Main.player[i];
                        SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack with { Volume = 2f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Projectile.Center);

                        if (Vector2.Distance(target.Center, Projectile.Center) < 2000f && target.active)
                        {
                            Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * 4;

                            Projectile.velocity.X = velocity.X;
                            Projectile.velocity.Y = velocity.Y;
                        }
                    }
                }
                if (Projectile.ai[1] >= 2 && Projectile.ai[1] <= 30) //accelerate
                {
                    Projectile.velocity *= 1.055f;

                }
                if (Projectile.ai[1] >= 40) //slowdown
                {
                    Projectile.velocity *= 0.96f;
                }
                if (Projectile.ai[1] > 90 + dashtime) //start new dash
                {
                    Projectile.ai[1] = 0;
                    dashcount ++; // limit of 8 dashes
                }
            }
            else 
            {
                Projectile.velocity *= 0.95f;
            }

            if (dashcount >= dashlimit)
            {
                Projectile.Kill();
            }
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.spriteDirection = Projectile.direction;

            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 115, Projectile.velocity.X * -0.2f, Projectile.velocity.Y * -0.2f, 50, default, 1f);
            Main.dust[dust].noGravity = true;

            //if boss moves to next phase kill projectile :)
            if (target == null)
                return;
            if (target != null)
            {
                if (target.ai[3] == 0)
                    Projectile.Kill();
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
        }
        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1.5f, Pitch = -0.5f, MaxInstances = -1 }, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<TheUltimateBossProjExplode>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1.25f;
            for (int i = 0; i < 30; i++) //Pink particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -10f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 115, perturbedSpeed.X, perturbedSpeed.Y, 50);
                dust.noGravity = true;
                dust.scale = 1.5f;
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
            if (linewidth > 0.1f)
            {
                Vector2 velocity = Projectile.velocity * 35;

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(0);
                Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Projectile.Center.X - 1 + (perturbedSpeed.X), Projectile.Center.Y - 3 + (perturbedSpeed.Y)), Color.Red, Color.Transparent, 5);
            }

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
    }

    //____________________
    public class TheUltimateBossProjExplode : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Explosion Extreme Pain");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = false;
            Projectile.timeLeft = 99;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = -49;
            DrawOriginOffsetY = -49;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
                Projectile.Kill();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.IndianRed;
            color.A = 255;
            return color;
        }
    }
}