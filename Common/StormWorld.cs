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
using Terraria.WorldBuilding;
using StormDiversMod.Items.Materials;
using MonoMod.Cil;
using StormDiversMod.NPCs;
using StormDiversMod.NPCs.Boss;

namespace StormDiversMod.Common
{
    public class StormWorld : ModSystem //For saving bools and displaying messages
    {
        //All of this is to save a boolean for ever per world

        public static bool planteraMessage; //For the message that appears when plantera is defeated
        public static bool eocMessage; //For the message when the eoc is defeated
        public static bool mechMessage; //For the message when a mech boss is defeated
        public static bool golemMessage; //For the message when the Golem is defeated
        public static bool bloodMessage; //For the message when the evuil boss is defeated

        public static bool stormBossDown; //when the Storm Boss is defeated
        public static bool aridBossDown; //when the Arid Boss is defeated
        public static bool ultimateBossDown; //when the Pain Boss is defeated

        public static bool GraniteChestSpawn;
        public static bool MarbleChestSpawn;
        public static bool MushroomChestSpawn;

        public static bool TouchGrassMode;
        public override void OnWorldLoad()
        {

            planteraMessage = false;
            eocMessage = false;
            mechMessage = false;
            golemMessage = false;
            bloodMessage = false;

            stormBossDown = false;
            aridBossDown = false;
            ultimateBossDown = false;

            GraniteChestSpawn = false;
            MarbleChestSpawn = false;
            MushroomChestSpawn = false;

            TouchGrassMode = false;
        }
        public override void OnWorldUnload()
        {
            planteraMessage = false;
            eocMessage = false;
            mechMessage = false;
            golemMessage = false;
            bloodMessage = false;

            stormBossDown = false;
            aridBossDown = false;
            ultimateBossDown = false;
            GraniteChestSpawn = false;
            MarbleChestSpawn = false;
            MushroomChestSpawn = false;

            TouchGrassMode = false;
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
            if (aridBossDown)
            {
                tag["aridBossDown"] = true;
            }
            if (ultimateBossDown)
            {
                tag["ultimateBossDown"] = true;
            }

            if (GraniteChestSpawn)
            {
                tag["GraniteChestSpawn"] = true;
            }
            if (MarbleChestSpawn)
            {
                tag["MarbleChestSpawn"] = true;
            }
            if (MushroomChestSpawn)
            {
                tag["MushroomChestSpawn"] = true;
            }
            if (TouchGrassMode)
            {
                tag["TouchGrassMode"] = true;
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
            aridBossDown = tag.ContainsKey("aridBossDown");
            ultimateBossDown = tag.ContainsKey("ultimateBossDown");

            GraniteChestSpawn = tag.ContainsKey("GraniteChestSpawn");
            MarbleChestSpawn = tag.ContainsKey("MarbleChestSpawn");
            MushroomChestSpawn = tag.ContainsKey("MushroomChestSpawn");

            TouchGrassMode = tag.ContainsKey("TouchGrassMode");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte(); //Message flags (8 max)

            flags[0] = planteraMessage;
            flags[1] = eocMessage;
            flags[2] = mechMessage;
            flags[3] = golemMessage;
            flags[4] = bloodMessage;

            writer.Write(flags);

            BitsByte flags2 = new BitsByte(); //Boss flags (8 max)

            flags2[0] = stormBossDown;
            flags2[1] = aridBossDown;
            flags2[2] = ultimateBossDown;

            writer.Write(flags2);

            BitsByte flags3 = new BitsByte(); //Chest flags (8 max)

            flags3[0] = GraniteChestSpawn;
            flags3[1] = MarbleChestSpawn;
            flags3[2] = MushroomChestSpawn;

            writer.Write(flags3);

            BitsByte flags4 = new BitsByte(); //Misc (8 max)

            flags4[0] = TouchGrassMode;

            writer.Write(flags3);

        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();

            planteraMessage = flags[0];
            eocMessage = flags[1];
            mechMessage = flags[2];
            golemMessage = flags[3];
            bloodMessage = flags[4];

            BitsByte flags2 = reader.ReadByte();

            stormBossDown = flags2[0];
            aridBossDown = flags2[1];
            ultimateBossDown = flags2[2];

            BitsByte flags3 = reader.ReadByte();

            GraniteChestSpawn = flags3[0];
            MarbleChestSpawn = flags3[1];
            MushroomChestSpawn = flags3[2];

            BitsByte flags4 = reader.ReadByte();

            TouchGrassMode = flags4[0];
        }
        public override void PostWorldGen()
        {

        }
        //int granplaced = 0;
        //int marbleplaced = 0;
        //int mushplaced = 0;
        public override void PreUpdateWorld()
        {
            if (TouchGrassMode)
                TouchGrassMode = false;

            //For the messages when a boss is defeated
            if (NPC.downedBoss1 && !eocMessage) //EoC
            {
                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("A stronger life force radiates from the Granite, Marble, and Mushroom caves!"), new Color(96, 211, 255));
                }
                else if (Main.netMode == 0) // Single Player
                {

                    Main.NewText("A stronger life force radiates from the Granite, Marble, and Mushroom caves!", 96, 211, 255);
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
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Infernal souls infect those that dwell in the underworld."), new Color(224, 141, 255));
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText("Infernal souls infect those that dwell in the underworld.", 224, 141, 255);
                }
                mechMessage = true;
            }
            if (NPC.downedPlantBoss && !planteraMessage) //Plant
            {
                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("The Derplings begin to shed their shells."), new Color(47, 86, 146));

                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("The ancient temple's curse has been lifted!"), new Color(204, 101, 22));

                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText("The Derplings begin to shed their shells.", 47, 86, 146);

                    Main.NewText("The ancient temple's curse has been lifted!", 204, 101, 22);
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
        public override void ModifyGameTipVisibility(IReadOnlyList<GameTipData> gameTips)
        {
            // If you wish to add your OWN tips, then you have to put them in the stupid Localization file which is stupid and stupid.
        }
    }
    public class KillPlant : GlobalNPC
    {
        public override void OnKill(NPC npc) //when plant is killed add a bool to the player
        {
            if (npc.type == NPCID.Plantera || (npc.type == ModContent.NPCType<StormBoss>() && GetInstance<ConfigurationsGlobal>().StormBossSkipsPlant)) //add bool to player
            {
                var player = Main.LocalPlayer;
                if (player.GetModPlayer<PlayerUpgrades>().NoTempleCurse == false)
                {
                    player.GetModPlayer<PlayerUpgrades>().NoTempleCurse = true;
                    //Main.NewText("Your curse has been lifted", Color.Orange);
                }
            }
            base.OnKill(npc);
        }
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