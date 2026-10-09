using Microsoft.Xna.Framework;
using Mono.Cecil;
using SteelSeries.GameSense.DeviceZone;
using StormDiversMod.Common;
using StormDiversMod.NPCs;
using StormDiversMod.Projectiles.Minions;
using StormDiversMod.Projectiles.SentryProjs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace StormDiversMod.Items.Tools
{
    public class SentryKiller : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Remote Sentry Detonator");
            //Tooltip.SetDefault("Removes all of your placed sentries");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = false;
                Item.shoot = ProjectileID.Bullet;
                Item.shootSpeed = 0f;
            Item.sentry = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, -10);
        }
        public override void UseAnimation(Player player)
        {
            base.UseAnimation(player);
        }
        public override void HoldItem(Player player)
        {
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Main.PlaySound(SoundID.NPCKilled, (int)position.X, (int)position.Y, 59);
            if (player.altFunctionUse != 2)
            {
                SoundEngine.PlaySound(SoundID.Item115 with { Volume = 0.5f, Pitch = 0.5f }, player.Center);

                for (int i = 0; i < 50; i++)
                {
                    float speedX = 0f;
                    float speedY = -5f;

                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dustIndex = Dust.NewDust(new Vector2(player.Center.X + (20 * player.direction), player.Center.Y - 16 * player.gravDir), 0, 0, 21, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 15)
            .AddIngredient(ItemID.Amethyst, 3)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
    public class Sentrykill : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            var player = Main.player[projectile.owner];

            if (projectile.sentry)
            {
                if (player.itemAnimation > 1 && player.HeldItem.type == ModContent.ItemType<Items.Tools.SentryKiller>())
                {
                    SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.5f, Pitch = 0.5f }, projectile.Center);

                    for (int i = 0; i < 75; i++)
                    {
                        float speedX = 0f;
                        float speedY = -5f;

                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 21, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1.5f);
                        Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].noGravity = true;

                    }
                    for (int i = 0; i < 50; i++)
                    {
                        float speedX = 0f;
                        float speedY = -3f;

                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);
                        Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].noGravity = true;

                    }
                    projectile.Kill();
                }
            }
        }
    }
}