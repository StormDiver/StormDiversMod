using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
   
    public class DesertJar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pharaoh's Urn");
            //Tooltip.SetDefault("Two sand particles will orbit you at close range, damaging enemies\nLeaves behind a damaging trail of sand when moving fast enough");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;

            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().DesertJarItem = Item;
            player.GetModPlayer<EquipmentEffects>().desertJar = true;
        }
       
        public override void AddRecipes()
        {
            /*CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 10)
            .AddIngredient(ItemID.AncientBattleArmorMaterial, 2)
            .AddTile(TileID.MythrilAnvil)
            .Register();*/
          
        }
    }
}