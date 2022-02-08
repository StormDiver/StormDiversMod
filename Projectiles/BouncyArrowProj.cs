﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace StormDiversMod.Projectiles
{
    public class BouncyArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bouncy Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = 1;
            
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            
            
            Projectile.tileCollide = true;
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.arrow = true;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            

        }
        int reflect = 5;
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;

            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();
            }
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X *0.8f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y *0.8f;
                }
            }
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 56);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            Projectile.damage = (Projectile.damage * 8) / 10;

            Projectile.velocity.X = Projectile.velocity.X * -0.6f;

            Projectile.velocity.Y = Projectile.velocity.Y * -0.6f;
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 56);
        }
       
        public override void Kill(int timeLeft)
        {
            
             //int item = Main.rand.NextBool(5) ? Item.NewItem(Projectile.getRect(), ModContent.ItemType<Ammo.BouncyArrow>()) : 0;
             Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
             SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 3; i++)
            {

                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 100, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }

        }
    }
}
