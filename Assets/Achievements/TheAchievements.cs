using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using StormDiversMod.Items.Accessory;
using StormDiversMod.Items.Armour;
using StormDiversMod.Items.Materials;
using StormDiversMod.Items.Misc;
using StormDiversMod.Items.Pets;
using StormDiversMod.Items.Tools;
using StormDiversMod.Items.Vanitysets;
using StormDiversMod.Items.Weapons;
using StormDiversMod.NPCs;
using StormDiversMod.NPCs.Boss;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Achievements;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.Assets.Achievements
{
    public class AchievementAridBoss : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            // There are 4 AchievementCategory options: Slayer, Collector, Explorer, and Challenger.
            Achievement.SetCategory(AchievementCategory.Slayer);
            AddNPCKilledCondition(ModContent.NPCType<AridBoss>());
        }
        public override Position GetDefaultPosition() => new After("BONED");
    }
    public class AchievementStormBoss : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Slayer);
            AddNPCKilledCondition(ModContent.NPCType<StormBoss>());
        }
        public override Position GetDefaultPosition() => new After("GET_A_LIFE");
    }
    public class AchievementUltimateBoss : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Slayer);
            AddNPCKilledCondition(ModContent.NPCType<TheUltimateBoss>());
        }
        public override Position GetDefaultPosition() => new After("CHAMPION_OF_TERRARIA");
    }
    public class AchievementNoPizza : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Slayer);
            AddNPCKilledCondition(ModContent.NPCType<SnowmanPizza>());
        }
        public override Position GetDefaultPosition() => new Before("DO_YOU_WANT_TO_SLAY_A_SNOWMAN");
    }
    public class AchievementMoonling : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Slayer);
            AddNPCKilledCondition(ModContent.NPCType<MoonDerp>());
        }
        public override Position GetDefaultPosition() => new After("CHAMPION_OF_TERRARIA");
    }
    public class AchievementScaryDerp : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Slayer);
            AddNPCKilledCondition(ModContent.NPCType<DerpMimic>());
        }
    }
    public class AchievementBiomeCore : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            var CraftCondition = AddItemCraftCondition([ModContent.ItemType<Items.Accessory.BiomeCore>()]);
        }
        public override Position GetDefaultPosition() => new After("ITS_HARD");
    }
    public class AchievementFastDrill : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            var CraftCondition = AddItemCraftCondition([ModContent.ItemType<Items.Tools.FastDrill2>()]);
        }
        public override Position GetDefaultPosition() => new After("MINER_FOR_FIRE");
    }
    public class AchievementEquinox : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            var CraftCondition = AddItemCraftCondition([ModContent.ItemType<Items.Weapons.LightDarkSword>()]);
        }
        public override Position GetDefaultPosition() => new After("BEGONE_EVIL");
    }
    public class AchievementStoneCannon4 : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            var CraftCondition = AddItemCraftCondition([ModContent.ItemType<Items.Weapons.StoneThrowerSuperLunar>()]);
        }
        public override Position GetDefaultPosition() => new After("CHAMPION_OF_TERRARIA");
    }
    public class AchievementPendants : ModAchievement
    {
        public CustomFlagCondition AchPendantsCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchPendantsCondition = AddCondition();
        }
        public override Position GetDefaultPosition() => new After("I_AM_LOOT");
    }
    public class AchievementThePain : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            var PickupCondition = AddItemPickupCondition([ModContent.ItemType<ThePainMask>()]);
        }
        
    }
    public class AchievementClayman : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            var PickupCondition = AddItemPickupCondition([ModContent.ItemType<TheClaymanMask>()]);
        }
        //public override Position GetDefaultPosition() => new After(ModContent.GetInstance<AchievementThePain>());
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(ModContent.GetInstance<AchievementThePain>());
        }
    }
    public class AchievementDerpKing : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            //all pickup to make it easier
            Achievement.SetCategory(AchievementCategory.Collector);
            var CraftHelmet = AddItemPickupCondition([ModContent.ItemType<DerplingBHeadgear>(), ModContent.ItemType<DerplingBMask>()]);//Regular or ancient
            var CraftChestplate = AddItemPickupCondition([ModContent.ItemType<DerplingBreastplate>()]);
            var CraftLeggings = AddItemPickupCondition([ModContent.ItemType<DerplingGreaves>()]);
            var CraftSword = AddItemPickupCondition([ModContent.ItemType<DerplingSword>()]);
            var CraftGun = AddItemPickupCondition([ModContent.ItemType<DerplingGun>()]);
            var CraftMagic = AddItemPickupCondition([ModContent.ItemType<DerplingStaff>()]);
            var CraftMinion = AddItemPickupCondition([ModContent.ItemType<DerplingMinion>()]);
            var CraftDrill = AddItemPickupCondition([ModContent.ItemType<DerplingDrill>()]);
            var CraftChainsaw = AddItemPickupCondition([ModContent.ItemType<DerplingChainsaw>()]);
            var CraftHammer = AddItemPickupCondition([ModContent.ItemType<DerplingJackhammer>()]);
            var CraftHook = AddItemPickupCondition([ModContent.ItemType<DerpHook>()]);
            var CraftFishrod = AddItemPickupCondition([ModContent.ItemType<FishingRodDerpling>()]);
            var PickupEye = AddItemPickupCondition([ModContent.ItemType<DerpEye>()]);
        }
        public override Position GetDefaultPosition() => new After("THE_GREAT_SOUTHERN_PLANTKILL");
    }
    public class AchievementLunarCosplay : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            var PickupSolarHead = AddItemPickupCondition([ModContent.ItemType<SelenianBMask>()]);
            var PickupSolarBody = AddItemPickupCondition([ModContent.ItemType<SelenianBody>()]);
            var PickupSolarLegs = AddItemPickupCondition([ModContent.ItemType<SelenianLegs>()]);
            var PickupStormHead = AddItemPickupCondition([ModContent.ItemType<StormDiverBMask>()]);
            var PickupStormBody = AddItemPickupCondition([ModContent.ItemType<StormDiverBody>()]);
            var PickupStormLegs = AddItemPickupCondition([ModContent.ItemType<StormDiverLegs>()]);
            var PickupNebulaHead = AddItemPickupCondition([ModContent.ItemType<PredictorBMask>()]);
            var PickupNebulaBody = AddItemPickupCondition([ModContent.ItemType<PredictorBody>()]);
            var PickupNebulaLegs = AddItemPickupCondition([ModContent.ItemType<PredictorLegs>()]);
            var PickupStarHead = AddItemPickupCondition([ModContent.ItemType<StargazerBMask>()]);
            var PickupStarBody = AddItemPickupCondition([ModContent.ItemType<StargazerBody>()]);
            var PickupStarLegs = AddItemPickupCondition([ModContent.ItemType<StargazerLegs>()]);
        }
        public override Position GetDefaultPosition() => new After("STAR_DESTROYER");
    }
    public class AchievementGnomed : ModAchievement
    {
        public CustomFlagCondition AchGnomeCondition { get; private set; }

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchGnomeCondition = AddCondition();
        }
    }
    public class AchievementStompBounce : ModAchievement
    {
        public CustomFlagCondition AchStompCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchStompCondition = AddCondition();
        }
        public override Position GetDefaultPosition() => new After("I_AM_LOOT");

    }
    public class AchievementQuack : ModAchievement
    {
        //public CustomFlagCondition 
        public CustomIntCondition AchQuackCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchQuackCondition = AddIntCondition(5);
        }
    }
    public class AchievementHugBear : ModAchievement
    {
        public CustomFlagCondition AchBearCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchBearCondition = AddCondition();
        }
    }
    public class AchievementHeartSteal : ModAchievement
    {
        public CustomFlagCondition AchHeartStealCondition { get; private set; }

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchHeartStealCondition = AddCondition();
        }
        public override Position GetDefaultPosition() => new After("ITS_GETTING_HOT_IN_HERE");
    }
    public class AchievementTwilight : ModAchievement
    {
        public CustomFlagCondition AchTwilightCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchTwilightCondition = AddCondition();
        }
        public override Position GetDefaultPosition() => new After("EXTRA_SHINY");

    }
    public class AchievementSuperHeart : ModAchievement
    {
        public CustomFlagCondition AchSuperHeartCondition { get; private set; }

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchSuperHeartCondition = AddCondition();
        }
        public override Position GetDefaultPosition() => new After("GET_A_LIFE");
    }
    public class AchievementNineLives : ModAchievement
    {
        public CustomFlagCondition AchNineLivesCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchNineLivesCondition = AddCondition();
        }
    }
    public class AchievementSantanked : ModAchievement
    {
        public CustomFlagCondition AchSantankCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchSantankCondition = AddCondition();
        }
        public override Position GetDefaultPosition() => new After("ICE_SCREAM");
    }
    public class AchievementNoShield : ModAchievement
    {
        public CustomFlagCondition AchShieldCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchShieldCondition = AddCondition();
        }
        public override Position GetDefaultPosition() => new After("STAR_DESTROYER");
    }
    public class AchievementThePets : ModAchievement
    {
        public CustomFlagCondition AchPetCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchPetCondition = AddCondition();
        }
    }
    public class AchievementWhack : ModAchievement
    {
        public CustomIntCondition AchWhackCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchWhackCondition = AddIntCondition(100);
        }
    }

    public class AchievementGlassShatter : ModAchievement
    {
        public CustomFlagCondition AchGlassCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchGlassCondition = AddCondition();
        }
        public override Position GetDefaultPosition() => new After("NO_HOBO");

    }
    /*public class AchievementTemple : ModAchievement //Eh, you'd have to create a new world AND character if you missed this one
    {
        public CustomFlagCondition AchTempleCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            AchTempleCondition = AddCondition();
        }
    }*/
}