using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System.Collections.Generic;
using static Terraria.GameContent.Animations.Actions.NPCs;
using static Terraria.ModLoader.ExtraJump;
using System.Security.Policy;

namespace StormDiversMod.Items.Accessory
{
    public class SantaWires : ModItem
    { 
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Santank Power Core");
            //Tooltip.SetDefault("Exposed wires will shock you back to life for after taking lethal damage, granting a 10 second grace period
            //During the period you are immune to damage and deal extra damage
              //After 10 seconds you will die unless you heal to above 33 % life
              // Has a 5 minute cooldown if you survive between uses
              // Won't save you if the attacks deals more damage than your max life");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 3));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 25;

            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;

            Item.accessory = true;

            Item.expert = true;
        }
        int thirdlife;
       
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            thirdlife = player.statLifeMax2 / 3;
            player.GetModPlayer<EquipmentEffects>().SantaCore = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {

                if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                {
                    line.Text = line.Text + " (" + thirdlife + ")";
                }

            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void PostUpdate()
        {
           
        }

    }
}