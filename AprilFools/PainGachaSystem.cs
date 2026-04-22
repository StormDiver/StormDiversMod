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
using StormDiversMod.AprilFools; 
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

namespace StormDiversMod.AprilFools
{
    //Overall save system
    public class GachaTimers : ModPlayer
    {
        public int Gacha5Pity; //Pity for 5 star
        public int Gacha4Pity; //Pity for 4 star
        public bool Gacha5050; //5050 Guaranteed?
        public int GachaCoinDropCount; //How many coins have been dropped by enemies
        public int GachaCoinDropCooldown; //Cooldown for when coin drops reset
        public int GachaCoinDropBossCount; //How many times has a boss dropped coins
        public int GachaCoinDropBossCooldown; //Cooldown for when bosses can drop more coins
        public override void PostUpdateEquips()
        {
            if (GetInstance<ConfigurationsGlobal>().AFGacha)
            {
                if (GachaCoinDropCooldown > 0)
                    GachaCoinDropCooldown--;
                else
                {
                    GachaCoinDropCooldown = 3600; //every minute reset the count (limit of 30)
                    GachaCoinDropCount = 0;
                }

                //Main.NewText("Drop Reset = " + GachaCoinDropCooldown, Color.Red);
                //Main.NewText("Drop Amount limit = " + GachaCoinDropCount + "/30", Color.Red);

                if (GachaCoinDropBossCooldown > 0)
                    GachaCoinDropBossCooldown--;
                else
                {
                    GachaCoinDropBossCooldown = 3600 * 4; //every 4 minutes reset the count (limit of 3 * 60) (45 per minute)
                    GachaCoinDropBossCount = 0;
                }
                //Main.NewText("Boss Cooldown = " + GachaCoinDropBossCooldown, Color.Orange);
                //Main.NewText("Boss Amount Limit = " + GachaCoinDropBossCount + "/3", Color.Orange);
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag["Gacha5Pity"] = Gacha5Pity;
            tag["Gacha4Pity"] = Gacha4Pity;
            tag["Gacha5050"] = Gacha5050;
            tag["GachaCoinDropCount"] = GachaCoinDropCount;
            tag["GachaCoinDropCooldown"] = GachaCoinDropCooldown;
            tag["GachaCoinDropBossCount"] = GachaCoinDropBossCount;
            tag["GachaCoinDropBossCooldown"] = GachaCoinDropBossCooldown;
        }
        public override void LoadData(TagCompound tag)
        {
            Gacha5Pity = tag.GetInt("Gacha5Pity");
            Gacha4Pity = tag.GetInt("Gacha4Pity");
            Gacha5050 = tag.GetBool("Gacha5050");
            GachaCoinDropCount = tag.GetInt("GachaCoinDropCount");
            GachaCoinDropCooldown = tag.GetInt("GachaCoinDropCooldown");
            GachaCoinDropBossCount = tag.GetInt("GachaCoinDropBossCount");
            GachaCoinDropBossCooldown = tag.GetInt("GachaCoinDropBossCooldown");
        }

        //idc about multiplayer
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            base.SendClientChanges(clientPlayer);
        }
        public override void CopyClientState(ModPlayer targetCopy)
        {
            base.CopyClientState(targetCopy);
        }
    }
    //Items
    public class PainCoin : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pain Coin");

