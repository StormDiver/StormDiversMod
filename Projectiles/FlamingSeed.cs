using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{
   
    public class FlamingSeed : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flaming Seed");
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.scale = 0.8f;
        }
        public override void AI()
        {
            Projectile.rotation += (float)Projectile.direction * -0.5f;

            if (Main.rand.Next(2) == 0)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 127, 0f, 0f, 0, new Color(255, 255, 255), 1.5f);
                dust.noGravity = true;

            }
     

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SuperBurnDebuff>(), 300);
            Projectile.damage = (Projectile.damage * 9 / 10);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<SuperBurnDebuff>(), 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.Item10 with{Volume = 1f, Pitch = 0.4f}, Projectile.Center);
                for (int i = 0; i < 5; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 127);
                    dust.scale = 0.7f;
                }

            }
           
        }
        
    }
}
