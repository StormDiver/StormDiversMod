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

namespace StormDiversMod.Items.Accessory
{
   
    public class SpectreAccessory : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spectre Skull");
            //Tooltip.SetDefault("Mana usage is negated when under the effects of mana sickness\nIncreases maximum mana by 40");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (ModLoader.HasMod("TRAEProject"))//DON'T FORGET THIS!!!!!!!
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "Mana usage is reduced by 25% while under the effects of mana sickness"; //Unusable pre mechs
                    }
                }
               
            }
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
            player.statManaMax2 += 40;
                player.GetModPlayer<EquipmentEffects>().SpectreSkull = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.SpectreBar, 20)
            .AddIngredient(ItemID.SuperManaPotion, 30)
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
                //"\nMana usage is negated when under the effects of mana sickness\nIncreases maximum mana by 40");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (ModLoader.HasMod("TRAEProject"))
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "Magic critical hits have a chance to spawn a mana star"; 
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                    {
                        line.Text = "Mana usage is reduced by 25% while under the effects of mana sickness"; 
                    }
                }

            }
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
            player.statManaMax2 += 40;

            player.GetModPlayer<EquipmentEffects>().SpectreSkull = true;
            player.manaMagnet = true;
            if (!ModLoader.HasMod("TRAEProject"))
            {
                player.manaCost -= 0.08f;
            }
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