using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
   
    public class FrostJar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frozen Urn");
            //Tooltip.SetDefault("Multiple frost spikes explode out of you when taking more than 1 damage\nIncreases critical strike chance and movement speed for 6 seconds afterwards" +
                //"\nTwo frost particles will orbit you at close range, damaging enemies\nLeaves behind a damaging trail of frost when moving fast enough");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightPurple;

           
            Item.accessory = true;
            

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().frostSpike = true;
            player.GetModPlayer<EquipmentEffects>().desertJar = true;
            player.GetModPlayer<EquipmentEffects>().frostJar = true; //makes the desert projs turn into frost projs

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<FrostAccess>(), 1)
            .AddIngredient(ModContent.ItemType<DesertJar>(), 1)
            .AddIngredient(ItemID.HallowedBar, 5)
            //.AddRecipeGroup("StormDiversMod:MechSoul", 5)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
           
        }
       
    }
}