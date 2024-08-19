using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using StormDiversMod.Items.Materials;
using Humanizer;

namespace StormDiversMod.Items.Accessory
{
    public class DeathsList : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Repear's list");
            //Tooltip.SetDefault("Killing enemies drops their soul which can be picked up, up to 9 souls can be carried at once
            //Bosses instead have a small chance to drop a soul when hit
            //Each collected soul increases your damage by 2 % and crit chance by 1 %
            //Souls will begin to escape if no souls are collected within 9 seconds, 
			//Collecting a soul will reset the escape countdown");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 7));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            /*foreach (TooltipLine line in tooltips)
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                    {
                        line.Text = line.Text + "\n[c/CC6600:Multiplayer only: Killing an enemy directly immediately collects their soul]"; //multiplayer sucks
                    }
                }
            }*/
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().deathList = true;
            player.GetModPlayer<EquipmentEffects>().Deathlistitem = Item;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Book, 1)
            .AddIngredient(ItemID.HellstoneBar, 10)
            .AddIngredient(ModContent.ItemType<CrackedHeart>(), 2)
            .AddTile(TileID.Anvils)
            .AddCondition(Condition.InGraveyard)
            .Register();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}