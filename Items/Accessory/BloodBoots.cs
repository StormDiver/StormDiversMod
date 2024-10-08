using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Common;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace StormDiversMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Shoes)]
    public class BloodBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloody Treads");
            //Tooltip.SetDefault("The wearer can run up to 36mph\nIncreases acceleration\nLeaves behind a damaging trail of blood when running along the ground");
            Item.ResearchUnlockCount = 1;
       
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {

            }
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().BloodBootsItem = Item;

            player.GetModPlayer<EquipmentEffects>().bloodBoots = true;
            //player.waterWalk = true;
           
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("StormDiversMod:RunBoots", 1)
           .AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 5)
           .AddTile(TileID.Anvils)
           .Register();

        }
     

    }
}