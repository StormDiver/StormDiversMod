﻿using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using System.Reflection.Metadata;

namespace StormDiversMod.Items.Weapons
{
    public class SpaceRockGlobe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Globe");
            //Tooltip.SetDefault("Summons a floating asteroid boulder at the cursor's location that explodes into many fragments");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.channel = true;

            Item.width = 25;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useTurn = false;
            //Item.channel = true;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item78;

            Item.damage = 60;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<SpaceGlobeProj>();
            Item.shootSpeed = 0f;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 38;
            }
            else
            {
                Item.mana = 25;
            }
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
       
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            //Main.projectile[index].originalDamage = Item.damage;
            return true;
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
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(6, 0);
        }

        public override void AddRecipes()
        {
           CreateRecipe()
          .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 7)
          .AddTile(TileID.MythrilAnvil)
          .Register();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
    //________________________________________________________________
    public class SpaceRockSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Blade");
            //Tooltip.SetDefault("Rains down asteroid fragments from the sky\nEach swing creates a large damaging aura
            //");
            Item.ResearchUnlockCount = 1;
        }
        int aura = 0;
        public override void SetDefaults()
        {
            Item.damage = 90;

            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 50;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.scale = 1f;
            Item.knockBack = 6;
            Item.shoot = ModContent.ProjectileType<SpaceSwordProj>();
            aura = ModContent.ProjectileType<Projectiles.AsteroidAura>();
            Item.noMelee = true;
            Item.shootSpeed = 16f;

        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(1) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6, 0f, 0f, 100, default, 1.5f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override void UseAnimation(Player player)
        {
            player.itemAnimationMax = Item.useAnimation;//seems to fix aura issue on first swing

            if (aura != 0 && !player.ItemAnimationActive)
            {
                Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                if (player.whoAmI == Main.myPlayer)
                {
                    Vector2 velocity = new Vector2(Math.Sign(mousePosition.X - player.Center.X), 0); // determines direction
                    int damage = (int)(player.GetTotalDamage(Item.DamageType).ApplyTo(Item.damage));
                    Projectile spawnedProj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.MountedCenter - velocity * 2, velocity * 5, aura, damage, Item.knockBack, Main.myPlayer,
                            Math.Sign(mousePosition.X - player.Center.X) * player.gravDir, player.itemAnimationMax, player.GetAdjustedItemScale(Item));
                    NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);

                }
                return;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);

            int numberProjectiles = 1 + Main.rand.Next(1);

            for (int index = 0; index < numberProjectiles; ++index)
                {
                    Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(150) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                    vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-250, 251); //Spawn Spread
                    vector2_1.Y -= (float)(70 * index);
                    float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                    float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                    if ((double)num13 < 0.0) num13 *= -1f;
                    if ((double)num13 < 20.0) num13 = 20f;
                    float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                    float num15 = Item.shootSpeed / num14;
                    float num16 = num12 * num15;
                    float num17 = num13 * num15;
                    float SpeedX = num16 + (float)Main.rand.Next(-10, 10) * 0.05f;  //this defines the projectile X position speed and randomnes
                    float SpeedY = num17 + (float)Main.rand.Next(-10, 10) * 0.05f;  //this defines the projectile Y position speed and randomnes
                int projid = Projectile.NewProjectile(source, new Vector2(vector2_1.X, vector2_1.Y), new Vector2(SpeedX, SpeedY), type, (int)(damage * 0.75f), 2.5f, player.whoAmI, 0.0f, (float)Main.rand.Next(5));
                Main.projectile[projid].DamageType = DamageClass.Melee;
            }
                SoundEngine.PlaySound(SoundID.Item13, player.Center);
              
            return false;
        }
        /*public override void ModifyItemScale(Player player, ref float scale)
        {
            if (Item.CountsAsClass(DamageClass.Melee))
            {
                float bonusSize = 1f;
                switch (Item.prefix)
                {
                    case PrefixID.Large:
                        bonusSize = (1.12f);
                        break;
                    case PrefixID.Massive:
                        bonusSize = (1.18f);
                        break;
                    case PrefixID.Dangerous:
                        bonusSize = (1.05f);
                        break;
                    case PrefixID.Savage:
                        bonusSize = (1.1f);
                        break;
                    case PrefixID.Bulky:
                        bonusSize = (1.1f);
                        break;
                    case PrefixID.Shameful:
                        bonusSize = (1.1f);
                        break;
                    case PrefixID.Legendary:
                        bonusSize = (1.1f);
                        break;
                    case PrefixID.Tiny:
                        bonusSize = (0.82f);
                        break;
                    case PrefixID.Terrible:
                        bonusSize = (0.87f);
                        break;
                    case PrefixID.Small:
                        bonusSize = (0.9f);
                        break;
                    case PrefixID.Unhappy:
                        bonusSize = (0.9f);
                        break;
                }
                scale *= bonusSize;
            }
        }*/
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 7)
         .AddTile(TileID.MythrilAnvil)
         .Register();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //____________________________________________________________________
    public class SpaceRockGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Blaster");
            //Tooltip.SetDefault("50% Chance not to consume Ammo\nFires out 2 bullets per shot");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.UseSound = SoundID.Item40;

            Item.damage = 40;
            Item.knockBack = 2f;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 15f;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useAmmo = AmmoID.Bullet;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        float accuracy = 10; //The amount of spread
        public override void HoldItem(Player player)
        {
            if (!player.controlUseItem)
            {
                if (accuracy < 10)//reduce accuracy when not shooting
                    accuracy += 0.1f; //1.6 seconds to reset accuracy to max
            }
            //Main.NewText("" + accuracy, 175, 17, 96);
            accuracy = (Math.Min(10, Math.Max(0.2f, accuracy))); //clamp between 0.3 and 10
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (accuracy > 0.2f)//Increases accuracy every shot
                accuracy -= 0.2f; //6.66 seconds to full accuracy?
            else
                accuracy = 0.2f;
            float numberProjectiles = 2;
            float rotation = MathHelper.ToRadians(accuracy);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            /*for (int i = 0; i < 2; i++)
          {
              Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(5));
              Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
              //Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 40);
          }*/
            return false;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= .5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 7)
         .AddTile(TileID.MythrilAnvil)
         .Register();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //_____________________
    public class SpaceRockMinion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Staff");
            //Tooltip.SetDefault("Summons a mini Asteroid to fight for you");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 46;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.autoReuse = true;

            Item.damage = 45;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item43;

            Item.mana = 10;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;

            Item.UseSound = SoundID.Item45;

            Item.buffType = ModContent.BuffType<Projectiles.Minions.SpaceRockMinionBuff>();
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.SpaceRockMinionProj>();
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
           .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 7)
           .AddTile(TileID.MythrilAnvil)
           .Register();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //________________________________
    public class SpaceRockWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Belt");
            //Tooltip.SetDefault("8 summon tag damage\n18% summon tag critical strike chance\nYour summons will focus struck enemies\nAsteroid fragments fall upon the targeted enemy when hit by summons");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.WhipProjs.SpaceRockWhipProj>(), 180, 7, 8, 28);
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.DamageType = DamageClass.SummonMeleeSpeed;

            Item.noMelee = true;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 7)
           .AddTile(TileID.MythrilAnvil)
           .Register();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}