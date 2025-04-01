using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace StormDiversMod.Items.Weapons
{
	public class ABladeofGrass : ModItem
	{
		public override void SetStaticDefaults() 
		{
            //DisplayName.SetDefault("A Blade of Grass"); 
            //Tooltip.SetDefault("Literally just that, what did you expect\nDoes not count as touching real grass");
            Item.ResearchUnlockCount = 0;
        }
        public override void SetDefaults() 
		{
			Item.damage = 1;
        
            Item.DamageType = DamageClass.Generic;
            Item.width = 10;
			Item.height = 10;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 0, 0);
             Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Grass;
			Item.autoReuse = true;
            Item.knockBack = 0;
            Item.noMelee = false;
            Item.shootSpeed = 0.1f;
            Item.scale = 0.9f;
            Item.shoot = 1;
        }
        public override void UseAnimation(Player player)
        {
          
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
    }
}