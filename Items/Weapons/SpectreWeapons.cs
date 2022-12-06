using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
    public class SpectreDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectre Dagger");
            Tooltip.SetDefault("Summons magical controllable daggers\nMaximum of 5 can be controlled at any time");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Magic;

            Item.UseSound = SoundID.Item1;

            Item.damage = 42;
            //Item.crit = 4;
            Item.knockBack = 2f;

            Item.shoot = ModContent.ProjectileType<SpectreDaggerProj>();
            
            Item.shootSpeed = 16f;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 12;
            }
            else
            {
                Item.mana = 8;
            }
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void HoldItem(Player player)
        {
            
        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 5;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {       
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10)); // This defines the projectiles random spread . 10 degree spread.
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.SpectreBar, 14)
           .AddTile(TileID.MythrilAnvil)
           .Register();
            
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }
    }
    //________________________________________________________________
    public class SpectreHose : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectre Scepter");
            Tooltip.SetDefault("Rapidly fires mini Spectre skulls that speed up rapidly\nDeals more damage the faster the skulls travels");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 11;
            }
            else
            {
                Item.mana = 7;
            }
            Item.UseSound = SoundID.Item8;

            Item.damage = 80;

            Item.knockBack = 3f;

            Item.shoot = ModContent.ProjectileType<SpectreHoseProj>();

            Item.shootSpeed = 6f;



            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<SpectreHoseProj>(), damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.SpectreBar, 14)
            .AddTile(TileID.MythrilAnvil)
            .Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }
    }
    //________________________________________________________________
    public class SpectreStaff2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectre Artifact");
            Tooltip.SetDefault("Summons spectre orbs that orbit around you at varying distances\nRight click to launch any orbs at their maximum orbital distance towards the cursor\nCan launch the orbs without needing to hold the weapon");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.HoldUp;

            Item.autoReuse = true;
            Item.UseSound = SoundID.Item43;

            Item.DamageType = DamageClass.Magic;

            Item.damage = 50;

            Item.knockBack = 0f;

            Item.useTime = 10;
            Item.useAnimation = 10;
            //Item.reuseDelay = 20;
            Item.shoot = ModContent.ProjectileType<SpectreStaffSpinProj>();
            Item.shootSpeed = 4.5f;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 18;
            }
            else
            {
                Item.mana = 12;
            }
            Item.noMelee = true; 
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 15;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
      
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.SpectreBar, 14)
          .AddTile(TileID.MythrilAnvil)
          .Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }
    }
}