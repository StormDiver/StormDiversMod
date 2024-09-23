using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Common;
using Terraria.Audio;
using StormDiversMod.NPCs.NPCProjs;
using System.Collections.Generic;
using Terraria.UI;

namespace StormDiversMod.Items.Vanitysets
{
    [AutoloadEquip(EquipType.Head)]
    public class FlatCap : ModItem
    { 
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Flat Cap");
            //Tooltip.SetDefault("Coated in ash");
            Item.ResearchUnlockCount = 1;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
    [AutoloadEquip(EquipType.Head)]
    public class PizzaCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Pizza Cap");
            //Tooltip.SetDefault("'None Pizza with left beef'");
            Item.ResearchUnlockCount = 1;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
    [AutoloadEquip(EquipType.Head)]
    public class GnomedHat : ModItem
    {
        public override void SetStaticDefaults()
        { 
            base.SetStaticDefaults(); 
            //DisplayName.SetDefault("Gnome Hat");
            //Tooltip.SetDefault("'You've been Gnomed'");
            Item.ResearchUnlockCount = 1;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }    
        public override void SetDefaults() 
        {
            Item.width = 18;
            Item.height = 18; 
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true; 
        } 
        public override void AddRecipes()
        {
           CreateRecipe()
          .AddIngredient(ItemID.GardenGnome, 1)
          .AddTile(TileID.WorkBenches)
          .Register();
        }
    }
} 