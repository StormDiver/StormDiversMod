using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Tools
{
    public class DerplingDrill : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Drill");
            //Tooltip.SetDefault("'Drill through the ground'");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.IsDrill[Item.type] = true;
        }

        public override void SetDefaults()
        {
            
            Item.damage = 40;
            Item.DamageType.CountsAsClass(DamageClass.Melee);
            Item.width = 40;
            Item.height = 22;

            Item.useTime = 3;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 5, 0, 0);
                     Item.rare = ItemRarityID.Lime;
            Item.knockBack = 1f;
           
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.DerpDrillProj>();
            Item.shootSpeed = 30f;
            Item.pick = 200;
            Item.tileBoost = 0;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true; 
            Item.channel = true; 
            Item.autoReuse = true;
            
            
        }    

        public override void AddRecipes()
        {

            CreateRecipe()
           .AddIngredient(ItemID.ChlorophyteBar, 14)
           .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 8)
           .AddTile(TileID.MythrilAnvil)
           .Register();
           
        }
    }
    public class DerplingChainsaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Chainsaw");
            //Tooltip.SetDefault("'Cut down the trees'");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.IsChainsaw[Item.type] = true;
        }

        public override void SetDefaults()
        {

            Item.damage = 55;
            Item.DamageType.CountsAsClass(DamageClass.Melee);
            Item.width = 60;
            Item.height = 20;
            Item.useTime = 3;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 5, 0, 0);
                     Item.rare = ItemRarityID.Lime;
            Item.knockBack = 4.6f;
          
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.DerpChainProj>();
            Item.shootSpeed = 40f;
            Item.axe = 24;
            Item.tileBoost = 0;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            
           
        }


        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.ChlorophyteBar, 14)
          .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 8)
          .AddTile(TileID.MythrilAnvil)
          .Register();

        }
    }
    public class DerplingJackhammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Jackhammer");
            //Tooltip.SetDefault("'Smash down the walls'");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {

            Item.damage = 50;
            Item.DamageType.CountsAsClass(DamageClass.Melee);
            Item.width = 50;
            Item.height = 20;
            Item.useTime = 4;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 5, 0, 0);
                     Item.rare = ItemRarityID.Lime;
            Item.knockBack = 5.2f;

            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.DerpJackProj>();
            Item.shootSpeed = 35f;
            Item.hammer = 100;
            Item.tileBoost = 0;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;

           
        }



        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.ChlorophyteBar, 14)
          .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 8)
          .AddTile(TileID.MythrilAnvil)
          .Register();

        }
    }
}