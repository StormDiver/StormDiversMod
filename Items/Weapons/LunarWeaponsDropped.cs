using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Projectiles;

namespace StormDiversMod.Items.Weapons
{
    public class LunarSelenianBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Selenian Blades");
            Tooltip.SetDefault("Spin in place upon striking an enemy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
           
            Item.damage = 80;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 38;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.shootSpeed = 17f;
            Item.shoot = ModContent.ProjectileType<Projectiles.SelenianBladeProj>();
            Item.UseSound = SoundID.Item7;
            Item.autoReuse = true;
            Item.noMelee = true;

        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            SoundEngine.PlaySound(SoundID.Item1, position);

            return true;
        }
        
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LunarSelenianBlade_Glow");


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
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }
    }
    //__________________________________________________________________________________________
    public class LunarVortexShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Diver Shotgun");
            Tooltip.SetDefault("Can be charged up to increase accuracy and damage\nGains additional damage, speed, and knockback at max charge\n'Stolen from the Legendary Storm Divers'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.useAmmo = AmmoID.Bullet;

            Item.damage = 100; //Extra 450 damage from base, + additonal 25% at max charge (100 + 450 = 550 * 1.2 = 660),
                               //4.5 bullets on average, add addition 30 frame cooldown everytime, fewer bullet will likely hit at lower charges
                               //(base damage X Shots per second X 4.5)
                               //~900dps  at no charge, (2 shots per second, 100 base damage)
                               //~1125dps at half second charge (1 shot per second, 250 base damage)
                               //~1188dps at second charge (0.66 shots per second, 400 base damage)
                               //~1238dps almost full charge, (0.5 shots per second, 550 base damage)
                               //~1485dps at max charge (~0.5 shots per second, 660 base damage)
            Item.DamageType = DamageClass.Ranged;

            Item.width = 30;
            Item.height = 20;
            Item.reuseDelay = 30;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.knockBack = 5f;
            Item.useTurn = false;
            Item.shoot = ModContent.ProjectileType<Projectiles.VortexShotgunGun>();
            Item.shootSpeed = 15f;
            //Item.UseSound = SoundID.Item12;
            Item.noMelee = true;
            //Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 2);
        }
        /*
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
            }
            else
            {
            }
            return base.CanUseItem(player);
        }*/
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.VortexShotgunGun>(), damage, knockback, player.whoAmI);

            return false;
        }
        /*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 25f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            if (player.altFunctionUse == 2)
            {
                type = ProjectileID.MoonlordBullet;
                //type = mod.ProjectileType("VortexRocketProj2");
                int numberProjectiles = 4 + Main.rand.Next(2); ; //This defines how many projectiles to shot.
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10)); // This defines the projectiles random spread . 10 degree spread.
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
                }
                SoundEngine.PlaySound(SoundID.Item36 with{Volume = 1f, Pitch = 0.5f}, player.Center);         
            }
            else
            {
                int numberProjectiles = 4 + Main.rand.Next(2); ; //This defines how many projectiles to shot.
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10)); // This defines the projectiles random spread . 10 degree spread.
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
                }
                SoundEngine.PlaySound(SoundID.Item36, player.Center);
            }
            for (int i = 0; i < 25; i++)
            {
                float speedY = -1.5f;
                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                int dust2 = Dust.NewDust(player.Center + muzzleOffset * 1.6f, 0, 0, 229, dustspeed.X + velocity.X / 8, dustspeed.Y + velocity.Y / 8, 229, default, 1.5f);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].scale = 1.5f;
            }
            return false;
        }*/
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0f;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LunarVortexShotgun_Glow");


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
   

    //__________________________________________________________________________________________
    public class LunarPredictorBrain : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Predictor Brain");
            Tooltip.SetDefault("Summons nebula projectiles that charge towards the cursor");

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
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;

            Item.UseSound = SoundID.Item8;

            Item.damage = 78;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType < Projectiles.PredictorBrainProj>();

            Item.shootSpeed = 5f;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 20;
            }
            else
            {
                Item.mana = 13;
            }

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;

            /* for (int i = 0; i < 5; i++)
             {
                 Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                 Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
             }*/
            int speedX = 0;

            int speedY = -10;
            for (int i = 0; i < 5; i++)
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
            return new Vector2(0, 0);
        }

        
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LunarPredictorBrain_Glow");


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
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //__________________________________________________________________________________________
    public class LunarStargazerLaser : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stargazer Core");
            Tooltip.SetDefault("Summons a floating Stardust Portal that fires piercing projectiles at nearby enemies in bursts\nRight click to target a specific enemy");

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
            Item.width = 25;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            //Item.channel = true;
            Item.DamageType = DamageClass.Summon;

            Item.UseSound = SoundID.Item78;

            Item.damage = 75;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.SentryProjs.StargazerCoreProj>();

            Item.shootSpeed = 0f;

            Item.mana = 10;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(6, 0);
        }

        
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LunarStargazerLaser_Glow");


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
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}