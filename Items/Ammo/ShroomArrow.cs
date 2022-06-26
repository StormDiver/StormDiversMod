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
            DisplayName.SetDefault("Shroomite Arrow");
            Tooltip.SetDefault("Can pierce once\nLeaves behind a trail of damaging mushrooms");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 36;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 16);
            Item.rare = ItemRarityID.Yellow;

            //Item.melee = true;
            Item.DamageType = DamageClass.Ranged;
            //Item.magic = true;
            //Item.summon = true;
            //Item.thrown = true;

            Item.damage = 20;
            Item.crit = 6;
            Item.knockBack = 2f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.ShroomArrowProj>();
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
}
