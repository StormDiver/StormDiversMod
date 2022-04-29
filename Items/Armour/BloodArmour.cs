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
    public class BloodBHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Hemoglobin Helmet");
            Tooltip.SetDefault("4% increased melee damage and critical strike chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.04f;
            player.GetCritChance(DamageClass.Melee) += 4;
        }

        public override void ArmorSetShadows(Player player)
        {
            //player.armorEffectDrawShadow = true;
            player.armorEffectDrawOutlines = true;
            if (player.HasBuff(ModContent.BuffType<Buffs.BloodBurstBuff>()))
            {
                if (Main.rand.Next(4) == 0)     //this defines how many dust to spawn
                {
                    Dust dust;
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Vector2 position = player.position;
                    dust = Main.dust[Terraria.Dust.NewDust(position, player.width, player.height, 5, 0f, 0f, 0, new Color(255, 255, 255), 1f)];

                }
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<BloodChainmail>() && legs.type == ItemType<BloodGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Multiple damaging blood orbs burst out of you upon striking an enemy with a melee weapon";

            player.GetModPlayer<StormPlayer>().BloodDrop = true;
   
        }
        
        public override void AddRecipes()
        {

            CreateRecipe()
             .AddRecipeGroup("StormDiversMod:EvilBars", 12)
            .AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 3)
            .AddTile(TileID.Anvils)
            .Register();
           
        }
    }

    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    
    public class BloodChainmail : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Hemoglobin Breastplate");
            Tooltip.SetDefault("6% increased melee damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Melee) += 0.06f;
         
      
        }
        public override void AddRecipes()
        {
          CreateRecipe()
             .AddRecipeGroup("StormDiversMod:EvilBars", 18)
            .AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 4)
            .AddTile(TileID.Anvils)
            .Register();

        }
      
    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class BloodGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Hemoglobin Greaves");
            Tooltip.SetDefault("4% increased melee critical strike chance\n12% increased melee and movement speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 4;
            player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
            player.moveSpeed += 0.12f;
           
        }
        public override void AddRecipes()
        {
            CreateRecipe()
               .AddRecipeGroup("StormDiversMod:EvilBars", 15)
              .AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 3)
              .AddTile(TileID.Anvils)
              .Register();



        }
    }
    //__________________________________________________________________________________________________________________________
   

}