using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using StormDiversMod.Items.Materials;
using StormDiversMod.Items.OresandBars;
using StormDiversMod.Items.Armour;
using Terraria.DataStructures;
using StormDiversMod.Items.Ammo;
using StormDiversMod.Items.Accessory;
using StormDiversMod.Items.Tools;
using StormDiversMod.Items.Vanitysets;
using StormDiversMod.Items.Weapons;

using static Terraria.ModLoader.ModContent;
using Terraria.Localization;


namespace StormDiversMod.Basefiles
{
    public class RecipeGroups : ModSystem
    {
        public override void AddRecipeGroups() //Recipe Groups
        {
            //recipe.AddRecipeGroup("StormDiversMod:EvilBars", 10);


            RecipeGroup group = new RecipeGroup(() => "Demonite or Crimtane Bar", new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar
            });
            RecipeGroup.RegisterGroup("StormDiversMod:EvilBars", group);

            group = new RecipeGroup(() => "Silver or Tungsten Bar", new int[]
           {
                ItemID.SilverBar,
                ItemID.TungstenBar
           });
            RecipeGroup.RegisterGroup("StormDiversMod:SilverBars", group);

            group = new RecipeGroup(() => "Gold or Platinum Bar", new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            });
            RecipeGroup.RegisterGroup("StormDiversMod:GoldBars", group);

            group = new RecipeGroup(() => "Gold or Platinum Ore", new int[]
            {
                ItemID.GoldOre,
                ItemID.PlatinumOre
            });
            RecipeGroup.RegisterGroup("StormDiversMod:GoldOres", group);

