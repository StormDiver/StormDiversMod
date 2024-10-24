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
using StormDiversMod.Common;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Accessory
{
   
    public class SpectreAccessory : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spectre Skull");
            //Tooltip.SetDefault("Enemies have a chance to drop a Spectre Star on death, which restores 200 mana\nThe star also grants the Spectre Star Buff which increases magic critical strike chance by up to 25% depending on the remaining duration");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
      
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 30;

            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Yellow;

            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().SpectreSkull = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.SpectreBar, 20)
            .AddIngredient(ItemID.SoulofFright, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 150;
            return color;
        }
    }
    public class SpectreAccessoryMagnet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magnet Skull Flower");
            //Tooltip.SetDefault("8% reduced mana usage\nAutomatically use mana potions when needed\nIncreases pickup range for mana stars" +
            //"\nEnemies have a chance to drop a Spectre Star on death, which restores 200 mana\nThe star also grants the Spectre Star Buff which increases magic critical strike chance by up to 25% depending on the remaining duration");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 30;

            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;

            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().SpectreSkull = true;
            player.manaMagnet = true;

            player.manaCost -= 0.08f;

            player.manaFlower = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<SpectreAccessory>(), 1)
           .AddIngredient(ItemID.MagnetFlower, 1)
           .AddTile(TileID.TinkerersWorkbench)
           .Register();
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 150;
            return color;

        }
    }
}