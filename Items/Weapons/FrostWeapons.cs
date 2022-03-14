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
			DisplayName.SetDefault("Blizzard Baton"); 
			Tooltip.SetDefault("Spins with the power of a small blizzard\nKnocks enemies in the direction you're facing and inflicts CryoBurn");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

		public override void SetDefaults() 
		{
			Item.damage = 40;

            Item.DamageType = DamageClass.Melee;
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
           
            SoundEngine.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 1);

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
            Item.shoot = ModContent.ProjectileType<Projectiles.ProtoGrenadeProj>();
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
                SoundEngine.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 61);

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
            Tooltip.SetDefault("Fires out frozen gas which inflicts CryoBurn\nUses gel for ammo");
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
            Item.useTime = 8;
            Item.useAnimation = 26;

            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item20;

            Item.damage = 22;
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
        public override void HoldItem(Player player)
        {
            player.armorPenetration = 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 14)
          .AddTile(TileID.MythrilAnvil)
          .Register();

        }
        public override bool CanConsumeAmmo(Player player)
        {
          
            return Main.rand.NextFloat() > .50f;
        }
    }
}