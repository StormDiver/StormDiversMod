using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
   
    public class IronShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Iron Shot");
            //Tooltip.SetDefault("Heavy bullet that obeys gravity and has a strong knockback");
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 3);
            Item.rare = ItemRarityID.White;

            Item.DamageType = DamageClass.Ranged;

            Item.damage = 6;

            Item.knockBack = 5.5f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.IronShotProj>();
            Item.shootSpeed = 2.5f;
            Item.ammo = AmmoID.Bullet;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<IronShot>(), 70);
            recipe.AddIngredient(ItemID.MusketBall, 70);
            recipe.AddIngredient(ItemID.IronBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class LeadShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lead Shot");
            //Tooltip.SetDefault("Heavy bullet that obeys gravity and has a strong knockback");
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 3);
            Item.rare = ItemRarityID.White;

            Item.DamageType = DamageClass.Ranged;

            Item.damage = 7;

            Item.knockBack = 5f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType <Projectiles.AmmoProjs.LeadShotProj>();
            Item.shootSpeed = 3f;
            Item.ammo = AmmoID.Bullet;
        }
        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<LeadShot>(), 70);
            recipe.AddIngredient(ItemID.MusketBall, 70);
            recipe.AddIngredient(ItemID.LeadBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
