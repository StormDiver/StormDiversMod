using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;


namespace StormDiversMod.Items.Accessory
{

    public class StormCoil : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overloaded Storm Core");
            Tooltip.SetDefault("Right click while holding UP to send a lightning portal to the cursor that zaps enemies\n" +
                "Right click while holding DOWN to return the portal to you");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 25;

            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;

            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;
            Item.expert = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            
                player.GetModPlayer<EquipmentEffects>().stormBossAccess = true;
   
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
    
}