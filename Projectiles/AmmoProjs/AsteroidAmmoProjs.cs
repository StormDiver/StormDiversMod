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
    public class AsteroidBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asteroid Bullet");
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
            Projectile.penetrate = 1;
            
            
            Projectile.tileCollide = true;


            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
           
            return true;
        }
        public override void AI()
        {
        
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(target.Center.X, target.Center.Y - 400), new Vector2(target.velocity.X * 0.5f, 10), ModContent.ProjectileType<Projectiles.SpaceFragment>(), (int)(Projectile.damage * 0.15f), Projectile.knockBack / 2, Projectile.owner);
            Main.projectile[projID].DamageType = DamageClass.Ranged;
            Main.projectile[projID].penetrate = 1;
            Main.projectile[projID].ArmorPenetration = 50;

            for (int i = 0; i < 25; i++)
            {

                var dust = Dust.NewDustDirect(new Vector2(target.Center.X, target.Center.Y - 400), 0, 0, 6);
                dust.scale = 1.2f;
                dust.noGravity = true;
                dust.velocity *= 3;
            }
        }

        public override void Kill(int timeLeft)
        {

            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f);
            }

        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
    //______________________________________________________________________________________
    public class AsteroidArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asteroid Arrow");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.aiStyle = 0;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.arrow = true;
            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.arrow = true;
            AIType = ProjectileID.WoodenArrowFriendly;
            //Creates no immunity frames
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            DrawOffsetX = 0;
            Projectile.gfxOffY = -11;
        }

        public override void AI()
        {
            Projectile.ai[1]++;

            if (Projectile.ai[1] < 45)
            {
                Projectile.velocity *= 0.9f;

            }         
            if (Projectile.ai[1] < 45)
            {
                Projectile.rotation = (Main.MouseWorld - Projectile.Center).ToRotation() + 1.57f;
            }
            if (Projectile.ai[1] == 45)
            {
                SoundEngine.PlaySound(SoundID.Item13, Projectile.position);

                Projectile.penetrate = -1;
                //Get the shoot trajectory from the Projectile and target
                float shootToX = Main.MouseWorld.X - Projectile.Center.X;
                float shootToY = Main.MouseWorld.Y - Projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                //If the distance between the live targeted npc and the Projectile is less than 480 pixels


                distance = 0.5f / distance;

                //Multiply the distance by a multiplier proj faster
                shootToX *= distance * 70f;
                shootToY *= distance * 70f;

                //Set the velocities to the shoot values
                Projectile.velocity.X = shootToX;
                Projectile.velocity.Y = shootToY;
            }
            if (Projectile.ai[1] >= 45)
            {
                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;


                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 130, default, 1.2f);
                dust.noGravity = true;
            }

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 130, default, 1.2f);
            }
            Projectile.damage = (Projectile.damage * 9) / 10;

        }

        public override void Kill(int timeLeft)
        {

            //int item = Main.rand.NextBool(5) ? Item.NewItem(Projectile.getRect(), ModContent.ItemType<Ammo.ShroomArrow>()) : 0;
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
           
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[1] < 45)
            {
                return false;
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
                return true;
            }
        }
   
        public override bool PreDraw(ref Color lightColor)
        {
            //if (Projectile.ai[1] >= 45)
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
            }
            return true;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
  
}
