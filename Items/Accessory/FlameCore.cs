using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
    public class FlameCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Betsy's Flame");
            Tooltip.SetDefault("Has a chance to summon Betsy's flames when using any weapon\nSlightly increases player acceleration");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.8f * Main.essScale);
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = -12;
           
            Item.accessory = true;
            Item.expert = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }
        //int particle = 5;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {   
            player.GetModPlayer<StormPlayer>().flameCore = true;
              
        }
 
    }
}