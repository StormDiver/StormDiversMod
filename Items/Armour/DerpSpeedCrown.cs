using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Armour
{
   
    [AutoloadEquip(EquipType.Head)]
    public class DerplingBCrown : ModItem
    {
       
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Messenger's Crown");
            Tooltip.SetDefault("Increases maximum movement speed");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxRunSpeed += 2;
            //player.moveSpeed += 0.10f;
            
        }

        public override void ArmorSetShadows(Player player)
        {
            if ((player.velocity.X > 2 || player.velocity.X < -2) && player.velocity.Y == 0)
            {
                player.armorEffectDrawShadow = true;
            }
            else
            { 
                player.armorEffectDrawShadow = false;
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return true;
        }

        public override void UpdateArmorSet(Player player)
        {
          

        }
 
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("StormDiversMod:GoldBars", 15)
            .AddIngredient(ItemID.Ruby, 1)
            .AddIngredient(ItemID.Feather, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }

  
   
}