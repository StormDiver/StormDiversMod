using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using StormDiversMod;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Common;
using Terraria.DataStructures;
using Humanizer;
using Newtonsoft.Json;
using StormDiversMod.Items.Ammo;

namespace StormDiversMod.Items.Weapons
{
    public class MineDetonate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sticky Mine Detonator");
            //Tooltip.SetDefault("Left click to throw out a Sticky Mine that sticks to surfaces, up to 6 can be thrown out at once
            // Right Click to detonate all the mines
            //Can be used to launch yourself into the air, also works on enemies and Town NPCs
            //Requires Sticky Mines as ammo");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.MineBombProj>();
            Item.useAmmo = ItemType<Ammo.MineBomb>();
            Item.shootSpeed = 8f;
            Item.noMelee = true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (!player.HasItem(ModContent.ItemType<MineBomb>())) //if no ammo left allow right click to still be used
                {
                    Item.useAmmo = 0;
                }
                Item.useStyle = ItemUseStyleID.HoldUp;
                Item.noUseGraphic = false;
                return true;
            }
            else
            {
                Item.useAmmo = ItemType<Ammo.MineBomb>();
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = true;

                return player.ownedProjectileCounts[Item.shoot] < 6;
            }
            // Ensures no more than 6 bombs
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.altFunctionUse == 2)
                return false;
            else
                return true;
            //return Main.rand.NextFloat() >= 0f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, -10);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
       
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Main.PlaySound(SoundID.NPCKilled, (int)position.X, (int)position.Y, 59);
            if (player.altFunctionUse == 2) //Right Click
            {
                SoundEngine.PlaySound(SoundID.Item149 with { Volume = 1.5f, Pitch = 0.5f }, player.Center);

                for (int i = 0; i < 50; i++)
                {
                    float speedX = 0f;
                    float speedY = -5f;

                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    if (player.gravDir == 1)
                    {
                        int dustIndex = Dust.NewDust(new Vector2(player.Center.X + (20 * player.direction), player.Top.Y - 10), 0, 0, 6, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);

                        Main.dust[dustIndex].noGravity = true;
                    }
                    else
                    {
                        int dustIndex = Dust.NewDust(new Vector2(player.Center.X + (20 * player.direction), player.Bottom.Y + 6), 0, 0, 6, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);

                        Main.dust[dustIndex].noGravity = true;
                    }
                }
                return false;
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item1 with { Volume = 1f, Pitch = 0f }, player.Center);

                int spikyprojs = 0;

                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<Projectiles.AmmoProjs.MineBombProj>())
                    {
                        spikyprojs++;
                    }
                }
                //Starts from 0
                if (spikyprojs >= 5)
                    CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.Red, "MAX", false);
                else
                    CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.White, spikyprojs + 1, false);

                return true;
            }
        }
    }
    //________________________________________________________________________________________________________________

    public class MineDetonateC4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sacthel Charger Detonator");
            //Tooltip.SetDefault("Left click to throw out a Sacthel Charger that sticks to surfaces, up to 1 can be thrown out at once
            // Right Click to detonate all the mines
            // Right Click while holding up to detonate all the mines
            //Creates a massive explosion that deals very high damage to enemies and allies alike, and destroys tiles in a large radius
            // Take extreme caution when detonating!
            //Requires Sacthel Charger as ammo");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.C4Proj>();
            Item.useAmmo = ItemType<Ammo.C4Ammo>();
            Item.shootSpeed = 12f;
            Item.noMelee = true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (!player.HasItem(ModContent.ItemType<C4Ammo>())) //if no ammo left allow right click to still be used
                {
                    Item.useAmmo = 0;
                }
                Item.useStyle = ItemUseStyleID.HoldUp;
                Item.noUseGraphic = false;
                return true;
            }
            else
            {
                Item.useAmmo = ItemType<Ammo.C4Ammo>();
                Item.useStyle = ItemUseStyleID.RaiseLamp;
                Item.noUseGraphic = true;
                return player.ownedProjectileCounts[Item.shoot] < 1;
            }
            // Ensures no more than 1 bombs
        }
        public override void HoldItem(Player player)
        {
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (player.altFunctionUse == 2)
                return false;
            else
                return true;
            //return Main.rand.NextFloat() >= 0f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(8, -10);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Main.PlaySound(SoundID.NPCKilled, (int)position.X, (int)position.Y, 59);
            if (player.altFunctionUse == 2) //Right Click
            {
                SoundEngine.PlaySound(SoundID.Item149 with { Volume = 1.5f, Pitch = 0.5f }, player.Center);

                for (int i = 0; i < 50; i++)
                {
                    float speedX = 0f;
                    float speedY = -5f;

                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    if (player.gravDir == 1)
                    {
                        int dustIndex = Dust.NewDust(new Vector2(player.Center.X + (5 * player.direction), player.Top.Y - 10), 0, 0, 6, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);

                        Main.dust[dustIndex].noGravity = true;
                    }
                    else
                    {
                        int dustIndex = Dust.NewDust(new Vector2(player.Center.X + (5 * player.direction), player.Bottom.Y + 6), 0, 0, 6, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);

                        Main.dust[dustIndex].noGravity = true;
                    }
                }
                return false;
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item1 with { Volume = 1f, Pitch = 0f }, player.Center);

                return true;
            }
        }
    }
}
