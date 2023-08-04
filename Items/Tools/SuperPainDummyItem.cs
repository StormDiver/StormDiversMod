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
            //Right click in inventory slot to change dummy type");
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
        int dummytype = 0;
        public override bool CanUseItem(Player player)
        {
            if (NPC.CountNPCS(ModContent.NPCType<SuperPainDummy>()) < 50)
                return true;
            else
                return false;
        }
        public override void RightClick(Player player)
        {
            dummytype++;

            if (dummytype > 4)
                dummytype = 0;

            //Main.NewText("pls: " + dummytype, 175, 17, 96);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {

                if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                {
                    if (dummytype == 0)
                        line.Text = line.Text + "\n[c/af1160:Current mode: Standard; Very fast health regeneration]";

                    else if (dummytype == 1)
                        line.Text = line.Text + "\n[c/af1160:Current mode: Floating; Very fast health regeneration and floats in the air]";

                    else if (dummytype == 2)
                        line.Text = line.Text + "\n[c/af1160:Current mode: Tough; Very fast health regeneration and high defense (50)]";

                    else if (dummytype == 3)
                        line.Text = line.Text + "\n[c/af1160:Current mode: Broken; Lower health and no regeneration]";

                    else if (dummytype == 4)
                        line.Text = line.Text + "\n[c/af1160:Current mode: Light; Very fast health regeneration but no knockback resistance]";

                }
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                    NPC.NewNPC(source, (int)Math.Round(Main.MouseWorld.X), (int)Math.Round(Main.MouseWorld.Y), ModContent.NPCType<SuperPainDummy>(), 0, dummytype);

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
            .AddRecipeGroup(RecipeGroupID.Wood, 100)
            .AddIngredient(ItemID.PinkGel, 5)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}