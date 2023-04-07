using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles.AmmoProjs
{
    public class ShroomBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroomite Bullet");
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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
            //DisplayName.SetDefault("Shroomite Arrow");
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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
   

}
