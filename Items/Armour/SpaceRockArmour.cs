using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class SpaceRockBHelmet : ModItem
    { //Offense
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Asteroid Helmet");
            //Tooltip.SetDefault("15% increased damage\n8% increased critical strike chance");
            Item.ResearchUnlockCount = 1;
            HeadLayer.RegisterData(Item.headSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Head")
            });
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Generic) += 0.15f;
            player.GetCritChance(DamageClass.Generic) += 8;
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }
    
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
            if (player.HasBuff(ModContent.BuffType<Buffs.SpaceRockOffence>()))
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
            return body.type == ItemType<SpaceRockChestplate>() && legs.type == ItemType<SpaceRockLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            //player.setBonus = "Grants the Orbital Strike buff that causes asteroid boulders to fall upon the next attacked enemy";
            player.setBonus = this.GetLocalization("SetBonus").Value;
            //player.AddBuff(mod.BuffType("SpaceRockOffence"), 1);
            player.GetModPlayer<ArmourSetBonuses>().spaceRockOffence = true;
            player.GetModPlayer<ArmourSetBonuses>().SetBonus_SpaceRockOffence = "true";
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 8)        
           .AddTile(TileID.MythrilAnvil)
           .Register();
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //__________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Head)]
    public class SpaceRockBMask : ModItem
    { //Defence
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Asteroid Mask");
            //Tooltip.SetDefault("5% increased damage\nIncreases health regeneration and grants immunity to knockback");
            Item.ResearchUnlockCount = 1;
            HeadLayer.RegisterData(Item.headSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Head")
            });
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 30;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.noKnockback = true;
            player.lifeRegen += 2;
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
            if (player.HasBuff(ModContent.BuffType<Buffs.SpaceRockDefence>()))
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
            return body.type == ItemType<SpaceRockChestplate>() && legs.type == ItemType<SpaceRockLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            //player.setBonus = "Grants the Orbital Defense buff that reduces damage of the next attack by 25%\n Also summons damaging asteroid boulders from the sky\nIsn't activated if the attack only deals 1 damage";
            player.setBonus = this.GetLocalization("SetBonus").Value;

            player.GetModPlayer<ArmourSetBonuses>().spaceRockDefence = true;
            player.GetModPlayer<ArmourSetBonuses>().SetBonus_SpaceRockDefence = "true";
        }


        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 8)
           .AddTile(TileID.MythrilAnvil)
           .Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    
    public class SpaceRockChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Asteroid Chestplate");
            //Tooltip.SetDefault("7% increased damage\n5% increased critical strike chance");
            Item.ResearchUnlockCount = 1;
            if (!Main.dedServ)
            {
                BodyGlowmaskPlayer.RegisterData(Item.bodySlot, () => new Color(255, 255, 255, 75) * 0.6f);
            }
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 22;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Generic) += 0.07f;
            player.GetCritChance(DamageClass.Generic) += 5;
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }

        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 11)
            .AddTile(TileID.MythrilAnvil)
            .Register();


        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class SpaceRockLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Asteroid Leggings");
            //Tooltip.SetDefault("6% increased damage\n5% increased critical strike chance\n50% increased movement speed");
            Item.ResearchUnlockCount = 1;
            LegsLayer.RegisterData(Item.legSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Legs")
            });
        }
       
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0,  10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.06f;
            player.GetCritChance(DamageClass.Generic) += 5;

            player.moveSpeed += 0.25f;

            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ModContent.ItemType<Items.OresandBars.SpaceRockBar>(), 9)
          .AddTile(TileID.MythrilAnvil)
          .Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
   

}