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
    public class EnchantedSwordProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Sword");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.light = 0.6f;
            Projectile.friendly = true;
            Projectile.penetrate = 5;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
          
            DrawOffsetX = 2;
            DrawOriginOffsetY = -10;
        }
        int speedup = 0;
        
        public override void AI()
        {
            /*Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);*/
            Projectile.spriteDirection = Projectile.direction;

            speedup++;
            if (speedup < 60)
            {
                Projectile.rotation = (0.4f * speedup);
            }
            if (speedup == 60)
            {
               
               
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 30);
                for (int i = 0; i < 10; i++)
                {

                     
                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15);
                }
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
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(shootToX, shootToY), ModContent.ProjectileType<EnchantedSwordProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile.Kill();
                    
                }
                    
                    
                

            }
            
           
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15);
                //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 60);
            }
            Projectile.damage = (Projectile.damage * 8) / 10;

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);

            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15);
                dust.noGravity = true;
            }
            return true;
        }

        public override void Kill(int timeLeft)
        {

/*
            Main.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);

            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15);
            }*/

        }
        

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
       
    }
    //___________________________________________________________________________________________
    public class EnchantedSwordProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Sword");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.light = 0.3f;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 200;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 2;
            DrawOriginOffsetY = -10;
        }
        
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            //Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);
            Projectile.spriteDirection = Projectile.direction;

           
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15);
                
            }
            Projectile.damage = (Projectile.damage * 9) / 10;

           
        }
       
        int reflect = 5;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();
            }

            {
                /*Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 1f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 1f;
                }*/

                if (Projectile.owner == Main.myPlayer)
                {
                    
                    float shootToX = Main.MouseWorld.X - Projectile.Center.X;
                    float shootToY = Main.MouseWorld.Y - Projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                    bool lineOfSight = Collision.CanHitLine(Main.MouseWorld, 0, 0, Projectile.position, Projectile.width, Projectile.height);

                    distance = 3f / distance;
                    shootToX *= distance * 7;
                    shootToY *= distance * 7;
                    if (lineOfSight)
                    {
                        Projectile.velocity.X = shootToX;
                        Projectile.velocity.Y = shootToY;
                    }
                    else
                    {
                        if (Projectile.velocity.X != oldVelocity.X)
                        {
                            Projectile.velocity.X = -oldVelocity.X * 1f;
                        }
                        if (Projectile.velocity.Y != oldVelocity.Y)
                        {
                            Projectile.velocity.Y = -oldVelocity.Y * 1f;
                        }
                    }

                }
                if (reflect > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 8);
                }
                return false;
            }


        }
        
       



        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);
            for (int i = 0; i < 30; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15);
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