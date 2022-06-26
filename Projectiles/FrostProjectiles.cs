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



namespace StormDiversMod.Projectiles
{

    public class FrostGrenadeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Grenade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;


            Projectile.light = 0.1f;
            Projectile.friendly = true;

            Projectile.aiStyle = 2;

            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.timeLeft = 200;

        }
       
       
        public override void AI()
        {

            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 100;
                Projectile.height = 100;
                Projectile.Center = Projectile.position;

                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                Projectile.knockBack = 3f;
                
            }
            else
            {
                
                if (Main.rand.NextBool())
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                    
                    dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                   
                }
            }
            }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            

            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(1) == 0) // the chance
            {
               
                    target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);

                

            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            for (int i = 0; i < 50; i++) //Frost particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -6f).RotatedByRandom(MathHelper.ToRadians(360));

                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 156, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 2f);
                Main.dust[dustIndex].noGravity = true;


            }
            for (int i = 0; i < 30; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -2.5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 2f;
                dust.velocity *= 2f;

            }
            for (int i = 0; i < 30; i++) //Grey dust fade
            {

                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default, 1f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }

        }

    }
    //___________________________________________________________________________________________________________________________________
    public class FrostSpinProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Spinner");

        }
        public override void SetDefaults()
        {

            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.ContinuouslyUpdateDamage = true;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(1) == 0) // the chance
            {

                target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);

            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);
        }
        // float hitbox = 150;
        // bool hitboxup;
        // bool hitboxdown;
        public override void AI()
        {

            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item7, Projectile.Center);
                Projectile.soundDelay = 60;
            }

            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
                    Projectile.Kill();
                }
            }
            Lighting.AddLight(Projectile.Center, 0f, 0.5f, 0.5f);
            Projectile.Center = player.MountedCenter;
            Projectile.position.X += player.width / 2 * player.direction;
            Projectile.spriteDirection = player.direction;
            Projectile.rotation += 0.15f * player.direction;
            /* if (Projectile.rotation > MathHelper.TwoPi)
             {
                 Projectile.rotation -= MathHelper.TwoPi;
             }
             else if (Projectile.rotation < 0)
             {
                 Projectile.rotation += MathHelper.TwoPi;
             }*/
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = Projectile.rotation;


            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135);  //this is the dust that this projectile will spawn
            Main.dust[dust].velocity /= 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            /* if (hitbox == 160)
             {
                 hitboxup = false;
                 hitboxdown = true;
             }
             if (hitbox == 150)
             {
                 hitboxup = true;
                 hitboxdown = false;
             }
             if (hitboxup == true)
             {
                 for (int i = 0; i < 10; i++)
                 {
                     hitbox++;
                 }
             }
             if (hitboxdown == true)
             {
                 hitbox--;
             }
             Projectile.width = (int)hitbox;
             Projectile.height = (int)hitbox;*/
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/FrostSpinProj");
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
       
    }
    //___________________________________________________________________________________________________________________________________
    public class FrostStarProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Frizbee");
        }
        public override void SetDefaults()
        {

            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
           Projectile.CloneDefaults(272);
           AIType = 272;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -6;
            DrawOriginOffsetY = -6;
        }

        
        public override void AI()
        {
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 0.7f);
            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex].noGravity = true;


            Projectile.rotation += (float)Projectile.direction * -0.6f;


            Projectile.tileCollide = true;

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {

                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 92, 0f, 0f, 0, new Color(255, 255, 255), 0.7f)];


            }
            {
                var player = Main.player[Projectile.owner];
                if (Main.rand.Next(1) == 0) // the chance
                {
                   
                        target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);

                    

                }
            }
            for (int i = 0; i < 3; i++)
            {

                float speedX = Main.rand.NextFloat(-5f, 5f);
                float speedY = Main.rand.NextFloat(-5f, 5f);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ProjectileID.CrystalShard, (int)(Projectile.damage * 0.33f), 0, Projectile.owner);
            }
            Projectile.Kill();

        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            
                {
                    target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);

                }
        
            
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {

                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 92, 0f, 0f, 0, new Color(255, 255, 255), 0.7f)];


            }
            for (int i = 0; i < 3; i++)
            {

                float speedX = Main.rand.NextFloat(-3f, 3f);
                float speedY = Main.rand.NextFloat(-3f, 3f);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ProjectileID.CrystalShard, (int)(Projectile.damage * 0.33f), 0, Projectile.owner);
            }
            Projectile.Kill();
            return false;
        }
       
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);

               

            }



        }
    }
    //___________________________________________________________________________________________________________________________________
    public class Frostthrowerproj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost");
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 125;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            if (Projectile.timeLeft > 125)
            {
                Projectile.timeLeft = 125;
            }
            if (Projectile.ai[0] > 16f)  //this defines where the flames starts
            {
                if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f, 130, default, 3f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 2.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f); //this defines the flames dust and color parcticles, like when they fall thru ground, change DustID to wat dust you want from Terraria
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
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(1) == 0) // the chance
            {
                
                    target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);

                

            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
    //___________________________________________________________________________________________________________________________________
    public class FrostAccessProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Fragment");
        }
        public override void SetDefaults()
        {

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
           
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 14;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }

        public override void AI()
        {
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 0.7f);
            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex].noGravity = true;


            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 180);


        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 180);
        }


        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width = 10, Projectile.height = 10, 135);
                }

            }
        }
    }
 
}