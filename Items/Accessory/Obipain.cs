using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Neck)]
    public class ObiPain : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Obsidian Necklace");
            //Tooltip.SetDefault("Increases armor penetration by 5\nGrants immunity to fire blocks\n'For those incredibly common moments when you have to fight high-defensed enemies while standing on fire blocks'");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 1, 54, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.defense = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 5;
            player.fireWalk = true;
        }
        public override void AddRecipes()
        {
          CreateRecipe()
         .AddIngredient(ItemID.SharkToothNecklace, 1)
         .AddIngredient(ItemID.ObsidianSkull, 1)
         .AddTile(TileID.TinkerersWorkbench)
         .Register();
        }
    }
}