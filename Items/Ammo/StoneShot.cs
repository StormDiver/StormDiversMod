using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class StoneShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Compact Boulder");
            Tooltip.SetDefault("For use with Stone Launchers");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Blue;
            Item.DamageType = DamageClass.Ranged;

            Item.damage = 10;
            
            Item.knockBack = 4f;
            Item.consumable = true;
            
            Item.shoot = ModContent.ProjectileType<Projectiles.StoneProj>();
            Item.shootSpeed = 5f;
            Item.ammo = Item.type;
        }

        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<StoneShot>(), 111);
            recipe.AddIngredient(ItemID.StoneBlock, 333);    
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

            recipe = Recipe.Create(ItemID.StoneBlock, 333);
            recipe.AddIngredient(ModContent.ItemType<StoneShot>(), 111);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

            

        }

    }
}
