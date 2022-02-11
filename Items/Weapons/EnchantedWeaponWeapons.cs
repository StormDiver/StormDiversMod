using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Weapons
{
    public class EnchantedSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Blade of Light");
            Tooltip.SetDefault("Summons mini Enchanted Swords that charge and ricochet towards the cursor and pierce\n'Not to be confused with The Blade of Night'");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 1, 60, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;

            Item.UseSound = SoundID.Item8;

            Item.damage = 44;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType < Projectiles.EnchantedSwordProj>();
            
            Item.shootSpeed = 1f;

            Item.mana = 10;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            

            //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);

            return false;
        }
        
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

       
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.SoulofLight, 8)
            .AddIngredient(ItemID.CrystalShard, 25)
            .AddIngredient(ItemID.LightShard)
          .AddTile(TileID.MythrilAnvil)
          .Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //_______________________________________________________________________________
    public class EnchantedAxeMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Soul Splitter");
            Tooltip.SetDefault("Summons mini Crimson Axes that split into multiple axes\n'Split the souls of your foes'");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 1, 60, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 33;
            Item.useAnimation = 33;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            Item.UseSound = SoundID.Item8;

            Item.damage = 37;
            //Item.crit = 4;
            Item.knockBack = 2f;

            Item.shoot = ModContent.ProjectileType<Projectiles.CrimsonAxeProj>();

            Item.shootSpeed = 1f;

            Item.mana = 12;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
         
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.SoulofNight, 8)
           .AddIngredient(ItemID.Ichor, 15)
           .AddIngredient(ItemID.DarkShard)
         .AddTile(TileID.MythrilAnvil)
         .Register();
           
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //_______________________________________________________________________________
    public class EnchantedHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Dream Crusher");
            Tooltip.SetDefault("Summons mini Cursed Hammers that rain down more hammers\n'Crush the dreams of your enemies'");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 1, 60, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;


            Item.DamageType = DamageClass.Magic;

            Item.UseSound = SoundID.Item8;

            Item.damage = 38;
            //Item.crit = 4;
            Item.knockBack = 2f;

            Item.shoot = ModContent.ProjectileType<Projectiles.CursedHammerProj>();

            Item.shootSpeed = 1f;

            Item.mana = 12;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
         
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

       
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ItemID.SoulofNight, 8)
          .AddIngredient(ItemID.CursedFlame, 15)
          .AddIngredient(ItemID.DarkShard)
        .AddTile(TileID.MythrilAnvil)
        .Register();
          
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}