using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class ChaosArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Chaos Arrow");
            //Tooltip.SetDefault("Teleports to the cursor after hitting an enemy, maintaining its velocity but losing 50% damage\n'Unleash the Chaos'");
            Item.ResearchUnlockCount = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 40;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 40);
            Item.rare = ItemRarityID.LightRed;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 4;

            Item.knockBack = 2.5f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.ChaosArrowProj>();
            Item.shootSpeed = 4f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<ChaosArrow>(), 100);
            recipe.AddIngredient(ItemID.WoodenArrow, 100);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ChaosShard>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}