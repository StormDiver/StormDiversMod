using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;

namespace StormDiversMod.Projectiles
{
    public class HarpyProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Harpy Feather");
            
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.light = 0f;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 90;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
          
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.ArmorPenetration = 5;

        }
        public override void AI()
        {
            /*Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);*/
            
                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
           

            //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 30);
            if (Main.rand.Next(2) == 0) // the chance
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 202, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;

            }



        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            
            /*if (Main.rand.Next(2) == 0) // the chance
            {
                target.AddBuff(BuffID.Poisoned, 240);
            }*/
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

         
                return true;
            
            
        }

        public override void Kill(int timeLeft)
        {


            //Main.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);

            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 202);
            }
            SoundEngine.PlaySound(SoundID.NPCHit11, Projectile.Center);

        }



    }
    //___________________________________________________________________________________________
    public class HarpyProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Big Harpy Feather");
           
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.light = 0f;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 120;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = -10;
        }
        
        public override void AI()
        {
            Projectile.rotation += (float)Projectile.direction * -0.3f;

            if (Main.rand.Next(2) == 0) // the chance
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 202, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;

            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            Projectile.damage = (Projectile.damage * 5) / 10;
            Projectile.velocity.X *= 0.5f;
            Projectile.velocity.Y *= 0.5f;


            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 202);
                
            }
            SoundEngine.PlaySound(SoundID.NPCHit11, Projectile.Center);

        }


        public override void Kill(int timeLeft)
        {



            //Main.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 202);
            }
            SoundEngine.PlaySound(SoundID.NPCHit11, Projectile.Center);
        }
       
    }
    //_______________________________________________
    public class HarpyArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Feather Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 7;
            Projectile.height = 7;

            Projectile.aiStyle = 0;
           
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 5;
            Projectile.arrow = true;
            Projectile.tileCollide = true;
            Projectile.knockBack = 8f;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
            Projectile.arrow = true;
            

            DrawOffsetX = -4;
            DrawOriginOffsetY = 0;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            /* int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 187, 0f, 0f, 100, default, 0.7f);
             Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
             Main.dust[dustIndex].noGravity = true;*/
            if (Main.rand.Next(2) == 0) // the chance
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 202, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;

            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            for (int i = 0; i < 5; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 202, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);
            }
        }

        public override void Kill(int timeLeft)
        {

            //Main.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 202);
            }
            SoundEngine.PlaySound(SoundID.NPCHit11, Projectile.Center);
        }

    }
    //___________________________________________________________________________
    public class HarpyYoyoProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Harpy Yoyo");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 5f;

            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 180f;

            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 16f;
        }
        public override void SetDefaults()
        {

            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 360;

            Projectile.scale = 1f;
            Projectile.aiStyle = 99;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            // drawOffsetX = -3;
            // drawOriginOffsetY = 1;
        }
        //int shoottime = 0;
      
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage); //update damage
            if (Main.rand.Next(2) == 0) // the chance
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 202, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;

            }
            /*shoottime++;
            if (shoottime >= 40)
            {


                for (int i = 0; i < 3; i++)
                {
                    // Calculate new speeds for other projectiles.
                    // Rebound at 40% to 70% speed, plus a random amount between -8 and 8
                    Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 7);

                    Vector2 perturbedSpeed = new Vector2(0, -4).RotatedByRandom(MathHelper.ToRadians(360));

                    Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("TurtleYoyoProj2"), (int)(Projectile.damage * 1.25f), 0f, Projectile.owner, 0f, 0f);
                    shoottime = 0;
                }
            }*/

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
    }
}