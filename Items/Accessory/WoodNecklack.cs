using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Neck)]

    public class WoodNecklace : ModItem //Now Beetle gauntlet
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Pendant");
            Tooltip.SetDefault("Reduces damage taken by 4 while in a forest"); //maybe trees drop more acorns, more wood??
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;

            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.White;
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().woodNecklace = true;
            
        }


        public override void AddRecipes()
        {
            

        }


    }
    
    /*public class Woodtiles : GlobalTile
    {
        public override void SetStaticDefaults()
        {
        }
       
        public override bool Drop(int i, int j, int type)
        {
           
            return true;
        }
    }*/
}