﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;

namespace StormDiversMod.Items.Weapons
{
    public class StoneThrower : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Launcher");
            Tooltip.SetDefault("Fire out all your unwanted stone at your foes\nRequires Compact Boulders, craft more with stone");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
           
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.damage = 23;
            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<StoneProj>();
            Item.useAmmo = ItemType<Ammo.StoneShot>();
            Item.UseSound = SoundID.Item61;

            
            //Item.crit = 0;
            Item.knockBack = 6f;

            Item.shootSpeed = 7.5f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
         
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 35f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            velocity.X = velocity.X + player.velocity.X;
            velocity.Y = velocity.Y + player.velocity.Y;

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.IllegalGunParts, 1)
         .AddIngredient(ItemID.StoneBlock, 250)
         .AddRecipeGroup("StormDiversMod:EvilMaterial", 25)
         .AddRecipeGroup(RecipeGroupID.IronBar, 25)
         .AddTile(TileID.Anvils)
         .Register();

         
           
        }
       
    }
    //_______________________________________________________________________________
    public class StoneThrowerHard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mega Stone Launcher");
            Tooltip.SetDefault("An upgraded stone launcher which makes stone far more deadly\nRequires Compact Boulders, craft more with stone");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 7, 50, 0);
            Item.rare = ItemRarityID.LightPurple;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.damage = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<StoneHardProj>();
            Item.useAmmo = ItemType<Ammo.StoneShot>();
            Item.UseSound = SoundID.Item61;


            //Item.crit = 0;
            Item.knockBack = 8f;

            Item.shootSpeed = 13f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StoneHardProj>(), damage, knockback, player.whoAmI);

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<StoneThrower>(), 1)
           .AddIngredient(ItemID.SoulofMight, 10)
           .AddIngredient(ItemID.SoulofSight, 10)
           .AddIngredient(ItemID.SoulofFright, 10)
           .AddIngredient(ItemID.HallowedBar, 15)
           .AddTile(TileID.MythrilAnvil)
           .Register();

        }

    }
    //_______________________________________________________________________________
    public class StoneThrowerSuper : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flaming Stone Launcher");
            Tooltip.SetDefault("Superheats the boulders and fires 2 to 3 at a time\nRequires Compact Boulders to work, craft more with stone");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.damage = 75;
            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<StoneSuperProj>();
            Item.useAmmo = ItemType<Ammo.StoneShot>();
            Item.UseSound = SoundID.Item38;


            //Item.crit = 0;
            Item.knockBack = 8f;

            Item.shootSpeed = 14f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 55f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            int numberProjectiles = 2 + Main.rand.Next(2); //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(13));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StoneSuperProj>(), damage, knockback, player.whoAmI);
            }

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ModContent.ItemType<StoneThrowerHard>(), 1)
          .AddIngredient(ItemID.ShroomiteBar, 15)
          .AddIngredient(ItemID.SpectreBar, 15)
          .AddIngredient(ItemID.LunarTabletFragment, 10)
          .AddIngredient(ItemID.BeetleHusk, 10)
          .AddTile(TileID.MythrilAnvil)
          .Register();

        }

    }

    //_______________________________________________________________________________
    public class StoneThrowerSuperLunar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunar Stone Launcher");
            Tooltip.SetDefault("Empowers boulders with the power of the celestial fragments\nRequires Compact Boulders to work, craft more with stone");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.damage = 120;
            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<StoneSuperProj>();
            Item.useAmmo = ItemType<Ammo.StoneShot>();
            Item.UseSound = SoundID.Item38;

            Item.crit = 12;
            Item.knockBack = 8f;

            Item.shootSpeed = 17f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 55f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            int choice = Main.rand.Next(4);
            if (choice == 0)
            {
                {

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(8));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StoneSolar>(), damage, knockback, player.whoAmI);
                }
            }
            else if (choice == 1)
            {
                {

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X * 1.3f, perturbedSpeed.Y * 1.3f), ModContent.ProjectileType<StoneVortex>(), damage, knockback, player.whoAmI);

                }
            }
            else if (choice == 2)
            {
                for (int i = 0; i < 3; i++)
                {

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StoneNebula>(), damage, knockback, player.whoAmI);

                }
            }
            else if (choice == 3)
            {

                {

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StoneStardust>(), damage, knockback, player.whoAmI);

                }
            }
            /*for (int i = 0; i < 3; i++)
            {

                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                Projectile.NewProjectile(position.X, position.Y, (float)(perturbedSpeed.X), (float)(perturbedSpeed.Y), mod.ProjectileType("StoneSuperProj"), damage, knockBack, player.whoAmI);
            }*/

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<StoneThrowerSuper>(), 1)
         .AddIngredient(ItemID.FragmentSolar, 15)
         .AddIngredient(ItemID.FragmentVortex, 15)
         .AddIngredient(ItemID.FragmentNebula, 15)
         .AddIngredient(ItemID.FragmentStardust, 15)
         .AddIngredient(ItemID.LunarBar, 15)
         .AddTile(TileID.LunarCraftingStation)
         .Register();

           
        }
       
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/StoneThrowerSuperLunar_Glow");


            //spriteBatch.Draw(texture, Item.Center - Main.screenPosition, new Rectangle(0, 0, Item.width, Item.height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw
           (
               texture,
               new Vector2
               (
                   Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                   Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f
               ),
               new Rectangle(0, 0, texture.Width, texture.Height),
               Color.White,
               rotation,
               texture.Size() * 0.5f,
               scale,
               SpriteEffects.None,
               0f
           );
        }
    }
}