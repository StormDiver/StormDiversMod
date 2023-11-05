using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using StormDiversMod.NPCs.NPCProjs;
using System.Collections.Generic;
using Terraria.UI;

namespace StormDiversMod.Items.Vanitysets
{
    [AutoloadEquip(EquipType.Head)]
    public class ThePainMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("ThePain");
            //Tooltip.SetDefault("When the pain is too much");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<TheClaymanMask>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                {
                    if (ItemSlot.ShiftInUse)
                        line.Text = "[c/FF1493:THE PAIN!!!!]";
                    else
                        line.Text = "When the pain is too much";
                }
            }
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
      
        public override bool OnPickup(Player player)
        {
            if (!GetInstance<ConfigurationsIndividual>().NoPain)
            {
                SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSound") with { Volume = 1.5f }, player.Center);
            }
            return base.OnPickup(player);
        }

    }
    [AutoloadEquip(EquipType.Head)]
    public class TheClaymanMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Clayman");
            //Tooltip.SetDefault("Sliently judge everybody around you");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ThePainMask>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                {
                    if (ItemSlot.ShiftInUse)
                        line.Text = "[c/FFDAB9:CLAYMAN!!!!]";
                    else
                        line.Text = "Sliently judge everybody around you";
                }
            }
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true; 
        }
        public override void ArmorSetShadows(Player player)
        {

        }
        public override bool OnPickup(Player player)
        {
            if (!GetInstance<ConfigurationsIndividual>().NoPain)
            {
                SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ClayManSound") with { Volume = 1.5f }, player.Center);
            }
            return base.OnPickup(player);
        }
    }

    [AutoloadEquip(EquipType.Head)]
    public class UltimateFearMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Perfectly Normal mask");
            //Tooltip.SetDefault("'Why's everybody looking at you like that, you're perfectly normal");
            Item.ResearchUnlockCount = 1;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                {
                    if (ItemSlot.ShiftInUse)
                        line.Text = "[c/4169E1:SUFFER!!!!]";
                    else
                        line.Text = "Why's everybody looking at you like that, you're perfectly normal";
                }
            }
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
        public override void ArmorSetShadows(Player player)
        {

        }
    }
}