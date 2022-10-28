using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Potions
{
    public class GunPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Marksmanship Potion");
            Tooltip.SetDefault("15% increased bullet damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

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
            Item.maxStack = 99;
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
            DisplayName.SetDefault("Shroomite Ammo Power Potion");
            Tooltip.SetDefault("Increases ammo damage by 10%");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
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
            Item.maxStack = 99;
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
            DisplayName.SetDefault("Spectre Empowerment Potion");
            Tooltip.SetDefault("Increases maximum mana by 60");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

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
            Item.maxStack = 99;
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
            DisplayName.SetDefault("Beetle Penetration Potion");
            Tooltip.SetDefault("Increases armor penetration of all melee weapons by 20");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

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
            Item.maxStack = 99;
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
            DisplayName.SetDefault("Spooky Curse Potion");
            Tooltip.SetDefault("Increases Summon damage and whip speed by 10%");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

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
            Item.maxStack = 99;
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
            recipe.AddIngredient(ItemID.SpookyWood, 40);
            recipe.AddIngredient(ItemID.Shiverthorn, 2);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();



        }
    }


}