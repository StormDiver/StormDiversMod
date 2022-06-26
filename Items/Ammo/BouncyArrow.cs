using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Ammo
{
    public class BouncyArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bouncy Arrow");
            Tooltip.SetDefault("Super bouncy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99; 
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 3);
            Item.rare = ItemRarityID.Blue;

            Item.DamageType = DamageClass.Ranged;


            Item.damage = 7;

            Item.knockBack = 0f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.BouncyArrowProj>();
            Item.shootSpeed = 1f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
         
            Recipe recipe = Recipe.Create(ModContent.ItemType<BouncyArrow>(), 25);
            recipe.AddIngredient(ItemID.WoodenArrow, 25);
            recipe.AddIngredient(ItemID.PinkGel, 1);

            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

           
        }
    }
}
