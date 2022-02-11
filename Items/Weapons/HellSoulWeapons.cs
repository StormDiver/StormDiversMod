﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;


namespace StormDiversMod.Items.Weapons
{
    public class HellSoulBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HellSoul Bow");
            Tooltip.SetDefault("Fires out a homing soul arrow every other shot");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 46;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item5;

            Item.damage = 50;
            //Item.crit = 4;
            Item.knockBack = 3f;

            Item.shoot = ProjectileID.WoodenArrowFriendly;

            Item.shootSpeed = 15f;
            
            Item.useAmmo = AmmoID.Arrow;
                

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        int shootarrow;
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            shootarrow++;
            if (shootarrow >= 2)
            {
                SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 8);

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                    float scale = 1f - (Main.rand.NextFloat() * .5f);
                    perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.HellSoulBowProj>(), damage, knockback, player.whoAmI);
                shootarrow = 0;
            }
            
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()           
           .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 14)
           .AddTile(TileID.Hellforge)
           .Register();
           
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/HellSoulBow_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f  ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
       
    }
    //__________________________________________________________________________________________________________

    public class HellSoulRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HellSoul Rifle");
            Tooltip.SetDefault("Converts regular bullets into soul bullets that pierce and inflict soulburn\nRight Click to zoom out");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {

            Item.width = 50;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightPurple;

            Item.useStyle = ItemUseStyleID.Shoot;

            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            //Item.UseSound = SoundID.Item40;

            Item.damage = 66;
            Item.knockBack = 3f;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 16f;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true;
        }

    
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }
        public override void HoldItem(Player player)
        {
            player.scope = true;
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 18;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            if (type == ProjectileID.Bullet)
            {
                type = ModContent.ProjectileType<Projectiles.HellSoulRifleProj>();
                SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 8);

            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 40);

            }
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

            Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 3), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            //Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 40);


            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 14)
             .AddTile(TileID.Hellforge)
             .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/HellSoulRifle_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
       
    }
    //__________________________________________________________________________________________________________

    public class HellSoulSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HellSoul Blade");
            Tooltip.SetDefault("Fires out a soul beam every other swing");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }

        public override void SetDefaults()
        {
            Item.damage = 65;

            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 50;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 6;
            Item.shoot = ModContent.ProjectileType<Projectiles.HellSoulSwordProj>();
            Item.shootSpeed = 20f;
            
            
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(2) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 173, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        int weaponattack = 2;
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            weaponattack--;
            if (weaponattack <= 0)
            {
      
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 8);
                weaponattack = 2;
            }
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(target.position, target.width, target.height, 173, 0, 0);
                dust.scale = 2;


            }
            target.AddBuff(ModContent.BuffType<Buffs.HellSoulFireDebuff>(), 480);
        }
        public override void OnHitPvp(Player player, Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType < Buffs.HellSoulFireDebuff>(), 480);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 14)
           .AddTile(TileID.Hellforge)
           .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/HellSoulSword_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
      
    }
    //__________________________________________________________________________________________________________
    public class HellSoulFlare : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HellSoul Flare");
            Tooltip.SetDefault("Summons multiple soul flames that charge towards the cursor");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            //Item.channel = true;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item20;

            Item.damage = 40;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.HellSoulMagicProj>(); 
        
            Item.shootSpeed = 0f;

            Item.mana = 12;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            int speedX = 0;
            int speedY = -10;
            for (int i = 0; i < 4; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(135));
                float scale = 1f - (Main.rand.NextFloat() * .5f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            return false;
        }
       
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 14)
          .AddTile(TileID.Hellforge)
          .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/HellSoulFlare_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //_______________________________________________________
    //_______________________________________________________________________

    public class HellSoulMinion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HellSoul Staff");
            Tooltip.SetDefault("Summons a HellSoul minion to fight for you");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });

        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<Projectiles.Minions.HellSoulMinionBuff>();
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.HellSoulMinionProj>();
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
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
           .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 14)
           .AddTile(TileID.Hellforge)
           .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/HellSoulMinion_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}