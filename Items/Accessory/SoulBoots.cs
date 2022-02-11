using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;


namespace StormDiversMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Shoes)]
    public class SoulBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Striders");
            Tooltip.SetDefault("Greatly increases movement speed and acceleration, and allows flight\n'Speed throughout the day and the night'");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ShoesLayer.RegisterData(Item.shoeSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Shoes")
            });
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<StormPlayer>().soulBoots = true;

           
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.SpectreBoots, 1)
            .AddIngredient(ItemID.HallowedBar, 6)
            .AddIngredient(ItemID.SoulofLight, 10)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
}