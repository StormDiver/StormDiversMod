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

namespace StormDiversMod.Items.Summons
{
    public class UltimateBossSummoner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mysterious Emblem");
            Tooltip.SetDefault("Summons the ultimate boss, make sure you're prepared for all the pain.");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }     
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 5, 0, 0);
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
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {

                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\n[c/ffa500:Very Buggy on multiplayer!]\n[c/ffa500:For the best experience fight the boss on single player!]"; //multiplayer sucks
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
            return NPC.downedMoonlord && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.ThePainBoss>());
            //return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.Center);
                if (player.gravDir == 1)
                {
                    int proj = Projectile.NewProjectile(null, new Vector2(player.Center.X + (25 * player.direction), player.Top.Y - 4), new Vector2(0, 0), ModContent.ProjectileType<ThePainBossProj2>(), 0, 0, Main.myPlayer);
                }
                else
                {
                    int proj = Projectile.NewProjectile(null, new Vector2(player.Center.X + (25 * player.direction), player.Bottom.Y + 4), new Vector2(0, 0), ModContent.ProjectileType<ThePainBossProj2>(), 0, 0, Main.myPlayer);

                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Boss.ThePainBoss>());

                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: ModContent.NPCType<NPCs.Boss.ThePainBoss>());
                }
            }

            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {   
            SoundEngine.PlaySound(SoundID.Item14, player.Center);
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Summons/UltimateBossSummoner_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarBar, 4)
            .AddIngredient(ItemID.FragmentSolar, 2)
            .AddIngredient(ItemID.FragmentVortex, 2)
            .AddIngredient(ItemID.FragmentNebula, 2)
            .AddIngredient(ItemID.FragmentStardust,2)

            .AddTile(TileID.LunarCraftingStation)
            .Register();

        }
    }

  
    
}