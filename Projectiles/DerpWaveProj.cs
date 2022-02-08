using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles       
{
 
    public class DerpWaveProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Derpling Shockwave");
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 120;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 40;
            Projectile.extraUpdates = 1;
            Projectile.knockBack = 8f;
            //Projectile.usesLocalNPCImmunity = true;
            //Projectile.usesIDStaticNPCImmunity = true;
            
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = false;
        }
        float airknock = 13;
        public override void AI()
        {
            Projectile.damage = (Projectile.damage * 100) / 101;
            airknock -= 0.14f;

            if (Projectile.ai[0] > 0f)  //this defines where the flames starts
            {
                if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
                {


                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Vector2 position = Projectile.position;
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 68, 0f, 0f, 100, default, 1.5f);

                    //Main.dust[dustIndex].scale = 0.5f + (float)Main.rand.Next(5) * 0.1f;
                    //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.friendly && target.lifeMax > 5 && !target.boss  && target.knockBackResist != 0f)
            {
                if (target.velocity.Y == 0)
                {
                    target.velocity.Y = -airknock - (target.knockBackResist * 2);
                    target.velocity.X = 4f * Projectile.direction;
                }
                else
                {
                    Projectile.knockBack = 20;
                    target.velocity.Y = (-airknock * 0.5f) - (target.knockBackResist * 2);
                    target.velocity.X = 7f * Projectile.direction;

                }
                target.AddBuff(ModContent.BuffType<DerpDebuff>(), 45);

                /*if (airknock > 0)
                {
                    airknock--;
                }*/
            }
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Projectile.Kill();
            return false;
        }
    }
}