using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Common;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ExampleMod.Common.Players;
using StormDiversMod.Items.Ammo;

namespace StormDiversMod.Items.Misc
{
    public class ZephyrFeather: ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Zephyr Feather");
            //Tooltip.SetDefault("Permanently increases movement speed");
            //ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<CleansingHeart>();
        }
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item92;
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.LightPurple;
            Item.ResearchUnlockCount = 1;
            Item.value = Item.sellPrice(0, 2, 50, 0);
        }
        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PlayerUpgrades>().ZephyrFeatherUpgrade == false)
                return true;
            else
                return false;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<PlayerUpgrades>().ZephyrFeatherUpgrade = true;
            for (int i = 0; i < 30; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, 3).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(new Vector2(player.Center.X + (15 * player.direction), player.Center.Y - 20 * player.gravDir), 0, 0, 67, perturbedSpeed.X, perturbedSpeed.Y, 0, default, 1f);
                Main.dust[dust2].noGravity = true;
            }
            return true;
        }
        public override void PostUpdate()
        {
        }
    }
   //recipe in newrecipesandshops
}