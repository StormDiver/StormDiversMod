using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Buffs;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
using System.Reflection.Metadata;


namespace StormDiversMod.Items.Weapons
{
    public class ShroomiteSharpshooter : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroomite Sharpshooter");
            //Tooltip.SetDefault("33% Chance not to consume Ammo\nBuilds up accuracy over several seconds, dealing extra damage at full accuracy\nRight Click to zoom out");
            Item.ResearchUnlockCount = 1;
         

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
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            //Item.UseSound = SoundID.Item40;

            Item.damage = 65;
            Item.crit = 16;
            Item.knockBack = 2f;
       
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 15f;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true; 
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }

        float accuracy = 15; //The amount of spread
        int resetaccuracy = 15; //How long to not fire for the accuracy to reset

        public override void HoldItem(Player player)
        {
            //player.scope = true;
            if (resetaccuracy == 0) //Resets accuracy when not firing
            {
                accuracy = 15;
               
            }
            if (resetaccuracy > 0)
                resetaccuracy--;
            //Main.NewText("" + accuracy, 175, 17, 96);
            accuracy = (Math.Min(15, Math.Max(0, accuracy))); //clamp between 0 and 15
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            if (accuracy > 0)//Increases accuracy every shot
            {
                accuracy -= 0.33f;
            }
            resetaccuracy = 15; //Prevents the accuracy from reseting while firing
            {
                {
                    //Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(accuracy));
                    if (accuracy <= 0)//When at full accuracy damage and knockback of the projectile is increased by 10%
                    {
                        if (type == ProjectileID.Bullet)
                            {
                                type = ProjectileID.BulletHighVelocity;
                            }
                        Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 2), new Vector2(velocity.X, velocity.Y), type, (int)(damage * 1.15f), knockback * 2, player.whoAmI);
                        SoundEngine.PlaySound(SoundID.Item40 with {Pitch = 0.3f }, position);
                    }
                    else
                    {
                        Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 2), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                        SoundEngine.PlaySound(SoundID.Item40, position);

                    }

                    //Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 40);
                }
            }
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(accuracy));
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .33f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ShroomiteBar, 14)
            .AddTile(TileID.MythrilAnvil)
            .Register();
           
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/ShroomiteSharpshooter_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {

            
            return true;
        }
       
    }
    //_______________________________________________________________________________
    public class ShroomiteFury : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroomite Fury");
            //Tooltip.SetDefault("Shoots out two additional super bouncy piercing arrows each shot");
            Item.ResearchUnlockCount = 1;

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
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item5;

            Item.damage = 72;
            //Item.crit = 4;
            Item.knockBack = 5f;

            Item.shoot = ProjectileID.WoodenArrowFriendly;

            Item.shootSpeed = 16f;

            Item.useAmmo = AmmoID.Arrow;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            

            for (int i = 0; i < 2; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10));
                float scale = 1f - (Main.rand.NextFloat() * .2f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<ShroomBowArrowProj>(), damage, knockback, player.whoAmI);
            }
 
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ShroomiteBar, 14)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/ShroomiteFury_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //__________________________________________________________________________________________________________
    public class ShroomiteLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroomite Launcher");
            //Tooltip.SetDefault("Fires Shroomite Rockets which explode into mushrooms\nRight click to fire Shroomite Grenades");
            Item.ResearchUnlockCount = 1;

            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 24;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ProjectileID.RocketI;
            Item.useAmmo = AmmoID.Rocket;
            // Item.UseSound = SoundID.Item92;
            Item.damage = 47;
            //Item.crit = 4;
            Item.knockBack = 6f;
            Item.shootSpeed = 10f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {

            if (player.altFunctionUse == 2)
            {
                Item.autoReuse = true;

            }
            else
            {
                Item.autoReuse = true;

            }

            return true;
        }

        //int gren = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            //gren++;
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            if (player.altFunctionUse == 2)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X * 1.2f, perturbedSpeed.Y * 1.2f), ModContent.ProjectileType<ShroomGrenProj>(), damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item61, position);
            }
            else
            {

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<ShroomRocketProj>(), damage, knockback, player.whoAmI);

                SoundEngine.PlaySound(SoundID.Item92, position);
            }

            return false;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ShroomiteBar, 14)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/ShroomiteLauncher_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}