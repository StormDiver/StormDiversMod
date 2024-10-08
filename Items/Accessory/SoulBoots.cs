using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Common;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace StormDiversMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Shoes)]
    public class SoulBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Soul Striders");
            //Tooltip.SetDefault("The wearer can run up to 46mph, and increases acceleration\n
            //\nIncreases jump speed, allows auto-jump, and increases fall resistance\n'Speed throughout the day and the night'");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            ShoesLayer.RegisterData(Item.shoeSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Shoes")
            });
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {

            }
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
            

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().soulBoots = true;
            player.frogLegJumpBoost = true;
            player.autoJump = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.AmphibianBoots, 1)
            .AddIngredient(ItemID.HallowedBar, 10)
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