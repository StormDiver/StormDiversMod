using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;

using Terraria.Utilities;

namespace StormDiversMod.Projectiles.SentryProjs
{
 
    public class StormSentryProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scandrone Sentry");
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
      
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.aiStyle = -1;
            DrawOffsetX = -3;

            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanDamage()
        {

            return false;
        }
        bool animate; //Animate every shot
        NPC target;
        NPC currenttarget = null; //Currently targetted enemy. uses so sentry looks at the correct one
        bool floatup = true;
        
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            player.UpdateMaxTurrets();

            Projectile.ai[0]++;//spawntime
            if (Projectile.ai[0] == 1)
            {
                for (int i = 0; i < 25; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 229, 0f, 0f, 0, default, 1.5f);
                    Main.dust[dustIndex].velocity *= 2;

                    Main.dust[dustIndex].noGravity = true;
                }
                Projectile.rotation = 1.57f;//Face down when first spawned

            }

    
            if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 24, Projectile.Center.Y - 24), 48, 48, 229, 0, 0, 130, default, 1f);

                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.5f;
            }
        
            Projectile.ai[1]++;//Shoottime
            
            

            //Getting the npc to fire at
            for (int i = 0; i < Main.maxNPCs; i++)
            {

                if (player.HasMinionAttackTargetNPC)
                {
                    target = Main.npc[player.MinionAttackTargetNPC];
                    Projectile.rotation = ((target.Center - Projectile.Center) / 360).ToRotation();//Look at the enemy

                }
                else
                {
                    target = Main.npc[i];

                }
                target.TargetClosest(true);
           
                if (Vector2.Distance(Projectile.Center, target.Center) <= 900f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.CanBeChasedBy() && target.type != NPCID.TargetDummy && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                {
                   
                    if (Projectile.ai[1] > 45)
                    {
                        currenttarget = target;
                        animate = true;
                        float projspeed = 18;
                        Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                      
                        for (int j = 0; j < 60; j++)
                        {
                            float speedY = -3.5f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 110, dustspeed.X, dustspeed.Y, 229, default, 1f);
                            Main.dust[dust2].noGravity = true;
                        }
                        for (int k = 0; k < 10; k++)
                        {
                            Dust dust;

                            dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 111, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 2f);
                            dust.noGravity = true;

                        }

                        SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
                        float numberProjectiles = 3;
                        float rotation = MathHelper.ToRadians(4);
                        for (int l = 0; l < numberProjectiles; l++)
                        {

                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, l / (numberProjectiles - 1)));
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                                ModContent.ProjectileType<StormSentryProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        }
                       
                        Projectile.ai[1] = 0;
                    }
                    if (currenttarget != null)
                    {
                        Projectile.rotation = ((currenttarget.Center - Projectile.Center) / 360).ToRotation();//Look at the enemy
                    }                 
                }            
            }
            if (animate)
            {
                AnimateProjectile();
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
        public void AnimateProjectile() 
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;

            }
            if (Projectile.frame == 5)
            {

                Projectile.frame = 0;
                animate = false;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
       
        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
            for (int i = 0; i < 50; i++)
            {

                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 229, 0f, 0f, 0, default, 1.5f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
     
    }
    //_____________________________________________ Proj
    public class StormSentryProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Sentry");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {

            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
      
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

          
            if (Projectile.ai[0] > 0f)  //this defines where the flames starts
            {
                if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
                {


                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 110, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= -0.3f;
   

                }
            }
            else
            {
                Projectile.ai[0] += 1f;
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
            for (int i = 0; i < 15; i++)
            {
                float speedY = -2f;

                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 110, dustspeed.X, dustspeed.Y);
                Main.dust[dust2].noGravity = true;
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
            return Color.White;
        }
    }
}
