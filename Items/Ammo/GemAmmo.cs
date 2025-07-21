using Microsoft.Xna.Framework;
using rail;
using StormDiversMod.Projectiles;
using StormDiversMod.Projectiles.AmmoProjs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace StormDiversMod.Items.Ammo
{
    public class GemAmethystBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Amethyst Bullet");
            //Tooltip.SetDefault("Can mine blocks on impact");
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 2);
            Item.rare = ItemRarityID.White;

            Item.DamageType = DamageClass.Ranged;

            Item.damage = 4;
            Item.crit = 0;
            Item.knockBack = 0.5f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.GemAmethystBulletProj>();
            Item.shootSpeed = 2.5f;
            Item.ammo = AmmoID.Bullet;
        }
        //It literally just ignores this >:(
        /*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projid = Projectile.NewProjectile(source, velocity, position, ModContent.ProjectileType<GemAmmoBulletProj>(), damage, knockback, Main.myPlayer, 0, 0, 0);
            Main.projectile[projid].ai[2] = 0;
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            int projtype = ModContent.ProjectileType<GemAmmoBulletProj>();
            Main.projectile[projtype].ai[2] = 0;
            type = projtype;
        }*/
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<GemAmethystBullet>(), 250);
            recipe.AddIngredient(ItemID.MusketBall, 250);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class GemTopazBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Topaz Bullet");
            //Tooltip.SetDefault("Can mine blocks on impact");
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 3);
            Item.rare = ItemRarityID.White;

            Item.DamageType = DamageClass.Ranged;

            Item.damage = 5;
            Item.crit = 0;
            Item.knockBack = 0.5f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.GemTopazBulletProj>();
            Item.shootSpeed = 2.5f;
            Item.ammo = AmmoID.Bullet;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            int projtype = ModContent.ProjectileType<GemAmmoBulletProj>();
            Main.projectile[projtype].ai[2] = 1;
            type = projtype;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<GemTopazBullet>(), 250);
            recipe.AddIngredient(ItemID.MusketBall, 250);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class GemSapphireBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sapphire Bullet");
            //Tooltip.SetDefault("Can mine blocks on impact");
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 5);
            Item.rare = ItemRarityID.Blue;

            Item.DamageType = DamageClass.Ranged;

            Item.damage = 5;
            Item.crit = 0;
            Item.knockBack = 1f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.GemSapphireBulletProj>();
            Item.shootSpeed = 2.5f;
            Item.ammo = AmmoID.Bullet;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            int projtype = ModContent.ProjectileType<GemAmmoBulletProj>();
            Main.projectile[projtype].ai[2] = 2;
            type = projtype;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<GemSapphireBullet>(), 250);
            recipe.AddIngredient(ItemID.MusketBall, 250);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class GemEmeraldBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Emerald Bullet");
            //Tooltip.SetDefault("Can mine blocks on impact");
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 6);
            Item.rare = ItemRarityID.Blue;

            Item.DamageType = DamageClass.Ranged;

            Item.damage = 6;
            Item.crit = 0;
            Item.knockBack = 1f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.GemEmeraldBulletProj>();
            Item.shootSpeed = 2.5f;
            Item.ammo = AmmoID.Bullet;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            int projtype = ModContent.ProjectileType<GemAmmoBulletProj>();
            Main.projectile[projtype].ai[2] = 3;
            type = projtype;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<GemEmeraldBullet>(), 250);
            recipe.AddIngredient(ItemID.MusketBall, 250);
            recipe.AddIngredient(ItemID.Emerald, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class GemRubyBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ruby Bullet");
            //Tooltip.SetDefault("Can mine blocks on impact");
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 9);
            Item.rare = ItemRarityID.Green;

            Item.DamageType = DamageClass.Ranged;

            Item.damage = 6;
            Item.crit = 0;
            Item.knockBack = 1.5f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.GemRubyBulletProj>();
            Item.shootSpeed = 2.5f;
            Item.ammo = AmmoID.Bullet;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            int projtype = ModContent.ProjectileType<GemAmmoBulletProj>();
            Main.projectile[projtype].ai[2] = 4;
            type = projtype;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<GemRubyBullet>(), 250);
            recipe.AddIngredient(ItemID.MusketBall, 250);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class GemAmberBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Amber Bullet");
            //Tooltip.SetDefault("Can mine blocks on impact");
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 12);
            Item.rare = ItemRarityID.Green;

            Item.DamageType = DamageClass.Ranged;

            Item.damage = 6;
            Item.crit = 0;
            Item.knockBack = 1.5f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.GemAmberBulletProj>();
            Item.shootSpeed = 2.5f;
            Item.ammo = AmmoID.Bullet;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            int projtype = ModContent.ProjectileType<GemAmmoBulletProj>();
            Main.projectile[projtype].ai[2] = 5;
            type = projtype;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<GemAmberBullet>(), 250);
            recipe.AddIngredient(ItemID.MusketBall, 250);
            recipe.AddIngredient(ItemID.Amber, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class GemDiamondBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Diamond Bullet");
            //Tooltip.SetDefault("Can mine blocks on impact");
            Item.ResearchUnlockCount = 99;
        }
        //int pickpower;
        public override void UpdateInventory(Player player)
        {
            /*if (player.GetBestPickaxe() != null)
            {
                Item bestPickaxe = player.GetBestPickaxe();
                int pickpower = bestPickaxe.pick;
            }*/
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 12);
            Item.rare = ItemRarityID.Green;

            Item.DamageType = DamageClass.Ranged;

            Item.damage = 7;
            Item.crit = 0;
            Item.knockBack = 1.5f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.GemDiamondBulletProj>();
            Item.shootSpeed = 2.5f;
            Item.ammo = AmmoID.Bullet;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            int projtype = ModContent.ProjectileType<GemAmmoBulletProj>();
            Main.projectile[projtype].ai[2] = 6;
            type = projtype;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<GemDiamondBullet>(), 250);
            recipe.AddIngredient(ItemID.MusketBall, 250);
            recipe.AddIngredient(ItemID.Diamond, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
