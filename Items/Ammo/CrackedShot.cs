using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class CrackedShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cracked Bullet");
            Tooltip.SetDefault("Has a chance to shatter into 4 shards in flight");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 15);
            Item.rare = ItemRarityID.Orange;


            Item.DamageType = DamageClass.Ranged;


            Item.damage = 6;
            Item.crit = 0;
            Item.knockBack = 0f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.CrackedShotProj>();
            Item.shootSpeed = 5f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<CrackedShot>(), 150);
            recipe.AddIngredient(ItemID.MusketBall, 150);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.CrackedHeart>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

        }

    }
}