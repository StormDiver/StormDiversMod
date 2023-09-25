using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Tools
{
	public class GladiatorPick : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName.SetDefault("Gladiator's Pickaxe"); 
			//Tooltip.SetDefault("Can mine Meteorite");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults() 
		{
			Item.damage = 8;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
			Item.height = 42;
			Item.useTime = 14;
			Item.useAnimation = 17;
			Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.pick = 60;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = true;
            Item.knockBack = 2;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;

        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 57, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("StormDiversMod:GoldBars", 10)
            .AddIngredient(ModContent.ItemType<Items.Materials.RedSilk>(), 2)
            .AddTile(TileID.Anvils)
            .Register();

        }
    
    }
    public class GladiatorAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gladiator's Waraxe");
            //Tooltip.SetDefault("Used to be a mighty weapon, now merely used to chop down trees");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 42;
            Item.useTime = 15;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.axe = 12;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.knockBack = 5;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;

        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 57, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
       
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("StormDiversMod:GoldBars", 10)
            .AddIngredient(ModContent.ItemType<Items.Materials.RedSilk>(), 2)
            .AddTile(TileID.Anvils)
            .Register();
        }

    }
    public class GladiatorHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gladiator's Warhammer");
            //Tooltip.SetDefault("Used to be a mighty weapon, now merely used to smash down walls");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.damage = 12;

            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 48;
            Item.useTime = 19;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.hammer = 60;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.knockBack = 6;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;

        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) < 2)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 57, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
               .AddRecipeGroup("StormDiversMod:GoldBars", 10)
               .AddIngredient(ModContent.ItemType<Items.Materials.RedSilk>(), 2)
               .AddTile(TileID.Anvils)
               .Register();
        }

    }
}