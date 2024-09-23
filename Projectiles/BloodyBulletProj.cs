using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using StormDiversMod.Items.Weapons;
using StormDiversMod.Common;

namespace StormDiversMod.Projectiles
{
    public class BloodyBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloody Bullet");      
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            
            Projectile.friendly = true;
            Projectile.penetrate = 1;
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
                    //Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<BloodyBulletProj2>(), (int)(Projectile.damage * 0.33), 0.5f, Projectile.owner);
                }
            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.GetGlobalNPC<NPCEffects>().bloodimmunetime = 10; //target immune to explosion for 10 frames

            float numberProjectiles = 2 + Main.rand.Next(3); //2-4

            for (int i = 0; i < numberProjectiles; i++)
            {
                //Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f).RotatedByRandom(MathHelper.ToRadians(20));
                Vector2 perturbedSpeed = new Vector2(0, -6).RotatedByRandom(MathHelper.ToRadians(120));

                float scale = 1f - (Main.rand.NextFloat() * .2f);
                perturbedSpeed = perturbedSpeed * scale;

                int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BloodyBulletProj2>(), Projectile.damage / 3, 1, Projectile.owner);
                Main.projectile[projID].DamageType = DamageClass.Ranged;

            }
            for (int i = 0; i < 30; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
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
        public override void OnKill(int timeLeft)
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
            //DisplayName.SetDefault("Blood Bullet Drop");
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
            Projectile.penetrate = 3;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 180;
            Projectile.knockBack = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.ArmorPenetration = 5;
        }
        //bool bloodspray = true;
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            return true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<NPCEffects>().bloodimmunetime > 0 || target.friendly) //Npcs immune to blood created from them
                return false;
            
            else
                return true;
        }
        public override void AI()
        {
            Projectile.ai[2]++;

            if (Main.rand.Next(5) == 0)
            {
                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 115, 0f, 3f, 0, new Color(255, 255, 255), 1f)];
                dust.velocity *= 0;

            }
            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.Center;
                dust = Terraria.Dust.NewDustPerfect(position, 115, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1.25f);

                dust.noGravity = true;
            }

            return;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            return false;
        }
        public override void OnKill(int timeLeft)
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