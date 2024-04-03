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
    public class DerplingBMask : ModItem
    {
        
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Derpling Mask");
            //Tooltip.SetDefault("10% increased damage\nIncreases maximum number of minions by 1");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.10f;
            //player.GetCritChance(DamageClass.Generic) += 5;
            player.maxMinions += 1;
        }

        public override void ArmorSetShadows(Player player)
        {
            if (player.velocity.Y == 0)
            {
                player.armorEffectDrawShadow = false;
            }
            else
            {
                player.armorEffectDrawShadow = true;

            }

            if (player.HasBuff(ModContent.BuffType<Buffs.DerpBuff>()))
            {
                player.armorEffectDrawOutlines = true;

            }
            else
            {
                player.armorEffectDrawOutlines = false;
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<DerplingBreastplate>() && legs.type == ItemType<DerplingGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            //player.setBonus = "Greatly increases jump and ascent speed, and allows auto jumping\nHold DOWN to increase falling speed\nCreates a large shockwave upon jumping that launches nearby enemies into the air";
            player.setBonus = this.GetLocalization("SetBonus").Value;
            player.GetModPlayer<ArmourSetBonuses>().derpJump = true;

        }
        public override void AddRecipes()
        {
        
            CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 10)
            .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }


    }
    

    //__________________________________________________________________________________________________________________________

    [AutoloadEquip(EquipType.Head)]
    public class DerplingBHeadgear : ModItem
    {
        
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Ancient Derpling Helmet");
            //Tooltip.SetDefault("10% increased damage\nIncreases maximum number of minions by 1");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.10f;
            //player.GetCritChance(DamageClass.Generic) += 5;
            player.maxMinions += 1;
        }

        public override void ArmorSetShadows(Player player)
        {
            if (player.velocity.Y == 0)
            {
                player.armorEffectDrawShadow = false;
            }
            else
            {
                player.armorEffectDrawShadow = true;

            }
            if (player.HasBuff(ModContent.BuffType<Buffs.DerpBuff>()))
            {
                player.armorEffectDrawOutlines = true;

            }
            else
            {
                player.armorEffectDrawOutlines = false;
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<DerplingBreastplate>() && legs.type == ItemType<DerplingGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            //player.setBonus = "Greatly increases jump and ascent speed, and allows auto jumping\nHold DOWN to increase falling speed\nCreates a large shockwave upon jumping that launches nearby enemies into the air";
            player.setBonus = this.GetLocalization("SetBonus").Value;
            player.GetModPlayer<ArmourSetBonuses>().derpJump = true;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 10)
            .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 5)
            .AddTile(TileID.DemonAltar)
            .Register();

        }


    }
   
    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    
    public class DerplingBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Derpling Breastplate");
            //Tooltip.SetDefault("8% increased damage\n8% increased critical strike chance");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
                     Item.rare = ItemRarityID.Lime;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Generic) += 0.08f;
            player.GetCritChance(DamageClass.Generic) += 8;
          

        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.ChlorophyteBar, 18)
           .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 8)
           .AddTile(TileID.MythrilAnvil)
           .Register();

        }
       
    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class DerplingGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Derpling Greaves");
            //Tooltip.SetDefault("7% increased damage and critical strike chance\nGrants immunity to fall damage");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
                     Item.rare = ItemRarityID.Lime;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Generic) += 0.07f;
            player.GetCritChance(DamageClass.Generic) += 7;
            player.noFallDmg = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 14)
            .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 7)
            .AddTile(TileID.MythrilAnvil)
            .Register();

        }

       
    }
    //_________________________
    
}