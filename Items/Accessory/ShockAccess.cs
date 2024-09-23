using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Common;
using Terraria.GameContent.Creative;
using Humanizer;
using System.Threading.Channels;
using System;


namespace StormDiversMod.Items.Accessory
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class ShockBand : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shock Charm");
            //Tooltip.SetDefault("Crits do lightning arc thingy");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().shockBand = true;
            player.GetModPlayer<EquipmentEffects>().ShockbandItem = Item;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<ClawsBone>(), 1)
         .AddIngredient(ModContent.ItemType<BlueCuffs>(), 1)
         .AddIngredient(ItemID.Bone, 15)
         .AddTile(TileID.TinkerersWorkbench)
         .Register();
        }
    }
    public class ShockQuiver : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shock Quiver");
            //Tooltip.SetDefault("Increases arrow damage by 10% and greatly increases arrow speed
            //20 % chance to not consume arrowsCrits do lightning arc thingy");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Pink;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().shockBand = true;
            player.GetModPlayer<EquipmentEffects>().ShockbandItem = Item;
            player.magicQuiver = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<ShockBand>(), 1)
         .AddIngredient(ItemID.MagicQuiver, 1)
         .AddTile(TileID.TinkerersWorkbench)
         .Register();
        }
    }
}