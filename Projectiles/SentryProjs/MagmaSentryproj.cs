﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework.Graphics;

namespace StormDiversMod.Projectiles.SentryProjs
{
    public class MagmaSentryProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magma Orb Sentry");
            Main.projFrames[Projectile.type] = 7;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 70;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -5;
            DrawOriginOffsetY = 2;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanDamage()
        {
            return false;
        }
     
        bool animate = false;
        NPC target;
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {          
            fallThrough = false;

            return true;
        }
        public override void AI()
        {
            if (Projectile.velocity.Y < 20)
            {
                Projectile.velocity.Y += 0.5f;
            }

            Projectile.ai[0]++; //spawntime
            if (Projectile.ai[0] <= 3)
                {
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X - 15, Projectile.position.Y), 30, Projectile.height, 6, 0f, 0f, 0, default, 1f);

                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                    Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }
            Projectile.rotation = 0;
           
            Main.player[Projectile.owner].UpdateMaxTurrets();
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }

            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 10, Projectile.Top.Y), 20, 20, 6, 0, 0, 130, default, 1.5f);

                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.5f;
            }        
          
            Projectile.ai[1]++;//shottitme
            //Getting the npc to fire at
            Player player = Main.player[Projectile.owner];

            for (int i = 0; i < 200; i++)
            {
                if (player.HasMinionAttackTargetNPC)
                {
                    target = Main.npc[player.MinionAttackTargetNPC];
                }
                else
                {
                    target = Main.npc[i];
                }
              
                if (Vector2.Distance(Projectile.Center, target.Center) <= 750f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.CanBeChasedBy() && target.type != NPCID.TargetDummy)
                {

                    if (Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                    {
                        target.TargetClosest(true);
                        float projspeed = 12;
                        Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y - 14)) * projspeed;

                        if (Projectile.ai[1] > 40)
                        {
                            animate = true;

                            for (int j = 0; j < 30; j++)
                            {
                                int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 10, Projectile.Top.Y), 20, 20, 6, 0, -2, 130, default, 1f);

                                //Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                                Main.dust[dust].velocity *= 2f;
                            }

                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Top.Y + 14), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<MagmaSentryProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                            SoundEngine.PlaySound(SoundID.Item45 with{Volume = 1f, Pitch = 0.5f}, Projectile.Center);
                            Projectile.ai[1] = 0;
                        }
                    }

                }
            }
            Projectile.ai[0] += 1f;


            if (animate)
            {
                AnimateProjectile();
            }
        }
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 3)
                {
                    Projectile.frame++;
                    //Projectile.frame %= 6; // Will reset to the first frame if you've gone through them all.
                    Projectile.frameCounter = 0;
                    
                }
                if (Projectile.frame == 7)
                {
                    
                    Projectile.frame = 0;
                    animate = false;
                }
            
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
       
        public override void OnKill(int timeLeft)
        {

            //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 45);
            for (int i = 0; i < 50; i++)
            {

                var dust2 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 15, Projectile.position.Y), 30, Projectile.height, 6);
                dust2.noGravity = true;
            }
        }
        public override void PostDraw(Color lightColor) //glowmask for animated
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/SentryProjs/MagmaSentryProj_Glow");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
    }
    //_____________________________________________________________________________________________________
    public class MagmaSentryProj2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magma Fire Orb");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {

            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 240;
            //aiType = ProjectileID.Meteor1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Main.rand.Next(1) == 0)
            {
            
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 0, new Color(255, 255, 255), .8f);
                dust.noGravity = true;
                dust.scale = 1.5f;

            }



            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            Projectile.damage = (Projectile.damage * 9) / 10;
            target.AddBuff(ModContent.BuffType<SuperBurnDebuff>(), 300);

        }
        int reflect = 3;
        public override bool OnTileCollide(Vector2 oldVelocity)

        {

            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();

            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 1f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 1f;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

               
                for (int i = 0; i < 25; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 127, 0f, 0f, 0, default, 2f);
                   
                    Main.dust[dustIndex].noGravity = true;
                }

            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool PreDraw(ref Color lightColor) //trail
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, 0);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                    color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
    }

}
