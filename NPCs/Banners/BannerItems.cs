using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using StormDiversMod.NPCs.Banners;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;

namespace StormDiversMod.NPCs.Banners          //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class BabyDerpBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Baby Derpling Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: BABY DERPLINGS??!!! YOU MONSTER!!!");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
           
            Item.width = 10;    
            Item.height = 24;  
            Item.maxStack = 9999;  
            Item.useTurn = true;
            Item.autoReuse = true;  
            Item.useAnimation = 15;  
            Item.useTime = 10;  
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);  
            Item.createTile = ModContent.TileType<BabyDerpBannerPlace>(); //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class VineDerpBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Camouflaged Derpling Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Camouflaged Derpling");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.KillsToBanner[Item.type] = 25;

        }
        public override void SetDefaults()
        {

            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<VineDerpBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class ScanDroneBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scandrone Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Scandrone");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {

            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0); 
            Item.createTile = ModContent.TileType<ScanDroneBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class StormDerpBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Storm Hopper Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Storm Hopper");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {

            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0); 
            Item.createTile = ModContent.TileType<StormDerpBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class VortCannonBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vortexian Cannon Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Vortexian Cannon");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {

            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0); 
            Item.createTile = ModContent.TileType<VortCannonBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class NebulaDerpBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brain Hopper Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Brain Hopper");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {

            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0); 
            Item.createTile = ModContent.TileType<NebulaDerpBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class StardustDerpBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star Hopper Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Star Hopper");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0); 
            Item.createTile = ModContent.TileType<StardustDerpBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class SolarDerpBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blazing Hopper Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Blazing Hopper");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {

            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0); 
            Item.createTile = ModContent.TileType<SolarDerpBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class MoonDerpBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moonling Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Moonling");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.KillsToBanner[Item.type] = 10;
        }
        public override void SetDefaults()
        {

            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0); 
            Item.createTile = ModContent.TileType<MoonDerpBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class SpaceRockHeadBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Orbiter Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Asteroid Orbiter");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {

            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<SpaceRockHeadBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    public class SpaceRockHeadLargeBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Charger Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Asteroid Charger");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {  
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<SpaceRockHeadLargeBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    public class GladiatorMiniBossBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fallen Warrior Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Fallen Warrior");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.KillsToBanner[Item.type] = 25;

        }
        public override void SetDefaults()
        {

            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<GladiatorMiniBossBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class GraniteMiniBossBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Surged Granite Core Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Surged Granite Core");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.KillsToBanner[Item.type] = 25;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<GraniteMiniBossBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class HellSoulBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heartless Soul Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Heartless Soul");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<HellSoulBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class MushroomMiniBossBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Angry Mushroom Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Angry Mushroom");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.KillsToBanner[Item.type] = 25;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<MushroomMiniBossBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class GolemMinionBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Temple Guardian Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Temple Guardian");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.KillsToBanner[Item.type] = 25;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<GolemMinionBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class HellMiniBossBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Soul Cauldron Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Soul Cauldron");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.KillsToBanner[Item.type] = 10;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<HellMiniBossBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class IceCoreBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frigid Snowflake Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Frigid Snowflake");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.KillsToBanner[Item.type] = 25;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<IceCoreBannerPlace>();  //This defines what type of tile this Item will place	
            Item.placeStyle = 0;
        }
    }
    public class SandCoreBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dune Blaster Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Dune Blaster");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.KillsToBanner[Item.type] = 25;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<SandCoreBannerPlace>();  //This defines what type of tile this Item will place
            Item.placeStyle = 0;
        }
    }
    public class MeteorDropperBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Meteor Bomber Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Meteor Bomber");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<MeteorDropperBannerPlace>();  //This defines what type of tile this Item will place
            Item.placeStyle = 0;
        }
    }
    public class GolemSentryBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lihzahrd Flametrap Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Lihzahrd Flametrap");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<GolemSentryBannerPlace>();  //This defines what type of tile this Item will place
            Item.placeStyle = 0;
        }
    }
    public class FrozenEyeBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frozen Eyefish Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Frozen Eyefish");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<FrozenEyeBannerPlace>();  //This defines what type of tile this Item will place
            Item.placeStyle = 0;
        }
    }
    public class FrozenSoulBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frozen Spirit Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Frozen Spirit");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.KillsToBanner[Item.type] = 25;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<FrozenSoulBannerPlace>();  //This defines what type of tile this Item will place
            Item.placeStyle = 0;
        }
    }
    public class ThePainSlimeBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pain Slime Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Pain Slime");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<ThePainSlimeBannerPlaced>();  //This defines what type of tile this Item will place
            Item.placeStyle = 0;
        }
    }
    public class TheClaySlimeBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Clay Slime Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Clay Slime");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<TheClaySlimeBannerPlaced>();  //This defines what type of tile this Item will place
            Item.placeStyle = 0;
        }
    }

    public class SnowmanPizzaBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pizza Delivery Snowman Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Pizza Delivery Snowman");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<SnowmanPizzaBannerPlaced>();  //This defines what type of tile this Item will place
            Item.placeStyle = 0;
        }
    }
    public class SnowmanBombBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Snowman Bomber Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Pizza Delivery Snowman");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.createTile = ModContent.TileType<SnowmanBombBannerPlaced>();  //This defines what type of tile this Item will place
            Item.placeStyle = 0;
        }
    }
}

////then add this to the custom npc you want to drop the banner and in public override void SetDefaults()
/*  banner = npc.type;
  bannerItem = mod.ItemType("CustomBannerItem"); //this defines what banner this npc will drop       */
