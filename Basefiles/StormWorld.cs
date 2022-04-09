using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Items.Weapons;
using StormDiversMod.Items.Pets;


namespace StormDiversMod.Basefiles
{
    public class StormWorld : ModSystem
    {
        //All of this is to save a boolean for ever per world
        

        public static bool planteraMessage; //For the message that appears when plantera is defeated
        public static bool eocMessage; //For the message when the eoc is defeated
        public static bool mechMessage; //For the message when a mech boss is defeated
        public static bool golemMessage; //For the message when the Golem is defeated
        public static bool bloodMessage; //For the message when the evuil boss is defeated

        public static bool stormBossDown; //when the Storm Boss is defeated


        public override void OnWorldLoad()
        {
  
            planteraMessage = false;
            eocMessage = false;
            mechMessage = false;
            golemMessage = false;
            bloodMessage = false;

            stormBossDown = false;
        }
        public override void OnWorldUnload()
        {
            planteraMessage = false;
            eocMessage = false;
            mechMessage = false;
            golemMessage = false;
            bloodMessage = false;

            stormBossDown = false;

        }
        public override void SaveWorldData(TagCompound tag)
        {
            if (planteraMessage)
            {
                tag["planteraMessage"] = true;
            }
            if (eocMessage)
            {
                tag["eocMessage"] = true;
            }
            if (mechMessage)
            {
                tag["mechMessage"] = true;
            }
            if (golemMessage)
            {
                tag["golemMessage"] = true;
            }
            if (bloodMessage)
            {
                tag["bloodMessage"] = true;
            }

            if (stormBossDown)
            {
                tag["stormBossDown"] = true;
            }
        }
        public override void LoadWorldData(TagCompound tag)
        {
            planteraMessage = tag.ContainsKey("planteraMessage");
            eocMessage = tag.ContainsKey("eocMessage");
            mechMessage = tag.ContainsKey("mechMessage");
            golemMessage = tag.ContainsKey("golemMessage");
            bloodMessage = tag.ContainsKey("bloodMessage");

            stormBossDown = tag.ContainsKey("stormBossDown");

        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
    
            flags[4] = planteraMessage;
            flags[5] = eocMessage;
            flags[6] = mechMessage;
            flags[7] = golemMessage;
            flags[8] = bloodMessage;

            flags[9] = stormBossDown;


            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
  
            planteraMessage = flags[4];
            eocMessage = flags[5];
            mechMessage = flags[6];
            golemMessage = flags[7];
            bloodMessage = flags[8];

            stormBossDown = flags[9];
        }

        public override void PostWorldGen()
        {
            //Make items appears in chests

            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];

                //For the wooden surface chests
                int[] ChestWoodStick = { ItemType<WoodPointyStick>() };
                int ChestWoodStickCount = 0;

                int[] ChestWoodBow = { ItemType<WoodCrossbow>() };
                int ChestWoodBowCount = 0;

