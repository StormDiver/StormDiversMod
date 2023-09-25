using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles.WhipProjs
{
	public class SpaceRockWhipProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// This makes the projectile use whip collision detection and allows flasks to be applied to it, also makes shadowflare armour work
			ProjectileID.Sets.IsAWhip[Type] = true;
		}
		public override void SetDefaults()
		{
			// This method quickly sets the whip's properties.
			Projectile.DefaultToWhip();

			// use these to change from the vanilla defaults
			Projectile.WhipSettings.Segments = 24;
			Projectile.WhipSettings.RangeMultiplier = 1.25f;
			Projectile.width = 18;
		}

		private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
       
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<WhiptagSpaceRockDebuff>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Projectile.damage = (Projectile.damage * 9) / 10;

		}

		// This method draws a line between all points of the whip, in case there's empty space between the sprites.
		Vector2 linepos;
		private void DrawLine(List<Vector2> list)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2 - 4, + 7);//offset tip

			linepos = list[0];
			for (int i = 0; i < list.Count - 1; i++)
			{
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.MediumPurple);
				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, linepos - Main.screenPosition, frame, Color.MediumPurple, rotation, origin, scale, SpriteEffects.None, 0);

				linepos += diff;
			}
		}
		public override void AI()
		{
			for (int i = 0; i < 2; i++)
			{
				var dust2 = Dust.NewDustDirect(linepos, Projectile.width, Projectile.height, 6);
				//int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1.5f);
				dust2.noGravity = true;
				dust2.scale = 1.5f;
				dust2.velocity *= 2;
				int dust3 = Dust.NewDust(linepos, Projectile.width, Projectile.height, 0, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default, 0.5f);
			}

		}
		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list);

			DrawLine(list);

			//Main.DrawWhip_WhipBland(Projectile, list);
			// The code below is for custom drawing.
			// If you don't want that, you can remove it all and instead call one of vanilla's DrawWhip methods, like above.
			// However, you must adhere to how they draw if you do.

			SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.instance.LoadProjectile(Type);
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 pos = list[0];

			for (int i = 0; i < list.Count - 1; i++)
			{
				// frame is for handle
				Rectangle frame = new Rectangle(0, 0, 16, 24);
				Vector2 origin = new Vector2(5, 12);
				float scale = 1.2f;

				// Tip
				if (i == list.Count - 2)
				{
					frame.Y = 62;
					frame.Height = 24;

				}
				//Segment 2
				else if (i % 2 != 0 && i >= 1)
				{
					frame.Y = 44;
					frame.Height = 16;
				}
				//segement 1
				else if (i % 2 == 0 && i >= 1)
				{
					frame.Y = 26;
					frame.Height = 16;
				}
				
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
				Color color = Lighting.GetColor(element.ToTileCoordinates());

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, Color.White, rotation, origin, scale, flip, 0);

				pos += diff;



			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}

	//__________________________________________________________________________________________________________________________________________________
	public class SpaceRockWhipProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Asteroid Whip Fragment");
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		}
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.light = 0.1f;
			Projectile.friendly = true;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Summon;
			// Projectile.aiStyle = 1;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 120;
			Projectile.penetrate = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.ArmorPenetration = 40;

		}
		int rotate;
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 25; i++)
            {

                var dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 6);
                dust.scale = 1.2f;
                dust.noGravity = true;
                dust.velocity *= 3;
            }
            base.OnSpawn(source);
        }
        public override void AI()
		{
			var player = Main.player[Projectile.owner];

			if (Projectile.position.Y > (player.position.Y))
			{
				Projectile.tileCollide = true;
			}
			else
			{
				Projectile.tileCollide = false;
			}
			rotate += 2;
			Projectile.rotation = rotate * 0.1f;
			if (!Main.dedServ)
			{
				Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
			}

			if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
				Main.dust[dust].noGravity = true; //this make so the dust has no gravity
				Main.dust[dust].velocity *= -0.3f;
				int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 0, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
				Main.dust[dust2].noGravity = true; //this make so the dust has no gravity
				Main.dust[dust2].velocity *= -0.3f;

			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{

		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{

				var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 0, 0, 0, 130, default, 0.5f);
				var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 130, default, 1f);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;

		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}
