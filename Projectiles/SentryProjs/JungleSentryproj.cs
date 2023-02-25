using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles.SentryProjs
{
   
    public class JungleSentryProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jungle Sentry");
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
      
            Projectile.width = 15;
            Projectile.height = 66;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -20;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.aiStyle = -1;
            DrawOriginOffsetY = 2;

        }
        public override bool? CanDamage()
        {
            return false;
        }
      
        bool animate = false;
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

            Projectile.ai[0]++;//spawntime
            if (Projectile.ai[0] <= 3)
                {
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X - 20, Projectile.position.Y), 52, Projectile.height, 40, 0f, 0f, 0, default, 1f);

                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                    Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }
            Projectile.rotation = 0;
            //Projectile.velocity.Y = 5;
           
            //Projectile.rotation += (float)Projectile.direction * -0.1f;
            Main.player[Projectile.owner].UpdateMaxTurrets();

            {
                if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X - 20, Projectile.position.Y), 52, Projectile.height, 40, 0, 0, 130, default, 1f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X - 20, Projectile.position.Y), 52, Projectile.height, 78, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
                }
            }

           
            Projectile.ai[1]++; //shoottime
            //Getting the npc to fire at
            for (int i = 0; i < 200; i++)
            {

                NPC target = Main.npc[i];
          
                if (Vector2.Distance(Projectile.Center, target.Center) <= 300f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.CanBeChasedBy() && target.type != NPCID.TargetDummy)
                {

                    if (Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                    {
                        if (Projectile.ai[1] > 100)
                        {
                            animate = true;

                        }
                        if (Projectile.ai[1] > 120)
                        {
                            for (int j = 0; j < 50; j++)
                            {
                                int dust = Dust.NewDust(new Vector2(Projectile.position.X - 20, Projectile.position.Y), 52, Projectile.height, 40, 0, -2, 130, default, 1f);

                                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                                Main.dust[dust].velocity *= 0.5f;
                            }
                            float numberProjectiles = 6;
                            float rotation = MathHelper.ToRadians(40);
                            //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                            for (int j = 0; j < numberProjectiles; j++)
                            {
                                float speedX = 0f;
                                float speedY = -6f;
                                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles) + 0.08f));
                                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<JungleSentryProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                            }

                            SoundEngine.PlaySound(SoundID.Item65, Projectile.Center);
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
                if (Projectile.frameCounter >= 6)
                {
                    Projectile.frame++;
                    //Projectile.frame %= 6; // Will reset to the first frame if you've gone through them all.
                    Projectile.frameCounter = 0;
                    
                }
                if (Projectile.frame == 6)
                {
                    
                    Projectile.frame = 0;
                    animate = false;
                }
            
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //target.AddBuff(mod.BuffType("AridSandDebuff"), 300);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            //target.AddBuff(mod.BuffType("AridSandDebuff"), 300);
        }
        public override void Kill(int timeLeft)
        {

            //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 45);
            for (int i = 0; i < 50; i++)
            {

                var dust2 = Dust.NewDustDirect(new Vector2(Projectile.position.X - 20, Projectile.position.Y), 52, Projectile.height, 40);
                dust2.noGravity = true;
            }
        }

    }
    //_____________________________________________________________________________________________________
    public class JungleSentryProj2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jungle Thorn ball");
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {

            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.aiStyle = 14;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Main.rand.Next(3) == 0)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 40);
                dust.noGravity = true;
                dust.scale = 0.8f;
            }
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {


        }
        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            
            return true;
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Grass, Projectile.position);

                for (int i = 0; i < 20; i++)
                {

                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 40);

                }
                for (int i = 0; i < 25; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 78, 0f, 0f, 0, default, 1f);
                   
                    Main.dust[dustIndex].noGravity = true;
                }

            }
        }
       
    }

}
