using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{
    public class BloodyBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Bullet");      
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 90;
            AIType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.extraUpdates = 1;
            Projectile.light = 0.2f;

        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.ai[0] += 1f;
            if (Main.rand.Next(2) == 0 && Projectile.ai[0] > 5)
            {

                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 115, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                dust2.noGravity = true;


            }
            if ((Projectile.velocity.X >= 3 || Projectile.velocity.X <= -3))
            {
                if (Main.rand.Next(5) == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<BloodyBulletProj2>(), (int)(Projectile.damage * 0.33), 0.5f, Projectile.owner);
                }
            }

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
                dust2.noGravity = true;
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);

            }

        }
        public override Color? GetAlpha(Color lightColor)
        {



            return Color.White;

        }

    }

    //____________________________________________________________________________________________
    public class BloodyBulletProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Bullet Drop");
        }
        public override void SetDefaults()
        {

            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.aiStyle = 2;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 120;
            Projectile.knockBack = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        //bool bloodspray = true;
        public override void AI()
        {


            if (Main.rand.Next(8) == 0)
            {
                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 115, 0f, 0f, 0, new Color(255, 255, 255), 1.5f)];
                dust.noGravity = true;
            }


            if (Main.rand.Next(10) == 0)
            {
                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 115, 0f, 3f, 0, new Color(255, 255, 255), 1f)];
                dust.velocity *= 0;

            }


            return;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 115, 0f, 0f, 0, new Color(255, 255, 255), 1f)];

            }
        }
    }
}