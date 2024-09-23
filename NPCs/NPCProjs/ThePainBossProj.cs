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

namespace StormDiversMod.NPCs.NPCProjs
{
    public class ThePainBossProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Very Painful Projectile");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.light = 0f;
            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 300;
            //aiType = ProjectileID.Bullet;
            //Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 1;

            Projectile.timeLeft = 300;

            Projectile.ArmorPenetration = 999;
            //drawOffsetX = 2;
            //drawOriginOffsetY = -10;
        }
        public override bool? CanDamage()
        {
            if (Projectile.ai[0] == 2 && Projectile.timeLeft >= 3)
                return false;
            else
                return true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 72, 0f, 0f, 0, default, 1.5f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
            if (Projectile.ai[0] == 2) //Stationary Attack
                Projectile.timeLeft = 90;

            base.OnSpawn(source);
        }
        public override void AI()
        {
            //Projectile.ai[0]
            //0 = Normal Shot
            //1 = Ring attack
            //2 = Stationary
            //3 = Vertical
            //4 = Horizontal
            //5 = Cross attack
            Projectile.ai[1]++;
            Projectile.rotation = Projectile.ai[1] / 5;
            if (Projectile.ai[0] != 1) //Accelerate
            {
                if (Projectile.ai[1] < 50)
                {
                    Projectile.velocity *= 1.04f;
                }
            }
            if (Projectile.velocity.X != 0 && Projectile.velocity.Y != 0) //no dust when stationary 
            {
                //Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X * -0.2f, Projectile.velocity.Y * -0.2f, 0, default, 1f);
                Main.dust[dust].noGravity = true;
            }

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

            if (Projectile.ai[0] == 1) //Circle Attack
            {
                if (Projectile.ai[1] < 50) //dust warning
                {
                    for (int i = 0; i < 1; i++)
                    {
                        Player player = Main.player[i];

                        Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y));
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                        Dust dust;
                        dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 72, new Vector2(perturbedSpeed.X * 15, perturbedSpeed.Y * 15), 0, new Color(255, 255, 255), 1f);
                        dust.noGravity = true;
                    }
                }

                if (Projectile.ai[1] == 50)
                {
                    SoundEngine.PlaySound(SoundID.Item42, Projectile.position);
                    for (int i = 0; i < 1; i++)
                    {
                        Player player = Main.player[i];

                        float projspeed = 4f;
                        Vector2 velocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(player.Center.X, player.Center.Y)) * projspeed;
                        //Set the velocities to the shoot values
                        Projectile.velocity.X = -velocity.X;
                        Projectile.velocity.Y = -velocity.Y;
                    }
                }
                if (Projectile.ai[1] >= 45 && Projectile.ai[1] <= 75)
                {

                    Projectile.velocity *= 1.04f;

                }
            }
            if (Projectile.ai[0] == 2 && Projectile.timeLeft > 3) //Dust warning for stationary ones
            {
                for (int i = 0; i < 5; i++)
                {
                    double deg = Main.rand.Next(0, 360); //The degrees
                    double rad = deg * (Math.PI / 180); //Convert degrees to radians
                    double dist = 80; //Distance away from the cursor
                    float dustx = Projectile.Center.X - (int)(Math.Cos(rad) * dist);
                    float dusty = Projectile.Center.Y - (int)(Math.Sin(rad) * dist);

                    var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 72, 0, 0);
                    dust.noGravity = true;
                    dust.velocity *= 0;
                    dust.scale = 1f;

                }
            }
            if (Projectile.ai[0] == 3) //Dust warning for vertical ones
            {
                if (Projectile.ai[1] < 5)
                {
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 400, 72, 0f, 0f, 0, default, 1f);
                            Main.dust[dust].noGravity = true;
                            Main.dust[dust].velocity *= 0;

                        }
                    }
                }
            }
            if (Projectile.ai[0] == 4) //Dust warning for horizontal ones
            {
                if (Projectile.ai[1] < 5)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        if (Projectile.velocity.X > 0)
                        {
                            int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 550, 0, 72, 0f, 0f, 0, default, 1f);
                            Main.dust[dust].noGravity = true;
                            Main.dust[dust].velocity *= 0;
                        }
                        else
                        {
                            int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 550, Projectile.Center.Y), 550, 0, 72, 0f, 0f, 0, default, 1f);
                            Main.dust[dust].noGravity = true;
                            Main.dust[dust].velocity *= 0;
                        }
                    }
                }
            }
            if (Projectile.ai[0] == 5) //Cross attack
            {
                Projectile.timeLeft--;
            }
            /*if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.ThePainBoss>())) //remove all proejctiles if boss is dead
            {
                Projectile.Kill();
            }*/

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            return false;

        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath6 with{Volume = 1.5f, Pitch = 0.5f}, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ThePainBossProj2>(), 0, 0, Projectile.owner);
            //Main.projectile[proj].scale = 1;

            /*for (int i = 0; i < 30; i++)
            {

                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 72, 0f, 0f, 0, default, 1.5f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 3;

            }*/
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
    public class ThePainBossProj2 : ModProjectile
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
            Projectile.timeLeft = 20;
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