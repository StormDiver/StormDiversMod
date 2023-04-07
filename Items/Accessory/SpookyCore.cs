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

    public class SpookyCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spooky Emblem");
            //Tooltip.SetDefault("Creates an aura of fear around you that affects enemies\nHas a greater range the lower on health you are");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;

            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Yellow;

            Item.accessory = true;
            

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetModPlayer<EquipmentEffects>().spooked = true;

        }



    }
    }
   
        