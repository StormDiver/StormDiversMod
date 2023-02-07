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


namespace StormDiversMod.Projectiles.WhipProjs
{
	public class DesertWhipProj : ModProjectile
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
			Projectile.WhipSettings.Segments = 16;
			Projectile.WhipSettings.RangeMultiplier = 1.5f;
			Projectile.width = 18;
		}

		private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
       
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<WhiptagForbiddenDebuff>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Projectile.damage = (Projectile.damage * 8) / 10;

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
				Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.Black);
				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, linepos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				linepos += diff;
			}
		}
		public override void AI()
		{
			int dust = Dust.NewDust(linepos, Projectile.width, Projectile.height, 10, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1.2f);
			Main.dust[dust].noGravity = true; //this make so the dust has no gravity

			int dust2 = Dust.NewDust(linepos, Projectile.width, Projectile.height, 54, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 130, default, 0.7f);

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
				Rectangle frame = new Rectangle(0, 0, 18, 20);
				Vector2 origin = new Vector2(5, 8);
				float scale = 1;

				// Tip
				if (i == list.Count - 2)
				{
					frame.Y = 76;
					frame.Height = 24;

				}
				//Segment 3
				else if (i % 2 != 0 && i >= 1)
				{
				//else if (i == 1 || i == 3 || i == 5 || i == 7 || i == 9 || i == 11 || i == 13 || i == 15)
				//{
					frame.Y = 58;
					frame.Height = 14;
				}
				//segement 2
				else if (i % 2 == 0 && i >= 1)
				{
				//else if (i == 2 || i == 4 || i == 6 || i == 8 || i == 10 || i == 12 || i == 14 || i == 16)
				//{
					frame.Y = 40;
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
	//________________________________________________________________________________________
	public class DesertWhipProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forbidden Whip Dust");
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
			Projectile.extraUpdates = 3;
			Projectile.timeLeft = 120;
			Projectile.penetrate = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.ArmorPenetration = 15;
		}
		
		public override bool? CanHitNPC(NPC target)
		{
			if (target.GetGlobalNPC<NPCEffects>().forbiddenimmunetime > 0 || target.friendly) //Npcs immune to explosion when activating it
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		public override void AI()
		{
			if (!Main.dedServ)
			{
				Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
			}

			if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 10, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);

				Main.dust[dust].noGravity = true; //this make so the dust has no gravity
				Main.dust[dust].velocity *= 0.5f;
				int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 54, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
			}
			
			return;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
			Projectile.damage = (Projectile.damage * 8) / 10;
		}
		public override void OnHitPvp(Player target, int damage, bool crit)

		{
			target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}

	}
}
