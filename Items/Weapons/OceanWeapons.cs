using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
    public class OceanSpell : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tidal Wave");
            Tooltip.SetDefault("Summons an orb of water that splashes on impact");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
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
            Item.UseSound = SoundID.Item20;

            Item.damage = 10;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<OceanSpellProj>();

            Item.shootSpeed = 12f;
            Item.scale = 0.9f;
            //Item.useAmmo = AmmoID.Arrow;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
         
            
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Coral, 10)
            .AddIngredient(ItemID.Starfish, 2)
            .AddIngredient(ItemID.Seashell, 2)
            .AddTile(TileID.WorkBenches)
            .Register();
           

        }
    }
    //_________________________________________________________________
    public class OceanSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade of the Sea");
            Tooltip.SetDefault("Fires out a blast of water each swing");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 15;

            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 38;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<OceanSmallProj>();
            Item.scale = 1.2f;

            Item.shootSpeed = 10f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(4)); // This defines the projectiles random spread . 10 degree spread.
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, (int)(damage * 0.7f), knockback, player.whoAmI);
            }
            SoundEngine.PlaySound(SoundID.Splash, player.Center);


            return false;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) < 3)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 33, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(BuffID.Wet, 300);
        }
        public override void OnHitPvp(Player player, Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Wet, 300);
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Coral, 10)
           .AddIngredient(ItemID.Starfish, 2)
           .AddIngredient(ItemID.Seashell, 2)
           .AddTile(TileID.WorkBenches)
           .Register();

        }

    }
    //____________________________________________________________________________________________________________________
    public class OceanGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Blaster");
            Tooltip.SetDefault("Converts bullets into pieces of coral that are not affected by water and obey gravity");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useTurn = false;
            Item.autoReuse = false;

            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<OceanCoralProj>();
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item85;

            Item.damage = 8;
            //Item.crit = 0;
            Item.knockBack = 1f;

            Item.shootSpeed = 12f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {


            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(1)); // This defines the projectiles random spread . 10 degree spread.
            int projID = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<OceanCoralProj>(), damage, knockback, player.whoAmI, 0, 1);
            Main.projectile[projID].timeLeft = 180;        

            return false;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Coral, 10)
           .AddIngredient(ItemID.Starfish, 2)
           .AddIngredient(ItemID.Seashell, 2)
           .AddTile(TileID.WorkBenches)
           .Register();

        }
    }
    //__________________________________________________________________
    
}