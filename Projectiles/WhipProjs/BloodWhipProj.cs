using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles.WhipProjs
{
	public class BloodWhipProj : ModProjectile
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
			Projectile.width = 18;
		}

		private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
       
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<WhiptagBloodDebuff>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Projectile.damage = (Projectile.damage * 7) / 10;

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
				Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.DarkRed);
				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, linepos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				linepos += diff;
			}
		}
		public override void AI()
		{
			//int dust2 = Dust.NewDust(linepos, Projectile.width, Projectile.height, 115, 0, 0, 0, default, 0.7f);
		}
        public override void EmitEnchantmentVisualsAt(Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            int dust2 = Dust.NewDust(boxPosition, boxWidth, boxHeight, 115, 0, 0, 0, default, 0.7f);
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
					frame.Height = 20;

				}
				//Segment 2
				else if (i % 2 != 0 && i >= 1)
				{
					frame.Y = 42;
					frame.Height = 14;
				}
				//segement 1
				else if (i % 2 == 0 && i >= 1)
				{
					frame.Y = 26;
					frame.Height = 14;
				}			

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
	public class BloodWhipProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bloody whip Orb");
		}
		public override void SetDefaults()
		{

			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.ignoreWater = true;
			Projectile.scale = 0.75f;
			Projectile.tileCollide = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft = Main.rand.Next(180, 210);
			Projectile.ArmorPenetration = 10;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
		public override bool? CanDamage()
		{
			return true;
		}
		int direction;
        public override void OnSpawn(IEntitySource source)
        {
			if (Main.rand.Next(2) == 0)
            {
				direction = -6;
            }
			else
            {
				direction = 6;
            }
			for (int i = 0; i < 10; i++)
			{
				Dust dust;
				// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
				Vector2 position = Projectile.position;
				dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 115, 0f, 0f, 0, new Color(255, 255, 255), 0.75f)];

			}
		}
		int distance = 40;
        public override void AI()
		{
			if (Projectile.timeLeft < 150)
			{
                Projectile.ai[1] += 1;
                distance++;
			}

			Projectile.rotation += (float)Projectile.direction * -0.3f;

			if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
			{
				Dust dust;
				// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
				Vector2 position = Projectile.position;
				dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 115, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
				dust.noGravity = true;
			}


			if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
			{
				Dust dust;
				// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
				Vector2 position = Projectile.Center;
				dust = Terraria.Dust.NewDustPerfect(position, 5, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);


			}
			Player player = Main.player[Projectile.owner];
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				if (Projectile.timeLeft > 1)
				{
					//Factors for calculations
					double deg = (((double)Projectile.ai[1]) * direction) + 90; //The degrees, you can multiply Projectile.ai[1] to make it orbit faster, may be choppy depending on the value
					double rad = deg * (Math.PI / 180); //Convert degrees to radians
					double dist = distance + (npc.width / 2); //Distance away from the npc, half the wdith plus 40

					Projectile.position.X = npc.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
					Projectile.position.Y = npc.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

					//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
					Projectile.ai[1] += 1;
				}
				else
                {
					//Projectile.position = npc.Center; //keep? NAH
                }
			}
			else
            {
				Projectile.Kill();
            }
			if (Projectile.timeLeft == 2 || Projectile.timeLeft == Projectile.timeLeft - 1)
            {
				for (int i = 0; i < 10; i++)
				{
					Dust dust;
					// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
					Vector2 position = Projectile.position;
					dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 115, 0f, 0f, 0, new Color(255, 255, 255), 0.75f)];

				}
			}
		}
        public override void OnKill(int timeLeft)
        {
			for (int i = 0; i < 10; i++)
			{
				Dust dust;
				// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
				Vector2 position = Projectile.position;
				dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 115, 0f, 0f, 0, new Color(255, 255, 255), 0.75f)];

			}
		}
    }
}
