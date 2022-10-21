using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
    public class SeekerBolt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hunter's Blade");
            Tooltip.SetDefault("A large throwable blade that homes into enemies when within range");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 2f;
            Item.shoot = ModContent.ProjectileType<Projectiles.SeekerKnifeProj>();
            Item.shootSpeed = 22f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.HallowedBar, 15)
            .AddIngredient(ItemID.SoulofMight, 10)
            .AddIngredient(ItemID.SoulofSight, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();

        }

    }
}
