using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Common;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;


namespace StormDiversMod.Items.Accessory
{

    public class FrostCube : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frozen Queen's Core");
            //Tooltip.SetDefault("Increases your max number of minions and sentries by 1\n15% Increased whip range");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;

            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Yellow;

            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.GetModPlayer<EquipmentEffects>().frostCube = true;   
            player.maxMinions += 1;
            player.maxTurrets += 1;
            //player.GetDamage(DamageClass.Summon) += 0.1f;
            player.whipRangeMultiplier += 1.15f;
            //player.whipUseTimeMultiplier *= 0.9f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
   
}