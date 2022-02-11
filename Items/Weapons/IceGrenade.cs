using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.Items.Weapons
{
    public class IceGrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Grenade");
            Tooltip.SetDefault("Inflicts frostburn on enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            
            Item.damage = 60;
            if (GetInstance<Configurations>().ThrowingTryhards)
            {
                Item.DamageType = DamageClass.Throwing;
            }
            else
            {
                Item.DamageType = DamageClass.Ranged;

            }
            Item.width = 10;
            Item.height = 14;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;  
            Item.knockBack = 8;
            Item.value = Item.sellPrice(0, 0, 0, 16);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 5f;
            Item.shoot = ModContent.ProjectileType < Projectiles.IceGrenadeProj>();
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.noMelee = true;

        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
    
            SoundEngine.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 1);
           
                return true;
            
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Grenade, 10)
            .AddIngredient(ItemID.IceTorch, 1)
            .AddTile(TileID.Anvils)
            .Register();

        }
    }
}