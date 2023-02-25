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
    
    
   
    //______________________________________________________________________________________________________
    public class IceSentryProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Sentry");
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

        }
        public override void SetDefaults()
        {
      
            Projectile.width = 34;
            Projectile.height = 40;
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
        //bool animate1 = false; //For making the core open up
        //bool animate2 = false; //This bool is used to indicate that the core is open
        //bool animate3 = false; //For closing the core
        bool floatup = true;
        NPC target;
        public override void AI()
        {
            
            if (opacity > 0)
            {
                opacity -= 10;
            }
            Projectile.alpha = opacity;
            //Projectile.rotation += (float)Projectile.direction * -0.1f;
            Main.player[Projectile.owner].UpdateMaxTurrets();
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }

            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 187, 0, 10, 130, default, 1f);

                Main.dust[dust].noGravity = true; //this make so the dust has no gravity

                int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 187, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f);
            }
            

            Player player = Main.player[Projectile.owner];
            Projectile.ai[1]++; //shoottime
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

                if (Vector2.Distance(Projectile.Center, target.Center) <= 500f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.CanBeChasedBy() && target.type != NPCID.TargetDummy && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                {
                    float projspeed = 8;
                    Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;

                    if (Projectile.ai[1] > 12)
                    {

                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(8));
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<IceSentryProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);

                        Projectile.ai[1] = 0;
                    }

                    //localai[1] , 0 = core closes, 1 = Open core, 2 = Keep core open, 3= close core

                    if (Projectile.localAI[1] != 2)//If the core isn't already open it'll activate the opening bool
                    {
                        Projectile.localAI[1] = 1;
                    }
                }
                else
                {
                    if (Projectile.localAI[1] == 2) //If there are no nearby enemies and the core is open, it'll then activate the closing bool
                    {
                        Projectile.localAI[1] = 3; //Activates the closing bool
                        
                    }
                }
            }


            if (Projectile.localAI[1] == 1)//For opening the core
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 3) 
                {
                    Projectile.frame++;
                  
                    Projectile.frameCounter = 0;

                }
                if (Projectile.frame == 6) //Once it reaches this frame it stops counting up and activates the bool to say the core is open
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = 6;
                    Projectile.localAI[1] = 2; //While this is true it'll stop this bool from being reactivated
                }
            }
           
            if (Projectile.localAI[1] == 3) //This is the value for closing the core
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 3) 
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }           
            }
            if (Projectile.frame >= 8) //Once all the frames are exhausted it resets back to frame zero
            {
                Projectile.frame = 0;
                Projectile.localAI[1] = 0; //Core closed active
            }
            if (Projectile.localAI[1] == 0) //Core closed
            {              
                    Projectile.frame = 0;
             
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
        


        public override void Kill(int timeLeft)
        {

            //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 45);
            for (int i = 0; i < 50; i++)
            {

                 
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 187);
                dust2.noGravity = true;
            }
        }

    }
    //_____________________________________________________________________________________________________
    public class IceSentryProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Stream");
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 75;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors

            }
            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 187, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f, 130, default, 2f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 2.5f;
                int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 187, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f); //this defines the flames dust and color parcticles, like when they fall thru ground, change DustID to wat dust you want from Terraria
            }
        
            return;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;

            target.AddBuff(ModContent.BuffType<UltraFrostDebuff>(), 180);

        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<UltraFrostDebuff>(), 180);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }

}
