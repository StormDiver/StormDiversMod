using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using StormDiversMod.NPCs.Banners;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using Terraria.Enums;
using StormDiversMod.Items.Accessory;
using StormDiversMod.Items.Materials;
using StormDiversMod.Items.OresandBars;

namespace StormDiversMod.NPCs.Banners
{
    public class IOUBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("IOU banner");
            //Tooltip.SetDefault("Can be converted into any modded banner");
            Item.ResearchUnlockCount = 0;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.IOU);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
    }
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.BabyDerp);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ModContent.ItemType<DerplingShell>(), 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.VineDerp);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.ChlorophyteOre, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.ScanDrone);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.FragmentVortex, 1)
           .AddTile(TileID.Loom)
           .Register();
        }
    }
    public class StormDerpBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stormling Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Stormling");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.StormDerp);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.FragmentVortex, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.VortCannon);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.FragmentVortex, 1)
           .AddTile(TileID.Loom)
           .Register();
        }
    }
    public class NebulaDerpBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brainling Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Brainling");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.NebulaDerp);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.FragmentNebula, 1)
           .AddTile(TileID.Loom)
           .Register();
        }
    }
    public class StardustDerpBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Starling Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Starling");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.StardustDerp);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.FragmentStardust, 1)
           .AddTile(TileID.Loom)
           .Register();
        }
    }
    public class SolarDerpBannerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blazling Banner");
            //Tooltip.SetDefault("Nearby players get a bonus against: Blazling");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.SolarDerp);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.FragmentSolar, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.MoonDerp);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.LunarOre, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.SpaceRockHead);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ModContent.ItemType<SpaceRock>(), 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.SpaceRockHeadLarge);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ModContent.ItemType<SpaceRock>(), 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.GladiatorMiniBoss);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ModContent.ItemType<RedSilk>(), 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.GraniteMiniBoss);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ModContent.ItemType<GraniteCore>(), 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.HellSoul);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.Hellstone, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.MushroomMiniBoss);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.GlowingMushroom, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.GolemMinion);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.LunarTabletFragment, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.HellMiniBoss);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ModContent.ItemType<SoulFire>(), 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.IceCore);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ModContent.ItemType<IceOre>(), 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.SandCore);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ModContent.ItemType<DesertOre>(), 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.MeteorDropper);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.Meteorite, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.GolemSentry);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.LunarTabletFragment, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.FrozenEye);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.FrostDaggerfish, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.FrozenSoul);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ModContent.ItemType<IceOre>(), 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.ThePainSlime);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.PinkGel, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.TheClaySlime);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ItemID.ClayBlock, 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.SnowmanPizza);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ModContent.ItemType<IceOre>(), 1)
           .AddTile(TileID.Loom)
           .Register();
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
            Item.DefaultToPlaceableTile(ModContent.TileType<BannersPlaced>(), (int)BannersPlaced.StyleID.SnowmanBomb);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<IOUBannerItem>(), 1)
           .AddIngredient(ModContent.ItemType<IceOre>(), 1)
           .AddTile(TileID.Loom)
           .Register();
        }
    }
}

////then add this to the custom npc you want to drop the banner and in public override void SetDefaults()
/*  banner = npc.type;
  bannerItem = mod.ItemType("CustomBannerItem"); //this defines what banner this npc will drop       */
