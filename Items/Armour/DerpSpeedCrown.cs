using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Armour
{
   
    [AutoloadEquip(EquipType.Head)]
    public class DerplingBCrown : ModItem
    {
       
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Messenger's Crown");
            //Tooltip.SetDefault("Increases movement speed and acceleration");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxRunSpeed += 2;
            player.runAcceleration *= 1.25f;
            player.runSlowdown = 0.8f;
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
            return (body.type == ItemID.GoldChainmail && legs.type == ItemID.GoldGreaves ) || (body.type == ItemID.PlatinumChainmail && legs.type == ItemID.PlatinumGreaves);

        }

        public override void UpdateArmorSet(Player player)
        {
            if (player.armor[1].type == ItemID.GoldChainmail && player.armor[2].type == ItemID.GoldGreaves)
            {
                //player.setBonus = "3 defense";
                player.setBonus = this.GetLocalization("SetBonus1").Value;
                player.statDefense += 3;
            }
            else if (player.armor[1].type == ItemID.PlatinumChainmail && player.armor[2].type == ItemID.PlatinumGreaves)
            {
                //player.setBonus = "5 defense";
                player.setBonus = this.GetLocalization("SetBonus2").Value;
                player.statDefense += 5;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("StormDiversMod:GoldBars", 15)
            .AddIngredient(ItemID.Ruby, 1)
            .AddIngredient(ItemID.Feather, 5)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }

  
   
}