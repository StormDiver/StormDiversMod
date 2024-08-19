using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Potions
{
    public class BarrierPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Endurance Healing Potion");
            //Tooltip.SetDefault("Reduces the damage of the next incoming attack by 25%");
            Item.ResearchUnlockCount = 30;

        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.LightRed;
            Item.healLife = 125;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.potion = true;
        }
        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.HeartBarrierBuff>(), 1800);
        }
        /* public override void GetHealLife(Player player, bool quickHeal, ref int healValue)
         {
             healValue = 75;
         }*/
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<BarrierPotion>(), 5);
            recipe.AddIngredient(ItemID.GreaterHealingPotion, 5);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.CrackedHeart>(), 1);
            recipe.AddIngredient(ItemID.UnicornHorn);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
    public class DoubleHealingPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Enhanced Healing Potion");
            //Tooltip.SetDefault("Potion sickness lasts longer than normal");
            Item.ResearchUnlockCount = 30;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            /*foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "HealLife")
                {
                    line.Text = line.Text +"\nRestores 50% of your maximum life up to 300";
                }

            }*/
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
                     Item.rare = ItemRarityID.Lime;
            Item.healLife = 300;
            Item.value = Item.sellPrice(0, 0, 15, 0);
            Item.potion = true;
            //Item.DefaultToHealingPotion(22, 32, 80);
        }
        public override void OnConsumeItem(Player player)
        {
                player.AddBuff(BuffID.PotionSickness, 5700); //95 seconds, 1:35
               // player.AddBuff(BuffID.PotionSickness, 4200); //70 seconds, 1:10

            //Time left is now dynamically changed 
        }
        public override void GetHealLife(Player player, bool quickHeal, ref int healValue)
        {
            if (player.statLifeMax2 > 600)
            {
                healValue = (player.statLifeMax2 / 2) - ((player.statLifeMax2 / 2) - 300);
            }
            else
            {
                healValue = (player.statLifeMax2 / 2);
            }
         }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<DoubleHealingPotion>(), 5);
            recipe.AddIngredient(ItemID.GreaterHealingPotion, 5);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.CrackedHeart>(), 1);
            recipe.AddIngredient(ItemID.LifeFruit, 1);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }

    }
    public class HeartPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heart Vial");
            //Tooltip.SetDefault("Enemies will drop a heart on death more often when you're not at max health
            //\nBosses also always drop more hearts");
            Item.ResearchUnlockCount = 20;

        }
        public override void OnConsumeItem(Player player)
        {
           
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;

            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.buffType = BuffType<Buffs.HeartBuff>(); //Specify an existing buff to be applied when used.
            Item.buffTime = 28800;
        }
        public override bool CanUseItem(Player player)
        {
            /*if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return false;
            }
            else*/
            {
                return true;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                /*if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\n[c/ff2500:Doesn't work on Multiplayer]"; //multiplayer sucks
                    }
                }*/
            }
        }

        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<HeartPotion>(), 5);
            recipe.AddIngredient(ItemID.BottledWater, 5);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.CrackedHeart>(), 1);
            recipe.AddIngredient(ItemID.Daybloom);
            recipe.AddIngredient(ItemID.Blinkroot);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();

        }
    }
    public class FruitHeartPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Super Heart Vial");
            //Tooltip.SetDefault("Enemies will have a chance to drop a super heart on death that heals for 50 life when you're not at max health 
            //\nBosses will always drop 2 super hearts");
            Item.ResearchUnlockCount = 20;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
                     Item.rare = ItemRarityID.Lime;

            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.buffType = BuffType<Buffs.FruitHeartBuff>(); //Specify an existing buff to be applied when used.
            Item.buffTime = 28800;
        }
        public override bool CanUseItem(Player player)
        {
            /*if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return false;
            }
            else*/
            {
                return true;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                /*if (Main.netMode == NetmodeID.MultiplayerClient)
                {

                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\n[c/ff2500:Doesn't work on Multiplayer]"; //multiplayer sucks
                    }
                }*/
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<FruitHeartPotion>(), 3);
            recipe.AddIngredient(ItemID.BottledWater, 3);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.CrackedHeart>(), 1);
            recipe.AddIngredient(ItemID.LifeFruit);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }

    }
}