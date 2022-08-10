using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using StormDiversMod.Basefiles;
namespace StormDiversMod.Projectiles       
{
   
    public class StompBootProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shockwave");
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            //Projectile.magic = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 40;
            Projectile.extraUpdates = 1;
            Projectile.knockBack = 8f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Generic;
        }

        public override void AI()
        {
            Projectile.damage = (Projectile.damage * 100) / 101;
            if (Projectile.ai[0] > 0f)  //this defines where the flames starts
            {
                if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
                {

                    
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Vector2 position = Projectile.position;
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.Bottom.Y - 10), Projectile.width, 12, 31, 0f, 0f, 100, default, 100f);
                    
                    Main.dust[dustIndex].scale = 0.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 203);
                 
                    dust2.noGravity = true;


                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            return;
        }
        
       
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
    public class StompBootProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stompwave");
        }
        public override void SetDefaults()
        {

            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            //Projectile.magic = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 9999;
            Projectile.extraUpdates = 0;
            //Projectile.knockBack = 2f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
            Projectile.DamageType = DamageClass.Generic;
        }

        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            Projectile.position.X = player.Center.X - 20;
            if (player.gravDir == 1)
            {
                Projectile.position.Y = player.Center.Y + 0;
            }
            else
            {
                Projectile.position.Y = player.Center.Y - 40;
            }
            Projectile.knockBack = 6;
            Projectile.velocity.X = player.direction; //knocks enemies in the direction facing
            if (player.GetModPlayer<EquipmentEffects>().falling == false || player.dead || !player.controlDown)
            {
                Projectile.Kill();
            }
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.Center.Y - 20), Projectile.width, 20, 31, 0f, 0f, 100, default, 1.5f);
            Main.dust[dustIndex].noGravity = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
    
}