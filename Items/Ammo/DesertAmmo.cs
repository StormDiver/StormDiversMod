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
            //DisplayName.SetDefault("Forbidden Bullet");
            //Tooltip.SetDefault("Creates a small cloud of forbidden dust on tile impact");
            Item.ResearchUnlockCount = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 40);
            Item.rare = ItemRarityID.Pink;


            Item.DamageType = DamageClass.Ranged;


            Item.damage = 12;
            Item.crit = 0;
            Item.knockBack = 2f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.DesertBulletProj>();
            Item.shootSpeed = 5f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<DesertBullet>(), 100);
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
            //DisplayName.SetDefault("Forbidden Arrow");
            //Tooltip.SetDefault("Speeds up after a short delay, ignoring gravity and dealing extra damage.");
            Item.ResearchUnlockCount = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 40;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 40);
            Item.rare = ItemRarityID.Pink;

            Item.DamageType = DamageClass.Ranged;

            Item.damage = 15;

            Item.knockBack = 3f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.DesertArrowProj>();
            Item.shootSpeed = 0f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
          
            Recipe recipe = Recipe.Create(ModContent.ItemType<DesertArrow>(), 100);
            recipe.AddIngredient(ItemID.WoodenArrow, 100);
            recipe.AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
