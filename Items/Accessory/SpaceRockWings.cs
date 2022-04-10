using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;


namespace StormDiversMod.Items.Accessory
{

    [AutoloadEquip(EquipType.Wings)]
    public class SpaceRockWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asteroid Wings");
            Tooltip.SetDefault("Allows flight and slow fall\nHas a fast horizontal movement and acceleration");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(190, 8.5f, 2.2f);
            WingsLayer.RegisterData(Item.wingSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Wings")
            });

        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;

            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
           
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
        
            /*if (player.controlDown && player.controlJump && player.wingTime > 0)
            { 
                if (Main.rand.Next(4) == 0)     //this defines how many dust to spawn
                {
                    player.wingTime += 1;
                }
            }*/

        }
      
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        { 
                ascentWhenFalling = 2f;
                ascentWhenRising = 0.3f;
                maxCanAscendMultiplier = 1f;
                maxAscentMultiplier = 2.2f;
                constantAscend = 0.15f;
        }
      

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 12)
           .AddIngredient(ItemID.SoulofFlight, 20)
           .AddTile(TileID.MythrilAnvil)
           .Register();
          

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}