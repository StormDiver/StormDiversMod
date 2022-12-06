using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
	public class FrostSpinner : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Frozen Polestar"); 
			Tooltip.SetDefault("Spins around and knocks enemies in the direction you're facing\nInflicts cryoburn on enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

		public override void SetDefaults() 
		{
			Item.damage = 40;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 50;
			Item.height = 74;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 100;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item7;
			Item.autoReuse = false;
            Item.useTurn = false;
            Item.channel = true;
            Item.knockBack = 3f;
            Item.shoot = ModContent.ProjectileType<Projectiles.FrostSpinProj>();
            Item.shootSpeed = 1f;
            Item.noMelee = true; 
            Item.noUseGraphic = true; 
            
        }
        public override void UseItemFrame(Player player)     //this defines what frame the player use when this weapon is used
        {
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 14)
           .AddTile(TileID.MythrilAnvil)
           .Register();

        }
    }
    //_______________________________________________________________________________
    public class FrostStar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Frizbee");
            Tooltip.SetDefault("Throws out a frizbee that shatters on impact");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {

            Item.damage = 45;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 38;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.shootSpeed = 14f;
            Item.shoot = ModContent.ProjectileType<Projectiles.FrostStarProj>();
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;

        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 3;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            SoundEngine.PlaySound(SoundID.Item1, position);

            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 14)
          .AddTile(TileID.MythrilAnvil)
          .Register();
        }
    }
    //_______________________________________________________________________________
    public class FrostLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Launcher");
            Tooltip.SetDefault("Fires out impact-exploding grenades that inflict CryoBurn\nRequires Prototype Grenades, purchase more from the Demolitionist");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            //Item.reuseDelay = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.ProtoGrenadeProj>();
            Item.useAmmo = ItemType<Ammo.ProtoGrenade>();
            Item.UseSound = SoundID.Item61;

            Item.damage = 45;
            //Item.crit = 4;
            Item.knockBack = 3f;
            Item.shootSpeed = 10f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
          
            for (int i = 0; i < 1; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(3));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.FrostGrenadeProj>(), damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item61, position);

            }



            return false;
        }

        /* public override bool ConsumeAmmo(Player player)
         {
             return Main.rand.NextFloat() >= .33f;
         }*/
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 14)
          .AddTile(TileID.MythrilAnvil)
          .Register();
        }


    }
    //_______________________________________________________________________________
    public class Frostthrower : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cryofreezer");
            Tooltip.SetDefault("Fires out a stream of super cold gas which inflicts CryoBurn\nUses gel for ammo\nIgnores 15 points of enemy defense");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {

            Item.width = 40;
            Item.height = 24;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 4;
            Item.useAnimation = 13;

            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item20;

            Item.damage = 21;
            Item.knockBack = 0.25f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Frostthrowerproj>();

            Item.shootSpeed = 3f;

            Item.useAmmo = AmmoID.Gel;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        bool candamage;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 3), new Vector2(velocity.X + player.velocity.X / 8, velocity.Y + player.velocity.Y / 8), type, damage, knockback, player.whoAmI);               
            return false;
        }
        public override void HoldItem(Player player)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 14)
          .AddTile(TileID.MythrilAnvil)
          .Register();

        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {

            return !(player.itemAnimation < Item.useAnimation - 2);

        }
    }
    public class FrostSentry : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frostspike Staff");
            Tooltip.SetDefault("Summons a frost sentry that fires piercing icicles at high speed at enemies");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            //Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
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
            Item.UseSound = SoundID.Item46;

            Item.damage = 60;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.SentryProjs.FrostSentryProj>();

            //Item.shootSpeed = 3.5f;

            Item.noMelee = true;
        }
        /* public override Vector2? HoldoutOffset()
         {
             return new Vector2(5, 0);
         }*/
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y - 37), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
        .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 14)
        .AddTile(TileID.MythrilAnvil)
        .Register();

        }
    }
}