            //Tooltip.SetDefault("'Used to create Pain Tickets'");
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 6));
            Item.ResearchUnlockCount = -1;
            ItemID.Sets.ShimmerTransformToItem[Item.type] = ItemID.SilverCoin;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var player = Main.LocalPlayer;

            foreach (TooltipLine line in tooltips)
            {
                if (GetInstance<ConfigurationsGlobal>().AFGacha)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        if (player.GetModPlayer<GachaTimers>().GachaCoinDropCount < 30)
                            line.Text = "[c/34f60d:Enemy Drop Limit = " + player.GetModPlayer<GachaTimers>().GachaCoinDropCount + "/30]";
                        else
                            line.Text = "[c/F54927:Enemy Drop Limit = " + player.GetModPlayer<GachaTimers>().GachaCoinDropCount + "/30]";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                    {
                        line.Text = "[c/efd127:Enemy Drop Limit reset in " + player.GetModPlayer<GachaTimers>().GachaCoinDropCooldown / 60 + " Seconds]";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                    {
                        if (player.GetModPlayer<GachaTimers>().GachaCoinDropBossCount < 3)
                            line.Text = "[c/34f60d:Boss Drop Limit = " + player.GetModPlayer<GachaTimers>().GachaCoinDropBossCount + "/3]";
                        else
                            line.Text = "[c/F54927:Boss Drop Limit = " + player.GetModPlayer<GachaTimers>().GachaCoinDropBossCount + "/3]";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip5")
                    {
                        line.Text = "[c/efd127:Boss Drop Limit Reset in " + player.GetModPlayer<GachaTimers>().GachaCoinDropBossCooldown / 60 + " Seconds]";
                    }
                }
                else
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        line.Text = "[c/7D7D7D:Drops Disabled via config]";
                    }
                    if (line.Mod == "Terraria" && (line.Name == "Tooltip3" || line.Name == "Tooltip4" || line.Name == "Tooltip5"))
                    {
                        line.Text = "";
                    }
                }
            }
        }
        public override void AddRecipes()
        {
            if (GetInstance<ConfigurationsGlobal>().AFGacha)
            {
                Recipe recipe = Recipe.Create(ModContent.ItemType<PainCoin>(), 5);
                recipe.AddIngredient(ItemID.GoldCoin, 1);

                recipe.Register();
            }
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
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
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
                    if (player.GetModPlayer<GachaTimers>().Gacha5Pity < 30) //different colours for 0-29, 30-59, and 60-89
                        line.Text = "[c/1EFF00:Golden Pain Pity = " + player.GetModPlayer<GachaTimers>().Gacha5Pity + "/90]";
                    else if (player.GetModPlayer<GachaTimers>().Gacha5Pity >= 30 && player.GetModPlayer<GachaTimers>().Gacha5Pity < 60)
                        line.Text = "[c/FFE100:Golden Pain Pity = " + player.GetModPlayer<GachaTimers>().Gacha5Pity + "/90]";
                    else
                        line.Text = "[c/FF6F00:Golden Pain Pity = " + player.GetModPlayer<GachaTimers>().Gacha5Pity + "/90]";
                }
                if (line.Mod == "Terraria" && line.Name == "Tooltip7")
                {
                    if (player.GetModPlayer<GachaTimers>().Gacha5050)
                        line.Text = "[c/34f60d:Golden Pain Guranteeded!]";
                    else
                        line.Text = "[c/F54927:Golden Pain Not Guranteeded!]";
                }
                if (line.Mod == "Terraria" && line.Name == "Tooltip8")
                {
                    line.Text = "[c/00CCFF:Uncommon Item Pity = " + player.GetModPlayer<GachaTimers>().Gacha4Pity + "/10]";
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
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (Main.rand.Next(90) == 0 || player.GetModPlayer<GachaTimers>().Gacha5Pity == (90 - 1)) //Pulling 5 star
                {
                    //Run 50/50
                    if (Main.rand.Next(2) == 0 || player.GetModPlayer<GachaTimers>().Gacha5050) //success or guranteed
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            float speedX = 0f;
                            float speedY = -5f;
                            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(player.Center.X, player.Center.Y), 0, 0, 170, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1.5f);
                        }
                        SoundEngine.PlaySound(SoundID.Item47 with { Volume = 1.5f, Pitch = 0.5f }, player.Center);
                        SoundEngine.PlaySound(SoundID.Item4 with { Volume = 1.5f, Pitch = -0.5f }, player.Center);

                        player.QuickSpawnItem(null, ItemType<TheGoldenPainMask>(), 1);

                        Main.NewText("Golden Pain Obtained!!!", Color.Gold);
                        CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.Gold, "Golden Pain Obtained!!!", true);
                        //set next 50/50 to be 50/50
                        player.GetModPlayer<GachaTimers>().Gacha5050 = false;
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
                        player.GetModPlayer<GachaTimers>().Gacha5050 = true;
                    }
                    //reset 5 star pity
                    player.GetModPlayer<GachaTimers>().Gacha5Pity = 0;
                }
                else //4 star pull
                {
                    if (Main.rand.Next(10) == 0 || player.GetModPlayer<GachaTimers>().Gacha4Pity == (10 - 1)) //success
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
                        player.GetModPlayer<GachaTimers>().Gacha5Pity += 1;
                        player.GetModPlayer<GachaTimers>().Gacha4Pity = 0;
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
                        player.GetModPlayer<GachaTimers>().Gacha4Pity += 1;
                        player.GetModPlayer<GachaTimers>().Gacha5Pity += 1;
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
    //Enemy Drops
    public class PainGachaDrops : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        //Drops--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void OnKill(NPC npc) //Used for items that don't work with the new method
        {
            //Pain Tickets and coins GACHA
            if (GetInstance<ConfigurationsGlobal>().AFGacha)
            {
                if (npc.lifeMax > 5 && !npc.boss && !npc.SpawnedFromStatue && Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].GetModPlayer<GachaTimers>().GachaCoinDropCount < 30) //30 limit, resets every minute
                {
                    Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<PainCoin>());
                    Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].GetModPlayer<GachaTimers>().GachaCoinDropCount++;
                }
                if (npc.boss)
                {
                    if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].GetModPlayer<GachaTimers>().GachaCoinDropBossCount < 3) //3 limit, resets every 5 minutes
                    {
                        Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<PainCoin>(), 60);
                        Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].GetModPlayer<GachaTimers>().GachaCoinDropBossCount++; // 1 minute cooldown
                    }
                }
                //200 from most bosses first time
                if ((npc.type == NPCID.EyeofCthulhu && !NPC.downedBoss1) || (npc.type == NPCID.BrainofCthulhu && !NPC.downedBoss2) || (npc.type == NPCID.SkeletronHead && !NPC.downedBoss3) ||
                    (npc.type == NPCID.QueenBee && !NPC.downedQueenBee) || (npc.type == NPCID.KingSlime && !NPC.downedSlimeKing) || (npc.type == NPCID.WallofFlesh && !Main.hardMode) ||
                    (npc.type == NPCID.TheDestroyer && !NPC.downedMechBoss1) || (npc.type == NPCID.Retinazer && !NPC.downedMechBoss2) || (npc.type == NPCID.SkeletronPrime && !NPC.downedMechBoss3) ||
                    (npc.type == NPCID.Plantera && !NPC.downedPlantBoss) || (npc.type == NPCID.Golem && !NPC.downedGolemBoss) || (npc.type == NPCID.HallowBoss && !NPC.downedEmpressOfLight) ||
                     (npc.type == NPCID.QueenSlimeBoss && !NPC.downedQueenSlime) || (npc.type == NPCID.CultistBoss && !NPC.downedAncientCultist) || (npc.type == NPCID.MoonLordCore && !NPC.downedMoonlord))
                {
                    Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0, 0), ModContent.ItemType<PainCoin>(), 200);
                }
            }
        }
    }

    //Chest Generation
    public class PainGacahChest : ModSystem
    {
        public override void PostWorldGen()
        {
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];

                //GACHA CHESTS
                if (GetInstance<ConfigurationsGlobal>().AFGacha)
                {
                    int[] ChestGachaPainToken = { ItemType<PainCoin>() };
                    int ChestGachaPainTokenCount = 0;
                    //Wooden Chests = 10
                    if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && (Main.tile[chest.x, chest.y].TileFrameX == 0 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 12 * 36)) //Look in Tiles_467 for the tile, start from 0
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGachaPainToken));
                                chest.item[inventoryIndex].stack = 10;
                                ChestGachaPainTokenCount = (ChestGachaPainTokenCount + 1) % ChestGachaPainToken.Length;
                                inventoryIndex++;

                                break;
                            }
                        }
                    }
                    //Gold/Ice/Desert/Jungle Chests = 20
                    if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && (Main.tile[chest.x, chest.y].TileFrameX == 1 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 8 * 36 ||
                        Main.tile[chest.x, chest.y].TileFrameX == 10 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 11 * 36)) //Look in Tiles_467 for the tile, start from 0
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGachaPainToken));
                                chest.item[inventoryIndex].stack = 20;
                                ChestGachaPainTokenCount = (ChestGachaPainTokenCount + 1) % ChestGachaPainToken.Length;
                                inventoryIndex++;

                                break;
                            }
                        }
                    }
                    //Gold/Ice/Desert Chests = 20
                    if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers2 && Main.tile[chest.x, chest.y].TileFrameX == 10 * 36) //Look in Tiles_467 for the tile, start from 0
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGachaPainToken));
                                chest.item[inventoryIndex].stack = 20;
                                ChestGachaPainTokenCount = (ChestGachaPainTokenCount + 1) % ChestGachaPainToken.Length;
                                inventoryIndex++;

                                break;
                            }
                        }
                    }
                    //Granite/marble/Mushroom/Water/Skyware/Webbed Chests = 30
                    if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && (Main.tile[chest.x, chest.y].TileFrameX == 50 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 51 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 32 * 36 ||
                        Main.tile[chest.x, chest.y].TileFrameX == 17 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 13 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 15 * 36)) //Look in Tiles_467 for the tile, start from 0
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGachaPainToken));
                                chest.item[inventoryIndex].stack = 30;
                                ChestGachaPainTokenCount = (ChestGachaPainTokenCount + 1) % ChestGachaPainToken.Length;
                                inventoryIndex++;

                                break;
                            }
                        }
                    }
                    //Locked Gold Chests = 25
                    if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && (Main.tile[chest.x, chest.y].TileFrameX == 2 * 36)) //Look in Tiles_467 for the tile, start from 0
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGachaPainToken));
                                chest.item[inventoryIndex].stack = 25;
                                ChestGachaPainTokenCount = (ChestGachaPainTokenCount + 1) % ChestGachaPainToken.Length;
                                inventoryIndex++;

                                break;
                            }
                        }
                    }
                    //Shadow Chests = 50
                    if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 4 * 36) //Look in Tiles_467 for the tile, start from 0
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGachaPainToken));
                                chest.item[inventoryIndex].stack = 50;
                                ChestGachaPainTokenCount = (ChestGachaPainTokenCount + 1) % ChestGachaPainToken.Length;
                                inventoryIndex++;

                                break;
                            }
                        }
                    }
                    //Lizard Chests = 100
                    if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 16 * 36) //Look in Tiles_467 for the tile, start from 0
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGachaPainToken));
                                chest.item[inventoryIndex].stack = 100;
                                ChestGachaPainTokenCount = (ChestGachaPainTokenCount + 1) % ChestGachaPainToken.Length;
                                inventoryIndex++;

                                break;
                            }
                        }
                    }
                    //Biome Chests = 200
                    if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && (Main.tile[chest.x, chest.y].TileFrameX == 24 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 25 * 36
                        || Main.tile[chest.x, chest.y].TileFrameX == 26 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 27 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 28 * 36)) //Look in Tiles_467 for the tile, start from 0
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGachaPainToken));
                                chest.item[inventoryIndex].stack = 200;
                                ChestGachaPainTokenCount = (ChestGachaPainTokenCount + 1) % ChestGachaPainToken.Length;
                                inventoryIndex++;

                                break;
                            }
                        }
                    }
                    //Biome Chest = 200
                    if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers2 && Main.tile[chest.x, chest.y].TileFrameX == 13 * 36) //Look in Tiles_467 for the tile, start from 0
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGachaPainToken));
                                chest.item[inventoryIndex].stack = 200;
                                ChestGachaPainTokenCount = (ChestGachaPainTokenCount + 1) % ChestGachaPainToken.Length;
                                inventoryIndex++;

                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    //The reward
    [AutoloadEquip(EquipType.Head)]
    public class TheGoldenPainMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Golden Pain");
            //Tooltip.SetDefault("The purest of pain");
            Item.ResearchUnlockCount = -1;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                {
                    if (ItemSlot.ShiftInUse)
                        line.Text = "[c/efd127:GOLDEN PAIN!!!!]";
                    else
                        line.Text = "[c/efd127:The Legendary Pain!]";
                }
            }
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.vanity = true;
        }

        public override bool OnPickup(Player player)
        {
            if (!GetInstance<ConfigurationsIndividual>().NoPain)
            {
                if (player.Male)
                    SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSound") with { Volume = 1.5f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, player.Center);
                else
                    SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSoundFemale") with { Volume = 1.5f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, player.Center);
            }
            return base.OnPickup(player);
        }
    }
    public class PainGachaItems : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModContent.ItemType<PainCoin>() || item.type == ModContent.ItemType<PainTicket>() || item.type == ModContent.ItemType<TheGoldenPainMask>())
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "ItemName")
                    {
                        line.Text = line.Text + "\n[c/FF1493:APRIL FOOLS!!!]";
                    }
                }
            }
        }
    }
}