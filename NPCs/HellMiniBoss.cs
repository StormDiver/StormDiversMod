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
    public class HellMiniBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Cauldron"); // Automatic from .lang files
                                                      // make sure to set this for your modnpcs.

        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;

            NPC.width = 42;
            NPC.height = 38;

            NPC.aiStyle = 14; 
            AIType = NPCID.FlyingSnake;
            //animationType = NPCID.BlueSlime;
            
            NPC.damage = 60;
            
            NPC.defense = 25;
            NPC.lifeMax = 3000;
            
            NPC.rarity = 3;
            

            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 2, 0, 0);

            

            NPC.lavaImmune = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.HellMiniBossBannerItem>();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A cauldron filled with hundreds of burning souls, doomed to spend eternity in hell.")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //NPC.lifeMax = (int)(NPC.lifeMax * 0.75f);
            //NPC.damage = (int)(NPC.damage * 0.75f);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (!NPC.AnyNPCs(ModContent.NPCType<HellMiniBoss>()) && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                return SpawnCondition.Underworld.Chance * 0.07f;
            }
            else
            {
                return SpawnCondition.Underworld.Chance * 0f;
            }

        }
        int shoottime = 0;
        int phasetime; //How long to remain in a phase
        bool phase1 = true; //FIrts phase, shoots regular souls
        bool phase2; //Summons homing souls
        bool phase3; //Summons 2-3 minions


        public override void AI()
        {
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[(BuffType<SuperBurnDebuff>())] = true;
            NPC.buffImmune[(BuffType<HellSoulFireDebuff>())] = true;
            NPC.buffImmune[(BuffType<UltraBurnDebuff>())] = true;
            NPC.buffImmune[BuffID.Confused] = true;

            NPC.spriteDirection = NPC.direction;
            NPC.rotation = NPC.velocity.X / 12;

            Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.3f * Main.essScale);


            Player player = Main.player[NPC.target];
            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
            if (phase1) //phase1 _________________________________________________________________________________________________________________________
            {
                NPC.defense = 15;

                if (distance <= 800f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                {
                    phasetime++;
                    shoottime++;

                    if (shoottime >= 90)
                    {
                        float projectileSpeed = 4.5f; // The speed of your projectile (in pixels per second).
                        int damage = 30; // The damage your projectile deals. normal x2, expert x4
                        float knockBack = 1;
                        int type = ModContent.ProjectileType<NPCs.NPCProjs.HellMiniBossProj1>();

                        Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                        new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;


                        SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 8);


                        for (int i = 0; i < 3; i++)
                        {
                            float posX = NPC.position.X + Main.rand.NextFloat(45f, -45f);
                            float posY = NPC.position.Y + Main.rand.NextFloat(45f, -45f);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {

                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                                Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), new Vector2(posX, posY), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                            }
                        }
                        NPC.velocity *= 0;
                        for (int i = 0; i < 20; i++)
                        {

                            var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 173);
                            dust.noGravity = true;
                            dust.velocity *= 3;
                            dust.scale = 2f;

                        }

                        shoottime = 0;
                    }
                }
                else
                {
                    shoottime = 60;

                }
                if (Main.rand.Next(5) == 0)
                {

                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Top.Y), NPC.width, NPC.height / 2, 135, 0, -2);
                    dust2.scale = 1.5f;
                    dust2.noGravity = true;

                }
                if (phasetime >= 400) //Phase 1 to 2
                {
                    shoottime = 0;

                    phase2 = true;
                    phase1 = false;
                    phasetime = 0;
                }
            }
            if (phase2) //Phase2_________________________________________________________________________________________________________________________
            {
                phasetime++;
                shoottime++;
                if (distance <= 800f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                {

                    if (shoottime >= 150)
                    {
                        float projectileSpeed = 5f; // The speed of your projectile (in pixels per second).
                        int damage = 20; // The damage your projectile deals. normal x2, expert x4
                        float knockBack = 1;
                        int type = ModContent.ProjectileType<NPCs.NPCProjs.HellMiniBossProj2>();

                        Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                        new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;


                        SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 8);


                        for (int i = 0; i < 1; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {                       
                                Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), type, damage, knockBack);

                                NPC.velocity *= 0.5f;

                            }
                        }
                        for (int i = 0; i < 20; i++)
                        {

                            var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 173);
                            dust.noGravity = true;
                            dust.velocity *= 3;
                            dust.scale = 1.5f;
                        }

                        shoottime = 0;
                    }
                }
                else
                {
                    shoottime = 70;

                }
                if (Main.rand.Next(3) == 0)
                {

                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Top.Y), NPC.width, NPC.height / 2, 135, 0, -4);
                    dust2.scale = 1.5f;
                    dust2.noGravity = true;

                }
                if (phasetime >= 500) //Phase 2 to 3
                {
                    shoottime = 0;

                    phase3 = true;
                    phase2 = false;
                    phasetime = 0;
                }
            }
            if (phase3) //summons minions _________________________________________________________________________________________________________________________
            {
                phasetime++;
                shoottime++;
                NPC.defense = 100;
                NPC.velocity *= 0f;
                if (distance <= 1200f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                {
                    if (shoottime >= 60)
                    {

                        int type = ModContent.NPCType<HellMiniBossMinion>();


                        SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 8);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            NPC.NewNPC((int)Math.Round(NPC.Center.X), (int)Math.Round(NPC.Center.Y), type);


                            for (int i = 0; i < 30; i++)
                            {
                                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 15, NPC.Center.Y - 15), 30, 30, 173);
                                dust.noGravity = true;
                                dust.velocity *= 3;
                                dust.scale = 1.5f;
                            }
                        }

                        shoottime = 0;
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Center.Y - 10), NPC.width, 4, 173, 0, -5);
                    dust2.noGravity = true;
                    dust2.scale = 1.5f;
                }

                if (phasetime >= 210) //Phase 3 to 1
                {
                    shoottime = 0;

                    phase1 = true;
                    phase3 = false;
                    phasetime = 0;
                }
            }
            if (player.dead)
            {
                phase1 = true;
                phase2 = false;
                phase3 = false;
                shoottime = 0;
                phasetime = 0;
            }

            if (Main.rand.Next(2) == 0)
            {

                var dust3 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Bottom.Y - 6), NPC.width, 6, 173, 0, 5);
                dust3.noGravity = true;

            }
        }

        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            if (phase1 || phase2)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 6)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 3) //Cycles through frames 0-2 when in phase 1 or 2
                {
                    npcframe = 0;
                }
            }
           
            if (phase3)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 3)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe <= 2 || npcframe >= 6) //Cycles through frames 3-5 when in phase 3
                {
                    npcframe = 3;
                }
            }

        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.HellSoulFireDebuff>(), 300);

        }
        public override void HitEffect(int hitDirection, double damage)
        {

            for (int i = 0; i < 2; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 173);
                dust.noGravity = true;
                dust.scale = 1.5f;
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) it will spawn this
            {
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossGore1").Type, 1f);
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossGore2").Type, 1f);
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossGore3").Type, 1f);
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossGore4").Type, 1f);
             
                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 173);
                    
                    dust.noGravity = true;
                    dust.velocity *= 5;
                    dust.scale = 2f;
                }
                for (int i = 0; i < 20; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 31, 0f, 0f, 0, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }

            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
            LeadingConditionRule isExpert = new LeadingConditionRule(new Conditions.IsExpert());

            isExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SoulFire>(), 1, 12, 16));
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SoulFire>(), 1, 10, 14));

            npcLoot.Add(notExpert);
            npcLoot.Add(isExpert);

        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/HellMiniBoss_Glow");
            Vector2 drawPos = new Vector2(0, 2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

        }
        
    }
}