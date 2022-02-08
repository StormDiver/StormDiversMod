using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;

namespace StormDiversMod.Projectiles
{
    
    
   
    //______________________________________________________________________________________________________
    public class CultistSentryProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cultist Sentry Orb");
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
      
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanDamage()
        {

            return false;
        }
        int shoottime = 0;
        int rotate;
        NPC target;
        public override void AI()
        {
            if (Projectile.alpha == 255)
            {
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 121);

            }
            if (Projectile.alpha > 5)
            {
                Projectile.alpha -= 5;
                
            }
            if (Projectile.alpha == 10)
            {
                for (int i = 0; i < 25; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 226, 0f, 0f, 0, default, 1.5f);
                    Main.dust[dustIndex].velocity *= 3;

                    Main.dust[dustIndex].noGravity = true;
                }
            }

            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;
            rotate += 1;
            //Projectile.rotation = rotate * 0.05f;
            
           
            Main.player[Projectile.owner].UpdateMaxTurrets();
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors

            //if (Projectile.ai[0] > 20f)  //this defines where the flames starts
            {
                if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 24, Projectile.Center.Y - 24), 48, 48, 226, 0, 0, 130, default, 1f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                }
            }
            //else
            {
                //Projectile.ai[0] += 1f;
            }
            if (Projectile.alpha < 10)
            {
                shoottime++;
            }
            Player player = Main.player[Projectile.owner];
            //Getting the npc to fire at
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

                if (distance < 600f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
                {

                    if (Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                    {
                        target.TargetClosest(true);

                        
                        if (shoottime > 25)
                        {

                            //Dividing the factor of 2f which is the desired velocity by distance
                            distance = 1.6f / distance;

                            //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                            shootToX *= distance * 6f;
                            shootToY *= distance * 6f;

                            for (int j = 0; j < 30; j++)
                            {
                                int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 16, Projectile.Center.Y - 16), 32, 32, 226, 0, 0, 130, default, 0.5f);

                                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                                Main.dust[dust].velocity *= 2f;
                            }
                            for (int k = 0; k < 10; k++)
                            {
                                Dust dust;
                                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                                
                                dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 111, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 2.5f);
                                dust.noGravity = true;

                            }

                            Vector2 perturbedSpeed = new Vector2(shootToX, shootToY).RotatedByRandom(MathHelper.ToRadians(0));

                            //Projectile.NewProjectile(Projectile.Center.X, Projectile.Top.Y + 14, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("MagmaSentryProj2"), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f);
                            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 122);

                            

                            Vector2 rotation = -Projectile.Center + target.Center;
                            float ai = Main.rand.Next(100);
                            Vector2 speed = Vector2.Normalize(rotation) * 10;
                            int projID = Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ProjectileID.CultistBossLightningOrbArc, Projectile.damage, .5f, Projectile.owner, rotation.ToRotation(), ai);

                            Main.projectile[projID].hostile = false;
                            Main.projectile[projID].friendly = true;
                            Main.projectile[projID].penetrate = 10;
                            Main.projectile[projID].usesLocalNPCImmunity = true;
                            Main.projectile[projID].localNPCHitCooldown = -1;
                            Main.projectile[projID].scale = 0.75f;
                            Main.projectile[projID].timeLeft = 180;
                            Main.projectile[projID].DamageType = DamageClass.Summon;



                            shoottime = 0;
                        }
                    }

                }
            }


            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;

            }
            if (Projectile.frame >= 3)
            {

                Projectile.frame = 0;
            }


        }
        

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
       
        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 122);
            for (int i = 0; i < 50; i++)
            {

                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 226, 0f, 0f, 0, default, 1.5f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.alpha <= 5)
            {
                Color color = Color.White;
                color.A = 150;
                return color;
            }
            else
            {
                return null;
            }
        }
    }
   

}
