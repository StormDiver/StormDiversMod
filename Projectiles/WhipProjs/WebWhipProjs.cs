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
	public class WebWhipProj : ModProjectile
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
			Projectile.WhipSettings.Segments = 12;
			Projectile.WhipSettings.RangeMultiplier = 1f;
			Projectile.width = 12;
		}

		private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
       
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<WhiptagWebDebuff>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Projectile.damage = (Projectile.damage * 5) / 10;

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
				Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.White);
				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, linepos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				linepos += diff;
			}
		}
		public override void AI()
		{
			//int dust = Dust.NewDust(linepos, Projectile.width, Projectile.height, 10, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1.2f);
			//Main.dust[dust].noGravity = true; //this make so the dust has no gravity

			int dust2 = Dust.NewDust(linepos, Projectile.width, Projectile.height, 31, 0, 0, 0, default, 0.7f);

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
				Rectangle frame = new Rectangle(0, 0, 18, 24);
				Vector2 origin = new Vector2(5, 8);
				float scale = 1;

				// Tip
				if (i == list.Count - 2)
				{
					frame.Y = 60;
					frame.Height = 18;

				}
				//Segment 3
				else if (i % 2 != 0 && i >= 1)
				{
					frame.Y = 42;
					frame.Height = 14;
				}
				//segement 2
				else if (i % 2 == 0 && i >= 1)
				{
					frame.Y = 26;
					frame.Height = 14;
				}
				//segment 1
				/*else if (i > 0)
				{
					frame.Y = 24;
					frame.Height = 14;
				}*/

				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
				Color color = Lighting.GetColor(element.ToTileCoordinates());

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

				pos += diff;



			}
			return false;
		}
	}
	//_______________________________________________
	
}
