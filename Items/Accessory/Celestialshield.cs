using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;            
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;
using Terraria.UI;


namespace StormDiversMod.Items.Accessory
{
    public class Celestialshield : ModItem
    {
        public override void SetStaticDefaults()
        {
            
            //DisplayName.SetDefault("Celestial Barrier");
            //Tooltip.SetDefault("Taking heavy damage regenerates the lost life over several seconds while granting additonal defense\nGrants immunity to debuffs inflicted by Extra-Terrestrial creatures");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Red;
           
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.buffImmune[BuffID.VortexDebuff] = true;
            player.buffImmune[BuffID.Obstructed] = true;
            player.buffImmune[BuffID.Electrified] = true;
            player.buffImmune[ModContent.BuffType<Buffs.ScanDroneDebuff>()] = true;
            player.noKnockback = true;
            player.GetModPlayer<EquipmentEffects>().lunarBarrier = true;

            if (player.hideVisibleAccessory[0])
            {

            }
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FragmentSolar, 20)
            .AddIngredient(ItemID.FragmentVortex, 20)
            .AddIngredient(ItemID.FragmentNebula, 20)
            .AddIngredient(ItemID.FragmentStardust, 20)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
           
        }
        public override Color? GetAlpha(Color lightColor)
        {

            return Color.White;
        }


    }
}