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

using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using StormDiversMod.Items.Ammo;
using StormDiversMod.Items.Accessory;
using StormDiversMod.Items.Tools;
using StormDiversMod.Items.Vanitysets;
using Terraria.ObjectData;

namespace StormDiversMod.Basefiles
{
    public class PostEvilDrop : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return NPC.downedBoss2;
            }
            return false;
        }
        public bool CanShowItemDropInUI()
        {
            return true;
        }
        public string GetConditionDescription()
        {
            return "Drops once the Evil boss has been defeated";
        }
    }
    public class PostSkeletronDrop : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return NPC.downedBoss3;
            }
            return false;
        }
        public bool CanShowItemDropInUI()
        {
            return true;
        }
        public string GetConditionDescription()
        {
            return "Drops once the Skeletron has been defeated";
        }
    }
    public class PostMechDrop : IItemDropRuleCondition
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
            return "Drops once any mechanical boss has been defeated";
        }
    }

    public class NPCDrops : GlobalNPC
    {
        
        public override bool InstancePerEntity => true;
        //Drops--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void OnKill(NPC npc) //Used for items that don't work with the new method
        {
            //Zone Drops----------------------------------------------------------------------------------------------------------------------
            if (!ModLoader.HasMod("TRAEProject"))
            {
                if ((Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUnderworldHeight) && (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                && (!npc.friendly && npc.lifeMax > 5 && npc.type != NPCID.TheHungry && npc.type != NPCID.TheHungryII)) //Hellsoul flames
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        if (Main.expertMode)
                        {
                            Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Materials.SoulFire>(), Main.rand.Next(1, 3));

                        }
                        else
                        {
                            Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Materials.SoulFire>());

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
                        if (Main.rand.Next(2) == 0)
                        {
                            if (Main.expertMode)
                            {
                                Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Materials.IceOre>(), Main.rand.Next(3, 5));
                            }
                            else
                            {
                                Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Materials.IceOre>(), Main.rand.Next(2, 5));

                            }
                        }

                    }
                }

                if (!Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneOverworldHeight && Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUndergroundDesert)
                {
                    if (!npc.friendly && npc.lifeMax > 5)

                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            if (Main.expertMode)
                            {
                                Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Materials.DesertOre>(), Main.rand.Next(3, 5));
                            }
                            else
                            {
                                Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Materials.DesertOre>(), Main.rand.Next(2, 5));

                            }
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
                        Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Pets.TwilightPetItem>());
                    }

                }
            }
            if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneSkyHeight && NPC.downedMoonlord) //Moonling Summoner
            {
                if (Main.rand.Next(100) == 0)
                {
                    Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Summons.MoonlingSummoner>());
                }
            }
            if (!Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneRockLayerHeight & !Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneNormalUnderground && Main.IsItStorming && StormWorld.stormBossDown == false) //StormBoss Summoner
            {
                if (Main.rand.Next(50) == 0)
                {
                    Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Summons.StormBossSummoner>());
                }
            }
            if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneUndergroundDesert && NPC.downedBoss3 && StormWorld.aridBossDown == false) //AridBoss Summoner
            {
                if (Main.rand.Next(50) == 0)
                {
                    Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Summons.AridBossSummon>());
                }
            }
            //No bestiary--------------------------------------------------------------------------------------------------------------------
            if (Main.rand.Next(5000) < 1 & npc.lifeMax > 5)
            {
                Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<ThePainMask>());
            }
            if (npc.boss)
            {
                if (Main.rand.Next(50) < 1)

                {
                    Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<TheClaymanMask>());
                }
            }         

                   
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            //drop rules
            LeadingConditionRule plantDead = new LeadingConditionRule(new Conditions.DownedPlantera());
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
            LeadingConditionRule isExpert = new LeadingConditionRule(new Conditions.IsExpert());

            IItemDropRule PostSkeletronDrop = new LeadingConditionRule(new PostSkeletronDrop());
            IItemDropRule PostEvilDrop = new LeadingConditionRule(new PostEvilDrop());
            IItemDropRule PostMechDrop = new LeadingConditionRule(new PostMechDrop());


            //Weapons-----------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (npc.type == NPCID.Duck || npc.type == NPCID.Duck2 || npc.type == NPCID.DuckWhite || npc.type == NPCID.DuckWhite2)
            {
                PostMechDrop.OnSuccess(ItemDropRule.Common(ModContent.ItemType<QuackStaff>(), 25));
            }
            if (npc.type == NPCID.Skeleton || npc.type == NPCID.MisassembledSkeleton || npc.type == NPCID.PantlessSkeleton || npc.type == NPCID.HeadacheSkeleton)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneBoomerang>(), 100));

            }
            if (npc.type == NPCID.UndeadMiner)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Tools.FastDrill>(), 20, 16));

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

            if (npc.type == NPCID.IceBat || npc.type == NPCID.SnowFlinx || npc.type == NPCID.SpikedIceSlime || npc.type == NPCID.UndeadViking)
            {
              
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceStaff>(), 50));

            }

            if (npc.type == NPCID.GrayGrunt || npc.type == NPCID.RayGunner || npc.type == NPCID.GigaZapper || npc.type == NPCID.MartianEngineer || npc.type == NPCID.MartianOfficer || npc.type == NPCID.BrainScrambler)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SuperDartLauncher>(), 150));

            }
            if (npc.type == NPCID.WallCreeper || npc.type == NPCID.WallCreeperWall)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WebStaff>(), 40));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WebWhip>(), 40));

            }
            if (npc.netID == NPCID.GreenSlime)
            {
                npcLoot.Add(ItemDropRule.OneFromOptions(40, ModContent.ItemType<WoodPointyStick>(), ModContent.ItemType<WoodCrossbow>(), ModContent.ItemType<WoodNecklace>()));
            }
            
            if (npc.type == NPCID.WyvernHead)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<WyvernBow>(), 5, 4));

            }

            if (npc.type == NPCID.GoblinShark || npc.type == NPCID.BloodEelHead)
            {

                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<BloodyRifle>(), 5, 4));
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<BloodySentry>(), 5, 4));

            }
            if (npc.type == NPCID.BloodNautilus)
            {
               

            }

            if (npc.type == NPCID.WalkingAntlion || npc.type == NPCID.FlyingAntlion || npc.type == NPCID.TombCrawlerHead || npc.type == NPCID.GiantWalkingAntlion || npc.type == NPCID.GiantFlyingAntlion) //needs to drop with ammo
            {
                IItemDropRule[] ancientGunAmmo = new IItemDropRule[] {
                ItemDropRule.Common(ModContent.ItemType<SandstoneGun>(), 1),
                ItemDropRule.Common(ItemID.MusketBall, 1, 60, 100),
                };
                npcLoot.Add(new FewFromRulesRule(2, 100, ancientGunAmmo));
            }


            if (npc.type == NPCID.SkeletonCommando || npc.type == NPCID.SkeletonSniper || npc.type == NPCID.TacticalSkeleton) //drop with ammo
            {
                ItemDropRule.Common(ModContent.ItemType<StickyLauncher>(), 20);
               

            }

            //Accessories--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (npc.type == NPCID.Demon)
            {
                PostSkeletronDrop.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HeartJar>(), 50));
                if (Main.rand.Next(50) == 0)
                {
                    Item.NewItem(new EntitySource_Loot(null), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<HeartJar>());
                }
            }
            
            if (npc.type == NPCID.Derpling)
            {

                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<DerpEye>(), 150, 100));

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

            if (npc.type == NPCID.GoblinSummoner)
            {
                notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Armour.ShadowFlameBMask>(), ModContent.ItemType<Items.Armour.ShadowFlameChestplate>(), ModContent.ItemType<Items.Armour.ShadowFlameGreaves>()));
                isExpert.OnSuccess(ItemDropRule.FewFromOptions(2, 1, ModContent.ItemType<Items.Armour.ShadowFlameBMask>(), ModContent.ItemType<Items.Armour.ShadowFlameChestplate>(), ModContent.ItemType<Items.Armour.ShadowFlameGreaves>()));
            }
           
            if (npc.type == NPCID.SolarSolenian)
            {
                npcLoot.Add(ItemDropRule.FewFromOptions(3, 50, ModContent.ItemType<Items.Vanitysets.SelenianBMask>(), ModContent.ItemType<Items.Vanitysets.SelenianBody>(), ModContent.ItemType<Items.Vanitysets.SelenianLegs>()));
            }
            if (npc.type == NPCID.VortexRifleman)
            {
                npcLoot.Add(ItemDropRule.FewFromOptions(3, 50, ModContent.ItemType<Items.Vanitysets.StormDiverBMask>(), ModContent.ItemType<Items.Vanitysets.StormDiverBody>(), ModContent.ItemType<Items.Vanitysets.StormDiverLegs>()));
            }
            if (npc.type == NPCID.NebulaSoldier)
            {
                npcLoot.Add(ItemDropRule.FewFromOptions(3, 50, ModContent.ItemType<Items.Vanitysets.PredictorBMask>(), ModContent.ItemType<Items.Vanitysets.PredictorBody>(), ModContent.ItemType<Items.Vanitysets.PredictorLegs>()));
            }
            if (npc.type == NPCID.StardustSoldier)
            {
                npcLoot.Add(ItemDropRule.FewFromOptions(3, 50, ModContent.ItemType<Items.Vanitysets.StargazerBMask>(), ModContent.ItemType<Items.Vanitysets.StargazerBody>(), ModContent.ItemType<Items.Vanitysets.StargazerLegs>()));
            }

            //Materials -----------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (npc.type == NPCID.ChaosElemental)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.ChaosShard>(), 4));

            }
            if (npc.type == NPCID.GraniteFlyer || npc.type == NPCID.GraniteGolem)
            {

                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Materials.GraniteCore>(), 2, 1));

            }
            if (npc.type == NPCID.GreekSkeleton)
            {

                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Materials.RedSilk>(), 2, 1));

            }
            if (npc.type == NPCID.ZombieEskimo || npc.type == NPCID.ArmedZombieEskimo)
            {

                isExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.BlueCloth>(), 1, 1, 2));
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.BlueCloth>(), 1, 1, 1));

            }

            if (npc.type == NPCID.Drippler || npc.type == NPCID.BloodZombie)
            {
                PostEvilDrop.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.BloodDrop>(), 2));
            }

            if (npc.type == NPCID.ZombieMerman || npc.type == NPCID.EyeballFlyingFish)
            {
                PostEvilDrop.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.BloodDrop>(), 1, 2, 4));
            }

            if (npc.type == NPCID.Derpling)
            {
                plantDead.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.DerplingShell>(), 1, 1, 2));
            }

            if (npc.type == NPCID.SantaNK1)
            {

                isExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SantankScrap>(), 1, 8, 12));
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SantankScrap>(), 1, 6, 10));

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
            //EoC
            if (npc.type == NPCID.EyeofCthulhu)
            {
                notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<EyeSword>(), ModContent.ItemType<EyeGun>(), ModContent.ItemType<EyeStaff>(), ModContent.ItemType<EyeMinion>()));
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tools.EyeHook>(), 4));

            }
            //Cultist boss----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (npc.type == NPCID.CultistBoss)
            {
               notExpert.OnSuccess(ItemDropRule.Common(33, ModContent.ItemType<CultistLazor>()));

                npcLoot.Add(ItemDropRule.BossBag(ItemID.CultistBossBag));
                //notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Weapons.CultistBow>(), ModContent.ItemType<Weapons.CultistSpear>(), ModContent.ItemType<Weapons.CultistTome>(), ModContent.ItemType<Weapons.CultistStaff>()));
                notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<CultistBow>(), ModContent.ItemType<CultistSpear>(), ModContent.ItemType<CultistTome>(), ModContent.ItemType<CultistStaff>()));

            }

            npcLoot.Add(plantDead);
            npcLoot.Add(notExpert);
            npcLoot.Add(isExpert);

            npcLoot.Add(PostSkeletronDrop);
            npcLoot.Add(PostEvilDrop);
            npcLoot.Add(PostMechDrop);

        }
    }
}
