using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.NPCs.NPCProjs
{
    public class ThePainSlimeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pain slime orb");
            Main.projFrames[Projectile.type] = 1;

        }
        public override void SetDefaults()
        {

            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Main.rand.Next(5) == 0)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 119, Projectile.velocity.X * 1, Projectile.velocity.Y * 1, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;
                dust.alpha = 100;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
                target.AddBuff(BuffID.Slimed, 300);
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
                SoundEngine.PlaySound(SoundID.NPCDeath1 with { Volume = 1f }, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 119);
                    dust.alpha = 100;
                }

            }
        }
    }

    //________________________________________________________
    public class TheClaySlimeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Clay slime orb");
            Main.projFrames[Projectile.type] = 1;

        }
        public override void SetDefaults()
        {

            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Main.rand.Next(5) == 0)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 236, Projectile.velocity.X * 1, Projectile.velocity.Y * 1, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;
                dust.alpha = 100;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Slimed, 300);
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

                SoundEngine.PlaySound(SoundID.NPCDeath1 with { Volume = 1f }, Projectile.Center);
                for (int i = 0; i < 10; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 236);
                    dust.alpha = 100;
                }

            }
        }
    }

}
