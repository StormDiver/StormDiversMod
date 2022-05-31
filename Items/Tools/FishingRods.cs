using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Tools
{
	internal class FishingRodDerpling : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Derpling Fishing Rod");
			Tooltip.SetDefault("Fires multiple lines at once");
			//ItemID.Sets.CanFishInLava[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{			
			 Item.width = 24;
			 Item.height = 28;
			 Item.useStyle = ItemUseStyleID.Swing;
			 Item.useAnimation = 8;
			 Item.useTime = 8;
			 Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.fishingPole = 45; // Sets the poles fishing power
			Item.shootSpeed = 15f; // Sets the speed in which the bobbers are launched. Wooden Fishing Pole is 9f and Golden Fishing Rod is 17f.
			Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.BobberDerpling>(); // The Bobber projectile.
		}

		public override void HoldItem(Player player)//Can make the line never break 
		{
			//player.accFishingLine = true;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) //multiple lines
        {          
			int bobberAmount = 3; // 3 bobbers
			float spreadAmount = 60f; // how much the different bobbers are spread out.

			for (int index = 0; index < bobberAmount; ++index)
			{
				Vector2 bobberSpeed = velocity + new Vector2(Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f, Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f);

				// Generate new bobbers
				Projectile.NewProjectile(source, position, bobberSpeed, type, 0, 0f, player.whoAmI);
			}
			return false;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes()
		{
			CreateRecipe()
		 .AddIngredient(ItemID.ChlorophyteBar, 10)
		 .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 5)
		 .AddTile(TileID.MythrilAnvil)
		 .Register();
		}
	}
}