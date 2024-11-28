using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using StormDiversMod.Common;
using Terraria.DataStructures;
using StormDiversMod.Items.BossTrophy;
using StormDiversMod.Items.Weapons;
using StormDiversMod.NPCs;
using System.IO;
using Terraria.GameContent.Generation;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using StormDiversMod.Buffs;
using StormDiversMod.Items.Pets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static Terraria.ModLoader.PlayerDrawLayer;
using System.Configuration;
using StormDiversMod.Projectiles;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;


namespace StormDiversMod.NPCs.Boss
{
    [AutoloadBossHead]
    public class TheUltimateBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Painbringer"); // Automatic from .lang files
                                                     // make sure to set this for your modnpcs.
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 32;

            NPC.width = 78;
            NPC.height = 130;

            NPC.aiStyle = -1;

            NPC.noGravity = true;
            NPC.damage = 250; //250/350/450

            NPC.defense = 100;
            NPC.lifeMax = 300000;
            NPC.gfxOffY = 0;

            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath62; 
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(1, 0, 0, 0);
            NPC.boss = true;
            if (!Main.dedServ)
            {
                //Music = MusicID.LunarBoss;
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/UltimateBossMusic");
            }
            NPC.npcSlots = 10f;
            NPC.noTileCollide = true;
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            NPC.dontTakeDamage = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.

				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("An entity bent on inflicting as much pain as possible to whoever it finds, using its skulls of judgement to deliver the pain. " +
                "Even when near death it won’t stop finding ways to inflict pain, even if it causes pain upon itself.")
            });
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            //300K Classic
            if (!Main.masterMode)
            {
                NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * balance); //450000
            }
            else
            {
                NPC.lifeMax = (int)(NPC.lifeMax * 0.67f * balance - 1000); //600000
            }
            //250/350/450
            NPC.damage = (int)(NPC.damage * 0.75f);

            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                if ((bool)calamityMod.Call("GetDifficultyActive", "death"))
                {
                    NPC.lifeMax = (int)(NPC.lifeMax * 1.5f);
                    NPC.damage = (int)(NPC.damage * 1.66f);
                }
                else if ((bool)calamityMod.Call("GetDifficultyActive", "revengeance"))
                {
                    NPC.lifeMax = (int)(NPC.lifeMax * 1.25f);
                    NPC.damage = (int)(NPC.damage * 1.33f);
                }
            }
        }
        float speed = 30;
        float inertia = 20; //movement for inertia attacks

        float movespeed = 1; //movespeed for non-inertia attacks

        float distanceToIdlePosition; //distance to movement positon
        float distance; //Distance to player

        int projdamage; //Damage of all projectiles
        int projcount; //Number of projs if applicable
        float projvelocity; //Velocity of projectiles

        int lifeleft; //0 = phase 1 1 = 3 phase 2, 2 = phase 3, 3 = Phase 4 and 5
        int animation;
        bool deathani;
        string Paintext; //The text in chat and over the boss
        bool Zenithtext;
        bool shooting; //should the attack use the shooting animation or not
        int projspread; //spread for attack 10
        int teleporttime; // time till teleport
        int clamteadmg = 100;
        Player player;

        public static int phase2HeadSlot = -1;
        public static int phase3HeadSlot = -1;
        public static int phase4HeadSlot = -1;
        public static int phase5HeadSlot = -1;

        public override void Load()
        {
            string texture = BossHeadTexture + "_Phase2"; // Texture Name
            phase2HeadSlot = Mod.AddBossHeadTexture(texture, -1); // -1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise

            string texture2 = BossHeadTexture + "_Phase3"; // Texture Name
            phase3HeadSlot = Mod.AddBossHeadTexture(texture2, -1); // -1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise

            string texture3 = BossHeadTexture + "_Phase4"; // Texture Name
            phase4HeadSlot = Mod.AddBossHeadTexture(texture3, -1); // -1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise

            string texture4 = BossHeadTexture + "_Phase5"; // Texture Name
            phase5HeadSlot = Mod.AddBossHeadTexture(texture4, -1); // -1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise
        }
        public override void BossHeadSlot(ref int index)
        {
            int slot = phase2HeadSlot;
            if (lifeleft == 1 && slot != -1)
            {
                // If the boss is in its second stage, display the other head icon instead
                index = slot;
            }
            int slot2 = phase3HeadSlot;
            if (lifeleft == 2 && slot2 != -1)
            {
                // If the boss is in its third stage, display the 3rd icon instead
                index = slot2;
            }
            int slot3 = phase4HeadSlot;
            if (lifeleft == 3 && NPC.ai[3] != 10 && slot3 != -1)
            {
                // If the boss is in its fourth stage, display the 4th icon instead
                index = slot3;
            }
            int slot4 = phase5HeadSlot;
            if ((lifeleft == 3 && NPC.ai[3] == 10 || deathani) && slot4 != -1)
            {
                // If the boss is in its fifth stage, display the 5th icon instead
                index = slot4;
            }
        }
        public override bool CheckDead() //For death animation
        {
            if (!deathani)
            {
                NPC.ai[3] = 0;
                NPC.ai[0] = 0;
                NPC.localAI[0] = 0;//Reset all ai values

                NPC.damage = 0;
                NPC.life = 1;
                //NPC.life = 100;
                NPC.dontTakeDamage = true;
                NPC.netUpdate = true;
                deathani = true;
                return false;
            }
            return true;
        }

        public override void AI()
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                if ((bool)calamityMod.Call("GetDifficultyActive", "death"))
                {
                    clamteadmg = 166; //1.66x
                }
                else if ((bool)calamityMod.Call("GetDifficultyActive", "revengeance"))
                {
                    clamteadmg = 133; //1.33x
                }
                else
                {
                    clamteadmg = 100; //1x
                }
            }
            if ((NPC.ai[3] == 9 && NPC.ai[0] >= 120) || NPC.ai[3] == 10) //Gain contact damage on last phases, after a small delay at first to prevent it touching the player
            {
                if (Main.getGoodWorld)
                    NPC.damage = (450 * clamteadmg) / 100;
                else if (Main.masterMode)
                    NPC.damage = (350 * clamteadmg) / 100;
                else if (Main.expertMode && !Main.masterMode)
                    NPC.damage = (250 * clamteadmg) / 100;
                else
                    NPC.damage = (150 * clamteadmg) / 100;

                //Main.NewText("Can damage", Color.Green);
            }
            else
            {
                NPC.damage = 0;
                //Main.NewText("Cannot damage", Color.Red);
            }

            if (Main.netMode != NetmodeID.Server)
            {
                // For visuals regarding NPC position, netOffset has to be concidered to make visuals align properly
                NPC.position += NPC.netOffset;
                NPC.position -= NPC.netOffset;
            }
            //===============AI fields================
            //NPC.ai[0] = time until next attack (+ Death animation timer)
            //NPC.ai[1] = X postion
            //NPC.ai[2] = Y postion
            //NPC.ai[3] = Phase
            //NPC.localAI[0] = Shootime
            //NPC.localAI[1] = death counter despawn
            //NPC.LocalAI[2 and 3] = (Attack dependant)

            //========================================
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            //======================================================MAIN MOVEMENT==============================================================================================

             player = Main.player[NPC.target]; //Code to move towards player
            distance = Vector2.Distance(player.Center, NPC.Center);

            if (!player.dead && !deathani && (NPC.ai[3] == 0 || NPC.ai[3] == 1 || NPC.ai[3] == 3 || NPC.ai[3] == 9 || NPC.ai[3] == 10)) //Inertia based movement
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 idlePosition = player.Center + new Vector2(NPC.ai[1], NPC.ai[2]);
                    Vector2 vectorToIdlePosition = idlePosition - NPC.Center;
                    distanceToIdlePosition = vectorToIdlePosition.Length();

                    //speed is dependant on attack
                    if (distanceToIdlePosition > 10f)
                    {
                        vectorToIdlePosition.Normalize();
                        vectorToIdlePosition *= speed;
                    }
                    NPC.velocity = (NPC.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                    NPC.netUpdate = true;
                }
            }
            if (!player.dead && !deathani && NPC.ai[3] != 0 && NPC.ai[3] != 1 && NPC.ai[3] != 3 && NPC.ai[3] != 9 && NPC.ai[3] != 10) //Non-inertia based movement
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 moveTo = player.Center;
                    Vector2 move = moveTo - NPC.Center + new Vector2(NPC.ai[1], NPC.ai[2]); //Postion around player
                    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                    if (magnitude > movespeed)
                    {
                        move *= movespeed / magnitude;
                    }
                    NPC.velocity = move;
                    NPC.netUpdate = true;

                }
            }
            NPC.rotation = NPC.velocity.X / 150;
            if (NPC.ai[3] is 1 or 3) //attacks 1 and 3 movement
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (NPC.localAI[3] == 0)//Loop round once then switch direction
                    {
                        NPC.localAI[2] += 1.5f;//rotation

                        if (NPC.localAI[2] >= 550)
                            NPC.localAI[3] = 1; //Rotate anti clockwise;
                    }
                    else
                    {
                        NPC.localAI[2] -= 1.5f;//rotation
                        if (NPC.localAI[2] <= 0)
                            NPC.localAI[3] = 0; //Rotate clockwise;
                    }

                    double deg = (NPC.localAI[2]);
                    double rad = deg * (Math.PI / 180);
                    double dist = 600; //Distance away from the player

                    //position

                    NPC.ai[1] = (int)(Math.Cos(rad) * dist);
                    NPC.ai[2] = (int)(Math.Sin(rad) * dist);
                    NPC.netUpdate = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (distanceToIdlePosition > 500f)
                    {
                        // Speed up the boss if it's away from the player
                        speed = 20f;
                        inertia = 20f;
                    }
                    else
                    {
                        // Slow down the boss if closer to the player
                        speed = 10f;
                        inertia = 35f;
                    }
                    NPC.netUpdate = true;
                }
            }

            if (distance > 1500 && NPC.ai[3] != 0)// teleport if far away
            {
                teleporttime++;

                for (int i = 0; i < 25; i++) //dust effect
                {
                    double deg = Main.rand.Next(0, 360); //The degrees
                    double rad = deg * (Math.PI / 180); //Convert degrees to radians
                    double dist = 120; //Distance away from the cursor
                    float dustx = player.Center.X - (int)(Math.Cos(rad) * dist);
                    float dusty = player.Center.Y - 400 - (int)(Math.Sin(rad) * dist);
                    {
                        var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 115, 0, 0, 50);
                        dust.noGravity = true;
                        dust.velocity *= 0;
                        dust.scale = 1.25f;
                    }
                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y - 400) - new Vector2(dustx, dusty)) * 25;
                    {
                        var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 115, velocity.X, velocity.Y, 50);
                        dust.noGravity = true;
                        dust.velocity *= 1f;
                        dust.scale = 1.5f;
                    }
                }
            }
            else
            {
                teleporttime = 0;
            }
            if (teleporttime >= 15)
            { 
                NPC.ai[0] -= 30;
                NPC.localAI[0] -= 30;
                NPC.Center = new Vector2(player.Center.X, player.Center.Y - 400);
                SoundEngine.PlaySound(SoundID.Item165 with { Volume = 1.5f, Pitch = 0.5f, MaxInstances = 1 }, NPC.Center);
                SoundEngine.PlaySound(SoundID.Item131 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1 }, NPC.Center);

                Dust.QuickDustLine(NPC.Center, new Vector2(NPC.oldPosition.X + NPC.width / 2, NPC.oldPosition.Y + NPC.height / 2), 152, Color.IndianRed); //centre to centre

                if (!GetInstance<ConfigurationsIndividual>().NoMessage)
                {
                    Paintext = "You cannot escape the pain that easily!";

                    CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.IndianRed, Paintext, true);
                    if (Main.netMode == 2) // Server
                    {
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.IndianRed);
                    }
                    else if (Main.netMode == 0) // Single Player
                    {
                        Main.NewText(Paintext, Color.IndianRed);
                    }
                }
                for (int i = 0; i < 150; i++)
                {
                    float speedY = -8f;
                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1.5f);
                    //Main.dust[dust2].noGravity = true;
                }

                teleporttime = 0;

            }
            /*if (distance > 20000 && NPC.ai[3] != 0)// Despawn if too far away
            {
                NPC.active = false;
            }*/
            if (player.dead && !deathani)//When player is dead slow down, mock the player, then fly away
            {
                if (NPC.localAI[1] == 60 && !GetInstance<ConfigurationsIndividual>().NoMessage)
                {
                    if (StormWorld.ultimateBossDown)
                    {
                        Paintext = "You handled it once before, but not this time!";
                    }
                    else if (Main.getGoodWorld)
                    {
                        Paintext = "You are not worthy!";
                    }
                    else
                    {
                        Paintext = "Guess you couldn't handle the Pain!";
                    }
                    CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.IndianRed, Paintext, true);
                    if (Main.netMode == 2) // Server
                    {
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.IndianRed);
                    }
                    else if (Main.netMode == 0) // Single Player
                    {
                        Main.NewText(Paintext, Color.IndianRed);
                    }
                }
                NPC.ai[3] = 0;
                if (NPC.localAI[1] > 120)
                {
                    if (NPC.velocity.Y < 25)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.velocity.Y -= 0.6f;
                            NPC.netUpdate = true;
                        }
                    }
                }
                else
                {
                    NPC.rotation = NPC.velocity.X / 50;
                    NPC.velocity *= 0.95f;
                }

                NPC.EncourageDespawn(60);

                NPC.localAI[1]++;
                if (NPC.localAI[1] > 300)
                {
                    NPC.active = false;
                }
            }
            else
            {
                NPC.localAI[1] = 0;
            }
            if (deathani) //DEATH ANIMATION=============================================================
            {    
                NPC.dontTakeDamage = true;
                NPC.ai[0]++;
                NPC.rotation = NPC.velocity.X / 50;
                NPC.velocity *= 0.95f;
                if (Main.rand.Next(8) == 0)
                {
                    int xprojpos = Main.rand.Next(-30, 30);
                    int yprojpos = Main.rand.Next(-65, 65);

                    int ProjID = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + xprojpos, NPC.Center.Y + yprojpos), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjExplode>(), 0, 0, Main.myPlayer);
                    Main.projectile[ProjID].scale = 0.6f;

                    for (int i = 0; i < 20; i++)
                    {
                        float speedY = -1f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(new Vector2(NPC.Center.X + xprojpos, NPC.Center.Y + yprojpos), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                        Main.dust[dust2].noGravity = false;
                    }
                    SoundEngine.PlaySound(SoundID.Item14 with { Volume = 1f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                }
                NPC.position.X += Main.rand.Next(-1, 2);
                NPC.position.Y += Main.rand.Next(-1, 2);

                if (NPC.ai[0] == 120)
                {
                    SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                    if (!GetInstance<ConfigurationsIndividual>().NoMessage)
                    {
                        if (StormWorld.ultimateBossDown)
                        {
                            Paintext = "Well... you did it... again...";
                        }
                        else if (Main.getGoodWorld)
                        {
                            Paintext = "You....are.....Worthy!";
                        }
                        else
                        {
                            Paintext = "Guess.... you could.... handle the Pain...";
                        }
                        CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.IndianRed, Paintext, true);
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.IndianRed);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText(Paintext, Color.IndianRed);
                        }
                    }
                }
                if (NPC.ai[0] >= 300)
                {
                    //Projectile.NewProjectile(npc.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0f, 0f, 305, 0, 0f, player.whoAmI, Main.myPlayer, player.statLifeMax2 - player.statLife); //Restore All health
                    NPC.life = 0;
                    //NPC.dontTakeDamage = false;
                    NPC.HitEffect(0, 0);
                    NPC.checkDead();
                }
            }

            if (!player.dead && !deathani)//begin AI===================================================== + start each phase
            {

                if (NPC.ai[3] == 0) //No attacks when first summoned
                {
                    NPC.dontTakeDamage = true;
                    shooting = false;
                    if (lifeleft == 0) //move above player at first, 
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = -350;
                    }
                    else //move above player when chaning phases, closer to plauer and move slower
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = -200;
                        NPC.velocity *= 0.97f;
                    }

                    if (distanceToIdlePosition > 200f)
                    {
                        // Speed up the boss if it's away from the player
                        speed = 25f;
                        inertia = 20f;
                    }
                    else
                    {
                        // Slow down the boss if closer to the player
                        speed = 15f;
                        inertia = 25f;
                    }

                    NPC.spriteDirection = 1;

                    NPC.ai[0]++;  //count to first attack
                    NPC.localAI[2] = 270; //Reset rotation
                    if (NPC.ai[0] == 120 && lifeleft > 0) //change visuals and create gore
                    {
                        if (lifeleft == 1)
                        {
                            //Paintext = "Not bad, but how much pain can you handle?";
                            for (int i = 0; i < 50; i++)
                            {
                                float speedY = -4f;
                                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                                int dust1 = Dust.NewDust(new Vector2(NPC.Center.X + 38, NPC.Center.Y - 38), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                                Main.dust[dust1].noGravity = false;
                            }
                            if (Main.netMode != NetmodeID.Server)
                            {
                                Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X + 38, NPC.Center.Y - 46), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore1").Type, 1f);
                                Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X + 38, NPC.Center.Y - 43), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore2").Type, 1f);
                                Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X + 38, NPC.Center.Y - 40), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore3").Type, 1f);
                            }
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 48, NPC.Center.Y - 38), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjExplode>(), 0, 0, Main.myPlayer);
                            Main.projectile[proj].scale = 0.75f;
                            animation = 1; //increase animation
                        }
                        if (lifeleft == 2)
                        {
                            //Paintext = "You're handling this better than I thought, but not for much longer!";
                            for (int i = 0; i < 50; i++)
                            {
                                float speedY = -4f;
                                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                                int dust1 = Dust.NewDust(new Vector2(NPC.Center.X - 48, NPC.Center.Y - 38), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                                Main.dust[dust1].noGravity = false;
                            }
                            if (Main.netMode != NetmodeID.Server)
                            {
                                Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 58, NPC.Center.Y - 46), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore1").Type, 1f);
                                Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 58, NPC.Center.Y - 43), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore2").Type, 1f);
                                Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 58, NPC.Center.Y - 40), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore3").Type, 1f);
                            }
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X - 48, NPC.Center.Y - 38), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjExplode>(), 0, 0, Main.myPlayer);
                            Main.projectile[proj].scale = 0.75f;
                            animation = 2; //increase animation
                        }
                        if (lifeleft == 3)
                        {
                            if (StormWorld.ultimateBossDown)
                            {
                                Paintext = "So we made it to this point again?";
                            }
                            else if (Main.getGoodWorld)
                            {
                                Paintext = "You still need to prove your worth!";
                            }
                            else
                            {
                                Paintext = "Alright, no more playing around!";
                            }
                            CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.IndianRed, Paintext, true);
                            if (Main.netMode == 2) // Server
                            {
                                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.IndianRed);
                            }
                            else if (Main.netMode == 0) // Single Player
                            {
                                Main.NewText(Paintext, Color.IndianRed);
                            }
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 54, NPC.Center.Y + 4), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjExplode>(), 0, 0, Main.myPlayer);
                            Main.projectile[proj].scale = 0.75f;
                            int proj2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X - 54, NPC.Center.Y + 4), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjExplode>(), 0, 0, Main.myPlayer);
                            Main.projectile[proj2].scale = 0.75f;
                            int proj3 = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 30, NPC.Center.Y + 58), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjExplode>(), 0, 0, Main.myPlayer);
                            Main.projectile[proj3].scale = 0.75f;
                            int proj4 = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X - 30, NPC.Center.Y + 58), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjExplode>(), 0, 0, Main.myPlayer);
                            Main.projectile[proj4].scale = 0.75f;
                            for (int i = 0; i < 50; i++)
                            {
                                float speedY = -4f;

                                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                                int dust1 = Dust.NewDust(new Vector2(NPC.Center.X + 54, NPC.Center.Y + 4), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                                Main.dust[dust1].noGravity = false;
                                int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 54, NPC.Center.Y + 4), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                                Main.dust[dust2].noGravity = false;
                                int dust3 = Dust.NewDust(new Vector2(NPC.Center.X + 30, NPC.Center.Y + 58), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                                Main.dust[dust3].noGravity = false;
                                int dust4 = Dust.NewDust(new Vector2(NPC.Center.X - 30, NPC.Center.Y + 58), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                                Main.dust[dust4].noGravity = false;
                            }

                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                if (Main.netMode != NetmodeID.Server)
                                {
                                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X + 44, NPC.Center.Y + 4), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore4").Type, 1f);
                                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 34, NPC.Center.Y + 4), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore4").Type, 1f);
                                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X + 20, NPC.Center.Y + 58), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore4").Type, 1f);
                                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 20, NPC.Center.Y + 58), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore4").Type, 1f);
                                }
                                int type = ModContent.NPCType<TheUltimateBossMinion>();

                                int npc1 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + 54, (int)NPC.Center.Y + 4, type);
                                Main.npc[npc1].localAI[1] = 0;
                                int npc2 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X - 54, (int)NPC.Center.Y + 4, type);
                                Main.npc[npc2].localAI[1] = 90;

                                int npc3 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + 30, (int)NPC.Center.Y + 58, type);
                                Main.npc[npc3].localAI[1] = 180;

                                int npc4 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X - 30, (int)NPC.Center.Y + 58, type);
                                Main.npc[npc4].localAI[1] = 270;
                                NPC.netUpdate = true;
                            }
                            animation = 3; //increase animation

                        }
                        SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        SoundEngine.PlaySound(SoundID.Roar with { Volume = 1f, Pitch = 0.5f }, NPC.Center);

                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[0] == 180 && lifeleft == 0)
                    {
                        if (StormWorld.ultimateBossDown)
                        {
                            Paintext = "Ready to experience pain again?";
                        }
                        else if (Main.getGoodWorld)
                        {
                            Paintext = "Ready to prove your worth?";
                        }
                        else
                        {
                            Paintext = "Ready to experience pain?";
                        }
                        CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.IndianRed, Paintext, true);
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.IndianRed);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText(Paintext, Color.IndianRed);
                        }
                    }
                    if ((NPC.ai[0] >= 180 && lifeleft != 0)|| (NPC.ai[0] >= 300 && lifeleft == 0)) //5 seconds when spawned, 3 seconds for each phase
                    {
                        NPC.dontTakeDamage = false;
                    }
                    if ((NPC.ai[0] >= 180 && lifeleft != 0) || (NPC.ai[0] >= 300 && lifeleft == 0) && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.dontTakeDamage = false;
                        NPC.ai[0] = 0;
                        NPC.localAI[0] = 0;
                        if (lifeleft == 0)
                        {
                            NPC.localAI[2] = 270; //Reset rotation
                        }
                        else if (lifeleft is 1 or 2)
                        {
                            NPC.localAI[2] = 90; //Reset rotation
                        }
                        else if (lifeleft == 3)
                        {
                            NPC.localAI[2] = 0; //Reset rotation
                        }
                        shooting = true;
                        if (lifeleft == 0)
                            NPC.ai[3] = 1; //change to attack 1
                        else if (lifeleft == 1)
                            NPC.ai[3] = 5; //change to new attack 5
                        else if (lifeleft == 2)
                            NPC.ai[3] = 7; //change to new attack 7
                        else if (lifeleft == 3)
                            NPC.ai[3] = 9; //change to new attack 9

                        
                        NPC.netUpdate = true;

                    }
                }
                Attacks(player); //All attacks are in this hook
            }
            //Phases, resets Ai to passive ================================================PHASE Changer
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.life >= NPC.lifeMax * 0.7f) //redundant but :ech:
                {
                    lifeleft = 0;
                }
                if (NPC.life < NPC.lifeMax * 0.7f && lifeleft == 0) //Phase 1 > 2 (70%)
                {
                    NPC.ai[3] = 0;
                    NPC.ai[0] = 0;
                    NPC.localAI[0] = 0;//Reset all ai values
                    NPC.localAI[2] = 0;
                    NPC.localAI[3] = 0;
                    lifeleft = 1;
                    NPC.netUpdate = true;

                }
                if (NPC.life < NPC.lifeMax * 0.4f && lifeleft == 1)//Phase 2 > 3 (40%)
                {
                    NPC.ai[3] = 0;
                    NPC.ai[0] = 0;
                    NPC.localAI[0] = 0;//Reset all ai values
                    NPC.localAI[2] = 0;
                    NPC.localAI[3] = 0;

                    lifeleft = 2;
                   
                    NPC.netUpdate = true;
                }
                if (NPC.life < NPC.lifeMax * 0.1f && lifeleft == 2) //Phase 3 > 4 (10%)
                {
                    Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/UltimateBossMusic2"); //Can change music for last phases
                    Main.musicFade[Main.curMusic] = 0.9f;

                    NPC.ai[3] = 0;
                    NPC.ai[0] = 0;
                    NPC.localAI[0] = 0;//Reset all ai values
                    NPC.localAI[2] = 0;
                    NPC.localAI[3] = 0;
                    lifeleft = 3;
                    NPC.netUpdate = true;
                }
            }
        }
        private void Attacks(Player player)//___________________________________________________________________________________________________________________________________________________
        {
            //NPC.localAI[2] = rotation
            //NPC.localAI[3] = rotation direction

            //MOVEMENT, sometimes orbits player, sometimes floats above/below
            //DEBUG DUST
            /*var dustt = Dust.NewDustDirect(new Vector2(player.Center.X + NPC.ai[1], player.Center.Y + NPC.ai[2]), 0, 0, 138);
            dustt.velocity *= 0;
            dustt.noGravity = true;*/

            //================================= projectile values ========================================== 
            if (Main.getGoodWorld && Main.masterMode)
            {
                projvelocity = 1.6f;
                projcount = (Main.rand.Next(10, 11)); // 8-10
            }
            else if (Main.masterMode) //Projectile changes, same for all attacks
            {
                projvelocity = 1.5f;
                projcount = (Main.rand.Next(7, 10)); // 7-9
            }
            else if (Main.expertMode && !Main.masterMode)
            {
                projvelocity = 1.4f;
                projcount = (Main.rand.Next(6, 9)); // 6-8
            }
            else
            {
                projvelocity = 1.3f;
                projcount = (Main.rand.Next(5, 8)); // 5-7
            }

            if (NPC.ai[3] == 1) //Attack 1 fire projectiles directly at player
            {
                NPC.ai[0]++;
                shooting = true;

                //movement code at top

                //damage
                if (Main.getGoodWorld && Main.masterMode)
                    projdamage = (55 * clamteadmg) / 100; //330 on ftw                          
                else if (Main.masterMode)
                    projdamage = (45 * clamteadmg) / 100; // 270 on master               
                else if (Main.expertMode && !Main.masterMode)
                    projdamage = (50 * clamteadmg) / 100; // 200 On expert
                else
                    projdamage = (70 * clamteadmg) / 100; // 140 on normal
                    
                if (NPC.ai[0] > 90) //Delay before firing
                {
                    NPC.localAI[0]++;
                    if (NPC.localAI[0] > 15)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            float speedY = -3f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1.25f);
                            Main.dust[dust2].noGravity = true;
                        }
                    }
                    if (((NPC.localAI[0] > 35 && lifeleft == 0) || (NPC.localAI[0] > 30 && lifeleft == 1) || (NPC.localAI[0] > 25 && lifeleft == 2)) && distance > 300)
                    {
                        NPC.velocity *= 0.5f;
                        //Dust.QuickDustLine(new Vector2(player.Center.X + 54, player.Center.Y + 4), new Vector2(NPC.Center.X + 54, NPC.Center.Y + 4), 35, Color.IndianRed); //centre to centre
                        //Dust.QuickDustLine(new Vector2(player.Center.X - 54, player.Center.Y + 4), new Vector2(NPC.Center.X - 54, NPC.Center.Y + 4), 35, Color.IndianRed); //centre to centre
                        //Dust.QuickDustLine(new Vector2(player.Center.X + 30, player.Center.Y + 58), new Vector2(NPC.Center.X + 30, NPC.Center.Y + 58), 35, Color.IndianRed); //centre to centre
                        //Dust.QuickDustLine(new Vector2(player.Center.X - 30, player.Center.Y + 58), new Vector2(NPC.Center.X - 30, NPC.Center.Y + 58), 35, Color.IndianRed); //centre to centre
                        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust1 = Dust.NewDust(new Vector2(NPC.Center.X + 54, NPC.Center.Y + 4), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust1].noGravity = true;
                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 54, NPC.Center.Y + 4), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust2].noGravity = true;
                            int dust3 = Dust.NewDust(new Vector2(NPC.Center.X + 30, NPC.Center.Y + 58), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust3].noGravity = true;
                            int dust4 = Dust.NewDust(new Vector2(NPC.Center.X - 30, NPC.Center.Y + 58), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust4].noGravity = true;
                        }

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity * 0.75f;
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 54, NPC.Center.Y + 4), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X - 54, NPC.Center.Y + 4), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 30, NPC.Center.Y + 58), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X - 30, NPC.Center.Y + 58), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                        }

                        NPC.localAI[0] = 0;

                    }
                }

                if (NPC.ai[0] >= 360 && NPC.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient) // 6 seconds
                {
                    NPC.ai[3]++;
                    NPC.localAI[0] = 0;
                    NPC.localAI[2] = 270f;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.ai[3] == 2) //Attack 2 fire out gravity obeying projs
            {
                NPC.ai[0]++;
                shooting = true;
                if (player.velocity.X > 5)//move ahead of player, or above if moving slowly
                NPC.ai[1] = 250;
                else if (player.velocity.X < -5)
                    NPC.ai[1] = -250;
                else
                    NPC.ai[1] = 0;

                //NPC.ai[1] = 250 * player.direction;
                NPC.ai[2] = -200;

                //movespeed
                float movedistance = Vector2.Distance(player.Center + new Vector2(NPC.ai[1], NPC.ai[2]), NPC.Center);
                if (distance < 1000)
                {
                    movespeed = movedistance / 14;
                }

                //damage
                if (Main.getGoodWorld && Main.masterMode)
                    projdamage = (55 * clamteadmg) / 100; //330 on ftw                          
                else if (Main.masterMode)
                    projdamage = (45 * clamteadmg) / 100; // 270 on master               
                else if (Main.expertMode && !Main.masterMode)
                    projdamage = (50 * clamteadmg) / 100; // 200 On expert
                else
                    projdamage = (70 * clamteadmg) / 100; // 140 on normal

                if (NPC.ai[0] > 60) //Delay before firing
                {
                    NPC.localAI[0]++;

                    if (NPC.localAI[0] > 45)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            float speedY = -3f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1.25f);
                            Main.dust[dust2].noGravity = true;
                        }
                    }
                    if ((NPC.localAI[0] > 80 && lifeleft == 0) || (NPC.localAI[0] > 70 && lifeleft == 1) || (NPC.localAI[0] > 60 && lifeleft == 2))
                    {
                        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1.25f);
                            Main.dust[dust2].noGravity = true;
                        }

                        for (int i = 0; i < projcount - 1; i++)
                        {
                            int shoot = Main.rand.Next(0, 4);
                            float xpos = NPC.Center.X;
                            float ypos = NPC.Center.Y;
                            switch (shoot)
                            {
                                case 0:
                                    xpos = NPC.Center.X + 54;
                                    ypos = NPC.Center.Y + 4;
                                    break;
                                case 1:
                                    xpos = NPC.Center.X - 54;
                                    ypos = NPC.Center.Y + 4;
                                    break;
                                case 2:
                                    xpos = NPC.Center.X + 30;
                                    ypos = NPC.Center.Y + 58;
                                    break;
                                case 3:
                                    xpos = NPC.Center.X - 30;
                                    ypos = NPC.Center.Y + 58;
                                    break;
                            }
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 perturbedSpeed = new Vector2(0, -7f).RotatedByRandom(MathHelper.ToRadians(90));

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(xpos, ypos), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjGravity>(), projdamage, 1, Main.myPlayer, 0, 0);
                            }
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y - 12), new Vector2(0, -7), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjGravity>(), projdamage, 1, Main.myPlayer, 0, 0);

                        NPC.localAI[0] = 0;
                    }
                }
                if (NPC.ai[0] >= 480 && NPC.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient) // 8 seconds
                {
                    NPC.ai[3]++;
                    NPC.localAI[0] = 0;
                    NPC.localAI[2] = 270;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
           
            if (NPC.ai[3] == 3) //Attack 3 rapidly summon projectiles in a plus shape
            {
                NPC.ai[0]++;
                shooting = true;
                //movement code at top
                //damage
                if (Main.getGoodWorld && Main.masterMode)
                    projdamage = (55 * clamteadmg) / 100; //330 on ftw                          
                else if (Main.masterMode)
                    projdamage = (45 * clamteadmg) / 100; // 270 on master               
                else if (Main.expertMode && !Main.masterMode)
                    projdamage = (50 * clamteadmg) / 100; // 200 On expert
                else
                    projdamage = (70 * clamteadmg) / 100; // 140 on normal

                if (NPC.ai[0] > 90) //Delay before firing
                {
                    NPC.localAI[0]++;
                    if (((NPC.localAI[0] > 50 && lifeleft == 0) || (NPC.localAI[0] > 45 && lifeleft == 1) || (NPC.localAI[0] > 40 && lifeleft == 2)) && distance > 300)
                    {
                        NPC.velocity *= 0.5f;

                        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 10; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust1 = Dust.NewDust(new Vector2(NPC.Center.X + 54, NPC.Center.Y + 4), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust1].noGravity = true;
                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 54, NPC.Center.Y + 4), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust2].noGravity = true;
                            int dust3 = Dust.NewDust(new Vector2(NPC.Center.X + 30, NPC.Center.Y + 58), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust3].noGravity = true;
                            int dust4 = Dust.NewDust(new Vector2(NPC.Center.X - 30, NPC.Center.Y + 58), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust4].noGravity = true;
                        }

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            float rotation = MathHelper.ToRadians(180);
                            float numberProjectiles = projcount; //4-6
                            int shoot = Main.rand.Next(0, 4);

                            for (int j = 0; j < numberProjectiles; j++)
                            {
                                Vector2 perturbedSpeed = new Vector2(0, projvelocity * .66f).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                                switch (shoot)
                                {
                                    case 0:
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 54, NPC.Center.Y + 4), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                                        break;
                                    case 1:
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X - 54, NPC.Center.Y + 4), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                                        break;
                                    case 2:
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 30, NPC.Center.Y + 58), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                                        break;
                                    case 3:
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X - 30, NPC.Center.Y + 58), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                                        break;
                                }
                            }
                        }
                        NPC.localAI[0] = 0;
                    }
                }
                if (NPC.ai[0] >= 360 && NPC.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient) // 6 seconds
                {
                    NPC.ai[3]++;
                    NPC.localAI[0] = 0;
                    NPC.localAI[2] = 270;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.ai[3] == 4) //Attack 4 circle of projectiles around player
            {
                NPC.ai[0]++;
                shooting = false;

                NPC.ai[1] = 0;
                NPC.ai[2] = -400;

                //damage
                if (Main.getGoodWorld && Main.masterMode)
                    projdamage = (45 * clamteadmg) / 100; //270 on ftw                          
                else if (Main.masterMode)
                    projdamage = (35 * clamteadmg) / 100; // 210 on master               
                else if (Main.expertMode && !Main.masterMode)
                    projdamage = (40 * clamteadmg) / 100; // 160 On expert
                else
                    projdamage = (55 * clamteadmg) / 110; // 110 on normal

                //movespeed
                float movedistance = Vector2.Distance(player.Center + new Vector2(NPC.ai[1], NPC.ai[2]), NPC.Center);
                if (distance < 1000)
                {
                    movespeed = movedistance / 20;
                }

                if (NPC.ai[0] > 60) //Delay before firing
                {
                    NPC.localAI[0]++;

                    if ((NPC.localAI[0] > 75 && lifeleft == 0) || (NPC.localAI[0] > 65 && lifeleft == 1) || (NPC.localAI[0] > 55 && lifeleft == 2))
                    {
                        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1.25f);
                            Main.dust[dust2].noGravity = true;
                        }
                        int circleprojcount;

                        if (Main.getGoodWorld && Main.masterMode) // only 1 odd and one even
                            circleprojcount = (Main.rand.Next(9, 11)); // 9-10
                        else if (Main.masterMode) 
                            circleprojcount = (Main.rand.Next(8, 10)); // 8-9
                        else if (Main.expertMode && !Main.masterMode)
                            circleprojcount = (Main.rand.Next(7, 9)); // 7-8
                        else
                            circleprojcount = (Main.rand.Next(6, 8)); // 6-7

                        float degreespread = 360 / circleprojcount; // divide proj count from 360
                        for (int i = 0; i < circleprojcount; i++)
                        {
                            NPC.localAI[3] += degreespread; //add the degrees to each projectile for an even spread
                            //Main.NewText("the pain " + NPC.localAI[3], Color.Red);

                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity; 
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                                //For the radius
                                double deg = NPC.localAI[3]; //The degrees
                                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                                double dist = 350; //Distance away from the player

                                float posX = player.Center.X - (int)(Math.Cos(rad) * dist);
                                float posY = player.Center.Y - (int)(Math.Sin(rad) * dist);

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(posX, posY), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjCharge>(), projdamage, 1, Main.myPlayer, 0, 0);
                            }
                        }
                        NPC.localAI[3] = 0; //reset spread
                        NPC.localAI[0] = 0;        
                    }
                }
                if (NPC.ai[0] >= 360 && NPC.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient) // 6 seconds
                {
                    if (lifeleft == 0)
                    {
                        NPC.ai[3] = 1;
                    }
                    else
                    {
                        NPC.ai[3]++;
                    }
                    NPC.localAI[0] = 0;
                    NPC.localAI[2] = 0;
                    NPC.localAI[3] = 0;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.ai[3] == 5) //Attack 5 projectiles from side (Phase 2+ Only)
            {
                if (NPC.ai[0] == 1) //set wheter left or right
                {
                    NPC.localAI[3] = Main.rand.Next(-1, 2);
                    if (NPC.localAI[3] != 1) // 0 = -1, 1 = 1
                        NPC.localAI[3] = -1;
                }
                shooting = false;
                if (NPC.localAI[3] == -1) //move left and right
                    NPC.ai[1] = 400;
                else if (NPC.localAI[3] == 1)
                    NPC.ai[1] = -400;
                
                NPC.ai[2] = -250;

                                  //movespeed
                float movedistance = Vector2.Distance(player.Center + new Vector2(NPC.ai[1], NPC.ai[2]), NPC.Center);
                if (distance < 1000)
                {
                    movespeed = movedistance / 20;
                }
                //damage
                if (Main.getGoodWorld && Main.masterMode)
                    projdamage = (45 * clamteadmg) / 100; //270 on ftw                          
                else if (Main.masterMode)
                    projdamage = (35 * clamteadmg) / 100; // 210 on master               
                else if (Main.expertMode && !Main.masterMode)
                    projdamage = (40 * clamteadmg) / 100; // 160 On expert
                else
                    projdamage = (55 * clamteadmg) / 110; // 110 on normal

                if (NPC.ai[0] < 90)
                {
                    NPC.ai[0]++;
                    NPC.localAI[2] = 0; //side changer
                }
                if (NPC.ai[0] >= 90) //Delay before firing
                {
                    NPC.localAI[2]++; //side changer
                    NPC.localAI[0]++;
                    if ((NPC.localAI[0] >= 50 && lifeleft == 1) || (NPC.localAI[0] >= 40 && lifeleft == 2))
                    {
                        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1.25f);
                            Main.dust[dust2].noGravity = true;
                        }

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            float posX = player.Center.X - 1050 * NPC.localAI[3];
                            for (int i = 0; i < projcount - 1; i++)
                            {
                                float posY = player.Center.Y + Main.rand.NextFloat(-800f, 800f);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(posX, posY), new Vector2(projvelocity * 0.9f * NPC.localAI[3], 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 1, 0);
                            }
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(player.Center.X - 1050 * NPC.localAI[3], player.Center.Y), new Vector2(projvelocity * 0.9f * NPC.localAI[3], 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 1, 0);
                        }
                        NPC.localAI[0] = 0;
                    }
                    if ((NPC.localAI[2] >= 199 && lifeleft == 1) || (NPC.localAI[2] >= 159 && lifeleft == 2)) // fires 3 blasts, then switches sides (1 frame before fired fourth shot)
                    {
                        SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack with { Volume = 2f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        NPC.ai[0]++; // add one to the counter
                        //Main.NewText("AI 0 = " + NPC.ai[0]);
                        NPC.localAI[3] *= -1; // change side
                        NPC.localAI[0] = 0; 
                        NPC.localAI[2] = 0; 
                    }
                }
                if (NPC.ai[0] >= 90 + 4 && Main.netMode != NetmodeID.MultiplayerClient) //4 side switches
                {
                    NPC.ai[3]++;
                    NPC.localAI[3] = 0;
                    NPC.localAI[0] = 0;
                    NPC.localAI[2] = 0;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }

            if (NPC.ai[3] == 6) //Attack 6 summon random projectiles that home in after a delay (Phase 2+ Only)
            {
                NPC.ai[0]++;

                shooting = false;

                NPC.ai[1] = 0;
                NPC.ai[2] = -350;

                NPC.localAI[2] = 90f;//rotation reset
                
                //damage
                if (Main.getGoodWorld && Main.masterMode)
                    projdamage = (45 * clamteadmg) / 100; //270 on ftw                          
                else if (Main.masterMode)
                    projdamage = (35 * clamteadmg) / 100; // 210 on master               
                else if (Main.expertMode && !Main.masterMode)
                    projdamage = (40 * clamteadmg) / 100; // 160 On expert
                else
                    projdamage = (55 * clamteadmg) / 110; // 110 on normal

                //movespeed
                float movedistance = Vector2.Distance(player.Center + new Vector2(NPC.ai[1], NPC.ai[2]), NPC.Center);
                if (distance < 1000)
                {
                    movespeed = movedistance / 20;
                }

                if (NPC.ai[0] > 90 && NPC.ai[0] <= 540) //Delay before firing and 1 second cooldown
                {
                    NPC.localAI[0]++;

                    if ((NPC.localAI[0] > 30 && lifeleft == 1) || (NPC.localAI[0] > 25 && lifeleft == 2))
                    {

                        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1.25f);
                            Main.dust[dust2].noGravity = true;
                        }

                        for (int i = 0; i < 1; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity; 
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                                //For the radius
                                double deg = Main.rand.Next(0, 360); //The degrees
                                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                                double dist = Main.rand.Next(300, 400); //Distance away from the player

                                float posX = player.Center.X - (int)(Math.Cos(rad) * dist) + player.velocity.X;
                                float posY = player.Center.Y - (int)(Math.Sin(rad) * dist) + player.velocity.X;

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(posX, posY), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjHome>(), projdamage, 1, Main.myPlayer, 0, 0);
                            }
                        }
                        NPC.localAI[0] = 0;
                    }
                }
                if (NPC.ai[0] >= 600 && Main.netMode != NetmodeID.MultiplayerClient) // 10 seconds
                {
                    if (lifeleft == 1)
                    {
                        NPC.ai[3] = 1;
                    }
                    else
                    {
                        NPC.ai[3]++;
                    }
                    NPC.localAI[0] = 0;
                    NPC.localAI[2] = 270;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }

            if (NPC.ai[3] == 7) //Attack 7 fire out projectiles in random directions after "teleporting" phase 3 only
            {
                NPC.ai[0]++;
                shooting = true;

                //use old no inertia movement system

                //movespeed
                float movedistance = Vector2.Distance(player.Center + new Vector2(NPC.ai[1], NPC.ai[2]), NPC.Center);
                if (distance < 1000)
                {
                    movespeed = movedistance / 17;
                }
                //damage
                if (Main.getGoodWorld && Main.masterMode)
                    projdamage = (55 * clamteadmg) / 100; //330 on ftw                          
                else if (Main.masterMode)
                    projdamage = (45 * clamteadmg) / 100; // 270 on master               
                else if (Main.expertMode && !Main.masterMode)
                    projdamage = (50 * clamteadmg) / 100; // 200 On expert
                else
                    projdamage = (70 * clamteadmg) / 100; // 140 on normal

                if (NPC.ai[0] > 120) //Delay before firing
                {
                    NPC.localAI[0]++;
                    //Main.NewText("" + NPC.ai[1] + " " + NPC.ai[2], 175, 17, 96);

                    if (NPC.localAI[0] == 1) //"dash" to location
                    {
                        //For the radius
                        double deg = Main.rand.Next(0, 360); //The degrees
                        double rad = deg * (Math.PI / 180); //Convert degrees to radians
                        double dist = Main.rand.Next(550, 600); //Distance away from the player

                        NPC.ai[1] = (int)(Math.Cos(rad) * dist);
                        NPC.ai[2] = (int)(Math.Sin(rad) * dist);
                        SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack with { Volume = 2f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                    }
                    if (NPC.localAI[0] > 30 && NPC.localAI[0] < 75)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            float speedY = -3f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1.25f);
                            Main.dust[dust2].noGravity = true;
                        }
                    }
                    if (NPC.localAI[0] == 60)
                    {

                        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        for (int i = 0; i < 25; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust1 = Dust.NewDust(new Vector2(NPC.Center.X + 54, NPC.Center.Y + 4), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust1].noGravity = true;
                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 54, NPC.Center.Y + 4), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust2].noGravity = true;
                            int dust3 = Dust.NewDust(new Vector2(NPC.Center.X + 30, NPC.Center.Y + 58), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust3].noGravity = true;
                            int dust4 = Dust.NewDust(new Vector2(NPC.Center.X - 30, NPC.Center.Y + 58), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust4].noGravity = true;
                        }
                        for (int i = 0; i < projcount / 2; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity * 0.85f;
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(360));

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 54, NPC.Center.Y + 4), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                                perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(360));
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X - 54, NPC.Center.Y + 4), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                                perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(360));
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 30, NPC.Center.Y + 58), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                                perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(360));
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X - 30, NPC.Center.Y + 58), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                            }
                        }
                        if (Main.netMode != NetmodeID.MultiplayerClient) //one always aimed at the player
                        {
                            Vector2 velocity2 = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity * 0.85f;
                            Vector2 perturbedSpeed2 = new Vector2(velocity2.X, velocity2.Y).RotatedByRandom(MathHelper.ToRadians(0));
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed2.X, perturbedSpeed2.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                        }  
                    }
                    if (NPC.localAI[0] >= 90)
                        NPC.localAI[0] = 0;
                }
                if (NPC.ai[0] >= 720 && NPC.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient) // 12 seconds
                {
                    NPC.ai[3]++;

                    NPC.localAI[0] = 0;
                    NPC.localAI[2] = 270;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.ai[3] == 8) //Attack 8 summons a large skull that dashes towards the player (Phase 3+ Only)
            {
                NPC.ai[0]++;
                shooting = false;

                NPC.ai[1] = 0;
                NPC.ai[2] = -400;

                //movespeed
                float movedistance = Vector2.Distance(player.Center + new Vector2(NPC.ai[1], NPC.ai[2]), NPC.Center);
                if (distance < 1000)
                {
                    movespeed = movedistance / 10;
                }

                 //damage
                if (Main.getGoodWorld && Main.masterMode)
                    projdamage = (45 * clamteadmg) / 100; //270 on ftw                          
                else if (Main.masterMode)
                    projdamage = (35 * clamteadmg) / 100; // 210 on master               
                else if (Main.expertMode && !Main.masterMode)
                    projdamage = (40 * clamteadmg) / 100; // 160 On expert
                else
                    projdamage = (55 * clamteadmg) / 110; // 110 on normal
                if (NPC.ai[0] > 60 && NPC.ai[0] < 120)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        float speedY = -6f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust1 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                        Main.dust[dust1].noGravity = true;
                    }
                    if (NPC.ai[0] % 10 == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item15 with { Volume = 2f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, NPC.Center);
                    }
                }
                if (NPC.ai[0] >= 120) //Delay before firing
                {
                    //NPC.ai[2] += 1.5f; //move closer to player
                    NPC.localAI[0]++;
                    if (NPC.localAI[0] == 1)
                    {
                        int shoot = Main.rand.Next(0, 4);
                        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        float speedY = -6f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                       
                        for (int i = 0; i < 50; i++)
                        {
                            int dust1 = Dust.NewDust(new Vector2(NPC.Center.X +54, NPC.Center.Y + 4), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust1].noGravity = true;
                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 54, NPC.Center.Y + 4), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust2].noGravity = true;
                        }
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 1; i++)
                            {
                                //Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity * 1f;
                                //Vector2 perturbedSpeed = new Vector2(0, projvelocity).RotatedByRandom(MathHelper.ToRadians(10));
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 54, NPC.Center.Y + 4), new Vector2(+15, 4), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjLarge>(), projdamage, 1, Main.myPlayer, 0, 0);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X - 54, NPC.Center.Y + 4), new Vector2(-15, 4), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjLarge>(), projdamage, 1, Main.myPlayer, 0, 0);
                            }
                        }

                        //NPC.localAI[0] = 0;
                    }
                }
                if (NPC.ai[0] >= 600 && Main.netMode != NetmodeID.MultiplayerClient) // 10 seconds
                {
                    NPC.ai[3] = 1;
                    NPC.ai[2] = 0;
                    NPC.localAI[0] = 0;
                    NPC.localAI[2] = 270;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
            /*if (NPC.ai[3] == 8) //Old Attck 8, rapidly fire projectiles downwards (Phase 3+ Only)
            {
                NPC.ai[0]++;
                NPC.localAI[0]++;
                shooting = true;

                NPC.ai[1] = 0;
                if (NPC.ai[0] < 120) //Delay before firing
                    NPC.ai[2] = -450;

                //movespeed
                float movedistance = Vector2.Distance(player.Center + new Vector2(NPC.ai[1], NPC.ai[2]), NPC.Center);
                if (distance < 1000)
                {
                    movespeed = movedistance / 10;
                }

                //damage
                if (Main.getGoodWorld && Main.masterMode)
                    projdamage = (55 * clamteadmg) / 100; //330 on ftw                          
                else if (Main.masterMode)
                    projdamage = (45 * clamteadmg) / 100; // 270 on master               
                else if (Main.expertMode && !Main.masterMode)
                    projdamage = (50 * clamteadmg) / 100; // 200 On expert
                else
                    projdamage = (70 * clamteadmg) / 100; // 140 on normal
                if (NPC.ai[0] > 60 && NPC.ai[0] < 120)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        float speedY = -3f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                        Main.dust[dust2].noGravity = true;
                    }
                }
                if (NPC.ai[0] >= 120) //Delay before firing
                {
                    NPC.ai[2] += 1.5f; //move closer to player

                    if ((NPC.localAI[0] > 10 && lifeleft == 0) || (NPC.localAI[0] > 10 && lifeleft == 1) || (NPC.localAI[0] > 10 && lifeleft == 2))
                    {
                        int shoot = Main.rand.Next(0, 4);
                        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        float speedY = -6f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                        float xpos = NPC.Center.X;
                        float ypos = NPC.Center.Y;
                        if (shoot == 0)
                        {
                            xpos = NPC.Center.X + 54;
                            ypos = NPC.Center.Y + 4;
                        }
                        else if (shoot == 1)
                        {
                            xpos = NPC.Center.X - 54;
                            ypos = NPC.Center.Y + 4;
                        }
                        else if (shoot == 2)
                        {
                            xpos = NPC.Center.X + 30;
                            ypos = NPC.Center.Y + 58;
                        }
                        else if (shoot == 3)
                        {
                            xpos = NPC.Center.X - 30;
                            ypos = NPC.Center.Y + 58;
                        }

                        for (int i = 0; i < 50; i++)
                        {
                            int dust1 = Dust.NewDust(new Vector2(xpos, ypos), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust1].noGravity = true;
                        }
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 1; i++)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity * 1f;
                                Vector2 perturbedSpeed = new Vector2(0, projvelocity).RotatedByRandom(MathHelper.ToRadians(10));
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(xpos, ypos), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                            }
                        }

                        NPC.localAI[0] = 0;
                    }
                }
                if (NPC.ai[0] >= 300 && NPC.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient) // 5 seconds
                {
                    NPC.ai[3] = 1;

                    NPC.localAI[0] = 0;
                    NPC.localAI[2] = 270;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }*/
            //For the final phase====================================================================================================================
            if (NPC.ai[3] == 9) //Attack 9, summon the "minions" and stay still
            {
                //minion spawning in ai3 = 0
                if (NPC.CountNPCS(ModContent.NPCType<TheUltimateBossMinion>()) > 0)
                {
                    NPC.ai[1] = 0;
                    NPC.ai[2] = -450;
                    NPC.localAI[2] = 0;
                }
                shooting = false;
                NPC.dontTakeDamage = true;

                NPC.ai[0]++;
                if (NPC.ai[0] >= 120) //
                {
                    NPC.localAI[0]++;
                }
                //Main.NewText("EXECUTE MY CODE!!!!!!!!!!! " + NPC.localAI[2], 175, 17, 96);

                if (NPC.localAI[0] > 300 && NPC.localAI[0] < 600) //stop moving for fast shooting
                {
                    movespeed = 0;
                    NPC.velocity *= 0.1f;
                    speed = 0f;
                    inertia = 5f;
                }
                else if (NPC.localAI[0] <= 300 || NPC.localAI[0] >= 600)//regular movement, stay above player pretty strictly 
                {
                    speed = 25f;
                    inertia = 10f;
                }
                if (NPC.localAI[0] >= 660)
                {
                    NPC.localAI[0] = 0;
                }

                if (NPC.CountNPCS(ModContent.NPCType<TheUltimateBossMinion>()) == 0) // 2 seconds after last minion killed next attack
                {
                    shooting = false;
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = -200;
                    NPC.localAI[0] = 0;
                    speed = 15f;
                    inertia = 15f;
                    NPC.localAI[2]++;
                    
                    if (NPC.localAI[2] > 120)
                    {
                        SoundEngine.PlaySound(SoundID.Roar with { Volume = 1f, Pitch = 0.5f }, NPC.Center);
                        if (StormWorld.ultimateBossDown)
                        {
                            Paintext = "You're... still able to handle all this pain?";
                        }
                        else if (Main.getGoodWorld)
                        {
                            Paintext = "Could... you really be worthy?";
                        }
                        else
                        {
                            Paintext = "How... are you handling all this pain?";
                        }
                        CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.IndianRed, Paintext, true);
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.IndianRed);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText(Paintext, Color.IndianRed);
                        }
                        
                        NPC.localAI[0] = 0;
                        NPC.ai[0] = 0;
                        NPC.ai[3] = 10;
                        NPC.localAI[1] = 0;

                    }
                }
            }
            if (NPC.ai[3] == 10) //Attack 10, tries to escape, last ditch attack
            {
                NPC.dontTakeDamage = false;

                NPC.ai[1] = 0;
                NPC.ai[2] = 0;

                NPC.ai[0]++;

                NPC.localAI[2] = 90f;//rotation

                for (int k = 0; k < NPC.buffImmune.Length; k++)
                {
                    NPC.buffImmune[k] = true;
                }
                
                if (Main.netMode != NetmodeID.MultiplayerClient) //flee from player
                {
                    if (distanceToIdlePosition < 250f)
                    {
                        // Speed up the boss if it's close to the player
                        speed = -9f;
                        inertia = 20f;
                    }
                    else if (distanceToIdlePosition >= 300 && distanceToIdlePosition < 450)
                    {
                        // Slow down the boss if farther to the player
                        speed = -6f;
                        inertia = 40f;
                    }
                    else if (distanceToIdlePosition >= 450)
                    {
                        // Slow down the boss if farther to the player
                        speed = 6f;
                        inertia = 40f;
                    }
                    NPC.netUpdate = true;
                }
                //damage
                if (Main.getGoodWorld && Main.masterMode)
                    projdamage = (50 * clamteadmg) / 100; //300 on ftw   270 ring                       
                else if (Main.masterMode)
                    projdamage = (40 * clamteadmg) / 100; // 240 on master    216 ring           
                else if (Main.expertMode && !Main.masterMode)
                    projdamage = (45 * clamteadmg) / 100; // 180 On expert  162 ring
                else
                    projdamage = (60 * clamteadmg) / 100; // 120 on normal 108 ring
                if (NPC.ai[0] > 60) //charge up
                {
                    NPC.localAI[0]++;
                    if (NPC.localAI[0] > 80 && NPC.localAI[0] < 180)
                    {
                        if (NPC.localAI[0] % 10 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item15 with { Volume = 2f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, NPC.Center);

                        }
                        for (int i = 0; i < 10; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust1 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            Main.dust[dust1].noGravity = true;
                        }
                    }
                    if (NPC.localAI[0] == 180) //set spread to 0 and burst of particles
                    {
                        projspread = 0;
                        //Dust.QuickDustLine(new Vector2(player.Center.X, player.Center.Y), new Vector2(NPC.Center.X, NPC.Center.Y + 10), 50, Color.IndianRed); //centre to centre
                        SoundEngine.PlaySound(SoundID.Zombie104 with { Volume = 0.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 150; i++)
                        {
                            float speedY = -8f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust1 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                        }
                    }
                    if (NPC.localAI[0] > 180) //fire projs
                    {
                        NPC.velocity *= 0.7f;
                        //NPC.AddBuff(ModContent.BuffType<YouCantEscapeDebuff>(), 2); //attack "damages" the boss
                        for (int i = 0; i < 1; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity;
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(projspread));

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(perturbedSpeed.X * 1.25f, perturbedSpeed.Y * 1.25f), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjShard>(), projdamage, 1, Main.myPlayer, 0, 0);
                            }
                        }

                        if (NPC.localAI[0] % 6 == 0) // once every 6 frames
                        {
                            projspread++;
                            SoundEngine.PlaySound(SoundID.Item42 with { Volume = 2f, Pitch = 0.5f, MaxInstances = -1 }, NPC.Center);
                        }
                        if (NPC.localAI[0] > 300) //ring shot at the end
                        {
                            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjExplode>(), 0, 0, Main.myPlayer);
                            Main.projectile[proj].scale = 1.25f;
                            for (int i = 0; i < 100; i++)
                            {
                                float speedY = -5f;

                                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                                int dust1 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                            }
                            for (int i = 0; i < projcount * 5; i++)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity / 2;
                                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(360));

                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(perturbedSpeed.X * 1f, perturbedSpeed.Y * 1f), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjShard>(), (int)(projdamage * 0.9f), 1, Main.myPlayer, 0, 0, 1);
                                }
                            }
                            NPC.localAI[0] = 0;
                        }
                    }
                }
            }
        }
        public override void UpdateLifeRegen(ref int damage)
        {
            if (NPC.localAI[0] > 180 && NPC.ai[3] == 10) //damage after every shot on final phase
            {
                NPC.lifeRegen -= 2000;
                damage = 100;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (!GetInstance<ConfigurationsIndividual>().NoMessage)
            {
                CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.IndianRed, "How's that for Pain?", true);
                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("How's that for Pain?"), Color.IndianRed);
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText("How's that for Pain?", Color.IndianRed);
                }
            }
        }
        int npcframe = 0;
        public override void FindFrame(int frameHeight)//Animations
        {
            //0-7, 8-15, 16-23, 24-27, 28-31
            NPC.frame.Y = npcframe * frameHeight;
            //NPC.spriteDirection = NPC.direction;
            NPC.frameCounter++;
            if (!deathani)
            {
                if (animation == 0) //Phase 1
                {
                    if (shooting)
                    {
                        NPC.frame.Y = npcframe * frameHeight;
                        if (NPC.frameCounter > 5)
                        {
                            npcframe++;
                            NPC.frameCounter = 0;
                        }
                        if (npcframe < 4 || npcframe > 7) //Cycles through frames
                        {
                            npcframe = 4;
                        }
                    }
                    else
                    {
                        NPC.frame.Y = npcframe * frameHeight;
                        if (NPC.frameCounter > 8)
                        {
                            npcframe++;
                            NPC.frameCounter = 0;
                        }
                        if (npcframe > 3) //Cycles through frames 0-3
                        {
                            npcframe = 0;
                        }
                    }
                }
                if (animation == 1) //Phase 2
                {
                    if (shooting)
                    {
                        NPC.frame.Y = npcframe * frameHeight;
                        if (NPC.frameCounter > 5)
                        {
                            npcframe++;
                            NPC.frameCounter = 0;
                        }
                        if (npcframe < 12 || npcframe > 15) //Cycles through frames 12-15
                        {
                            npcframe = 12;
                        }
                    }
                    else
                    {
                        NPC.frame.Y = npcframe * frameHeight;
                        if (NPC.frameCounter > 8)
                        {
                            npcframe++;
                            NPC.frameCounter = 0;
                        }
                        if (npcframe < 8 || npcframe > 11) //Cycles through frames 8-11
                        {
                            npcframe = 8;
                        }
                    }
                }
                if (animation == 2) //Phase 3
                {
                    if (shooting)
                    {
                        NPC.frame.Y = npcframe * frameHeight;
                        if (NPC.frameCounter > 5)
                        {
                            npcframe++;
                            NPC.frameCounter = 0;
                        }
                        if (npcframe < 20 || npcframe > 23) //Cycles through frames 20-23
                        {
                            npcframe = 20;
                        }
                    }
                    else
                    {
                        NPC.frame.Y = npcframe * frameHeight;
                        if (NPC.frameCounter > 8)
                        {
                            npcframe++;
                            NPC.frameCounter = 0;
                        }
                        if (npcframe <16 || npcframe > 19) //Cycles through frames 16-19
                        {
                            npcframe = 16;
                        }
                    }
                }
                if (animation == 3) //Phase 4
                {
                    NPC.frame.Y = npcframe * frameHeight;
                    if (NPC.frameCounter > 5)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }
                    if (npcframe < 24 || npcframe > 27) //Cycles through frames 24-27 always
                    {
                        npcframe = 24;
                    }
                }
            }
            else
            {
                NPC.frame.Y = npcframe * frameHeight;
                if (NPC.frameCounter > 80 && npcframe < 31) //slowly change through frames 28-31 dead
                {
                    int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjExplode>(), 0, 0, Main.myPlayer);
                    Main.projectile[proj].scale = 1.75f;
                    SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                    for (int i = 0; i < 50; i++)
                    {
                        float speedY = -6f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1f);
                        Main.dust[dust2].noGravity = false;
                    }

                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe < 28) 
                {
                    npcframe = 28;
                }
            }
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
            //boss trophy
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.BossTrophy.UltimateBossTrophy>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Furniture.VortexiaPaintingItem>(), 1));

            //pain
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.PainStaff>(), 1));
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanitysets.BossMaskUltimateBoss>(), 7));
            //notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Furniture.PainMusicBoxitem>(), 10));

            //expert and master loot
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.BossTrophy.UltimateBossBag>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.BossTrophy.UltimateBossRelic>()));
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<Items.Pets.UltimateBossPetItem>(), 4));
            npcLoot.Add(notExpert);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 10; i++)
            {
                float speedY = -4f;

                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 115, dustspeed.X, dustspeed.Y, 50, default, 1.25f);
                Main.dust[dust2].noGravity = true;
            }
            if (NPC.life <= 0 && deathani)          //this make so when the npc has 0 life(dead) he will spawn this
            {  
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjExplode>(), 0, 0, Main.myPlayer);
                Main.projectile[proj].scale = 2.5f;
                SoundEngine.PlaySound(SoundID.Item14 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 5, NPC.Center.Y), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore5").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 5, NPC.Center.Y), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore6").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 5, NPC.Center.Y), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore7").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 5, NPC.Center.Y), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore8").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 5, NPC.Center.Y), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore9").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 5, NPC.Center.Y), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore10").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 5, NPC.Center.Y), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore11").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 5, NPC.Center.Y), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore12").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X - 5, NPC.Center.Y), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore13").Type, 1f);

                }
                for (int i = 0; i < 150; i++)
                {

                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 115, 0, 0, 50);
                    dust.velocity *= 2;
                    dust.scale = 1.5f;
                }
                for (int i = 0; i < 150; i++)
                {
                    float speedY = -8f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(new Vector2(NPC.Center.X - 5, NPC.Center.Y + 10), 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1.5f);
                    Main.dust[dust2].noGravity = false;
                }
            }
        }
        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref StormWorld.ultimateBossDown, -1); //set boss downed      
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (NPC.ai[3] == 10) //50% DR on final attack
            {
                modifiers.FinalDamage *= 0.5f;
            }
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (NPC.ai[3] == 10) //50% DR on final attack
            {
                modifiers.FinalDamage *= 0.5f;
            }
            if (!StormWorld.ultimateBossDown)
            {
                if (!GetInstance<ConfigurationsIndividual>().NoMessage)
                {
                    if (projectile.type == ProjectileID.FinalFractal) //Zenith is overpowered >:(
                    {
                        //modifiers.FinalDamage.Flat = 1;
                        //modifiers.DisableCrit();
                        SoundEngine.PlaySound(SoundID.Item16 with { Volume = 2f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        Paintext = "Zenith means no Pain >:(";
                        if (!Zenithtext)
                        {
                            CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.IndianRed, Paintext, true);

                            if (Main.netMode == 2) // Server
                            {
                                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.IndianRed);
                            }
                            else if (Main.netMode == 0) // Single Player
                            {
                                Main.NewText(Paintext, Color.IndianRed);
                            }
                            Zenithtext = true;
                        }
                    }
                }
            }
            if (projectile.type == ModContent.ProjectileType<Projectiles.PainProj>()) //Reaction
            {
            }
        }
       
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) //Trial
        {
            if (!Main.dedServ)
            {
                Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/Boss/TheUltimateBoss");

                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + NPC.frame.Size() / 2f + new Vector2(-25, 0);
                    Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
                float speen1 = 9f + 3f * (float)Math.Cos((float)Math.PI * 2f * Main.GlobalTimeWrappedHourly);
                Vector2 spinningpoint5 = Vector2.UnitX * speen1;

                Color color2 = Color.IndianRed * (speen1 / 12f) * 0.8f;
                color2.A /= 3;
                if (!deathani && NPC.dontTakeDamage)
                {
                    for (float speen2 = 0f; speen2 < (float)Math.PI * 2f; speen2 += (float)Math.PI / 2f)
                    {
                        Vector2 finalpos = NPC.position + new Vector2(0, 58) + spinningpoint5.RotatedBy(speen2);
                        spriteBatch.Draw(texture, new Vector2(finalpos.X - screenPos.X + (float)(NPC.width / 2) * NPC.scale, finalpos.Y - screenPos.Y + (float)NPC.height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f * NPC.scale), NPC.frame, color2, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    }
                }
            }
            return true;
        }
      
        public override Color? GetAlpha(Color lightColor)
        {
            if (lifeleft == 3 && NPC.ai[3] >= 9)
            {
                return Color.LightCoral;
            }
            else
            {
                return Color.White;
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            position += new Vector2(0, 0); //Moves healthbar down a little
            return true;
        }
    }
}