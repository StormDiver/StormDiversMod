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
        //melee
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Derpling Mask");
            Tooltip.SetDefault("16% increased melee damage\n6% increased melee critical strike chance\n15% increased melee speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 30;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Melee) += 0.16f;
            player.GetCritChance(DamageClass.Melee) += 6;
            player.meleeSpeed += 0.15f;
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
            player.setBonus = "Greatly increases jump, ascent, and max falling speed, and grants immunity to fall damage\nCreates a large shockwave upon jumping that launches nearby enemies into the air";

            player.GetModPlayer<StormPlayer>().derpJump = true;

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
    //______________________________________________________________________________________
    [AutoloadEquip(EquipType.Head)]
    public class DerplingBHelmet : ModItem
    {
        //ranged
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Derpling Helmet");
            Tooltip.SetDefault("12% increased ranged damage\n10% increased ranged critical strike chance\n20% chance not to consume ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
                     Item.rare = ItemRarityID.Lime;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Ranged) += 0.12f;
            player.GetCritChance(DamageClass.Ranged) += 10;
            player.ammoCost80 = true;
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
            player.setBonus = "Greatly increases jump, ascent, and max falling speed, and grants immunity to fall damage\nCreates a large shockwave upon jumping that launches nearby enemies into the air";

            player.GetModPlayer<StormPlayer>().derpJump = true;
         
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
        //magic
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Derpling Headgear");
            Tooltip.SetDefault("14% increased magic damage\n7% increased magic critical strike chance\nIncreases maximum mana by 80");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Magic) += 0.14f;
            player.GetCritChance(DamageClass.Magic) += 7;
            player.statManaMax2 += 80;
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
            player.setBonus = "Greatly increases jump, ascent, and max falling speed, and grants immunity to fall damage\nCreates a large shockwave upon jumping that launches nearby enemies into the air";

            player.GetModPlayer<StormPlayer>().derpJump = true;

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
    //______________________________________________________________________________________
    [AutoloadEquip(EquipType.Head)]
    public class DerplingBCrown : ModItem
    {
        //summon
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Derpling Crown");
            Tooltip.SetDefault("17% increased summoner damage\nIncreases your max number of minions by 1");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {

            player.maxMinions += 1;
            player.GetDamage(DamageClass.Summon) += 0.17f;

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
            player.setBonus = "Greatly increases jump, ascent, and max falling speed, and grants immunity to fall damage\nCreates a large shockwave upon jumping that launches nearby enemies into the air\nIncreases your max number of minions by 2";

            player.maxMinions += 2;

            player.GetModPlayer<StormPlayer>().derpJump = true;


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

    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    
    public class DerplingBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Derpling Breastplate");
            Tooltip.SetDefault("7% increased damage\n6% increased critical strike chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

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

            player.GetDamage(DamageClass.Generic) += 0.07f;
            player.GetCritChance(DamageClass.Generic) += 6;
          

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
            DisplayName.SetDefault("Derpling Greaves");
            Tooltip.SetDefault("6% increased damage and critical strike chance\n25% increased movement speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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

            player.GetDamage(DamageClass.Generic) += 0.06f;
            player.GetCritChance(DamageClass.Generic) += 6;
            player.moveSpeed += 0.25f;

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
   
}