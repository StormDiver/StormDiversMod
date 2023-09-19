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
using Humanizer;
using Newtonsoft.Json;


namespace StormDiversMod.Items.Accessory
{
    public class AridCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ancient Scarab");
            //Tooltip.SetDefault("Creates an Arid aura around the cursor, enemies within the aura receive 20% extra damage
            //Enemies continue to receive extra damage outside the aura for 3.2 seconds, but the amount falls off during this time
            //Aura requires a line of sight");
    Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 25;

            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;

            Item.accessory = true;
            
            Item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().aridBossAccess = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
    
}