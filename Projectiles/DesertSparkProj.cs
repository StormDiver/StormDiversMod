using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;

using Terraria.Utilities;

namespace StormDiversMod.Projectiles
{


	public class DesertSparkProj : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Desert Spark");
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 4;

        }
        public override void SetDefaults()
		{

			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.timeLeft = 25;
			Projectile.aiStyle = 14;
			Projectile.ArmorPenetration = 5;
		}
        public override bool? CanDamage()
        {
            if (Projectile.timeLeft < 15)
                return true;
            else
                return false;
        }
        public override void AI()
		{
			Projectile.localAI[1]++;
			
			if (Projectile.localAI[1] >= 6)
			{
				for (int i = 0; i < 3; i++)
				{
					float speedY = -0.75f;

					Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

					int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 1f);
					Main.dust[dust2].noGravity = true;
				}

				Projectile.localAI[1] = 0;
			}

            Dust dust;
          
            dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 226, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;


            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{

		}
		public override bool OnTileCollide(Vector2 oldVelocity)

		{
			Projectile.Kill();
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Vector2 dustspeed = new Vector2(0, 0.75f).RotatedByRandom(MathHelper.ToRadians(360));

				int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 1f);
				Main.dust[dust2].noGravity = true;
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

	}
}
	

