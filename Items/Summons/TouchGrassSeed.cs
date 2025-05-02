using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.NPCs;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.Generation;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Items.Weapons;
using StormDiversMod.Items.Pets;
using StormDiversMod.Common;
using Humanizer;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Intrinsics.X86;
using Terraria.UI;

namespace StormDiversMod.Items.Summons
{
    /*public class TouchGrassSeed : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Special Grass Seed");
            //Tooltip.SetDefault("Use to activate Touch Grass mode for this world
            //In this mode you must stand on any grass block every 5 minutes or face the consequences
            //While standing on grass your damage dealt is increased by 20 % and damage taken is reduced by 20 %
            //This cannot be undone, use at your own risk");
            Item.ResearchUnlockCount = 0;
        }     
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 20;
            Item.height = 32;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useTurn = false;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }
        public override void PostUpdate()
        {
           
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip3" && ItemSlot.ShiftInUse)
                {
                    line.Text = "[c/31cb31:50% increased damage]\n[c/31cb31:25% increased crit chance]\n[c/31cb31:33% damage reduction]\n[c/31cb31:Greatly increases move speed and acceleration]\n[c/31cb31:Allows you to dash]";
                }
                if (StormWorld.TouchGrassMode)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                    {
                        line.Text = "[c/005000:Touch Grass Mode has already been enabled!]";
                    }
                }
                if (line.Mod == "Terraria" && line.Name == "Tooltip5")
                {
                    line.Text = "[c/ff8c00:" + line.Text + "]";
                }
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool CanUseItem(Player player)
        {
            if (StormWorld.TouchGrassMode == false)
                return true;
            else
                return false;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 100; i++)
                {
                    float speedX = 0f;
                    float speedY = -5f;

                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(new Vector2(player.Center.X + (5 * player.direction), player.Center.Y - 22 * player.gravDir), 0, 0, 3, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);
                    Main.dust[dust2].noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.Grass, player.Center);

                if (!StormWorld.TouchGrassMode)
                {
                    StormWorld.TouchGrassMode = true;
                    Main.NewText("Touch Grass mode has been enabled", Color.LimeGreen);
                    CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.LimeGreen, "Touch Grass mode has been enabled!", false);
                    player.QuickSpawnItem(null, ItemID.RecallPotion, 10);
                    player.QuickSpawnItem(null, ItemID.PotionOfReturn, 3);
                }
                else //unused
                {
                    StormWorld.TouchGrassMode = false;
                    Main.NewText("Touch Grass mode has been disabled", Color.DarkGreen);
                }
            }
            return true;
        }
        //failsafe incase accidentally activated
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            if (StormWorld.TouchGrassMode)
            {
                StormWorld.TouchGrassMode = false;
                Main.NewText("Touch Grass mode has been disabled", Color.DarkGreen);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddTile(TileID.Grass)
            .Register();
        }
    }*/
}