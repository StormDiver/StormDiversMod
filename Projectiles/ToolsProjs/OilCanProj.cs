using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StormDiversMod.Common;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace StormDiversMod.Projectiles.ToolsProjs   
{
   
    public class OilCanProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Oil Stream");
        }

        public override bool? CanDamage()
        {
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            //Projectile.knockBack = 8f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {

            if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
            {

                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 240, 0f, 0f, 0, default, 1);

                Main.dust[dustIndex].fadeIn = 1f;
                Main.dust[dustIndex].noGravity = true;

                Dust dust;
                dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 240, new Vector2(0f, 0f), 0, new Color(255, 100, 0), 1f);
                dust.fadeIn = 1;
                dust.noGravity = true;

                //var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 109, 0f, 0f, 150);

                //dust2.noGravity = true;

            }
           
            for (int i = 0; i < 200; i++)//for applying oil to enemies
            {
                NPC target = Main.npc[i];

                float npcdistance = Vector2.Distance(target.Center, Projectile.Center);

                if (npcdistance <= Projectile.width / 2 + target.width / 2 && target.active && target.lifeMax >= 5 && !target.dontTakeDamage)
                {
                    target.AddBuff(BuffID.Oiled, Main.rand.Next(900, 1201)); //15-20 seconds
                    Projectile.Kill();
                }
            }

            return;
        }
        
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Item86 with { Volume = 1f, Pitch = -2 }, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 240);
                    dust.scale = 1.5f;
                }

            }
        }
    }
    
}