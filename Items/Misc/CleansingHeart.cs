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
using System.Collections.Generic;
using Terraria.UI;
using Terraria.DataStructures;
using StormDiversMod.Projectiles;

namespace StormDiversMod.Items.Misc
{
    public class CleansingHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cleansing Charm");
            //Tooltip.SetDefault("Removes all permanent shimmer upgrades from you");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item117;
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.LightPurple;
            Item.ResearchUnlockCount = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                {
                    if (player.usedAegisCrystal)
                        line.Text = line.Text + "\n[c/b225e6: Vital Crystal]";
                    if (player.usedAegisFruit)
                        line.Text = line.Text + "\n[c/b225e6: Aegis Fruit]";
                    if (player.usedArcaneCrystal)
                        line.Text = line.Text + "\n[c/b225e6: Arcane Crystal]";
                    if (player.usedAmbrosia)
                        line.Text = line.Text + "\n[c/b225e6: Ambrosia]";
                    if (player.usedGummyWorm)
                        line.Text = line.Text + "\n[c/b225e6: Gummy Worm]";
                    if (player.usedGalaxyPearl)
                        line.Text = line.Text + "\n[c/b225e6: Galaxy Pearl]";
                    if (player.ateArtisanBread)
                        line.Text = line.Text + "\n[c/b225e6: Artisan Loaf]";
                    if (player.GetModPlayer<PlayerUpgrades>().ZephyrFeatherUpgrade)
                        line.Text = line.Text + "\n[c/b225e6: Zephyr Feather]";
                    if (!player.usedAegisCrystal && !player.usedAegisFruit && !player.usedArcaneCrystal && !player.usedAmbrosia
                            && !player.usedGummyWorm && !player.usedGalaxyPearl && !player.ateArtisanBread && !player.GetModPlayer<PlayerUpgrades>().ZephyrFeatherUpgrade)
                        line.Text = line.Text + "\n[c/888888: No permanent effects active]";
                }

            }
        }
        public override bool CanUseItem(Player player)
        {
            if (player.usedAegisCrystal || player.usedAegisFruit || player.usedArcaneCrystal || player.usedAmbrosia || player.usedGummyWorm || player.usedGalaxyPearl || player.ateArtisanBread || player.GetModPlayer<PlayerUpgrades>().ZephyrFeatherUpgrade)
                return true;
            else
                return false;
        }
        public override bool? UseItem(Player player)
        {
            if (player.usedAegisCrystal)
                Item.NewItem(new EntitySource_Loot(player), new Vector2(player.position.X, player.position.Y), new Vector2(player.width, player.height), ItemID.AegisCrystal);

            if (player.usedAegisFruit)
                Item.NewItem(new EntitySource_Loot(player), new Vector2(player.position.X, player.position.Y), new Vector2(player.width, player.height), ItemID.AegisFruit);
            if (player.usedArcaneCrystal)
                Item.NewItem(new EntitySource_Loot(player), new Vector2(player.position.X, player.position.Y), new Vector2(player.width, player.height), ItemID.ArcaneCrystal);
            if (player.usedAmbrosia)
                Item.NewItem(new EntitySource_Loot(player), new Vector2(player.position.X, player.position.Y), new Vector2(player.width, player.height), ItemID.Ambrosia);
            if (player.usedGummyWorm)
                Item.NewItem(new EntitySource_Loot(player), new Vector2(player.position.X, player.position.Y), new Vector2(player.width, player.height), ItemID.GummyWorm);
            if (player.usedGalaxyPearl)
                Item.NewItem(new EntitySource_Loot(player), new Vector2(player.position.X, player.position.Y), new Vector2(player.width, player.height), ItemID.GalaxyPearl);
            if (player.ateArtisanBread)
                Item.NewItem(new EntitySource_Loot(player), new Vector2(player.position.X, player.position.Y), new Vector2(player.width, player.height), ItemID.ArtisanLoaf);
            if (player.GetModPlayer<PlayerUpgrades>().ZephyrFeatherUpgrade)
                Item.NewItem(new EntitySource_Loot(player), new Vector2(player.position.X, player.position.Y), new Vector2(player.width, player.height), ModContent.ItemType<ZephyrFeather>());

            player.usedAegisCrystal = false;
            player.usedAegisFruit = false;
            player.usedArcaneCrystal = false;
            player.usedAmbrosia = false;
            player.usedGummyWorm = false;
            player.usedGalaxyPearl = false;
            player.ateArtisanBread = false;
            player.GetModPlayer<PlayerUpgrades>().ZephyrFeatherUpgrade = false;

            for (int i = 0; i < 30; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, 3).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(new Vector2(player.Center.X + (15 * player.direction), player.Center.Y - 20 * player.gravDir), 0, 0, 67, perturbedSpeed.X, perturbedSpeed.Y, 0, default, 1f);
                Main.dust[dust2].noGravity = true;
            }

            return true;
        }
    }
    //recipe in newrecipesandshops
}