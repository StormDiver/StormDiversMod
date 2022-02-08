using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;


namespace StormDiversMod.Projectiles
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
        int shoottime = 0;
        int spawntime;
        bool floatup = true;
        int floattime;
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

            spawntime++;
            shoottime++;
            if (spawntime <= 3)
            {
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 62, 0f, 0f, 0, default, 1f);

                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                    Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }
            //Getting the npc to fire at
            if (spawntime >= 30)
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC target = Main.npc[i];
                    target.TargetClosest(true);

                    float distanceX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                    float distanceY = target.position.Y + (float)target.height * 0.5f - Projectile.Center.Y;

                    if (((distanceX >= -75f && distanceX <= 75f) && (distanceY >= 0f && distanceY <= 750f)) && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                    {
                        /*if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
                        {
                            int dust2 = Dust.NewDust(new Vector2(Projectile.position.X + 19, Projectile.Bottom.Y - 10), 20, 4, 0, 0, 3, 0, default, 1f);
                        }*/

                        if (Main.rand.Next(3) == 0)
                        {
                            Dust dust;
                            dust = Terraria.Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Bottom.Y - 5), 112, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
                            dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;

                            dust.noGravity = true;
                            dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                        }

                        float xpos = (Main.rand.NextFloat(-10, 10));

                        if (shoottime > 15)
                        {

                            Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X + xpos, Projectile.Bottom.Y - 10), new Vector2(xpos / 15, 8), ModContent.ProjectileType<MeteorSentryProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 12);

                            shoottime = 0;
                        }

                       
                    }

                  

                }
            }

            Projectile.ai[0] += 1f;

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
                floattime++;
                if (floattime <= 30)
                {
                    Projectile.velocity.Y -= 0.01f;

                }
                else
                {
                    Projectile.velocity.Y += 0.01f;

                }
                if (floattime >= 60) //Halfway through it slows down
                {
                    floatup = false;
                    floattime = 0;
                }
            }
            if (!floatup) //Floating downwards
            {
                floattime++;

                if (floattime <= 30)
                {
                    Projectile.velocity.Y += 0.01f;
                }
                else
                {
                    Projectile.velocity.Y -= 0.01f;

                }
                if (floattime >= 60) //Halfway through it slows down
                {
                    floatup = true;
                    floattime = 0;
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
