using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Projectiles;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Weapons
{
    public class AncientStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arid Sandblast Staff");
            Tooltip.SetDefault("Creates an explosive blast of sand at the cursor's location");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


        }
        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 1 , 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useTurn = false;
            //Item.channel = true;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item78;

            Item.damage = 40;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<AncientStaffProj>();// mod.ProjectileType("AncientStaffProj");
            
            Item.shootSpeed = 1f;

            Item.mana = 10;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override bool CanUseItem(Player player)
        {
            if (Collision.CanHitLine(Main.MouseWorld, 1, 1, player.Center, 0, 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;

            return false;
        }
       
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        /*public override Vector2? HoldoutOffset()
        {
            return new Vector2(6, 0);
        }*/

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Bone, 40)
            .AddIngredient(ItemID.FossilOre, 15)
            .AddTile(TileID.Anvils)
            .Register();         
        }
      

    }
    //______________________________________________________________________________________________________
    public class AncientKnives : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arid Knives");
            Tooltip.SetDefault("Throw out several knives that pierce after spinning");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


        }
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;
            Item.width = 10;
            Item.height = 10;
            Item.maxStack = 1;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<AncientKnivesProj>();
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 3 + Main.rand.Next(2); ; //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            { 

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, (int) (knockback), player.whoAmI);
                //Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 40);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Bone, 40)
            .AddIngredient(ItemID.FossilOre, 15)
            .AddTile(TileID.Anvils)
            .Register();

        }
    }
    //_______________________________________________________________________________
    public class AncientFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arid Sandblaster");
            Tooltip.SetDefault("Fires out a stream of burning sand\nUses gel for ammo\nIgnores 10 points of enemy defense");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {

            Item.width = 40;
            Item.height = 24;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 4;
            Item.useAnimation = 13;

            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item20;

            Item.damage = 18;
            Item.knockBack = 0.2f;
            Item.shoot = ModContent.ProjectileType<AncientFlameProj>(); 

            Item.shootSpeed = 2.5f;

            Item.useAmmo = AmmoID.Gel;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
        public override void HoldItem(Player player)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Bone, 40)
            .AddIngredient(ItemID.FossilOre, 15)
            .AddTile(TileID.Anvils)
            .Register();

        }
        bool candamage;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 40;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            if (candamage)
            {
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y-3), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI, 0, 1);
                candamage = false;
            }
            else
            {
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y-3), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI, 0, 0);
                candamage = true;

            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {

            return !(player.itemAnimation < Item.useAnimation - 2);

        }
    }
    //____________________________________________________
    public class AncientMinion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arid Command Staff");
            Tooltip.SetDefault("Summons an Ancient Arid Minion to fight for you");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.knockBack = 2f;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<Projectiles.Minions.AncientMinionBuff>();
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.AncientMinionProj>();
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
            .AddIngredient(ItemID.Bone, 40)
            .AddIngredient(ItemID.FossilOre, 15)
            .AddTile(TileID.Anvils)
            .Register();

        }

    }
}