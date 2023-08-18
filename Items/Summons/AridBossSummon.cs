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

namespace StormDiversMod.Items.Summons
{
    public class AridBossSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cracked Horn");
            //Tooltip.SetDefault("Seems to be cursed, use with caution");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item
            Item.ResearchUnlockCount = 3;
        }     
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 60;
            Item.useAnimation = 60;
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
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {

                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\n[c/ffa500:Fairy Buggy on multiplayer!]\n[c/ffa500:For the best experience fight the boss on single player!]"; //multiplayer sucks
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
            return (Main.player[Player.FindClosest(player.position, player.width, player.height)].ZoneDesert || Main.player[Player.FindClosest(player.position, player.width, player.height)].ZoneUndergroundDesert) && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.AridBoss>());
            //return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 100; i++)
                {
                    float speedX = 0f;
                    float speedY = -5f;

                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                   
                        int dust2 = Dust.NewDust(new Vector2(player.Center.X + (25 * player.direction), player.Center.Y - 16 * player.gravDir), 0, 0, 138, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);
                        Main.dust[dust2].noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.Item14, player.Center);
                SoundEngine.PlaySound(SoundID.Roar, player.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Boss.AridBoss>());

                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: ModContent.NPCType<NPCs.Boss.AridBoss>());

                }
            }

            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            
            SoundEngine.PlaySound(SoundID.Item14, player.Center);

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Bone, 40)
            .AddIngredient(ItemID.FossilOre, 15)
            .AddTile(TileID.Anvils)
            .Register();

        }
    }

  
    
}