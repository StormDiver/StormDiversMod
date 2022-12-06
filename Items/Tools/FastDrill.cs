using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Tools
{
    public class FastDrill : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Drill");
            Tooltip.SetDefault("'Speeds up your mining experience'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 22;
        
            Item.useTime = 5;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.knockBack = 1f;
           
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.FastDrillProj>();
            Item.shootSpeed = 27f;
            Item.pick = 45;
            Item.tileBoost = -1;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true; 
            Item.channel = true; 
            Item.autoReuse = true;
            
            
        }
       
    }
    public class FastDrill2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mega Drill");
            Tooltip.SetDefault("'Greatly speeds up your mining experience'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {

            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 22;

            Item.useTime = 4;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.knockBack = 1f;
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.FastDrill2Proj>();
            Item.shootSpeed = 36f;
            Item.pick = 100;
            Item.tileBoost = 0;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;


        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<FastDrill>(), 1)
            .AddRecipeGroup("StormDiversMod:EvilBars", 10)
            .AddIngredient(ItemID.HellstoneBar, 10)
            .AddIngredient(ItemID.MeteoriteBar, 10)
            .AddTile(TileID.Anvils)
            .Register();


        }
    }
}