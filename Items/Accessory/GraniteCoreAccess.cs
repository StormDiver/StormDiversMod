using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;

namespace StormDiversMod.Items.Accessory
{
    public class GraniteCoreAccess : ModItem
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Granite Core");
            Tooltip.SetDefault("Taking more than 1 damage increases damage by 50% for 4 seconds\nHas a 10 second cooldown afterwards");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
            }
        }
        public override void SetDefaults()
        {

            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;


        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().graniteBuff = true;
            
           
        }
        

    }
}