            group = new RecipeGroup(() => "Shadow Scale or Tissue Sample", new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample
            });
            RecipeGroup.RegisterGroup("StormDiversMod:EvilMaterial", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Base Running Boots", new int[]
            {
                ItemID.HermesBoots,
                ItemID.FlurryBoots,
                ItemID.SailfishBoots,
                ItemID.SandBoots
            });
            RecipeGroup.RegisterGroup("StormDiversMod:RunBoots", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Prehardmode Anvil", new int[]
            {
                ItemID.IronAnvil,
                ItemID.LeadAnvil,

            });
            RecipeGroup.RegisterGroup("StormDiversMod:Anvils", group);

            group = new RecipeGroup(() => "Cobalt or Palladium Bar", new int[]
          {
                ItemID.CobaltBar,
                ItemID.PalladiumBar,

          });
            RecipeGroup.RegisterGroup("StormDiversMod:LowHMBars", group);

            group = new RecipeGroup(() => "Mythril or Orichalcum Bar", new int[]
           {
                ItemID.MythrilBar,
                ItemID.OrichalcumBar,

           });
            RecipeGroup.RegisterGroup("StormDiversMod:MidHMBars", group);

            group = new RecipeGroup(() => "Adamantite or Titanium Bar", new int[]
           {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar,

           });
            RecipeGroup.RegisterGroup("StormDiversMod:HighHMBars", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Tombstone", new int[]
          {
                ItemID.Tombstone,
                ItemID.GraveMarker,
                ItemID.CrossGraveMarker,
                ItemID.Headstone,
                ItemID.Gravestone,
                ItemID.Obelisk,
                ItemID.RichGravestone1,
                ItemID.RichGravestone2,
                ItemID.RichGravestone3,
                ItemID.RichGravestone4,
                ItemID.RichGravestone5,
          });
            RecipeGroup.RegisterGroup("StormDiversMod:Tombstones", group);


            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Mech Soul", new int[]
           {
                ItemID.SoulofMight,
                ItemID.SoulofSight,
                ItemID.SoulofFright,
           });
            RecipeGroup.RegisterGroup("StormDiversMod:MechSoul", group);
        }
    }
    public class NewRecipes : GlobalItem
    {
        public override void AddRecipes()
        {
            //Mining armour
            /*Recipe recipe = Recipe.Create(ItemID.MiningShirt, 1);
            recipe.AddIngredient(ItemID.Silk, 25);
            recipe.AddRecipeGroup("StormDiversMod:GoldOres", 35);
            recipe.AddTile(TileID.Loom);
            recipe.Register();

            recipe = Recipe.Create(ItemID.MiningPants, 1);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddRecipeGroup("StormDiversMod:GoldOres", 30);
            recipe.AddTile(TileID.Loom);
            recipe.Register();*/

            //Gladiator gear
            Recipe recipe2 = Recipe.Create(ItemID.GladiatorHelmet, 1);
            recipe2.AddRecipeGroup("StormDiversMod:GoldBars", 12);
            recipe2.AddIngredient(ModContent.ItemType<RedSilk>(), 2);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();

            recipe2 = Recipe.Create(ItemID.GladiatorBreastplate, 1);
            recipe2.AddRecipeGroup("StormDiversMod:GoldBars", 18);
            recipe2.AddIngredient(ModContent.ItemType<RedSilk>(), 3);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();

            recipe2 = Recipe.Create(ItemID.GladiatorLeggings, 1);
            recipe2.AddRecipeGroup("StormDiversMod:GoldBars", 15);
            recipe2.AddIngredient(ModContent.ItemType<RedSilk>(), 2);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();

            recipe2 = Recipe.Create(ItemID.Gladius, 1);
            recipe2.AddRecipeGroup("StormDiversMod:GoldBars", 10);
            recipe2.AddIngredient(ModContent.ItemType<RedSilk>(), 3);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();

            //Snow armour
            Recipe recipe3 = Recipe.Create(ItemID.EskimoHood, 1);
            recipe3.AddIngredient(ModContent.ItemType<BlueCloth>(), 10);
            recipe3.AddIngredient(ItemID.Silk, 10);
            recipe3.AddTile(TileID.Loom);
            recipe3.Register();

            recipe3 = Recipe.Create(ItemID.EskimoCoat, 1);
            recipe3.AddIngredient(ModContent.ItemType<BlueCloth>(), 20);
            recipe3.AddIngredient(ItemID.Silk, 20);
            recipe3.AddTile(TileID.Loom);
            recipe3.Register();


            recipe3 = Recipe.Create(ItemID.EskimoPants, 1);
            recipe3.AddIngredient(ModContent.ItemType<BlueCloth>(), 15);
            recipe3.AddIngredient(ItemID.Silk, 15);
            recipe3.AddTile(TileID.Loom);
            recipe3.Register();

            //ROD
            Recipe recipe4 = Recipe.Create(ItemID.RodofDiscord, 1);
            recipe4.AddIngredient(ModContent.ItemType<ChaosShard>(), 25);
            recipe4.AddIngredient(ItemID.CrystalShard, 30);
            recipe4.AddIngredient(ItemID.SoulofLight, 25);
            recipe4.AddIngredient(ItemID.HallowedBar, 20);
            recipe4.AddTile(TileID.MythrilAnvil);
            recipe4.Register();

            //Sime staff
            Recipe recipe5 = Recipe.Create(ItemID.SlimeStaff, 1);
            recipe5.AddRecipeGroup(RecipeGroupID.Wood, 50);
            recipe5.AddIngredient(ItemID.Gel, 100);
            recipe5.AddTile(TileID.WorkBenches);
            recipe5.Register();

            //Tombstone
            Recipe recipe6 = Recipe.Create(ItemID.Tombstone, 1);
            recipe6.AddIngredient(ItemID.StoneBlock, 50);
            recipe6.AddTile(TileID.HeavyWorkBench);
            recipe6.Register();

            //Frost armour
            Recipe recipe7 = Recipe.Create(ItemID.FrostHelmet, 1);
            recipe7.AddIngredient(ModContent.ItemType<IceBar>(), 10);
            recipe7.AddIngredient(ItemID.FrostCore);
            recipe7.AddTile(TileID.MythrilAnvil);
            recipe7.Register();

            recipe7 = Recipe.Create(ItemID.FrostBreastplate, 1);
            recipe7.AddIngredient(ModContent.ItemType<IceBar>(), 18);
            recipe7.AddIngredient(ItemID.FrostCore);
            recipe7.AddTile(TileID.MythrilAnvil);
            recipe7.Register();

            recipe7 = Recipe.Create(ItemID.FrostLeggings, 1);
            recipe7.AddIngredient(ModContent.ItemType<IceBar>(), 14);
            recipe7.AddIngredient(ItemID.FrostCore);
            recipe7.AddTile(TileID.MythrilAnvil);
            recipe7.Register();

            //Forbidden armour
            Recipe recipe8 = Recipe.Create(ItemID.AncientBattleArmorHat, 1);
            recipe8.AddIngredient(ModContent.ItemType<DesertBar>(), 10);
            recipe8.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe8.AddTile(TileID.MythrilAnvil);
            recipe8.Register();

            recipe8 = Recipe.Create(ItemID.AncientBattleArmorShirt, 1);
            recipe8.AddIngredient(ModContent.ItemType<DesertBar>(), 18);
            recipe8.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe8.AddTile(TileID.MythrilAnvil);
            recipe8.Register();

            recipe8 = Recipe.Create(ItemID.AncientBattleArmorPants, 1);
            recipe8.AddIngredient(ModContent.ItemType<DesertBar>(), 14);
            recipe8.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe8.AddTile(TileID.MythrilAnvil);
            recipe8.Register();

            //Santank weapons
            Recipe recipe9 = Recipe.Create(ItemID.ChainGun, 1);
            recipe9.AddIngredient(ModContent.ItemType<SantankScrap>(), 18);
            recipe9.AddTile(TileID.MythrilAnvil);
            recipe9.Register();

            recipe9 = Recipe.Create(ItemID.ElfMelter, 1);
            recipe9.AddIngredient(ModContent.ItemType<SantankScrap>(), 18);
            recipe9.AddTile(TileID.MythrilAnvil);
            recipe9.Register();

            //Blood tear and rod
            Recipe recipe10 = Recipe.Create(ItemID.BloodFishingRod, 1);
            recipe10.AddRecipeGroup("StormDiversMod:EvilBars", 12);
            recipe10.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 8);
            recipe10.AddTile(TileID.Anvils);
            recipe10.Register();

            recipe10 = Recipe.Create(ItemID.BloodMoonStarter, 1);
            recipe10.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 8);
            recipe10.AddTile(TileID.Anvils);
            recipe10.Register();

            //hardmode Blood moon items
            /*Recipe recipe11 = Recipe.Create(ItemID.DripplerFlail, 1);
            recipe11.AddIngredient(ItemID.HallowedBar, 10);
            recipe11.AddIngredient(ItemID.SoulofMight, 10);
            recipe11.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 8);
            recipe11.AddTile(TileID.MythrilAnvil);
            recipe11.Register();

            recipe11 = Recipe.Create(ModContent.ItemType<Items.Weapons.BloodyRifle>(), 1);
            recipe11.AddIngredient(ItemID.HallowedBar, 10);
            recipe11.AddIngredient(ItemID.SoulofSight, 10);
            recipe11.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 8);
            recipe11.AddTile(TileID.MythrilAnvil);
            recipe11.Register();

            recipe11 = Recipe.Create(ItemID.SharpTears, 1);
            recipe11.AddIngredient(ItemID.HallowedBar, 10);
            recipe11.AddIngredient(ItemID.SoulofFright, 10);
            recipe11.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 8);
            recipe11.AddTile(TileID.MythrilAnvil);
            recipe11.Register();

            recipe11 = Recipe.Create(ItemID.BloodHamaxe, 1);
            recipe11.AddIngredient(ItemID.HallowedBar, 10);
            recipe11.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 8);
            recipe11.AddTile(TileID.MythrilAnvil);
            recipe11.Register();

            recipe11 = Recipe.Create(ItemID.SanguineStaff, 1);
            recipe11.AddIngredient(ItemID.HallowedBar, 10);
            recipe11.AddIngredient(ItemID.SoulofMight, 5);
            recipe11.AddIngredient(ItemID.SoulofSight, 5);
            recipe11.AddIngredient(ItemID.SoulofFright, 5);
            recipe11.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 10);
            recipe11.AddTile(TileID.MythrilAnvil);
            recipe11.Register();*/
        }

    }
  
    //Shops_________________________
    public class ShopItems : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            base.ModifyShop(shop);

            if (shop.NpcType == NPCID.Demolitionist)
            {
                
                shop.Add(ItemID.MiningShirt, Condition.PlayerCarriesItem(ItemID.MiningHelmet));
                shop.Add(ItemID.MiningPants, Condition.PlayerCarriesItem(ItemID.MiningHelmet));

                shop.Add(ModContent.ItemType<ProtoGrenade>(), Condition.PlayerCarriesItem(ModContent.ItemType<ProtoLauncher>()));
                shop.Add(ModContent.ItemType<ProtoGrenade>(), Condition.PlayerCarriesItem(ModContent.ItemType<FrostLauncher>()));
                shop.Add(ModContent.ItemType<ProtoGrenade>(), Condition.PlayerCarriesItem(ModContent.ItemType<MechTheSeeker>()));
                //lol idfc

                if (!GetInstance<ConfigurationsGlobal>().StormBossSkipsPlant) //shrug
                {
                    shop.Add(ItemID.RocketI, Condition.PlayerCarriesItem(ModContent.ItemType<StormLauncher>()));
                }
                
                shop.Add(ModContent.ItemType<StickyBomb>(), Condition.DownedSkeletron);
            }

            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add(ModContent.ItemType<Quack>(), Condition.DownedEyeOfCthulhu);
             
                shop.Add(ItemID.RainHat, Condition.InRain);
                shop.Add(ItemID.RainCoat, Condition.InRain);
                shop.Add(ModContent.ItemType<RainBoots>(), Condition.InRain);
            }
            if (shop.NpcType == NPCID.Mechanic)
            {
                shop.Add(ModContent.ItemType<Aircan>());

                shop.Add(ItemID.CombatWrench);
            }

            if (shop.NpcType == NPCID.Steampunker)
            {
                shop.Add(ModContent.ItemType<MechanicalRepeater>(), Condition.DownedSkeletronPrime, Condition.DownedDestroyer, Condition.DownedTwins);
            }

            if (shop.NpcType == NPCID.Cyborg)
            {
                shop.Add(ModContent.ItemType<MagicArrow>(), Condition.DownedPlantera);
            }

            if (shop.NpcType == NPCID.DyeTrader)
            {
                shop.Add(ItemID.DyeTradersScimitar);
            }

            if (shop.NpcType == NPCID.Painter)
            {
                shop.Add(ItemID.PainterPaintballGun);
            }

            if (shop.NpcType == NPCID.Stylist)
            {
                shop.Add(ItemID.StylistKilLaKillScissorsIWish);
            }

            if (shop.NpcType == NPCID.PartyGirl)
            {
                shop.Add(ItemID.PartyGirlGrenade); //fine have 2 I really dgaf        
            }


            if (shop.NpcType == NPCID.Princess)
            {
                shop.Add(ItemID.PrincessWeapon);          
            }

            if (shop.NpcType == NPCID.BestiaryGirl)
            {
                shop.Add(ModContent.ItemType<Items.Furniture.TheGoodBoyItem>()); //fine alway avalible 
            }
            if (shop.NpcType == NPCID.Demolitionist)
            {
                shop.Add(ItemID.Seed, Condition.PlayerCarriesItem(ModContent.ItemType<MoltenSeedLauncher>()));
            }
        }
    }
}
