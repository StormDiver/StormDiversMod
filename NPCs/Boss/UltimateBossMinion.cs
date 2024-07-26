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
using Terraria.GameContent.UI.BigProgressBar;

namespace StormDiversMod.NPCs.Boss

{
    public class TheUltimateBossMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pain Crystal"); 
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 7;
            
            NPC.width = 50;
            NPC.height = 50;

            NPC.aiStyle = -1;

            NPC.damage = 10;//Contact damage removed in AI

            NPC.defense = 75;
            
            NPC.lifeMax = 30000;

            NPC.HitSound = SoundID.Item25;
            NPC.DeathSound = SoundID.Item107;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.noGravity = true;
            NPC.despawnEncouraged = false;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            //Banner = Item.NPCtoBanner(ModContent.NPCType<HellMiniBossMinion>());
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            //Unlock automatically when main derpling is defeated
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPCType<TheUltimateBoss>()], quickUnlock: true);
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Crystals formerly attached to the Painbringer that summon damaging skulls. Once the main boss has taken enough damage they separate from it and attack by themselves as a last resort.")
            });
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            if (Main.masterMode)
            {
                NPC.lifeMax = (int)(NPC.lifeMax * 0.67f * balance) - 100; //60000
            }
            else if (Main.expertMode && !Main.masterMode)
            {
                NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * balance); //45000

            }
            //NPC.damage = (int)(NPC.damage * 0.75f);
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                if ((bool)calamityMod.Call("GetDifficultyActive", "death"))
                {
                    NPC.lifeMax = (int)(NPC.lifeMax * 1.5f);
                }
                else if ((bool)calamityMod.Call("GetDifficultyActive", "revengeance"))
                {
                    NPC.lifeMax = (int)(NPC.lifeMax * 1.25f);
                }
            }
        }
        
        NPC target; //target boss?
        Player player; //player target
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 50; i++)
            {
                float speedY = -3f;

                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1f);
                Main.dust[dust2].noGravity = true;
            }
        }
        int projdamage;
        float projvelocity;
        int projcount;
        float projspread;
        float movespeed = 1;
        int shootspeed = 1;
        bool canshoot = true;
        bool fastshoot = false;
        int clamteadmg = 100;
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
            //if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.boss = true;//prevent despawn
            }
            NPC.damage = 0;
            if (NPC.CountNPCS(ModContent.NPCType<TheUltimateBoss>()) > 0)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].type == ModContent.NPCType<TheUltimateBoss>() && Main.npc[i].active)
                    {
                        target = Main.npc[i];
                    }
                }
            }
            else target = null;

            if (NPC.CountNPCS(ModContent.NPCType<TheUltimateBoss>()) == 0)
            {
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj4>(), 0, 0, Main.myPlayer);
                Main.projectile[proj].scale = 0.8f;
                for (int i = 0; i < 4; i++)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X, NPC.Center.Y), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore13").Type, 1f);
                    }
                }
                for (int i = 0; i < 30; i++)
                {
                    float speedY = -4f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1.5f);
                    Main.dust[dust2].noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                NPC.boss = false; //no death message

                NPC.life = 0;

                return;
            }
            
            //===============AI fields================
            //NPC.ai[0] = Shootime
            //NPC.ai[1] = X postion
            //NPC.ai[2] = Y postion
            //NPC.localAI[1] = Rotation
            //NPC.LocalAI[2] = Time until attacks

            //========================================

            NPC.noTileCollide = true;
            /*if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }*/
            Dust.QuickDust(NPC.Center, Color.DeepPink);
            if (target != null)
            {
                if ((target.localAI[0] > 300 && target.localAI[0] < 360) || target.localAI[0] > 600) //no shooting between phases          
                    canshoot = false;

                else
                    canshoot = true;

                if (target.localAI[0] >= 360 && target.localAI[0] < 600) //fast firing
                    fastshoot = true;
                else
                    fastshoot = false;

                if (!canshoot) //rotation //when charging attack no rotation
                    NPC.localAI[1] = NPC.localAI[1];

                else if (fastshoot) //Shooting fast rotate anticlock
                    NPC.localAI[1] -= projcount;
                else //else clockwise
                    NPC.localAI[1] += projcount * 2;

                player = Main.player[target.target];

            }
            //Movement
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                double deg = (NPC.localAI[1]);
                double rad = deg * (Math.PI / 180);
                double dist = 100; //Distance away from the npc

                //position
                NPC.ai[1] = (int)(Math.Cos(rad) * dist); //X pos
                NPC.ai[2] = (int)(Math.Sin(rad) * dist); //Y pos

                Vector2 idlePosition = target.Center + new Vector2(NPC.ai[1], NPC.ai[2]);
                Vector2 vectorToIdlePosition = idlePosition - NPC.Center;
                if (movespeed < 50)
                {
                    movespeed += 0.4f;
                }
                NPC.rotation = (target.Center - NPC.Center).ToRotation();

                Vector2 moveTo = target.Center;
                Vector2 move = moveTo - NPC.Center + new Vector2(NPC.ai[1], NPC.ai[2]); //Postion around player
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > movespeed)
                {
                    move *= movespeed / magnitude;
                }
                NPC.velocity = move;
                NPC.netUpdate = true;

            }
            //Damage values and projspeed===============================
            if (Main.getGoodWorld)
            {
                projdamage = (70 * clamteadmg) / 100; //140/280/420 on ftw
                projvelocity = 1.4f;
            }
            else if (Main.masterMode)
            {
                projdamage = (50 * clamteadmg) / 100; // 300 on master               
                projvelocity = 1.3f;
            }
            else if (Main.expertMode && !Main.masterMode)
            {
                projdamage = (55 * clamteadmg) / 100; // 220 On expert
                projvelocity = 1.2f;
            }
            else
            {
                projdamage = (70 * clamteadmg) / 100; // 140 on normal
                projvelocity = 1.1f;
            }
            //projectile count and spread
            if (NPC.CountNPCS(ModContent.NPCType<TheUltimateBossMinion>()) == 1) 
            {
                projcount = 4;
                projspread = 30; 
            }
            else if (NPC.CountNPCS(ModContent.NPCType<TheUltimateBossMinion>()) == 2)
            {
                projcount = 3;
                projspread = 20;
            }
            else if (NPC.CountNPCS(ModContent.NPCType<TheUltimateBossMinion>()) == 3)
            {
                projcount = 2;
                projspread = 10;
            }
            else
            {
                projcount = 1;
                projspread = 0; 
            }
            if (!canshoot) //charge up between 2 attacks
            {
                for (int i = 0; i < 5; i++)
                {
                    float speedY = -3f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust1 = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y), 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1f);
                    Main.dust[dust1].noGravity = true;
                }
                if (target.localAI[0] % 20 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item15 with { Volume = 1f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, NPC.Center);

                }
            }

            //teleport to main npc if too far
            float distance = Vector2.Distance(target.Center, NPC.Center);
            if (distance > 1000)
            {
                NPC.position = target.Center;
            }
            NPC.localAI[2]++;
            if (NPC.localAI[2] >= 180 && !player.dead)
            {
                NPC.localAI[0]++;

                if (fastshoot) //fast shooting
                {
                    shootspeed = 4;
                }
                else //normal shooting 
                {
                    shootspeed = 21 - (projcount * 2);
                }
                if (canshoot)
                {
                    NPC.ai[0]++;//Shoot time
                }
                else
                {
                    NPC.ai[0] = 0;
                }
                if (NPC.ai[0] >= shootspeed && canshoot && target != null)
                {

                    for (int j = 0; j < 5; j++)
                    {
                        float speedY = -6f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust1 = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y), 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1.25f);
                        Main.dust[dust1].noGravity = true;

                    }

                    int type = ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>();

                    //Main.NewText("ffs " + projcount + projspread, 204, 101, 22);

                    Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * -projvelocity;
                    float rotation = MathHelper.ToRadians(projspread);
                    //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                    if (!fastshoot)
                    {
                        for (int k = 0; k < projcount; k++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(projspread));

                                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, projdamage, 2, Main.myPlayer, 0);
                                Main.projectile[proj].scale = 0.75f;
                            }
                        }
                        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { Volume = 1.2f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, NPC.Center);

                    }
                    else //when fast shooting only fire 1 projectile
                    {
                        int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(velocity.X, velocity.Y), type, projdamage, 2, Main.myPlayer, 0);
                        Main.projectile[proj].scale = 0.75f;
                        SoundEngine.PlaySound(SoundID.Item42 with { Volume = 1.2f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);

                    }
                    NPC.ai[0] = 0;
                }

            }

            /*if (Main.rand.Next(4) == 0)
            {
                var dust3 = Dust.NewDustDirect(new Vector2(NPC.Center.X, NPC.Center.Y), NPC.width, NPC.height, 72, 0, 3);
                dust3.noGravity = false;
            }*/
        }

        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = npcframe * frameHeight;
            NPC.frameCounter++;

            if (NPC.frameCounter > 8)
            {
                npcframe++;
                NPC.frameCounter = 0;
            }
            if (npcframe >= 7) //Cycles through frames 0-6
            {
                npcframe = 0;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
                        
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                NPC.boss = false; //no death message
            }
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 5; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 72);
                dust.noGravity = true;

            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj4>(), 0, 0, Main.myPlayer);
                Main.projectile[proj].scale = 0.8f;
                for (int i = 0; i < 4; i++)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X, NPC.Center.Y), NPC.velocity, Mod.Find<ModGore>("TheUltimateBossGore13").Type, 1f);
                    }
                }
                for (int i = 0; i < 30; i++)
                {
                    float speedY = -4f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(NPC.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1.5f);
                    Main.dust[dust2].noGravity = true;
                }
                NPC.boss = false;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 250;
            return color;
        }
    }
}