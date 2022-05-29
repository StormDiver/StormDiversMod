using System;
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
	public class DerplingSword : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Derpling Sword"); 
			Tooltip.SetDefault("Fire out multiple Derpling Shell Shards every other swing");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

		public override void SetDefaults() 
		{
			Item.damage = 80;

            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
			Item.height = 50;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;  
            Item.value = Item.sellPrice(0, 5, 0, 0);
                     Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 6;
            Item.shoot = ModContent.ProjectileType < Projectiles.DerpMeleeProj>();
            Item.shootSpeed = 15f;
            Item.scale = 1.2f;
        }
        int weaponattack = 2;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            weaponattack--;
            if (weaponattack <= 0)
            {
                int numberProjectiles = 2 + Main.rand.Next(3); //This defines how many projectiles to shot.
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(12)); // This defines the projectiles random spread . 10 degree spread.
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, (int)(damage * 0.75f), knockback, player.whoAmI);
                }
                SoundEngine.PlaySound(SoundID.NPCHit22, player.Center);
                weaponattack = 2;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 10)
           .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 6)
           .AddTile(TileID.MythrilAnvil)
           .Register();

          
        }

    }
    //_______________________________________________________________________
    public class DerplingGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Derpling Rifle");
            Tooltip.SetDefault("I know it looks cruel, but it had to be done\nFour round burst, only the first shot consumes ammo");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {

            Item.width = 50;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 4;
            Item.useAnimation = 14;
            Item.reuseDelay = 15;
            Item.useTurn = false;
            Item.autoReuse = true;
            //Item.UseSound = SoundID.Item38;
            Item.DamageType = DamageClass.Ranged;


            Item.damage = 35;
            Item.crit = 6;
            Item.knockBack = 2f;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 10f;

            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }


        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, -3);
        }

        //int secondfire = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            SoundEngine.PlaySound(SoundID.Item40, position);
            {
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
            }

            return false;

        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
     
            return !(player.itemAnimation < Item.useAnimation - 2);

        }
       
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ItemID.ChlorophyteBar, 10)
        .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 6)
        .AddTile(TileID.MythrilAnvil)
        .Register();
        }
    }
    //_________________________________________________________________________
    public class DerplingStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Derpling Scepter");
            Tooltip.SetDefault("Rapidly fires out magical Derpling Shell Shards\nHas a small chance to fire out a larger shard that homes and explodes into smaller shards");
            Item.staff[Item.type] = true;

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 54;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.autoReuse = true;
            // Item.UseSound = SoundID.Item43;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 48;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item13;
            Item.shoot = ModContent.ProjectileType < Projectiles.DerpMagicProj2>();
            Item.shootSpeed = 18f;
            Item.mana = 5;

            Item.noMelee = true; //Does the weapon itself inflict damage?

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            int choice = Main.rand.Next(10);
            if (choice == 0)
            {
                {

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(18));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X * 0.6f, perturbedSpeed.Y * 0.6f), ModContent.ProjectileType<Projectiles.DerpMagicProj>(), damage, knockback, player.whoAmI);
                    SoundEngine.PlaySound(SoundID.Item20, player.Center);

                }
            }
            else
            {
                {

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(7));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.DerpMagicProj2>(), damage, knockback, player.whoAmI);

                }
            }
            return false;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.ChlorophyteBar, 10)
         .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 6)
         .AddTile(TileID.MythrilAnvil)
         .Register();
        }
    }
    //_________________________________________________________
    public class DerplingMinion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Derpling Staff");
            Tooltip.SetDefault("Summons a buffed baby Derpling to fight for you");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 46;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.autoReuse = true;
            // Item.UseSound = SoundID.Item43;

            Item.damage = 30;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item43;


            Item.mana = 10;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;

            Item.UseSound = SoundID.Item44;

            Item.buffType = ModContent.BuffType<Projectiles.Minions.DerplingMinionBuff>();
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.DerplingMinionProj>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(8, 8);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            player.AddBuff(Item.buffType, 2);

            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);

            return false;



        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.ChlorophyteBar, 10)
           .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 6)
           .AddTile(TileID.MythrilAnvil)
           .Register();

        }
    }
}