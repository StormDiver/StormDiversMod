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

namespace StormDiversMod.NPCs.Boss

{
    public class AridBossMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Spirit"); 
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
            
            NPC.width = 28;
            NPC.height = 44;

            NPC.aiStyle = -1;

            NPC.damage = 0;//No contact damage
           
            NPC.defense = 0;
            NPC.lifeMax = 750;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.2f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.noGravity = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            //Banner = Item.NPCtoBanner(ModContent.NPCType<HellMiniBossMinion>());
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            //Unlock automatically when main derpling is defeated
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPCType<AridBoss>()], quickUnlock: true);
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Ghosts of the Ancient husk's victims, doomed to forever protect their new master in battle")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //NA, has no health scaling so always 750
            if (Main.masterMode)
            {
                NPC.lifeMax = (int)(NPC.lifeMax * 0.6f); //1080
            }
            else if (Main.expertMode && !Main.masterMode)
            {
                NPC.lifeMax = (int)(NPC.lifeMax * 0.75f); //900

            }
            //NPC.damage = (int)(NPC.damage * 0.75f);
        }
        
        int shoottime = 0;
        bool shooting;
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 50; i++)
            {
                float speedY = -3f;

                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X, dustspeed.Y, 100, default, 1f);
                Main.dust[dust2].noGravity = true;
            }

            NPC.ai[1] = Main.rand.Next(0, 360);
            NPC.ai[2] = Main.rand.Next(0, 2);
        }
        float speed = 9;
        float inertia = 40;
        float distanceToIdlePosition; //distance to player

        public override void AI()
        {
            NPC.noTileCollide = true;
            NPC.buffImmune[(BuffType<AridSandDebuff>())] = true;
            NPC.buffImmune[BuffID.Confused] = true;

            shoottime++;
            NPC.rotation = NPC.velocity.X / 50;
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            Player player = Main.player[NPC.target]; //Code to move towards player

            if (!player.dead)
            {
                if (NPC.ai[2] == 0)
                {
                    NPC.ai[1] -= 2;
                }
                else
                {
                    NPC.ai[1] += 2;

                }
                double deg = (NPC.ai[1]);
                double rad = deg * (Math.PI / 180);
                double dist = 150; //Distance away from the player

                //position
                NPC.localAI[2] = (int)(Math.Cos(rad) * dist);
                NPC.localAI[3] = (int)(Math.Sin(rad) * dist);


                Vector2 idlePosition = player.Center + new Vector2(NPC.localAI[2], NPC.localAI[3]);
                Vector2 vectorToIdlePosition = idlePosition - NPC.Center;
                //distanceToIdlePosition = vectorToIdlePosition.Length();

                //speed is dependant on attack
                distanceToIdlePosition = vectorToIdlePosition.Length();

                //speed is dependant on attack
                if (distanceToIdlePosition > 10f)
                {
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                }
                NPC.velocity = (NPC.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                NPC.rotation = NPC.velocity.X / 50;

                if (distanceToIdlePosition > 300f)
                {
                    // Speed up if it's away from the player
                    speed = 8f;
                    inertia = 30f;
                }
                else
                {
                    // Slow down if closer to the player
                    speed = 6f;
                    inertia = 40f;
                }
            }
            else
            {
                NPC.rotation = 0;


                if (NPC.velocity.Y < 25)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.velocity.Y += 0.2f;
                        NPC.netUpdate = true;
                    }
                }

            }
            if (NPC.position.X >= player.position.X)
            {
                NPC.spriteDirection = 1;
            }
            if (NPC.position.X < player.position.X)
            {
                NPC.spriteDirection = -1;

            }
            
            float distance = Vector2.Distance(player.Center, NPC.Center);

            if (distance  <= 600f)
            {
                if (shoottime >= 100)
                {                
                    float speedY = -2f;
                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    int dust = Dust.NewDust(NPC.Center, 0, 0, 55, dustspeed.X + NPC.velocity.X, dustspeed.Y + NPC.velocity.Y, 100, default, 1f);
                    Main.dust[dust].noGravity = true;

                    Vector2 dustvelocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * 8;
                    int dust2 = Dust.NewDust(NPC.Center, 0, 0, 55, dustvelocity.X + NPC.velocity.X, dustvelocity.Y + NPC.velocity.Y, 100, default, 1f);
                    Main.dust[dust2].noGravity = true;
                }
                if (shoottime >= 120)
                {
                    shooting = true;

                    float projectileSpeed = 13f; // The speed of your projectile (in pixels per second).
                    int damage = 12; // 24/48/72 damage
                    float knockBack = 3;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.AridBossSandProj>();

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;


                    SoundEngine.PlaySound(SoundID.Item20 with { Volume = 1f, Pitch = 0.2f }, NPC.Center);

                    for (int i = 0; i < 1; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10)); 
                                                                                                                                    
                            float scale = 1f - (Main.rand.NextFloat() * .3f);
                            perturbedSpeed = perturbedSpeed * scale;
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                            Main.projectile[proj].scale = 0.5f;
                            Main.projectile[proj].timeLeft = 60;

                        }
                    }
                                      
                    shoottime = 0;
                }
            }
            else
            {
                shoottime = 70;
                shooting = false;

            }
            if (Main.rand.Next(5) == 0)
            {

                var dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Top.Y), NPC.width, NPC.height / 2, 55, 0, 3);
                dust2.scale = 0.75f;
                dust2.noGravity = true;

            }
            if (Main.rand.Next(7) == 0)
            {

                var dust3 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Bottom.Y - 4), NPC.width, 6, 138, 0, 3);
                dust3.noGravity = false;
                dust3.scale = 0.75f;
            }

            if (NPC.CountNPCS(ModContent.NPCType<AridBoss>()) == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        int goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                        Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                        goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                        Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                        goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                        Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                        goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                        Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                    }
                }
                SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1f, Pitch = 0f }, NPC.Center);

                NPC.life = 0;
            }
        }

        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = npcframe * frameHeight;
            NPC.frameCounter++;
            if (!shooting)
            {
                if (NPC.frameCounter > 8)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 4) //Cycles through frames 0-3
                {
                    npcframe = 0;
                }
            }
            if (shooting)
            {
                if (NPC.frameCounter > 8)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe <= 3) //Cycles through frames 4-7 then resets
                {
                    npcframe = 4;
                }
                if (npcframe >= 8)
                {
                    shooting = false;
                }
            }

        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
                        
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 138);
                dust.scale = 0.5f;

            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
             
                for (int i = 0; i < 2; i++)
                {
                    int goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;
        }
    }
}