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
using Terraria.DataStructures;
using StormDiversMod.Basefiles;

namespace StormDiversMod.NPCs

{
    public class SuperPainDummy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Super Pain Dummy");
            NPCID.Sets.CantTakeLunchMoney[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 20;

            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
            NPCID.Sets.MPAllowedEnemies[Type] = true;
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
            switch (NPC.ai[0])
            {
                case 0:
                    typeName = "Standard " + typeName;
                    break;
                case 1:
                    typeName = "Tough " + typeName;
                    break;
                case 2:
                    typeName = "Broken " + typeName;
                    break;
                case 3:
                    typeName = "Light " + typeName;
                    break;
                case 4:
                    typeName = "Chonky " + typeName;
                    break;
            }
            switch (NPC.ai[1])
            {
                case 0:
                    typeName = "Grounded " + typeName;
                    break;
                case 1:
                    typeName = "Floating " + typeName;
                    break;
            }
        }
        bool die;
        int hurttime;
        int extraframe;
        float yheight;
        string spawntext;
        public override void OnSpawn(IEntitySource source)
        {
            switch (Main.rand.Next(6))
            {
                case 0:
                    spawntext = "Existence is Pain!";
                    break;
                case 1:
                    spawntext = "Time for me to suffer!";
                    break;
                case 2:
                    spawntext = "How much pain is there?";
                    break;
                case 3:
                    spawntext = "I'm Mr.PainDummy, look at me!!";
                    break;
                case 4:
                    spawntext = "Why must you bring me into this world?";
                    break;
                case 5:
                    spawntext = "ThePain!";
                    break;
            }

            if (!GetInstance<ConfigurationsIndividual>().NoPain)
            {
                SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/PainSound") with { Volume = 1f, MaxInstances = 5, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
            }
            CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.DeepPink, spawntext, false);

            yheight = NPC.position.Y; //Set spawn Y postion for floating light
            //set frame for each type
            extraframe = (int)(NPC.ai[0] * 4); // 0 for standard, 4 for tough, 8 for broken, 12 for light, 16 for chonky

            if (NPC.ai[1] == 1) //extra 2 frames if Floating
                extraframe += 2;
           
