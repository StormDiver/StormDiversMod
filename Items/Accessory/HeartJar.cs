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
            DisplayName.SetDefault("Heart Emblem");
            Tooltip.SetDefault("Some enemies will drop a super heart when hit below half life\nEnemies that drop the heart lose life rapidly\nIncreases maximum health by 20");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;

            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<StormPlayer>().heartSteal = true;
            player.statLifeMax2 += 20;
        }

       
         
    }
}