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
using StormDiversMod.Buffs;

using StormDiversMod.Items.Pets;

namespace StormDiversMod.NPCs.Boss
{
    [AutoloadBossHead]
    public class AridBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Husk"); // Automatic from .lang files
                                                     // make sure to set this for your modnpcs.
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 4;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }
        public override void SetDefaults()
        {

            Main.npcFrameCount[NPC.type] = 16;

            NPC.width = 62;
            NPC.height = 100;

            NPC.aiStyle = -1;

            NPC.noGravity = true;
            NPC.damage = 40; //40/60/90

            NPC.defense = 15;
            NPC.lifeMax = 10000;

            NPC.gfxOffY = 0;
            
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath24;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 5, 0, 0);
            NPC.boss = true;
            if (!Main.dedServ)
            {
                Music = MusicID.Boss5;
            }
            NPC.npcSlots = 10f;
            NPC.noTileCollide = true;
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A heavily withered set of ancient armour that was once buried in the depths of the desert, possessed by an unknown entity. " +
                "All those who try to slay it end up serving it for eternity. Legends say the remains of the former owner are still inside.")
            });
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //10K Classic
            if (!Main.masterMode)
            {
                NPC.lifeMax = (int)(14000 / 2 * bossLifeScale); //14K
            }
            else
            {
                NPC.lifeMax = (int)(18000 / 3 * bossLifeScale); //18K 

            }
            //30/45/66
            NPC.damage = (int)(NPC.damage * 0.75f);
        }
        float speed = 10;
        float inertia = 40;
        float distanceToIdlePosition; //distance to player

        bool halflife;//Half health

        bool animateattack; //Wheter the claws move

        int projdamage; //Damage of all projectiles
        int projcount; //Number of projs if applicable
        float projvelocity; //Velocity of projectiles

        float distanceX;//}
        float distanceY;//}
        float distance; //}Distance between boss and player for projectiles
        public static int phase2HeadSlot = -1;
        public override void Load()
        {
            // We want to give it a second boss head icon, so we register one
            string texture = BossHeadTexture + "_Phase2"; // Texture name
            phase2HeadSlot = Mod.AddBossHeadTexture(texture, -1); // -1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise
        }
        public override void BossHeadSlot(ref int index)
        {
            int slot = phase2HeadSlot;
            if (halflife && slot != -1)
            {
                // If the boss is in its second stage, display the other head icon instead
                index = slot;
            }
        }

        public override void AI()
        {          
            NPC.buffImmune[(BuffType<AridSandDebuff>())] = true;

            if (Main.netMode != NetmodeID.Server)
            {
                // For visuals regarding NPC position, netOffset has to be concidered to make visuals align properly
                NPC.position += NPC.netOffset;
                NPC.position -= NPC.netOffset;
            }
         
            NPC.buffImmune[BuffID.Confused] = true;

            //===============AI fields================

            //NPC.ai[0] = Shootime
            //NPC.ai[1 and 2] = (Attack dependant)
            //NPC.ai[3] = Phase
            //NPC.localAI[0] = time until next attack
            //NPC.localAI[1] = death counter despawn
            //NPC.localAI[2] = X postion
            //NPC.localAI[3] = Y postion

            //========================================
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }


            //======================================================MOVEMENT==============================================================================================         
            Player player = Main.player[NPC.target]; //Code to move towards player

            if (!player.dead)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 idlePosition = player.Center + new Vector2(NPC.localAI[2], NPC.localAI[3]);
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

            if (NPC.ai[3] == 0 || player.dead) //what way to face
            {
                if (NPC.velocity.X > 0.1f)
                {
                    NPC.spriteDirection = -1;
                }
                if (NPC.velocity.X < -0.1f)
                {
                    NPC.spriteDirection = 1;

                }
            }
            else
            {
                if (NPC.position.X >= player.position.X)
                {
                    NPC.spriteDirection = 1;
                }
                if (NPC.position.X < player.position.X)
                {
                    NPC.spriteDirection = -1;

                }
            }
            //====================================================================================================================================================

            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            distanceX = player.Center.X - NPC.Center.X;
            distanceY = player.Center.Y - NPC.Center.Y;
            distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));


            if (distance > 10000 && NPC.ai[3] != 0)// Despawn if too far away
            {
                NPC.active = false;
            }

      
            if (!halflife && NPC.life <= NPC.lifeMax / 2) //at half life
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[0] = 180;
                    NPC.ai[0] = 0;//Reset all ai values
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;

                    NPC.velocity *= 0;
                    NPC.ai[3] = 0;
                    NPC.netUpdate = true;
                }
                if (Main.netMode != NetmodeID.Server)//Drop some gore when changing phase
                {
                    int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Top.Y + 20), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionGenericProj>(), 0, 0, Main.myPlayer);
                    Main.projectile[proj].scale = 1f;
                    SoundEngine.PlaySound(SoundID.Roar with { Volume = 1f, Pitch = 0.5f }, NPC.Center);
                    SoundEngine.PlaySound(SoundID.Item74, NPC.Center);

                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("AridBossGore5").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("AridBossGore6").Type, 1f);
                }
                halflife = true;
            }

            if (player.dead)//When player is dead fly down
            {
                NPC.ai[3] = 0;

                if (NPC.velocity.Y < 25)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.velocity.Y += 0.2f;
                        NPC.netUpdate = true;
                    }
                }

                NPC.EncourageDespawn(60);

                NPC.localAI[1]++;
                if (NPC.localAI[1] > 240)
                {
                    NPC.active = false;
                }
            }
            else
            {
                NPC.localAI[1] = 0;
            }
            //dusts
            if (!halflife)
            {
                if (Main.rand.Next(5) == 0)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 25, NPC.Bottom.Y - 10), 50, 20, 55, 0, 2);
                    dust.scale = 0.75f;
                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.Center.X - 25, NPC.Bottom.Y - 60), 50, 20, 138, 0, 2);
                    dust2.scale = 0.75f;

                }
            }
            else
            {
                if (Main.rand.Next(3) == 0 )
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 25, NPC.Bottom.Y - 10), 50, 20, 55, 0, 2);
                    dust.scale = 0.75f;
                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.Center.X - 25, NPC.Bottom.Y - 60), 50, 20, 138, 0, 2);
                    dust2.scale = 0.75f;
                }
            }

            if (!player.dead)//begin AI
            {
                if (NPC.ai[3] == 0) //No attacks when first summoned
                {

                    NPC.localAI[2] = 0;
                    NPC.localAI[3] = -150;
                    if (distanceToIdlePosition > 300f)
                    {
                        // Speed up the boss if it's away from the player
                        speed = 12f;
                        inertia = 30f;
                    }
                    else
                    {
                        // Slow down the boss if closer to the player
                        speed = 8f;
                        inertia = 40f;
                    }
                    if (NPC.velocity.X > 0.1f)
                    {
                        NPC.spriteDirection = -1;
                    }
                    if (NPC.velocity.X < -0.1f)
                    {
                        NPC.spriteDirection = 1;

                    }

                    NPC.localAI[0]++;  //count to first attack               
                    if (NPC.localAI[0] >= 300 && Main.netMode != NetmodeID.MultiplayerClient) //Change to first attack
                    {
                        NPC.ai[3]++;

                        NPC.localAI[0] = 0;
                        NPC.netUpdate = true;
                    }
                }

                Attacks(player); //All attacks are in this hook

                if (NPC.ai[3] >= 7) //go back to first attack if value somehow goes over 7
                {
                    NPC.ai[3] = 1;
                }
            }          

            //DEBUG
           /*var dustt = Dust.NewDustDirect(new Vector2(player.Center.X + NPC.localAI[2], player.Center.Y + NPC.localAI[3]), 0, 0, 138);
            dustt.velocity *= 0;
            dustt.noGravity = true;*/
        }
        private void Attacks(Player player)//___________________________________________________________________________________________________________________________________________________
        {
            if (NPC.ai[3] == 1) //Attack 1, sweep over player firing sandblasts, only explode below half life
            {
                NPC.dontTakeDamage = false;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.damage = 30;
                    NPC.netUpdate = true;
                }

                NPC.localAI[0]++;

               
                if (NPC.localAI[0] == 1)//Xpos
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (NPC.position.X > player.position.X)
                        {
                            NPC.localAI[2] = -300;

                            NPC.ai[1] = 1; //Move to right;
                        }
                        else
                        {
                            NPC.localAI[2] = 300;

                            NPC.ai[1] = 0; //Move to left;
                        }
                        NPC.netUpdate = true;
                    }
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (NPC.ai[1] == 1)//Move depending on picked side
                    {
                        NPC.localAI[2] -= 3;

                        if (NPC.localAI[2] <= -300)
                        {
                            NPC.ai[1] = 0; //Move to left;
                        }
                    }
                    else
                    {
                        NPC.localAI[2] += 3;
                        if (NPC.localAI[2] >= 300)
                        {
                            NPC.ai[1] = 1; //Move to right;
                        }
                    }
                    NPC.localAI[3] = -300;
                    NPC.netUpdate = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (distanceToIdlePosition > 300f)
                    {
                        // Speed up the boss if it's away from the player
                        speed = 11f;
                        inertia = 30f;
                    }
                    else
                    {
                        // Slow down the boss if closer to the player
                        speed = 7f;
                        inertia = 40f;
                    }
                    NPC.netUpdate = true;
                }
                if (Main.masterMode) //Projectile changes
                {
                    projdamage = 14; //84 on master
                    projvelocity = 11f;
                    projcount = 3;

                }
                else if (Main.expertMode && !Main.masterMode)
                {
                    projdamage = 15; // 60 On expert
                    projvelocity = 10f;
                    projcount = 2;
                }
                else
                {
                    projdamage = 20; // 40 on normal
                    projvelocity = 9f;
                    projcount = 1;
                }

                NPC.ai[0]++;
                if (NPC.ai[0] > 55 || (halflife && NPC.ai[0] > 30)) //telegraphing
                {
                    float speedY = -3f;
                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    int dust = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X + NPC.velocity.X, dustspeed.Y + NPC.velocity.Y, 100, default, 1f);
                    Main.dust[dust].noGravity = true;

                    Vector2 dustvelocity = Vector2.Normalize(new Vector2(player.Center.X + (player.velocity.X * 15), player.Center.Y + (player.velocity.X * 15)) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity;
                    int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustvelocity.X + NPC.velocity.X, dustvelocity.Y + NPC.velocity.Y, 100, default, 1f);
                    Main.dust[dust2].noGravity = true;
                }
                if (NPC.ai[0] > 75 || (halflife && NPC.ai[0] > 50))
                {
                    animateattack = true;
                    SoundEngine.PlaySound(SoundID.Item20 with { Volume = 1.5f, Pitch = 00f }, NPC.Center);

                    for (int i = 0; i < 50; i++)
                    {
                        float speedY = -3f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X, dustspeed.Y, 100, default, 1f);
                        Main.dust[dust2].noGravity = true;
                    }

                    for (int i = 0; i < projcount; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X + (player.velocity.X * 15), player.Center.Y + (player.velocity.X * 15)) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity; //slight predictive 
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10));
                            if (!halflife)
                            {

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                                    ModContent.ProjectileType<NPCs.NPCProjs.AridBossSandProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                            }

                            else if (halflife)
                            {

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X * 1.1f, perturbedSpeed.Y * 1.1f),
                                    ModContent.ProjectileType<NPCs.NPCProjs.AridBossSandProj>(), projdamage, 1, Main.myPlayer, 0, 1);
                            }
                        }
                    }
                    NPC.ai[0] = 0;
                }

                if (NPC.localAI[0] > (600 + Main.rand.Next(300)) && Main.netMode != NetmodeID.MultiplayerClient)
                //Change to next attack
                {
                    NPC.ai[0] = 0;//Reset all ai values
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;


                    NPC.ai[3]++;
                    NPC.localAI[0] = 0;
                    NPC.netUpdate = true;
                }
            }
        
            //________________________________________________________________________________________________________________________
            if (NPC.ai[3] == 2)//attack 2, fly next to play and throw out knives, faster firing below half health
            {
                NPC.localAI[0]++;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[3] = -100; //Ypos
                    NPC.netUpdate = true;
                }
                if (NPC.localAI[0] == 1)//Xpos
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (NPC.position.X > player.position.X)
                        {
                            NPC.localAI[2] = 450;                        
                        }
                        else
                        {
                            NPC.localAI[2] = -450;
                        }
                        NPC.netUpdate = true;
                    }
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (distanceToIdlePosition > 300f)
                    {
                        // Speed up the boss if it's away from the player
                        speed = 11f;
                        inertia = 30f;
                    }
                    else
                    {
                        // Slow down the boss if closer to the player
                        speed = 7f;
                        inertia = 40f;
                    }
                    NPC.netUpdate = true;
                }
                if (Main.masterMode) //Projectile changes
                {
                    projdamage = 12; //72 on master
                    projvelocity = 12f;
                    projcount = 5;

                }
                else if(Main.expertMode && !Main.masterMode)
                {
                    projdamage = 13; // 52 On expert
                    projvelocity = 11f;
                    projcount = 4;

                }
                else
                {
                    projdamage = 16; // 32 on normal
                    projvelocity = 10f;
                    projcount = 3;
                }
                if (NPC.localAI[0] > 90)
                {
                    NPC.ai[0]++;
                    if (NPC.ai[0] > 55 || (halflife && NPC.ai[0] > 30))
                    {
                        float speedY = -3f;
                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                        int dust = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X + NPC.velocity.X, dustspeed.Y +NPC.velocity.Y, 100, default, 1f);
                        Main.dust[dust].noGravity = true;

                        Vector2 dustvelocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity; 
                        Vector2 perturbeddustSpeed = new Vector2(dustvelocity.X, dustvelocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                        int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, perturbeddustSpeed.X + NPC.velocity.X, perturbeddustSpeed.Y + NPC.velocity.Y, 100, default, 1f);
                        Main.dust[dust2].noGravity = true;
                    }

                    if (NPC.ai[0] > 75 || (halflife && NPC.ai[0] > 50))
                    {
                        animateattack = true;
                        SoundEngine.PlaySound(SoundID.Item17 with { Volume = 1f, Pitch = 0f }, NPC.Center);
                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -3f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X, dustspeed.Y, 100, default, 1f);
                            Main.dust[dust2].noGravity = true;
                        }

                        float numberProjectiles = projcount + Main.rand.Next(2);
                        float rotation = MathHelper.ToRadians(40);

                        Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y - 50) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity;
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X - 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                                ModContent.ProjectileType<NPCs.NPCProjs.AridBossShardProj>(), projdamage, 1);
                            }
                        }
                        NPC.ai[0] = 0;
                    }
                }

                if (NPC.localAI[0] > (450 + Main.rand.Next(150)) && Main.netMode != NetmodeID.MultiplayerClient)
                //Change to next attack
                {
                    NPC.ai[0] = 0;//Reset all ai values
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;

                    NPC.ai[3]++;
                    NPC.localAI[0] = 0;
                    NPC.netUpdate = true;
                }

            }
            //________________________________________________________________________________________________________________________
            if (NPC.ai[3] == 3)//attack 3, fly near player with flame thrower (Half life only)
            {
                if (!halflife)
                {
                    NPC.ai[3]++;
                }
                NPC.localAI[0]++;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[2] = 0; //Xpos
                    NPC.localAI[3] = 0; //Ypos
                    NPC.netUpdate = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (NPC.localAI[0] <= 60)
                    {
                        speed = 0f;
                        inertia = 10f;
                    }
                    else
                    {

                        speed = 14f;
                        inertia = 70f;

                    }
                    NPC.netUpdate = true;
                }
                if (Main.masterMode) //Projectile changes
                {
                    projdamage = 10; //60 on master
                    projvelocity = 4.5f;

                }
                else if(Main.expertMode && !Main.masterMode)
                {
                    projdamage = 12; // 48 On expert
                    projvelocity = 3.5f;

                }
                else
                {
                    projdamage = 15; // 30 on normal
                    projvelocity = 2.5f;

                }
                if (NPC.localAI[0] < 90)
                {
                    float speedY = -6f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X, dustspeed.Y, 100, default, 1f);
                    Main.dust[dust2].noGravity = true;
                }
                if (NPC.localAI[0] >= 90)
                {
                    NPC.ai[0]++;

                    if (NPC.ai[0] > 5)
                    {
                        animateattack = true;
                        SoundEngine.PlaySound(SoundID.Item13 with { Volume = 1f, Pitch = 0f }, NPC.Center);

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity;

                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10));

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X + NPC.velocity.X / 2, perturbedSpeed.Y + NPC.velocity.Y / 2),
                                ModContent.ProjectileType<NPCs.NPCProjs.AridBossFlameProj>(), projdamage, 1);

                        }
                        NPC.ai[0] = 0;
                    }
                }

                if (NPC.localAI[0] > (360) && Main.netMode != NetmodeID.MultiplayerClient)
                //Change to next attack
                {
                    NPC.ai[0] = 0;//Reset all ai values
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;


                    NPC.ai[3]++;
                    NPC.localAI[0] = 0;
                    NPC.netUpdate = true;
                }

            }
            //________________________________________________________________________________________________________________________
            if (NPC.ai[3] == 4)//attack 4, summon minions
            {           
                 NPC.localAI[0]++;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[2] = 0; //Xpos
                    NPC.localAI[3] = 0; //Ypos
                    NPC.netUpdate = true;
                }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    speed = 0f;
                    inertia = 40f;
                    NPC.netUpdate = true;
                }
                NPC.velocity *= 0.95f;
                if (Main.masterMode) //Projectile changes
                {
                    projcount = 4;

                }
                else if(Main.expertMode && !Main.masterMode) 
                {
                    projcount = 3;
                }
                else
                {
                    projcount = 2;
                }
                if (NPC.localAI[0] <= 90)
                {
                    float speedY = -6f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X, dustspeed.Y, 100, default, 1f);
                    Main.dust[dust2].noGravity = true;
                }

                if (NPC.localAI[0] == 60)
                {
                    animateattack = true;
                    SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1f, Pitch = 0.2f }, NPC.Center);
                    for (int i = 0; i < 100; i++)
                    {
                        float speedY = -6f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X, dustspeed.Y, 100, default, 1f);
                        Main.dust[dust2].noGravity = true;
                    }

                    int type = ModContent.NPCType<AridBossMinion>();


                    SoundEngine.PlaySound(SoundID.Item8, NPC.Center);

                    for (int i = 0; i < projcount; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.NewNPC(NPC.GetSource_FromAI(), (int)Math.Round(NPC.Center.X + Main.rand.Next(-150, 150)), (int)Math.Round(NPC.Center.Y + Main.rand.Next(-150, 150)), type);
                        }
                    }
                }

                if (NPC.localAI[0] > (90) && Main.netMode != NetmodeID.MultiplayerClient)
                //Change to next attack
                {
                    SoundEngine.PlaySound(SoundID.Item45 with { Volume = 1f, Pitch = 0.25f }, NPC.Center);

                    NPC.ai[0] = 0;//Reset all ai values
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;

                    NPC.ai[3]++;
                    

                    NPC.localAI[0] = 0;
                    NPC.netUpdate = true;
                }

            }
            //_______________________________________________
            if (NPC.ai[3] == 5)//attack 5, orbit player until minions are defeated, fires sand blasts below half health
            {              
 
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (NPC.ai[2] == 0)//Loop round once then switc direction
                    {
                        NPC.ai[1]++;//rotation

                        if (NPC.ai[1] >= 550)
                        {
                            NPC.ai[2] = 1; //Rotate anti clockwise;
                        }
                    }
                    else
                    {
                        NPC.ai[1]--;//rotation
                        if (NPC.ai[1] <= 0)
                        {
                            NPC.ai[2] = 0; //Rotate clockwise;
                        }
                    }
                    NPC.netUpdate = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    double deg = (NPC.ai[1]);
                    double rad = deg * (Math.PI / 180);
                    double dist = 400; //Distance away from the player

                    //position

                    NPC.localAI[2] = (int)(Math.Cos(rad) * dist);
                    NPC.localAI[3] = (int)(Math.Sin(rad) * dist);
                    NPC.netUpdate = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (distanceToIdlePosition > 250f)
                    {
                        // Speed up the boss if it's away from the player
                        speed = 10f;
                        inertia = 30f;
                    }
                    else
                    {
                        // Slow down the boss if closer to the player
                        speed = 6.5f;
                        inertia = 40f;
                    }
                    NPC.netUpdate = true;
                }

                NPC.dontTakeDamage = true;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.damage = 0;
                    NPC.netUpdate = true;
                }

                if (Main.masterMode) //Projectile changes
                {
                    projdamage = 16; //96 on master
                    projvelocity = 13f;
                }
                else if (Main.expertMode && !Main.masterMode)
                {
                    projdamage = 18; // 72 On expert
                    projvelocity = 11f;
                }
                else
                {
                    projdamage = 25; // 50 on normal
                    projvelocity = 9f;
                }
                if (halflife) //fire sand below half life
                {
                    if (distance > 200) // no firing if near player
                    {
                        NPC.ai[0]++;
                        if (NPC.ai[0] > 70) //telegraphing
                        {
                            float speedY = -3f;
                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                            int dust = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X + NPC.velocity.X, dustspeed.Y + NPC.velocity.Y, 100, default, 1f);
                            Main.dust[dust].noGravity = true;

                            Vector2 dustvelocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity;
                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustvelocity.X + NPC.velocity.X, dustvelocity.Y + NPC.velocity.Y, 100, default, 1f);
                            Main.dust[dust2].noGravity = true;
                        }
                        if (NPC.ai[0] > 90)
                        {
                            animateattack = true;
                            SoundEngine.PlaySound(SoundID.Item20 with { Volume = 1f, Pitch = 0.5f }, NPC.Center);
                            for (int i = 0; i < 50; i++)
                            {
                                float speedY = -3f;

                                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                                int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X, dustspeed.Y, 100, default, 1f);
                                Main.dust[dust2].noGravity = true;
                            }
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {

                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projvelocity;
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                                    ModContent.ProjectileType<NPCs.NPCProjs.AridBossSandProj>(), projdamage, 0);

                            }
                            NPC.ai[0] = 0;
                        }
                    }
                }
                /*for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].type == ModContent.NPCType<AridBossMinion>())
                    {
                        NPC Minion = Main.npc[i];
                        Dust.QuickDustLine(NPC.Center, player.Center, 50, Color.Orange);
                    }
                }*/
                for (int i = 0; i < 8; i++) //shield visual
                {
                    double deg2 = Main.rand.Next(0, 360); //The degrees
                    double rad2 = deg2 * (Math.PI / 180); //Convert degrees to radians
                    double dist2 = 60; //Distance away from the NPC
                    float dustx = NPC.Center.X - (int)(Math.Cos(rad2) * dist2);
                    float dusty = NPC.Center.Y - (int)(Math.Sin(rad2) * dist2);
                    //if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, NPC.position, NPC.width, NPC.height))//no dust unless line of sight

                    var dust2 = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 138, 0, 0);
                    dust2.noGravity = true;
                    dust2.scale = 0.75f;
                    dust2.velocity *= 0;
                    dust2.fadeIn = 0.5f;

                    Vector2 velocity = Vector2.Normalize(new Vector2(NPC.Center.X, NPC.Center.Y) - new Vector2(dustx, dusty)) * 10; //slight predictive 
                    var dust3 = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 138, velocity.X, velocity.Y);
                    dust3.noGravity = true;
                    dust3.scale = 0.5f;
                    dust3.velocity *= 0.5f;
                }

                if (NPC.CountNPCS(ModContent.NPCType<AridBossMinion>()) == 0) // 60 seconds after last minion killed next attack
                {
                    NPC.localAI[0]++;
                }
                if (NPC.localAI[0] >= 60 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SoundEngine.PlaySound(SoundID.Item45 with { Volume = 1f, Pitch = -0.3f }, NPC.Center);

                    NPC.dontTakeDamage = false;

                    NPC.ai[0] = 0;//Reset all ai values
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;

                    NPC.ai[3]++;

                    NPC.localAI[0] = 0;
                    NPC.netUpdate = true;
                }
            }
            //______________________________________
            if (NPC.ai[3] == 6) //attack 6, summon dust above player that falls down, halflife only
            {
                NPC.dontTakeDamage = false;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.damage = 30;
                    NPC.netUpdate = true;
                }
                if (!halflife)
                {
                    NPC.ai[3] = 1;
                }
                NPC.localAI[0]++;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[2] = 0; //Xpos
                    NPC.localAI[3] = -300; //Ypos
                    NPC.netUpdate = true;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (distanceToIdlePosition > 300f)
                    {
                        // Speed up the boss if it's away from the player
                        speed = 13f;
                        inertia = 30f;
                    }
                    else
                    {
                        // Slow down the boss if closer to the player
                        speed = 8f;
                        inertia = 40f;
                    }
                    NPC.netUpdate = true;
                }

                if (Main.masterMode) //Projectile changes
                {
                    projdamage = 16; //96 on master
                    projvelocity = 15f;
                    projcount = 4;

                }
                else if (Main.expertMode && !Main.masterMode)
                {
                    projdamage = 18; // 72 On expert
                    projvelocity = 13f;
                    projcount = 3;
                }
                else
                {
                    projdamage = 25; // 50 on normal
                    projvelocity = 11f;
                    projcount = 2;

                }
                if (NPC.localAI[0] > 120 && NPC.localAI[0] <= 600)
                {
                    NPC.ai[0]++;
                    if (NPC.ai[0] > 20) //telegraphing
                    {
                        float speedY = -3f;
                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                        int dust = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X + NPC.velocity.X, dustspeed.Y + NPC.velocity.Y, 100, default, 1f);
                        Main.dust[dust].noGravity = true;

                        int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, 0 + NPC.velocity.X, projvelocity + NPC.velocity.Y, 100, default, 1f);
                        Main.dust[dust2].noGravity = true;
                    }
                    if (NPC.ai[0] > 40)
                    {
                        animateattack = true;
                        SoundEngine.PlaySound(SoundID.Item20 with { Volume = 1.5f, Pitch = 00f }, NPC.Center);
                        for (int i = 0; i < 50; i++)
                        {
                            float speedY = -3f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X, dustspeed.Y, 100, default, 1f);
                            Main.dust[dust2].noGravity = true;
                        }

                        for (int i = 0; i < projcount; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(player.Center.X + Main.rand.Next(-350, 350), player.Center.Y - 600), new Vector2(0, projvelocity),
                                ModContent.ProjectileType<NPCs.NPCProjs.AridBossSandProj>(), projdamage, 1, Main.myPlayer, 0, 0);
                            }
                        }
                        NPC.ai[0] = 0;
                    }
                }
                if (NPC.localAI[0] > (700) && Main.netMode != NetmodeID.MultiplayerClient)
                //Change to next attack
                {
                    NPC.ai[0] = 0;//Reset all ai values
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;

                    //NPC.ai[3]++;
                    NPC.ai[3] = 1;

                    NPC.localAI[0] = 0;
                    NPC.netUpdate = true;
                }
            }
        }

        int npcframe = 0;
        public override void FindFrame(int frameHeight)//Animations
        {
            NPC.frame.Y = npcframe * frameHeight;
            //NPC.spriteDirection = NPC.direction;
            NPC.frameCounter++;
            if (!halflife) //above half health
            {
                if (!animateattack)
                {                 
                    if (NPC.frameCounter > 6)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }
                    if (npcframe >= 4) //Cycles through frames 0-3 for non attacking
                    {
                        npcframe = 0;
                    }
                }
                if (animateattack)
                {
                    if (NPC.frameCounter > 6)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }
                    if (npcframe <= 3) //Cycles through frames 4-7 for attacking
                    {
                        npcframe = 4;
                    }
                    if  (npcframe >= 7)
                    {
                        animateattack = false;
                        npcframe = 0;

                    }
                }
            }          
            if (halflife)
            {
                if (!animateattack)
                {
                    if (NPC.frameCounter > 6)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }
                    if (npcframe <= 7 || npcframe >= 12) //Cycles through frames 8-11 on last phase
                    {
                        npcframe = 8;
                    }
                }
                if (animateattack)
                {
                    if (NPC.frameCounter > 6)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }
                    if (npcframe <= 11) //Cycles through frames 12-15 for attacking
                    {
                        npcframe = 12;
                    }
                    if (npcframe >= 16)
                    {
                        animateattack = false;
                        npcframe = 8;

                    }
                }
            }
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {

        }
        public override void HitEffect(int hitDirection, double damage)
        {
           
            for (int i = 0; i < 6; i++)
            {
                float speedY = -3f;

                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 138, dustspeed.X, dustspeed.Y, 100, default, 1f);
                Main.dust[dust2].noGravity = true;
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                NPC.velocity *= 0.5f;
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionGenericProj>(), 0, 0, Main.myPlayer);
                Main.projectile[proj].scale = 1.75f;
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("AridBossGore1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("AridBossGore2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("AridBossGore3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("AridBossGore4").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("AridBossGore4").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("AridBossGore4").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("AridBossGore4").Type, 1f);

                }
                for (int i = 0; i < 150; i++)
                {

                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 138);
                    dust.velocity *= 2;
                }
                for (int i = 0; i < 150; i++)
                {
                    float speedY = -12f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X, dustspeed.Y, 100, default, 1.5f);
                    Main.dust[dust2].noGravity = true;
                }
            }
        }
        public override void OnKill()
        { 
            /*if (StormWorld.airdBossDown == false)
            {
                StormWorld.aridBossDown = true;
            }*/
            NPC.SetEventFlagCleared(ref StormWorld.aridBossDown, -1); //set boss downed      
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());

            //boss trophy
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.BossTrophy.AridBossTrophy>(), 10));

            //mask and weapons on normal mode
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanitysets.BossMaskAridBoss>(), 7));

            notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<AncientKnives>(), ModContent.ItemType<AncientFlame>(), ModContent.ItemType<AncientStaff>(), ModContent.ItemType<AncientMinion>()));
            notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Armour.AridBMask>(), ModContent.ItemType<Items.Armour.AridChestplate>(), ModContent.ItemType<Items.Armour.AridGreaves>()));

            //expert and master loot
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.BossTrophy.AridBossBag>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.BossTrophy.AridBossRelic>()));
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<Items.Pets.AridBossPetItem>(), 4));

            npcLoot.Add(notExpert);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) //Trial
        {
            if (!Main.dedServ)
            {
                Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/Boss/AridBoss");

                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + NPC.frame.Size() / 2f + new Vector2(0, -10);
                    Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Lighting.AddLight(NPC.Center, Color.Orange.ToVector3() * 0.5f);

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/Boss/AridBoss_Glow");
            Vector2 drawPos = new Vector2(0, -2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

        }
     
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            position += new Vector2(0, 0); //Moves healthbar down a little
            return true;
        }
    }
}