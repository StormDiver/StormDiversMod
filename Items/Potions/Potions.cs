using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Common;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Potions
{
    public class GunPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Marksmanship Potion");
            //Tooltip.SetDefault("15% increased gun damage");
            Item.ResearchUnlockCount = 20;
            ItemID.Sets.DrinkParticleColors[Type] = [
                new Color(254, 236, 161),
                new Color(212, 212, 212),
                new Color(250, 213, 64)
            ];
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.buffType = BuffType<Buffs.GunBuff>(); //Specify an existing buff to be applied when used.
            Item.buffTime = 18000; //The amount of time the buff declared in Item.buffType will last in ticks. 5400 / 60 is 90, so this buff will last 90 seconds.
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<GunPotion>(), 1);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.Lens);
            recipe.AddIngredient(ItemID.Blinkroot);
            recipe.AddIngredient(ItemID.Moonglow);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();

        }
    }
    //____________________________________________________

    public class ShroomitePotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroomite Crit Power Potion");
            //Tooltip.SetDefault("Increases ranged critical strike damage by 10%");
            Item.ResearchUnlockCount = 20;
            ItemID.Sets.DrinkParticleColors[Type] = [
               new Color(140, 238, 255),
                new Color(0, 140, 244),
                new Color(56, 37, 232)
           ];
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.buffType = BuffType<Buffs.ShroomiteBuff>(); //Specify an existing buff to be applied when used.
            Item.buffTime = 14400; //The amount of time the buff declared in Item.buffType will last in ticks. 5400 / 60 is 90, so this buff will last 90 seconds.
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<ShroomitePotion>(), 1);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.ShroomiteBar, 1);
            recipe.AddIngredient(ItemID.Moonglow, 2);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();


        }
    }
    //______________________________________________________________________
    public class SpectrePotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spectre Empowerment Potion");
            //Tooltip.SetDefault("Increases magic damage by 20% when below 50% mana");
            Item.ResearchUnlockCount = 20;
            ItemID.Sets.DrinkParticleColors[Type] = [
               new Color(196, 247, 255),
                new Color(140, 238, 255),
                new Color(35, 200, 254)
           ];
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.buffType = BuffType<Buffs.SpectreBuff>(); //Specify an existing buff to be applied when used.
            Item.buffTime = 14400; //The amount of time the buff declared in Item.buffType will last in ticks. 5400 / 60 is 90, so this buff will last 90 seconds.
        }

        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<SpectrePotion>(), 1);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.Ectoplasm, 2);
            recipe.AddIngredient(ItemID.Waterleaf, 2);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 150;
            return color;
        }
    }
    //______________________________________________________________________
    public class BeetlePotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Beetle Penetration Potion");
            //Tooltip.SetDefault("Increases armor penetration of all melee weapons by 20");
            Item.ResearchUnlockCount = 20;
            ItemID.Sets.DrinkParticleColors[Type] = [
               new Color(213, 170, 218),
                new Color(110, 93, 136),
                new Color(82, 57, 96)
           ];
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.buffType = BuffType<Buffs.BeetleBuff>(); //Specify an existing buff to be applied when used.
            Item.buffTime = 14400; //The amount of time the buff declared in Item.buffType will last in ticks. 5400 / 60 is 90, so this buff will last 90 seconds.
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<BeetlePotion>(), 1);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.BeetleHusk, 2);
            recipe.AddIngredient(ItemID.Fireblossom, 2);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();

        }
    }
    //____________________________________________________

    public class SpookyPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spooky Curse Potion");
            //Tooltip.SetDefault("Increases Summon damage and whip speed by 10%");
            Item.ResearchUnlockCount = 20;
            ItemID.Sets.DrinkParticleColors[Type] = [
               new Color(87, 77, 128),
                new Color(70, 52, 102),
                new Color(247, 136, 0)
           ];
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.buffType = BuffType<Buffs.SpookyBuff>(); //Specify an existing buff to be applied when used.
            Item.buffTime = 14400; //The amount of time the buff declared in Item.buffType will last in ticks. 5400 / 60 is 90, so this buff will last 90 seconds.
        }

        public override void AddRecipes()
        {

            Recipe recipe = Recipe.Create(ModContent.ItemType<SpookyPotion>(), 1);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.SpookyWood, 20);
            recipe.AddIngredient(ItemID.Shiverthorn, 2);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();



        }
    }


}