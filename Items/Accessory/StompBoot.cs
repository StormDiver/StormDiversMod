using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
   
    //__________________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Shoes)]
    public class StompBoot : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heavy Boots");
            Tooltip.SetDefault("Hold DOWN to fall faster and create a shockwave upon hitting the ground\nWhile falling faster you will damage any enemy you fall on\n'What did you think would happen if you attached an Anvil to a pair of boots?'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 1, 25, 0);
            Item.rare = ItemRarityID.Orange;

            Item.defense = 3;
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }



        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<StormPlayer>().bootFall = true;
        }
       
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.RocketBoots, 1)
           .AddIngredient(ItemID.Chain, 3)
           .AddRecipeGroup("StormDiversMod:Anvils")        
           .AddTile(TileID.Anvils)
           .Register();

           
        }
    }
    //________________________________________________________________________________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Shoes)]
    public class StompBootHorse : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heavy Horseshoe Boots");
            Tooltip.SetDefault("Hold DOWN to fall faster and create a shockwave upon hitting the ground\nWhile falling faster you will damage any enemy you fall on\nNegates fall damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.Orange;

             Item.defense = 3;
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }

        //bool falling;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<StormPlayer>().bootFall = true;
            player.noFallDmg = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<StompBoot>(), 1)
           .AddIngredient(ItemID.LuckyHorseshoe, 1)
           .AddTile(TileID.TinkerersWorkbench)
           .Register();

           
        }
    }
}