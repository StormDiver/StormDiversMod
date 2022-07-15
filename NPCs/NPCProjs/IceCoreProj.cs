using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
using Terraria.Audio;



namespace StormDiversMod.NPCs.NPCProjs
{

    
    public class IceCoreProj: ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Spike");
        }
        public override void SetDefaults()
        {
            Projectile.coldDamage = true;

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 2;
           
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 14;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            //drawOffsetX = -2;
            //drawOriginOffsetY = -2;
        }
        bool dustspawn = false;
        public override void AI()
        {
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 0.7f);
            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex].noGravity = true;

            
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (!dustspawn)
            {
                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
                    dust.velocity *= 2;
                }
                dustspawn = true;
            }
        }

       
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.SuperFrostBurn>(), 180);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void Kill(int timeLeft)
        {
           
                SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
                }

            
        }
    }
    //___________________________________________________________________________________________________________________________________
    //___________________________________________________________________________________________________________________________________
    //___________________________________________________________________________________________________________________________________

}