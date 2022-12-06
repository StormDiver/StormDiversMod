using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace StormDiversMod.Items.Weapons
{

    //___________________________________________
    public class GladiatorSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gladiator's Glaive");
            Tooltip.SetDefault("Slow but powerful");

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
            Item.damage = 21;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 64;
            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.knockBack = 5f;
            Item.shoot = ModContent.ProjectileType<Projectiles.GladiatorSpearProj>();
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

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0)); // This defines the projectiles random spread . 10 degree spread.
               
            
            //Main.PlaySound(SoundID.NPCHit, (int)player.position.X, (int)player.position.Y, 9);

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
             .AddRecipeGroup("StormDiversMod:GoldBars", 10)
             .AddIngredient(ModContent.ItemType<Items.Materials.RedSilk>(), 3)
             .AddTile(TileID.Anvils)
             .Register();


        }

    }
    //______________________________________
    public class GladiatorBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gladiator's Bow");
            Tooltip.SetDefault("Fires arrows at a high velocity");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item5;

            Item.damage = 16;
            //Item.crit = 4;
            Item.knockBack = 3f;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 18f;
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
        
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("StormDiversMod:GoldBars", 10)
            .AddIngredient(ModContent.ItemType<Items.Materials.RedSilk>(), 3)
            .AddTile(TileID.Anvils)
            .Register();

        }
    }
    //__________________________________________________
    public class GladiatorStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gladiator's Staff");
            Tooltip.SetDefault("Fires out a magical piercing beam");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 8;
            }
            else
            {
                Item.mana = 5;
            }
            Item.UseSound = SoundID.Item8;

            Item.damage = 20;

            Item.knockBack = 3f;

            Item.shoot = ModContent.ProjectileType<Projectiles.GladiatorStaffProj>();

            Item.shootSpeed = 8f;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
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
           
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 50f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
             .AddRecipeGroup("StormDiversMod:GoldBars", 10)
             .AddIngredient(ModContent.ItemType<Items.Materials.RedSilk>(), 3)
             .AddTile(TileID.Anvils)
             .Register();

        }

    }
}