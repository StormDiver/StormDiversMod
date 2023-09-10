using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

using Terraria;
using Terraria.DataStructures;


using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;

using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;

namespace StormDiversMod.Items.Accessory
{
 
    public class HeartJar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Life Emblem");
            //Tooltip.SetDefault("Enemies have a chance to heal you once for 20 health when they fall below half life, bosses always heal for 50\nEnemies that that grant you health lose life rapidly\nIncreases maximum health by 20");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            
            Item.accessory = true;
            
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().heartSteal = true;
            player.statLifeMax2 += 20;
        }          
    }
    [AutoloadEquip(EquipType.Neck)]

    public class HeartJarPS : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heart Charm");
            //Tooltip.SetDefault("Provides life regeneration and reduces the cooldown of healing potions by 25%\nEnemies have a chance to heal you once for 20 health when they fall below half life, bosses always heal for 50\nEnemies that that grant you health lose life rapidly\nIncreases maximum health by 20");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }     
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;

            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().heartSteal = true;
            player.statLifeMax2 += 20;
            player.pStone = true;
            player.lifeRegen = +2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<HeartJar>(), 1)
            .AddIngredient(ItemID.CharmofMyths, 1)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();

        }

    }
}