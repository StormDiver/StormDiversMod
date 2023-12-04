using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;


namespace StormDiversMod.Projectiles.SentryProjs
{
    
    public class SkyKnightSentryProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star Warrior Sentry");
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 32;
            //Projectile.light = 1f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 99999999;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            //drawOffsetX = 2;
            //drawOriginOffsetY = 2;

        }
        public override bool? CanDamage()
        {

            return false;
        }
        int shoottime = 0;
        int summontime = 0;
        bool scaleup;
        bool animate;
        NPC target;
        public override void AI()
        {
            summontime ++;
            if (summontime <3)
            {
                SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);

                for (int i = 0; i < 50; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 162, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 2f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 2.5f;
                }
            }
            var player = Main.player[Projectile.owner];

            Projectile.position.X = player.Center.X  - Projectile.width / 2;
            if (player.gravDir == 1)
            {
                Projectile.position.Y = player.Center.Y - 75 - Projectile.height / 2;
                Projectile.rotation = 0;

            }
            else
            {
                Projectile.position.Y = player.Center.Y + 75 - Projectile.height / 2;
                Projectile.rotation = 3.15f;

            }


            Projectile.alpha = (int)0.5f;

        
            if (Main.rand.Next(3) == 0)
            {
                if (player.gravDir == 1)
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 162, 0, 5, 130, default, 1.5f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 1f;
                }
                else
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 162, 0, -5, 130, default, 1.5f);
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 1f;
                }
            }
            shoottime++;
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

                if (Vector2.Distance(Projectile.Center, target.Center) <= 500f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy && target.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0) && Collision.CanHitLine(Projectile.Center, 0, 0, player.Center, 0, 0))
                {
                    float projspeed = 12;
                    Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                   
                    if (shoottime > 60)
                    {

                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(8));

                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<SkyKnightSentryProj2>(), 30, Projectile.knockBack, Projectile.owner);

                        SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);

                        shoottime = 0;
                        animate = true;
                    }

                }

            }
            if (animate)
            {
                AnimateProjectile();
            }
            //Projectile.ai[0] += 1f;
            if (Projectile.owner == Main.myPlayer)
            {
                if (player.GetModPlayer<ArmourSetBonuses>().skyKnightSet == false || player.dead)

                {

                    Projectile.Kill();
                    return;
                }
            }
            //To make the sentry pulse
            if (scaleup)
            {
                Projectile.scale += 0.01f;
            }
            else
            {
                Projectile.scale -= 0.01f;
            }
            if (Projectile.scale >= 1.15f)
            {
                scaleup = false;
            }
            if (Projectile.scale <= 0.85f)
            {
                scaleup = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.NPCDeath7 with { Volume = 0.6f}, Projectile.Center);

            for (int i = 0; i < 50; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 162, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 2f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 2.5f;
            }
        }
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;

            }
            if (Projectile.frame >= 3)
            {
                Projectile.frame = 0;
                animate = false;
            }
        }



        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }
    }


    //____________________________________________________________
    public class SkyKnightSentryProj2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star Warrior Star");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            //Projectile.minion = true;
            Projectile.timeLeft = 120;
            //Projectile.light = 0.5f;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 0;
            DrawOffsetX = 0;
            //drawOriginOffsetY = -9;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            
        }
        int dusttime = 0;
        int rotate;
        Vector2 newMove;
        public override void AI()
        {

            rotate += 2;
            Projectile.rotation = rotate * 0.1f;
            dusttime++;
            if (dusttime > 3)
            {
                for (int i = 0; i < 10; i++)
                {
                    float X = Projectile.Center.X * (float)i;
                    float Y = Projectile.Center.Y * (float)i;


                    int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 162, 0, 0, 100, default, 1.5f);
                    Main.dust[dust].position.X = X;
                    Main.dust[dust].position.Y = Y;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;

                }
            }

            if (Projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0] = 1f;
                }

            Player player = Main.player[Projectile.owner];
            Vector2 move = Vector2.Zero;
                float distance = 100f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
                    {
                        if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                        {
                        if (player.HasMinionAttackTargetNPC)
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
                if (target)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }
        
        private void AdjustMagnitude(ref Vector2 vector)
        {
            
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                if (magnitude > 14f)
                {
                    vector *= 14f / magnitude;
                }
            
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
  
            
        }
        
      
        
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath7 with {Volume = 0.6f}, Projectile.Center);

                for (int i = 0; i < 20; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 162, 0, 0, 120, default, 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    

                    Main.dust[dust].velocity *= 1.5f;
                }

            }
        }

      
        public override Color? GetAlpha(Color lightColor)
        {



            Color color = Color.White;
            color.A = 150;
            return color;



        }

    }
    
}
