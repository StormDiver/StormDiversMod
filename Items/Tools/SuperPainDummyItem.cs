using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Humanizer;
using StormDiversMod.NPCs;
using StormDiversMod.Items.Weapons;
using Terraria.GameContent.ItemDropRules;
using StormDiversMod.Items.Furniture;
using StormDiversMod.Items.Vanitysets;
using System.Collections.Generic;
using Terraria.UI;

namespace StormDiversMod.Items.Tools
{
	public class SuperPainDummyItem : ModItem
	{
		public override void SetStaticDefaults() 
		{
            //DisplayName.SetDefault("Super Pain Dummy"); 
            //Tooltip.SetDefault("Left click to place a Super Pain Dummy anywhere, a limit of 50 can be placed at a time
            //Right click to remove all placed dummies, dummies are also removed if a boss is alive
            //Dummies can be targeted by minions / homing projectiles, and can activate any item's special effect
            //Right click in the inventory to change dummy type (Standard/Tough/Broken/Light/Chonky)
            //Shift - Right click to toggle between floating or grounded");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() 
		{
            Item.width = 20;
			Item.height = 32;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Blue;
			Item.autoReuse = true;
            Item.useTurn = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.UseSound = SoundID.Item20;
        }
        int dummytype = 0; //0 = Standard, 1 = Tough, 2 = Broken, 3 = Light, 4 = Chonky
        int dummyflight = 0; //0 = No fly, 1 = fly
        public override bool CanUseItem(Player player)
        {
            if (NPC.CountNPCS(ModContent.NPCType<SuperPainDummy>()) < 50 && Main.netMode != NetmodeID.MultiplayerClient)
                return true;
            else
                return false;
        }
        public override void RightClick(Player player)
        {
            if (!ItemSlot.ShiftInUse) //change type
            {
                dummytype++;
                if (dummytype > 4)
                    dummytype = 0;
            }
            else //toggle flight
            {
                dummyflight++;
                if (dummyflight > 1)
                    dummyflight = 0;
            }
            //Main.NewText("pls: " + dummytype, 175, 17, 96);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {

                if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                {
                    line.Text = line.Text + "\n[c/af1160:Current mode:]";

                    if (dummyflight == 0)
                        line.Text = line.Text + "[c/af1160: Grounded,]";

                    if (dummyflight == 1)
                        line.Text = line.Text + "[c/af1160: Floating,]";

                    if (dummytype == 0)
                        line.Text = line.Text + "[c/af1160: Standard;]\n[c/af1160:- Very fast health regeneration]\n[c/af1160:- Useful for general weapon testing]";

                    else if (dummytype == 1)
                        line.Text = line.Text + "[c/af1160: Tough;]\n[c/af1160:- Very fast health regeneration and high defense (50)]\n[c/af1160:- Useful for testing armor penetration or low damage weapons]";

                    else if (dummytype == 2)
                        line.Text = line.Text + "[c/af1160: Broken;]\n[c/af1160:- Lower health and no regeneration]\n[c/af1160:- Useful for seeing how quickly you can deal a certain amount of damage]";

                    else if (dummytype == 3)
                        line.Text = line.Text + "[c/af1160: Light;]\n[c/af1160:- Very fast health regeneration but no knockback resistance]\n[c/af1160:- Useful for testing knockback]";

                    else if (dummytype == 4)
                        line.Text = line.Text + "[c/af1160: Chonky;]\n[c/af1160:- Very fast health regeneration and reflects certain projectiles]\n[c/af1160:- Useful for testing which projectiles can be reflected]";

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    line.Text = line.Text + "\n[c/ff2500:Doesn't work on Multiplayer]"; //multiplayer sucks

                }
                if (line.Mod == "Terraria" && line.Name == "ItemName")
                {
                    if (dummytype == 0)
                        line.Text = "Standard " + line.Text;
                    else if (dummytype == 1)
                        line.Text = "Tough " + line.Text;
                    else if (dummytype == 2)
                        line.Text = "Broken " + line.Text;
                    else if (dummytype == 3)
                        line.Text = "Light " + line.Text;
                    else if (dummytype == 4)
                        line.Text = "Chonky " + line.Text;

                    if (dummyflight == 0)
                        line.Text = "Grounded " + line.Text;
                    else if (dummyflight == 1)
                        line.Text = "Floating " + line.Text;
                }
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                    NPC.NewNPC(source, (int)Math.Round(Main.MouseWorld.X), (int)Math.Round(Main.MouseWorld.Y), ModContent.NPCType<SuperPainDummy>(), 0, dummytype, dummyflight); //Ai 0 for type, Ai 1 for flight

                for (int i = 0; i < 2; i++)
                {
                    int goreIndex = Gore.NewGore(null, new Vector2(Main.MouseWorld.X - 15, Main.MouseWorld.Y - 30), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                    goreIndex = Gore.NewGore(null, new Vector2(Main.MouseWorld.X - 15, Main.MouseWorld.Y - 30), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                    goreIndex = Gore.NewGore(null, new Vector2(Main.MouseWorld.X - 15, Main.MouseWorld.Y - 30), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                    goreIndex = Gore.NewGore(null, new Vector2(Main.MouseWorld.X - 15, Main.MouseWorld.Y - 30), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                }
            }

            return false;
        }
        public override bool ConsumeItem(Player player) => false;

        public override bool CanRightClick()
        {
            return true;
        }
       
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ItemID.TargetDummy, 1)

            .AddIngredient(ItemID.PinkGel, 15)
            .AddTile(TileID.Sawmill)
            .Register();
        }
    }
}