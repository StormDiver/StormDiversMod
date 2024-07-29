using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.DataStructures;
using StormDiversMod.Projectiles.ToolsProjs;

namespace StormDiversMod.Items.Tools
{
    internal class FishingRodCoral : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fisher of the Sea");
            //Tooltip.SetDefault("Fires 2 lines at once");
            //ItemID.Sets.CanFishInLava[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 8;
            Item.useTime = 8;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.fishingPole = 10; // Sets the poles fishing power
            Item.shootSpeed = 10f; // Sets the speed in which the bobbers are launched. Wooden Fishing Pole is 9f and Golden Fishing Rod is 17f.
            Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.BobberCoral>(); // The Bobber projectile.
        }

        public override void HoldItem(Player player)//Can make the line never break 
        {
            //player.accFishingLine = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) //multiple lines
        {
            int bobberAmount = 2; // 3 bobbers
            float spreadAmount = 30f; // how much the different bobbers are spread out.

            for (int index = 0; index < bobberAmount; ++index)
            {
                Vector2 bobberSpeed = velocity + new Vector2(Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f, Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f);

                // Generate new bobbers
                Projectile.NewProjectile(source, position, bobberSpeed, type, 0, 0f, player.whoAmI);
            }
            return false;
        }

        public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        {
            // Change these two values in order to change the origin of where the line is being drawn.
            lineOriginOffset = new Vector2(43, -25);

            // Sets the fishing line's color. Note that this will be overridden by the colored string accessories.
            if (bobber.ModProjectile is BobberCoral bobberCoral)
            {
                // ExampleBobber has custom code to decide on a line color.
                lineColor = bobberCoral.FishingLineColor;
            }
            else
            {
                // If the bobber isn't ExampleBobber, a Fishing Bobber accessory is in effect and we use DiscoColor instead.
                lineColor = Main.DiscoColor;
            }
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Coral, 8)
            .AddIngredient(ItemID.Starfish, 2)
            .AddIngredient(ItemID.Seashell, 2)
            .AddTile(TileID.WorkBenches)
            .Register();

        }
    }

    internal class FishingRodDerpling : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Derpling Fishing Rod");
			//Tooltip.SetDefault("Fires multiple lines at once");
			//ItemID.Sets.CanFishInLava[Item.type] = true;
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{			
			 Item.width = 24;
			 Item.height = 28;
			 Item.useStyle = ItemUseStyleID.Swing;
			 Item.useAnimation = 8;
			 Item.useTime = 8;
			 Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.fishingPole = 45; // Sets the poles fishing power
			Item.shootSpeed = 15f; // Sets the speed in which the bobbers are launched. Wooden Fishing Pole is 9f and Golden Fishing Rod is 17f.
			Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.BobberDerpling>(); // The Bobber projectile.
		}

		public override void HoldItem(Player player)//Can make the line never break 
		{
			//player.accFishingLine = true;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) //multiple lines
        {          
			int bobberAmount = 3; // 3 bobbers
			float spreadAmount = 60f; // how much the different bobbers are spread out.

			for (int index = 0; index < bobberAmount; ++index)
			{
				Vector2 bobberSpeed = velocity + new Vector2(Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f, Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f);

				// Generate new bobbers
				Projectile.NewProjectile(source, position, bobberSpeed, type, 0, 0f, player.whoAmI);
			}
			return false;
		}

        public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        {
            // Change these two values in order to change the origin of where the line is being drawn.
            lineOriginOffset = new Vector2(47, -25);

            // Sets the fishing line's color. Note that this will be overridden by the colored string accessories.
            if (bobber.ModProjectile is BobberDerpling bobberDerpling)
            {
                // ExampleBobber has custom code to decide on a line color.
                lineColor = bobberDerpling.FishingLineColor;
            }
            else
            {
                // If the bobber isn't ExampleBobber, a Fishing Bobber accessory is in effect and we use DiscoColor instead.
                lineColor = Main.DiscoColor;
            }
        }
        public override void AddRecipes()
		{
			CreateRecipe()
		 .AddIngredient(ItemID.ChlorophyteBar, 10)
		 .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 5)
		 .AddTile(TileID.MythrilAnvil)
		 .Register();
		}
	}

    internal class FishingRodLunar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Celestial Pole");
            //Tooltip.SetDefault("Fires multiple lines at once\nAllows fishing in lava");
            //ItemID.Sets.CanFishInLava[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 8;
            Item.useTime = 8;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 7, 50, 0);
            Item.rare = ItemRarityID.Red;
            Item.fishingPole = 100; // Sets the poles fishing power
            Item.shootSpeed = 20f; // Sets the speed in which the bobbers are launched. Wooden Fishing Pole is 9f and Golden Fishing Rod is 17f.
            Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.BobberLunar>(); // The Bobber projectile.
        }

        public override void HoldItem(Player player)//Can make the line never break 
        {
            player.accFishingLine = true; //doesn't work :pain:
            player.accFishingBobber = true;
            player.accLavaFishing = true;
            player.accTackleBox = true;
        }
        int bobberai;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) //multiple lines
        {
            int bobberAmount = 4; // 4 bobbers
            float spreadAmount = 80f; // how much the different bobbers are spread out.

            for (int index = 0; index < bobberAmount; ++index)
            {
                Vector2 bobberSpeed = velocity + new Vector2(Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f, Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f);

                // Generate new bobbers
                Projectile.NewProjectile(source, position, bobberSpeed, type, 0, 0f, player.whoAmI, 0, 0, bobberai);
                bobberai++;
            }

            bobberai = 0;
            return false;
        }

        public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        {
            // Change these two values in order to change the origin of where the line is being drawn.
            lineOriginOffset = new Vector2(47, -25);

            // Sets the fishing line's color. Note that this will be overridden by the colored string accessories.
            if (bobber.ModProjectile is BobberLunar bobberLunar)
            {
                if (bobberLunar.Projectile.ai[2] == 0)//solar
                    lineColor = new Color(225, 154, 0, 255);
                else if (bobberLunar.Projectile.ai[2] == 1) //vortex
                    lineColor = new Color(0, 238, 161, 255);
                else if (bobberLunar.Projectile.ai[2] == 2) //nebula
                    lineColor = new Color(255, 96, 232, 255);
                else  //Stardust
                    lineColor = new Color(0, 173, 246, 255);

                //lineColor.A = 0;
                // ExampleBobber has custom code to decide on a line color.
                //lineColor = bobberLunar.FishingLineColor;
            }
            else
            {
                // If the bobber isn't ExampleBobber, a Fishing Bobber accessory is in effect and we use DiscoColor instead.
                lineColor = Main.DiscoColor;
            }
        }

        public override void AddRecipes()
        {
         CreateRecipe()
         .AddIngredient(ItemID.FragmentSolar, 5)
         .AddIngredient(ItemID.FragmentVortex, 5)
         .AddIngredient(ItemID.FragmentNebula, 5)
         .AddIngredient(ItemID.FragmentStardust, 5)
         .AddTile(TileID.LunarCraftingStation)
         .Register();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}