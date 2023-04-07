using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;



namespace StormDiversMod.NPCs.NPCProjs
{
    public class MoonDerpBoltProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moonling Bolt");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;

        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            
            Projectile.light = 0.4f;
            
            Projectile.friendly = false;
            Projectile.hostile = true;
            
            Projectile.penetrate = 1;
            
            
            Projectile.tileCollide = true;
            Projectile.scale = 1.1f;
            Projectile.tileCollide = false;


            Projectile.extraUpdates = 1;
            
           
            Projectile.timeLeft = 600;
            //aiType = ProjectileID.LostSoulHostile;
            Projectile.aiStyle = 0;
            // Projectile.CloneDefaults(452);
            //aiType = 452;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }
        
       
        
        
        public override void AI()
        {
            
          
            
          
          

            for (int i = 0; i < 10; i++)
            {
                float X = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                float Y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;


                int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 229, 0, 0, 100, default, 1f);
                Main.dust[dust].position.X = X;
                Main.dust[dust].position.Y = Y;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0f;

            }

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            
         
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            for (int i = 0; i < 15; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 229, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 130, default, 1.2f);
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {

            Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.NPCDeath55, Projectile.Center);
            for (int i = 0; i < 15; i++)
            {

                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 229, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
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
    //_________________________________________________________________
    public class MoonDerpEyeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moonling Eye");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;

        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;


            Projectile.light = 0.4f;

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = 1;


            Projectile.scale = 1.1f;


            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;

            Projectile.timeLeft = 600;
            //aiType = ProjectileID.LostSoulHostile;
            Projectile.aiStyle = -1;
            // Projectile.CloneDefaults(452);
            //aiType = 452;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }

        public override void AI()
        {

            Projectile.ai[0]++;

            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 229, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
            dust.noGravity = true;
            dust.fadeIn = 1f;


            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
           
            if (Projectile.ai[0] >= 180)
            {

                for (int i = 0; i < 100; i++)
                {
                    Player target = Main.player[i];              
                    
                    if (Vector2.Distance(target.Center, Projectile.Center) < 2000 && Projectile.ai[0] >= 120)
                    {
                        float projspeed = 9f;
                        Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                        //Set the velocities to the shoot values
                        Projectile.velocity.X = velocity.X;
                        if (Projectile.velocity.Y < 6)
                        {
                            Projectile.velocity.Y += 0.1f;
                        }
                    }
                }             
            }
        }
        /* private void AdjustMagnitude(ref Vector2 vector)
         {
             float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
             if (magnitude > 4f)
             {
                 vector *= 4f / magnitude;
             }
         }*/
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {

            Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.NPCDeath43, Projectile.Center);
            for (int i = 0; i < 40; i++)
            {

                int dust2 = Dust.NewDust(Projectile.Center - Projectile.velocity, Projectile.width, Projectile.height, 229, 0f, 0f, 200, default, 1.2f);
                Main.dust[dust2].velocity *= -5f;
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].fadeIn = 1f;

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
