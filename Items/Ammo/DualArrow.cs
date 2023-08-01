using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class DualArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dual Arrow");
            //Tooltip.SetDefault("Two arrows tied together, has a chance to split in midair");
            Item.ResearchUnlockCount = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Blue;

            Item.DamageType = DamageClass.Ranged;


            Item.damage = 6;

            Item.knockBack = 2.5f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.DualArrowProj>();
            Item.shootSpeed = 1f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<DualArrow>(), 50);
            recipe.AddIngredient(ItemID.WoodenArrow, 50);
            recipe.AddIngredient(ItemID.Rope, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

          
        }
    }
}
