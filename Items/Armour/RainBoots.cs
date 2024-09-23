using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using Terraria.Localization;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Legs)]
    public class RainBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Rain Boots");
            ////Tooltip.SetDefault("Rain");
            ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = false;
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.rare = ItemRarityID.White;
            Item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {

        }

        public override void ArmorSetShadows(Player player)
        {

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ItemID.RainHat && body.type == ItemID.RainCoat;

        }

        public override void UpdateArmorSet(Player player)
        {
            if (Main.raining)
            {

                player.AddBuff(ModContent.BuffType<RainBuff>(), 2);
            }
            if (ModLoader.HasMod("TRAEProject"))
            {
                //player.setBonus = "15% increased Movement Speed while raining";
                player.setBonus = this.GetLocalization("SetBonus2").Value;
            }
            else
            {
               //player.setBonus = "50% increased Movement Speed while raining";
                player.setBonus = this.GetLocalization("SetBonus1").Value;
            }
        }
    }
}