                int[] ChestWoodNeck = { ItemType<Items.Accessory.WoodNecklace>() };
                int ChestWoodNeckCount = 0;

                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 0 * 36) //Look in Tiles_21 for the tile, start from 0
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if (WorldGen.genRand.NextBool(2))
                            {
                                int choice = Main.rand.Next(3);

                                //if (WorldGen.genRand.NextBool(2))
                                if (choice == 0)
                                {
                                    chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestWoodStick));
                                    ChestWoodStickCount = (ChestWoodStickCount + 1) % ChestWoodStick.Length;
                                }
                                else if (choice == 1)
                                {
                                    chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestWoodBow));
                                    ChestWoodBowCount = (ChestWoodBowCount + 1) % ChestWoodBow.Length;
                                }
                                else
                                {
                                    chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestWoodNeck));
                                    ChestWoodNeckCount = (ChestWoodNeckCount + 1) % ChestWoodNeck.Length;
                                }
                            }

                            break;
                        }
                    }

                }

                //For the Icicle Saff in Frozen Chest
                int[] ChestIceStaff = { ItemType<IceStaff>() };
                int ChestIceStaffCount = 0;


                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 11 * 36) //Look in Tiles_21 for the tile, start from 0
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if (WorldGen.genRand.NextBool(4))
                            {

                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestIceStaff));
                                ChestIceStaffCount = (ChestIceStaffCount + 1) % ChestIceStaff.Length;

                            }

                            break;
                        }
                    }

                }


                //For Underground desert chests
                int[] ChestAncientGun = { ItemType<SandstoneGun>() };
                int ChestAncientGunCount = 0;

                int[] ChestAncientGunAmmo = { ItemID.MusketBall };
                int ChestAncientGunAmmoCount = 0;



                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers2 && Main.tile[chest.x, chest.y].TileFrameX == 10 * 36) //Look in Tiles_467 for the tile, start from 0
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {

                            if (WorldGen.genRand.NextBool(4)) //4
                            {

                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestAncientGun));
                                ChestAncientGunCount = (ChestAncientGunCount + 1) % ChestAncientGun.Length;
                                inventoryIndex++;

                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestAncientGunAmmo));
                                chest.item[inventoryIndex].stack = WorldGen.genRand.Next(60, 100);
                                ChestAncientGunAmmoCount = (ChestAncientGunAmmoCount + 1) % ChestAncientGunAmmo.Length;

                            }
                         

                            break;
                        }
                    }

                }

                //For the Mossy Repeater in Jungle Chest
                int[] ChestMossyRep = { ItemType<MossRepeater>() };
                int ChestMossyRepCount = 0;


                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 10 * 36) //Look in Tiles_21 for the tile, start from 0
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if (WorldGen.genRand.NextBool(4))
                            {

                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestMossyRep));
                                ChestMossyRepCount = (ChestMossyRepCount + 1) % ChestMossyRep.Length;

                            }

                            break;
                        }
                    }

                }
                //For the Mini Drill in Golden chests
                int[] ChestMiniDrill = { ItemType<Items.Tools.FastDrill>()};
                int ChestMiniDrillCount = 0;


                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 1 * 36) //Look in Tiles_21 for the tile, start from 0
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if (WorldGen.genRand.NextBool(7))
                            {

                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestMiniDrill));
                                ChestMiniDrillCount = (ChestMiniDrillCount + 1) % ChestMiniDrill.Length;

                            }

                            break;
                        }
                    }

                }

                //For Dungeon Chests
                int[] ChestLauncher = { ItemType<ProtoLauncher>() };
                int ChestLauncherCount = 0;

                int[] ChestLauncherAmmo = { ItemType<Items.Ammo.ProtoGrenade>() };
                int ChestLauncherAmmoCount = 0;

                int[] ChestTwilightPet = { ItemType<TwilightPetItem>() };
                int ChestTwilightPetCount = 0;


                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 2 * 36) //Look in Tiles_21 for the tile, start from 0
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                           
                            if (WorldGen.genRand.NextBool(4)) //4
                            {

                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestLauncher));
                                ChestLauncherCount = (ChestLauncherCount + 1) % ChestLauncher.Length;
                                inventoryIndex++;

                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestLauncherAmmo));
                                chest.item[inventoryIndex].stack = WorldGen.genRand.Next(75, 120);
                                ChestLauncherAmmoCount = (ChestLauncherAmmoCount + 1) % ChestLauncherAmmo.Length;
                                inventoryIndex++;

                            }
                            if (WorldGen.genRand.NextBool(5)) // 5
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestTwilightPet));
                                ChestTwilightPetCount = (ChestTwilightPetCount + 1) % ChestTwilightPet.Length;

                            }

                            break;
                        }
                    }

                }

                //For the Jar of Hearts in shadow chests
                int[] ChestHeart = { ItemType<Items.Accessory.HeartJar>() };
                int ChestHeartCount = 0;

                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 4 * 36)//Look in Tiles_21 for the tile, start from 0
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if (WorldGen.genRand.NextBool(3))
                            {

                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestHeart));
                                ChestHeartCount = (ChestHeartCount + 1) % ChestHeart.Length;



                            }

                            break;
                        }
                    }

                }

                //For the Webstaff in Web chests
                int[] ChestWeb = { ItemType<WebStaff>() };
                int ChestWebCount = 0;
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 15 * 36)//Look in Tiles_21 for the tile, start from 0
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if (WorldGen.genRand.NextBool(1))
                            {

                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestWeb));
                                ChestWebCount = (ChestWebCount + 1) % ChestWeb.Length;



                            }

                            break;
                        }
                    }

                }

                //For the Granite weapons
                int[] ChestGraniteRanged = { ItemType<GraniteRifle>() };
                int ChestGraniteRangedCount = 0;
                int[] ChestGraniteAmmo = { ItemID.MusketBall };
                int ChestGraniteCountAmmo = 0;
                int[] ChestGraniteMelee = { ItemType<GraniteYoyo>() };
                int ChestGraniteMeleeCount = 0;
                int[] ChestGraniteMage = { ItemType<GraniteStaff>() };
                int ChestGraniteMageCount = 0;

                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 50 * 36)//Look in Tiles_21 for the tile, start from 0
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            int choice = Main.rand.Next(3);

                            //if (WorldGen.genRand.NextBool(2))
                            if (choice == 0)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGraniteRanged));
                                ChestGraniteRangedCount = (ChestGraniteRangedCount + 1) % ChestGraniteRanged.Length;
                                inventoryIndex++;

                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGraniteAmmo));
                                chest.item[inventoryIndex].stack = WorldGen.genRand.Next(60, 100);
                                ChestGraniteCountAmmo = (ChestGraniteCountAmmo + 1) % ChestGraniteAmmo.Length;

                            }
                            if (choice == 1)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGraniteMelee));
                                ChestGraniteMeleeCount = (ChestGraniteMeleeCount + 1) % ChestGraniteMelee.Length;
                            }
                            if (choice == 2)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestGraniteMage));
                                ChestGraniteMageCount = (ChestGraniteMageCount + 1) % ChestGraniteMage.Length;
                            }

                            break;
                        }
                    }

                }
                //For the Marble weapons
                int[] ChestMarbleRanged = { ItemType<GladiatorBow>() };
                int ChestMarbleRangedCount = 0;
                int[] ChestMarbleMelee = { ItemType<GladiatorSpear>() };
                int ChestMarbleMeleeCount = 0;
                int[] ChestMarbleMage = { ItemType<GladiatorStaff>() };
                int ChestMarbleMageCount = 0;
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 51 * 36)//Look in Tiles_21 for the tile, start from 0
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            int choice = Main.rand.Next(3);

                            //if (WorldGen.genRand.NextBool(2))
                            if (choice == 0)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestMarbleRanged));
                                ChestMarbleRangedCount = (ChestMarbleRangedCount + 1) % ChestMarbleRanged.Length;
                               

                            }
                            if (choice == 1)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestMarbleMelee));
                                ChestMarbleMeleeCount = (ChestMarbleMeleeCount + 1) % ChestMarbleMelee.Length;
                            }
                            if (choice == 2)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestMarbleMage));
                                ChestMarbleMageCount = (ChestMarbleMageCount + 1) % ChestMarbleMage.Length;
                            }

                            break;
                        }
                    }

                }
                //For the Mushroom weapons
                int[] ChestMushroomRanged = { ItemType<MushroomBow>() };
                int ChestMushroomRangedCount = 0;
                int[] ChestMushroomMelee = { ItemType<MushroomSword>() };
                int ChestMushroomMeleeCount = 0;
                int[] ChestMushroomMage = { ItemType<MushroomStaff>() };
                int ChestMushroomMageCount = 0;
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 32 * 36)//Look in Tiles_21 for the tile, start from 0
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            int choice = Main.rand.Next(3);

                            //if (WorldGen.genRand.NextBool(2))
                            if (choice == 0)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestMushroomRanged));
                                ChestMushroomRangedCount = (ChestMushroomRangedCount + 1) % ChestMushroomRanged.Length;


                            }
                            if (choice == 1)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestMushroomMelee));
                                ChestMushroomMeleeCount = (ChestMushroomMeleeCount + 1) % ChestMushroomMelee.Length;
                            }
                            if (choice == 2)
                            {
                                chest.item[inventoryIndex].SetDefaults(Main.rand.Next(ChestMushroomMage));
                                ChestMushroomMageCount = (ChestMushroomMageCount + 1) % ChestMushroomMage.Length;
                            }

                            break;
                        }
                    }

                }


            }



        }
        public override void PreUpdateWorld()
        {
            //For the messages when a boss is defeated
            if (NPC.downedBoss1 && !eocMessage) //EoC
            {
                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("A stronger life force radiates from the minor underground biomes!"), new Color(96, 211, 255));
                }
                else if (Main.netMode == 0) // Single Player
                {

                    Main.NewText("A stronger life force radiates from the minor underground biomes!", 96, 211, 255);
                }
                eocMessage = true;
            }
            if (NPC.downedBoss2 && !bloodMessage) //Eow/BoC
            {
                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Essences of the blood moon begin to drop."), new Color(233, 70, 70));
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText("Essences of the blood moon begin to drop.", 233, 70, 70);
                }
                bloodMessage = true;
            }
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !mechMessage) //Mech
            {
                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Fiery souls infect those trapped in the underworld."), new Color(224, 141, 255));
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText("Fiery souls infect those trapped in the underworld.", 224, 141, 255);
                }
                mechMessage = true;
            }
            if (NPC.downedPlantBoss && !planteraMessage) //Plant
            {
                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("The Derplings begin to shed their shells."), new Color(47, 86, 146));

                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("The ancient temple defenses have greatly weakened!"), new Color(204, 101, 22));

                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText("The Derplings begin to shed their shells.", 47, 86, 146);

                    Main.NewText("The ancient temple defenses have greatly weakened!", 204, 101, 22);
                }
                planteraMessage = true;
            }        
           
            if (NPC.downedGolemBoss && !golemMessage) //Golem
            {
                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Sentient asteroids have entered the atmosphere!"), new Color(179, 151, 238));
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText("Sentient asteroids have entered the atmosphere!", 179, 151, 238);
                }
                golemMessage = true;
            }
            
            //To spawn the ores
            /*if (SpawnIceOre && !IceSpawned)
            {
                if (!GetInstance<Configurations>().PreventOreSpawn) 
                {
                    Main.NewText("Frozen ores can now be obtained from the depths of the Frozen Caves", 0, 255, 255);
                }
                else  //If generating ore is disabled
                {
                    Main.NewText("Frozen ores now drop from the creatures in the depths of the Frozen Caves", 0, 255, 255);

                }

                if (!GetInstance<Configurations>().PreventOreSpawn)
                {
                    for (int k = 0; k < (int)((WorldGen.rockLayer * Main.maxTilesY) * 200E-05); k++)   //40E-05 is how many veins ore is going to spawn , change 40 to a lover value if you want less vains ore or higher value for more veins ore
                    {
                        int X = WorldGen.genRand.Next(0, Main.maxTilesX);
                        int Y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 100);
                        //this is the coordinates where the veins ore will spawn, so in Cavern layer
                        Tile tile = Framing.GetTileSafely(X, Y);
                        if (tile.type == TileID.IceBlock)
                        {
                            WorldGen.TileRunner(X, Y, WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(15, 22), mod.TileType("IceOrePlaced"));   // is the vein ore sizes, so 9 to 15 blocks or 5 to 9 blocks, 
                        }
                    }
                }
                IceSpawned = true;
            }
            if (SpawnDesertOre && !DesertSpawned)
            {
                if (!GetInstance<Configurations>().PreventOreSpawn)
                {
                    Main.NewText("Arid ores can now be obtained from the depths of the Sandy Tunnels", 204, 132, 0);
                }
                else //If generating ore is disabled
                {
                    Main.NewText("Arid ores now drop from creatures in the depths of the Sandy Tunnels", 204, 132, 0);

                }

                if (!GetInstance<Configurations>().PreventOreSpawn)
                {
                    for (int k = 0; k < (int)((WorldGen.worldSurfaceLow * Main.maxTilesY) * 2000E-05); k++)   //40E-05 is how many veins ore is going to spawn , change 40 to a lover value if you want less vains ore or higher value for more veins ore
                    {
                        int X = WorldGen.genRand.Next((int)0, Main.maxTilesX);
                        int Y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY); //this is the coordinates where the veins ore will spawn, so in Cavern layer
                        Tile tile = Framing.GetTileSafely(X, Y);
                        if (tile.type == TileID.HardenedSand)
                        {
                            WorldGen.TileRunner(X, Y, WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(10, 16), mod.TileType("DesertOrePlaced"));   //is the vein ore sizes, so 9 to 15 blocks or 5 to 9 blocks, 
                        }
                    }
                }
                DesertSpawned = true;
            }*/

        }
    }
    public class WorldOre : GlobalNPC
    {
        /*public override void NPCLoot(NPC npc)
        {
            //set bools when the enemy is killed for the first time, these are saved at the top
            if (npc.type == NPCID.IceGolem) //this is where you choose what vanilla npc you want  , for a modded npc add this instead  if (npc.type == mod.NPCType("ModdedNpcName"))
            {
                if (!StormWorld.SpawnIceOre)
                {
                    StormWorld.SpawnIceOre = true;
                }
            }
            if (npc.type == NPCID.SandElemental) //this is where you choose what vanilla npc you want  , for a modded npc add this instead  if (npc.type == mod.NPCType("ModdedNpcName"))
            {
                if (!StormWorld.SpawnDesertOre)
                {

                    StormWorld.SpawnDesertOre = true;
                }

            }
        }*/
    }
}