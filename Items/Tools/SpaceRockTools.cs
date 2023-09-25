using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Tools
{
    public class SpaceRockDrillSaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid DrillSaw");
            //Tooltip.SetDefault("'Not to be confused with the SawDrill'");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.IsChainsaw[Item.type] = true;
            ItemID.Sets.IsDrill[Item.type] = true;
        }

        public override void SetDefaults()
        {
            
            Item.damage = 60;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 40;
            Item.height = 22;

            Item.useTime = 3;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 10, 0, 0);
                     Item.rare = ItemRarityID.Cyan;
            Item.knockBack = 1.5f;
       
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.SpaceRockDrillSawProj>();
            Item.shootSpeed = 50;
            Item.pick = 210;
            Item.axe = 30;
            Item.tileBoost = 1;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true; 
            Item.channel = true; 
            Item.autoReuse = true;
            
        }


        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 9)
          .AddTile(TileID.MythrilAnvil)
          .Register();
           
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
   
    public class SpaceRockJackhammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Jackhammer");
            //Tooltip.SetDefault("'Great for smashing down walls'");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {

            Item.damage = 60;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 50;
            Item.height = 20;
            Item.useTime = 4;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.knockBack = 6f;

            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.SpaceRockJackhammerProj>();
            Item.shootSpeed = 40;
            Item.hammer = 100;
            Item.tileBoost = 1;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 9)
            .AddTile(TileID.MythrilAnvil)
            .Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}