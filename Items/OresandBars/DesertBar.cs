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
    public class DesertBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Bar");
            Tooltip.SetDefault("Used in the creation of forbidden armour and weapons");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 80;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;

        }

        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.createTile = TileType<DesertBarPlaced>();
            Item.consumable = true;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                        .AddRecipeGroup("StormDiversMod:MidHMBars", 1)
                        .AddIngredient(ModContent.ItemType<Items.Materials.DesertOre>(), 3)
                        .AddTile(TileID.Hellforge)
                        .Register();

        }

        //_____________________________________________________________________________________________________________
        public class DesertBarPlaced : ModTile
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

                AddMapEntry(new Color(238, 204, 34), Language.GetText("Forbidden Bar")); // localized text for "Metal Bar"
            }

            public override bool Drop(int i, int j)
            {
                Tile t = Main.tile[i, j];
                int style = t.TileFrameX / 18;
                if (style == 0) // It can be useful to share a single tile with multiple styles. This code will let you drop the appropriate bar if you had multiple.
                {
                    Item.NewItem(i * 16, j * 16, 16, 16, ItemType<DesertBar>());
                }
                return base.Drop(i, j);
            }
        }
    }
    //____________________________________________________________________________________________________
   
    //____________________________________________________________________________________
    public class DesertOrePlaced : ModTile
    {

        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
            Main.tileOreFinderPriority[Type] = 600; // Metal Detector value
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 700; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Forbidden Ore");
            AddMapEntry(new Color(238, 204, 34), name);

            DustType = 54;
            ItemDrop = ModContent.ItemType<Items.Materials.DesertOre>();
            SoundType = SoundID.Tink;
            SoundStyle = 1;
            MineResist = 4f;
            MinPick = 100;
        }


    }
}