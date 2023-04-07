using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Tools
{
    public class TeddyBear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Teddy Bear");
            //Tooltip.SetDefault("Hug the bear to regenerate life\n'Full of love'");
            Item.ResearchUnlockCount = 1;


        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 38;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 120;
            Item.useAnimation = 120;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.holdStyle = 0;
            Item.noMelee = true; 
        }

        public override void HoldItem(Player player)
        {
            //Item.holdStyle = 1;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }
        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<EquipmentEffects>().bearcool > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.GetModPlayer<EquipmentEffects>().bearcool >= 0)
                {
                    player.AddBuff(ModContent.BuffType<Buffs.TeddyBuff>(), 300);
                    player.GetModPlayer<EquipmentEffects>().bearcool = 600;
                }
            }
            
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Silk, 20)
            .AddIngredient(ItemID.Cobweb, 45)
            .AddIngredient(ItemID.GreenThread, 3)
         .AddTile(TileID.Loom)
         .Register();


        }
    }
}