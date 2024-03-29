using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Tools
{
	public class DesertPick : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName.SetDefault("Forbidden Pickaxe"); 
			//Tooltip.SetDefault("Can mine Adamantite and Titanium");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults() 
		{
			Item.damage = 20;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
			Item.height = 42;
			Item.useTime = 8;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;  
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.pick = 160;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = true;
            Item.knockBack = 5;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;

        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) < 3)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 10, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 16)
          .AddTile(TileID.MythrilAnvil)
          .Register();

           
        }

    }
    public class DesertHamaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Hamaxe");
            //Tooltip.SetDefault("Strong enough to destroy Altars");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.damage = 45;

            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 48;
            Item.useTime = 16;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.axe = 30;
            Item.hammer = 80;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.knockBack = 7;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;

        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) < 3)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 10, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 16)
           .AddTile(TileID.MythrilAnvil)
           .Register();

        }

    }
}