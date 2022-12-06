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
    public class TurtleShellWeapon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Turtle Shell");
            Tooltip.SetDefault("Toss the shells back at your foes\nGrants extra defense while attacking enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            
            Item.damage = 55;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 24;
           
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.knockBack = 8;
            Item.value = Item.sellPrice(0, 2, 40, 0);
                     Item.rare = ItemRarityID.Lime;
            Item.shootSpeed = 14f;
            Item.shoot = ModContent.ProjectileType<TurtleShellProj>();
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
          
            SoundEngine.PlaySound(SoundID.Item1, position);
           
                
                return true;
           
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 16)
            .AddIngredient(ItemID.TurtleShell, 1)
            .AddTile(TileID.MythrilAnvil)
            .Register();

        }
    }
    //________________________________________________________________
    public class TurtleSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Turtle Pike");
            Tooltip.SetDefault("Grants extra defense while attacking enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            Item.damage = 60;
            //Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 58;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 2, 40, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 4f;
            Item.shoot = ModContent.ProjectileType<TurtleSpearProj>();
            Item.shootSpeed = 9f;
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
           
            /* projshoot++;
             if (projshoot >= 2)
             {
                 Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
                 Projectile.NewProjectile(position.X, position.Y, (float)(perturbedSpeed.X * 1f), (float)(perturbedSpeed.Y * 1f), mod.ProjectileType("TurtleProj"), (int)(damage * 1.5), knockBack, player.whoAmI);
                 Main.PlaySound(SoundID.NPCHit, (int)player.Center.X, (int)player.Center.Y, 24);
                 projshoot = 0;
             }*/
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 16)
            .AddIngredient(ItemID.TurtleShell, 1)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
    //________________________________________________________________
    public class TurtleYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Spinning Tortoise");
            Tooltip.SetDefault("Fires out yoyo heads randomly\nGrants extra defense while attacking enemies");
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 25;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            //Item.crit = 0;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 20;
            Item.height = 26;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 2, 40, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.useTurn = true;
            Item.knockBack = 4f;
            Item.shoot = ModContent.ProjectileType<TurtleYoyoProj>();
            // Item.shootSpeed = 9f;
            Item.noMelee = true;
            Item.noUseGraphic = true;

        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            /* projshoot++;
             if (projshoot >= 2)
             {
                 Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
                 Projectile.NewProjectile(position.X, position.Y, (float)(perturbedSpeed.X * 1f), (float)(perturbedSpeed.Y * 1f), mod.ProjectileType("TurtleProj"), (int)(damage * 1.5), knockBack, player.whoAmI);
                 Main.PlaySound(SoundID.NPCHit, (int)player.Center.X, (int)player.Center.Y, 24);
                 projshoot = 0;
             }*/
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ItemID.ChlorophyteBar, 16)
             .AddIngredient(ItemID.TurtleShell, 1)
             .AddTile(TileID.MythrilAnvil)
             .Register();
        }
    }
}