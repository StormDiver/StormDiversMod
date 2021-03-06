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
using Terraria.GameContent.ItemDropRules;

namespace StormDiversMod.Basefiles
{
    /*public class PostMechItemDrop : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return NPC.downedMechBossAny;
            }
            return false;
        }
        public bool CanShowItemDropInUI()
        {
            return true;
        }
        public string GetConditionDescription()
        {
            return "Obtained once any mechanical boss has been defeated";
        }
    }*/
    public class TreasureBagsandCrates : GlobalItem
    {

        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            IItemDropRule MechItemDrop = new LeadingConditionRule(new PostMechDrop());

            if (item.type == ItemID.EyeOfCthulhuBossBag)
            {
                itemLoot.Add(ItemDropRule.OneFromOptions(1, ItemType<EyeSword>(), ItemType<EyeGun>(), ItemType<EyeStaff>(), ItemType<EyeMinion>()));

                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Tools.EyeHook>(), 4));

            }
            if (item.type == ItemID.SkeletronPrimeBossBag || item.type == ItemID.TwinsBossBag || item.type == ItemID.DestroyerBossBag)
            {
                MechItemDrop.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Accessory.PrimeAccess>(), 1));
            }
            if (item.type == ItemID.BossBagBetsy)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessory.FlameCore>(), 1));

            }
            if (item.type == ItemID.CultistBossBag)
            {
                itemLoot.Add(ItemDropRule.OneFromOptions(1, ItemType<CultistSpear>(), ItemType<CultistBow>(), ItemType<CultistTome>(), ItemType<CultistStaff>()));

                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CultistLazor>(), 20));
            }

            if (item.type == ItemID.LockBox)
            {
                IItemDropRule[] protoLauncherAmmo = new IItemDropRule[] {
                ItemDropRule.Common(ModContent.ItemType<ProtoLauncher>(), 1),
                ItemDropRule.Common(ModContent.ItemType<Items.Ammo.ProtoGrenade>(), 1, 60, 100),
                };
                itemLoot.Add(new FewFromRulesRule(2, 5, protoLauncherAmmo));

                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TwilightPetItem>(), 5));
            }
            if (item.type == ItemID.WoodenCrate || item.type == ItemID.WoodenCrateHard)
            {
                itemLoot.Add(ItemDropRule.OneFromOptions(2, ItemType<WoodCrossbow>(), ItemType<WoodPointyStick>(), ItemType<Items.Accessory.WoodNecklace>()));

            }

            if (item.type == ItemID.FrozenCrate || item.type == ItemID.FrozenCrateHard)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceStaff>(), 10));

            }
            if (item.type == ItemID.JungleFishingCrate || item.type == ItemID.JungleFishingCrateHard)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MossRepeater>(), 10));

            }
            if (item.type == ItemID.OasisCrate || item.type == ItemID.OasisCrateHard)
            {
                IItemDropRule[] ancientGunAmmo = new IItemDropRule[] {
                ItemDropRule.Common(ModContent.ItemType<SandstoneGun>(), 1),
                ItemDropRule.Common(ItemID.MusketBall, 1, 60, 100),             
                };
                itemLoot.Add(new FewFromRulesRule(2, 10, ancientGunAmmo));           
            }
            if (item.type == ItemID.FrozenCrateHard)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.OresandBars.IceBar>(), 6, 1, 10));

            }
            if (item.type == ItemID.OasisCrateHard)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.OresandBars.DesertBar>(), 6, 1, 10));

            }


            itemLoot.Add(MechItemDrop);

        }
        /*public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag" && arg == ItemID.BossBagBetsy) //besty Treasure bag
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<Items.Accessory.FlameCore>(), Main.rand.Next(1, 1));
            
            }
          
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) //Any mech boss treasure bag post 3 mechs
            {
                if (context == "bossBag" && arg == ItemID.SkeletronPrimeBossBag || arg == ItemID.TwinsBossBag || arg == ItemID.DestroyerBossBag)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<Items.Accessory.PrimeAccess>(), Main.rand.Next(1, 1));
                }
            }
            if (context == "bossBag" && arg == ItemID.EyeOfCthulhuBossBag) //EoC treasure bag
            {
                int choice = Main.rand.Next(4);

                if (choice == 0)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<EyeSword>(), Main.rand.Next(1, 1));
                }
                if (choice == 1)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<EyeGun>(), Main.rand.Next(1, 1));
                }
                if (choice == 2)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<EyeStaff>(), Main.rand.Next(1, 1));
                }
                if (choice == 3)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<EyeMinion>(), Main.rand.Next(1, 1));

                }
                if (Main.rand.NextBool(4))
                {

                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<Items.Tools.EyeHook>(), Main.rand.Next(1, 1));


                }
            }

            if (context == "bossBag" && arg == ItemID.CultistBossBag) //Cultist treasure bag
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<Items.Accessory.LunaticHood>(), Main.rand.Next(1, 1));

                if (Main.rand.Next(100) < 5)

                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<CultistLazor>(), Main.rand.Next(1, 1));


                }
                int choice = Main.rand.Next(4);

                if (choice == 0)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<CultistBow>(), Main.rand.Next(1, 1));
                }
                if (choice == 1)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<CultistSpear>(), Main.rand.Next(1, 1));
                }
                if (choice == 2)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<CultistTome>(), Main.rand.Next(1, 1));
                }
                if (choice == 3)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<CultistStaff>(), Main.rand.Next(1, 1));

                }
            }
          
            if (context == "lockBox") //proto launcher
            {
                if (Main.rand.Next(100) < 20)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<ProtoLauncher>(), Main.rand.Next(1, 1));
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<Items.Ammo.ProtoGrenade>(), Main.rand.Next(52, 80));
                }
                if (Main.rand.Next(100) < 20)
                {
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<TwilightPetItem>(), Main.rand.Next(1, 1));
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
                        player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<WoodPointyStick>());

                    }
                    else if (choice == 1)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<WoodCrossbow>());

                    }
                    else
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<Items.Accessory.WoodNecklace>());

                    }
                }
            }
            if (context == "crate" && (arg == ItemID.FrozenCrate || arg == ItemID.FrozenCrateHard)) //Icicle Staff
            {
                if (Main.rand.Next(100) < 10)
                {
                    
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<IceStaff>());

                }
            }
            if (context == "crate" && (arg == ItemID.JungleFishingCrate || arg == ItemID.JungleFishingCrateHard)) //Mossy repeater
            {
                if (Main.rand.Next(100) < 10)
                {

                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<MossRepeater>());

                }
            }
            if (context == "crate" && (arg == ItemID.OasisCrate || arg == ItemID.OasisCrateHard)) //Ancient Revolver
            {
                if (Main.rand.Next(100) < 10)
                {

                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<SandstoneGun>());
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemID.MusketBall, Main.rand.Next(60, 100));
                }
            }
            if (context == "crate" && arg == ItemID.FrozenCrateHard) //Ice bars
            {
                if (Main.hardMode)
                {
                    if (Main.rand.Next(100) < 15)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<IceBar>(), Main.rand.Next(1, 10));
                    }
                }
            }

            if (context == "crate" && arg == ItemID.OasisCrateHard) //Forbidden Bars
            {
              
                if (Main.hardMode)
                {
                    if (Main.rand.Next(100) < 15)
                    {
                        player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<DesertBar>(), Main.rand.Next(1, 10));
                    }
                }
            }
           
        }*/


    }

}
   


