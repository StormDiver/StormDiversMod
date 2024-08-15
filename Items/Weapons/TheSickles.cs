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
using StormDiversMod.Items.Materials;


namespace StormDiversMod.Items.Weapons
{
   
    public class TheSickle : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nine Lives");
            //Tooltip.SetDefault("Left Click to use as a spinning weapon, Right Click to throw both blades in succession");
            Item.ResearchUnlockCount = 1;  
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 24;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 100;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.channel = true;
            Item.DamageType = DamageClass.MeleeNoSpeed;

            Item.shoot = ModContent.ProjectileType<TheSickleProj2>();
            Item.damage = 35;
            //Item.crit = 4;
            Item.knockBack = 2f;
            Item.shootSpeed = 1f;
            Item.noUseGraphic = true;
            Item.noMelee = true; 
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = 1;
                Item.useTime = 15;
                Item.useAnimation = 30;
                Item.channel = false;
            }
            else
            {
                Item.useStyle = 3;
                Item.useTime = 15;
                Item.useAnimation = 15;
                Item.channel = true;
            }

            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (player.altFunctionUse == 2)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                int projid = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X * 12f, perturbedSpeed.Y * 12f), ModContent.ProjectileType<TheSickleProj2>(), (int)(damage * 1.25f), knockback * 2, player.whoAmI);             
                SoundEngine.PlaySound(SoundID.Item71 with { Volume = 1f, Pitch = 0.5f }, position);
            }
            else
            {
                if (player.ownedProjectileCounts[Item.shoot] < 1)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<TheSickleProj>(), damage, knockback, player.whoAmI);

                    SoundEngine.PlaySound(SoundID.Item71 with { Volume = 1f, Pitch = 0.5f }, position);
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Sickle, 2)
            .AddIngredient(ItemID.HellstoneBar, 10)
            .AddIngredient(ModContent.ItemType<CrackedHeart>(), 2)
            .AddTile(TileID.Anvils)
            .AddCondition(Condition.InGraveyard)
            .Register();
        }  
    }
    //____________________________________________________________________________
    public class TheScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Last Chance");
            //Tooltip.SetDefault("Left Click to use as a spinning weapon, Right Click to throw both scythes in succession\n"You have one shot"");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 24;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = 100;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.channel = true;
            Item.DamageType = DamageClass.MeleeNoSpeed;

            Item.shoot = ModContent.ProjectileType<TheScytheProj2>();
            Item.damage = 50;
            //Item.crit = 4;
            Item.knockBack = 2f;
            Item.shootSpeed = 1f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = 1;
                Item.useTime = 13;
                Item.useAnimation = 26;
                Item.channel = false;
                return player.ownedProjectileCounts[Item.shoot] < 3;
            }
            else
            {
                Item.useStyle = 3;
                Item.useTime = 13;
                Item.useAnimation = 13;
                Item.channel = true;
                return player.ownedProjectileCounts[Item.shoot] < 1;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (player.altFunctionUse == 2)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                int projid = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X * 15, perturbedSpeed.Y * 15), ModContent.ProjectileType<TheScytheProj2>(), (int)(damage * 1.25f), knockback * 2, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item71 with { Volume = 1f, Pitch = 0.5f }, position);
            }
            else
            {
                if (player.ownedProjectileCounts[Item.shoot] < 1)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<TheScytheProj>(), damage, knockback, player.whoAmI);

                    SoundEngine.PlaySound(SoundID.Item71 with { Volume = 1f, Pitch = 0.5f }, position);
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<TheSickle>(), 1)
            .AddIngredient(ItemID.SoulofNight, 15)
            .AddIngredient(ItemID.SoulofMight, 3)
            .AddIngredient(ItemID.SoulofSight, 3)
            .AddIngredient(ItemID.SoulofFright, 3)
            .AddTile(TileID.MythrilAnvil)
            .AddCondition(Condition.InGraveyard)
            .Register();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}