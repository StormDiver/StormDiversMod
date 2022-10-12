using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class FrostBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Bullet");
            Tooltip.SetDefault("Can pierce through walls, but scatters when doing so");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 40);
            Item.rare = ItemRarityID.Pink;


            Item.DamageType = DamageClass.Ranged;


            Item.damage = 10;
            Item.crit = 0;
            Item.knockBack = 0f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.FrostBulletProj>();
            Item.shootSpeed = 5f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            

            Recipe recipe = Recipe.Create(ModContent.ItemType<FrostBullet>(), 100);
            recipe.AddIngredient(ItemID.MusketBall, 100);
            recipe.AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        
    }
    //________________________________________________________________________
    public class FrostArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Arrow");
            Tooltip.SetDefault("Explodes into tiny shards on impact");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 44;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 40);
            Item.rare = ItemRarityID.Pink;


            Item.DamageType = DamageClass.Ranged;


            Item.damage = 15;
     
            Item.knockBack = 2f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.FrostArrowProj>();
            Item.shootSpeed = 3f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            

            Recipe recipe = Recipe.Create(ModContent.ItemType<FrostArrow>(), 100);
            recipe.AddIngredient(ItemID.WoodenArrow, 100);
            recipe.AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
