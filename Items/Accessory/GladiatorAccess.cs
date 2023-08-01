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
    public class GladiatorAccess : ModItem
    {
        public override void SetStaticDefaults()
        {
            
            //DisplayName.SetDefault("Warrior's Trophy");
            //Tooltip.SetDefault("While above 75% HP your critical strike chance is increased by 20%");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {

            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;

            

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.statLife >= player.statLifeMax2 * 0.75f)
            {
                player.AddBuff(ModContent.BuffType<Buffs.GladiatorAccessBuff>(), 2);          
            }
        }
       

    }
}