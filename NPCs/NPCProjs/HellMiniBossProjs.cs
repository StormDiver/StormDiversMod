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
    public class HellMiniBossProj1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("HellSoul Bolt");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            Main.projFrames[Projectile.type] = 4;

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
            Projectile.scale = 1f;


            Projectile.extraUpdates = 0;
            
           
            Projectile.timeLeft = 180;
            //aiType = ProjectileID.LostSoulHostile;
            Projectile.aiStyle = 0;
            // Projectile.CloneDefaults(452);
            //aiType = 452;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }

        public override void AI()
        {
            Projectile.ai[0]++;

            if (Projectile.ai[0] <= 20)
            {
                Projectile.velocity.X *= 1.06f;
                Projectile.velocity.Y *= 1.06f;

            }



            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f, 0, new Color(255, 255, 255), 1f)];
            dust.noGravity = true;
            dust.scale = 0.8f;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            AnimateProjectile();


        }
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            for (int i = 0; i < 15; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 173, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 130, default, 1.2f);
                dust.noGravity = true;
            }
            target.AddBuff(ModContent.BuffType<Buffs.HellSoulFireDebuff>(), 300);

        }
        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.NPCDeath6 with{Volume = 0.75f}, Projectile.Center);

            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 173);
                dust.scale = 2;
                dust.velocity *= 2;
            }

        }
        /*public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)  //this make the Projectile sprite rotate perfectaly around the player
        {

            Vector2 drawOrigin = new Vector2(Main.ProjectileTexture[Projectile.type].Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                spriteBatch.Draw(Main.ProjectileTexture[Projectile.type], drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

            }
            return true;

        }*/
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;
        }

    }
    //_________________________________________________________________
    public class HellMiniBossProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("HellSoul Giant Flame");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            Main.projFrames[Projectile.type] = 4;

        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;


            Projectile.light = 0.4f;

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = 1;


            Projectile.scale = 1.25f;


            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;

            Projectile.timeLeft = 300;
            //aiType = ProjectileID.LostSoulHostile;
            Projectile.aiStyle = -1;
            // Projectile.CloneDefaults(452);
            //aiType = 452;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }
        float speed = 5;
        float inertia = 30;
        float distanceToIdlePosition; //distance to player
        public override void AI()
        {

            Projectile.ai[0]++;

            Projectile.rotation = Projectile.velocity.X / 20;

            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 173, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
            dust.noGravity = true;
            dust.fadeIn = 1f;
            if (Projectile.ai[0] >= 30 && Projectile.ai[0] <= 180)
            {
               
                    //Player target = Main.player;
                    Vector2 idlePosition = Main.LocalPlayer.Center;
                    Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
                    distanceToIdlePosition = vectorToIdlePosition.Length();

                    //if (Collision.CanHit(Projectile.Center, 0, 0, Main.MouseWorld, 0, 0))
                    {
                        if (distanceToIdlePosition > 10f)
                        {
                            vectorToIdlePosition.Normalize();
                            vectorToIdlePosition *= speed;
                        }
                        Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                    }
                
            }
            //Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            /*if (Projectile.ai[0] >= 30 && Projectile.ai[0] <= 180)
            {

                for (int i = 0; i < 100; i++)
                {
                    Player target = Main.player[i];
                    
                    //If the distance between the live targeted npc and the Projectile is less than 480 pixels
                    if (Vector2.Distance(target.Center, Projectile.Center) < 750f && target.active)
                    {                       
                        float projspeed = 4f;
                        Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                        //Set the velocities to the shoot values
                        Projectile.velocity.X = velocity.X;
                        Projectile.velocity.Y = velocity.Y;
                    }                                     
                }               
            }*/
            AnimateProjectile();

        }
        /* private void AdjustMagnitude(ref Vector2 vector)
         {
             float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
             if (magnitude > 4f)
             {
                 vector *= 4f / magnitude;
             }
         }*/
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Buffs.HellSoulFireDebuff>(), 300);

            Projectile.Kill();
        }
        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.NPCDeath6 with{Volume = 0.75f}, Projectile.Center);


            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 173);
                dust.scale = 2;
                dust.velocity *= 2;
            }

        }
        /*  public override bool PreDraw(ref Color lightColor)
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

        }*/
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;
        }

    }
}
