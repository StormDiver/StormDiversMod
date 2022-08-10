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
    
    public class StardustSentryProj : ModProjectile
    {
       
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stardust Sentry");
            Main.projFrames[Projectile.type] = 4;
            //ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 36;
            //Projectile.light = 1f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;

            //Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.timeLeft = 36000;
            //drawOffsetX = 2;
            //drawOriginOffsetY = 2;
            Projectile.sentry = true;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanDamage()
        {
        
            return false;
        }
        //int supershot = 0;
        bool floatup = true;
        NPC target;

        public override void AI()
        {
            Projectile.alpha = (int)0.5f;
            Projectile.ai[0]++; //spawntime
            
            Main.player[Projectile.owner].UpdateMaxTurrets();
            AnimateProjectile();
            // Projectile.TurretShouldPersist();
            if (Projectile.ai[0] < 30)
            {
                Projectile.Opacity = 0;
                Dust dust;

                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 135, 0f, 0f, 0, new Color(255, 255, 255), 2f)];
                dust.noGravity = true;
            }
             
            if (Projectile.ai[0] == 30)
            {
                for (int i = 0; i < 50; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X * 1, Projectile.velocity.Y * 1, 120, default, 2f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 2.5f;
                }
                SoundEngine.PlaySound(SoundID.NPCHit5, Projectile.Center);
            }
            if (Projectile.ai[0] >= 30)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 111, 0, 5, 130, default, 1f);

                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.5f;
            }
            Projectile.ai[1]++;//shoottime
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
                

                if (distance < 700 && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy)  
                {
                   
                    if (Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                    {
                        target.TargetClosest(true);
                        if (Projectile.ai[1] > 40)
                        {
                            //supershot++;


                            //Dividing the factor of 2f which is the desired velocity by distance
                            //distance = 1.6f / distance;

                            //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                            //shootToX *= distance * 3.5f;
                            //shootToY *= distance * 3.5f;

                            
                            for (int j = 0; j < 3; j++)
                            {


                                float speedX = 0f;
                                float speedY = -4.5f;
                                
                                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(60));
                                
                                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StardustSentryProj2>(), Projectile.damage, 1, Projectile.owner);
                                SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);

                                Projectile.ai[1] = 0;
                            }
                                for (int k = 0; k < 25; k++)
                            {
                                Dust dust2;


                                dust2 = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 4f, 0, new Color(255, 255, 255), 2f)];
                                dust2.noGravity = true;
                                dust2.velocity *= 2;
                            }
                        }
                    }
                    /*if (supershot > 4)
                    {
                        target.TargetClosest(true);
                        Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, shootToX * 3, shootToY * 3, mod.ProjectileType("StardustSentryProj3"), (int) (Projectile.damage * 2f), (int)(Projectile.knockBack * 2), Main.myPlayer, 0f, 0f); //Spawning a projectile mod.ProjectileType("FlamethrowerProj") is an example of how to spawn a modded Projectile. if you want to shot a terraria prjectile add instead ProjectileID.Nameofterrariaprojectile
                        Main.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 60);
                        supershot = 0;

                    }*/
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

            SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);    

            for (int i = 0; i < 50; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f, 120, default, 2f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
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
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
      
  
         /*public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
         {
            if (opacity > 30)
            {
                Texture2D texture = mod.GetTexture("Projectiles/StardustSentryProj");

                spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame), Projectile.GetAlpha(Color.White), Projectile.rotation, Projectile.Size / 2f, Projectile.scale, Projectile.spriteDirection == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                
            }

         }*/
       public override Color? GetAlpha(Color lightColor)
        {

            
            if (Projectile.ai[0] > 30)
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

    //____________________________________________________________
    public class StardustSentryProj2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Golden Flow Invader");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            //Projectile.minion = true;
            Projectile.timeLeft = 300;
            //Projectile.light = 0.5f;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 0;
            DrawOffsetX = -5;
            //drawOriginOffsetY = -9;
            Projectile.DamageType = DamageClass.Summon;
            
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.Opacity = 0;
        }
        int dusttime = 0;
        int hometime = 0;
        Vector2 newMove;

        public override void AI()
        {
           

            dusttime++;
            if (dusttime >= 5)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X, Projectile.velocity.Y, 120, default, 0.8f);   //this make so when this projectile is active has dust around , change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
                dusttime = 0;
            }
            
            AnimateProjectile();
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            Player player = Main.player[Projectile.owner];

            hometime++;
           if (hometime > 6)
            {
                Projectile.alpha = (int)0.5f;
            }
            if (hometime > 30)
            {
                if (Projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;
                float distance = 750f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
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
                    Projectile.velocity = (10 * Projectile.velocity + move) / 10f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            if (hometime > 30)
            {
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                if (magnitude > 12f)
                {
                    vector *= 12f / magnitude;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
 
        }
        
      
        
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);

                for (int i = 0; i < 20; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X, Projectile.velocity.Y, 120, default, 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 2f;

                    //Main.dust[dust].velocity *= 2.5f;
                }

            }
        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {


            if (hometime > 6)
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
    //____________________________________________________________
    public class StardustSentryProj3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fast Golden Flow Invader");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 19;
            Projectile.height = 19;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
           // Projectile.minion = true;
            Projectile.timeLeft = 180;
            Projectile.light = 0.2f;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.ignoreWater = true;
            DrawOffsetX = -10;
            Projectile.DamageType = DamageClass.Summon;
            //drawOriginOffsetY = -9;
            //Projectile.CloneDefaults(625);
            //aiType = 338;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Opacity = 1f;
        }
        int dusttime = 0;
        public override void AI()
        {

            Projectile.alpha = (int)0.5f;
            

            dusttime++;
            if (dusttime >= 2)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X, Projectile.velocity.Y, 120, default, 0.8f);   //this make so when this projectile is active has dust around , change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
                dusttime = 0;
            }

            AnimateProjectile();
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

           

        }
       
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            Projectile.damage -= (Projectile.damage / 20);

            SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);
            for (int i = 0; i < 20; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X, Projectile.velocity.Y, 120, default, 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
                //Main.dust[dust].velocity *= 2.5f;
            }


        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);
                for (int i = 0; i < 20; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width * 2, Projectile.height * 2, 135, Projectile.velocity.X, Projectile.velocity.Y, 120, default, 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 2.5f;
                }

            }
        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
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
