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
using StormDiversMod.Basefiles;
using StormDiversMod.Projectiles;
using Terraria.GameContent.Drawing;
using Steamworks;
using System.Collections.Generic;

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
        float linewidth = 5;
        public override void OnSpawn(IEntitySource source)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

            }, Main.myPlayer);
            /*for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 72, 0f, 0f, 0, default, 0.5f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }*/
            for (int i = 0; i < 1; i++)
            {
                playerpos = Main.player[i].Center;
            }
            projpos = Projectile.Center;
            projspeed = Projectile.velocity * 500;
            base.OnSpawn(source);
        }
        public override void AI()
        {
            if (linewidth > 0.1f)
            {
                linewidth -= 0.1f;
            }
            //Projectile.ai[0]
            //0 = Normal Shot
            //1 = No Dust telegraph
            //2 = From circle attack
            //3 = Faster Acceleration normal shot
            //4 = Horizontal
            //5 = Cross attack
            Projectile.ai[1]++;

            if (Projectile.ai[1] < 60 && Projectile.ai[0] != 3)
            {
                Projectile.velocity *= 1.04f;
            }
            if (Projectile.ai[1] < 25 && Projectile.ai[0] == 3)
            {
                Projectile.velocity *= 1.1f;
            }


            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            Projectile.spriteDirection = Projectile.direction;
            
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X * -0.2f, Projectile.velocity.Y * -0.2f, 0, default, 1f);
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
           
            
            if (Projectile.ai[0] == 4) //Dust warning for horizontal ones
            {
                /*if (Projectile.ai[1] == 1)
                {
                    if (Projectile.velocity.X > 0)
                    {
                        Dust.QuickDustLine(Projectile.Center, new Vector2(Projectile.Center.X + 900, Projectile.Center.Y), 35, Color.DeepPink); //centre to centre

                    }
                    else
                    {
                        Dust.QuickDustLine(Projectile.Center, new Vector2(Projectile.Center.X - 900, Projectile.Center.Y), 35, Color.DeepPink); //centre to centre

                    }
                }*/
            }
           
            if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.TheUltimateBoss>())) //remove all proejctiles if boss is dead
            {
               // Projectile.Kill();
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
            SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1.5f, Pitch = -0.5f, MaxInstances = -1 }, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<TheUltimateBossProj4>(), 0, 0, Projectile.owner);
            for (int i = 0; i < 30; i++) //Pink particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -10f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 72, perturbedSpeed.X, perturbedSpeed.Y);
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
                    if (Projectile.ai[0] is 0 or 3 or 5) //regular attack
                        Utils.DrawLine(Main.spriteBatch, new Vector2(projpos.X, projpos.Y), new Vector2(projpos.X + projspeed.X, projpos.Y + projspeed.Y), Color.DeepPink, Color.Transparent, linewidth);

                    else if (Projectile.ai[0] is 2) //circle attack
                        Utils.DrawLine(Main.spriteBatch, new Vector2(projpos.X, projpos.Y), new Vector2(playerpos.X, playerpos.Y), Color.DeepPink, Color.Transparent, linewidth);

                    else if (Projectile.ai[0] is 4) //horizonal attack
                    {
                        if (Projectile.velocity.X > 0)
                            Utils.DrawLine(Main.spriteBatch, new Vector2(projpos.X, projpos.Y), new Vector2(projpos.X + 1100, projpos.Y), Color.DeepPink, Color.Transparent, linewidth);
                        else
                            Utils.DrawLine(Main.spriteBatch, new Vector2(projpos.X, projpos.Y), new Vector2(projpos.X - 1100, projpos.Y), Color.DeepPink, Color.Transparent, linewidth);
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
    //____________________
    public class TheUltimateBossProj2 : ModProjectile
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
            if (Projectile.ai[0] == 2 && Projectile.timeLeft >= 210) //30 frames after starting to move, deal damaging for homing 
                return false;
            else
                return true;
        }
        Vector2 projpos;
        float linewidth = 5;

        public override void OnSpawn(IEntitySource source)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

            }, Main.myPlayer);
            /*for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 72, 0f, 0f, 0, default, 0.5f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }*/
            if (Projectile.ai[0] == 2) //Homing Attack
                Projectile.timeLeft = 300;

            projpos = Projectile.Center;

            base.OnSpawn(source);
        }
        double dist = 0; //Distance away from the projectile

        float projspeed = 2f;

        //For homing attack
        float speed;
        float inertia;
        float distanceToIdlePosition; //distance to player
        public override void AI()
        {
            if (linewidth > 0.1f)
            {
                linewidth -= 0.1f;
            }
            //Projectile.ai[0]
            //1 = Ring attack
            //4 = Rapid summon attack //unused
            //2 = Homing
            //3 = Vertical
            Projectile.ai[1]++;
            if (Projectile.ai[0] == 3) //Accelerate
            {
                if (Projectile.ai[1] < 60)
                {
                    Projectile.velocity *= 1.04f;
                }
            }
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

            if (Projectile.ai[0] == 1 || Projectile.ai[0] == 4) //Static then charge
            {
                if (Projectile.ai[1] < 50) 
                {
                    Projectile.rotation += 0.3f;
                }

                if (Projectile.ai[1] == 50)
                {

                    SoundEngine.PlaySound(SoundID.Item42, Projectile.position);
                    if (Projectile.ai[0] == 4)
                    {
                        if (Main.getGoodWorld)
                            projspeed = 4f;
                        else if (Main.masterMode)
                            projspeed = 3.6f;
                        else if (Main.expertMode && !Main.masterMode)
                            projspeed = 3.3f;
                        else
                            projspeed = 3f;
                    }
                    else
                    {
                        projspeed = 2f;

                    }
                    for (int i = 0; i < 1; i++)
                    {
                        Player player = Main.player[i];
                        //Dust.QuickDustLine(Projectile.Center, player.Center, 35, Color.DeepPink); //centre to centre
                        Vector2 velocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(player.Center.X + (player.velocity.X / 5), player.Center.Y + (player.velocity.Y / 5))) * -projspeed;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<TheUltimateBossProj>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 2, 10);
                        Projectile.Kill();
                        //Set the velocities to the shoot values
                        //Projectile.velocity.X = -velocity.X;
                        //Projectile.velocity.Y = -velocity.Y;
                    }
                }
            }
            if (Projectile.ai[0] == 2 && Projectile.timeLeft > 3) //For homing ones
            {
                Projectile.rotation += 0.2f;
                if (Projectile.timeLeft > 240)
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X * -0.2f, Projectile.velocity.Y * -0.2f, 0, default, 1.5f);
                    Main.dust[dust].noGravity = true;
                    Projectile.hide = true;
                }
                else
                    Projectile.hide = false;

                if (Projectile.timeLeft == 240)
                {
                    //Projectile.velocity.Y = -6;
                }

                if (Projectile.timeLeft >= 60 && Projectile.timeLeft <= 240)
                {
                    if (Main.getGoodWorld)
                    {
                        speed = 17;
                        inertia = 80;
                    }
                    else if (Main.masterMode)
                    {
                        speed = 16;
                        inertia = 80;
                    }
                    else if (Main.expertMode && !Main.masterMode)
                    {
                        speed = 15;
                        inertia = 80;
                    }
                    else
                    {
                        speed = 14;
                        inertia = 80;
                    }
                    //Player target = Main.player;
                    Vector2 idlePosition = Main.LocalPlayer.Center;
                    Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
                    distanceToIdlePosition = vectorToIdlePosition.Length();

                    //if (Collision.CanHit(Projectile.Center, 0, 0, Main.MouseWorld, 0, 0))
                    {
                        if (distanceToIdlePosition > 10f)
                        {
                            vectorToIdlePosition.Normalize();
                            vectorToIdlePosition *= speed;
                        }
                        Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                    }

                }
            }
            if (Projectile.ai[0] == 3) //For vertical ones
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X * -0.2f, Projectile.velocity.Y * -0.2f, 0, default, 1f);
                Main.dust[dust].noGravity = true;

                if (Projectile.ai[1] == 1)
                {
                    
                }
            }
            if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.TheUltimateBoss>())) //remove all proejctiles if boss is dead
            {
                //Projectile.Kill();
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
            if (Projectile.ai[0] != 1 && Projectile.ai[0] != 4) //Circle Attack
            {
                SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1.5f, Pitch = -0.5f, MaxInstances = -1 }, Projectile.Center);

                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<TheUltimateBossProj4>(), 0, 0, Projectile.owner);
            }
            for (int i = 0; i < 30; i++) //Pink particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -10f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 72, perturbedSpeed.X, perturbedSpeed.Y);
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
                    if (Projectile.ai[0] is 3)//vertical attack
                        Utils.DrawLine(Main.spriteBatch, new Vector2(projpos.X, projpos.Y), new Vector2(projpos.X, projpos.Y + 800), Color.DeepPink, Color.Transparent, linewidth);
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

    public class TheUltimateBossProj3 : ModProjectile
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
        Vector2 projspeed;
        Vector2 projpos;

        float linewidth = 3;
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 72, 0f, 0f, 0, default, 1f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 3;

            }
            if (Projectile.ai[2] == 0) //Burst has slower vlecoity, so increase line range
            projspeed = Projectile.velocity * 300;
            else
                projspeed = Projectile.velocity * 600;

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

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

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
                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 72, 0f, 0f, 0, default, 1f);
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
                    Utils.DrawLine(Main.spriteBatch, new Vector2(projpos.X, projpos.Y), new Vector2(projpos.X + projspeed.X, projpos.Y + projspeed.Y), Color.DeepPink, Color.Transparent, linewidth);
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
    public class TheUltimateBossProj4 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Explosion Extreme Pain");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = false;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            DrawOffsetX = 25;
            DrawOriginOffsetY = 25;
            Projectile.light = 0.9f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.Pink;
            color.A = 255;
            return color;
        }
    }
}