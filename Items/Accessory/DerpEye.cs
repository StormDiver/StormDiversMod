﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Common;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;


namespace StormDiversMod.Items.Accessory
{

    public class DerpEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eye of the Derpling");
            //Tooltip.SetDefault("Greatly increases luck\nIncreases critical strike damage by 15%\n'Lucky you'");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Pink;

            Item.accessory = true;
            

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().derpEye = true;
        }
    }
    public class DerpEyeGolem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eye of the Jungle");
            //Tooltip.SetDefault("10% increased critical strike chance\nGreatly increases luck\nIncreases critical strike damage by 15%");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Lime;

            Item.accessory = true;
            

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Generic) += 10;

            player.GetModPlayer<EquipmentEffects>().derpEyeGolem = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<DerpEye>(), 1)
            .AddIngredient(ItemID.EyeoftheGolem, 1)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }

    }
}