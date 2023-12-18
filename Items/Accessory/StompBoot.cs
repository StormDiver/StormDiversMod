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
            //DisplayName.SetDefault("Heavy Boots");
            //Tooltip.SetDefault("Allows flight\nHold DOWN to fall faster and create a shockwave upon hitting the ground\n
            //The shockwave will be larger and deal more damage the further you fall\nWhile falling faster you will damage any enemy you fall on and avoid contact damage\nJumping immediately after stomping grants a large jump boost

            //'What did you think would happen if you attached an Anvil to a pair of boots?'");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;

            Item.defense = 3;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().bootFall = true;
        }
       
        public override void AddRecipes()
        {
        }
    }
    //________________________________________________________________________________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Shoes)]
    public class StompBootHorse : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heavy Horseshoe Boots");
            //Tooltip.SetDefault("Allows flight and negates fall damage\nHold DOWN to fall faster and create a shockwave upon hitting the ground
            //The shockwave will be larger and deal more damage the further you fall\nWhile falling faster you will damage any enemy you fall on and avoid contact damage\nJumping immediately after stomping grants a large jump boost
            //");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;

             Item.defense = 3;
            Item.accessory = true;
        }

        //bool falling;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().bootFall = true;
            player.GetModPlayer<EquipmentEffects>().bootFallLuck = true;
            player.rocketBoots = 1;
            player.vanityRocketBoots = 1;
            player.noFallDmg = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<StompBoot>(), 1)
            .AddIngredient(ItemID.RocketBoots, 1)
           .AddIngredient(ItemID.LuckyHorseshoe, 1)
           .AddTile(TileID.TinkerersWorkbench)
           .Register();

           
        }
    }
}