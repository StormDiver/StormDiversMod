using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Buffs;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;

namespace StormDiversMod.NPCs

{
    public class SuperPainDummy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Super Pain Dummy");
            NPCID.Sets.CantTakeLunchMoney[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 5;

            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true // Hides this NPC from the bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
        }
        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 48;

            NPC.aiStyle = -1;

            NPC.noTileCollide = false;

            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 999999999; //999 Million
            NPC.noGravity = false;
            
            NPC.HitSound = SoundID.NPCHit15;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.gfxOffY = -2;
           
        }
        public override void ModifyTypeName(ref string typeName)
        {
            if (NPC.ai[0] == 0)
                typeName = "Standard " + typeName;

            else if (NPC.ai[0] == 1)
                typeName = "Floating " + typeName;

            else if (NPC.ai[0] == 2)
                typeName = "Tough " + typeName;

            else if (NPC.ai[0] == 3)
                typeName = "Broken " + typeName;

            else if (NPC.ai[0] == 4)
                typeName = "Light " + typeName;
        }
        bool die;
        public override void AI()
        {
            //Ai 0 = Normal
            //Ai 1 = Floating
            //Ai 2 = Tough
            //Ai 3 = Broken (Less health, no regen)
            //Ai 4 = Weak (No KB resist)

            if (NPC.ai[0] == 1)
            {
                NPC.velocity.Y = 0;
                NPC.noGravity = true;
            }
            if (NPC.ai[0] == 2)
            {
                NPC.defense = 50;
            }
            if (NPC.ai[0] == 3 && NPC.life > 50000)
            {
                NPC.lifeMax = 50000;
                NPC.life = 50000;
            }
            if (NPC.ai[0] == 4)
            {
                NPC.knockBackResist = 1;
                if (NPC.velocity.Y == 0)
                    NPC.velocity.X *= 0.95f;
            }

            Player player = Main.LocalPlayer;
            if (player.controlUseTile && (player.HeldItem.type == ModContent.ItemType<Items.Tools.SuperPainDummyItem>()) && player.noThrow == 0)
            {
                die = true;
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC bosscheck = Main.npc[i];

                if (bosscheck.active && bosscheck.boss)
                 die = true;
            }
            if (die)
            {
                NPC.life -= 2147483647;
                SoundEngine.PlaySound(SoundID.NPCDeath6, NPC.Center);

                for (int i = 0; i < 2; i++)
                {
                    int goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                }
            }
        }
        public override void UpdateLifeRegen(ref int damage)
        {
            if (NPC.ai[0] is 0 or 1 or 2 or 4)
                NPC.lifeRegen += 50000;
        }
        int npcframe = 0;

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = npcframe * frameHeight;

            if (NPC.ai[0] == 0)
            npcframe = 0;
            else if (NPC.ai[0] == 1)
                npcframe = 1;
            else if(NPC.ai[0] == 2)
                npcframe = 2;
            else if(NPC.ai[0] == 3)
                npcframe = 3;
            else if (NPC.ai[0] == 4)
                npcframe = 4;
        }
        
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            if (NPC.life <= 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    int goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                }

            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (NPC.ai[0] == 3)
                return true;
            else
                return false;
        }
    }
}