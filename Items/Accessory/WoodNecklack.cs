using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;
using System;
using System.Collections.Generic;
using StormDiversMod.Items.Weapons;
using Microsoft.Xna.Framework.Graphics;

namespace StormDiversMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Neck)]

    public class WoodNecklace : ModItem //Now Beetle gauntlet
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wooden Pendant");
            //Tooltip.SetDefault("Reduces damage taken by 4 and slightly increases life regen while in a forest");
            Item.ResearchUnlockCount = 1;
        }
       
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;

            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.White;
            Item.accessory = true;
            

        }
        bool frozentip;
        bool deserttip;
        public override void UpdateInventory(Player player)
        {
            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklaceFrozen>()))
                frozentip = true;
            else
                frozentip = false;

            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklaceDesert>()))
                deserttip = true;
            else
                deserttip = false;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().woodNecklace = true;

            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklaceFrozen>()))
                frozentip = true;
            else
                frozentip = false;

            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklaceDesert>()))
                deserttip = true;
            else
                deserttip = false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (frozentip)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\n[c/75d2d8:Pair with the Frozen pendant to extend the effects into the snow biome and vice versa!]";
                    }
                }
                if (deserttip)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\n[c/a87746:Pair with the Mandible pendant to extend the effects into the desert biome and vice versa!]";
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            

        }


    }
    [AutoloadEquip(EquipType.Neck)]
    public class WoodNecklaceFrozen : ModItem //Now Beetle gauntlet
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frozen Pendant");
            //Tooltip.SetDefault("Enemies within a certain radius of you are inflicted with frostburn\nGrants immunity to frostburn");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;

            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.White;
            Item.accessory = true;


        }
        bool foresttip;
        bool deserttip;
        public override void UpdateInventory(Player player)
        {
            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklace>()))
                foresttip = true;
            else
                foresttip = false;

            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklaceDesert>()))
                deserttip = true;
            else
                deserttip = false;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().frozenNecklace = true;

            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklace>()))
                foresttip = true;
            else
                foresttip = false;

            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklaceDesert>()))
                deserttip = true;
            else
                deserttip = false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (foresttip)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = line.Text + "\n[c/25982d:Pair with the Wooden pendant to extend the effects into the forest biome and vice versa!]";
                    }
                }
                if (deserttip)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = line.Text + "\n[c/a87746:Pair with the Mandible pendant to extend the effects into the desert biome and vice versa!]";
                    }
                }
            }
        }

        public override void AddRecipes()
        {


        }


    }
    [AutoloadEquip(EquipType.Neck)]
    public class WoodNecklaceDesert : ModItem //Now Beetle gauntlet
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Antlion Necklace");
            //Tooltip.SetDefault("Grants immunity to suffication, and increases damage by 1 while in a desert biome");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;

            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.White;
            Item.accessory = true;
        }
        bool foresttip;
        bool frozentip;
        public override void UpdateInventory(Player player)
        {
            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklace>()))
                foresttip = true;
            else
                foresttip = false;

            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklaceFrozen>()))
                frozentip = true;
            else
                frozentip = false;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().desertNecklace = true;

            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklace>()))
                foresttip = true;
            else
                foresttip = false;

            if (player.HasItemInAnyInventory(ModContent.ItemType<WoodNecklaceFrozen>()))
                frozentip = true;
            else
                frozentip = false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (foresttip)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = line.Text + "\n[c/25982d:Pair with the Wooden pendant to extend the effects into the forest biome and vice versa!]";
                    }
                }
                if (frozentip)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = line.Text + "\n[c/75d2d8:Pair with the Frozen pendant to extend the effects into the snow biome and vice versa!]";
                    }
                }
            }
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