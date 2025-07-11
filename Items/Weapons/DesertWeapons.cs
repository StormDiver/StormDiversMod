using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using System.Collections.Generic;


namespace StormDiversMod.Items.Weapons
{
	public class DesertSpear : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName.SetDefault("Forbidden Pike"); 
			//Tooltip.SetDefault("Unleash the power of the forbidden sands");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.Spears[Item.type] = true;

        }
        public override void SetDefaults() 
		{
			Item.damage = 30;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
			Item.height = 50;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 4f;
            Item.shoot = ModContent.ProjectileType<Projectiles.DesertSpearProj>();
            Item.shootSpeed = 5f;
            Item.noMelee = true; 
            Item.noUseGraphic = true; 
            
        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X * 0.5f, perturbedSpeed.Y * 0.5f), ModContent.ProjectileType<Projectiles.DesertSpearTipProj>(), damage, knockback, player.whoAmI);
            return true;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
        .AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 14)
        .AddTile(TileID.MythrilAnvil)
        .Register();
        }
    }
    //_______________________________________________________________________________________________
    public class DesertBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Fury");
            //Tooltip.SetDefault("Converts all arrows to Ancient Forbidden arrows that rain down the heat of the Desert");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 34;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item5;
            Item.damage = 45;
            //Item.crit = 4;
            Item.knockBack = 5f;

            Item.shoot = ModContent.ProjectileType<Projectiles.DesertBowProj>();
            Item.shootSpeed = 8f;
            Item.useAmmo = AmmoID.Arrow;
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        int forbiddenarrow;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ModContent.ProjectileType<Projectiles.AmmoProjs.DesertArrowProj>())
                forbiddenarrow = 1;
            else
                forbiddenarrow = 0;
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.DesertBowProj>(), damage, knockback, player.whoAmI, 0, forbiddenarrow);
            //Main.NewText("Forbidden Arrow? " + forbiddenarrow, Color.Orange);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 14)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
    //_______________________________________________________________________________________________
    public class DesertSpell : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Storm");
            //Tooltip.SetDefault("Summons the Forbidden sands");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 18;
            }
            else
            {
                Item.mana = 12;
            }
            Item.UseSound = SoundID.Item20;

            Item.damage = 37;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.DesertSpellProj>();

            Item.shootSpeed = 3f;

            //Item.useAmmo = AmmoID.Arrow;
            Item.scale = 0.9f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            for (int i = 0; i < 3; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20)); // This defines the projectiles random spread . 10 degree spread.
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
       .AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 14)
       .AddTile(TileID.MythrilAnvil)
       .Register();


        }
    }

    //_______________________________________________________________________________________________
    public class DesertStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Staff");
            //Tooltip.SetDefault("Summons a floating Forbidden Sentry that blasts sand in all directions");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            //Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 10;
            Item.UseSound = SoundID.Item78;

            Item.damage = 32;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.SentryProjs.DesertStaffProj>();

            //Item.shootSpeed = 3.5f;



            Item.noMelee = true;
        }
        /* public override Vector2? HoldoutOffset()
         {
             return new Vector2(5, 0);
         }*/
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
       .AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 14)
       .AddTile(TileID.MythrilAnvil)
       .Register();

        }
    }

    //________________________________
    public class DesertWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Whip");
            //Tooltip.SetDefault("12% summon tag critical strike chance\nYour summons will focus struck enemies\nForbidden sand jumps from the targeted enemy when hit by summons");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.WhipProjs.DesertWhipProj>(), 40, 2, 4, 30);
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            //Item.useTime = 30;
            //Item.useAnimation = 30;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            //Item.damage = 40;
            //Item.knockBack = 1f;
            //Item.shootSpeed = 4f;
            
            Item.noMelee = true;
        }
        public override bool MeleePrefix()
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
       .AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 14)
       .AddTile(TileID.MythrilAnvil)
       .Register();

        }
    }
}