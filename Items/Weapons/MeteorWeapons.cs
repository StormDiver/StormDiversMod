using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace StormDiversMod.Items.Weapons
{
    public class MeteorSentry : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Space Saucer Staff");
            //Tooltip.SetDefault("Summons a floating saucer that fires lasers at enemies below\nRight click to target a specific enemy");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            Item.ResearchUnlockCount = 1;

            //Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 40, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 10;
            Item.UseSound = SoundID.Item60;

            Item.damage = 28;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.SentryProjs.MeteorSentryProj > ();

            //Item.shootSpeed = 3.5f;
 
            Item.noMelee = true; 
        }
        public override bool CanUseItem(Player player)
        {
            /*if (Collision.CanHitLine(Main.MouseWorld, 1, 1, player.position, player.width, player.height))
            {
                return true;
            }
            else
            {
                return false;
            }*/
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;
            return false;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.MeteoriteBar, 20)
            .AddTile(TileID.Anvils)
            .Register();
         

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //______________________________________
    public class MeteorBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Space Bow");
            //Tooltip.SetDefault("Rains arrows from the sky\nAllows Meteor Arrows to pass through tiles");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 40, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item9;
            
            Item.damage = 20;
            //Item.crit = 4;
            Item.knockBack = 3f;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Arrow;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 57, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            for (int index = 0; index < 1; ++index)
            {
                Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
                vector2_1.Y -= (float)(100 * index);
                float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if ((double)num13 < 0.0) num13 *= -1f;
                if ((double)num13 < 20.0) num13 = 20f;
                float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                float num15 = Item.shootSpeed / num14;
                float num16 = num12 * num15;
                float num17 = num13 * num15;
                float SpeedX = num16 + (float)Main.rand.Next(-20, 20) * 0.02f;  //this defines the projectile X position speed and randomnes
                float SpeedY = num17 + (float)Main.rand.Next(-20, 20) * 0.02f;  //this defines the projectile Y position speed and randomnes
                Projectile.NewProjectile(source, new Vector2(vector2_1.X, vector2_1.Y), new Vector2(SpeedX, SpeedY), type, damage, knockback, player.whoAmI, 0.0f, (float)Main.rand.Next(5));
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ItemID.MeteoriteBar, 20)
             .AddTile(TileID.Anvils)
             .Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //_____________________________________________
    //____________________________________________________________________________
    public class MeteorSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Space Spear");
            //Tooltip.SetDefault("Striking an enemy summons 2 meteor fragments from the sky");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.Spears[Item.type] = true;

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (ModLoader.HasMod("TRAEProject"))//DON'T FORGET THIS!!!!!!!
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\nHold right click to charge and release to throw the spear";
                    }
                }

            }
        }
        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 64;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 0, 40, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 4f;
            Item.shoot = ModContent.ProjectileType<MeteorSpearProj>();
            Item.shootSpeed = 4f;
            Item.noMelee = true;
            Item.noUseGraphic = true;

        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.MeteoriteBar, 20)
            .AddTile(TileID.Anvils)
            .Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}