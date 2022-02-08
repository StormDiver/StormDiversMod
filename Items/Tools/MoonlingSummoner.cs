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


namespace StormDiversMod.Items.Tools
{
    public class MoonlingSummoner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonling Core");
            Tooltip.SetDefault("Summons a Moonling that will try to kill you\nSafer to use near solid ground");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning Item.
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;


        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useTurn = false;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            ItemID.Sets.ItemNoGravity[Item.type] = true;
            //ItemID.Sets.ItemIconPulse[Item.type] = true;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool CanUseItem(Player player)
        {
            return NPC.downedMoonlord && !NPC.AnyNPCs(ModContent.NPCType<NPCs.MoonDerp>());
            //return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                //Main.NewText("The Storm God has awoken!", 175, 75, 255);
                SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
                for (int i = 0; i < 50; i++)
                {

                    Vector2 vel = new Vector2(Main.rand.NextFloat(20, 20), Main.rand.NextFloat(-20, -20));
                    int dust2 = Dust.NewDust(new Vector2(player.Center.X - 5, player.Top.Y), 10, 10, 229, 0f, 0f, 200, default, 0.8f);
                    Main.dust[dust2].velocity *= 2f;
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].scale = 1.5f;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.MoonDerp>());

                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: ModContent.NPCType<NPCs.MoonDerp>());
                }
            }
           
            return true;
        }
      
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 255;
            return color;

        }
    }
}