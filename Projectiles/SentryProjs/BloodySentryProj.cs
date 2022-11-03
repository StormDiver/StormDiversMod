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
 
    public class BloodySentryProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Urchin Sentry");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {    
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.aiStyle = -1;
           
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanDamage()
        {

            return false;
        }
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
                    int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 115, 0f, 0f, 0, default, 1.5f);
                    Main.dust[dustIndex].velocity *= 2;

                    Main.dust[dustIndex].noGravity = true;
                }
                Projectile.rotation = 1.57f;//Face down when first spawned

            } 
            if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 24, 24, 115);

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
              
                //Getting the shooting trajectory
                float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y + (float)target.height * 0.5f - Projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                //bool lineOfSight = Collision.CanHitLine(Projectile.Center, 1, 1, target.Center, 1, 1);
                //If the distance between the projectile and the live target is active

                if (distance < 600f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                {
                    if (Projectile.ai[1] > 15)
                    {
                        currenttarget = target;

                        //Dividing the factor of 2f which is the desired velocity by distance
                        distance = 1.6f / distance;

                        //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                        shootToX *= distance * 5f;
                        shootToY *= distance * 5f;

                        for (int j = 0; j < 10; j++)
                        {


                            int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 115, shootToX, shootToY, 0, default, 1f);
                            Main.dust[dust2].noGravity = true;
                        }                   
                        SoundEngine.PlaySound(SoundID.NPCHit9, Projectile.position);

                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(shootToX, shootToY),
                            ModContent.ProjectileType<BloodySentryProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        Projectile.ai[1] = 0;
                    }
                    if (currenttarget != null)
                    {
                        Projectile.rotation = ((currenttarget.Center - Projectile.Center) / 360).ToRotation();//Look at the enemy
                    }
                   
                }
               

            }
                    
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;

            }
            if (Projectile.frame >= 4)
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

            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);
            for (int i = 0; i < 50; i++)
            {

                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 115, 0f, 0f, 0, default, 1.5f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
     
    }
    //_____________________________________________ Proj
    public class BloodySentryProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Urchin Sentry Blood");
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {

            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 75;
      
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = true;
            Projectile.ArmorPenetration = 6;
        }

        public override void AI()
        {
            if (Projectile.ai[1] > 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    float X = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                    float Y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;

                    int dust = Dust.NewDust(new Vector2(X, Y), 0, 0, 115, 0, 0, 100, default, 1f);
                    Main.dust[dust].position.X = X;
                    Main.dust[dust].position.Y = Y;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;
                }
                
            }
            else
            {
                Projectile.ai[1]++;
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
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 115, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }
        }
      
       
    }
}
