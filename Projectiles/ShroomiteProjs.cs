using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{
    public class ShroomBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomite Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;
            
            Projectile.friendly = true;
            Projectile.timeLeft = 400;
            Projectile.penetrate = 3;
            
            
            Projectile.tileCollide = true;


            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

        }
        int reflect = 4;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();
            }
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }
        int dusttime;
        public override void AI()
        {

             dusttime++;
           
            if (dusttime > 3)
            {
                for (int i = 0; i < 10; i++)
                {
                    float X = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                    float Y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
                  

                    int dust = Dust.NewDust(new Vector2(X, Y), 0, 0, 206, 0, 0, 100, default, 1f);
                    Main.dust[dust].position.X = X;
                    Main.dust[dust].position.Y = Y;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;


                }
            }

           
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 25; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 206, default, 1.2f);
            }
            Projectile.damage = (Projectile.damage * 9) / 10;
        }

        public override void Kill(int timeLeft)
        {

            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
            }

        }

       

    }
    //______________________________________________________________________________________
    public class ShroomArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomite Arrow");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.aiStyle = 1;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 2;
            Projectile.arrow = true;
            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.arrow = true;
            AIType = ProjectileID.WoodenArrowFriendly;
            //Creates no immunity frames
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }
        int spwmushroom = 8;
        
        public override void AI()
        {

            spwmushroom--;
            if (Main.rand.Next(12) == 0)
            {
                
                
                int speedX = 0;
                int speedY = 0;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(speedX, speedY), ModContent.ProjectileType<ShroomMush>(), (int)(Projectile.damage * 0.5f), 0, Projectile.owner);
                spwmushroom = 8;
            }
           /* trail++;
            if (trail > 2)
            {
                Dust dust;

                Vector2 position = Projectile.Center;
                dust = Terraria.Dust.NewDustPerfect(position, 206, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;
            }*/
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 130, default, 1.2f);
            }
            Projectile.damage = (Projectile.damage * 9) / 10;

        }

        public override void Kill(int timeLeft)
        {

            //int item = Main.rand.NextBool(5) ? Item.NewItem(Projectile.getRect(), ModContent.ItemType<Ammo.ShroomArrow>()) : 0;
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
           
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }
        public override void PostDraw(Color drawColor)
        {
            //Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/ShroomArrowProj_Glow");

            //Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Center, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


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
    }
    //______________________________________________________________________________________________
    public class ShroomRocketProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomite Rocket");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;


            Projectile.light = 0.2f;
            Projectile.friendly = true;

            // Projectile.CloneDefaults(134);
            // aiType = 134;
            Projectile.aiStyle = 0;
            Projectile.extraUpdates = 1;
            //aiType = ProjectileID.Bullet;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.timeLeft = 300;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();

            return false;
        }
        int timeleft = 240;

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            timeleft--;
            if (timeleft <= 0)
            {
                Projectile.Kill();
            }
            /*for (int i = 0; i < 5; i++)
            {
                float x2 = Projectile.Center.X - Projectile.velocity.X / 20f * (float)i;
                float y2 = Projectile.Center.Y - Projectile.velocity.Y / 20f * (float)i;
                int j = Dust.NewDust(new Vector2(x2, y2), 0, 0, 206);
                //Main.dust[num165].alpha = alpha;
                Main.dust[j].position.X = x2;
                Main.dust[j].position.Y = y2;
                Main.dust[j].velocity *= 0.1f;
                Main.dust[j].noGravity = true;
                Main.dust[j].scale = 1.5f;
            }*/
            for (int i = 0; i < 2; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 10, 10, 206, 0f, 0f, 100, default, 1.5f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 0.5f;

                Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;

            }
            int dustIndex2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 1f);
            Main.dust[dustIndex2].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex2].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex2].noGravity = true;


        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {


        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);



            for (int i = 0; i < 25; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31);
                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.velocity *= 4f;
                dust.fadeIn = 1f;

            }
            for (int i = 0; i < 25; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 206);
                dust.noGravity = true;

                dust.scale = 2f;
                dust.velocity *= 4f;
                dust.fadeIn = 1f;

            }

            if (Projectile.owner == Main.myPlayer)
            {
                int numberProjectiles = 6 + Main.rand.Next(3);
                for (int i = 0; i < numberProjectiles; i++)
                {


                    // Calculate new speeds for other projectiles.
                    // Rebound at 40% to 70% speed, plus a random amount between -8 and 8
                    //float speedX = -Projectile.velocity.X * Main.rand.NextFloat(.4f, .7f) + Main.rand.NextFloat(-16f, 16f);
                    //float speedY = -Projectile.velocity.Y * Main.rand.NextFloat(.4f, .7f) + Main.rand.NextFloat(-16f, 16f);
                    Vector2 perturbedSpeed = new Vector2(0, -12).RotatedByRandom(MathHelper.ToRadians(360));

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                        ModContent.ProjectileType<ShroomMush>(), (int)(Projectile.damage * 0.4f), 1, Projectile.owner);
                }
            }
        }
        public override void PostDraw(Color drawColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/ShroomRocketProj_Glow");

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Center, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }
    }
    //______________________________________________________________________________________________
    public class ShroomGrenProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomite Grenade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;


            Projectile.light = 0.1f;
            Projectile.friendly = true;

            Projectile.CloneDefaults(133);
            AIType = 133;

            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.timeLeft = 450;

        }
        

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

           
             
                //Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.5f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.5f;
                }
            
           // Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 56);
            return false;
        }
        int timeleft = 240;
        public override void AI()
        {
            timeleft--;
            if (timeleft <= 0)
            {
                Projectile.Kill();
            }
            if ((Projectile.velocity.X >= 0.5 || Projectile.velocity.X <= -0.5) || (Projectile.velocity.Y >= 0.5 || Projectile.velocity.Y <= -0.5))
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 206, 0f, 0f, 100, default, 1.5f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                // Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {


        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 62);


            for (int i = 0; i < 25; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31);
                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.velocity *= 4f;
                dust.fadeIn = 1f;

            }
            for (int i = 0; i < 25; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 206);
                dust.noGravity = true;

                dust.scale = 2f;
                dust.velocity *= 4f;
                dust.fadeIn = 1f;

            }
            if (Projectile.owner == Main.myPlayer)
            {
                int numberProjectiles = 6 + Main.rand.Next(3);
                for (int i = 0; i < numberProjectiles; i++)
                {
                    
                    Vector2 perturbedSpeed = new Vector2(0, -12).RotatedByRandom(MathHelper.ToRadians(360));

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                        ModContent.ProjectileType<ShroomMush>(), (int)(Projectile.damage * 0.4f), 1, Projectile.owner);
                }
            }
        }
        public override void PostDraw(Color drawColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/ShroomGrenProj_Glow");

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Center, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }
    }
   
    //______________________________________________________________________________________________
    public class ShroomMush : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spinning Mushroom");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.CloneDefaults(131);
            AIType = 131;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 50;
            Projectile.tileCollide = false;

        }
        int damagetime;
        public override void AI()
        {
            Projectile.penetrate = 1;
            damagetime++;
        }
        public override bool? CanDamage()
        {
            if (damagetime < 8)
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
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 50;
            return color;

        }

    }
    //____________________________________________________________________________________________________________________________________________
    public class ShroomSetRocketProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Launcher Attachment Rocket");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.aiStyle = 0;
            Projectile.light = 0.1f;

            Projectile.friendly = true;
            Projectile.timeLeft = 400;
            Projectile.penetrate = -1;


            Projectile.tileCollide = true;


            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
            //aiType = ProjectileID.Bullet;


        }


        // int dusttime = 10;
        public override void AI()
        {


            /*  Dust dust;

              Vector2 position = Projectile.Center;
              dust = Terraria.Dust.NewDustPerfect(position, 206, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
              dust.noGravity = true;*/

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            for (int i = 0; i < 10; i++)
            {
                float x2 = Projectile.Center.X - Projectile.velocity.X / 20f * (float)i;
                float y2 = Projectile.Center.Y - Projectile.velocity.Y / 20f * (float)i;
                int j = Dust.NewDust(new Vector2(x2, y2), 1, 1, 206);
                //Main.dust[num165].alpha = alpha;
                Main.dust[j].position.X = x2;
                Main.dust[j].position.Y = y2;
                Main.dust[j].velocity *= 0.1f;
                Main.dust[j].noGravity = true;
                Main.dust[j].scale = 1f;
            }
            
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 175;
                Projectile.height = 175;
                Projectile.Center = Projectile.position;


                Projectile.knockBack = 3f;

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
        }

        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 74);

            Projectile.alpha = 255;
            for (int i = 0; i < 50; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31);
                dust.noGravity = true;
                dust.scale = 2.5f;
                dust.velocity *= 4f;
                dust.fadeIn = 1f;

            }
            for (int i = 0; i < 50; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 206);
                dust.noGravity = true;

                dust.scale = 3f;
                dust.velocity *= 4f;
                dust.fadeIn = 1.5f;

            }
            for (int i = 0; i < 80; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 206, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 2f;
            }

            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

        }
        public override Color? GetAlpha(Color lightColor)
        {



            return Color.White;

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


    }
    //____________________________________________________________________________________________________________________________________________
    public class ShroomBowArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomite Glowing arrow");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;


            //Projectile.light = 0.6f;
            Projectile.friendly = true;

             //Projectile.CloneDefaults(225);
            //aiType = 225;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.aiStyle = 1;
            Projectile.penetrate = 2;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.timeLeft = 300;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }
        int reflect = 3;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();
            }
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 1.3f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 1.3f;
                }
            }
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 56);
            return false;
        }


        public override void AI()
        {
            /*for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                //If the npc is hostile

                //Get the shoot trajectory from the projectile and target
                float shootToX = target.Center.X - Projectile.Center.X;
                float shootToY = target.Center.Y - Projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                //If the distance between the live targeted npc and the projectile is less than 480 pixels
                if (distance < 300f && target.active)
                {

                    distance = 0.5f / distance;

                    //Multiply the distance by a multiplier proj faster
                    shootToX *= distance * 12;
                    shootToY *= distance * 12;

                    //Set the velocities to the shoot values
                    Projectile.velocity.X = shootToX;
                    Projectile.velocity.Y = shootToY;
                }

            }*/
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;


        }

        public override void Kill(int timeLeft)
        {

            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
           
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
            }

        }
       
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
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

    }

}
