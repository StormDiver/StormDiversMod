using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace StormDiversMod.Items.Tools
{
	internal class DerpHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Derpling Hook"); // The item's name in-game.
			Tooltip.SetDefault("Can fire out 3 hooks simultaneously, 2 can be attached at a time");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; // Amount of this item needed to research and become available in Journey mode's duplication menu. Amount based on vanilla hooks' amount needed
		}

		public override void SetDefaults()
		{
			// Copy values from the Amethyst Hook
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.shootSpeed = 16f; // This defines how quickly the hook is shot.
			Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.DerpHookProj>(); // Makes the item shoot the hook's projectile when used.
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
}