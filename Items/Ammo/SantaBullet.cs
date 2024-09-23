using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class SantaBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Santa's Bullet");
            //Tooltip.SetDefault("Ricochets twice, exploding on the third impact, or on impact with enemies\n'You must be on the super naughty list to have one of these'");
            Item.ResearchUnlockCount = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 15);
            Item.rare = ItemRarityID.Yellow;

            Item.DamageType = DamageClass.Ranged;

            Item.damage = 13;
            Item.knockBack = 6f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.SantaBulletProj>();
            Item.shootSpeed = 5f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<SantaBullet>(), 150);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SantankScrap>(), 1);
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
