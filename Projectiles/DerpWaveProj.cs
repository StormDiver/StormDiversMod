using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Buffs;
using StormDiversMod.Common;

namespace StormDiversMod.Projectiles       
{
 
    public class DerpWaveProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Shockwave");
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
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 1;
            Projectile.knockBack = 8f;
            Projectile.usesLocalNPCImmunity = true;            
            Projectile.localNPCHitCooldown = 30;
            Projectile.tileCollide = false;
        }
        float airknock = 13;
        public override void AI()
        {
            //Projectile.damage = (Projectile.damage * 99) / 100;
            Projectile.damage -= 2;
            airknock -= 0.14f;

            var player = Main.player[Projectile.owner];
            //increase in height
            if (player.gravDir == 1)
            {
                Projectile.height += 4;
                Projectile.position.Y -= 4;
            }
            else
            {
                Projectile.height += 4;
                //Projectile.position.Y += 4; grows from top, no need to move it down
            }

            Vector2 position = Projectile.position;
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 68, 0f, 0f, 100, default, 1.5f);

            Main.dust[dustIndex].noGravity = true;
            var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 203);

            dust2.noGravity = true;


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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
                //target.AddBuff(ModContent.BuffType<DerpDebuff>(), 60);
                target.GetGlobalNPC<NPCEffects>().derplaunched = true;


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