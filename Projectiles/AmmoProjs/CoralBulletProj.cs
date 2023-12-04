using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles.AmmoProjs
{
    
    public class CoralBulletProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Coral Bullet");

        }
        public override void SetDefaults()
        {

            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = 1;
            //aiType = ProjectileID.WoodenArrowFriendly;
            Projectile.ignoreWater = true;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            AIType = ProjectileID.Bullet;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            if (Main.rand.Next(4) == 0)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 33, 0f, 0f, 0, new Color(255, 255, 255), 0.75f);
                dust.noGravity = true;

            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Wet, 300);
            SoundEngine.PlaySound(SoundID.Item85, Projectile.Center);
            for (int i = 0; i < 5; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 25);
                dust.scale = 0.7f;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(BuffID.Wet, 300);
            }
            base.OnHitPlayer(target, info);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Item85, Projectile.Center);
                for (int i = 0; i < 5; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 25);
                    dust.scale = 0.7f;
                }
            }         
        }       
    }
    //________________________________________________________ 
}
