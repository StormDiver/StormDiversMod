using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class AsteroidArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asteroid Arrow");
            Tooltip.SetDefault("Quickly charges towards the cursor after a short delay, pierces after charging");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 36;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.rare = ItemRarityID.Yellow;

            //Item.melee = true;
            Item.DamageType = DamageClass.Ranged;
            //Item.magic = true;
            //Item.summon = true;
            //Item.thrown = true;

            Item.damage = 12;
            Item.knockBack = 2f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.AsteroidArrowProj>();
            Item.shootSpeed = 1f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<AsteroidArrow>(), 150);
            recipe.AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 1);
            recipe.AddIngredient(ItemID.WoodenArrow, 150);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();


        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    public class AsteroidBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asteroid Bullet");
            Tooltip.SetDefault("Summons a small asteroid fragment above struck enemies\nThe fragment deals low damage but ignores 50 defense");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.rare = ItemRarityID.Yellow;


            Item.DamageType = DamageClass.Ranged;


            Item.damage = 10;
            Item.knockBack = 1f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.AsteroidBulletProj>();
            Item.shootSpeed = 2f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void OnConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.NextBool(10))
            {


            }
        }

        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<AsteroidBullet>(), 150);
            recipe.AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 1);
            recipe.AddIngredient(ItemID.MusketBall, 150);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
