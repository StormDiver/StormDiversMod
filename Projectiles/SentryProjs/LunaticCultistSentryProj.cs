using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles.SentryProjs
{
    
    public class LunaticExpertSentryProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cultist Sentry");
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 46;
            //Projectile.light = 1f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 50;
            Projectile.timeLeft = 999999999;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = true;

        }
        public override bool? CanDamage()
        {

            return false;
        }
        int shoottime = 0;
        int summontime = 0;
        bool directionright;
        bool animateidle = true;
        bool animateshoot;
        NPC target;

        public override void AI()
        {
            summontime ++;
            if (summontime <3)
            {
                SoundEngine.PlaySound(SoundID.NPCHit55 with{Volume = 0.5f, Pitch = 0.5f}, Projectile.Center);

                for (int i = 0; i < 50; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 2f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 2.5f;
                }
            }
            Player player = Main.player[Projectile.owner];
            //Sets location and direction when spawned
            if (summontime <= 1)
            {
                if (Projectile.position.X < player.position.X)
                {
                    directionright = false;
                  
                }
                else
                {
                    directionright = true;
                    
                }

            }
            if (directionright)//Sets one to the left and one to the right of the player
            {              
                Projectile.position.X = player.Center.X - 80 - Projectile.width / 2;
 
            }
            else
            {
                Projectile.position.X = player.Center.X + 80 - Projectile.width / 2;

            }
            if (player.gravDir == 1) //Wil appear above the player if gravity is flipped
            {
                Projectile.position.Y = player.Center.Y - 40 - Projectile.height / 2;
                Projectile.rotation = 0;
            }
            else
            {
                Projectile.position.Y = player.Center.Y + 40 - Projectile.height / 2;
                Projectile.rotation = 3.15f;
            }
            if ((player.direction == 1 && player.gravDir == 1) || (player.direction == -1 && player.gravDir == -1)) //Face same direction as player
            {
                Projectile.spriteDirection = 1;

            }
            else
            {
                Projectile.spriteDirection = -1;

            }

            if (Main.rand.Next(3) == 0)
            {
                if (player.gravDir == 1)
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.Bottom.Y - 15), Projectile.width, 10, 173, 0, 5, 130, default, 1.5f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 1f;
                }
                else
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.Top.Y - 2), Projectile.width, 10, 173, 0, -5, 130, default, 1.5f);
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

                //Getting the shooting trajectory
                float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y + (float)target.height * 0.5f - Projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                //bool lineOfSight = Collision.CanHitLine(Projectile.Center, 1, 1, target.Center, 1, 1);
                //If the distance between the projectile and the live target is active         

                if (distance < 600f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                {

                    distance = 1.6f / distance;

                    //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                    shootToX *= distance * 6f;
                    shootToY *= distance * 6f;

                    int damage = (int)player.GetTotalDamage(DamageClass.Summon).ApplyTo(100); 
                    if (shoottime > 30)
                    {

                        for (int j = 0; j < 25; j++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
                        {
                            int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 10, Projectile.Center.Y - 10), 20, 20, 173, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.5f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                            Main.dust[dust].noGravity = true;
                            Main.dust[dust].velocity *= 2f;
                        }


                        Vector2 perturbedSpeed = new Vector2(shootToX, shootToY).RotatedByRandom(MathHelper.ToRadians(8));
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<LunaticExpertSentryProj2>(), damage, Projectile.knockBack, Projectile.owner);
                        SoundEngine.PlaySound(SoundID.Item77 with{Volume = 0.5f, Pitch = 0.5f}, Projectile.Center);

                        shoottime = 0;
                        animateidle = false;
                        Projectile.frame = 4;

                        animateshoot = true;
                    }


                }

              
            }
            
                AnimateProjectile();
            
            //Projectile.ai[0] += 1f;
            if (player.GetModPlayer<EquipmentEffects>().lunaticHood == false || player.dead)

            {
                
                Projectile.Kill();
                return;
            }
            
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.NPCDeath59 with{Volume = 0.5f, Pitch = 0.5f}, Projectile.Center);

            for (int i = 0; i < 50; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 2f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 2.5f;
            }
        }
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            if (animateidle)
            {
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
            if (animateshoot)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 6) 
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;

                }
                if (Projectile.frame >= 8)
                {
                    Projectile.frame = 0;
                    animateidle = true;
                    animateshoot = false;
                }
            }
        }


        /*public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }*/
    }


    //____________________________________________________________
    public class LunaticExpertSentryProj2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cultist Shadow Fireball");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            //Projectile.minion = true;
            Projectile.timeLeft = 120;
            //Projectile.light = 0.5f;
            Projectile.scale =1f;
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

            rotate += 3;
            Projectile.rotation = rotate * 0.1f;
            dusttime++;
            if (dusttime > 3)
            {
                for (int i = 0; i < 10; i++)
                {
                    float X = Projectile.Center.X * (float)i;
                    float Y = Projectile.Center.Y * (float)i;


                    int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 173, 0, 0, 100, default, 2f);
                    Main.dust[dust].position.X = X;
                    Main.dust[dust].position.Y = Y;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;

                }
            }

            if (Main.rand.Next (3)== 0)
            {
                
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 2f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
               
            }

            if (Projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0] = 1f;
                }
            Player player = Main.player[Projectile.owner];

            Vector2 move = Vector2.Zero;
                float distance = 300f;
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.NPCHit6 with{Volume = 0.3f}, Projectile.Center);

                for (int i = 0; i < 20; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0, 0, 120, default, 2f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    

                    Main.dust[dust].velocity *= 1.5f;
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
