using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Weapons
{
    public class StickyBomb : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spike Bomb");
            //Tooltip.SetDefault("Explodes into many damaging spikes\nDoes not destroy tiles");
            Item.ResearchUnlockCount = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 2, 50);
            Item.rare = ItemRarityID.Orange;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.DamageType = DamageClass.Ranged;
    
            Item.damage = 40;

            Item.knockBack = 2f;
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<Projectiles.SpikeBombProj>();
            Item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<StickyBomb>(), 20);
            recipe.AddIngredient(ItemID.Bomb, 20);
            recipe.AddRecipeGroup("StormDiversMod:EvilBars", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

        }

    }
}
