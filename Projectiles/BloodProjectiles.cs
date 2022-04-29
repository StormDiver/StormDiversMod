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
                SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 13);
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
            SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 13);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 13);

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
        /* OLD SPEAR CODE
        public float MovementFactor // Change this value to alter how fast the spear moves
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            // Sadly, Projectile/ModProjectile does not have its own
            Player projOwner = Main.player[Projectile.owner];
            // Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
            // As long as the player isn't frozen, the spear can move
            if (!projOwner.frozen)
            {
                if (MovementFactor == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    MovementFactor = 1.5f; // Make sure the spear moves forward when initially thrown out
                    Projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 2) // Somewhere along the item animation, make sure the spear moves back
                {
                    MovementFactor -= 1f;
                }
                else // Otherwise, increase the movement factor
                {
                    MovementFactor += 1.4f;
                }
            }
            // Change the spear position based off of the velocity and the movementFactor
            Projectile.position += Projectile.velocity * MovementFactor;
            // When we reach the end of the animation, we can kill the spear projectile
            if (projOwner.itemAnimation == 0)
            {
                Projectile.Kill();
            }
            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            // Offset by 90 degrees here
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(90f);
            }

            // These dusts are added later, for the 'ExampleMod' effect
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, 5, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 1, Scale: 1.2f);
                dust.noGravity = true;
                dust.velocity += Projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;
            }
        
        }*/


    }
   
    //_______________________________________________
    public class BloodOrbitProj : ModProjectile
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
            if (player.GetModPlayer<StormPlayer>().BloodOrb == false || player.dead)
            {
                Projectile.Kill();
            }



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

            // drawOffsetX = -3;
            // drawOriginOffsetY = 1;
        }
        int shoottime = 0;
        public override void AI()
        {
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
            SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 13);

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
}