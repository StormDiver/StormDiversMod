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
using StormDiversMod.Items.Furniture;


namespace StormDiversMod.NPCs.Boss
{
    [AutoloadBossHead]
    public class ThePainBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ultimate Pain"); // Automatic from .lang files
                                                     // make sure to set this for your modnpcs.
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 25;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);

            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;

            NPC.width = 138;
            NPC.height = 138;

            NPC.aiStyle = -1;

            NPC.noGravity = true;
            NPC.damage = 250; //250/350/450

            NPC.defense = 100;
            NPC.lifeMax = 300000;

            NPC.gfxOffY = 0;

            //NPC.HitSound = SoundID.NPCHit2; //Says "thepain"
            //NPC.DeathSound = SoundID.NPCDeath24; //That was painful
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(1, 0, 0, 0);
            if (Main.zenithWorld)
            {
                NPC.boss = true;
                if (!Main.dedServ)
                {
                    //Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/PainRemix");
                    Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/UltimatePainMusic");
                }
            }
            NPC.npcSlots = 10f;
            NPC.noTileCollide = true;
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            NPC.dontTakeDamage = true;

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            //bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPCType<TheUltimateBoss>()], quickUnlock: true); //for gfb worlds, have full bestiary
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            //250K Classic
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
        float speed = 50;
        float inertia = 10;
        float distanceToIdlePosition; //distance to movement positon
        float distance; //Distance to player

        int projdamage; //Damage of all projectiles
        int projcount; //Number of projs if applicable
        float projvelocity; //Velocity of projectiles

        int lifeleft; //0 = full 1 = 3 quater, 2 = half, 1 = quarter
        bool deathani;
        string Paintext; //The text in chat and over the boss
        bool Zenithtext;
        int teleporttime; // time till teleport
        int clamteadmg = 100;

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
            NPC.damage = 0;
            NPC.buffImmune[BuffID.Confused] = true;

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


            //======================================================MOVEMENT==============================================================================================

            Player player = Main.player[NPC.target]; //Code to move towards player

            if (!player.dead && !deathani && Main.zenithWorld)
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
                NPC.rotation = NPC.velocity.X / 50;

            }

            distance = Vector2.Distance(player.Center, NPC.Center);
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
                        var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 72, 0, 0);
                        dust.noGravity = true;
                        dust.velocity *= 0;
                        dust.scale = 1.25f;
                    }
                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y - 400) - new Vector2(dustx, dusty)) * 25;
                    {
                        var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 72, velocity.X, velocity.Y);
                        dust.noGravity = true;
                        dust.velocity *= 0.5f;
                        dust.scale = 1.25f;
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

                Dust.QuickDustLine(NPC.Center, new Vector2(NPC.oldPosition.X + NPC.width / 2, NPC.oldPosition.Y + NPC.height / 2), 152, Color.DeepPink); //centre to centre

                if (!GetInstance<ConfigurationsIndividual>().NoMessage)
                {
                    Paintext = "You cannot escape the pain that easily!";

                    CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.DeepPink, Paintext, true);
                    if (Main.netMode == 2) // Server
                    {
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.DeepPink);
                    }
                    else if (Main.netMode == 0) // Single Player
                    {
                        Main.NewText(Paintext, Color.DeepPink);
                    }
                }
                for (int i = 0; i < 150; i++)
                {
                    float speedY = -8f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1.5f);
                    //Main.dust[dust2].noGravity = true;
                }

                teleporttime = 0;

            }
            /*if (distance > 10000 && NPC.ai[3] != 0)// Despawn if too far away
            {
                NPC.active = false;
            }*/

            if ((player.dead && !deathani) || !Main.zenithWorld)//When player is dead slow down, mock the player, then fly away
            {
                if (NPC.localAI[1] == 60)
                {
                    if (!Main.zenithWorld)
                    {
                        Paintext = "Wait I shouldn't be here...";
                    }
                    else if (!StormWorld.ultimateBossDown && player.HasItem(ModContent.ItemType<PainSword>()))
                    {
                        Paintext = "Even when cheating, you still lose!!!!";
                    }
                    else if (StormWorld.ultimateBossDown)
                    {
                        Paintext = "You handled it once before, but not this time!";
                    }
                    else
                    {
                        Paintext = "Guess you couldn't handle the Pain!";
                    }
                    CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.DeepPink, Paintext, true);
                    if (Main.netMode == 2) // Server
                    {
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.DeepPink);
                    }
                    else if (Main.netMode == 0) // Single Player
                    {
                        Main.NewText(Paintext, Color.DeepPink);
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

                NPC.localAI[1]++;
                if (NPC.localAI[1] > 300 && Main.zenithWorld)
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
                if (Main.rand.Next(5) == 0)
                {
                    int xprojpos = Main.rand.Next(-70, 70);
                    int yprojpos = Main.rand.Next(-70, 70);

                    int ProjID = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + xprojpos, NPC.Center.Y + yprojpos), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj2>(), 0, 0, Main.myPlayer);
                    Main.projectile[ProjID].scale = 0.75f;

                    for (int i = 0; i < 20; i++)
                    {
                        float speedY = -2f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(new Vector2(NPC.Center.X + xprojpos, NPC.Center.Y + yprojpos), 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1f);
                        //Main.dust[dust2].noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.Item14 with { Volume = 1f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                }
                if (NPC.ai[0] == 120)
                {
                    if (!StormWorld.ultimateBossDown && player.HasItem(ModContent.ItemType<PainSword>()))
                    {
                        Paintext = "CHEATER!!!!";
                    }
                    else if (StormWorld.ultimateBossDown)
                    {
                        Paintext = "Well... you did it... again...";
                    }
                    else
                    {
                        Paintext = "Guess.... you could.... handle the Pain...";
                    }
                    CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.DeepPink, Paintext, true);
                    if (Main.netMode == 2) // Server
                    {
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.DeepPink);
                    }
                    else if (Main.netMode == 0) // Single Player
                    {
                        Main.NewText(Paintext, Color.DeepPink);
                    }

                }
                if (NPC.ai[0] >= 300)
                {
                    Projectile.NewProjectile(null, NPC.Center.X, NPC.Center.Y, 0f, 0f, 305, 0, 0f, player.whoAmI, Main.myPlayer, player.statLifeMax2 - player.statLife); //Restore All health

                    NPC.life = 0;
                    NPC.HitEffect(0, 0);
                    NPC.checkDead();
                }
            }     

            if (!player.dead && !deathani && Main.zenithWorld)//begin AI=====================================================
            {
                if (NPC.ai[3] > 0)
                    NPC.dontTakeDamage = false;

                if (NPC.ai[3] == 0) //No attacks when first summoned
                {
                    NPC.ai[1] = 0;
                    NPC.ai[2] = -350;
                    if (distanceToIdlePosition > 400f) //consistent speed
                    {
                        // Speed up the boss if it's away from the player
                        speed = 10f;
                        inertia = 30f;
                    }
                    else
                    {
                        // Slow down the boss if closer to the player
                        speed = 8f;
                        inertia = 40f;
                    }

                    NPC.spriteDirection = 1;

                    NPC.ai[0]++;  //count to first attack
                    NPC.localAI[2] = 270; //Reset rotation
                    if (NPC.ai[0] == 180 && Main.netMode != NetmodeID.MultiplayerClient) //taunt text and heal
                    {
                        if (lifeleft == 0)//phase 1
                        {                           
                            if (!StormWorld.ultimateBossDown && player.HasItem(ModContent.ItemType<PainSword>())) //Player cheating >:(
                            {
                                Paintext = "Wh...Why do you have that..?";
                            }
                            else if (StormWorld.ultimateBossDown) //After defeating it
                            {
                                Paintext = "I see you've handled pain before...";
                            }
                            else //Before defeating it
                            {
                                Paintext = "Ready to experience Pain?";
                            }
                            Projectile.NewProjectile(null, NPC.Center.X, NPC.Center.Y, 0f, 0f, 305, 0, 0f, player.whoAmI, Main.myPlayer, player.statLifeMax2 - player.statLife); //Restore All health
                          
                            SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSound") with { Volume = 2f, Pitch = 0f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        }
                        if (lifeleft == 1)//phase 2
                        {
                            if(!StormWorld.ultimateBossDown && player.HasItem(ModContent.ItemType<PainSword>()))
                            {
                                Paintext = "Hey that's cheating...";
                            }
                            else if (StormWorld.ultimateBossDown)
                            {
                                Paintext = "Well I'll still show you pain...";
                            }
                            else
                            {
                                Paintext = "Get ready for More Pain...";
                            }
                            Projectile.NewProjectile(null, NPC.Center.X, NPC.Center.Y, 0f, 0f, 305, 0, 0f, player.whoAmI, Main.myPlayer, (player.statLifeMax2 - player.statLife) * 0.75f); //Restore 75% of lost health
                           
                            SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSound") with { Volume = 2f, Pitch = -0.15f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        }
                        else if (lifeleft == 2)//phase 3
                        {                     
                            if (!StormWorld.ultimateBossDown && player.HasItem(ModContent.ItemType<PainSword>()))
                            {
                                Paintext = "Come on fight me fairly first...";
                            }
                            else if (StormWorld.ultimateBossDown)
                            {
                                Paintext = "Let's keep going...";
                            }
                            else
                            {
                                Paintext = "The Pain will continue...";
                            }
                            Projectile.NewProjectile(null, NPC.Center.X, NPC.Center.Y, 0f, 0f, 305, 0, 0f, player.whoAmI, Main.myPlayer, (player.statLifeMax2 - player.statLife) * 0.5f); //Restore 50% of lost health
                          
                            SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSound") with { Volume = 2f, Pitch = -0.3f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        }
                        else if (lifeleft == 3)//phase 4
                        {             
                            if (!StormWorld.ultimateBossDown && player.HasItem(ModContent.ItemType<PainSword>()))
                            {
                                Paintext = "Please..........";
                            }
                            else if (StormWorld.ultimateBossDown)
                            {
                                Paintext = "This still hurts me...";
                            }
                            else
                            {
                                Paintext = "I'm in more Pain here...";
                            }
                            Projectile.NewProjectile(null, NPC.Center.X, NPC.Center.Y, 0f, 0f, 305, 0, 0f, player.whoAmI, Main.myPlayer, (player.statLifeMax2 - player.statLife) * 0.25f); //Restore 25% of lost health
                          
                            SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSound") with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        }
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.DeepPink);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText(Paintext, Color.DeepPink);
                        }
                        CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.DeepPink, Paintext, true);
                        player.AddBuff(ModContent.BuffType<YouCantEscapeDebuff>(), 9999999);

                        NPC.netUpdate = true;
                    }
                    if (NPC.ai[0] >= 300)
                    {
                        NPC.dontTakeDamage = false;
                    }
                    if (NPC.ai[0] >= 300 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.dontTakeDamage = false;
                        NPC.ai[3]++; //Always do first attack frist
                                     //NPC.ai[3] = 5; //Testing
                        NPC.ai[0] = 0;
                        NPC.netUpdate = true;

                    }
                }
                Attacks(player); //All attacks are in this hook

            }
            //Movement is the same in all attacks
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (distanceToIdlePosition > 500f)
                {
                    // Speed up the boss if it's away from the player
                    speed = 40f;
                    inertia = 10f;
                }
                else
                {
                    // Slow down the boss if closer to the player
                    speed = 18f;
                    inertia = 20f;
                }
                NPC.netUpdate = true;
            }

            //Phases, resets Ai to passive
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.life >= NPC.lifeMax * 0.75f) //redundant but :ech:
                {
                    lifeleft = 0;
                }
                if (NPC.life < NPC.lifeMax * 0.75f  && lifeleft == 0) //Phase 1 > 2
                {
                    NPC.ai[3] = 0;
                    NPC.ai[0] = 0;
                    NPC.localAI[0] = 0;//Reset all ai values

                    lifeleft = 1;
                    NPC.dontTakeDamage = true;
                    //SoundEngine.PlaySound(SoundID.Roar with { Volume = 1f, Pitch = 0.5f }, NPC.Center);
                    SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                    SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/RoarSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                    NPC.netUpdate = true;

                }
                if (NPC.life < NPC.lifeMax * 0.5f && lifeleft == 1)//Phase 2 > 3
                {
                    NPC.ai[3] = 0;
                    NPC.ai[0] = 0;
                    NPC.localAI[0] = 0;//Reset all ai values

                    lifeleft = 2;
                    NPC.dontTakeDamage = true;
                    SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                    SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/RoarSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                    NPC.netUpdate = true;

                }
                if (NPC.life < NPC.lifeMax * 0.25f && lifeleft == 2) //Phase 3 > 4
                {
                    NPC.ai[3] = 0;
                    NPC.ai[0] = 0;
                    NPC.localAI[0] = 0;//Reset all ai values

                    lifeleft = 3;
                    NPC.dontTakeDamage = true;
                    SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                    SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/RoarSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                    NPC.netUpdate = true;

                }
            }
        }

        private void Attacks(Player player)//___________________________________________________________________________________________________________________________________________________
        {
            //NPC.localAI[2] = rotation
            //NPC.localAI[3] = rotation direction

            //MOVEMENT, always orbits player
            //DEBUG DUST
            /*var dustt = Dust.NewDustDirect(new Vector2(player.Center.X + NPC.ai[1], player.Center.Y + NPC.ai[2]), 0, 0, 138);
            dustt.velocity *= 0;
            dustt.noGravity = true;*/
            //Attacks Movement===========================================
            if (NPC.ai[3] > 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (NPC.localAI[3] == 0)//Loop round once then switch direction
                    {
                        NPC.localAI[2] += 1.8f;//rotation

                        if (NPC.localAI[2] >= 550)
                        {
                            NPC.localAI[3] = 1; //Rotate anti clockwise;
                        }
                    }
                    else
                    {
                        NPC.localAI[2] -= 1.8f;//rotation
                        if (NPC.localAI[2] <= 0)
                        {
                            NPC.localAI[3] = 0; //Rotate clockwise;
                        }
                    }
                    NPC.netUpdate = true;
                }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    double deg = (NPC.localAI[2]);
                    double rad = deg * (Math.PI / 180);
                    double dist = 450; //Distance away from the player

                    //position

                    NPC.ai[1] = (int)(Math.Cos(rad) * dist);
                    NPC.ai[2] = (int)(Math.Sin(rad) * dist);
                    NPC.netUpdate = true;
                }

            }
            //=================================
            if (Main.getGoodWorld)
            {
                if (Main.masterMode) //Projectile changes, same for all attacks
                {
                    projdamage = (50 * clamteadmg) / 100; //200 Expert>Master, 300 Master>Legendary
                   //Main.NewText("Master " + projdamage, Color.Red);
                }
                else if (Main.expertMode && !Main.masterMode) //only muliplies by x1 in GFB
                {
                    projdamage = (80 * clamteadmg) / 100; //160 Classic > Expert
                    //Main.NewText("Expert " + projdamage, Color.Orange);
                }
                else
                {
                    projdamage = (60 * clamteadmg) / 100; //120 unused
                   // Main.NewText("Classic " + projdamage, Color.White);
                }
                projvelocity = 1.5f;
                if (lifeleft != 3)
                    projcount = (Main.rand.Next(8, 11)); // 8-10
                else
                    projcount = (Main.rand.Next(10, 13)); // 10-12
            }
            /*else
            {
                if (Main.masterMode)
                {
                    projdamage = (60 * clamteadmg) / 100; //360
                    projvelocity = 2f;
                    if (lifeleft != 3)
                        projcount = (Main.rand.Next(8, 14)); // 8-13
                    else
                        projcount = (Main.rand.Next(10, 17)); // 10-16

                }
                else if (Main.expertMode && !Main.masterMode)
                {
                    projdamage = (75 * clamteadmg) / 100; //300
                    projvelocity = 1.8f;
                    if (lifeleft != 3)

                        projcount = (Main.rand.Next(6, 11)); // 6-10
                    else
                        projcount = (Main.rand.Next(8, 14)); // 8-13
                }
                else
                {
                    projdamage = (120 * clamteadmg) / 100; //240
                    projvelocity = 1.6f;
                    if (lifeleft != 3)

                        projcount = (Main.rand.Next(5, 9)); // 5-8
                    else
                        projcount = (Main.rand.Next(7, 12)); // 7-11
                }
            }*/

            //Change attack
            if (NPC.ai[3] > 0 && NPC.ai[0] >= Main.rand.Next(300, 500) && lifeleft != 3 && Main.netMode != NetmodeID.MultiplayerClient) //Attack order is randomised after first attack
            {
                NPC.localAI[0] = 0;//Reset all ai values

                if (lifeleft == 0)
                    NPC.ai[3] = (Main.rand.Next(1, 5)); //1-4
                else if (lifeleft == 1)
                    NPC.ai[3] = (Main.rand.Next(1, 7)); //1-6
                else if (lifeleft == 2)
                    NPC.ai[3] = (Main.rand.Next(1, 9)); //1-8
                NPC.ai[0] = 0;
                //Main.NewText("Attack " + NPC.ai[3], 204, 101, 22);

                NPC.netUpdate = true;
            }

            if (NPC.ai[3] == 1) //Attack 1 fire projectiles directly at player
            {
                NPC.ai[0]++;
                NPC.localAI[0]++;

                if (NPC.ai[0] > 30 || (lifeleft == 3 && NPC.ai[0] > 15)) //Delay before firing
                {
                    if ((NPC.localAI[0] > 30 && (lifeleft == 0)) || (NPC.localAI[0] > 25 && (lifeleft == 1 || lifeleft == 3)) || (NPC.localAI[0] > 20 && lifeleft == 2))
                    {
                        //Dust.QuickDustLine(player.Center, NPC.Center, 50, Color.DeepPink); //centre to centre

                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ShootSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        SoundEngine.PlaySound(SoundID.Item42 with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1f);
                            //Main.dust[dust2].noGravity = true;
                        }

                        for (int i = 0; i < projcount; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity; 
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                                //For the radius
                                double deg = Main.rand.Next(0, 360); //The degrees
                                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                                double dist = 60; //Distance away from the npc

                                float posX = NPC.Center.X - (int)(Math.Cos(rad) * dist);
                                float posY = NPC.Center.Y - (int)(Math.Sin(rad) * dist);

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(posX, posY), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);

                                /*Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                                        ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);*/
                            }
                        }
                        NPC.localAI[0] = 0;
                        if (lifeleft == 3)
                        {
                            NPC.ai[3] = (Main.rand.Next(1, 9)); //1-8
                            NPC.ai[0] = 0;
                        }
                    }
                }
            }

            if (NPC.ai[3] == 2) //Attack 2 rain projectiles from sky
            {
                NPC.ai[0]++;

                NPC.localAI[0]++;
                /*if (NPC.localAI[0] > 55) //telegraphing, NOPE >:)
                {
                    float speedY = -6f;
                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    int dust = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X + NPC.velocity.X, dustspeed.Y + NPC.velocity.Y, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                }*/
                if (NPC.ai[0] > 30 || (lifeleft == 3 && NPC.ai[0] > 15)) //Delay before firing
                {
                    if ((NPC.localAI[0] > 35 && (lifeleft == 0)) || (NPC.localAI[0] > 30 && (lifeleft == 1 || lifeleft == 3)) || (NPC.localAI[0] > 25 && lifeleft == 2))
                    {
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ShootSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        SoundEngine.PlaySound(SoundID.Item42 with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1f);
                            //Main.dust[dust2].noGravity = true;
                        }

                        for (int i = 0; i < projcount; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity; //slight predictive 
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                                float posX = player.position.X + Main.rand.NextFloat(1000f, -1000f);
                                float posY = player.position.Y + Main.rand.NextFloat(-650f, -650f);


                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(posX, posY), new Vector2(0, projvelocity), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 3, 0);

                                /*Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                                        ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);*/

                            }
                        }
                        NPC.localAI[0] = 0;
                        if (lifeleft == 3)
                        {
                            NPC.ai[3] = (Main.rand.Next(1, 9)); //1-8
                            NPC.ai[0] = 0;
                        }
                    }
                }
            }

            if (NPC.ai[3] == 3) //Attack 3 circle of projectiles around player
            {
                NPC.ai[0]++;


                NPC.localAI[0]++;
                /*if (NPC.localAI[0] > 55) //telegraphing, NOPE >:)
                {
                    float speedY = -6f;
                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    int dust = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X + NPC.velocity.X, dustspeed.Y + NPC.velocity.Y, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                }*/
                if (NPC.ai[0] > 30 || (lifeleft == 3 && NPC.ai[0] > 15)) //Delay before firing
                {
                    if ((NPC.localAI[0] > 50 && (lifeleft == 0)) || (NPC.localAI[0] > 45 && (lifeleft == 1 || lifeleft == 3)) || (NPC.localAI[0] > 40 && lifeleft == 2 ))
                    {
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ShootSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        SoundEngine.PlaySound(SoundID.Item42 with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1f);
                            //Main.dust[dust2].noGravity = true;
                        }

                        for (int i = 0; i < projcount; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity; //slight predictive 
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                                //For the radius
                                double deg = Main.rand.Next(0, 360); //The degrees
                                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                                double dist = 500; //Distance away from the player

                                float posX = player.Center.X - (int)(Math.Cos(rad) * dist);
                                float posY = player.Center.Y - (int)(Math.Sin(rad) * dist);

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(posX, posY), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 1, 0);
                            }
                        }
                        NPC.localAI[0] = 0;
                        if (lifeleft == 3)
                        {
                            NPC.ai[3] = (Main.rand.Next(1, 9)); //1-8
                            NPC.ai[0] = 0;
                        }
                    }
                }
            }
           
            if (NPC.ai[3] == 4) //Attack 4 fire out projectiles in random directions
            {
                NPC.ai[0]++;

                NPC.localAI[0]++;
                if (NPC.ai[0] > 30 || (lifeleft == 3 && NPC.ai[0] > 15)) //Delay before firing
                {
                    if ((NPC.localAI[0] > 30 && (lifeleft == 0))  || (NPC.localAI[0] > 25 && (lifeleft == 1 || lifeleft == 3)) || (NPC.localAI[0] > 20 && lifeleft == 2))
                    {
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ShootSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        SoundEngine.PlaySound(SoundID.Item42 with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1f);
                            //Main.dust[dust2].noGravity = true;
                        }

                        for (int i = 0; i < projcount; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity;
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(360));

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                            }
                        }
                        NPC.localAI[0] = 0;

                        if (lifeleft == 3)
                        {
                            NPC.ai[3] = (Main.rand.Next(1, 9)); //1-8
                            NPC.ai[0] = 0;
                        }
                    }
                }
            }
            if (NPC.ai[3] == 5) //Attack 5 rapidly summon projectiles in a plus shape (Phase 2+ only)
            {
                NPC.ai[0]++;
                NPC.localAI[0]++;
                  
                if (NPC.ai[0] > 30 || (lifeleft == 3 && NPC.ai[0] > 15)) //Delay before firing
                {
                    if ((NPC.localAI[0] > 30 && (lifeleft == 0)) || (NPC.localAI[0] > 25 && (lifeleft == 1 || lifeleft == 3)) || (NPC.localAI[0] > 20 && lifeleft == 2))
                    {
                        for (int i = 0; i < 200; i++)
                        {                         
                            int dust2 = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 600), 0, 1200, 72, 0f, 0f, 0, default, 1f);
                            Main.dust[dust2].noGravity = true;
                            Main.dust[dust2].velocity *= 0;                      

                            int dust4 = Dust.NewDust(new Vector2(NPC.Center.X - 600, NPC.Center.Y), 1200, 0, 72, 0f, 0f, 0, default, 1f);
                            Main.dust[dust4].noGravity = true;
                            Main.dust[dust4].velocity *= 0;
                        }

                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ShootSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        SoundEngine.PlaySound(SoundID.Item42 with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                float rotation = MathHelper.ToRadians(180);
                                float numberProjectiles = 4; //4 to 5

                                for (int j = 0; j < numberProjectiles; j++)
                                {
                                    Vector2 perturbedSpeed = new Vector2(0, projvelocity).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + Main.rand.Next(-50, 50), NPC.Center.Y + Main.rand.Next(-50, 50)), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 5, 0);
                                }
                            }
                        }

                        NPC.localAI[0] = 0;

                        if (lifeleft == 3)
                        {
                            NPC.ai[3] = (Main.rand.Next(1, 9)); //1-8
                            NPC.ai[0] = 0;
                        }
                    }
                }
            }
            if (NPC.ai[3] == 6) //Attack 6 summon random projectiles that splode early (Phase 2+ Only)
            {
                NPC.ai[0]++;

                NPC.localAI[0]++;
                /*if (NPC.localAI[0] > 55) //telegraphing, NOPE >:)
                {
                    float speedY = -6f;
                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    int dust = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X + NPC.velocity.X, dustspeed.Y + NPC.velocity.Y, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                }*/
                if (NPC.ai[0] > 30 || (lifeleft == 3 && NPC.ai[0] > 15)) //Delay before firing
                {
                    if ((NPC.localAI[0] > 30 && (lifeleft == 0)) || (NPC.localAI[0] > 25 && (lifeleft == 1 || lifeleft == 3)) || (NPC.localAI[0] > 20 && lifeleft == 2))
                    {
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ShootSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        SoundEngine.PlaySound(SoundID.Item42 with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1f);
                            //Main.dust[dust2].noGravity = true;
                        }

                        for (int i = 0; i < projcount; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity; //slight predictive 
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                                //For the radius
                                double deg = Main.rand.Next(0, 360); //The degrees
                                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                                double dist = Main.rand.Next(50, 500); //Distance away from the player

                                float posX = player.Center.X - (int)(Math.Cos(rad) * dist);
                                float posY = player.Center.Y - (int)(Math.Sin(rad) * dist);

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(posX, posY), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 2, 0);
                            }
                        }
                        NPC.localAI[0] = 0;
                        if (lifeleft == 3)
                        {
                            NPC.ai[3] = (Main.rand.Next(1, 9)); //1-8
                            NPC.ai[0] = 0;
                        }
                    }
                }
            }

            if (NPC.ai[3] == 7) //Attack 7 projectiles from side (Phase 3+ Only)
            {
                NPC.ai[0]++;

                NPC.localAI[0]++;
                /*if (NPC.localAI[0] > 55) //telegraphing, NOPE >:)
                {
                    float speedY = -6f;
                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    int dust = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X + NPC.velocity.X, dustspeed.Y + NPC.velocity.Y, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                }*/
                if (NPC.ai[0] > 30 || (lifeleft == 3 && NPC.ai[0] > 15)) //Delay before firing
                {
                    if ((NPC.localAI[0] > 40 && (lifeleft == 0)) || (NPC.localAI[0] > 35 && (lifeleft == 1 || lifeleft == 3)) || (NPC.localAI[0] > 30 && lifeleft == 2))
                    {
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ShootSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        SoundEngine.PlaySound(SoundID.Item42 with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1f);
                            //Main.dust[dust2].noGravity = true;
                        }

                        for (int i = 0; i < projcount; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                if (Main.rand.Next(2) == 0)//left to right
                                {
                                    float posX = player.position.X + Main.rand.NextFloat(-1050, -1050f);
                                    float posY = player.position.Y + Main.rand.NextFloat(-600f, 600f);

                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(posX, posY), new Vector2(projvelocity, 0), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 4, 0);
                                }
                                else//right to left
                                {
                                    float posX = player.position.X + Main.rand.NextFloat(1050, 1050f);
                                    float posY = player.position.Y + Main.rand.NextFloat(-600f, 600f);

                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(posX, posY), new Vector2(-projvelocity, 0), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 4, 0);

                                }
                                /*Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                                        ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 0, 0);*/

                            }
                        }
                        NPC.localAI[0] = 0;
                        if (lifeleft == 3)
                        {
                            NPC.ai[3] = (Main.rand.Next(1, 9)); //1-8
                            NPC.ai[0] = 0;
                        }
                    }
                }      
            }
            if (NPC.ai[3] == 8) //Attack 8 rapidly summon projectiles that charge towards player (Phase 3+ Only)
            {
                NPC.ai[0]++;
                NPC.localAI[0]++;

                if (NPC.ai[0] > 30 || (lifeleft == 3 && NPC.ai[0] > 15)) //Delay before firing
                {
                    if ((NPC.localAI[0] > 14 && (lifeleft == 0)) || (NPC.localAI[0] > 12 && (lifeleft == 1 || lifeleft == 3)) || (NPC.localAI[0] > 10 && lifeleft == 2))
                    {
                        //SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ShootSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                        SoundEngine.PlaySound(SoundID.Item42 with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -6f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1f);
                            //Main.dust[dust2].noGravity = true;
                        }

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>(), projdamage, 1, Main.myPlayer, 1, -40); //Little extra time
                        }

                        NPC.localAI[0] = 0;

                        if (lifeleft == 3)
                        {
                            NPC.ai[3] = (Main.rand.Next(1, 9)); //1-8
                            NPC.ai[0] = 0;
                        }
                    }
                }
            }
            
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.DeepPink, "How's that for Pain?", true);
            if (Main.netMode == 2) // Server
            {
                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("How's that for Pain?"), Color.DeepPink);
            }
            else if (Main.netMode == 0) // Single Player
            {
                Main.NewText("How's that for Pain?", Color.DeepPink);
            }
            //UNUSED
        }
        int npcframe = 0;
        public override void FindFrame(int frameHeight)//Animations
        {
            NPC.frame.Y = npcframe * frameHeight;
            //NPC.spriteDirection = NPC.direction;
            NPC.frameCounter++;
            if (!deathani)
            {
                if (lifeleft == 0) //Phase 1
                {
                    /*if (NPC.frameCounter > 6)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }*/
                    //if (npcframe <= 0 || npcframe >= 3) //Cycles through frames 0-3 for attacking
                    {
                        npcframe = 0;
                    }
                }
                if (lifeleft == 1) //Phase 2
                {
                    npcframe = 1;
                }
                if (lifeleft == 2) //Phase 3
                {
                    npcframe = 2;
                }
                if (lifeleft == 3) //Phase 4
                {
                    npcframe = 3;
                }
            }
            else
            {
                npcframe = 4;
            }
        }
        public override void BossLoot(ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
            //boss trophy
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.BossTrophy.PainBossTrophy>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Furniture.VortexiaPaintingItem>(), 1));
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PainMusicBoxitem>(), 10));

            //pain normal mode
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.PainSword>(), 1));
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanitysets.BossMaskUltimateBoss>(), 7));

            //expert and master loot
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.BossTrophy.PainBossBag>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.BossTrophy.PainBossRelic>()));
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<Items.Pets.UltimateBossPetItem>(), 4));
            npcLoot.Add(notExpert);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/PainSound") with { Volume = 1f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
            for (int i = 0; i < 10; i++)
            {
                float speedY = -4f;

                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 72, dustspeed.X, dustspeed.Y, 100, default, 1f);
                //Main.dust[dust2].noGravity = true;
            }
            if (NPC.life <= 0 && deathani)          //this make so when the npc has 0 life(dead) he will spawn this
            {
               
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj2>(), 0, 0, Main.myPlayer);
                Main.projectile[proj].scale = 2.5f;
                SoundEngine.PlaySound(SoundID.Item14 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSound") with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        int xgorepos = Main.rand.Next(-70, 70);
                        int ygorepos = Main.rand.Next(-70, 70);
                        Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X + xgorepos, NPC.Center.Y + ygorepos), NPC.velocity, Mod.Find<ModGore>("ThePainBossGore1").Type, 1f);
                    }
                }
                for (int i = 0; i < 150; i++)
                {

                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 72);
                    dust.velocity *= 2;
                }
                for (int i = 0; i < 150; i++)
                {
                    float speedY = -8f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1.5f);
                    //Main.dust[dust2].noGravity = true;
                }
            }
        }
        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref StormWorld.ultimateBossDown, -1); //set boss downed      
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 1.5f;
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 1.5f;
            if (!StormWorld.ultimateBossDown)
            {
                if (projectile.type == ProjectileID.FinalFractal) //Zenith is cheating >:(
                {
                    //modifiers.FinalDamage.Flat -= 9999; 
                    //modifiers.DisableCrit();
                    SoundEngine.PlaySound(SoundID.Item16 with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                    Paintext = "Zenith means no Pain >:(";
                    if (!Zenithtext)
                    {
                        CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 12, 4), Color.DeepPink, Paintext, true);

                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.DeepPink);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText(Paintext, Color.DeepPink);
                        }
                        Zenithtext = true;
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
                Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/Boss/ThePainBoss");

                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + NPC.frame.Size() / 2f + new Vector2(0, 2);
                    Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
                float speen1 = 9f + 3f * (float)Math.Cos((float)Math.PI * 2f * Main.GlobalTimeWrappedHourly);
                Vector2 spinningpoint5 = Vector2.UnitX * speen1;

                Color color2 = Color.Pink * (speen1 / 12f) * 0.8f;
                color2.A /= 3;
                if (!deathani)
                {
                    for (float speen2 = 0f; speen2 < (float)Math.PI * 2f; speen2 += (float)Math.PI / 2f)
                    {
                        Vector2 finalpos = NPC.position + new Vector2(0, 40) + spinningpoint5.RotatedBy(speen2);
                        spriteBatch.Draw(texture, new Vector2(finalpos.X - screenPos.X + (float)(NPC.width / 2) * NPC.scale, finalpos.Y - screenPos.Y + (float)NPC.height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f * NPC.scale), NPC.frame, color2, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    }
                }
            }
            return true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            position += new Vector2(0, 0); //Moves healthbar down a little
            return true;
        }
    }
}