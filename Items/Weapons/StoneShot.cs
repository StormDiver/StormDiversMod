using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;

using StormDiversMod.Basefiles;
using static Terraria.ModLoader.ModContent;


namespace StormDiversMod.Items.Weapons
{
    public class StoneShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Compact Boulder");
            Tooltip.SetDefault("You might have trouble throwing these far");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.White;

            Item.DamageType = DamageClass.Ranged;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.damage = 16;
            Item.knockBack = 4f;
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<Projectiles.StoneProj>();
            Item.shootSpeed = 5f;
            if (ModLoader.HasMod("ThoriumMod"))
            {
                Item.ammo = Item.type;
            }
        }

        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<StoneShot>(), 1);
            recipe.AddIngredient(ItemID.StoneBlock, 3);    
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

            recipe = Recipe.Create(ItemID.StoneBlock, 3);
            recipe.AddIngredient(ModContent.ItemType<StoneShot>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

    }
}
