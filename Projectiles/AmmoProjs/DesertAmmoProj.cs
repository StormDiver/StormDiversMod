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
    public class DesertBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Bullet");
          
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            AIType = ProjectileID.Bullet;
            Projectile.aiStyle = 1;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.extraUpdates = 1;
            Projectile.light = 0.3f;
        }
        public override void AI()
        {
            /*Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);*/
            Projectile.spriteDirection = Projectile.direction;

           
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<DesertArrowDust>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
            Main.projectile[projID].timeLeft = 600;
            Main.projectile[projID].penetrate = 3;

            for (int i = 0; i < 10; i++)
            {           
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 54);
                dust2.noGravity = true;
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {


            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

        }
        public override Color? GetAlpha(Color lightColor)
        {



            return Color.White;

        }

    }
   
    //____________________________________________________________________________
    public class DesertArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.aiStyle = 1;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 1;
            Projectile.arrow = true;
            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.arrow = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            DrawOffsetX = -4;
            DrawOriginOffsetY = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {     
            return true;
        }

        public override void AI()
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] == 1)
            {
                Projectile.velocity *= 0.75f;
            }
                if (Projectile.ai[1] > 30 && Projectile.ai[1] < 60)
            {
                Projectile.velocity *= 1.06f;
            }
            if (Projectile.ai[1] == 30)
            {
                Projectile.penetrate = 3;
                SoundEngine.PlaySound(SoundID.Item34, Projectile.Center);

                Projectile.damage = (Projectile.damage * 11) / 10;
            }
            if (Projectile.ai[1] >= 30)
            {

                Projectile.aiStyle = 0;

                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 10, 0f, 0f, 100, default, 0.7f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 54);
                dust2.noGravity = true;
            }
        }

    }
    
}