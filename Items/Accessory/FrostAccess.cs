using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
   
    public class FrostAccess : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen Heart");
            Tooltip.SetDefault("Multiple frost spikes explode out of you when taking more than 1 damage\nIncreases critical strike chance and movement speed for 6 seconds afterwards");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;

           
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().frostSpike = true;
            
        }

        public override void AddRecipes()
        {
           /* CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 10)
            .AddIngredient(ItemID.FrostCore, 2)
            .AddTile(TileID.MythrilAnvil)
            .Register();*/
           
        }
       
    }
}