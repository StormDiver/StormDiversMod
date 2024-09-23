using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.NPCs;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.Generation;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Items.Weapons;
using StormDiversMod.Items.Pets;
using StormDiversMod.NPCs.NPCProjs;
using StormDiversMod.Items.Vanitysets;
using StormDiversMod.Common;

namespace StormDiversMod.Items.Summons
{
    public class UltimateBossSummoner : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mysterious Skull");
            //Tooltip.SetDefault("Summons the Painbringer, make sure you're prepared for all the pain");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item
            Item.ResearchUnlockCount = 3;
        }     
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            Item.shoot = ModContent.ProjectileType<Projectiles.StormLightningProj>();
            //ItemID.Sets.ItemIconPulse[Item.type] = true;
        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (Main.remixWorld)
                { 

                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "Summons the.... wait this doesn't seem right, there's too much pain!"; 
                    }
                }
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool CanUseItem(Player player)
        {
            if (Main.remixWorld)
                return NPC.downedMoonlord && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.ThePainBoss>());
            else
                return NPC.downedMoonlord && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.TheUltimateBoss>());
            //return true;
        }
        String Paintext = "";
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Main.remixWorld)
                        NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Boss.ThePainBoss>());
                    else
                        NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Boss.TheUltimateBoss>());
                }
                else
                {
                    if (Main.remixWorld)
                        NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: ModContent.NPCType<NPCs.Boss.ThePainBoss>());
                    else
                        NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: ModContent.NPCType<NPCs.Boss.TheUltimateBoss>());
                }
               
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, new Vector2(player.Center.X + (25 * player.direction), player.Center.Y - 16 * player.gravDir), new Vector2(0, 0), ModContent.ProjectileType<TheUltimateBossProj4>(), 0, 0, Main.myPlayer);

            SoundEngine.PlaySound(SoundID.Item14, player.Center);
            return false;
        }

        /*public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<ThePainMask>(), 1)
            .AddIngredient(ItemID.LunarBar, 5)

            .AddTile(TileID.LunarCraftingStation)
            .Register();

        }*/
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 255;
            return color;

        }
    }
}