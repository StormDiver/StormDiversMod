using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
    //__________________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class ClawsBone : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Raptor Claws");
            //Tooltip.SetDefault("12% increased melee and whip speed\nEnables autoswing for melee weapons and whips\nIncreases melee and whip armor penetration by 5");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.GetModPlayer<EquipmentEffects>().blueCuffs = true;
            player.GetArmorPenetration(DamageClass.Melee) += 5;
            player.GetArmorPenetration(DamageClass.SummonMeleeSpeed) += 5;
            player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
            player.autoReuseGlove = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ItemID.FeralClaws, 1)
         .AddIngredient(ItemID.SharkToothNecklace, 1)
         .AddTile(TileID.TinkerersWorkbench)
         .Register();
        }
    }
    //__________________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class ClawsFrost : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glacial Claws");
            //Tooltip.SetDefault("12% increased melee and whip speed\nEnables autoswing for melee weapons and whips\nIncreases melee and whip armor penetration by 5\nMelee and whip attacks have a chance to inflict glacial burn");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().blueClaws = true;
            player.GetArmorPenetration(DamageClass.Melee) += 5;
            player.GetArmorPenetration(DamageClass.SummonMeleeSpeed) += 5;
            player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
            player.autoReuseGlove = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<ClawsBone>(), 1)
         .AddIngredient(ModContent.ItemType<BlueCuffs>(), 1)
         .AddIngredient(ItemID.Bone, 15)
         .AddTile(TileID.TinkerersWorkbench)
         .Register();
        }
    }

    //__________________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class ClawsSpooky : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nightmare Claws");
            //Tooltip.SetDefault("12% increased melee and whip speed\nEnables autoswing for melee weapons and whips\nIncreases melee and whip armor penetration by 5\nMelee and whip attacks have a chance to inflict Ultra Burn
            //Creates an aura of fear around you that affects enemies\nHas a greater range the lower on health you are");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Yellow;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().spookyClaws = true;
            player.GetArmorPenetration(DamageClass.Melee) += 12;
            player.GetArmorPenetration(DamageClass.SummonMeleeSpeed) += 12;
            player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
            player.autoReuseGlove = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<ClawsFrost>(), 1)
         .AddIngredient(ModContent.ItemType<SpookyCore>(), 1)
         .AddTile(TileID.TinkerersWorkbench)
         .Register();
        }
    }
    public class ClawsProjs : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        int dustproj;
        public override void AI(Projectile projectile) //Dust effects
        {
            var player = Main.player[projectile.owner];

            if (!player.GetModPlayer<EquipmentEffects>().spookyClaws == true)
                dustproj = 135;
            else
                dustproj = 200;

            if (projectile.aiStyle != 20)
            {
                if (player.GetModPlayer<EquipmentEffects>().blueClaws == true || player.GetModPlayer<EquipmentEffects>().spookyClaws == true)
                {
                    if (projectile.owner == Main.myPlayer && projectile.friendly && projectile.CountsAsClass(DamageClass.Melee) && projectile.damage >= 1 && !projectile.npcProj && !projectile.hostile)
                    {
                        if (Main.rand.Next(4) < 2)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, dustproj, 0f, 0f, 100, default, 1f);
                            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                            Main.dust[dustIndex].noGravity = true;
                        }
                    }
                }
            }   
        }
    }
    public class ClawsMelee : GlobalItem
    {
        public override bool InstancePerEntity => true;

        int dustproj;
        public override void MeleeEffects(Item Item, Player player, Rectangle hitbox) //Dust Effects
        {
            if (!player.GetModPlayer<EquipmentEffects>().spookyClaws == true)
                dustproj = 135;
            else
                dustproj = 200;

            if (player.GetModPlayer<EquipmentEffects>().blueClaws == true || player.GetModPlayer<EquipmentEffects>().spookyClaws == true)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustproj, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }
        }          
        
    }
}