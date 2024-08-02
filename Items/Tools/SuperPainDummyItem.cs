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
            //Shift-Right click to toggle between floating or grounded
            //Ctrl-Right click to change extra attributes for certain types");
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
        int dummyextra = 0; //Extra attributes for certain types (3 for now)
        string defencevalue;
        string healthvalue;
        string knockbackvalue;
        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < Main.maxNPCs; i++) //can't spawn if boss is active
            {
                NPC bosscheck = Main.npc[i];

                if (bosscheck.active && bosscheck.boss)
                    return false;
            }
            if (NPC.CountNPCS(ModContent.NPCType<SuperPainDummy>()) < 50 && Main.netMode != NetmodeID.MultiplayerClient)
                return true;
            else
                return false;
        }
        public override void RightClick(Player player)
        {
            if (!ItemSlot.ShiftInUse && !ItemSlot.ControlInUse) //change type
            {
                dummyextra = 0; //reset extra attribute
                dummytype++;
                if (dummytype > 4)
                    dummytype = 0;
            }
            if (ItemSlot.ShiftInUse && !ItemSlot.ControlInUse) //toggle flight
            {
                dummyflight++;
                if (dummyflight > 1)
                    dummyflight = 0;
            }
            if (!ItemSlot.ShiftInUse && ItemSlot.ControlInUse) //toggle extra
            {
                dummyextra++;
                if (dummyextra > 3)
                    dummyextra = 0;
            }
            //Main.NewText("pls: " + dummytype, 175, 17, 96);
        }
        public override void UpdateInventory(Player player) //cycle damage types
        {
            switch (dummyextra)
            {
                case 0:
                    defencevalue = "5";
                    healthvalue = "5,000";
                    knockbackvalue = "0%";
                    break;
                case 1:
                    defencevalue = "10";
                    healthvalue = "10,000";
                    knockbackvalue = "25%";
                    break;
                case 2:
                    defencevalue = "25";
                    healthvalue = "25,000";
                    knockbackvalue = "50%";
                    break;
                case 3:
                    defencevalue = "50";
                    healthvalue = "50,000";
                    knockbackvalue = "75%";
                    break;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                {
                    if (ItemSlot.ShiftInUse && !ItemSlot.ControlInUse) //toggle flight
                    {
                        line.Text = "[c/ffdf00:> " + line.Text + "]";
                    }
                }
                if (line.Mod == "Terraria" && line.Name == "Tooltip5")
                {
                    if (!ItemSlot.ShiftInUse && ItemSlot.ControlInUse) //toggle extra
                    {
                        line.Text = "[c/ffdf00:> " + line.Text + "]";
                    }
                }
                if (line.Mod == "Terraria" && line.Name == "Tooltip5")
                {
                    line.Text = line.Text + "\n[c/af1160:Current mode:]";
                    switch (dummyflight)
                    {
                        case 0:
                            line.Text = line.Text + "[c/b0791b: Grounded,]";
                            break;
                        case 1:
                            line.Text = line.Text + "[c/1dc3ba: Floating,]";
                            break;
                    }

                    switch (dummytype)
                    {
                        case 0:
                            line.Text = line.Text + "[c/e272e1: Standard;]\n[c/af1160:- Very fast health regeneration]\n[c/af1160:- Useful for general weapon testing]";
                            break;
                        case 1:
                            line.Text = line.Text + "[c/7d28df: Tough;]\n[c/af1160:- Very fast health regeneration and high defense]\n[c/af1160:- Useful for testing armor penetration or low damage weapons]" +
                                "\n[c/af1160:Extra Attribute: Defense = " + defencevalue + "]";
                            break;
                        case 2:
                            line.Text = line.Text + "[c/3ecd0d: Broken;]\n[c/af1160:- Lower health and no regeneration]\n[c/af1160:- Useful for seeing how quickly you can deal a certain amount of damage]" +
                                "\n[c/af1160:Extra Attribute: Health = " + healthvalue + "]";
                            break;
                        case 3:
                            line.Text = line.Text + "[c/0db9cd: Light;]\n[c/af1160:- Very fast health regeneration but no knockback resistance]\n[c/af1160:- Useful for testing knockback]" +
                                "\n[c/af1160:Extra Attribute: Knockback resistance = " + knockbackvalue + "]";
                            break;
                        case 4:
                            line.Text = line.Text + "[c/e8e006: Chonky;]\n[c/af1160:- Very fast health regeneration and reflects certain projectiles]\n[c/af1160:- Useful for testing which projectiles can be reflected]";
                            break;
                    }
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                    line.Text = line.Text + "\n[c/ff2500:Doesn't work on Multiplayer]"; //multiplayer sucks

                }
                if (line.Mod == "Terraria" && line.Name == "ItemName")
                {
                    switch (dummytype)
                    {
                        case 0:
                            line.Text = "Standard " + line.Text;
                            break;
                        case 1:
                            line.Text = "Tough " + line.Text;
                            break;
                        case 2:
                            line.Text = "Broken " + line.Text;
                            break;
                        case 3:
                            line.Text = "Light " + line.Text;
                            break;
                        case 4:
                            line.Text = "Chonky " + line.Text;
                            break;
                    }
                    switch (dummyflight)
                    {
                        case 0:
                            line.Text = "Grounded " + line.Text;
                            break;
                        case 1:
                            line.Text = "Floating " + line.Text;
                            break;
                    }
                }
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(source, (int)Math.Round(Main.MouseWorld.X), (int)Math.Round(Main.MouseWorld.Y), ModContent.NPCType<SuperPainDummy>(), 0, dummytype, dummyflight, dummyextra); //Ai 0 for type, Ai 1 for flight, Ai 2 for extra attribute

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