using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System;
using ReLogic.Content;

//Credit to the clicker class for all of this, 

namespace StormDiversMod.Basefiles
{
		public class DrawLayerData
		{
			public static Color DefaultColor() => new Color(255, 255, 255, 0) * 0.7f;

			public Asset<Texture2D> Texture { get; init; }

			public Func<Color> Color { get; init; } = DefaultColor;
		}
	

	public sealed class HeadLayer : PlayerDrawLayer
	{
		private static Dictionary<int, DrawLayerData> HeadLayerData { get; set; }

		
		public static void RegisterData(int headSlot, DrawLayerData data)
		{
			if (!HeadLayerData.ContainsKey(headSlot))
			{
				HeadLayerData.Add(headSlot, data);
			}
		}

		public override void Load()
		{
			HeadLayerData = new Dictionary<int, DrawLayerData>();
		}

		public override void Unload()
		{
			HeadLayerData = null;
		}

		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Head);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || drawPlayer.invis || drawPlayer.head == -1)
			{
				return false;
			}

			return true;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!HeadLayerData.TryGetValue(drawPlayer.head, out DrawLayerData data))
			{
				return;
			}

			Color color = drawPlayer.GetImmuneAlphaPure(data.Color(), drawInfo.shadow);

			Texture2D texture = data.Texture.Value;
			Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.bodyFrame.Width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition;
			Vector2 headVect = drawInfo.headVect;
			DrawData drawData = new DrawData(texture, drawPos.Floor() + headVect, drawPlayer.bodyFrame, color, drawPlayer.headRotation, headVect, 1f, drawInfo.playerEffect, 0)
			{
				shader = drawInfo.cHead
			};
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
	//___________________________________________________

	public class BodyGlowmaskPlayer : ModPlayer
	{
		private static Dictionary<int, Func<Color>> BodyColor { get; set; }

		public static void RegisterData(int bodySlot, Func<Color> color)
		{
			if (!BodyColor.ContainsKey(bodySlot))
			{
				BodyColor.Add(bodySlot, color);
			}
		}

		public override void Load()
		{
			BodyColor = new Dictionary<int, Func<Color>>();
		}

		public override void Unload()
		{
			BodyColor = null;
		}

		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
		{
			if (!BodyColor.TryGetValue(Player.body, out Func<Color> color))
			{
				return;
			}

			drawInfo.bodyGlowColor = color();
			drawInfo.armGlowColor = color();
		}
	}


	//____________________________________________________
	public sealed class LegsLayer : PlayerDrawLayer
	{
		private static Dictionary<int, DrawLayerData> LegsLayerData { get; set; }

		
		public static void RegisterData(int legSlot, DrawLayerData data)
		{
			if (!LegsLayerData.ContainsKey(legSlot))
			{
				LegsLayerData.Add(legSlot, data);
			}
		}

		public override void Load()
		{
			LegsLayerData = new Dictionary<int, DrawLayerData>();
		}

		public override void Unload()
		{
			LegsLayerData = null;
		}

		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Leggings);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || drawPlayer.invis || drawPlayer.legs == -1)
			{
				return false;
			}
			return true;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!LegsLayerData.TryGetValue(drawPlayer.legs, out DrawLayerData data))
			{
				return;
			}

			Color color = drawPlayer.GetImmuneAlphaPure(data.Color(), drawInfo.shadow);

			Texture2D texture = data.Texture.Value;
			Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.legFrame.Width / 2, drawPlayer.height - drawPlayer.legFrame.Height + 4f) + drawPlayer.legPosition;
			Vector2 legsOffset = drawInfo.legsOffset;
			DrawData drawData = new DrawData(texture, drawPos.Floor() + legsOffset, drawPlayer.legFrame, color, drawPlayer.legRotation, legsOffset, 1f, drawInfo.playerEffect, 0)
			{
				shader = drawInfo.cLegs
			};
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
	//________________________________
	public sealed class WingsLayer : PlayerDrawLayer
	{
		private static Dictionary<int, DrawLayerData> WingsLayerData { get; set; }

		
		public static void RegisterData(int wingSlot, DrawLayerData data)
		{
			if (!WingsLayerData.ContainsKey(wingSlot))
			{
				WingsLayerData.Add(wingSlot, data);
			}
		}

		public override void Load()
		{
			WingsLayerData = new Dictionary<int, DrawLayerData>();
		}

		public override void Unload()
		{
			WingsLayerData = null;
		}

		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Wings);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || drawPlayer.invis || drawPlayer.wings == -1)
			{
				return false;
			}
			return true;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!WingsLayerData.TryGetValue(drawPlayer.wings, out DrawLayerData data))
			{
				return;
			}

			Color color = drawPlayer.GetImmuneAlphaPure(data.Color(), drawInfo.shadow);

			Texture2D texture = data.Texture.Value;

			Vector2 directions = drawPlayer.Directions;
			Vector2 offset = new Vector2(0f, 7f);
			Vector2 position = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height / 2) + offset;

			int num11 = 0;
			int num12 = 0;
			int numFrames = 4;

			position += new Vector2(num12 - 9, num11 + 2) * directions;
			position = position.Floor();
			Rectangle frame = new Rectangle(0, texture.Height / numFrames * drawPlayer.wingFrame, texture.Width, texture.Height / numFrames);
			DrawData drawData = new DrawData(texture, position.Floor(), frame, color, drawPlayer.bodyRotation, new Vector2(texture.Width / 2, texture.Height / numFrames / 2), 1f, drawInfo.playerEffect, 0)
			{
				shader = drawInfo.cWings
			};
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
	//________________________________
	//____________________________________________________
	public sealed class ShoesLayer : PlayerDrawLayer
	{
		private static Dictionary<int, DrawLayerData> ShoesLayerData { get; set; }


		public static void RegisterData(int shoeSlot, DrawLayerData data)
		{
			if (!ShoesLayerData.ContainsKey(shoeSlot))
			{
				ShoesLayerData.Add(shoeSlot, data);
			}
		}

		public override void Load()
		{
			ShoesLayerData = new Dictionary<int, DrawLayerData>();
		}

		public override void Unload()
		{
			ShoesLayerData = null;
		}

		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Shoes);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || drawPlayer.invis || drawPlayer.shoe == -1)
			{
				return false;
			}
			return true;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!ShoesLayerData.TryGetValue(drawPlayer.shoe, out DrawLayerData data))
			{
				return;
			}

			Color color = drawPlayer.GetImmuneAlphaPure(data.Color(), drawInfo.shadow);

			Texture2D texture = data.Texture.Value;
			Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.legFrame.Width / 2, drawPlayer.height - drawPlayer.legFrame.Height + 4f) + drawPlayer.legPosition;
			Vector2 shoeOffset = drawInfo.legsOffset;
			DrawData drawData = new DrawData(texture, drawPos.Floor() + shoeOffset, drawPlayer.legFrame, color, drawPlayer.legRotation, shoeOffset, 1f, drawInfo.playerEffect, 0)
			{
				shader = drawInfo.cShoe
			};
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
	//________________________________________________________________________________
	public sealed class HeldItemLayer : PlayerDrawLayer
	{
		private static Dictionary<(int type, int useStyle), DrawLayerData> ItemLayerData { get; set; }

		
		public static void RegisterData(int type, DrawLayerData data, int useStyle = -1)
		{
			var tuple = new ValueTuple<int, int>(type, useStyle);
			if (!ItemLayerData.ContainsKey(tuple))
			{
				ItemLayerData.Add(tuple, data);
			}
		}

		public override void Load()
		{
			ItemLayerData = new Dictionary<(int, int), DrawLayerData>();
		}

		public override void Unload()
		{
			ItemLayerData = null;
		}

		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.HeldItem);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Item heldItem = drawInfo.heldItem;
			bool usingItem = drawPlayer.itemAnimation > 0 && heldItem.useStyle != 0;
			bool holdingSuitableItem = heldItem.holdStyle != 0 && !drawPlayer.pulley;
			if (!drawPlayer.CanVisuallyHoldItem(heldItem))
			{
				holdingSuitableItem = false;
			}

			if (drawInfo.shadow != 0f || drawPlayer.JustDroppedAnItem || drawPlayer.frozen || !(usingItem || holdingSuitableItem) || heldItem.type <= 0 || drawPlayer.dead || heldItem.noUseGraphic || drawPlayer.wet && heldItem.noWet || drawPlayer.happyFunTorchTime && drawPlayer.HeldItem.createTile == TileID.Torches && drawPlayer.itemAnimation == 0)
			{
				return false;
			}

			return true;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			Item heldItem = drawInfo.heldItem;
			int useStyle = heldItem.useStyle;

			DrawLayerData data = null;
			foreach (var pair in ItemLayerData)
			{
				var tuple = pair.Key;
				var value = pair.Value;
				if (tuple.type == heldItem.type)
				{
					if (tuple.useStyle == useStyle)
					{
						data = value; //If found matching useStyle, successful
						break;
					}
					else if (tuple.useStyle == -1)
					{
						data = value; //Keep track of fallback
					}
				}
			}

			if (data == null)
			{
				return;
			}

			Texture2D weaponGlow = data.Texture.Value;
			float adjustedItemScale = drawPlayer.GetAdjustedItemScale(heldItem);
			Vector2 position = new Vector2((int)(drawInfo.ItemLocation.X - Main.screenPosition.X), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y));
			Rectangle? sourceRect = new Rectangle(0, 0, weaponGlow.Width, weaponGlow.Height);

			if (useStyle == ItemUseStyleID.Swing)
			{
				Vector2 origin = new Vector2(drawPlayer.direction == -1 ? weaponGlow.Width : 0, drawPlayer.gravDir == -1 ? 0 : weaponGlow.Height);
				DrawData drawData = new DrawData(weaponGlow, position, sourceRect, data.Color(), drawPlayer.itemRotation, origin, adjustedItemScale, drawInfo.itemEffect, 0);
				drawInfo.DrawDataCache.Add(drawData);
			}
			else if (useStyle == ItemUseStyleID.Shoot)
			{
				if (Item.staff[heldItem.type])
				{
					float num9 = drawInfo.drawPlayer.itemRotation + 0.785f * (float)drawInfo.drawPlayer.direction;
					float num10 = 0f;
					float num11 = 0f;
					Vector2 originStaff = new Vector2(0f, weaponGlow.Height);

					if (drawInfo.drawPlayer.gravDir == -1f)
					{
						if (drawInfo.drawPlayer.direction == -1)
						{
							num9 += 1.57f;
							originStaff = new Vector2(weaponGlow.Width, 0f);
							num10 -= weaponGlow.Width;
						}
						else
						{
							num9 -= 1.57f;
							originStaff = Vector2.Zero;
						}
					}
					else if (drawInfo.drawPlayer.direction == -1)
					{
						originStaff = new Vector2(weaponGlow.Width, weaponGlow.Height);
						num10 -= weaponGlow.Width;
					}

					ItemLoader.HoldoutOrigin(drawInfo.drawPlayer, ref originStaff);

					DrawData drawDataStaff = new DrawData(weaponGlow, new Vector2((int)(drawInfo.ItemLocation.X - Main.screenPosition.X + originStaff.X + num10), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y + num11)), sourceRect, data.Color(), num9, originStaff, adjustedItemScale, drawInfo.itemEffect, 0);
					drawInfo.DrawDataCache.Add(drawDataStaff);

					return;
				}

				Vector2 vector5 = new Vector2(weaponGlow.Width / 2, weaponGlow.Height / 2);
				Vector2 vector6 = Main.DrawPlayerItemPos(drawPlayer.gravDir, heldItem.type);
				int num12 = (int)vector6.X;
				vector5.Y = vector6.Y;
				Vector2 origin = new Vector2(-num12, weaponGlow.Height / 2);
				if (drawPlayer.direction == -1)
				{
					origin = new Vector2(weaponGlow.Width + num12, weaponGlow.Height / 2);
				}

				DrawData drawData = new DrawData(weaponGlow, new Vector2((int)(drawInfo.ItemLocation.X - Main.screenPosition.X + vector5.X), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y + vector5.Y)), sourceRect, data.Color(), drawPlayer.itemRotation, origin, adjustedItemScale, drawInfo.itemEffect, 0);
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}