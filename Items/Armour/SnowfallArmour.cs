using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using StormDiversMod.Items.Materials;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class SnowfallBMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Snowfall Hood");
            //Tooltip.SetDefault("5% increased critical strike chance");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 5;
        }

        public override void ArmorSetShadows(Player player)
        {
            if (player.controlJump && player.velocity.Y > 0)
            {
                player.armorEffectDrawShadowLokis = true;
                if (player.controlUp)
                {
                    player.armorEffectDrawOutlines = true;
                }
                else
                {
                    player.armorEffectDrawOutlines = false;
                }
            }
            else
            {
                player.armorEffectDrawShadowLokis = false;
            }
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<SnowfallChest>() && legs.type == ItemType<SnowfallLeg>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Hold JUMP to glide\nHold UP while gliding to glide even slower";
            player.GetModPlayer<ArmourSetBonuses>().snowfallSet = true;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<BlueCloth>(), 4)
             .AddIngredient(ItemID.FlinxFur, 2)
             .AddTile(TileID.Loom)
             .Register();
        }
    }

    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    
    public class SnowfallChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Snowfall Coat");
            //Tooltip.SetDefault("Increases damage by a flat 1");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic).Flat += 1;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<BlueCloth>(), 8)
             .AddIngredient(ItemID.FlinxFur, 3)
             .AddTile(TileID.Loom)
             .Register();
        }
    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class SnowfallLeg : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Snowfall Boots");
            //Tooltip.SetDefault("10% increased jump and movement speed");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.jumpSpeedBoost += 0.4f;
            player.moveSpeed += 0.1f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<BlueCloth>(), 6)
             .AddIngredient(ItemID.FlinxFur, 2)
             .AddTile(TileID.Loom)
             .Register();
        }
    }
    //__________________________________________________________________________________________________________________________
   

}