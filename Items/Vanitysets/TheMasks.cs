using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using StormDiversMod.NPCs.NPCProjs;

namespace StormDiversMod.Items.Vanitysets
{
    [AutoloadEquip(EquipType.Head)]
    public class ThePainMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("ThePain");
            //Tooltip.SetDefault("When the pain is too much");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<TheClaymanMask>();
        }
    
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
      
        public override bool OnPickup(Player player)
        {
            if (!GetInstance<ConfigurationsIndividual>().NoPain)
            {
                SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSound") with { Volume = 1.5f }, player.Center);
            }
            return base.OnPickup(player);
        }

    }
    [AutoloadEquip(EquipType.Head)]
    public class TheClaymanMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Clayman");
            //Tooltip.SetDefault("Sliently judge everybody around you");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ThePainMask>();

        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true; 
        }
        public override void ArmorSetShadows(Player player)
        {

        }
        public override bool OnPickup(Player player)
        {
            if (!GetInstance<ConfigurationsIndividual>().NoPain)
            {
                SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ClayManSound") with { Volume = 1.5f }, player.Center);
            }
            return base.OnPickup(player);
        }
    }
}