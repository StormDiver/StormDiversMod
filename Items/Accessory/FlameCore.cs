using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Common;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace StormDiversMod.Items.Accessory
{
    public class FlameCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Betsy's Flame");
            //Tooltip.SetDefault("Summons homing flames when using any weapon near enemies\nIncreases acceleration");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.8f * Main.essScale);
            }
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
           
            Item.accessory = true;
            Item.expert = true;
        }
        //int particle = 5;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().FlameCoreItem = Item;
            player.GetModPlayer<EquipmentEffects>().flameCore = true;
              
        }
    }
}