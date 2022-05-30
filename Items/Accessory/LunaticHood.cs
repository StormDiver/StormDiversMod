using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Accessory
{
   
    public class LunaticHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunatic Hood of Command");
            Tooltip.SetDefault("Summons 2 mini cultists minions that fly next to you and fire shadow fireballs at enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 7, 50, 0);
            Item.rare = ItemRarityID.Red;

            Item.accessory = true;
            Item.expert = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }


        //int skulltime = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            
            player.GetModPlayer<EquipmentEffects>().lunaticHood = true;
           
           
        }
        //Drops from treasure bag
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}