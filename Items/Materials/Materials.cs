using Microsoft.Xna.Framework;
using StormDiversMod.Common;
using StormDiversMod.Items.Ammo;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace StormDiversMod.Items.Materials
{
    public class ChaosShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Chaos Orb");

            //Tooltip.SetDefault("'Imbued with pure chaos'");

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 5));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 25;
        }
        public override bool GrabStyle(Player player)
        {
            float range = 8f;
            Vector2 vectorItemToPlayer = player.Center - Item.Center;
            float distanceToPlayer = Vector2.Distance(player.Center, Item.Center);
            float InverseDistanceToPlayer = range / distanceToPlayer;
            Item.velocity = Item.velocity + -vectorItemToPlayer * InverseDistanceToPlayer * .02f;
            Item.velocity = Collision.TileCollision(Item.position, Item.velocity, Item.width, Item.height);
            return true;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.LightRed;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }

        public override void AddRecipes()
        {
            //VanillaRecipes   
        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //____________________________________________________________________________________
    public class GraniteCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Granite Power Cell");

            //Tooltip.SetDefault("Seems to be pulsing with energy");
            Item.ResearchUnlockCount = 15;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 20;
            Item.maxStack = 9999;
            ItemID.Sets.ItemIconPulse[Item.type] = true;

            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.Blue;
        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
            }
        }
        public override void AddRecipes()
        {

        }
    }
    //____________________________________________________________________________________
    public class RedSilk : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Warrior Cloth");
            //Tooltip.SetDefault("Used to create Items of a fallen Gladiator");
            Item.ResearchUnlockCount = 15;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            //NewRecipes
        }
    }
    //____________________________________________________________________________________
    public class BlueCloth : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Insulated Fabric");
            //Tooltip.SetDefault("Can be used to keep warm");
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 9999;

            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            //VanillaRecipes
        }
    }
    //____________________________________________________________________________________
    public class DerplingShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Shell");
            //Tooltip.SetDefault("'Tough, but malleable'");
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Lime;
        }
    }
    //____________________________________________________________________________________

    public class CrackedHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Broken Heart");

            //Tooltip.SetDefault("'Almost devoid of life'");
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<Items.Misc.CleansingHeart>();
        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.2f * Main.essScale);
            }
            if (Item.lavaWet && Item.velocity.Y > -2f)
                Item.velocity.Y -= 0.25f;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Orange;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //____________________________________________________________________________________
    public class BloodDrop : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloody Drop");

            //Tooltip.SetDefault("A drop of blood that's somehow able to hold its shape");
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 20;
            Item.maxStack = 9999;

            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {

        }
    }
    //____________________________________________________________________________________
    public class SpaceRock : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Fragment");

            //Tooltip.SetDefault("Seems to be infused with some strange energy");

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.rare = ItemRarityID.Cyan;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }

        public override void AddRecipes()
        {

        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //_____________________________________
    public class SoulFire : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Infernal Soul Flame");

            //Tooltip.SetDefault("A soul that never stops burning");

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Item.ResearchUnlockCount = 25;

        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.LightPurple;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void AddRecipes()
        {

        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 150;
            return color;
        }
    }
    //____________________________________________________________________________________
    public class IceOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Shard");

            //Tooltip.SetDefault("Retrieved from the depths of the frozen caves");
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.maxStack = 9999;

            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Pink;
        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
            }
        }
    }
    //_________________________________________________________________________________________________
    public class DesertOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Shard");
            //Tooltip.SetDefault("Retrieved from the depths of the deserted caves");
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Pink;
        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
            }

        }
    }
    //_________________________________________________________________________________________________
    public class SantankScrap : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mechanical Scrap");
            //Tooltip.SetDefault("All that remains of the mighty Santank");
            Item.ResearchUnlockCount = 25;

        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Yellow;

        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
            }
        }
    }
    //_________________________________________________________________________________________________
    public class GlassArmourShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Broken Armour Shard");
            //Tooltip.SetDefault("Well it worked until you got hit");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.IgnoresEncumberingStone[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Gray;
            Item.noGrabDelay = 0;
        }
        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange *= 3;
        }
        public override bool GrabStyle(Player player)
        {
            Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(Item.Center.X, Item.Center.Y)) * 15;
            Item.velocity = velocity;
            return true;
        }
        public override void PostUpdate()
        {
           
        }
    }
    //_____________________________________________________________
    public class PainCoin : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pain Coin");

            //Tooltip.SetDefault("'Used to create Pain Tickets'");
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 6));
            Item.ResearchUnlockCount = -1;
            ItemID.Sets.ShimmerTransformToItem[Item.type] = ItemID.SilverCoin;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var player = Main.LocalPlayer;

            foreach (TooltipLine line in tooltips)
            {
                if (!GetInstance<ConfigurationsGlobal>().NoGacha4U)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        if (player.GetModPlayer<PlayerUpgrades>().GachaCoinDropCount < 30)
                            line.Text = "[c/34f60d:Enemy Drop Limit = " + player.GetModPlayer<PlayerUpgrades>().GachaCoinDropCount + "/30]";
                        else
                            line.Text = "[c/F54927:Enemy Drop Limit = " + player.GetModPlayer<PlayerUpgrades>().GachaCoinDropCount + "/30]";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                    {
                        line.Text = "[c/efd127:Enemy Drop Limit reset in " + player.GetModPlayer<PlayerUpgrades>().GachaCoinDropCooldown / 60 + " Seconds]";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                    {
                        if (player.GetModPlayer<PlayerUpgrades>().GachaCoinDropBossCount < 3)
                            line.Text = "[c/34f60d:Boss Drop Limit = " + player.GetModPlayer<PlayerUpgrades>().GachaCoinDropBossCount + "/3]";
                        else
                            line.Text = "[c/F54927:Boss Drop Limit = " + player.GetModPlayer<PlayerUpgrades>().GachaCoinDropBossCount + "/3]";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip5")
                    {
                        line.Text = "[c/efd127:Boss Drop Limit Reset in " + player.GetModPlayer<PlayerUpgrades>().GachaCoinDropBossCooldown / 60 + " Seconds]";
                    }
                }
                else
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        line.Text = "[c/7D7D7D:Drops Disabled]";
                    }
                    if (line.Mod == "Terraria" && (line.Name == "Tooltip3" || line.Name == "Tooltip4" || line.Name == "Tooltip5"))
                    {
                        line.Text = "";
                    }
                }
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<PainCoin>(), 5);
            recipe.AddIngredient(ItemID.GoldCoin, 1);
            
            recipe.Register();
        }
        public override void PostUpdate()
        {
            // Spawn some light and dust when dropped in the world
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.4f);
            }
            if (Item.timeSinceItemSpawned % 12 == 0)
            {
                Vector2 center = Item.Center + new Vector2(0f, Item.height * -0.1f);

                // This creates a randomly rotated vector of length 1, which gets it's components multiplied by the parameters
                Vector2 direction = Main.rand.NextVector2CircularEdge(Item.width * 0.6f, Item.height * 0.6f);
                float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

                Dust dust = Dust.NewDustPerfect(center + direction * distance, DustID.SilverFlame, velocity);
                dust.scale = 0.5f;
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.alpha = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
}
