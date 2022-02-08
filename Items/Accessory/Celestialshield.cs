using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;            
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
    public class Celestialshield : ModItem
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Celestial Barrier");
            Tooltip.SetDefault("Grants immunity to debuffs inflicted by Extra-Terrestrial creatures\nTaking heavy damage greatly increases health regeneration while protecting you\nDuration depends on the amount of damage received");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {

            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
           
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.buffImmune[BuffID.VortexDebuff] = true;
            player.buffImmune[BuffID.Obstructed] = true;
            player.buffImmune[BuffID.Electrified] = true;
            player.buffImmune[ModContent.BuffType<Buffs.ScanDroneDebuff>()] = true;
            player.buffImmune[ModContent.BuffType<Buffs.NebulaDebuff>()] = true;
            player.noKnockback = true;
            player.GetModPlayer<StormPlayer>().lunarBarrier = true;

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