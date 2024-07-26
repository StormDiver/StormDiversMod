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
            //DisplayName.SetDefault("Meteor Sentry");
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
            Projectile.DamageType = DamageClass.Summon;
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
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
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
            if (Projectile.ai[0] >= 10) //charge up dust first
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
                 
                    float distanceX = target.position.X + ((float)target.width * 0.5f) - Projectile.Center.X;
                    float distanceY = target.position.Y + ((float)target.height * 0.5f) - Projectile.Center.Y;

                    if (((distanceX >= -500f && distanceX <= 500f) && (distanceY >= 0f && distanceY <= 650f)) && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.CanBeChasedBy() && target.type != NPCID.TargetDummy && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
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

                        if (Projectile.ai[1] > 17) //actually fire
                        {
                            float projspeed = 16;
                            Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                          
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Bottom.Y - 10), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<MeteorSentryProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                            SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);

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
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item113 with{ Volume = 1f, Pitch = -0.5f}, Projectile.Center);

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
            //DisplayName.SetDefault("Meteor Sentry laser");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.light = 0.4f;
            Projectile.scale = 1f;

            Projectile.aiStyle = 0;
           
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ArmorPenetration = 5;
            Projectile.extraUpdates = 1;
        }
        int dusttime;
        int hometime;
        Vector2 newMove;

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
            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            /*Player player = Main.player[Projectile.owner];
            Vector2 move = Vector2.Zero;
            float distance = 200f;
            bool target = false;
            if (hometime <= 15)
            {
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
                    {
                        if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                        {
                            if (player.HasMinionAttackTargetNPC) //Make sure it homes into targetted enemy
                            {

                                newMove = Main.npc[player.MinionAttackTargetNPC].Center - Projectile.Center;
                            }
                            else
                            {
                                newMove = Main.npc[k].Center - Projectile.Center;
                            }
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
            }
            if (target && hometime <= 15)
            {
                hometime++;
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 9.5f;
                AdjustMagnitude(ref Projectile.velocity);
            }*/
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {

            /*float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 9f)
            {
                vector *= 9f / magnitude;
            }*/

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 62, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                dust.scale = 0.5f;
                dust.velocity *= 0.5f;

            }
            //Projectile.damage = Projectile.damage / 10 * 9;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 5; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 62, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
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
