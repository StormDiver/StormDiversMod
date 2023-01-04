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
    public class LightDarkWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harmonia Wings");
            Tooltip.SetDefault("Allows flight and slow fall");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            if (!Main.dedServ)
            {
                WingsLayer.RegisterData(Item.wingSlot, new DrawLayerData()
                {
                    Texture = ModContent.Request<Texture2D>(Texture + "_Wings"),
                    Color = () => Color.White * 0.75f
                });
            }
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(80, 7.2f, 1.65f);
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;

            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
           
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

        }
      
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        { 
                ascentWhenFalling = 1.3f;
                ascentWhenRising = 0.2f;
                maxCanAscendMultiplier = 1.2f;
                maxAscentMultiplier = 2f;
                constantAscend = 0.12f;
        }     
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LightShard, 1)
            .AddIngredient(ItemID.DarkShard, 1)
           .AddIngredient(ItemID.Feather, 15)
           .AddIngredient(ItemID.SoulofFlight, 20)
           .AddTile(TileID.MythrilAnvil)
           .Register();        
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Accessory/LightDarkWings_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}