            if (NPC.ai[0] == 2) //Broken
            {
                switch (NPC.ai[2]) //look Ma, switch cases instead of if statments!
                {
                    case 0:
                        NPC.lifeMax = 5000;
                        NPC.life = 5000;
                        break;
                    case 1:
                        NPC.lifeMax = 10000;
                        NPC.life = 10000;
                        break;
                    case 2:
                        NPC.lifeMax = 25000;
                        NPC.life = 25000;
                        break;
                    case 3:
                        NPC.lifeMax = 50000;
                        NPC.life = 50000;
                        break;
                }
            }
        }
        public override bool? CanFallThroughPlatforms()
        {
            if (NPC.ai[1] is 1) //Fall through if floating
                return true;
            else
                return false;
        }
        public override void AI()
        {
            //Ai 0 0 = Normal
            //Ai 0 1 = Tough
            //Ai 0 2 = Broken (Less health, no regen)
            //Ai 0 3 = Light (No KB resist)
            //Ai 0 4 = Chonky (reflects projects)


            //Ai 1 0 = Grounded
            //Ai 1 1 = Floating

            if (NPC.ai[0] == 1) //Tough
            {
                switch (NPC.ai[2])
                {
                    case 0:
                        NPC.defense = 5;
                        break;
                    case 1:
                        NPC.defense = 10;
                        break;
                    case 2:
                        NPC.defense = 25;
                        break;
                    case 3:
                        NPC.defense = 50;
                        break;
                }
            }
            //broken variant in spawn hook
            if (NPC.ai[0] == 3 && NPC.ai[1] == 0) //Light grounded
            {
                switch (NPC.ai[2])
                {
                    case 0:
                        NPC.knockBackResist = 1;
                        if (NPC.velocity.Y == 0)
                            NPC.velocity.X *= 0.9f;
                        break;
                    case 1:
                        NPC.knockBackResist = 0.75f;
                        if (NPC.velocity.Y == 0)
                            NPC.velocity.X *= 0.92f;
                        break;
                    case 2:
                        NPC.knockBackResist = 0.5f;
                        if (NPC.velocity.Y == 0)
                            NPC.velocity.X *= 0.94f;
                        break;
                    case 3:
                        NPC.knockBackResist = 0.25f;
                        if (NPC.velocity.Y == 0)
                            NPC.velocity.X *= 0.96f;
                        break;
                }
            }

            if (NPC.ai[1] == 1 && NPC.ai[0] != 3) //Non light floating
            {
                NPC.velocity.Y = 0;
                NPC.noGravity = true;
            }
            else if (NPC.ai[1] == 1 && NPC.ai[0] == 3) //Light Floating
            {
                switch (NPC.ai[2])
                {
                    case 0:
                        NPC.knockBackResist = 1;
                        break;
                    case 1:
                        NPC.knockBackResist = 0.75f;
                        break;
                    case 2:
                        NPC.knockBackResist = 0.5f;
                        break;
                    case 3:
                        NPC.knockBackResist = 0.25f;
                        break;
                }

                NPC.velocity *= 0.98f;
                NPC.noGravity = true;

                if (NPC.position.Y < (yheight - 95) && hurttime == 0) //move down to orignal ypos
                {
                    if (NPC.velocity.Y < 2)
                        NPC.velocity.Y += 0.1f;
                }
            }
            if (NPC.ai[0] == 4) //Chonky
            {
                NPC.reflectsProjectiles = true;
            }

            Player player = Main.LocalPlayer;
            if (player.controlUseTile && (player.HeldItem.type == ModContent.ItemType<Items.Tools.SuperPainDummyItem>()) && player.noThrow == 0)
            {
                die = true;
            }
            if (player.position.X <= NPC.position.X)//face the player
                NPC.spriteDirection = -1;
            else
                NPC.spriteDirection = 1;

            for (int i = 0; i < Main.maxNPCs; i++) //remove if boss is active
            {
                NPC bosscheck = Main.npc[i];

                if (bosscheck.active && bosscheck.boss)
                    die = true;
            }
            if (die)//rip
            {
                switch (Main.rand.Next(6))
                {
                    case 0:
                        spawntext = "Free at last!";
                        break;
                    case 1:
                        spawntext = "No more pain for me!";
                        break;
                    case 2:
                        spawntext = "RIP in peace me!";
                        break;
                    case 3:
                        spawntext = "I fufilled my purpose!";
                        break;
                    case 4:
                        spawntext = "It's finally over!";
                        break;
                    case 5:
                        spawntext = "ThePain no more!";
                        break;
                }

                CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.DeepPink, spawntext, false);
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
            if (NPC.ai[0] != 3 || (NPC.ai[0] == 3 && NPC.velocity.Y == 0 && NPC.ai[1] == 0) || (NPC.ai[0] == 3 && (NPC.velocity.X <= 0.5f && NPC.velocity.X >= -0.5f) && NPC.ai[1] == 1)) // light one only changes to normal once landed on ground, or when x is slower than 0.5
            {
                if (hurttime > 0)
                    hurttime--;
            }

        }
        public override void UpdateLifeRegen(ref int damage)
        {
            if (NPC.ai[0] is 0 or 1 or 3 or 4) //Standard, Tough, Light, and Chonky have regen
                NPC.lifeRegen += 100000;
        }
        int npcframe = 0;

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = npcframe * frameHeight;

            if (hurttime == 0) //when hurt is displays the pained frame for 60 frames (60 frames after landing on the ground for light)
                npcframe = 0 + extraframe;
            else
                npcframe = 1 + extraframe;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            hurttime = 60;
            
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            if (NPC.life <= 0)
            {
                CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.DeepPink, "Freedom!", false);

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
            if (NPC.ai[0] == 2) //Only broken has health bar
                return true;
            else
                return false;
        }
    }
}