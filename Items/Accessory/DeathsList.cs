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


namespace StormDiversMod.Items.Accessory
{

    public class DeathsList : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Repear's list");
            //Tooltip.SetDefault("Kill enemies to collect up to 9 souls, each collected soul increases the damage and crit chance of the weapon
            //Souls will begin to escape if no enemies are killed within 9 seconds");
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