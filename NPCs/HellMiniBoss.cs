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

            NPC.width = 44;
            NPC.height = 44;

            NPC.aiStyle = 14; 
            AIType = NPCID.FlyingSnake;
            //animationType = NPCID.BlueSlime;
            
            NPC.damage = 60;
            
            NPC.defense = 15;
            NPC.lifeMax = 3000;
            
            NPC.rarity = 3;
            
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 2, 0, 0);
            //NPC.noTileCollide = true;

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
                return SpawnCondition.Underworld.Chance * 0.06f;
            }
            else
            {
                return SpawnCondition.Underworld.Chance * 0f;
            }

        }
        int shoottime = 0;
        int phasetime; //How long to remain in a phase
       
        int phase = 0; //0 = phase 1, 1 = phase 2, 2 = phase 3

        public override void AI()
        {
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.OnFire3] = true;
            NPC.buffImmune[(BuffType<SuperBurnDebuff>())] = true;
            NPC.buffImmune[(BuffType<HellSoulFireDebuff>())] = true;
            NPC.buffImmune[(BuffType<UltraBurnDebuff>())] = true;
            NPC.buffImmune[BuffID.Confused] = true;

            NPC.spriteDirection = NPC.direction;
            NPC.rotation = NPC.velocity.X / 12;
            if (!Main.dedServ)
            {
                Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.3f * Main.essScale);
            }
            NPC.rotation = NPC.velocity.X / 100;


            Player player = Main.player[NPC.target];
            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
            if (phase == 0) //phase1 _________________________________________________________________________________________________________________________
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


                        SoundEngine.PlaySound(SoundID.Item8, NPC.Center);


                        for (int i = 0; i < 3; i++)
                        {
                            float posX = NPC.position.X + Main.rand.NextFloat(45f, -45f);
                            float posY = NPC.position.Y + Main.rand.NextFloat(45f, -45f);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {

                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(posX, posY), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
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

                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Top.Y), NPC.width, NPC.height / 2, 135, 0, -10);
                    dust2.scale = 1.5f;
                    dust2.noGravity = true;

                }
                if (phasetime >= 400 && Main.netMode != NetmodeID.MultiplayerClient) //Phase 1 to 2
                {
                    shoottime = 0;
                    
                    phasetime = 0;
                    phase = 1;
                    NPC.netUpdate = true;
                }
            }
            if (phase == 1) //Phase2_________________________________________________________________________________________________________________________
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


                        SoundEngine.PlaySound(SoundID.Item8, NPC.Center);


                        for (int i = 0; i < 1; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {                       
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), type, damage, knockBack);

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

                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Top.Y), NPC.width, NPC.height / 2, 135, 0, -10);
                    dust2.scale = 1.5f;
                    dust2.noGravity = true;

                }
                if (phasetime >= 500 && Main.netMode != NetmodeID.MultiplayerClient) //Phase 2 to 3, or back to 0 if too many minions are summoned
                {
                    shoottime = 0;
                    phasetime = 0;
                    if (NPC.CountNPCS(ModContent.NPCType<HellMiniBossMinion>()) < 9)
                    {
                        phase = 2;
                    }
                    else
                    {
                        phase = 0;
                    }
                    NPC.netUpdate = true;

                }
            }
            if (phase == 2) //summons minions _________________________________________________________________________________________________________________________
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


                        SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            NPC.NewNPC(NPC.GetSource_FromAI(), (int)Math.Round(NPC.Center.X), (int)Math.Round(NPC.Center.Y), type);


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


                if (phasetime >= 210 && Main.netMode != NetmodeID.MultiplayerClient) //Phase 3 to 1
                {
                    shoottime = 0;
                    phasetime = 0;
                    phase = 0;
                    NPC.netUpdate = true;
                }
            }
            if (player.dead && Main.netMode != NetmodeID.MultiplayerClient)
            {
                phase = 0;
                shoottime = 0;
                phasetime = 0;
                NPC.netUpdate = true;

            }

            if (Main.rand.Next(4) == 0)
            {

                var dust3 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Bottom.Y - 12), NPC.width, 6, 6, 0, 5);
                dust3.noGravity = false;
                    dust3.scale = 0.8f;

            }
        }

        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            if (phase != 2)
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
            else
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 3)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe <= 2 || npcframe >= 6) //Cycles through frames 3-5 when summoning minions
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
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 2; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 173);
                dust.noGravity = true;
                dust.scale = 1.5f;
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) it will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossGore4").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossGore5").Type, 1f);

                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 173);
                    
                    dust.noGravity = true;
                    dust.velocity *= 5;
                    dust.scale = 2f;
                }
                for (int i = 0; i < 20; i++)
                {

                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 135);
                    dust2.scale = 1.5f;
                    dust2.noGravity = true;

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

            isExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SoulFire>(), 1, 15, 24));
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SoulFire>(), 1, 12, 20));

            /*int itemType = ModContent.ItemType<Items.Materials.SoulFire>();
            var parameters = new DropOneByOne.Parameters()
            {
                ChanceNumerator = 1,
                ChanceDenominator = 1,
                MinimumStackPerChunkBase = 1,
                MaximumStackPerChunkBase = 1,
                MinimumItemDropsCount = 15,
                MaximumItemDropsCount = 24,
            };

            isExpert.OnSuccess(new DropOneByOne(itemType, parameters));*/


            npcLoot.Add(notExpert);
            npcLoot.Add(isExpert);

        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/HellMiniBoss_Glow");
            Vector2 drawPos = new Vector2(0, -2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 200;
            return color;

        }
    }
}