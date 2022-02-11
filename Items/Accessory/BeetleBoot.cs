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
    [AutoloadEquip(EquipType.HandsOn)]

    public class BeetleBoot : ModItem //Now Beetle gauntlet
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beetle Gauntlet");
            Tooltip.SetDefault("Critical striking an enemy with a melee weapon causes mini beetles to burst out of them and swarm nearby enemies\nIncreases melee knockback\n12% increased melee speed\nEnables auto swing for melee weapons\nIncreases the size of melee weapons");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;

            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<StormPlayer>().beetleFist = true;
            player.meleeSpeed += 0.12f;
            player.autoReuseGlove = true;
            player.kbGlove = true;
            player.meleeScaleGlove = true;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.BeetleHusk, 10)
          .AddIngredient(ItemID.PowerGlove, 1)
          .AddIngredient(ItemID.SoulofMight, 10)
          .AddTile(TileID.MythrilAnvil)

          .Register();
           

        }


    }
}