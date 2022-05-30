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
    public class ChestGeneration : ModSystem
    {

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
                int[] ChestMiniDrill = { ItemType<Items.Tools.FastDrill>() };
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
    }
}