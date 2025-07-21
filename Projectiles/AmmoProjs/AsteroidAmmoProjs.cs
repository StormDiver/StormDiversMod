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
            //DisplayName.SetDefault("Asteroid Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.CanHitPastShimmer[Projectile.type] = true;
            ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
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
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Projectile.oldVelocity; //prevents accuracy from getting messed up
            Projectile.alpha = 255;
            //75% damage, 66% speed and knockback
            int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity * 0.66f, ModContent.ProjectileType<Projectiles.AmmoProjs.AsteroidBulletProj>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack * 0.66f, Projectile.owner);
            Main.projectile[projID].tileCollide = false; //only spawn one proj
            Main.projectile[projID].timeLeft = 90;
            Main.projectile[projID].ai[2] = 1; //new dust particles
            Main.projectile[projID].extraUpdates = Projectile.extraUpdates; //new dust particles

            return true;
        }
        public override void AI()
        {
            if (Projectile.ai[2] == 1)//create dust when in wall piercing mode
            {
                if (Main.rand.Next(2) == 0)
                Dust.NewDustDirect(new Vector2(Projectile.position.X - 2, Projectile.position.Y - 2), Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            /*int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(target.Center.X + 75, target.Center.Y - 300), new Vector2(-4 + target.velocity.X * 0.5f, 15), ModContent.ProjectileType<Projectiles.SpaceFragment>(), (int)(Projectile.damage * 0.2f), 0, Projectile.owner);
            Main.projectile[projID].DamageType = DamageClass.Ranged;
            Main.projectile[projID].penetrate = 1;
            Main.projectile[projID].ArmorPenetration = 50;

            int projID2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(target.Center.X - 75, target.Center.Y - 300), new Vector2(4 + target.velocity.X * 0.5f, 15), ModContent.ProjectileType<Projectiles.SpaceFragment>(), (int)(Projectile.damage * 0.2f), 0, Projectile.owner);
            Main.projectile[projID2].DamageType = DamageClass.Ranged;
            Main.projectile[projID2].penetrate = 1;
            Main.projectile[projID2].ArmorPenetration = 50;*/

            /*for (int i = 0; i < 25; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(target.Center.X + 75, target.Center.Y - 300), 0, 0, 6);
                dust.scale = 1.2f;
                dust.noGravity = true;
                dust.velocity *= 3;
                var dust2 = Dust.NewDustDirect(new Vector2(target.Center.X - 75, target.Center.Y - 300), 0, 0, 6);
                dust2.scale = 1.2f;
                dust2.noGravity = true;
                dust2.velocity *= 3;
            }*/
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[2] == 0)
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
           
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X - 2, Projectile.position.Y - 2), Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f);
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
            //DisplayName.SetDefault("Asteroid Arrow");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
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
               
                float projspeed = 35;
                Vector2 velocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                //Set the velocities to the shoot values
                Projectile.velocity.X = velocity.X;
                Projectile.velocity.Y = velocity.Y;
            }
            if (Projectile.ai[1] >= 45)
            {
                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;


                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 130, default, 1.2f);
                dust.noGravity = true;
            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 130, default, 1.2f);
            }
            Projectile.damage = (Projectile.damage * 9) / 10;
        }
        public override void OnKill(int timeLeft)
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
