using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;


namespace StormDiversMod.Projectiles.SentryProjs
{

    //______________________________________________________________________________________________________
    public class MeteorSentryProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor Sentry");
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {

            Projectile.width = 58;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.light = 0.4f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
        }
        public override bool? CanDamage()
        {

            return false;
        }
        int opacity = 255;
        bool floatup = true;
        NPC target;

        public override void AI()
        {
            if (opacity > 0)
            {
                opacity -= 10;
            }
            Projectile.alpha = opacity;
            Main.player[Projectile.owner].UpdateMaxTurrets();
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors

            {
                if (Main.rand.Next(10) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X + 19 , Projectile.Bottom.Y - 10), 20, 4, 6, 0, 30, 0, default, 1f);

                    //Main.dust[dust].noGravity = true; //this make so the dust has no gravity


                }
            }

            Projectile.ai[0]++;//Spawntime
            Projectile.ai[1]++;//Shoottime
            if (Projectile.ai[0] <= 3)
            {
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 62, 0f, 0f, 0, default, 1f);

                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                    Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }

            Player player = Main.player[Projectile.owner];

            //Getting the npc to fire at
            if (Projectile.ai[0] >= 30)
            {
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
                    //Getting the shooting trajectory
                    float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                    float shootToY = target.position.Y + (float)target.height * 0.5f - Projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                    //bool lineOfSight = Collision.CanHitLine(Projectile.Center, 1, 1, target.Center, 1, 1);
                    //If the distance between the projectile and the live target is active

                    float distanceX = target.position.X + ((float)target.width * 0.5f) - Projectile.Center.X;
                    float distanceY = target.position.Y + ((float)target.height * 0.5f) - Projectile.Center.Y;

                    if (((distanceX >= -500f && distanceX <= 500f) && (distanceY >= 0f && distanceY <= 650f)) && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                    {
                        target.TargetClosest(true);

                        /*if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
                        {
                            int dust2 = Dust.NewDust(new Vector2(Projectile.position.X + 19, Projectile.Bottom.Y - 10), 20, 4, 0, 0, 3, 0, default, 1f);
                        }*/

                        if (Main.rand.Next(3) == 0)
                        {
                            Dust dust;
                            dust = Terraria.Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Bottom.Y - 5), 62, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
                            dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;

                            dust.noGravity = true;
                            dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                        }

                        //float xpos = (Main.rand.NextFloat(-10, 10));

                        if (Projectile.ai[1] > 60)
                        {

                            //Dividing the factor of 2f which is the desired velocity by distance
                            distance = 2f / distance;

                            //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                            shootToX *= distance * 10f;
                            shootToY *= distance * 10f;

                               Vector2 perturbedSpeed = new Vector2(shootToX, shootToY).RotatedByRandom(MathHelper.ToRadians(0));

                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Bottom.Y - 10), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<MeteorSentryProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 12);

                            Projectile.ai[1] = 0;
                        }

                       
                    }

                  

                }
            }


            //Animation
            { 
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 5)
                {
                    Projectile.frame++;

                    Projectile.frameCounter = 0;

                }
                if (Projectile.frame >= 6) //Once all the frames are exhausted it resets back to frame zero
                {
                    Projectile.frame = 0;

                }

            }


            if (floatup) //Floating upwards
            {
                Projectile.localAI[0]++; //Floattime
                if (Projectile.localAI[0] <= 30)
                {
                    Projectile.velocity.Y -= 0.01f;

                }
                else //Halfway through it slows down
                {
                    Projectile.velocity.Y += 0.01f;

                }
                if (Projectile.localAI[0] >= 60)
                {
                    floatup = false;
                    Projectile.localAI[0] = 0;
                }
            }
            if (!floatup) //Floating downwards
            {
                Projectile.localAI[0]++;

                if (Projectile.localAI[0] <= 30)
                {
                    Projectile.velocity.Y += 0.01f;
                }
                else //Halfway through it slows down
                {
                    Projectile.velocity.Y -= 0.01f;

                }
                if (Projectile.localAI[0] >= 60)
                {
                    floatup = true;
                    Projectile.localAI[0] = 0;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 113, 1, -0.5f);

            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 65, 0f, 0f, 0, default, 1f);

                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //___________________________________________________________________
    public class MeteorSentryProj2: ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor Sentry laser");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;

        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 300;
            Projectile.light = 0.4f;
            Projectile.scale = 1f;

            Projectile.aiStyle = 0;
           
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Summon;

        }
        int dusttime;

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;


            dusttime++;
            if (dusttime >= 5)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 62, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 0.8f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 5; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 62, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                dust.scale = 0.5f;
                dust.velocity *= 0.5f;

            }
            Projectile.damage = Projectile.damage / 10 * 9;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 5; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 62, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                    dust.scale = 0.5f;
                    dust.velocity *= 0.5f;

                }
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
            color.A = 150;
            return color;

        }

    }
   
}
