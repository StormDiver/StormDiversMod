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
            DisplayName.SetDefault("Derpling Mask");
            Tooltip.SetDefault("10% increased damage\n5% increased critical strike chance\nReduces damage taken by 6%");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

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
            player.GetCritChance(DamageClass.Generic) += 5;
            player.endurance += 0.06f;
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
            player.setBonus = "Greatly increases jump, ascent, and max falling speed\nCreates a large shockwave upon jumping that launches nearby enemies into the air";

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
        
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Derpling Headgear");
            Tooltip.SetDefault("5% increased damage\n12% increased critical strike chance\nIncreases maximum movement speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.05f;


            player.GetCritChance(DamageClass.Generic) += 12;
            player.maxRunSpeed += 2;
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
            player.setBonus = "Greatly increases jump, ascent, and max falling speed\nCreates a large shockwave upon jumping that launches nearby enemies into the air";

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
            Tooltip.SetDefault("6% increased damage and critical strike chance\nGrants immunity to fall damage");
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
    //______________________________________________________________________________________
    //[AutoloadEquip(EquipType.Head)]
    public class DerplingBHelmet : ModItem
    {
        //ranged
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Derpling Helmet");
            Tooltip.SetDefault("Unused, can be crafted into the Derpling Mask or Headgear");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 0;
        }

        public override void UpdateEquip(Player player)
        {

            /* player.GetDamage(DamageClass.Ranged) += 0.12f;
             player.GetCritChance(DamageClass.Ranged) += 10;
             player.ammoCost80 = true;*/
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

        /*public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<DerplingBreastplate>() && legs.type == ItemType<DerplingGreaves>();
        }*/

        public override void UpdateArmorSet(Player player)
        {

        }
        public override void AddRecipes()
        {
            /*CreateRecipe()
           .AddIngredient(ItemID.ChlorophyteBar, 10)
           .AddIngredient(ModContent.ItemType<Items.Materials.DerplingShell>(), 5)
           .AddTile(TileID.MythrilAnvil)
           .Register();*/
            Recipe recipe = Mod.CreateRecipe(ModContent.ItemType<DerplingBMask>(), 1);
            recipe.AddIngredient(this, 1);
            recipe.Register();

            recipe = Mod.CreateRecipe(ModContent.ItemType<DerplingBHeadgear>(), 1);
            recipe.AddIngredient(this, 1);
            recipe.Register();
        }


    }
}