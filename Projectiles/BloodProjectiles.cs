using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using static Terraria.ModLoader.ModContent;


namespace StormDiversMod.Projectiles
{

    public class BloodDropProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Drop");
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = 2;
            //Projectile.CloneDefaults(48);
            //aiType = 48;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.knockBack = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        bool bloodspray = true;
        public override void AI()
        {


            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 5, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }


            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.Center;
                dust = Terraria.Dust.NewDustPerfect(position, 115, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
                if (bloodspray)
                {
                    dust.noGravity = true;
                }

            }

            if (!bloodspray)
            {
                Projectile.velocity.X = 0;
                //Projectile.velocity.Y = 0;
            }
            return;
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            if (bloodspray)
            {
                SoundEngine.PlaySound(SoundID.NPCHit13, Projectile.Center);
            }
            bloodspray = false;

            //Projectile.timeLeft = 100;
            return false;
        }
    }
    //------------------------------------------
    public class BloodSwordProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Trial");
        }
        public override void SetDefaults()
        {

            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.penetrate = 3;
            Projectile.timeLeft = 50;
            Projectile.knockBack = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        //bool bloodspray = true;
        public override void AI()
        {
            if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 5, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }


            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 5, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
            }
            return;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            SoundEngine.PlaySound(SoundID.NPCHit13, Projectile.Center);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCHit13, Projectile.Center);

            Projectile.Kill();
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 5, 0f, 0f, 0, new Color(255, 255, 255), 1f)];

            }
        }
    }
    //______________________________________________________________________________________________________________________
    public class BloodSpearProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Spear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 19;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }
        protected virtual float HoldoutRangeMin => 25f;
        protected virtual float HoldoutRangeMax => 100f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner]; // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            int duration = player.itemAnimationMax; // Define the duration the projectile will exist in frames

            player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

            // Reset projectile time left if necessary
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }

            Projectile.velocity = Vector2.Normalize(Projectile.velocity); // Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

            float halfDuration = duration * 0.5f;
            float progress;

            // Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation.
            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }

            // Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

            // Apply proper rotation to the sprite.
            if (Projectile.spriteDirection == -1)
            {
                // If sprite is facing left, rotate 45 degrees
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                // If sprite is facing right, rotate 135 degrees
                Projectile.rotation += MathHelper.ToRadians(135f);
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, 5, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 1, Scale: 1.2f);
                dust.noGravity = true;
                dust.velocity += Projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;
            }
        }       
    }
   
    //_______________________________________________
    public class BloodOrbitProj : ModProjectile //Unused
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Orb");
        }
        public override void SetDefaults()
        {

            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
  
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90;
           
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {

            Projectile.rotation += (float)Projectile.direction * -0.3f;

            if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 5, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }


            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.Center;
                dust = Terraria.Dust.NewDustPerfect(position, 5, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);


            }
            Player p = Main.player[Projectile.owner];

            //Factors for calculations
            double deg = (double)Projectile.ai[1] * -4.1f; //The degrees, you can multiply Projectile.ai[1] to make it orbit faster, may be choppy depending on the value
            double rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = 100; //Distance away from the player

            /*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */

            Projectile.position.X = p.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = p.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
            Projectile.ai[1] += 1f;
            var player = Main.player[Projectile.owner];
       

        }
    }
    //_________________________________________
    public class BloodYoyoProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart Attack Yoyo");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 5f;

            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 200f;

            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 13f;
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
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            // drawOffsetX = -3;
            // drawOriginOffsetY = 1;
        }
        int shoottime = 0;
        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<NPCEffects>().yoyoimmunetime > 0) //Static immunity
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
            Player player = Main.player[Projectile.owner];
            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage); //update damage
            if (Main.rand.Next(2) == 0) // the chance
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 5, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = false;

            }
            shoottime++;
            if (shoottime >= 8)
            {
               
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<BloodYoyoProj2>(), (int)(Projectile.damage * .5f), 0, Projectile.owner);
                shoottime = 0;
                
            }

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.GetGlobalNPC<NPCEffects>().yoyoimmunetime = 10; //frames of static immunity

        }

    }
    //____________________________________________________________________________________________
    public class BloodYoyoProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Yoyo trail");
        }
        public override void SetDefaults()
        {

            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 20;
            Projectile.knockBack = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        //bool bloodspray = true;
        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 5, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }


            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 5, 0f, 0f, 0, new Color(255, 255, 255), 1f)];


            }


            return;
        }

      
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCHit13, Projectile.Center);

            Projectile.Kill();
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 5, 0f, 0f, 0, new Color(255, 255, 255), 1f)];

            }
        }
    }
    //____________________________________________________________________________________________
    public class BloodBootProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Boot Trail");
        }
        public override void SetDefaults()
        {

            Projectile.width = 40;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Generic;

            Projectile.penetrate = 4;
            Projectile.timeLeft = 120;
            Projectile.knockBack = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        //bool bloodspray = true;
        public override void AI()
        {


            if (Main.rand.Next(3) == 0) 
            {
                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 115, 0f, 0f, 0, new Color(255, 255, 255), 1.5f)];
                dust.noGravity = true;
            }


            if (Main.rand.Next(2) == 0) 
            {
                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 115, 0f, 5f, 0, new Color(255, 255, 255), 1f)];
                dust.velocity *= 0;

            }


            return;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 5, 0f, 0f, 0, new Color(255, 255, 255), 1f)];

            }
        }
    }
    //________________________
    public class BloodGrenadeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Grenade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;


            Projectile.friendly = true;

            Projectile.aiStyle = 14;


            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            if (GetInstance<ConfigurationsGlobal>().ThrowingTryhards)
            {
                Projectile.DamageType = DamageClass.Throwing;

            }
            else
            {
                Projectile.DamageType = DamageClass.Ranged;

            }
            Projectile.timeLeft = 180;
            DrawOriginOffsetY = -4;
        }
        public override void AI()
        {

            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 5, 0f, 0f, 100, default, 1f);
            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex].noGravity = true;

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Projectile.Kill();
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.4f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.4f;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            //SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            SoundEngine.PlaySound(SoundID.NPCHit9, Projectile.Center);

            float numberProjectiles = 3 + Main.rand.Next(3);

            for (int i = 0; i < numberProjectiles; i++)
            {

                float speedX = 0f;
                float speedY = -4f;
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(135));
                float scale = 1f - (Main.rand.NextFloat() * .5f);
                perturbedSpeed = perturbedSpeed * scale;

                int projID = Projectile.NewProjectile(null, new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BloodDropProj>(), Projectile.damage / 3, 1, Projectile.owner);
                Main.projectile[projID].DamageType = DamageClass.Ranged;
                Main.projectile[projID].timeLeft = 180;

            }

            for (int i = 0; i < 20; i++) //blood particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));

                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 5, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 2f);
                Main.dust[dustIndex].noGravity = true;


            }
            for (int i = 0; i < 20; i++) //Large blood particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -1f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 115, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 1f;
                dust.velocity *= 2f;

            }
            /*for (int i = 0; i < 20; i++) //Grey dust fade
            {

                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default, 1f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }*/
        }
    }
}