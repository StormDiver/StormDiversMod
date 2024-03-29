﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class OceanShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Coral Bullet");
            //Tooltip.SetDefault("Unaffected by water but has a limited range\nPierces once");
            Item.ResearchUnlockCount = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 18;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 2);
            Item.rare = ItemRarityID.Blue;

            //Item.melee = true;
            Item.DamageType = DamageClass.Ranged;

            //Item.magic = true;
            //Item.summon = true;
            //Item.thrown = true;

            Item.damage = 4;
            
            Item.knockBack = 0f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.CoralBulletProj>();
            Item.shootSpeed = 0f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<OceanShard>(), 200);
            recipe.AddIngredient(ItemID.Coral, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

           
        }
       
    }
}