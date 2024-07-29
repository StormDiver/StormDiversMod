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
    public class CoralEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Whirlpool Emblem");
            //Tooltip.SetDefault("Using most weapons summons a water orb from the sky that travels towards the cursor's location");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            

            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().coralEmblem = true;
        }
       
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Coral, 8)
           .AddIngredient(ItemID.Starfish, 2)
           .AddIngredient(ItemID.Seashell, 2)
           .AddTile(TileID.WorkBenches)
           .Register();

        }
    }
   
   
}