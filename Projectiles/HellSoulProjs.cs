using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{
    
    public class HellSoulBowProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellsoul Arrow");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.light = 0.4f;
            Projectile.scale = 1f;
            
            Projectile.aiStyle = 0;
            //drawOffsetX = -9;
            //drawOriginOffsetY = -9;
           
        }
       
       
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            AnimateProjectile();
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f, 0, new Color(255, 255, 255), 1f)];
            dust.noGravity = true;
            dust.scale = 0.8f;

            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 500f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
                {
                    if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                    {
                        Vector2 newMove = Main.npc[k].Center - Projectile.Center;
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
                Projectile.velocity = (10 * Projectile.velocity + move) / 10.2f;
                AdjustMagnitude(ref Projectile.velocity);
            }

        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 13f)
            {
                vector *= 13f / magnitude;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);

        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.NPCDeath6 with {Volume = 0.5f}, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173);

                    dust.scale = 1.5f;
                    dust.velocity *= 2;

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
    //__________________________________________________________________________________________________________

    public class HellSoulRifleProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellsoul Bullet");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.light = 0.4f;
            Projectile.scale = 1f;

            Projectile.aiStyle = 0;
            //drawOffsetX = -9;
            //drawOriginOffsetY = -9;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;


        }
        int dusttime;

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            AnimateProjectile();

            dusttime++;
            if (dusttime >= 5)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 0.8f;
            }
        }
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                dust.scale = 1.5f;


            }
            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);
            Projectile.damage= Projectile.damage / 10 *  8;
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);
        }
    
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 0.5f }, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173);
                    dust.scale = 1.5f;
                    dust.velocity *= 2;
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
    //__________________________________________________________________________________________________________

    public class HellSoulSwordProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellsoul Blade");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180;
            Projectile.light = 0.4f;
            Projectile.scale = 1f;

            Projectile.aiStyle = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

        }
        int damagetime;

    
        public override bool? CanDamage()
        {
            if (damagetime <= 60)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void AI()
        {
            damagetime++;
            if (damagetime == 1)
            {
                for (int i = 0; i < 10; i++)
                {

                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173);
                    dust2.scale = 1.5f;
                    dust2.velocity *= 2;
                }
            }

                AnimateProjectile();
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f, 0, new Color(255, 255, 255), 1f)];
            dust.noGravity = true;
            dust.scale = 0.8f;
            if (damagetime > 60)
            {

                if (Projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;
                float distance = 500f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
                    {
                        if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                        {
                            Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                            if (distanceTo < distance)
                            {

                                move = newMove;
                                distance = distanceTo;
                                target = true;

                                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
                                DrawOriginOffsetY = 0;

                            }
                            
                        }
                    }
                   
                }
                if (target)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (15 * Projectile.velocity + move) / 14f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
                else
                {
                    Projectile.rotation = damagetime * 0.5f;
                    DrawOriginOffsetY = -7;
                    Projectile.velocity *= 0.9f;
                }
            }
            else
            {
                Projectile.rotation = damagetime * 0.5f;
                DrawOriginOffsetY = -7;
            }
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            if (damagetime > 60)
            {
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                if (magnitude > 13f)
                {
                    vector *= 13f / magnitude;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                dust.scale = 1.5f;


            }
            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);
            Projectile.damage = Projectile.damage / 10 * 9;
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);
        }
  
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                //SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 0.5f }, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173);
                    dust.scale = 1.5f;
                    dust.velocity *= 2;
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
  
    //__________________________________________________________________________________________________________

    public class HellSoulMagicProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellsoul Flare");
            Main.projFrames[Projectile.type] = 4;

        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.light = 0.4f;
            Projectile.scale = 1f;

            Projectile.aiStyle = 0;
            //drawOffsetX = -9;
            //drawOriginOffsetY = -9;

        }

        int charge;
        public override void AI()
        {
            Projectile.velocity.X *= 0.9f;
            Projectile.velocity.Y *= 0.9f;
            charge++;

            AnimateProjectile();
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f, 0, new Color(255, 255, 255), 1f)];
            dust.noGravity = true;
            dust.scale = 0.8f;
            Projectile.rotation = Projectile.velocity.X / 20;

            if (charge == 50)
            {


                SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
               
                //for (int i = 0; i < 10; i++)
                if (Projectile.owner == Main.myPlayer)
                {
                    //target = Main.MouseWorld;
                    //target.TargetClosest(true);
                    float shootToX = Main.MouseWorld.X - Projectile.Center.X;
                    float shootToY = Main.MouseWorld.Y - Projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                    bool lineOfSight = Collision.CanHitLine(Main.MouseWorld, 0, 0, Projectile.position, Projectile.width, Projectile.height);


                    distance = 3f / distance;
                    shootToX *= distance * 7;
                    shootToY *= distance * 7;
                     Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(shootToX, shootToY), ModContent.ProjectileType<HellSoulMagicProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile.Kill();

                }


            }

        }
       
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);

        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {


                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173);

                    dust.velocity *= 2;

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
    public class HellSoulMagicProj2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellsoul Flare");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.light = 0.4f;
            Projectile.scale = 1f;

            Projectile.aiStyle = 0;
            //drawOffsetX = -9;
            //drawOriginOffsetY = -9;
            


        }
        int dusttime;

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            AnimateProjectile();

            dusttime++;
            if (dusttime >= 5)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 0.8f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                dust.scale = 1.5f;

            }
            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);
           
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);
        }
  
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 0.5f }, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173);
                    dust.scale = 1.5f;
                    dust.velocity *= 2;
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
    //__________________________________________________________________________________________________________

    public class HellSoulArmourProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellsoul Soul");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.light = 0.4f;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.aiStyle = 0;
            //drawOffsetX = -9;
            //drawOriginOffsetY = -9;

        }
        int damagetime = 0;
        public override bool? CanDamage()
        {
            if (damagetime <= 10)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void AI()
        {
            damagetime++;
            Projectile.rotation = Projectile.velocity.X / 20;

            AnimateProjectile();
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f, 0, new Color(255, 255, 255), 1f)];
            dust.noGravity = true;
            dust.scale = 0.8f;
            if (damagetime > 10)
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
                            Vector2 newMove = Main.npc[k].Center - Projectile.Center;
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
                    Projectile.velocity = (15 * Projectile.velocity + move) / 15f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            if (damagetime > 10)
            {
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                if (magnitude > 13.5f)
                {
                    vector *= 13.5f / magnitude;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.timeLeft -= 30;
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.7f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.7f;
            }

            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173);
                dust.scale = 1f;
                dust.velocity *= 2;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);

        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<HellSoulFireDebuff>(), 300);
        }
   
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 0.5f }, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173);
                    dust.scale = 1.5f;
                    dust.velocity *= 2;
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