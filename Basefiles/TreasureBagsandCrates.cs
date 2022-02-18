using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using StormDiversMod.Items.Armour;
using StormDiversMod.Items.Weapons;
using StormDiversMod.Items.Materials;
using StormDiversMod.Items.Pets;
using StormDiversMod.Basefiles;
using StormDiversMod.Items.OresandBars;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.Basefiles
{
    public class TreasureBagsandCrates : GlobalItem
    {


        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag" && arg == ItemID.BossBagBetsy) //besty Treasure bag
            {
                player.QuickSpawnItem(ItemType<Items.Accessory.FlameCore>(), Main.rand.Next(1, 1));
            }
          
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) //Any mech boss treasure bag post 3 mechs
            {
                if (context == "bossBag" && arg == ItemID.SkeletronPrimeBossBag || arg == ItemID.TwinsBossBag || arg == ItemID.DestroyerBossBag)
                {
                    player.QuickSpawnItem(ItemType<Items.Accessory.PrimeAccess>(), Main.rand.Next(1, 1));
                }
            }
           
            if (context == "bossBag" && arg == ItemID.CultistBossBag) //Cultist treasure bag
            {
                player.QuickSpawnItem(ItemType<Items.Accessory.LunaticHood>(), Main.rand.Next(1, 1));

                if (Main.rand.Next(100) < 5)

                {
                    player.QuickSpawnItem(ItemType<CultistLazor>(), Main.rand.Next(1, 1));
                   

                }
                int choice = Main.rand.Next(4);

                if (choice == 0)
                {
                    player.QuickSpawnItem(ItemType<CultistBow>(), Main.rand.Next(1, 1));
                }
                if (choice == 1)
                {
                    player.QuickSpawnItem(ItemType<CultistSpear>(), Main.rand.Next(1, 1));
                }
                if (choice == 2)
                {
                    player.QuickSpawnItem(ItemType<CultistTome>(), Main.rand.Next(1, 1));
                }
                if (choice == 3)
                {
                    player.QuickSpawnItem(ItemType<CultistStaff>(), Main.rand.Next(1, 1));

                }
            }
            /*if (context == "bossBag" && Main.hardMode)
            {
                if (Main.rand.Next(100) < 2)

                {
                    player.QuickSpawnItem(ItemType<ContestArmourHelmet>(), Main.rand.Next(1, 1));
                    player.QuickSpawnItem(ItemType<ContestArmourChestplate>(), Main.rand.Next(1, 1));
                    player.QuickSpawnItem(ItemType<ContestArmourLeggings>(), Main.rand.Next(1, 1));
                }

            }*/
            if (context == "lockBox") //proto launcher
            {
                if (Main.rand.Next(100) < 20)
                {
                    player.QuickSpawnItem(ItemType<ProtoLauncher>(), Main.rand.Next(1, 1));
                    player.QuickSpawnItem(ItemType<Items.Ammo.ProtoGrenade>(), Main.rand.Next(52, 80));
                }
                if (Main.rand.Next(100) < 20)
                {
                    player.QuickSpawnItem(ItemType<TwilightPetItem>(), Main.rand.Next(1, 1));
                }
            }
            if (context == "crate" && (arg == ItemID.WoodenCrate || arg == ItemID.WoodenCrateHard)) //Wooden items
            {
                if (Main.rand.Next(100) < 8)
                {
                    int choice = Main.rand.Next(3);

                    //if (WorldGen.genRand.NextBool(2))
                    if (choice == 0)
                    {
                        player.QuickSpawnItem(ItemType<WoodPointyStick>());

                    }
                    else if (choice == 1)
                    {
                        player.QuickSpawnItem(ItemType<WoodCrossbow>());

                    }
                    else
                    {
                        player.QuickSpawnItem(ItemType<Items.Accessory.WoodNecklace>());

                    }
                }
            }
            if (context == "crate" && (arg == ItemID.FrozenCrate || arg == ItemID.FrozenCrateHard)) //Icicle Staff
            {
                if (Main.rand.Next(100) < 10)
                {
                    
                    player.QuickSpawnItem(ItemType<IceStaff>());

                }
            }
            if (context == "crate" && (arg == ItemID.JungleFishingCrate || arg == ItemID.JungleFishingCrateHard)) //Mossy repeater
            {
                if (Main.rand.Next(100) < 10)
                {

                    player.QuickSpawnItem(ItemType<MossRepeater>());

                }
            }
            if (context == "crate" && (arg == ItemID.OasisCrate || arg == ItemID.OasisCrateHard)) //Ancient Revolver
            {
                if (Main.rand.Next(100) < 10)
                {

                    player.QuickSpawnItem(ItemType<SandstoneGun>());
                    player.QuickSpawnItem(ItemID.MusketBall, Main.rand.Next(60, 100));
                }
            }
            if (context == "crate" && arg == ItemID.FrozenCrateHard) //Ice bars
            {
                if (Main.hardMode)
                {
                    if (Main.rand.Next(100) < 15)
                    {
                        player.QuickSpawnItem(ItemType<IceBar>(), Main.rand.Next(1, 10));
                    }
                }
            }

            if (context == "crate" && arg == ItemID.OasisCrateHard) //Forbidden Bars
            {
              
                if (Main.hardMode)
                {
                    if (Main.rand.Next(100) < 15)
                    {
                        player.QuickSpawnItem(ItemType<DesertBar>(), Main.rand.Next(1, 10));
                    }
                }
            }
           
        }


    }

}
   


