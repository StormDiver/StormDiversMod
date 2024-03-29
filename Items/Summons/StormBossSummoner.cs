﻿using System;
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
    public class StormBossSummoner : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Storm Beacon");
            //Tooltip.SetDefault("Signals a powerful foe");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item.
            Item.ResearchUnlockCount = 3;


        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\n[c/00FFA5:The Signal appears to be blocked by the souls of the mechanical bosses]"; //Unusable pre mechs
                    }
                }
                /*if (Main.netMode == NetmodeID.MultiplayerClient)
                {

                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\n[c/ffa500:Fairy Buggy on multiplayer!]\n[c/ffa500:For the best experience fight the boss on single player!]"; //multiplayer sucks
                    }
                }*/
            }
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Lime;
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
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool CanUseItem(Player player)
        {
            return NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.StormBoss>());
            //return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 50; i++)
                {

                    int dust2 = Dust.NewDust(new Vector2(player.Center.X - 5, player.Top.Y), 10, 10, 229, 0f, 0f, 200, default, 0.8f);
                    Main.dust[dust2].velocity *= 2f;
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].scale = 1.5f;
                }
                SoundEngine.PlaySound(SoundID.Roar, player.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Boss.StormBoss>());

                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: ModContent.NPCType<NPCs.Boss.StormBoss>());

                }
            }

            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            for (int i = 0; i < 5; i++)
            {
               
                    Vector2 rotation = -player.Center + (player.Center + new Vector2(0, -250 * player.gravDir));

                    float ai = Main.rand.Next(100);

                    int projID = Projectile.NewProjectile(source, new Vector2(player.Center.X + (15 * player.direction), player.Center.Y - 20 * player.gravDir), new Vector2(0, -8 * player.gravDir),
                        ModContent.ProjectileType<Projectiles.StormLightningProj>(), 0, .5f, player.whoAmI, rotation.ToRotation(), ai);
                    Main.projectile[projID].damage = 0;
                    Main.projectile[projID].tileCollide = false;

                    Main.projectile[projID].timeLeft = 190;
            }
            SoundEngine.PlaySound(SoundID.Item122, player.Center);

            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 255;
            return color;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.FallenStar, 5)
           .AddIngredient(ItemID.SoulofMight, 1)
           .AddIngredient(ItemID.SoulofSight, 1)
           .AddIngredient(ItemID.SoulofFright, 1)

           .AddTile(TileID.MythrilAnvil)
           .Register();
        }
    }

  
    
}