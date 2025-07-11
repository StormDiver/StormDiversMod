﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Utilities;
using StormDiversMod.NPCs;
using Terraria.DataStructures;

namespace StormDiversMod.NPCs.NPCProjs
{
    public class StormBossBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Overloaded Scandrone Bolt");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            //Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            
            Projectile.light = 0.4f;
            
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 400;
            Projectile.penetrate = 4;

            
            Projectile.tileCollide = false;
            Projectile.scale = 1.2f;

            
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = -1;
            //aiType = ProjectileID.VortexBeaterRocket;
            
        }          
        
        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            {
                float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
                int j = Dust.NewDust(new Vector2(x2, y2), 1, 1, 229);
                //Main.dust[num165].alpha = alpha;
                Main.dust[j].position.X = x2;
                Main.dust[j].position.Y = y2;
                Main.dust[j].velocity *= 0f;
                Main.dust[j].noGravity = true;
                Main.dust[j].scale = 1.3f;
            }
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
           
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
           
        }

        public override void OnKill(int timeLeft)
        {

            Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
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
           
            return color;

        }
    }
    //__________________________
    public class StormBossMine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Overloaded Scandrone Mine");
           
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;

            Projectile.light = 0.4f;

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = -1;


            Projectile.tileCollide = false;
            Projectile.scale = 1.1f;

            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 300;
            //aiType = ProjectileID.LostSoulHostile;
            Projectile.aiStyle = -1;
            // Projectile.CloneDefaults(452);
            //aiType = 452;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }


        readonly int homerandom = Main.rand.Next(60, 180);
        public override void AI()
        {
           
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 229, 0, 0);
                dust.noGravity = true;
                dust.scale = 0.75f;
            
            Projectile.ai[0]++;

            Projectile.rotation = Projectile.ai[0] / 15;


            if (Projectile.ai[0] <= homerandom)
            {
                Projectile.velocity.X *= 0.95f;
                Projectile.velocity.Y *= 0.95f;
            }

            if (Projectile.ai[0] == homerandom)
            {
                for (int i = 0; i < 20; i++)
                {

                    int dust2 = Dust.NewDust(Projectile.Center - Projectile.velocity, Projectile.width, Projectile.height, 229, 0f, 0f, 50, default, 1f);
                    Main.dust[dust2].noGravity = true;

                }
            }
            if (Projectile.ai[0] == homerandom)
            {
                for (int i = 0; i < 10; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(0, -2f).RotatedByRandom(MathHelper.ToRadians(360));

                    var dust3 = Dust.NewDustDirect(Projectile.Center, 0, 0, 229, perturbedSpeed.X, perturbedSpeed.Y);
                    dust3.noGravity = true;

                    dust3.scale = 1f;

                }
                for (int i = 0; i < 200; i++)
                {
                    Player target = Main.player[i];
                   
                    if (Vector2.Distance(target.Center, Projectile.Center) < 2000f && target.active)
                    {
                        float projspeed = 6f;
                        Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;

                        Projectile.velocity.X = velocity.X;
                        Projectile.velocity.Y = velocity.Y;
                    }
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionVortexProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 0.7f;

            for (int i = 0; i < 20; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, -3f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 229, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;

                dust.scale = 1.5f;
                dust.fadeIn = 1.5f;

            }

            /*for (int i = 0; i < 20; i++)
                {
                    float speedY = -4f;

                    Vector2 perturbedSpeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y, 229, default, 1.5f);
                    Main.dust[dust2].noGravity = true;
                }*/
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
    //__________________________
    public class StormBossMineLarge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Overloaded Scandrone Large Mine");

        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;

            Projectile.light = 0.8f;

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = -1;


            Projectile.tileCollide = false;
            Projectile.scale = 1.1f;

            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 300;
            Projectile.aiStyle = -1;
         
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }


        readonly int homerandom = Main.rand.Next(45, 120);
        public override void AI()
        {
           
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 229, 0, 0);
                dust.noGravity = true;
                dust.scale = 0.75f;
            
            Projectile.ai[0]++;

            Projectile.rotation = Projectile.ai[0] / 15;


            if (Projectile.ai[0] <= homerandom)
            {
                Projectile.velocity.X *= 0.98f;
                Projectile.velocity.Y *= 0.98f;
            }

            if (Projectile.ai[0] == homerandom)
            {
                for (int i = 0; i < 20; i++)
                {

                    int dust2 = Dust.NewDust(Projectile.Center - Projectile.velocity, Projectile.width, Projectile.height, 229, 0f, 0f, 50, default, 1f);
                    Main.dust[dust2].noGravity = true;

                }
            }

            if (Projectile.ai[0] == homerandom)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(0, -2.5f).RotatedByRandom(MathHelper.ToRadians(360));

                    var dust3 = Dust.NewDustDirect(Projectile.Center, 0, 0, 229, perturbedSpeed.X, perturbedSpeed.Y);
                    dust3.noGravity = true;

                    dust3.scale = 1f;

                }
                for (int i = 0; i < 200; i++)
                {
                    Player target = Main.player[i];

                    if (Vector2.Distance(target.Center, Projectile.Center) < 2000f && target.active)
                    {
                        float projspeed = 7f;
                        Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;

                        Projectile.velocity.X = velocity.X;
                        Projectile.velocity.Y = velocity.Y;
                    }
                }
            }
            if (Projectile.timeLeft <= 3)
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                // Set to transparent. This Projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original Projectile center. This makes the Projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 150;
                Projectile.height = 150;
                Projectile.Center = Projectile.position;

                Projectile.knockBack = 6f;

            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionVortexProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1.5f;

           
            for (int i = 0; i < 30; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, -7f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 229, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;

                dust.scale = 1.5f;
                dust.fadeIn = 1.5f;

            }
            /*for (int i = 0; i < 30; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31);
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                    dust.velocity *= 4f;
                    dust.fadeIn = 1f;

                }*/
            

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.timeLeft > 3)
            {
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                }

                return true;
            }
        
            else
            {
                return false;
            }
        }
     

    }
    //__________________________________
    public class StormBossBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Overloaded Scandrone Bomb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.light = 0.1f;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = 14;

        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            Player player= Main.LocalPlayer;
            if (player.position.Y < Projectile.position.Y)
            fallThrough = false;
            else
            fallThrough = true;
            return true;
        }
        public override void AI()
        {
            Dust dust2;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position2 = Projectile.Center;
            dust2 = Terraria.Dust.NewDustPerfect(position2, 229, new Vector2(0f, 0f), 0, new Color(255, 100, 0), 1f);
            dust2.noGravity = true;

            Projectile.ai[0]++;
            if (Projectile.timeLeft <= 3)
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                // Set to transparent. This Projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original Projectile center. This makes the Projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;
                Projectile.width = 150;
                Projectile.height = 150;
                Projectile.Center = Projectile.position;
                Projectile.knockBack = 6f;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);


            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionVortexProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1.5f;
            for (int i = 0; i < 30; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, -7f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 229, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;

                dust.scale = 1.5f;
                dust.fadeIn = 1.5f;

            }
            /*for (int i = 0; i < 30; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31);
                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.velocity *= 4f;
                dust.fadeIn = 1f;

            }*/
        }   
    }
    //_____________________________
    public class StormBossLightning : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Overloaded Scandrone Lightning");
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = 180;
            Projectile.scale = 0.75f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 75;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.localAI[1] < 1f)
            {
                Projectile.localAI[1] += 2f;
                Projectile.position += Projectile.velocity;
                Projectile.velocity = Vector2.Zero;
            }
            return false;
        }

        public override bool? Colliding(Rectangle myRect, Rectangle targetRect)
        {
            for (int i = 0; i < Projectile.oldPos.Length && (Projectile.oldPos[i].X != 0f || Projectile.oldPos[i].Y != 0f); i++)
            {
                myRect.X = (int)Projectile.oldPos[i].X;
                myRect.Y = (int)Projectile.oldPos[i].Y;
                if (myRect.Intersects(targetRect))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!Main.dedServ)
            {
                Color color = Lighting.GetColor((int)((double)Projectile.position.X + (double)Projectile.width * 0.5) / 16, (int)(((double)Projectile.position.Y + (double)Projectile.height * 0.5) / 16.0));
                Vector2 end = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
                Projectile.GetAlpha(color);
                Vector2 vector = new Vector2(Projectile.scale) / 2f;
                for (int i = 0; i < 2; i++)
                {
                    float num = ((Projectile.localAI[1] == -1f || Projectile.localAI[1] == 1f) ? (-0.2f) : 0f);
                    if (i == 0)
                    {
                        vector = new Vector2(Projectile.scale) * (0.5f + num);
                        DelegateMethods.c_1 = new Color(34, 221, 151, 0) * 0.5f;
                    }
                    else
                    {
                        vector = new Vector2(Projectile.scale) * (0.3f + num);
                        DelegateMethods.c_1 = new Color(255, 255, 255, 0) * 0.5f;
                    }
                    DelegateMethods.f_1 = 1f;
                    for (int j = Projectile.oldPos.Length - 1; j > 0; j--)
                    {
                        if (!(Projectile.oldPos[j] == Vector2.Zero))
                        {
                            Vector2 start = Projectile.oldPos[j] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                            Vector2 end2 = Projectile.oldPos[j - 1] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                            Utils.DrawLaser(Main.spriteBatch, tex, start, end2, vector, DelegateMethods.LightningLaserDraw);
                        }
                    }
                    if (Projectile.oldPos[0] != Vector2.Zero)
                    {
                        DelegateMethods.f_1 = 1f;
                        Vector2 start2 = Projectile.oldPos[0] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                        Utils.DrawLaser(Main.spriteBatch, tex, start2, end, vector, DelegateMethods.LightningLaserDraw);
                    }
                }
            }
            return false;
        }

        public override void AI()
        {
            if (Projectile.scale > 0.05f)
            {
                Projectile.scale -= 0.005f;
            }
            else
            {
                Projectile.timeLeft = 0;
            }

            //Main.NewText("Tester " + Projectile.scale, 0, 204, 170); //Inital Scale

            /*if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = Projectile.velocity.ToRotation();
            }*/
            if (Projectile.localAI[1] == 0f && Projectile.ai[0] >= 900f)
            {
                Projectile.ai[0] -= 1000f;
                Projectile.localAI[1] = -1f;
            }
            int frameCounter = Projectile.frameCounter;
            Projectile.frameCounter = frameCounter + 1;
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, 0.3f, 0.45f, 0.5f);
            }
            if (Projectile.velocity == Vector2.Zero)
            {
                if (Projectile.frameCounter >= Projectile.extraUpdates * 2)
                {
                    Projectile.frameCounter = 0;
                    bool flag = true;
                    for (int i = 1; i < Projectile.oldPos.Length; i++)
                    {
                        if (Projectile.oldPos[i] != Projectile.oldPos[0])
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        Projectile.Kill();
                        return;
                    }
                }
                if (Main.rand.Next(Projectile.extraUpdates) == 0)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        float num = Projectile.rotation + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
                        float num2 = (float)Main.rand.NextDouble() * 0.8f + 1f;
                        Vector2 vector = new Vector2((float)Math.Cos(num) * num2, (float)Math.Sin(num) * num2);
                        int num3 = Dust.NewDust(Projectile.Center, 0, 0, 226, vector.X, vector.Y);
                        Main.dust[num3].noGravity = true;
                        Main.dust[num3].scale = 1.2f;
                    }
                    if (Main.rand.Next(5) == 0)
                    {
                        Vector2 vector2 = Projectile.velocity.RotatedBy(1.5707963705062866) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;
                        int num4 = Dust.NewDust(Projectile.Center + vector2 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
                        Dust dust = Main.dust[num4];
                        dust.velocity *= 0.5f;
                        Main.dust[num4].velocity.Y = 0f - Math.Abs(Main.dust[num4].velocity.Y);
                    }
                }
            }
            else
            {
                if (Projectile.frameCounter < Projectile.extraUpdates * 2)
                {
                    return;
                }
                Projectile.frameCounter = 0;
                float num5 = Projectile.velocity.Length();
                UnifiedRandom unifiedRandom = new UnifiedRandom((int)Projectile.ai[1]);
                int num6 = 0;
                Vector2 spinningpoint = -Vector2.UnitY;
                while (true)
                {
                    int num7 = unifiedRandom.Next();
                    Projectile.ai[1] = num7;
                    num7 %= 100;
                    float f = (float)num7 / 100f * ((float)Math.PI * 2f);
                    Vector2 vector3 = f.ToRotationVector2();
                    if (vector3.Y > 0f)
                    {
                        vector3.Y *= -1f;
                    }
                    bool flag2 = false;
                    if (vector3.Y > -0.02f)
                    {
                        flag2 = true;
                    }
                    if (vector3.X * (float)(Projectile.extraUpdates + 2) * 2f * num5 + Projectile.localAI[0] > 40f)
                    {
                        flag2 = true;
                    }
                    if (vector3.X * (float)(Projectile.extraUpdates + 2) * 2f * num5 + Projectile.localAI[0] < -40f)
                    {
                        flag2 = true;
                    }
                    if (flag2)
                    {
                        if (num6++ >= 100)
                        {
                            Projectile.velocity = Vector2.Zero;
                            /*if (Projectile.localAI[1] < 1f)
							{
								Projectile.localAI[1] += 2f;
							}*/
                            Projectile.localAI[1] = 1f;
                            break;
                        }
                        continue;
                    }
                    spinningpoint = vector3;
                    break;
                }
                if (Projectile.velocity != Vector2.Zero)
                {

                    Projectile.localAI[0] += spinningpoint.X * (float)(Projectile.extraUpdates + 1) * 2f * num5;
                    Projectile.velocity = spinningpoint.RotatedBy(Projectile.ai[0] + (float)Math.PI / 2f) * num5;
                    Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                }
                /*if (Main.rand.Next(4) == 0 && Main.netMode != 1 && Projectile.localAI[1] == 0f)
				{
					float num8 = (float)Main.rand.Next(-3, 4) * ((float)Math.PI / 3f) / 3f;
					Vector2 vector4 = Projectile.ai[0].ToRotationVector2().RotatedBy(num8) * Projectile.velocity.Length();
					if (!Collision.CanHitLine(Projectile.Center, 0, 0, Projectile.Center + vector4 * 80f, 0, 0))
					{
						Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X - vector4.X, Projectile.Center.Y - vector4.Y), new Vector2(vector4.X, vector4.Y), Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, vector4.ToRotation() + 1000f, Projectile.ai[1]);
					}
				}*/
            }
            /*{
                float num = base.Projectile.rotation + (float)Math.PI / 2f + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
                float num2 = (float)Main.rand.NextDouble() * 2f + 2;
                Vector2 vector = new Vector2((float)Math.Cos(num) * num2, (float)Math.Sin(num) * num2);
                int num3 = Dust.NewDust(base.Projectile.oldPos[base.Projectile.oldPos.Length - 1], 0, 0, 229, vector.X, vector.Y);
                Main.dust[num3].noGravity = true;
                Main.dust[num3].scale = 1.7f;
            }*/
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
           
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.rand.Next(50) == 0)
            {
                float num = Projectile.rotation + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
                float num2 = (float)Main.rand.NextDouble() * 0.8f + 1f;
                Vector2 vector = new Vector2((float)Math.Cos(num) * num2, (float)Math.Sin(num) * num2);
                int num3 = Dust.NewDust(Projectile.Center, 0, 0, 226, vector.X, vector.Y);
                Main.dust[num3].noGravity = true;
                Main.dust[num3].scale = 1.2f;
            }
        }
    }
    //__________________________________________________________________________________________________
    public class StormBossLightningPortal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Overloaded Scandrone Portal");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.light = 0.6f;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 300;
            Projectile.light = 1;
            Projectile.aiStyle = -1;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

        }
        public override bool? CanDamage() => false;

        float xPos;
        float yPos;
        Vector2 velocity;
        Vector2 rotation;
        Vector2 playerpos; //set pos pre fire

        Player player;

        int movespeed = 20;
        float linewidth = 5;
        public override void OnSpawn(IEntitySource source)
        {
            //set the target
            Projectile.scale = 0f;

            for (int i = 0; i < 1; i++)
            {
                player = Main.player[i];
                //player = Main.npc[NPCs.StormBoss].target;

                for (int j = 0; j < 50; j++)
                {
                    float speedY = -4f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 110, dustspeed.X, dustspeed.Y, 229, default, 1.5f);
                    Main.dust[dust2].noGravity = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float projspeed = 6;
                    velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;

                    xPos = Projectile.Center.X - player.Center.X;
                    yPos = Projectile.Center.Y - player.Center.Y;
                    Projectile.netUpdate = true;
                }
                rotation = -Projectile.Center + player.Center; //set the lightning rotation as well

            }
            if (Projectile.ai[1] == 1)
            {
                Projectile.ai[0] += 30; //Skip ahead when firing down
            }
        }
        public override void AI()
        {
            
            //projecile.ai[1], 0 for orbital portals, 1 for stright down portals
            Projectile.ai[0]++;

            if (Main.rand.Next(5) == 0)
            {

                var dust = Dust.NewDustDirect(Projectile.Center + new Vector2(-2, -2), 4, 4, 229, 0, 0);
                dust.noGravity = true;
                dust.scale = 1f;

            }
            if (Main.rand.Next(4) == 0)
            {

                float speedY = -2f;

                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 229, dustspeed.X, dustspeed.Y, 229, default, 1.2f);
                Main.dust[dust2].noGravity = true;

            }

            if (Projectile.ai[0] <= 60)//Increase opticaity and size
            {
                Projectile.alpha -= 5;
                if (Projectile.scale <= 1)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    { 
                        if (Projectile.ai[1] == 0)
                        {
                            Projectile.scale += 0.08f;
                        }
                        if (Projectile.ai[1] == 1)
                        {
                            Projectile.scale += 0.15f;
                        }
                        Projectile.netUpdate = true;

                    }

                }
            }
            
            if (Projectile.ai[0] < 60)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Projectile.ai[1] == 0) //Only move vertically if straight portals
                    {
                        movespeed = 20;
                    }
                   
                    Vector2 moveTo = player.Center;
                    Vector2 move = moveTo - Projectile.Center + new Vector2(xPos, yPos); //Postion around player
                    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                    if (magnitude > movespeed)
                    {
                        move *= movespeed / magnitude;
                    }
                    Projectile.velocity = move;
                    if (Projectile.ai[1] == 1) //Only move vertically if straight portals
                    {
                        movespeed = 40;
                        Projectile.velocity.X = 0;
                    }

                    Projectile.netUpdate = true;
                }

                for (int i = 0; i < 1; i++)
                {
                    playerpos = Main.player[i].Center;
                }
                //Projectile.position.X = (player.Center.X - (Projectile.width / 2)) + xPos;
                //Projectile.position.Y = (player.Center.Y - (Projectile.height / 2)) + yPos;
            }
            else
            {
                Projectile.velocity *= 0;
            }
            if (Projectile.ai[0] >= 60 && Projectile.ai[0] <= 72)
            {
                if (linewidth > 0.1f)
                {
                    linewidth -= 0.1f;
                }
                if (Main.rand.Next(3) == 0)
                {
                    if (Projectile.ai[1] == 0) //aim indicator
                    {
                        Dust dust;
                        dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 229, new Vector2(velocity.X * 2f + Projectile.velocity.X, velocity.Y * 2f + Projectile.velocity.Y), 0, new Color(255, 255, 255), 2f);
                        dust.noGravity = true;
                        dust.velocity *= 1.5f;
                    }
                    else if (Projectile.ai[1] == 1)
                    {
                        Dust dust;
                        dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 229, new Vector2(0, 5), 0, new Color(255, 255, 255), 2f);
                        dust.noGravity = true;
                        dust.velocity *= 1.5f;
                    }
                }
            }
            if (Projectile.ai[0] == 72) //Fire lightning
            {
                for (int j = 0; j < 30; j++)
                    {
                        float speedY = -3f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 229, dustspeed.X, dustspeed.Y, 229, default, 2f);
                        Main.dust[dust2].noGravity = true;
                    }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Projectile.ai[1] == 0)
                    {
                        float ai = Main.rand.Next(25);
                        int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(velocity.X * 1.15f, velocity.Y * 1.15f),
                        ModContent.ProjectileType<StormBossLightning>(), Projectile.damage, .5f, Main.myPlayer, rotation.ToRotation(), ai);
                        Main.projectile[projID].scale = 1f;
                        Main.projectile[projID].timeLeft = 190;

                        Main.projectile[projID].tileCollide = false;
                    }
                    else if (Projectile.ai[1] == 1)
                    {
                        Vector2 rotation2 = -Projectile.Center + (Projectile.Center + new Vector2(0, 250));
                        float ai = Main.rand.Next(25);
                        int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 6),
                        ModContent.ProjectileType<StormBossLightning>(), Projectile.damage, .5f, Main.myPlayer, rotation2.ToRotation(), ai);
                        Main.projectile[projID].scale = 0.9f;
                        Main.projectile[projID].timeLeft = 170;

                        Main.projectile[projID].tileCollide = false;
                    }
                }
                
                SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);

            }
            if (Projectile.ai[0] >= 82)//Fade out and kill
            {
                Projectile.alpha += 5;
                if (Projectile.scale >= 0)
                {
                    Projectile.scale -= 0.1f;
                }
                else
                {
                    
                    Projectile.Kill();
                }
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (linewidth > 0.1f)
            {
                if (Projectile.ai[0] >= 60 && Projectile.ai[0] <= 72)
                {
                    if (Projectile.ai[1] == 0) //aim indicator
                        Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X + 2.5f, Projectile.Center.Y + 2.5f), new Vector2(playerpos.X + 2.5f, playerpos.Y + 2.5f), Color.Turquoise, Color.Transparent, linewidth);
                    else if (Projectile.ai[1] == 1)
                        Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X + 2.5f, Projectile.Center.Y + 2.5f), new Vector2(Projectile.Center.X + 2.5f, Projectile.Center.Y + 500), Color.Turquoise, Color.Transparent, linewidth);

                }
            }
            return base.PreDraw(ref lightColor);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = (byte)Projectile.alpha;
            return color;
        }

        public override void OnKill(int timeLeft)
        {
            //Main.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);
        }
    }
}
