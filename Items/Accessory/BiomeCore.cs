using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Common;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;

namespace StormDiversMod.Items.Accessory
{
    public class BiomeCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Biome Core");
            //Tooltip.SetDefault("Taking more than 1 damage increases damage by 50% for 4 seconds with a 10 second cooldown afterwards" +
                //"\nWhile above 75% HP your critical strike chance is increased by 20%\nIncreases damage dealt and reduces damage taken when losing health");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 1;
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
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            
            Item.accessory = true;
        }

      
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().graniteBuff = true;
            player.GetModPlayer<EquipmentEffects>().mushroomSuper = true;
            if (player.statLife >= player.statLifeMax2 * 0.66f)
            {
                player.AddBuff(ModContent.BuffType<Buffs.GladiatorAccessBuff>(), 2);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<GraniteCoreAccess>(), 1)
            .AddIngredient(ModContent.ItemType<GladiatorAccess>(), 1)
            .AddIngredient(ModContent.ItemType<SuperMushroom>(), 1)
            .AddIngredient(ItemID.SoulofNight, 3)
            .AddIngredient(ItemID.SoulofLight, 3)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }
    }
}