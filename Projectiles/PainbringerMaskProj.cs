using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles
{
    
    public class PainbringerMaskProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pain Crystals");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 22;
            //Projectile.light = 1f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 50;
            Projectile.timeLeft = 999999999;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];

            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
            }
        }
        public override void AI()
        {
            //Dust.QuickDust(Projectile.Center, Color.DeepPink);
            /*Dust dust;
            dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 177, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1.25f);
            dust.noGravity = true;*/

            if (Main.rand.Next(8) == 0)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 72, 0, 0, 100, default, 0.75f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;

            }
            Player player = Main.player[Projectile.owner];
            //Sets location and direction when spawned

            if (Projectile.ai[1] == 0) //far left
            {
                Projectile.position.X = player.Center.X - 30 - Projectile.width / 2;
                Projectile.position.Y = player.Center.Y - 5 * player.gravDir - Projectile.height / 2;
            }
            else if (Projectile.ai[1] == 1) //left
            {
                Projectile.position.X = player.Center.X - 20 - Projectile.width / 2;
                Projectile.position.Y = player.Center.Y - 30 * player.gravDir - Projectile.height / 2;
            }
            else if(Projectile.ai[1] == 2) //right
            {
                Projectile.position.X = player.Center.X + 20 - Projectile.width / 2;
                Projectile.position.Y = player.Center.Y - 30 * player.gravDir - Projectile.height / 2;
            }
            else if (Projectile.ai[1] == 3)//far right
            {
                Projectile.position.X = player.Center.X + 30 - Projectile.width / 2;
                Projectile.position.Y = player.Center.Y - 5 * player.gravDir - Projectile.height / 2;

            }

            //Main.NewText("FFS" + Projectile.ai[1], 220, 63, 139);

            Projectile.rotation = 0;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;

            }
            if (Projectile.frame >= 5)
            {
                Projectile.frame = 0;
            }
            //Projectile.ai[0] += 1f;
            if ((player.armor[0].type != ModContent.ItemType<Items.Vanitysets.BossMaskUltimateBoss>() && player.armor[10].type != ModContent.ItemType<Items.Vanitysets.BossMaskUltimateBoss>()) || player.dead)
            {
                Projectile.Kill();
                return;
            }

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void Kill(int timeLeft)
        {

            //SoundEngine.PlaySound(SoundID.NPCDeath59 with{Volume = 0.5f, Pitch = 0.5f}, Projectile.Center);

            for (int i = 0; i < 10; i++) 
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
            }
        }
       
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
