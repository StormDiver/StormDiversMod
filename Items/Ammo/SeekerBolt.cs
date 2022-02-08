using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class SeekerBolt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seeker Bolts");
            Tooltip.SetDefault("For use with The Seeker");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 18;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 80);
            Item.rare = ItemRarityID.Pink;

            Item.DamageType = DamageClass.Ranged; 
           

            Item.damage = 15;
            Item.knockBack = 1f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.SeekerBoltProj>();
            Item.shootSpeed = 5f;
            Item.ammo = Item.type;
        }

        public override void AddRecipes()
        {

            Recipe recipe = Mod.CreateRecipe(ModContent.ItemType<SeekerBolt>(), 333);
            recipe.AddIngredient(ItemID.HallowedBar, 1);
            recipe.AddIngredient(ItemID.SoulofSight, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

           
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
