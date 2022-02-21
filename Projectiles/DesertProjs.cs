﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{
    public class DesertBowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Bow Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 12;

            Projectile.aiStyle = 1;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
           
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;           
            Projectile.arrow = true;
            AIType = ProjectileID.WoodenArrowFriendly;
            
        }
        int dropdust = 10;
        public override void AI()
        {
            if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 10, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);

                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                
                int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 54, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
            }
            dropdust--;
            if ((Projectile.velocity.X >= 3 || Projectile.velocity.X <= -3))
                if (Main.rand.Next(13) == 0)
                {
              
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 3), ModContent.ProjectileType<DesertArrowDust>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                    dropdust = 10;
            }

        }
        // int reflect = 5;


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }

        public override void Kill(int timeLeft)
        {
            
             
             SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {

                Vector2 vel = new Vector2(Main.rand.NextFloat(20, 20), Main.rand.NextFloat(-20, -20));
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
            }

        }
    }
    //________________________________________________________________________________________
    public class DesertArrowDust : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Arrow Dust");

        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.light = 0.1f;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            // Projectile.aiStyle = 1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 2;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors

            if (Projectile.ai[0] > 0f)  //this defines where the flames starts
            {
                if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 10, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 54, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
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
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
            Projectile.damage = (Projectile.damage * 9) / 10;
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }


    }
    //________________________________________________________________________________________
    public class DesertSpearProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Spear");
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
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, 54, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 200, Scale: 1.2f);
                dust.noGravity = true;
                dust.velocity += Projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;
            }


        }
      

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }
    }
    //________________________________________________________________________________________
    public class DesertSpearTipProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Spear Dust");

        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.light = 0.1f;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;            
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 150;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors

            if (Projectile.ai[0] > 30f)  //this defines where the flames starts
            {
                if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 10, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 54, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
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
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }


    }
    //________________________________________________________________________________________
   
    public class DesertSpellProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Sand");
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 140;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors

            if (Projectile.ai[0] > 0f)  //this defines where the flames starts
            {
                if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 10, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 54, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
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
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
    //______________________________________________________________________________________________________
    public class DesertJarProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Dust");
            
            
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;

            Projectile.friendly = true;
           
            Projectile.timeLeft = 90;
            Projectile.penetrate = 1;


            Projectile.tileCollide = true;
            Projectile.scale = 1f;
            Projectile.knockBack = 0;


            Projectile.aiStyle = 14;
            //Projectile.usesIDStaticNPCImmunity = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;


        }

        public override void AI()
        { 
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors

            if (Projectile.ai[0] > 0f)  //this defines where the flames starts
            {
                if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 10, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 54, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
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
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 180);
            Projectile.damage = (Projectile.damage * 9) / 10;
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 180);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            return false;
        }
        public override void Kill(int timeLeft)
        {

            Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
            

        }

    }
   
}
