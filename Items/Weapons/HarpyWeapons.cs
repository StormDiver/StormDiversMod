﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
    public class HarpyStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Feather Scepter");
            //Tooltip.SetDefault("Fires out a spread of 3 feathers\nThe center feather deals more damage and pierces once");
            Item.staff[Item.type] = true;

            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 34;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 14;
            }
            else
            {
                Item.mana = 9;
            }
            Item.UseSound = SoundID.Item8;

            Item.damage = 12;
         
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.HarpyProj2>();

            Item.shootSpeed = 11f;
   
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 40f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            float numberProjectiles = 2;
            float rotation = MathHelper.ToRadians(6);

            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.HarpyProj>(), damage, knockback, player.whoAmI);
            }
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.HarpyProj2>(), (int)(damage * 1.5f), knockback, player.whoAmI);

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddRecipeGroup("StormDiversMod:EvilBars", 10)
          .AddIngredient(ItemID.Feather, 6)
          .AddTile(TileID.Anvils)
          .Register();         
        }
    }
    //_____________________________________________________________________________________________________________________________
    public class HarpyBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Feathered Bow");
            //Tooltip.SetDefault("Converts Wooden Arrows into Feather Arrows that ignore gravity and pierce");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item5;

            Item.damage = 18;
            //Item.crit = 4;
            Item.knockBack = 3f;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 7f;
            Item.useAmmo = AmmoID.Arrow;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<Projectiles.HarpyArrowProj>();
            }
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddRecipeGroup("StormDiversMod:EvilBars", 10)
           .AddIngredient(ItemID.Feather, 6)
           .AddTile(TileID.Anvils)
           .Register();

        }
    }
    //_______________________________________________________________
    public class HarpyYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Feather Thrower");
            //Tooltip.SetDefault("Launches sharp feathers at nearby enemies");
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 25;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            //Item.crit = 0;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 20;
            Item.height = 26;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.useTurn = true;
            Item.knockBack = 4f;
            Item.shoot = ModContent.ProjectileType<Projectiles.HarpyYoyoProj>();
            Item.shootSpeed = 9f;
            Item.noMelee = true;
            Item.noUseGraphic = true;

        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            /* projshoot++;
             if (projshoot >= 2)
             {
                 Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
                 Projectile.NewProjectile(position.X, position.Y, (float)(perturbedSpeed.X * 1f), (float)(perturbedSpeed.Y * 1f), mod.ProjectileType("TurtleProj"), (int)(damage * 1.5), knockBack, player.whoAmI);
                 Main.PlaySound(SoundID.NPCHit, (int)player.Center.X, (int)player.Center.Y, 24);
                 projshoot = 0;
             }*/
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
          .AddRecipeGroup("StormDiversMod:EvilBars", 10)
          .AddIngredient(ItemID.Feather, 6)
          .AddTile(TileID.Anvils)
          .Register();

        }
    }
     //______________________________________________________________________________________________________
    public class HarpyMinion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Feather Command Staff");
            //Tooltip.SetDefault("Summons a special feather minion to fight for you");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.knockBack = 0.1f;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<Projectiles.Minions.HarpyMinionBuff>();
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.HarpyMinionProj>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);

            // Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position.
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddRecipeGroup("StormDiversMod:EvilBars", 10)
          .AddIngredient(ItemID.Feather, 6)
          .AddTile(TileID.Anvils)
          .Register();

        }
    }
}