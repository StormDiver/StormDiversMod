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
			DisplayName.SetDefault("Eyeball Hook"); // The item's name in-game.
			Tooltip.SetDefault("Fires out one long but slow hook");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; // Amount of this item needed to research and become available in Journey mode's duplication menu. Amount based on vanilla hooks' amount needed
		}

		public override void SetDefaults()
		{
			// Copy values from the Amethyst Hook
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.shootSpeed = 12f; // This defines how quickly the hook is shot.
			Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.EyeHookProj>(); // Makes the item shoot the hook's projectile when used.
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 50, 0);

		}


	}
}