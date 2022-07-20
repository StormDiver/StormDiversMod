using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.OresandBars
{
    public class SpaceRockBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asteroid Bar");
            Tooltip.SetDefault("Radiating with energy");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 94;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;

        }

        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.createTile = TileType<SpaceRockBarPlaced>();
            Item.consumable = true;
            Item.autoReuse = true;

        }
        public override void AddRecipes()
        {


            CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 1)
            .AddIngredient(ModContent.ItemType<Items.Materials.SpaceRock>(), 1)
            .AddTile(TileID.AdamantiteForge)
            .Register();

        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {

            return Color.White;

        }

    }
    //______________________________________________________________________________
    public class SpaceRockBarPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(147, 112, 219), Language.GetText("Asteroid Bar")); // localized text for "Metal Bar"
        }

        public override bool Drop(int i, int j)
        {
            Tile t = Main.tile[i, j];
            int style = t.TileFrameX / 18;
            if (style == 0) // It can be useful to share a single tile with multiple styles. This code will let you drop the appropriate bar if you had multiple.
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<SpaceRockBar>());
            }
            return base.Drop(i, j);
        }

    }
   
}