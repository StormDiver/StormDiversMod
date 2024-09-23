using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Common;

using StormDiversMod.Items.Materials;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Items.Weapons;
using System.Collections.Generic;

namespace StormDiversMod.Items.OresandBars
{
    public class IceBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Bar");
            //Tooltip.SetDefault("Used in the creation of frozen armor and weapons");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 80;
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
        }

        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.createTile = TileType<IceBarPlaced>();
            Item.consumable = true;
            Item.autoReuse = true;
        }
        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            if (extractinatorBlockType == TileID.ChlorophyteExtractinator)
            {
                resultType = ModContent.ItemType<DesertBar>();
                resultStack = 1;

            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("StormDiversMod:MidHMBars", 1)
            .AddIngredient(ModContent.ItemType<Items.Materials.IceOre>(), 3)
            .AddTile(TileID.Hellforge)
            .Register();
        }
    }
    //______________________________________________________________________________
    public class IceBarPlaced : ModTile
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

            AddMapEntry(new Color(0, 255, 255), Language.GetText("Frost Bar")); // localized text for "Metal Bar"
        }

        /*public override bool Drop(int i, int j)
        {
            Tile t = Main.tile[i, j];
            int style = t.TileFrameX / 18;
            if (style == 0) // It can be useful to share a single tile with multiple styles. This code will let you drop the appropriate bar if you had multiple.
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<IceBar>());
            }
            return base.Drop(i, j);
        }*/
    }
    //___________________________________________________________________________

    //___________________________________________________________________________
    public class IceOrePlaced : ModTile //Legacy Item, cannot be generated anymore or placed
    {

        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
            Main.tileOreFinderPriority[Type] = 600; // Metal Detector value, see 
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 700; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = true;
            //Main.tileMerge[TileID.IceBlock] = true;

            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Frost Ore");
            AddMapEntry(new Color(0, 255, 255), name);

            DustType = 92;
            //ItemDrop = ModContent.ItemType<IceOre>();
            HitSound = SoundID.Tink;
            MineResist = 4f;
            MinPick = 100;
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<IceOre>(), 1);
        }
    }
}