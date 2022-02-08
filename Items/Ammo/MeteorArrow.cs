using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class MeteorArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor Arrow");
            Tooltip.SetDefault("Slightly attracted towards enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Blue;

            Item.DamageType = DamageClass.Ranged;


            Item.damage = 5;

            Item.knockBack = 0f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.MeteorArrowProj>();
            Item.shootSpeed = 1f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {


            Recipe recipe = Mod.CreateRecipe(ModContent.ItemType<MeteorArrow>(), 100);
            recipe.AddIngredient(ItemID.WoodenArrow, 100);
            recipe.AddIngredient(ItemID.MeteoriteBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
