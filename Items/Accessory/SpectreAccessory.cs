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
            DisplayName.SetDefault("Spectre Skull");
            Tooltip.SetDefault("Mana usage is negated when under the effects of mana sickness\nIncreases maximum mana by 40");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 30;

            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Yellow;

            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 40;
            player.manaFlower = true;
            if (Main.LocalPlayer.HasBuff(BuffID.ManaSickness))
            {
                
                player.manaCost *= 0f;
                
                    Dust dust;
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Vector2 position = Main.LocalPlayer.position;
                    dust = Main.dust[Terraria.Dust.NewDust(position, player.width, player.height, 15, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                    dust.noGravity = true;
                player.GetModPlayer<StormPlayer>().SpectreSkull = true;
            }
            

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
}