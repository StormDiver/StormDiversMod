using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{
    
    
   
    //______________________________________________________________________________________________________
    public class DesertStaffProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Sentry");
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
      
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            
        }
        public override bool? CanDamage()
        {

            return false;
        }
        int opacity = 255;
        int shoottime = 0;
        bool animate = false;

        bool floatup = true;
        int floattime;
        public override void AI()
        {
            if (opacity > 0)
            {
                opacity -= 10;
            }
            Projectile.alpha = opacity;
            //Projectile.rotation += (float)Projectile.direction * -0.1f;
            Main.player[Projectile.owner].UpdateMaxTurrets();
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors

            //if (Projectile.ai[0] > 20f)  //this defines where the flames starts
            {
                if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 10, 0, 10, 130, default, 1f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 54, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
                }
            }

            //else
            {
                //Projectile.ai[0] += 1f;
            }
            shoottime++;
            //Getting the npc to fire at
            for (int i = 0; i < 200; i++)
            {

                NPC target = Main.npc[i];

                //Getting the shooting trajectory
                float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y + (float)target.height * 0.5f - Projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                //bool lineOfSight = Collision.CanHitLine(Projectile.Center, 1, 1, target.Center, 1, 1);
                //If the distance between the projectile and the live target is active

                if (distance < 200f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
                {

                    if (Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                    {
                        if (shoottime > 80)
                        {
                            AnimateProjectile();


                            float numberProjectiles = 12;
                            float rotation = MathHelper.ToRadians(180);
                            //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                            for (int j = 0; j < numberProjectiles; j++)
                            {
                                float speedX = 0f;
                                float speedY = 3.5f;
                                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                                Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<DesertStaffProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                            }

                            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 45);
                            animate = true;
                            shoottime = 0;
                        }
                    }

                }
            }
            Projectile.ai[0] += 1f;


            if (animate)
            {
                AnimateProjectile();
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
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
                {
                    Projectile.frame++;
                    //Projectile.frame %= 6; // Will reset to the first frame if you've gone through them all.
                    Projectile.frameCounter = 0;
                    
                }
                if (Projectile.frame == 8)
                {
                    
                    Projectile.frame = 0;
                    animate = false;
                }
            
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }
        public override void Kill(int timeLeft)
        {

            //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 45);
            for (int i = 0; i < 50; i++)
            {

                Vector2 vel = new Vector2(Main.rand.NextFloat(20, 20), Main.rand.NextFloat(-20, -20));
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 54);
                dust2.noGravity = true;
            }
        }

    }
    //_____________________________________________________________________________________________________
    public class DesertStaffProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Sand");
        }
        public override void SetDefaults()
        {

            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors

            if (Projectile.ai[0] > 5f)  //this defines where the flames starts
            {
                if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 10, Projectile.Center.Y - 10), 20, 20, 10, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 10, 10, 54, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            return;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;

            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 180);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 180);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }

}
