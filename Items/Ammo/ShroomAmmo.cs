using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Ammo
{
    public class ShroomArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroomite Arrow");
            //Tooltip.SetDefault("Emits damaging mushrooms in flight/nCan pierce once");
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 36;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 16);
            Item.rare = ItemRarityID.Yellow;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 10;
            Item.knockBack = 5f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.ShroomArrowProj>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Arrow;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<ShroomArrow>(), 150);
            recipe.AddIngredient(ItemID.ShroomiteBar, 1);
            recipe.AddIngredient(ItemID.WoodenArrow, 150);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    public class ShroomBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroomite Bullet");
            //Tooltip.SetDefault("Ricochets off walls thrice and pierces twice");
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 16);
            Item.rare = ItemRarityID.Yellow;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 15;
            Item.crit = 6;
            Item.knockBack = 3f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.ShroomBulletProj>();
            Item.shootSpeed = 5f;
            Item.ammo = AmmoID.Bullet;
        }
        public override void OnConsumeAmmo(Item ammo, Player player)
        {

        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<ShroomBullet>(), 150);
            recipe.AddIngredient(ItemID.ShroomiteBar, 1);
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
