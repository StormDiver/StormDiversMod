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
using StormDiversMod.Basefiles;
using Terraria.DataStructures;
using StormDiversMod.Items.BossTrophy;
using StormDiversMod.Items.Weapons;
using StormDiversMod.NPCs;
using System.IO;
using Terraria.GameContent.Generation;
using Terraria.Localization;
using Terraria.ModLoader.IO;

using StormDiversMod.Items.Pets;
using StormDiversMod.Projectiles;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StormDiversMod.Buffs;

namespace StormDiversMod.NPCs.Boss
{
    [AutoloadBossHead]
    public class StormBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Overloaded Scandrone"); // Automatic from .lang files
                                                 // make sure to set this for your modnpcs.
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 7;
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
            
            Main.npcFrameCount[NPC.type] = 24;
            
            NPC.width = 75;
            NPC.height = 75;

            NPC.aiStyle = -1;
          
            NPC.noGravity = true;
            NPC.damage = 100; //100/150/225
            
            NPC.defense = 25;
            NPC.lifeMax = 40000;
            
            NPC.gfxOffY = -12;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 15, 0, 0);
            NPC.boss = true;
            if (!Main.dedServ)
            {
                Music = MusicID.Boss4;
            }
            NPC.npcSlots = 10f;         
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A terrifying experiment that escaped from the vortex homeworld prematurely, infused with tons of lunar energy, " +
                "this empowered Scandrone is capable of unleashing a range of powerful attacks onto foes.")
            });
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            if (!Main.masterMode)
            {
                NPC.lifeMax = (int)(NPC.lifeMax * 0.75f); //60K
            }
            else
            {
                NPC.lifeMax = (int)(NPC.lifeMax * 0.7f); //84K 

            }
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
    
        float movespeed = 5f; //Speed of the npc
        float distance; //distance to player
        bool rotateright = false; //What side is the player on so it can rotate in the right direction

        bool charge = false; //s the boss doing any charge attack? If so ignore defualt movement

        bool halflife;//Second phase
        bool lowlife;//third phase
        float rotation; //Rotation of NPC when orbiting
        bool animateclaws; //Wheter the claws move

        int projdamage; //Damage of all projectiles

        int portalamount; //how many portals to summon

        int clamteadmg = 100; // 1x (100) normaly, 1.33x (133) for rev and 1.66x (166) for death

        public static int phase2HeadSlot = -1;
        public static int phase3HeadSlot = -1;

        bool deathani;

        public override void Load()
        {
            string texture = BossHeadTexture + "_Phase2"; // Texture Name
            phase2HeadSlot = Mod.AddBossHeadTexture(texture, -1); // -1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise

            string texture2 = BossHeadTexture + "_Phase3"; // Texture Name
            phase3HeadSlot = Mod.AddBossHeadTexture(texture2, -1); // -1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise
        }
        public override void BossHeadSlot(ref int index)
        {
            int slot = phase2HeadSlot;
            if (halflife && !lowlife && slot != -1)
            {
                // If the boss is in its second stage, display the other head icon instead
                index = slot;
            }
            int slot2 = phase3HeadSlot;
            if (lowlife && slot2 != -1)
            {
                // If the boss is in its third stage, display the 3rd icon instead
                index = slot2;
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
            //Main.NewText("pain " + clamteadmg, 175, 17, 96);

            if (Main.netMode != NetmodeID.Server)
            {
                // For visuals regarding NPC position, netOffset has to be concidered to make visuals align properly
                NPC.position += NPC.netOffset;
                NPC.position -= NPC.netOffset;
            }
            //===============AI fields================

            //NPC.ai[0] = time until next attack
            //NPC.ai[1] = X postion
            //NPC.ai[2] = Y postion
            //NPC.ai[3] = Phase
            //NPC.localAI[0] = Shootime
            //NPC.localAI[1] = death counter despawn
            //NPC.localAI[2 and 3] = Attack dependant

            //========================================
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            Player player = Main.player[NPC.target]; //Code to move towards player
            //Main movement, xpos and ypos change per attack, code ignroed when charging

            distance = Vector2.Distance(player.Center, NPC.Center);

            if (Main.getGoodWorld) //defense break barrier for ftw
            {
                for (int i = 0; i < 50; i++) 
                {
                    double deg = Main.rand.Next(0, 360); //The degrees
                    double rad = deg * (Math.PI / 180); //Convert degrees to radians
                    double dist = 500; //distance away from the player
                    float dustx = NPC.Center.X - (int)(Math.Cos(rad) * dist);
                    float dusty = NPC.Center.Y - (int)(Math.Sin(rad) * dist);
                    if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, NPC.position, NPC.width, NPC.height))//no dust unless lien of sight
                    {
                        var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 160, 0, 0);
                        dust.noGravity = true;
                        dust.scale = 2.5f;
                        dust.velocity *= 0;
                    }
                }
                if (distance < 500)
                {
                    player.AddBuff(ModContent.BuffType<ScanDroneDebuff>(), 2);
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!charge && !player.dead && !deathani)
                {
                    Vector2 moveTo = player.Center;
                    Vector2 move = moveTo - NPC.Center + new Vector2(NPC.ai[1], NPC.ai[2]); //Postion around player
                    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                    if (magnitude > movespeed)
                    {
                        move *= movespeed / magnitude;
                    }
                    NPC.velocity = move;
                    NPC.velocity.X *= 0.9f;
                }
                NPC.netUpdate = true;
            }
            NPC.noTileCollide = true;

            //Main targetting code for projectiles          

            if (distance > 2000 && !player.dead && NPC.ai[3] != 0 && Main.netMode != NetmodeID.MultiplayerClient)//Speed up if too far away
            {

                movespeed *= 1.03f;
                NPC.netUpdate = true;

                //NPC.position.X = player.position.X;
                //NPC.position.Y = player.position.Y - 150;
            }
            if (distance > 10000 && NPC.ai[3] != 0)// Despawn if too far away
            {
                NPC.active = false;
            }
            if (charge) //melee damage while charging
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (!Main.expertMode)
                    {
                        NPC.damage = (100 * clamteadmg) / 100;
                    }
                    if (Main.expertMode && !Main.masterMode)
                    {
                        NPC.damage = (150 * clamteadmg) / 100;
                    }
                    if (Main.masterMode)
                    {
                        NPC.damage = (200 * clamteadmg) / 100;
                    }
                    NPC.netUpdate = true;
                }
            }
            else
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.damage = 0;
                    NPC.netUpdate = true;
                }
            }
           
            //____________________________________________________________________________________________________________________________________
            if (!player.dead && !deathani)
            {
                if (NPC.ai[3] == 0) //No attacks when first summoned, or when changing to phase 3
                {
                    animateclaws = true;
                   
                    if (!lowlife && NPC.ai[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient)//Set postion when first spawned
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = -200;
                        NPC.netUpdate = true;

                    }

                    NPC.ai[0]++;

                    if (!charge && NPC.ai[0] >= 60 && Main.netMode != NetmodeID.MultiplayerClient)  //After one second set new values (prevents boss from glitching out when spawned via cheatsheet, and makes boss back up before charging)
                    {
                        if (!lowlife)
                        {

                            movespeed = distance / 35;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = -250;
                        }
                        else if (lowlife) //If in phase 3, move further up before being charge
                        {

                            movespeed = distance / 25;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = -400;
                        }
                        NPC.netUpdate = true;
                    }
                    NPC.rotation = (player.MountedCenter - NPC.Center).ToRotation();
                 
                    if (!charge && NPC.ai[0] >= 120 && Main.netMode != NetmodeID.MultiplayerClient) //Change to first attack
                    {
                        if (!lowlife)
                        {
                            NPC.ai[3]++;
                        }
                        else if (lowlife) //If in phase 3, go to the last attack
                        {
                            NPC.defense = 0; //reduced defence to 0
                            NPC.dontTakeDamage = false;

                            NPC.ai[3] = 5;
                        }
                        NPC.ai[0] = 0;
                        NPC.netUpdate = true;

                    }
                }

                Attacks(player); //All attacks are in this hook

                if (NPC.ai[3] >= 7) //go back to first attack when cycle is complete 
                {
                    NPC.ai[3] = 1;
                }
            }
            //____________________________________________________________________________________________________________________________________
            if (player.dead && !deathani)//When player is dead fly away
            {
                NPC.ai[3] = 0;

                if (NPC.velocity.Y > -25)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.velocity.Y -= 0.5f;
                        NPC.netUpdate = true;
                    }
                }
                
                NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 0f;
                NPC.EncourageDespawn(60);

                NPC.localAI[1]++;
                if (NPC.localAI[1] > 180)
                {
                    NPC.active = false;
                }
            }
            else
            {
                NPC.localAI[1] = 0;
            }
            //________________________________________________________________________
            if (deathani) //DEATH ANIMATION=============================================================
            {
                NPC.dontTakeDamage = true;
                NPC.ai[0]++;
                NPC.rotation = NPC.ai[0] / 3;
                NPC.velocity *= 0.95f;
                NPC.position.X += Main.rand.Next(-1, 2);
                NPC.position.Y += Main.rand.Next(-1, 2);
                if (NPC.ai[0] % 8 == 0)
                {
                    int xprojpos = Main.rand.Next(-25, 25);
                    int yprojpos = Main.rand.Next(-25, 25);

                    int ProjID = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + xprojpos, NPC.Center.Y + yprojpos), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionVortexProj>(), 0, 0, Main.myPlayer);
                    Main.projectile[ProjID].scale = 0.6f;

                    for (int i = 0; i < 10; i++)
                    {
                        float speedY = -1f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(new Vector2(NPC.Center.X + xprojpos, NPC.Center.Y + yprojpos), 0, 0, 156, dustspeed.X, dustspeed.Y, 100, default, 1f);
                        Main.dust[dust2].noGravity = true;
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(0, 3).RotatedByRandom(MathHelper.ToRadians(360));
                    
                        float ai = Main.rand.Next(100);
                        int projID = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, 0),
                            ModContent.ProjectileType<NPCs.NPCProjs.StormBossLightning>(), 0, .5f, Main.myPlayer, perturbedSpeed.ToRotation(), ai);
                        Main.projectile[projID].scale = 0.75f;
                        Main.projectile[projID].timeLeft = 140;

                    }
                    //SoundEngine.PlaySound(SoundID.Item14 with { Volume = 1f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, NPC.Center);
                    SoundEngine.PlaySound(SoundID.Item96, NPC.Center);

                }
               
                if (NPC.ai[0] >= 180)
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 0);
                    NPC.checkDead();
                }
            }
            //Weather effects
            if (!GetInstance<ConfigurationsGlobal>().PreventBossStorm)
            {
                if (!player.dead && !deathani)
                {
                    if (Main.cloudAlpha < 0.97f)
                    {
                        Main.cloudAlpha += 0.03f;
                    }
                    if (Main.cloudAlpha >= 0.97f)
                    {
                        Main.cloudAlpha = 0.98f;
                    }
                    if (Main.windSpeedTarget < 0.7f)
                    {
                        Main.windSpeedTarget += 0.02f;
                    }
                    //Main.NewText("pain " + Main.cloudAlpha, 175, 17, 96);
                }
                else if (deathani)
                {
                    if (Main.cloudAlpha > 0.01f)
                    {
                        Main.cloudAlpha -= 0.005f;
                    }
                    if (Main.windSpeedTarget > 0f)
                    {
                        Main.windSpeedTarget -= 0.005f;
                    }
                }
                else if (player.dead) //clear twice as quickly if player dies
                {
                    if (Main.cloudAlpha > 0.02f)
                    {
                        Main.cloudAlpha -= 0.01f;
                    }
                    if (Main.windSpeedTarget > 0f)
                    {
                        Main.windSpeedTarget -= 0.01f;
                    }
                }
            }
            //Main.maxRaining = 1;

            if (!deathani)
            {
                if (!halflife && NPC.life < NPC.lifeMax / 2) //Below half health new attack
                {
                    NPC.localAI[0] = 0;//rest cooldown for attacks
                    SoundEngine.PlaySound(SoundID.Roar with { Volume = 1f, Pitch = 0.5f }, NPC.Center);
                    //if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 50; i++)
                        {

                            var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 226);
                            dust.noGravity = true;
                            dust.fadeIn = 1;
                            dust.scale = 1.5f;
                        }
                    }
                    halflife = true;
                }
                if (!lowlife && NPC.life < NPC.lifeMax / 10 && Main.expertMode) //Below 10% health one attack, expert+ only
                {
                    NPC.dontTakeDamage = true; //immune to damage for short time

                    SoundEngine.PlaySound(SoundID.Roar with { Volume = 1f, Pitch = 0.5f }, NPC.Center);

                    if (Main.netMode != NetmodeID.Server)//Drop some gore when changing phase
                    {
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StormBossGore4").Type, 1f);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StormBossGore5").Type, 1f);
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        //rest everything
                        charge = false;
                        NPC.localAI[0] = 0;
                        NPC.localAI[2] = 0;
                        NPC.localAI[3] = 0;

                        NPC.ai[3] = 0; //Revert to passive mode, when passive mode is over it will go to attack 5
                        NPC.ai[0] = 0;
                        NPC.velocity *= 0.5f;
                        movespeed = 12f;
                        NPC.ai[1] = 0;
                        NPC.ai[2] = -200;
                        NPC.netUpdate = true;
                    }
                    lowlife = true;
                }
            }
            if (halflife && !lowlife) //Below half life new particle effect
            {
                if (Main.rand.Next(5) == 0)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 220);
                    dust.noGravity = true;
                    dust.scale = 1f;
                }
            }
            if (lowlife) //Below 5% health only use dash attack and remove defence, also new particle effect
            {
                //defence added and removed in defence phase

                //if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 3; i++)
                    {

                        var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 226);
                        dust.noGravity = true;
                        dust.fadeIn = 0.5f;
                        dust.scale = 1f;
                    }
                }

            }
            else //when not in low life one basic particle effect
            {
                //if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Main.rand.Next(5) == 0 && !charge)
                    { 

                        var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 226);
                        dust.noGravity = true;
                        dust.scale = 1f;
                    }
                    if (Main.rand.Next(2) == 0 && charge)
                    {
                        var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 226);
                        dust.noGravity = true;
                        dust.scale = 1f;
                    }
                }
            }
       
        }
        Vector2 dashvelocity; //for attack 5 predictive dash
        private void Attacks(Player player)//___________________________________________________________________________________________________________________________________________________
        {
            //__________________________________________________________________________
            //NPC.localAI[2] = Oribit time
            //NPC.localAI[3] = Charge Time
            if (NPC.ai[3] == 1) //First attack, spin and charge
            {
                if (!charge) //Not charging
                {
                    if (Main.expertMode) //Projectile damage
                    {
                        projdamage = (25 * clamteadmg) / 100; // 100 On expert (150 on master)
                    }
                    else
                    {
                        projdamage = (35 * clamteadmg) / 100; // 70 on normal
                    }

                    animateclaws = true;
                    //phasetime++;
                    NPC.ai[0]++; //Only count to next phase when not charging
                    if (NPC.ai[0] == 1)//reset postion for phase
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            NPC.ai[1] = 0;
                            NPC.ai[2] = -150;

                            NPC.netUpdate = true;
                        }
                    }
                    if (NPC.ai[0] > 30) //Only start firing after a delay
                    {
                        NPC.localAI[0]++;
                    }
                    if (NPC.localAI[0] > 40 || (halflife && NPC.localAI[0] > 30))//dust warning
                    {
                        Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y));
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                        Dust dust;
                        dust = Terraria.Dust.NewDustPerfect(NPC.Center, 229, new Vector2((perturbedSpeed.X * 10) + (NPC.velocity.X), (perturbedSpeed.Y * 10) + NPC.velocity.Y), 0, new Color(255, 255, 255), 1.25f);
                        dust.noGravity = true;
                    }
                    if (NPC.localAI[0] > 50 || (halflife && NPC.localAI[0] > 40))//faster firing on half life
                    {
                        float projectileSpeed = 9f;
                        SoundEngine.PlaySound(SoundID.Item96, NPC.Center);

                        //if (Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            for (int i = 0; i < 50; i++)
                            {
                                Dust dust;
                                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                                dust = Terraria.Dust.NewDustPerfect(new Vector2(NPC.Center.X + 12 * NPC.direction, NPC.Center.Y), 156, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
                                dust.noGravity = true;
                                dust.scale = 2.5f;
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(7));

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                                    ModContent.ProjectileType<NPCs.NPCProjs.StormBossBolt>(), projdamage, 1);
                            }
                            NPC.netUpdate = true;
                        }
                        NPC.localAI[0] = 0;
                    }

                    NPC.rotation = (player.MountedCenter - NPC.Center).ToRotation();//Look at the player

                    NPC.localAI[3] = 0; //reset dash

                    if (Main.netMode != NetmodeID.MultiplayerClient) //movement speed
                    {
                        movespeed = 1f * distance / 30;
                        NPC.netUpdate = true;
                    }

                    if (distance > 500) //Travelling towards player
                    {
                        if (NPC.position.X < player.position.X)
                        {
                            rotateright = true; //Spins one way
                        }
                        else
                        {
                            rotateright = false; //Spins the other
                        }
                        if (rotateright) //Reset Rotation depedning on side
                        {
                            rotation = 180; 
                        }
                        else
                        {
                            rotation = 270;
                        }
                       

                    }
                    else //Oribiting player
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if (movespeed < 7.5f) //minimum speed
                            {
                                movespeed = 7.5f;
                            }

                            NPC.netUpdate = true;
                        }
                        //Factors for calculations
                        double deg = (rotation);
                        double rad = deg * (Math.PI / 180);
                        double dist = 300; //Distance away from the player

                        //position
                        NPC.ai[1] = (int)(Math.Cos(rad) * dist);
                        NPC.ai[2] = (int)(Math.Sin(rad) * dist);

                        //angle
                        if (rotateright)
                        {
                            rotation += 0.8f;
                        }
                        else
                        {
                            rotation -= 0.8f;

                        }
                        NPC.localAI[2]++;//Count up to dash only when orbiting
                        if (NPC.localAI[2] > (180 + Main.rand.Next(180)) && Main.netMode != NetmodeID.MultiplayerClient)

                        {
                            charge = true;
                            NPC.netUpdate = true;

                        }
                    }
                }
                if (charge) //Charging
                {
                    if (Main.expertMode) //Projectile damage
                    {
                        projdamage = (30 * clamteadmg) / 100; // 120 On expert (180 on master)
                    }
                    else
                    {
                        projdamage = (40 * clamteadmg) / 100; // 80 on normal
                    }

                    NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 0f;
                    animateclaws = false;

                    NPC.localAI[2] = 0;
                    NPC.localAI[0]++; //Dropping mines
                    NPC.localAI[3]++; //Charging

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * 2;
                    //if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 229);
                        dust.noGravity = true;
                    }
                    if (NPC.localAI[3] == 1)//Start Dash
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.velocity.X = velocity.X;
                            NPC.velocity.Y = velocity.Y;
                            rotation += 180; //New target is opposite where charged
                            NPC.netUpdate = true;
                        }
                        SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 1f, Pitch = -0.5f, MaxInstances = 0 }, NPC.Center);

                    }
                    if (NPC.localAI[3] > 1 && NPC.localAI[3] < 15)//Accelerate
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            NPC.velocity *= 1.18f;
                            NPC.netUpdate = true;
                        }
                    }
                    if (NPC.localAI[3] > 35) //Decelerate 
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            NPC.velocity *= 0.96f;
                            NPC.netUpdate = true;

                        }
                    }
                    if (NPC.localAI[3] < 45 && NPC.localAI[0] > 9 || (halflife && NPC.localAI[0] > 6))//faster firing on half life

                    {
                        SoundEngine.PlaySound(SoundID.Item34, NPC.Center);

                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -3f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 110, dustspeed.X, dustspeed.Y, 229, default, 1.5f);
                            Main.dust[dust2].noGravity = true;
                        }
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)),
                                ModContent.ProjectileType<NPCs.NPCProjs.StormBossMine>(), projdamage, 1);
                        }
                        NPC.localAI[0] = 0;

                    }
                    if (NPC.localAI[3] >= 90 || (halflife && NPC.localAI[3] >= 60) && Main.netMode != NetmodeID.MultiplayerClient)//return faster below half life
                    {
                        charge = false;
                        NPC.localAI[0] = 0;
                        NPC.netUpdate = true;
                    }
                }
                if (!charge && NPC.ai[0] > (420 + Main.rand.Next(300)) && Main.netMode != NetmodeID.MultiplayerClient)
                //Change to next attack
                {
                    NPC.localAI[0] = 0;//Reset all ai values
                    NPC.localAI[2] = 0;
                    NPC.localAI[3] = 0;


                    NPC.ai[3]++;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
            //_____________________________________________________________________________________________
            if (NPC.ai[3] == 2 || NPC.ai[3] == 6) //Second and sixth attack, sweeps above the player dropping bombs 
            {
                if (Main.expertMode) //Projectile damage
                {
                    projdamage = (25 * clamteadmg) / 100; // 100 On expert (150 on master)
                }
                else
                {
                    projdamage = (35 * clamteadmg) / 100; // 70 on normal
                }
                NPC.ai[0]++;
                animateclaws = false;

                if (NPC.ai[0] == 1)//reset postion for phase
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (NPC.position.X < player.position.X)
                        {
                            NPC.ai[1] = -250;

                            rotateright = true; //Moves one way
                        }
                        else
                        {
                            NPC.ai[1] = 250;

                            rotateright = false; //Moves the other
                        }
                        NPC.netUpdate = true;
                    }
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    movespeed = 25;
                    NPC.netUpdate = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (rotateright)//Move depending on picked side
                    {
                        NPC.ai[1] += 5;
                    }
                    else
                    {
                        NPC.ai[1] -= 5;
                    }
                    NPC.ai[2] = -250;
                    NPC.netUpdate = true;
                }
                NPC.rotation = 1.55f;
                NPC.localAI[0]++;

                if (NPC.localAI[0] > 12 || (halflife && NPC.localAI[0] > 9))
                {
                    SoundEngine.PlaySound(SoundID.Item95 with { Volume = 1f, Pitch = 0.2f }, NPC.Center);
                    for (int i = 0; i < 30; i++)
                    {
                        float speedY = -3f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(NPC.Center, 0, 0, 110, dustspeed.X, dustspeed.Y, 229, default, 1.5f);
                        Main.dust[dust2].noGravity = true;
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {                      
                        for (int i = 0; i < 2; i++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(Main.rand.Next(-5, 5), -3),
                                   ModContent.ProjectileType<NPCs.NPCProjs.StormBossBomb>(), projdamage, 1);
                        }
                    }
                    NPC.localAI[0] = 0;
                }
                if (NPC.ai[0] > (100 + Main.rand.Next(100)) && Main.netMode != NetmodeID.MultiplayerClient) //Change to next attack
                {
                    NPC.localAI[2] = 0;
                    NPC.localAI[0] = 0;

                    NPC.ai[3]++;

                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
            //_____________________________________________________________________________________________
            if (NPC.ai[3] == 3) //Third attack, spins above player, summoning portals around the player
            {
                /*if (!Main.expertMode) //if not expert skip attack, now performed on classic too
                {
                    NPC.ai[3]++;
                }
                if (Main.expertMode)*/
                {

                    if (Main.masterMode) //Portal amounts
                    {
                        portalamount = 7;
                    }
                    else if (Main.expertMode && !Main.masterMode)
                    {
                        portalamount = 5;
                    }
                    else
                    {
                        portalamount = 3;
                    }

                    if (Main.expertMode)
                    {
                        projdamage = (25 * clamteadmg) / 100; // 100 On expert (150 on master)
                    }
                    else
                    {
                        projdamage = (35 * clamteadmg) / 100; // 70 on normal
                    }

                    animateclaws = false;

                    NPC.rotation = NPC.ai[0] / 3;
                    NPC.ai[0]++;
                    NPC.localAI[0]++;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        movespeed = 15;
                        NPC.ai[1] = 0;
                        NPC.ai[2] = -250;
                        NPC.netUpdate = true;
                    }
                    

                    if ((NPC.localAI[0] >= 90 || (halflife && NPC.localAI[0] >= 75)) && NPC.ai[0] <= 300)//One extra portal below half life
                    {
                        SoundEngine.PlaySound(SoundID.Item121, NPC.Center);
                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -10f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 229, dustspeed.X, dustspeed.Y, 229, default, 1.5f);
                            Main.dust[dust2].noGravity = true;
                        }

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {                          
                            for (int i = 0; i < portalamount; i++) //randomly summoned armound the player
                            {
                                double deg = Main.rand.Next(0, 360); //The degrees
                                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                                double dist = 350; //distance away from the player


                                float positionX = player.Center.X - (int)(Math.Cos(rad) * dist);
                                float positionY = player.Center.Y - (int)(Math.Sin(rad) * dist);

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(positionX, positionY), new Vector2(0, 0),
                                           ModContent.ProjectileType<NPCs.NPCProjs.StormBossLightningPortal>(), projdamage, 1, Main.myPlayer, 0, 0); 
                            }
                            
                        }
                        NPC.localAI[0] = 0;

                    }
                }
                if (NPC.ai[0] > 420 && Main.netMode != NetmodeID.MultiplayerClient) //Delay after last portal to change to next attack
                {

                    NPC.localAI[0] = 0;

                    NPC.ai[3]++;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
            //_____________________________________________________________________________________________
            if (NPC.ai[3] == 4) //Fourth attack, flies to one side and fires lightning, and summons lightning portals above
            {
                if (Main.expertMode) //Projectile damage
                {
                    projdamage = (25 * clamteadmg) / 100; // 100 On expert (150 on master)
                }
                else
                {
                    projdamage = (35 * clamteadmg) / 100; // 70 on normal
                }

                NPC.ai[0]++;
                animateclaws = true;

                if (NPC.ai[0] == 1) //reset postion for phase
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (player.velocity.X <= 0) //Choose what side to fly to for next attack depending on moving player direction
                        {
                            NPC.ai[1] = 300;
                        }
                        else
                        {
                            NPC.ai[1] = -300;
                        }
                        NPC.netUpdate = true;
                    }
                }
                if (NPC.ai[0] > 60) //only begin to fire after a delay
                {
                    NPC.localAI[0]++;
                }
                NPC.rotation = (player.MountedCenter - NPC.Center).ToRotation();

                NPC.ai[2] = 0;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    movespeed = 1.1f * distance / 10;
                    NPC.netUpdate = true;
                }
                if (NPC.localAI[0] >= 15 || (halflife && NPC.localAI[0] > 20)) //dust warning
                {
                    //if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y));
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                        Dust dust;
                        dust = Terraria.Dust.NewDustPerfect(NPC.Center, 229, new Vector2((perturbedSpeed.X * 15) + (NPC.velocity.X * 2), perturbedSpeed.Y + NPC.velocity.Y), 0, new Color(255, 255, 255), 1.25f);
                        dust.noGravity = true;

                        // Draw a line between the NPC and its destination, represented as dusts every 20 pixels
                        //Dust.QuickDustLine(NPC.Center, player.Center, 20, Color.Green);

                    }
                }

                if (Main.masterMode) //Portal amounts
                {
                    portalamount = 6;
                }
                else if (Main.expertMode && !Main.masterMode)
                {
                    portalamount = 5;
                }
                else
                {
                    portalamount = 4;
                }
                if ( NPC.localAI[0] > 50) //same speed on both phases
                {
                    //if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -5.5f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 229, dustspeed.X, dustspeed.Y, 229, default, 1.75f);
                            Main.dust[dust2].noGravity = true;
                        }

                        float projectileSpeed = 7f; // The speed of your projectile (in pixels per second).


                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                            Vector2 rotation = -NPC.Center + player.Center;

                            float ai = Main.rand.Next(100);
                            int projID = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, 0),
                                ModContent.ProjectileType<NPCs.NPCProjs.StormBossLightning>(), projdamage, .5f, Main.myPlayer, rotation.ToRotation(), ai);
                            Main.projectile[projID].scale = 0.9f;

                            //Vertical portals on phase 2 only
                            if (NPC.ai[0] <= 310 && halflife)//Don't summon any on last bolt
                            {
                                SoundEngine.PlaySound(SoundID.Item121, NPC.Center);

                                for (int i = 0; i < portalamount; i++) //randomly summoned above the player
                                {

                                    float positionX = player.Center.X + Main.rand.NextFloat(-400f, 400f);
                                    float positionY = player.Center.Y - 300;

                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(positionX, positionY), new Vector2(0, 0),
                                               ModContent.ProjectileType<NPCs.NPCProjs.StormBossLightningPortal>(), projdamage, 1, Main.myPlayer, 0, 1);
                                }
                            }
                        }
                        
                    }
                    SoundEngine.PlaySound(SoundID.Item122 with { Volume = 1f, Pitch = 0.5f }, NPC.Center);

                    NPC.localAI[0] = 0;
                }

                if (NPC.ai[0] > (360) && Main.netMode != NetmodeID.MultiplayerClient) //Change to next attack
                {
                    NPC.localAI[0] = 0;

                    NPC.ai[3]++;

                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
            //_____________________________________________________________________________________________
            if (NPC.ai[3] == 5) //Fifth attack, below half health, rapid charging and summoning homing projs
            {
                //NPC.localAI[3] == Dash Charge time

                NPC.dontTakeDamage = false;

                if (!halflife) //if not below half health skip attack
                {
                    NPC.ai[3]++;

                }
                if (halflife)
                {
                    if (Main.expertMode) //Projectile damage
                    {
                        projdamage = (35 * clamteadmg) / 100; // 140 On expert (126 portal) (210 on master (189 portal))
                    }
                    else
                    {
                        projdamage = (45 * clamteadmg) / 100; // 90 on normal (81 portal)
                    }
                    if (Main.masterMode) //Portal amounts
                    {
                        portalamount = 2;
                    }
                    else
                    {
                        portalamount = 1;
                    }

                    animateclaws = false;
                    if (!lowlife)//don't change attack on last phase
                    {
                        NPC.ai[0]++;
                    }
                    NPC.localAI[0]++;
                    NPC.localAI[3]++; //dashtime
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        charge = true;
                        NPC.netUpdate = true;
                    }

                    NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 0f;
                    
                    //"predictive" aim, aims infront of the player, not for first dash in phase 2 to prevent unfairness
                    if (distance > 250 && (NPC.ai[0] >= 5 || lowlife))
                    {
                        dashvelocity = Vector2.Normalize(new Vector2(player.Center.X + (player.velocity.X * 10), player.Center.Y + (player.velocity.Y * 10)) - new Vector2(NPC.Center.X, NPC.Center.Y)) * 5;

                        //dust effect for debugging
                        /*var dust = Dust.NewDustDirect(new Vector2(player.Center.X + (player.velocity.X * 10), player.Center.Y + (player.velocity.Y * 10)), 0, 0, 294);
                        dust.scale = 2;
                        dust.noGravity = true;*/
                        //----
                    }
                    else //if close to player, just target player directly
                    {
                        dashvelocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * 5;

                        //dust effect for debugging
                        /*var dust = Dust.NewDustDirect(new Vector2(player.Center.X, player.Center.Y), 0, 0, 294);
                        dust.scale = 2;
                        dust.noGravity = true;*/
                        //----
                    }

                    if (NPC.localAI[3] == 1)//Start dash
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.velocity.X = dashvelocity.X;
                            NPC.velocity.Y = dashvelocity.Y;

                            if (lowlife) //Summon a single portal per dash on phase 3
                            {
                                SoundEngine.PlaySound(SoundID.Item121, NPC.Center);

                                for (int i = 0; i < portalamount; i++) //randomly summoned above the player
                                {

                                    //float positionX = player.Center.X + Main.rand.NextFloat(-200f, 200f);
                                    //float positionY = player.Center.Y - 300;

                                    double deg = Main.rand.Next(0, 360); //The degrees
                                    double rad = deg * (Math.PI / 180); //Convert degrees to radians
                                    double dist = 350; //Distance away from the player


                                    float positionX = player.Center.X - (int)(Math.Cos(rad) * dist);
                                    float positionY = player.Center.Y - (int)(Math.Sin(rad) * dist);

                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(positionX, positionY), new Vector2(0, 0),
                                               ModContent.ProjectileType<NPCs.NPCProjs.StormBossLightningPortal>(), (projdamage * 9) / 10, 1, Main.myPlayer, 0, 0);
                                }
                            }
                            NPC.netUpdate = true;
                        }
                        SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 1f, Pitch = -0.5f, MaxInstances = 0 }, NPC.Center);
                    }
                    if (NPC.localAI[3] > 1 && NPC.localAI[3] < 8) //Accelerate
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.velocity *= 1.3f;
                            NPC.netUpdate = true;
                        }
                    }
                    if ((NPC.localAI[3] > 30 && !lowlife) || (NPC.localAI[3] > 35 && lowlife)) //Decelerate
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.velocity *= 0.96f;
                            NPC.netUpdate = true;
                        }
                    }
                    if ((NPC.localAI[3] > 60 && !lowlife) || (NPC.localAI[3] > 70 && lowlife)) //Reset dash, slower in phase 2
                    {
                        NPC.localAI[3] = 0;
                    }
                    if (NPC.localAI[0] > 25) //Mines
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -3f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 110, dustspeed.X, dustspeed.Y, 229, default, 1.5f);
                            Main.dust[dust2].noGravity = true;
                        }
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5)),
                            ModContent.ProjectileType<NPCs.NPCProjs.StormBossMineLarge>(), projdamage, 1);

                        }
                        SoundEngine.PlaySound(SoundID.Item34, NPC.Center);

                        NPC.localAI[0] = 0;
                    }
                }
                if (NPC.ai[0] >= (300) && Main.netMode != NetmodeID.MultiplayerClient) //Change to next attack
                {
                    charge = false;

                    NPC.localAI[0] = 0;
                    NPC.localAI[2] = 0;
                    NPC.localAI[3] = 0;
                    NPC.ai[3]++;

                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
        }

        int npcframe = 0;
        public override void FindFrame(int frameHeight)//Animations
        {
            NPC.frame.Y = npcframe * frameHeight;

            //NPC.spriteDirection = NPC.direction;
            if (!halflife && !lowlife) //above half health
            {
                if (!animateclaws)
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter > 6)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }
                    if (npcframe >= 6) //Cycles through frames 0-5 for still claws
                    {
                        npcframe = 0;
                    }
                }
                if (animateclaws)
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter > 6)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }
              
                    if (npcframe <= 5 || npcframe >= 12) //Cycles through frames 6-11 for moving claws
                    {
                        npcframe = 6;

                    }
                }
            }
            if (halflife && !lowlife) //half life
            {
                if (!animateclaws)
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter > 6)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }
                    if (npcframe <= 11 || npcframe >= 16) //Cycles through frames 12-15 when not moving claws
                    {
                        npcframe = 12;
                    }
                }
                if (animateclaws)
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter > 6)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }
                
                    if (npcframe <= 15 || npcframe >= 20) //Cycles through frames 16-19 when moving claws
                    {
                        npcframe = 16;

                    }
                }
            }
            if (lowlife || deathani)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 6)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe <= 19 || npcframe >= 24) //Cycles through frames 20-23 on last phase
                {
                    npcframe = 20;
                }
            }
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            
        }
        public override void HitEffect(NPC.HitInfo hit)
        {  
            for (int i = 0; i < 8; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 25, NPC.Center.Y - 25), 50, 50, 229);
                dust.scale = 0.5f;

            }
            for (int i = 0; i < 3; i++)
            {
                float speedY = -3f;

                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(NPC.Center, 0, 0, 229, dustspeed.X, dustspeed.Y, 229, default, 1.5f);
                Main.dust[dust2].noGravity = true;
            }
            if (NPC.life <= 0 && deathani)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 1f, Pitch = -0.5f, MaxInstances = -1 }, NPC.Center);

                NPC.velocity *= 0.5f;
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionVortexProj>(), 0, 0, Main.myPlayer);
                Main.projectile[proj].scale = 1.75f;

                float numberProjectiles = 16;
                float rotation = MathHelper.ToRadians(180);
                for (int j = 0; j < numberProjectiles; j++) //Lightning is just for visuals
                {

                    Vector2 perturbedSpeed = new Vector2(0, 2f).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));

                    float ai = Main.rand.Next(100);
                    int projID = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                        ModContent.ProjectileType<NPCs.NPCProjs.StormBossLightning>(), 0, 0, Main.myPlayer, perturbedSpeed.ToRotation(), ai);

                    Main.projectile[projID].tileCollide = false;
                    Main.projectile[projID].scale = 0.8f;


                }

                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StormBossGore1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StormBossGore2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StormBossGore3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StormBossGore5").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StormBossGore5").Type, 1f);

                }
                for (int i = 0; i < 150; i++)
                {
                     
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 220);
                    dust.velocity *= 2;
                }
                for (int i = 0; i < 150; i++)
                {
                    float speedY = -12f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(NPC.Center, 0, 0, 110, dustspeed.X, dustspeed.Y, 229, default, 1.5f);
                    Main.dust[dust2].noGravity = true;
                }
            }
        }
        public override void OnKill()
        {
            if (GetInstance<ConfigurationsGlobal>().StormBossSkipsPlant)
            {
                Item.NewItem(NPC.GetSource_Loot(), (int)NPC.Center.X, (int)NPC.Center.Y, NPC.width, NPC.height, ItemID.TempleKey);

            }
            /*if (StormWorld.stormBossDown == false)
            {
                StormWorld.stormBossDown = true;
            }*/

            NPC.SetEventFlagCleared(ref StormWorld.stormBossDown, -1); //set boss downed

            if (NPC.downedPlantBoss == false && GetInstance<ConfigurationsGlobal>().StormBossSkipsPlant)
            {
                NPC.downedPlantBoss = true;

                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Screams are echoing from the dungeon..."), new Color(73, 201, 127));
                }
                else if (Main.netMode == 0) // Single Player
                {

                    Main.NewText("Screams are echoing from the dungeon...", 73, 201, 127);
                }
            }
            if (!Main.expertMode)
            Item.NewItem(new EntitySource_Loot(null), NPC.position, new Vector2(NPC.width, NPC.height), ItemID.RocketI, Main.rand.Next(50, 101)); //make rockets not appear in bestiary
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());

            //boss trophy
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.BossTrophy.StormBossTrophy>(), 10));
          
            //Misc drops
            notExpert.OnSuccess(ItemDropRule.OneFromOptionsWithNumerator(3, 2, ModContent.ItemType<Items.Tools.StormHook>(), ModContent.ItemType<Items.Accessory.StormWings>()));

            //mask and weapons on normal mode
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanitysets.BossMaskStormBoss>(), 7)); 
            notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<StormKnife>(), ModContent.ItemType<StormLauncher>(), ModContent.ItemType<StormStaff>(), ModContent.ItemType<StormSentryStaff>()));
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<VortexiaWeapon>(), 10));

            //notExpert.OnSuccess(ItemDropRule.Common(ItemID.RocketI, 1, 50, 100)); //idk how to make it only drop along side launcher so :shrug:

            //expert and master loot
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.BossTrophy.StormBossBag>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.BossTrophy.StormBossRelic>()));
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<Items.Pets.StormBossPetItem>(), 4));

            npcLoot.Add(notExpert);

            //Temple Key
            //npcLoot.Add(ItemDropRule.Common(ItemID.TempleKey, 1));
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) //Glowmask
         {
             Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/Boss/StormBoss_Glow");
             Vector2 drawPos = new Vector2(0, 1) + NPC.Center - Main.screenPosition;

             spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
         }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) //Trial
        {
            if (!Main.dedServ)
            {
                Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/Boss/StormBoss");

                if (charge)
                {
                    for (int k = 0; k < NPC.oldPos.Length; k++)
                    {
                        Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + NPC.frame.Size() / 2f + new Vector2(-12, 8);
                        Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                        Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    }
                }
                Color color2 = new Color(34, 221, 151, 40);
                float scaleFactor13 = 0.5f + (NPC.GetAlpha(color2).ToVector3() - new Vector3(0.5f)).Length() * 0.5f;
                if (charge)
                {
                    for (int num149 = 0; num149 < 4; num149++)
                    {
                        spriteBatch.Draw(texture, NPC.position - screenPos + new Vector2((float)(NPC.width) * NPC.scale / 2f * NPC.scale, (float)(NPC.height) * NPC.scale / Main.npcFrameCount[NPC.type] + 4f * NPC.scale + 32) + NPC.velocity.RotatedBy((float)num149 * ((float)Math.PI / 2f)) * scaleFactor13, NPC.frame, new Microsoft.Xna.Framework.Color(64, 64, 64, 0), NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                    }
                }
                /*float speen1 = 9f + 3f * (float)Math.Cos((float)Math.PI * 2f * Main.GlobalTimeWrappedHourly);
                Vector2 spinningpoint5 = Vector2.UnitX * speen1;
                Color color2 = Color.Turquoise * (speen1 / 12f) * 0.8f;
                color2.A /= 3;
                if (halflife)
                {
                    for (float speen2 = 0f; speen2 < (float)Math.PI * 2f; speen2 += (float)Math.PI / 2f)
                    {
                        Vector2 finalpos = NPC.position + new Vector2(0, 30) + spinningpoint5.RotatedBy(speen2);
                        spriteBatch.Draw(texture, new Vector2(finalpos.X - screenPos.X + (float)(NPC.width / 2) * NPC.scale, finalpos.Y - screenPos.Y + (float)NPC.height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f * NPC.scale), NPC.frame, color2, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    }
                }*/
            }
                return true;        
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            position += new Vector2(0, 25); //Moves healthbar down a little
            return true;
        }  
    }
}