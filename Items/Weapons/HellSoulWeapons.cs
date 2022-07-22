using System;
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
            DisplayName.SetDefault("Hell Fury");
            Tooltip.SetDefault("Fires out a homing soul arrow every shot");
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
                SoundEngine.PlaySound(SoundID.Item8, player.Center);

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                    float scale = 1f - (Main.rand.NextFloat() * .5f);
                    perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.HellSoulBowProj>(), damage, knockback, player.whoAmI);
             
            
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()           
           .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 12)
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
            DisplayName.SetDefault("The HellBlaster");
            Tooltip.SetDefault("Fires out a blast of bullets alongside a piercing soul bullet");
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

            Item.UseSound = SoundID.Item38;
            
            Item.damage = 28;
            Item.knockBack = 6f;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 16f;
            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true;
        }

    
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
       
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 1;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
          
            int numberProjectiles = 4; //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(15)); // This defines the projectiles random spread . 10 degree spread.
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }

            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X * 1.5f, velocity.Y * 1.5f), ModContent.ProjectileType<Projectiles.HellSoulRifleProj>(), damage, knockback, player.whoAmI);


            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 12)
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
            DisplayName.SetDefault("Soul Blade");
            Tooltip.SetDefault("Fires out a spread of soul beams every third swing");
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
            Item.shootSpeed = 10f;
            
            
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
        int weaponattack = 3;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            weaponattack--;
            if (weaponattack <= 0)
            {
                float numberProjectiles = 3;
                float rotation = MathHelper.ToRadians(14);
                for (int j = 0; j < numberProjectiles; j++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                    int projID = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.HellSoulSwordProj>(), (int)(damage * 0.5f), (int)(knockback * 0.33f), player.whoAmI);
                }
                //Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), type, (int)(damage * 1.2f), knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item8, player.Center);
                weaponattack = 3;
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
           .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 12)
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
            DisplayName.SetDefault("Hell Flare");
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            int speedX = 0;
            int speedY = -10;
            for (int i = 0; i < 4; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(135));
                float scale = 1f - (Main.rand.NextFloat() * .5f);
                perturbedSpeed = perturbedSpeed * scale;
                if (player.gravDir == 1)
                {
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, -perturbedSpeed.Y), type, damage, knockback, player.whoAmI);

                }
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
          .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 12)
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
            DisplayName.SetDefault("Soul Cauldron Staff");
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
            Item.damage = 50;
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
           .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 12)
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