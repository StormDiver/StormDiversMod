using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Tools
{
	public class BloodPax : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Bloody Pax"); 
			Tooltip.SetDefault("'Try not to make a mess with this'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults() 
		{
			Item.damage = 15;
            Item.crit = 0;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 42;
			Item.useTime = 13;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.pick = 65;
            Item.axe = 20;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.knockBack = 3;
            Item.scale = 1.1f;
            
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) < 2)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddRecipeGroup("StormDiversMod:EvilBars", 16)
           .AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 5)
           .AddTile(TileID.Anvils)
           .Register();


        }

    }
    public class BloodHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Hammer");
            Tooltip.SetDefault("'Try not to leave blood stains on the wall'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.damage = 22;

            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 48;
            Item.useTime = 17;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.hammer = 55;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.knockBack = 7;
            Item.scale = 1.25f;

        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) < 2)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddRecipeGroup("StormDiversMod:EvilBars", 16)
           .AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 5)
           .AddTile(TileID.Anvils)
           .Register();
           

        }

    }
}