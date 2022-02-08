using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class DesertBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Bullet");
            Tooltip.SetDefault("Has a chance to spilt into two mid-flight");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 46;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 40);
            Item.rare = ItemRarityID.Pink;


            Item.DamageType = DamageClass.Ranged;


            Item.damage = 12;
            Item.crit = 0;
            Item.knockBack = 0f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.DesertBulletProj>();
            Item.shootSpeed = 5f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Mod.CreateRecipe(ModContent.ItemType<DesertBullet>(), 100);
            recipe.AddIngredient(ItemID.MusketBall, 100);
            recipe.AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
           
        }
 
    }
    //________________________________________________________________________
    public class DesertArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Arrow");
            Tooltip.SetDefault("Bounces twice, spins after bouncing");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 40;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 40);
            Item.rare = ItemRarityID.Pink;

            Item.DamageType = DamageClass.Ranged;



            Item.damage = 15;

            Item.knockBack = 2f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.DesertArrowProj>();
            Item.shootSpeed = 3f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
          
            Recipe recipe = Mod.CreateRecipe(ModContent.ItemType<DesertArrow>(), 100);
            recipe.AddIngredient(ItemID.WoodenArrow, 100);
            recipe.AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
