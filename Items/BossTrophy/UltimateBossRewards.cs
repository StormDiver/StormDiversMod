using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using System;
using ReLogic.Content;

namespace StormDiversMod.Items.BossTrophy
{
	//TROPHY_______________________________
	public class UltimateBossTrophy : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Ultimate Pain Trophy");
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<UltimateBossTrophyPlaced>());

			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
	}
	//______________
	public class UltimateBossTrophyPlaced : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(120, 85, 60), Language.GetText("MapObject.Trophy"));
			DustType = 7;
		}
		/*public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<UltimateBossTrophy>());
		}*/
	}
	//___________________________RELIC
	public class UltimateBossRelic : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Ultimate Pain Relic");
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
			// The place style (here by default 0) is important if you decide to have more than one relic share the same tile type (more on that in the tiles' code)
			Item.DefaultToPlaceableTile(ModContent.TileType<UltimateBossRelicPlaced>(), 0);

			Item.width = 30;
			Item.height = 40;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Master;
			Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
			Item.value = Item.buyPrice(0, 5);
		}
	}
	//_____________________
	public class UltimateBossRelicPlaced : ModTile
	{
		public const int FrameWidth = 18 * 3;
		public const int FrameHeight = 18 * 4;
		public const int HorizontalFrames = 1;
		public const int VerticalFrames = 1; // Optional: Increase this number to match the amount of relics you have on your extra sheet, if you choose to go the Item.placeStyle way

		public Asset<Texture2D> RelicTexture;

		// Every relic has its own extra floating part, should be 50x50. Optional: Expand this sheet if you want to add more, stacked vertically
		// If you do not go the optional way, and you extend from this class, you can override this to point to a different texture
		public virtual string RelicTextureName => "StormDiversMod/Items/BossTrophy/UltimateBossRelicPlaced";

		// All relics use the same pedestal texture, this one is copied from vanilla
		public override string Texture => "StormDiversMod/Items/BossTrophy/RelicPedestal";

		public override void Load()
		{
			if (!Main.dedServ)
			{
				// Cache the extra texture displayed on the pedestal
				RelicTexture = ModContent.Request<Texture2D>(RelicTextureName);
			}
		}
		public override void Unload()
		{
			// Unload the extra texture displayed on the pedestal
			RelicTexture = null;
		}

		public override void SetStaticDefaults()
		{
            Main.tileShine[Type] = 400; // Responsible for golden particles
            Main.tileFrameImportant[Type] = true; // Any multitile requires this
            TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4); // Relics are 3x4
            TileObjectData.newTile.LavaDeath = false; // Does not break when lava touches it
            TileObjectData.newTile.DrawYOffset = 2; // So the tile sinks into the ground
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft; // Player faces to the left
            TileObjectData.newTile.StyleHorizontal = false; // Based on how the alternate sprites are positioned on the sprite (by default, true)

            // This controls how styles are laid out in the texture file. This tile is special in that all styles will use the same texture section to draw the pedestal.
            TileObjectData.newTile.StyleWrapLimitVisualOverride = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.styleLineSkipVisualOverride = 0; // This forces the tile preview to draw as if drawing the 1st style.
                                                                    // Register an alternate tile data with flipped direction
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile); // Copy everything from above, saves us some code
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; // Player faces to the right
			TileObjectData.addAlternate(1);

			// Register the tile data itself
			TileObjectData.addTile(Type);

			// Register map name and color
			// "MapObject.Relic" refers to the translation key for the vanilla "Relic" text
			AddMapEntry(new Color(233, 207, 94), Language.GetText("MapObject.Relic"));
		}
		
		/*public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int placeStyle = frameX / FrameWidth;

			int itemType = 0;
			switch (placeStyle)
			{
				case 0:
					itemType = ModContent.ItemType<UltimateBossRelic>();
					break;
					// Optional: Add more cases here
			}

			if (itemType > 0)
			{
				// Spawn the item
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, itemType);
			}
		}*/

		public override bool CreateDust(int i, int j, ref int type)
		{
			return false;
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{			
			tileFrameX %= FrameWidth; // Clamps the frameX
			tileFrameY %= FrameHeight * 2; // Clamps the frameY (two horizontally aligned place styles, hence * 2)
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{	
			if (drawData.tileFrameX % FrameWidth == 0 && drawData.tileFrameY % FrameHeight == 0)
			{
				Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
			}
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			// This is lighting-mode specific, always include this if you draw tiles manually
			Vector2 offScreen = new Vector2(Main.offScreenRange);
			if (Main.drawToScreen)
			{
				offScreen = Vector2.Zero;
			}
			// Take the tile, check if it actually exists
			Point p = new Point(i, j);
			Tile tile = Main.tile[p.X, p.Y];
			if (tile == null || !tile.HasTile)
			{
				return;
			}

			// Get the initial draw parameters
			Texture2D texture = RelicTexture.Value;

			int frameY = tile.TileFrameX / FrameWidth; // Picks the frame on the sheet based on the placeStyle of the item
			Rectangle frame = texture.Frame(HorizontalFrames, VerticalFrames, 0, frameY);

			Vector2 origin = frame.Size() / 2f;
			Vector2 worldPos = p.ToWorldCoordinates(24f, 64f);

			Color color = Lighting.GetColor(p.X, p.Y);

			bool direction = tile.TileFrameY / FrameHeight != 0; // This is related to the alternate tile data we registered before
			SpriteEffects effects = direction ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			// Some math magic to make it smoothly move up and down over time
			const float TwoPi = (float)Math.PI * 2f;
			float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 5f);
			Vector2 drawPos = worldPos + offScreen - Main.screenPosition + new Vector2(0f, -40f) + new Vector2(0f, offset * 4f);

			// Draw the main texture
			spriteBatch.Draw(texture, drawPos, frame, color, 0f, origin, 1f, effects, 0f);

			// Draw the periodic glow effect
			float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 0.7f;
			Color effectColor = color;
			effectColor.A = 0;
			effectColor = effectColor * 0.1f * scale;
			for (float num5 = 0f; num5 < 1f; num5 += 355f / (678f * (float)Math.PI))
			{
				spriteBatch.Draw(texture, drawPos + (TwoPi * num5).ToRotationVector2() * (6f + offset * 2f), frame, effectColor, 0f, origin, 1f, effects, 0f);
			}
		}
	}
}