using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using StormDiversMod.Basefiles;

namespace StormDiversMod.Projectiles
{
  
    //______________________________________________________________________________________________________
    public class AncientStaffProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Sand Explosion");
            
            
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
           
            Projectile.timeLeft = 40;
            Projectile.penetrate = -1;


            Projectile.tileCollide = true;
            Projectile.scale = 1f;
            Projectile.knockBack = 0;


            Projectile.aiStyle = -1;
            //Projectile.usesIDStaticNPCImmunity = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;


        }

        public override void AI()
        { 
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
           
                if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 138, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
                }
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft == 3)
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
               
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 150;
                Projectile.height = 150;
                Projectile.Center = Projectile.position;

               

                Projectile.knockBack = 6f;

            }
        }
        public override bool? CanDamage()
        {
            if (Projectile.timeLeft > 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //target.AddBuff(mod.BuffType("AridSandDebuff"), 180);
            target.AddBuff(BuffID.OnFire, 180);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(BuffID.OnFire, 180);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            return false;
        }
        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);

            for (int i = 0; i < 50; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 138);
                dust.noGravity = true;
                dust.scale = 2f;
                dust.velocity *= 3.5f;
                dust.fadeIn = 1f;

            }
            for (int i = 0; i < 50; i++)
            {
                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 55, 0f, 0f, 0, default, 1f)];
                dust.noGravity = true;
                dust.scale = 1f;


            }
          

        }

    }
    //_______________________________________
    public class AncientKnivesProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Throwing Knife");

        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = 0;


            Projectile.friendly = true;
            Projectile.timeLeft = 35;
            Projectile.penetrate = 1;

            Projectile.tileCollide = true;


            Projectile.DamageType = DamageClass.Melee;
            Projectile.light = 0.1f;

            DrawOffsetX = 0;
            DrawOriginOffsetY = -0;

            Projectile.light = 0.2f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }


        int spinspeed = 0;

        public override void AI()
        {

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (Projectile.timeLeft == 15)
            {
                Projectile.penetrate = 3;

            }
            if (Projectile.timeLeft <= 15)
            {

                spinspeed++;
                Projectile.rotation = (0.4f * spinspeed) * Projectile.direction;

                DrawOriginOffsetY = -8;

                Projectile.alpha += 17;
                Projectile.light -= 0.01f;
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
                dust.scale = 0.5f;
            }
            return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);

            Projectile.damage = (Projectile.damage * 9 / 10);
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
                dust.scale = 0.5f;
            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
           
        {
            target.AddBuff(BuffID.OnFire, 180);

            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
                dust.scale = 0.5f;
            }
        }


        public override void Kill(int timeLeft)
        {
            
            
        }



    }
    //___________________________________________________________________________
    public class AncientFlameProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Sand Stream");
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            if (Projectile.ai[0] > 13f)  //this defines where the flames starts
            {
                if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 138, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
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
            

                target.AddBuff(BuffID.OnFire, 180);



            
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
           
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(BuffID.OnFire, 180);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
    //______________________________________________________________________________________________________
    public class AncientArmourProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Sand Explosion");

        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;
            Projectile.friendly = true;

            Projectile.timeLeft = 4;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;
            Projectile.scale = 1f;
            Projectile.knockBack = 0;
           
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

        }
        public override bool? CanDamage()
        {
            if (Projectile.timeLeft <= 3)//No damage on first frame spawned
            {
                return true;
            }
            else
            {
                return false;
            }
        }
     

        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<NPCEffects>().aridimmunetime > 0 || target.friendly) //Npcs immune to explosion when activating it
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
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }

            if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 138, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);

                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.5f;
                int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
            }
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft == 3)
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;

                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 250;
                Projectile.height = 250;
                Projectile.Center = Projectile.position;



                Projectile.knockBack = 6f;

            }
        }
    
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //target.AddBuff(mod.BuffType("AridSandDebuff"), 180);
            target.AddBuff(BuffID.OnFire, 180);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(BuffID.OnFire, 180);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            return false;
        }
        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);

            for (int i = 0; i < 50; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 138);
                dust.noGravity = true;
                dust.scale = 2f;
                dust.velocity *= 5f;
                dust.fadeIn = 1f;

            }
            for (int i = 0; i < 50; i++)
            {

                float speedY = -2f;

                Vector2 perturbedSpeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 55, perturbedSpeed.X, perturbedSpeed.Y);

                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.velocity *= 4f;
                dust.fadeIn = 1f;

            }
            for (int i = 0; i < 50; i++)
            {
                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 55, 0f, 0f, 0, default, 1f)];
                dust.noGravity = true;
                dust.scale = 1f;


            }


        }

    }
}
