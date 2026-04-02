using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Common;
using StormDiversMod.Items.Accessory;
using StormDiversMod.Items.Ammo;
using StormDiversMod.Items.Materials;
using StormDiversMod.Items.Pets;
using StormDiversMod.Items.Tools;
using StormDiversMod.Items.Vanitysets;
using StormDiversMod.Items.Weapons;
using StormDiversMod.NPCs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Intrinsics.X86;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Generation;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using static System.Net.Mime.MediaTypeNames;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.Items.Summons
{
    public class PainTicket : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pain Ticket");
            //Tooltip.SetDefault("90 Pity, 50/50 for first sucess");
            Item.ResearchUnlockCount = -1;
        }     
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.width = 20;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useTurn = false;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            //ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;

        }
        public override void PostUpdate()
        {
            // Spawn some light and dust when dropped in the world
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.4f);
            }
            if (Item.timeSinceItemSpawned % 12 == 0)
            {
                Vector2 center = Item.Center + new Vector2(0f, Item.height * -0.1f);

                // This creates a randomly rotated vector of length 1, which gets it's components multiplied by the parameters
                Vector2 direction = Main.rand.NextVector2CircularEdge(Item.width * 0.6f, Item.height * 0.6f);
                float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

                Dust dust = Dust.NewDustPerfect(center + direction * distance, DustID.SilverFlame, velocity);
                dust.scale = 0.5f;
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.alpha = 0;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var player = Main.LocalPlayer;

            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip6")
                {
                    if (player.GetModPlayer<PlayerUpgrades>().Gacha5Pity < 30) //different colours for 0-29, 30-59, and 60-89
                        line.Text = "[c/1EFF00:Golden Pain Pity = " + player.GetModPlayer<PlayerUpgrades>().Gacha5Pity + "/90]";
                    else if (player.GetModPlayer<PlayerUpgrades>().Gacha5Pity >= 30 && player.GetModPlayer<PlayerUpgrades>().Gacha5Pity < 60)
                        line.Text = "[c/FFE100:Golden Pain Pity = " + player.GetModPlayer<PlayerUpgrades>().Gacha5Pity + "/90]";
                    else
                        line.Text = "[c/FF6F00:Golden Pain Pity = " + player.GetModPlayer<PlayerUpgrades>().Gacha5Pity + "/90]";
                }
                if (line.Mod == "Terraria" && line.Name == "Tooltip7")
                {
                    if (player.GetModPlayer<PlayerUpgrades>().Gacha5050)
                        line.Text = "[c/34f60d:Golden Pain Guranteeded!]";
                    else
                        line.Text = "[c/F54927:Golden Pain Not Guranteeded!]";
                }
                if (line.Mod == "Terraria" && line.Name == "Tooltip8")
                {
                    line.Text = "[c/00CCFF:Uncommon Item Pity = " + player.GetModPlayer<PlayerUpgrades>().Gacha4Pity + "/10]";
                }
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool CanUseItem(Player player)
        {
            return false;
        }
        public override bool? UseItem(Player player)
        {
            return false;
        }
        //failsafe incase accidentally activated
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (Main.rand.Next(90) == 0 || player.GetModPlayer<PlayerUpgrades>().Gacha5Pity == (90 - 1)) //Pulling 5 star
                {
                    //Run 50/50
                    if (Main.rand.Next(2) == 0 || player.GetModPlayer<PlayerUpgrades>().Gacha5050) //success or guranteed
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            float speedX = 0f;
                            float speedY = -5f;
                            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(player.Center.X , player.Center.Y), 0, 0, 170, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1.5f);
                        }
                        SoundEngine.PlaySound(SoundID.Item47 with { Volume = 1.5f, Pitch = 0.5f }, player.Center);
                        SoundEngine.PlaySound(SoundID.Item4 with { Volume = 1.5f, Pitch = -0.5f }, player.Center);

                        player.QuickSpawnItem(null, ItemType<TheGoldenPainMask>(), 1);

                        Main.NewText("Golden Pain Obtained!!!", Color.Gold);
                        CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.Gold, "Golden Pain Obtained!!!", true);
                        //set next 50/50 to be 50/50
                        player.GetModPlayer<PlayerUpgrades>().Gacha5050 = false;
                    }
                    else //failing 50/50
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            float speedX = 0f;
                            float speedY = -5f;
                            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(player.Center.X, player.Center.Y), 0, 0, 170, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1.5f);
                            Main.dust[dust2].noGravity = true;
                        }
                        SoundEngine.PlaySound(SoundID.Item47 with { Volume = 1.5f, Pitch = -0.5f }, player.Center);
                        SoundEngine.PlaySound(SoundID.Item4 with { Volume = 1.5f, Pitch = -0.5f }, player.Center);

                        player.QuickSpawnItem(null, ItemType<ThePainMask>(), 1);
                        //Main.NewText("50/50 failed...", Color.Yellow);
                        CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.Yellow, "50/50 failed...", true);
                        //set next 50/50 to be successful
                        player.GetModPlayer<PlayerUpgrades>().Gacha5050 = true;
                    }
                    //reset 5 star pity
                    player.GetModPlayer<PlayerUpgrades>().Gacha5Pity = 0;
                }
                else //4 star pull
                {
                    if (Main.rand.Next(10) == 0 || player.GetModPlayer<PlayerUpgrades>().Gacha4Pity == (10 - 1)) //success
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            float speedX = 0f;
                            float speedY = -5f;
                            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(player.Center.X, player.Center.Y), 0, 0, 16, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1.5f);
                            Main.dust[dust2].noGravity = true;
                        }
                        SoundEngine.PlaySound(SoundID.Item4 with { Volume = 1f, Pitch = -0.5f }, player.Center);

                        switch (Main.rand.Next(6))
                        {
                            case 0:
                                player.QuickSpawnItem(null, ItemID.Amethyst, 25);
                                break;
                            case 1:
                                player.QuickSpawnItem(null, ItemID.Sapphire, 25);
                                break;
                            case 2:
                                player.QuickSpawnItem(null, ItemID.Topaz, 25);
                                break;
                            case 3:
                                player.QuickSpawnItem(null, ItemID.Emerald, 25);
                                break;
                            case 4:
                                player.QuickSpawnItem(null, ItemID.Ruby, 25);
                                break;
                            case 5:
                                player.QuickSpawnItem(null, ItemID.Diamond, 25);
                                break;
                        }
                        //Main.NewText("Uncommon Pull Success", Color.LightBlue);
                        CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.LightBlue, "Uncommon Pull Success", false);
                        //Add one count to 5 star pity and reset 4 star pity
                        player.GetModPlayer<PlayerUpgrades>().Gacha5Pity += 1;
                        player.GetModPlayer<PlayerUpgrades>().Gacha4Pity = 0;
                    }
                    else //failed, 3 star pull
                    {
                        SoundEngine.PlaySound(SoundID.Item50, player.Center);

                        switch (Main.rand.Next(6))
                        {
                            case 0:
                                player.QuickSpawnItem(null, ItemID.DirtBlock, 50);
                                break;
                            case 1:
                                player.QuickSpawnItem(null, ItemID.StoneBlock, 50);
                                break;
                            case 2:
                                player.QuickSpawnItem(null, ItemID.MudBlock, 50);
                                break;
                            case 3:
                                player.QuickSpawnItem(null, ItemID.Wood, 50);
                                break;
                            case 4:
                                player.QuickSpawnItem(null, ItemID.SandBlock, 50);
                                break;
                            case 5:
                                player.QuickSpawnItem(null, ItemID.SiltBlock, 50);
                                break;
                        }
                        //Main.NewText("4 Star Failure", Color.Red);
                        //add one count to both pities
                        player.GetModPlayer<PlayerUpgrades>().Gacha4Pity += 1;
                        player.GetModPlayer<PlayerUpgrades>().Gacha5Pity += 1;
                    }
                }
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<PainCoin>(), 160)
         .Register();
        }
    }
}