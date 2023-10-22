using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace StormDiversMod.Items.Tools
{
	internal class EyeHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Eyeball Hook"); // The item's name in-game.
			//Tooltip.SetDefault("Fires out one long but slow hook");

			Item.ResearchUnlockCount = 1; // Amount of this item needed to research and become available in Journey mode's duplication menu. Amount based on vanilla hooks' amount needed
		}

		public override void SetDefaults()
		{
			// Copy values from the Amethyst Hook
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.shootSpeed = 10f; // This defines how quickly the hook is shot.
			Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.HookEyeProj>(); // Makes the item shoot the hook's projectile when used.
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 50, 0);
		}
	}

	//_________________________________________
	internal class DerpHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Derpling Hook"); // The item's name in-game.
			//Tooltip.SetDefault("Can fire out 4 hooks simultaneously, 3 can be attached at a time");

			Item.ResearchUnlockCount = 1; // Amount of this item needed to research and become available in Journey mode's duplication menu. Amount based on vanilla hooks' amount needed
		}

		public override void SetDefaults()
		{
			// Copy values from the Amethyst Hook
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.shootSpeed = 16f; // This defines how quickly the hook is shot.
			Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.HookDerpProj>(); // Makes the item shoot the hook's projectile when used.
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 5, 0, 0);

		}
		public override void AddRecipes()
		{
			CreateRecipe()
		   .AddIngredient(ItemID.ChlorophyteBar, 8)
		   .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 4)
		   .AddTile(TileID.MythrilAnvil)
		   .Register();

		}

	}
	//__________________________________________________
	internal class StormHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Lightning Hook"); // The item's name in-game.
			//Tooltip.SetDefault("Fires out a single fast hook");

			Item.ResearchUnlockCount = 1; // Amount of this item needed to research and become available in Journey mode's duplication menu. Amount based on vanilla hooks' amount needed
		}

		public override void SetDefaults()
		{
			// Copy values from the Amethyst Hook
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.shootSpeed = 18f; // This defines how quickly the hook is shot.
			Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.HookStormProj>(); // Makes the item shoot the hook's projectile when used.
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 5, 0, 0);

		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Tools/StormHook_Glow");

			spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
				new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
		}

	}
}