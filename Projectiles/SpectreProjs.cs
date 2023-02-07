using System;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
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
   
    public class SpectreHoseProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectre Bolt");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.light = 0.6f;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 240;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 0.8f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            DrawOffsetX = -2;
            DrawOriginOffsetY = 0;
            
        }
    
        int speedup = 0;
        public override void AI()
        {            
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            //Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);

            AnimateProjectile();
           
            for (int i = 0; i < 10; i++)
            {
                float X = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                float Y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
               

                int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 66, 0, 0, 100, default, 1f);
                Main.dust[dust].position.X = X;
                Main.dust[dust].position.Y = Y;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0f;
            }
            speedup++;
            if (speedup <= 50)
            {
                Projectile.velocity.X *= 1.03f;
                Projectile.velocity.Y *= 1.03f;
                Projectile.damage += 1;
               
            }       
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 264);
                dust.scale = 1.5f;
                dust.velocity *= 2;
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);
            for (int i = 0; i < 25; i++)
            {               
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 264);
                dust.scale = 1.5f;
                dust.velocity *= 2;
                dust.noGravity = true;
            }
        }
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
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
    
    //_______________________________________________________________________________________________

    public class SpectreStaffSpinProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectre Orb");
            //Main.projFrames[Projectile.type] = 4;

            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 3600;
            Projectile.ignoreWater = true;

            Projectile.tileCollide = false;
            Projectile.penetrate = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.light = 0.1f;


            //Projectile.CloneDefaults(297);
            //aiType = 297;

        }
        int currentdistance = 0;
        int maxdistance = 0;
        int speed;
        bool lineOfSight;
        public override void OnSpawn(IEntitySource source)
        {
            maxdistance = Main.rand.Next(80, 160);
            if (maxdistance % 2 == 0)
            {
                speed = 8;
            }
            else
            {
                speed = -8;
            }
            if (maxdistance> 120)
            {
                speed = (speed * 15) / 20;
            }
        }
        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
               
            }
            //picks a random direction and distance
            if (currentdistance <= maxdistance) //Disttime will count up making the orb orbit further out until it reaches the orbittime value
            {
                currentdistance++;
            }
            if (currentdistance == maxdistance - 1)
            {
                SoundEngine.PlaySound(SoundID.Item30 with { Volume = 0.5f }, Projectile.Center);

                for (int i = 0; i < 20; i++)
                {
                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 185);
                    dust2.noGravity = true;
                }
            }
            Player player = Main.player[Projectile.owner];

            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(Projectile.originalDamage); //update damage

            //Factors for calculations

            double deg = (double)Projectile.ai[1] * speed; //The degrees, you can multiply Projectile.ai[1] to make it orbit faster, may be choppy depending on the value
            double rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = currentdistance; //Distance away from the player

            /*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */

            Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
            Projectile.ai[1] += 1f;
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }
            //AnimateProjectile();

            Dust dust;
            Vector2 position = Projectile.Center;
            dust = Terraria.Dust.NewDustPerfect(position, 185, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;

            lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, player.position, player.width, player.height);

            if (player.controlUseTile && lineOfSight && currentdistance >= maxdistance) //will fire projectile once it reaches maximum orbit and has a line of sight with the player
            {

                if (Projectile.owner == Main.myPlayer)
                {
                    {
                        //target = Main.MouseWorld;
                        //target.TargetClosest(true);
                        float shootToX = Main.MouseWorld.X - Projectile.Center.X;
                        float shootToY = Main.MouseWorld.Y - Projectile.Center.Y;
                        float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                        bool lineOfSight = Collision.CanHitLine(Main.MouseWorld, 0, 0, Projectile.position, Projectile.width, Projectile.height);

                        distance = 3f / distance;
                        shootToX *= distance * 8;
                        shootToY *= distance * 8;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(shootToX, shootToY), ModContent.ProjectileType<SpectreStaffSpinProj2>(), (int)(Projectile.damage * 2f), Projectile.knockBack + 2, Projectile.owner);
                        Projectile.Kill();
                    }
                }
            }
        }
        
        public override bool? CanDamage()
        {
            if (!lineOfSight)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {              
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 185);
                    dust.noGravity = true;
            }
        }
        /*public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }*/
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 150;
            return color;
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
    //_______________________________________________________________________________________________
    public class SpectreStaffSpinProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectre Orb");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }

        public override void SetDefaults() 
        {
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.light = 0.1f;
            //Projectile.CloneDefaults(297);
            // aiType = 297;
        }
        bool lineOfSight;    
        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.Center;
            dust = Terraria.Dust.NewDustPerfect(position, 185, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;

            lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, player.position, player.width, player.height);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 185);
                dust.noGravity = true;
                dust.scale = 0.7f;
            }
        }
        public override void Kill(int timeLeft)
        {

            for (int i = 0; i < 10; i++)
            {                
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 185);
                dust.noGravity = true;
                dust.scale = 0.7f;
            }

        }
        
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;
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
    
    //_______________________________________________________________________________________________
    public class SpectreDaggerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectre Dagger");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.light = 0.4f;
            Projectile.friendly = true;
            
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.penetrate = 5;
            
        }
        int releasetime = 0;

        float speed;
        float inertia;
        float distanceToIdlePosition; //distance to player
        public override void AI()
        {
            releasetime++;
            Projectile.DamageType = DamageClass.Magic;

            var player = Main.player[Projectile.owner];
            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(Projectile.originalDamage); //update damage

            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.Center;
            dust = Terraria.Dust.NewDustPerfect(position, 185, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;
            dust.scale = 1f;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            /*if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }*/
            speed = 30f;
            inertia = 8f;

            Vector2 idlePosition = Main.MouseWorld;
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-50, 50));
            distanceToIdlePosition = vectorToIdlePosition.Length();
            if (player.controlUseItem && player.HeldItem.type == ModContent.ItemType<Items.Weapons.SpectreDagger>() && !player.dead)
            {
                //if (Collision.CanHit(Projectile.Center, 0, 0, Main.MouseWorld, 0, 0))
                {
                    Projectile.timeLeft = 120;
                    if (distanceToIdlePosition > 10f)
                    {
                        vectorToIdlePosition.Normalize();
                        vectorToIdlePosition *= speed;
                    }
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
            }
            if (distanceToIdlePosition > 100f)
            {
                Projectile.tileCollide = true;
            }
            else
            {
                Projectile.tileCollide = false;
            }
            /*Vector2 move = Vector2.Zero;
            float distance = 1000f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (player.controlUseItem && player.HeldItem.type == ModContent.ItemType<Items.Weapons.SpectreDagger>() && !player.dead)
                    {
                        if (Collision.CanHit(Projectile.Center, 0, 0, Main.MouseWorld, 0, 0))
                        {
                            Projectile.timeLeft = 120;
                            Vector2 newMove = Main.MouseWorld - Projectile.Center;
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
            }*/
            if (player.releaseUseItem && releasetime >= 10 || player.HeldItem.type != ModContent.ItemType<Items.Weapons.SpectreDagger>())
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Projectile.velocity.X, Projectile.velocity.Y), ModContent.ProjectileType<SpectreDaggerProj2>(), Projectile.damage, 0, Projectile.owner);
                Projectile.Kill();
            }
            /*if (target)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (12 * Projectile.velocity + move) / 12.5f;
                AdjustMagnitude(ref Projectile.velocity);
            }   */          
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
           /* float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 20f)
            {
                vector *= 20f / magnitude;
            }*/
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 3; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 185);
                dust.noGravity = true;
            }
            //Projectile.damage += 12;
        }

        public override void Kill(int timeLeft)
        {
            //Main.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 185);
                dust.noGravity = true;
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
        /* public void AnimateProjectile() // Call this every frame, for example in the AI method.
         {
             Projectile.frameCounter++;
             if (Projectile.frameCounter >= 4) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
             {
                 Projectile.frame++;
                 Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                 Projectile.frameCounter = 0;
             }
         }*/
    }
    //_______________________________________________________________________________________________
    //_______________________________________________________________________________________________
    public class SpectreDaggerProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectre Dagger");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.light = 0.4f;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.penetrate = 2;

        }

        public override void AI()
        {
            Projectile.DamageType = DamageClass.Magic;

            var player = Main.player[Projectile.owner];

            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.Center;
            dust = Terraria.Dust.NewDustPerfect(position, 185, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;
            dust.scale = 1f;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;          
        }
       
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 3; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 185);
                dust.noGravity = true;
            }
            //Projectile.damage += 12;
        }


        public override void Kill(int timeLeft)
        {
            //Main.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 185);
                dust.noGravity = true;
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
