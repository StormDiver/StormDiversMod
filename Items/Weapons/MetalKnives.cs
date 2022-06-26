using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using StormDiversMod;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;


namespace StormDiversMod.Items.Weapons
{
    public class MetalSilverKnive : ModItem 
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver Throwing Knife");
            Tooltip.SetDefault("Ricochets off tiles");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 14;
            if (GetInstance<ConfigurationsGlobal>().ThrowingTryhards)
            {
                Item.DamageType = DamageClass.Throwing;
            }
            else
            {
                Item.DamageType = DamageClass.Ranged;

            }
            Item.width = 10;
            Item.height = 10;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 0, 0, 15);
            Item.rare = ItemRarityID.White;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<ThrowingSilverKniveProj>();
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;

        }

        
        public override void AddRecipes()
        {
            
            Recipe recipe = Recipe.Create(ModContent.ItemType<MetalSilverKnive>(), 100);
            recipe.AddIngredient(ItemID.ThrowingKnife, 100);
            recipe.AddIngredient(ItemID.SilverBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class MetalTungstenKnive : ModItem //Actually Throwing knives
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tungsten Throwing Knife");
            Tooltip.SetDefault("Ricochets off tiles");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.damage = 15;
            if (GetInstance<ConfigurationsGlobal>().ThrowingTryhards)
            {
                Item.DamageType = DamageClass.Throwing;
            }
            else
            {
                Item.DamageType = DamageClass.Ranged;

            }
            Item.width = 10;
            Item.height = 10;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.knockBack = 2.5f;
            Item.value = Item.sellPrice(0, 0, 0, 15);
            Item.rare = ItemRarityID.White;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<ThrowingTungstenKniveProj>();
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;

        }



        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<MetalTungstenKnive>(), 100);
            recipe.AddIngredient(ItemID.ThrowingKnife, 100);
            recipe.AddIngredient(ItemID.TungstenBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
   
}
