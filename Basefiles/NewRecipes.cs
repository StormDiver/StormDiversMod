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
            Recipe recipe = Mod.CreateRecipe(ItemID.MiningShirt, 1);
            recipe.AddIngredient(ItemID.Silk, 25);
            recipe.AddRecipeGroup("StormDiversMod:GoldOres", 35);
            recipe.AddTile(TileID.Loom);
            recipe.Register();


            Recipe recipe2 = Mod.CreateRecipe(ItemID.MiningPants, 1);
            recipe2.AddIngredient(ItemID.Silk, 20);
            recipe2.AddRecipeGroup("StormDiversMod:GoldOres", 30);
            recipe2.AddTile(TileID.Loom);
            recipe2.Register();

            Recipe recipe3 = Mod.CreateRecipe(ItemID.GladiatorHelmet, 1);
            recipe3.AddRecipeGroup("StormDiversMod:GoldBars", 12);
            recipe3.AddIngredient(ModContent.ItemType<RedSilk>(), 2);
            recipe3.AddTile(TileID.Anvils);
            recipe3.Register();

            Recipe recipe4 = Mod.CreateRecipe(ItemID.GladiatorBreastplate, 1);
            recipe4.AddRecipeGroup("StormDiversMod:GoldBars", 18);
            recipe4.AddIngredient(ModContent.ItemType<RedSilk>(), 3);
            recipe4.AddTile(TileID.Anvils);
            recipe4.Register();

            Recipe recipe5 = Mod.CreateRecipe(ItemID.GladiatorLeggings, 1);
            recipe5.AddRecipeGroup("StormDiversMod:GoldBars", 15);
            recipe5.AddIngredient(ModContent.ItemType<RedSilk>(), 2);
            recipe5.AddTile(TileID.Anvils);
            recipe5.Register();

            Recipe recipe6 = Mod.CreateRecipe(ItemID.EskimoHood, 1);
            recipe6.AddIngredient(ModContent.ItemType<BlueCloth>(), 10);
            recipe6.AddIngredient(ItemID.Silk, 10);
            recipe6.AddTile(TileID.Loom);
            recipe6.Register();

            Recipe recipe7 = Mod.CreateRecipe(ItemID.EskimoCoat, 1);
            recipe7.AddIngredient(ModContent.ItemType<BlueCloth>(), 20);
            recipe7.AddIngredient(ItemID.Silk, 20);
            recipe7.AddTile(TileID.Loom);
            recipe7.Register();


            Recipe recipe8 = Mod.CreateRecipe(ItemID.EskimoPants, 1);
            recipe8.AddIngredient(ModContent.ItemType<BlueCloth>(), 15);
            recipe8.AddIngredient(ItemID.Silk, 15);
            recipe8.AddTile(TileID.Loom);
            recipe8.Register();

            Recipe recipe9 = Mod.CreateRecipe(ItemID.RodofDiscord, 1);
            recipe9.AddIngredient(ModContent.ItemType<ChaosShard>(), 12);
            recipe9.AddIngredient(ItemID.CrystalShard, 30);
            recipe9.AddIngredient(ItemID.SoulofLight, 25);
            recipe9.AddIngredient(ItemID.HallowedBar, 20);
            recipe9.AddTile(TileID.MythrilAnvil);
            recipe9.Register();

            Recipe recipe10 = Mod.CreateRecipe(ItemID.SlimeStaff, 1);
            recipe10.AddRecipeGroup(RecipeGroupID.Wood, 50);
            recipe10.AddIngredient(ItemID.Gel, 100);
            recipe10.AddTile(TileID.WorkBenches);
            recipe10.Register();

            Recipe recipe11 = Mod.CreateRecipe(ItemID.Tombstone, 1);
            recipe11.AddIngredient(ItemID.StoneBlock, 50);
            recipe11.AddTile(TileID.HeavyWorkBench);
            recipe11.Register();

            Recipe recipe12 = Mod.CreateRecipe(ItemID.FrostHelmet, 1);
            recipe12.AddIngredient(ModContent.ItemType<IceBar>(), 10);
            recipe12.AddIngredient(ItemID.FrostCore);
            recipe12.AddTile(TileID.MythrilAnvil);
            recipe12.Register();

            Recipe recipe13 = Mod.CreateRecipe(ItemID.FrostBreastplate, 1);
            recipe13.AddIngredient(ModContent.ItemType<IceBar>(), 18);
            recipe13.AddIngredient(ItemID.FrostCore);
            recipe13.AddTile(TileID.MythrilAnvil);
            recipe13.Register();

            Recipe recipe14 = Mod.CreateRecipe(ItemID.FrostLeggings, 1);
            recipe14.AddIngredient(ModContent.ItemType<IceBar>(), 14);
            recipe14.AddIngredient(ItemID.FrostCore);
            recipe14.AddTile(TileID.MythrilAnvil);
            recipe14.Register();

            Recipe recipe15 = Mod.CreateRecipe(ItemID.AncientBattleArmorHat, 1);
            recipe15.AddIngredient(ModContent.ItemType<DesertBar>(), 10);
            recipe15.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe15.AddTile(TileID.MythrilAnvil);
            recipe15.Register();

            Recipe recipe16 = Mod.CreateRecipe(ItemID.AncientBattleArmorShirt, 1);
            recipe16.AddIngredient(ModContent.ItemType<DesertBar>(), 18);
            recipe16.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe16.AddTile(TileID.MythrilAnvil);
            recipe16.Register();

            Recipe recipe17 = Mod.CreateRecipe(ItemID.AncientBattleArmorPants, 1);
            recipe17.AddIngredient(ModContent.ItemType<DesertBar>(), 14);
            recipe17.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe17.AddTile(TileID.MythrilAnvil);
            recipe17.Register();

            Recipe recipe18 = Mod.CreateRecipe(ItemID.ChainGun, 1);
            recipe18.AddIngredient(ModContent.ItemType<SantankScrap>(), 18);
            recipe18.AddTile(TileID.MythrilAnvil);
            recipe18.Register();

            Recipe recipe19 = Mod.CreateRecipe(ItemID.EldMelter, 1);
            recipe19.AddIngredient(ModContent.ItemType<SantankScrap>(), 18);
            recipe19.AddTile(TileID.MythrilAnvil);
            recipe19.Register();

            Recipe recipe20 = Mod.CreateRecipe(ItemID.Gladius, 1);
            recipe20.AddRecipeGroup("StormDiversMod:GoldBars", 10);
            recipe20.AddIngredient(ModContent.ItemType<RedSilk>(), 3);
            recipe20.AddTile(TileID.Anvils);
            recipe20.Register();
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
