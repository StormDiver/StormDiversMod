using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using Terraria.Localization;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class AridBMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Arid Mask");
            //Tooltip.SetDefault("6% increased critical strike chance");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<AridChestplate>();

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 6;
        }

        public override void ArmorSetShadows(Player player)
        {
            //player.armorEffectDrawShadow = true;
            player.armorEffectDrawOutlines = true;
            
                if (Main.rand.Next(8) == 0)     //this defines how many dust to spawn
                {
                int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 138, player.velocity.X * 1f, player.velocity.Y * 1f, 130, default, 1.5f);

                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.5f;
                int dust2 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 55, player.velocity.X, player.velocity.Y, 130, default, 0.5f);

            }
            
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<AridChestplate>() && legs.type == ItemType<AridGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            //player.setBonus = "Critical hits create an explosion from the struck enemy which damages nearby enemies";
            player.setBonus = this.GetLocalization("SetBonus").Value;
            player.GetModPlayer<ArmourSetBonuses>().aridCritSet = true;
            player.GetModPlayer<ArmourSetBonuses>().SetBonus_Arid = "true";
        }
    }

    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    
    public class AridChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Arid Breastplate");
            //Tooltip.SetDefault("Increases critical strike damage by 10%");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<AridGreaves>();

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetModPlayer<EquipmentEffects>().aridCritChest = true;

        }     
    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class AridGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Arid Greaves");
            //Tooltip.SetDefault("6% increased critical strike chance\n12% increased movement speed");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<AridBMask>();

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 6;
            player.moveSpeed += 0.12f;
           
        }
    }
    //__________________________________________________________________________________________________________________________
   

}