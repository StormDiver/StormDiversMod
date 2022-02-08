using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using StormDiversMod.Items.Weapons;
using StormDiversMod.Dusts;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using StormDiversMod.Items.Ammo;
using StormDiversMod.Items.Accessory;
using StormDiversMod.Items.Tools;
using StormDiversMod.Items.Vanitysets;

namespace StormDiversMod.Basefiles
{

    public class NPCDrops : GlobalNPC
    {
        
        public override bool InstancePerEntity => true;
        //Drops--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void OnKill(NPC npc) //Used for items that don't work with the new method
        {
            //Zone Drops----------------------------------------------------------------------------------------------------------------------
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) //HellSoul Flame
            {
                if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUnderworldHeight)
                {


                    if (!npc.friendly && npc.lifeMax > 5 && npc.type != NPCID.TheHungry && npc.type != NPCID.TheHungryII)

                    {
                        if (Main.rand.Next(1) == 0)
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Materials.SoulFire>());
                        }
                    }
                }

            }
            if (Main.hardMode) //Frost and Arid Shard
            {

                if (!Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneOverworldHeight && Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneSnow)
                {
                    if (!npc.friendly && npc.lifeMax > 5)

                    {
                        if (Main.rand.Next(1) == 0)
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Materials.IceOre>(), Main.rand.Next(3, 5));
                        }

                    }
                }

                if (!Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneOverworldHeight && Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUndergroundDesert)
                {
                    if (!npc.friendly && npc.lifeMax > 5)

                    {
                        if (Main.rand.Next(1) == 0)
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Materials.DesertOre>(), Main.rand.Next(3, 5));
                        }
                    }
                }

            }
            if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneDungeon) //Twilight pet
            {

                if (!npc.friendly && npc.lifeMax > 5 && npc.type != NPCID.TheHungry && npc.type != NPCID.TheHungryII)

                {
                    if (Main.rand.Next(150) == 0)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Pets.TwilightPetItem>());
                    }

                }
            }
            if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneSkyHeight && NPC.downedMoonlord) //Moonling Summoner
            {
                if (Main.rand.Next(100) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Tools.MoonlingSummoner>());
                }
            }
            /*if (npc.type == NPCID.CultistBoss) //Cultist Treasurebag, new method works now
            {
                if (Main.expertMode) 
                {

                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CultistBossBag);

                }
            }*/
            //Items with drop requirements-------------------------------------------------------------------------------
            if ((NPC.downedMechBossAny && !NPC.downedMoonlord) && (npc.type == NPCID.Duck || npc.type == NPCID.Duck2 || npc.type == NPCID.DuckWhite || npc.type == NPCID.DuckWhite2))
            {

                if (Main.rand.Next(25) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<QuackStaff>());
                }


            }

            if (npc.type == NPCID.SkeletonCommando || npc.type == NPCID.SkeletonSniper || npc.type == NPCID.TacticalSkeleton)
            {
                if (Main.rand.Next(100) < 2)

                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.StickyLauncher>());
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Ammo.StickyBomb>(), Main.rand.Next(50, 101));

                }
            }
            if (NPC.downedBoss3 && npc.type == NPCID.Demon)
            {
                if (Main.rand.Next(50) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<HeartJar>());
                }
            }

            if ((NPC.downedBoss1) && (npc.type == NPCID.Drippler || npc.type == NPCID.BloodZombie))
            {
                if (Main.rand.Next(2) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Materials.BloodDrop>());

                }
            }
            if (Main.rand.Next(5000) < 1)

            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ThePainMask>());
            }
            if (npc.type == NPCID.VortexRifleman)
            {
                if (Main.rand.Next(100) < 2)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.StormDiverBMask>());
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.StormDiverBody>());
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.StormDiverLegs>());
                }
            }
            if (npc.type == NPCID.SolarSolenian)
            {
                if (Main.rand.Next(100) < 2)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.SelenianBMask>());
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.SelenianBody>());
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.SelenianLegs>());
                }
            }
            if (npc.type == NPCID.NebulaSoldier)
            {
                if (Main.rand.Next(100) < 2)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.PredictorBMask>());
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.PredictorBody>());
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.PredictorLegs>());
                }
            }
            if (npc.type == NPCID.StardustSoldier)
            {
                if (Main.rand.Next(100) < 2)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.StargazerBMask>());
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.StargazerBody>());
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Vanitysets.StargazerLegs>());
                }
            }

        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            LeadingConditionRule plantDead = new LeadingConditionRule(new Conditions.DownedPlantera());
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
            LeadingConditionRule isExpert = new LeadingConditionRule(new Conditions.IsExpert());

            //Weapons-----------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (npc.type == NPCID.Skeleton || npc.type == NPCID.MisassembledSkeleton || npc.type == NPCID.PantlessSkeleton || npc.type == NPCID.HeadacheSkeleton)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneBoomerang>(), 100));

            }
            
            if (npc.type == NPCID.SolarSolenian)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunarSelenianBlade>(), 50));

            }
            if (npc.type == NPCID.VortexRifleman)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunarVortexShotgun>(), 50));

            }
            if (npc.type == NPCID.NebulaSoldier)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunarPredictorBrain>(), 50));

            }
            if (npc.type == NPCID.StardustSoldier)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunarStargazerLaser>(), 50));

            }
            if (npc.type == NPCID.EnchantedSword)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EnchantedSword>(), 25));

            }
            if (npc.type == NPCID.CrimsonAxe)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EnchantedAxeMagic>(), 25));

            }
            if (npc.type == NPCID.CursedHammer)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EnchantedHammer>(), 25));

            }
            if (npc.type == NPCID.IceQueen)
            {

                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<IceSentry>(), 10, 15));

            }
            if (npc.type == NPCID.Hornet || npc.type == NPCID.HornetFatty || npc.type == NPCID.HornetHoney || npc.type == NPCID.HornetLeafy || npc.type == NPCID.HornetSpikey || npc.type == NPCID.HornetStingy)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MossRepeater>(), 50));

            }
        

          
            if (npc.type == NPCID.GrayGrunt || npc.type == NPCID.RayGunner || npc.type == NPCID.GigaZapper || npc.type == NPCID.MartianEngineer || npc.type == NPCID.MartianOfficer || npc.type == NPCID.BrainScrambler)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SuperDartLauncher>(), 150));

            }
            if (npc.type == NPCID.WallCreeper || npc.type == NPCID.WallCreeperWall)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WebStaff>(), 50));

            }
            //Accessories--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (npc.type == NPCID.Derpling)
            {

                plantDead.OnSuccess(ItemDropRule.NormalvsExpert(ModContent.ItemType<DerpEye>(), 100, 50));

            }
            if (npc.type == NPCID.CursedSkull)
            {

                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<EyeofDungeon>(), 30, 25));

            }

            if (npc.type == NPCID.IceQueen)
            {

                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<FrostCube>(), 15, 10));

            }

            if (npc.type == NPCID.Pumpking)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<SpookyCore>(), 15, 10));
            }
            //Armour/Vanity----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (npc.type == NPCID.ZombieRaincoat)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armour.RainBoots>(), 20));

            }
          //Rest in old code because :shrug:


            //Materials -----------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (npc.type == NPCID.ChaosElemental)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.ChaosShard>(), 4));

            }
            if (npc.type == NPCID.GraniteFlyer || npc.type == NPCID.GraniteGolem)
            {

                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Materials.GraniteCore>(), 3, 2));

            }
            if (npc.type == NPCID.GreekSkeleton)
            {

                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Materials.RedSilk>(), 3, 2));

            }
            if (npc.type == NPCID.ZombieEskimo || npc.type == NPCID.ArmedZombieEskimo)
            {

                isExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.BlueCloth>(), 1, 1, 2));
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.BlueCloth>(), 1, 1, 1));

            }

            if (npc.type == NPCID.Derpling)
            {

                plantDead.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.DerplingShell>(), 1, 1, 2));

            }

            if (npc.type == NPCID.SantaNK1)
            {

                isExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SantankScrap>(), 1, 5, 8));
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SantankScrap>(), 1, 4, 6));



            }
            //Pets-----------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (npc.type == NPCID.VortexRifleman)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Pets.StormLightItem>(), 50));

            }

            //Misc----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            if (npc.type == NPCID.Duck || npc.type == NPCID.Duck2 || npc.type == NPCID.DuckWhite || npc.type == NPCID.DuckWhite2)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Tools.Quack>(), 25));

            }
            //Cultist boss----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (npc.type == NPCID.CultistBoss)
            {
               notExpert.OnSuccess(ItemDropRule.NormalvsExpert(ModContent.ItemType<CultistLazor>(), 50, 33));



                npcLoot.Add(ItemDropRule.BossBag(ItemID.CultistBossBag));
                //notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Weapons.CultistBow>(), ModContent.ItemType<Weapons.CultistSpear>(), ModContent.ItemType<Weapons.CultistTome>(), ModContent.ItemType<Weapons.CultistStaff>()));
                notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<CultistBow>(), ModContent.ItemType<CultistSpear>(), ModContent.ItemType<CultistTome>(), ModContent.ItemType<CultistStaff>()));

                /*int choice = Main.rand.Next(4);

                if (choice == 0)
                {

                }
                if (choice == 1)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.CultistSpear>(), 1));

                }
                if (choice == 2)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.CultistTome>(), 1));

                }
                if (choice == 3)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.CultistStaff>(), 1));

                }*/
            }

            npcLoot.Add(plantDead);
            npcLoot.Add(notExpert);
            npcLoot.Add(isExpert);

        }
    }
}
