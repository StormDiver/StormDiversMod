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


using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.Basefiles
{
    public class NewRecipes : GlobalItem
    {
        public override void AddRecipes()
        {
            //Mining armour
            Recipe recipe = Mod.CreateRecipe(ItemID.MiningShirt, 1);
            recipe.AddIngredient(ItemID.Silk, 25);
            recipe.AddRecipeGroup("StormDiversMod:GoldOres", 35);
            recipe.AddTile(TileID.Loom);
            recipe.Register();

            recipe = Mod.CreateRecipe(ItemID.MiningPants, 1);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddRecipeGroup("StormDiversMod:GoldOres", 30);
            recipe.AddTile(TileID.Loom);
            recipe.Register();

            //Gladiator gear
            Recipe recipe2 = Mod.CreateRecipe(ItemID.GladiatorHelmet, 1);
            recipe2.AddRecipeGroup("StormDiversMod:GoldBars", 12);
            recipe2.AddIngredient(ModContent.ItemType<RedSilk>(), 2);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();

            recipe2 = Mod.CreateRecipe(ItemID.GladiatorBreastplate, 1);
            recipe2.AddRecipeGroup("StormDiversMod:GoldBars", 18);
            recipe2.AddIngredient(ModContent.ItemType<RedSilk>(), 3);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();

            recipe2 = Mod.CreateRecipe(ItemID.GladiatorLeggings, 1);
            recipe2.AddRecipeGroup("StormDiversMod:GoldBars", 15);
            recipe2.AddIngredient(ModContent.ItemType<RedSilk>(), 2);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();

            recipe2 = Mod.CreateRecipe(ItemID.Gladius, 1);
            recipe2.AddRecipeGroup("StormDiversMod:GoldBars", 10);
            recipe2.AddIngredient(ModContent.ItemType<RedSilk>(), 3);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();

            //Snow armour
            Recipe recipe3 = Mod.CreateRecipe(ItemID.EskimoHood, 1);
            recipe3.AddIngredient(ModContent.ItemType<BlueCloth>(), 10);
            recipe3.AddIngredient(ItemID.Silk, 10);
            recipe3.AddTile(TileID.Loom);
            recipe3.Register();

            recipe3 = Mod.CreateRecipe(ItemID.EskimoCoat, 1);
            recipe3.AddIngredient(ModContent.ItemType<BlueCloth>(), 20);
            recipe3.AddIngredient(ItemID.Silk, 20);
            recipe3.AddTile(TileID.Loom);
            recipe3.Register();


            recipe3 = Mod.CreateRecipe(ItemID.EskimoPants, 1);
            recipe3.AddIngredient(ModContent.ItemType<BlueCloth>(), 15);
            recipe3.AddIngredient(ItemID.Silk, 15);
            recipe3.AddTile(TileID.Loom);
            recipe3.Register();

            //ROD
            Recipe recipe4 = Mod.CreateRecipe(ItemID.RodofDiscord, 1);
            recipe4.AddIngredient(ModContent.ItemType<ChaosShard>(), 25);
            recipe4.AddIngredient(ItemID.CrystalShard, 30);
            recipe4.AddIngredient(ItemID.SoulofLight, 25);
            recipe4.AddIngredient(ItemID.HallowedBar, 20);
            recipe4.AddTile(TileID.MythrilAnvil);
            recipe4.Register();

            //Sime staff
            Recipe recipe5 = Mod.CreateRecipe(ItemID.SlimeStaff, 1);
            recipe5.AddRecipeGroup(RecipeGroupID.Wood, 50);
            recipe5.AddIngredient(ItemID.Gel, 100);
            recipe5.AddTile(TileID.WorkBenches);
            recipe5.Register();

            //Tombstone
            Recipe recipe6 = Mod.CreateRecipe(ItemID.Tombstone, 1);
            recipe6.AddIngredient(ItemID.StoneBlock, 50);
            recipe6.AddTile(TileID.HeavyWorkBench);
            recipe6.Register();

            //Frost armour
            Recipe recipe7 = Mod.CreateRecipe(ItemID.FrostHelmet, 1);
            recipe7.AddIngredient(ModContent.ItemType<IceBar>(), 10);
            recipe7.AddIngredient(ItemID.FrostCore);
            recipe7.AddTile(TileID.MythrilAnvil);
            recipe7.Register();

            recipe7 = Mod.CreateRecipe(ItemID.FrostBreastplate, 1);
            recipe7.AddIngredient(ModContent.ItemType<IceBar>(), 18);
            recipe7.AddIngredient(ItemID.FrostCore);
            recipe7.AddTile(TileID.MythrilAnvil);
            recipe7.Register();

            recipe7 = Mod.CreateRecipe(ItemID.FrostLeggings, 1);
            recipe7.AddIngredient(ModContent.ItemType<IceBar>(), 14);
            recipe7.AddIngredient(ItemID.FrostCore);
            recipe7.AddTile(TileID.MythrilAnvil);
            recipe7.Register();

            //Forbidden armour
            Recipe recipe8 = Mod.CreateRecipe(ItemID.AncientBattleArmorHat, 1);
            recipe8.AddIngredient(ModContent.ItemType<DesertBar>(), 10);
            recipe8.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe8.AddTile(TileID.MythrilAnvil);
            recipe8.Register();

            recipe8 = Mod.CreateRecipe(ItemID.AncientBattleArmorShirt, 1);
            recipe8.AddIngredient(ModContent.ItemType<DesertBar>(), 18);
            recipe8.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe8.AddTile(TileID.MythrilAnvil);
            recipe8.Register();

            recipe8 = Mod.CreateRecipe(ItemID.AncientBattleArmorPants, 1);
            recipe8.AddIngredient(ModContent.ItemType<DesertBar>(), 14);
            recipe8.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe8.AddTile(TileID.MythrilAnvil);
            recipe8.Register();

            //Santank weapons
            Recipe recipe9 = Mod.CreateRecipe(ItemID.ChainGun, 1);
            recipe9.AddIngredient(ModContent.ItemType<SantankScrap>(), 18);
            recipe9.AddTile(TileID.MythrilAnvil);
            recipe9.Register();

            recipe9 = Mod.CreateRecipe(ItemID.EldMelter, 1);
            recipe9.AddIngredient(ModContent.ItemType<SantankScrap>(), 18);
            recipe9.AddTile(TileID.MythrilAnvil);
            recipe9.Register();

            //Blood moon items
            Recipe recipe10 = Mod.CreateRecipe(ItemID.BloodRainBow, 1);           
            recipe10.AddRecipeGroup("StormDiversMod:EvilBars", 12);
            recipe10.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 4);
            recipe10.AddTile(TileID.Anvils);
            recipe10.Register();

            recipe10 = Mod.CreateRecipe(ItemID.VampireFrogStaff, 1);
            recipe10.AddRecipeGroup("StormDiversMod:EvilBars", 12);
            recipe10.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 4);
            recipe10.AddTile(TileID.Anvils);
            recipe10.Register();

            recipe10 = Mod.CreateRecipe(ItemID.BloodFishingRod, 1);
            recipe10.AddRecipeGroup("StormDiversMod:EvilBars", 12);
            recipe10.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 4);
            recipe10.AddTile(TileID.Anvils);
            recipe10.Register();

            recipe10 = Mod.CreateRecipe(ItemID.BloodMoonStarter, 1);
            recipe10.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 8);
            recipe10.AddTile(TileID.Anvils);
            recipe10.Register();

            //hardmode Blood moon items
            Recipe recipe11 = Mod.CreateRecipe(ItemID.DripplerFlail, 1);
            recipe11.AddIngredient(ItemID.HallowedBar, 10);
            recipe11.AddIngredient(ItemID.SoulofMight, 10);
            recipe11.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 8);
            recipe11.AddTile(TileID.MythrilAnvil);
            recipe11.Register();

            recipe11 = Mod.CreateRecipe(ModContent.ItemType<Items.Weapons.BloodyRifle>(), 1);
            recipe11.AddIngredient(ItemID.HallowedBar, 10);
            recipe11.AddIngredient(ItemID.SoulofSight, 10);
            recipe11.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 8);
            recipe11.AddTile(TileID.MythrilAnvil);
            recipe11.Register();

            recipe11 = Mod.CreateRecipe(ItemID.SharpTears, 1);
            recipe11.AddIngredient(ItemID.HallowedBar, 10);
            recipe11.AddIngredient(ItemID.SoulofFright, 10);
            recipe11.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 8);
            recipe11.AddTile(TileID.MythrilAnvil);
            recipe11.Register();

            recipe11 = Mod.CreateRecipe(ItemID.BloodHamaxe, 1);
            recipe11.AddIngredient(ItemID.HallowedBar, 10);
            recipe11.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 8);
            recipe11.AddTile(TileID.MythrilAnvil);
            recipe11.Register();

            recipe11 = Mod.CreateRecipe(ItemID.SanguineStaff, 1);
            recipe11.AddIngredient(ItemID.HallowedBar, 10);
            recipe11.AddIngredient(ItemID.SoulofMight, 5);
            recipe11.AddIngredient(ItemID.SoulofSight, 5);
            recipe11.AddIngredient(ItemID.SoulofFright, 5);
            recipe11.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 10);
            recipe11.AddTile(TileID.MythrilAnvil);
            recipe11.Register();
        }

    }
    public class VanillaShops : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case NPCID.Demolitionist:
                    {
                        if (Main.LocalPlayer.HasItem(ItemID.MiningHelmet))

                        {
                            shop.item[nextSlot].SetDefaults(ItemID.MiningShirt);
                            nextSlot++;
                            shop.item[nextSlot].SetDefaults(ItemID.MiningPants);
                            nextSlot++;

                        }
                    }
                    break;
            }
           
        }
    }
}
