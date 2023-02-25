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

namespace StormDiversMod.NPCs
{
    [AutoloadBossHead]
    public class MoonDerp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonling"); // Automatic from .lang files
                                                // make sure to set this for your modnpcs.
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 4;
            NPCID.Sets.MPAllowedEnemies[Type] = true;

        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;

            NPC.width = 60;
            NPC.height = 42;
          
            NPC.damage = 125;

            NPC.defense = 60;
            NPC.lifeMax = 30000;
            NPC.aiStyle = -1;

            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit56;
            NPC.DeathSound = SoundID.NPCDeath62;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 10, 0, 0);
            NPC.rarity = 5;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.MoonDerpBannerItem>();
            //NPC.boss = true;
            NPC.BossBar = Main.BigBossProgressBar.NeverValid;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A Derpling that has been corrupted by lunar energy, allowing it to possess a fraction of the Moon Lord’s power.")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax / 3 * 2);
        }
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
            if (halflife3 && slot != -1)
            {
                // If the boss is in its second stage, display the other head icon instead
                index = slot;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (NPC.downedMoonlord && !NPC.AnyNPCs(ModContent.NPCType<MoonDerp>()) && !GetInstance<ConfigurationsGlobal>().NoMoonling4U)
            {
                return SpawnCondition.Sky.Chance * 0.07f;
            }
            else
            {
                return SpawnCondition.Sky.Chance * 0f;
            }
        }

        int shootspeed = 0; // TIem between the 2 shots
        int eyetime = 0; // Counts up to fire the Eyes
        bool halflife3 = false; //When below half health enter second phase
        int timetoshoot = 120; //How often bolts will fire, gets faster in phase 2
        int timetoshootspeed = 10; //How rapid it fires
        float movespeed = 8f; //Speed of the npc

        
        public int poschoice = 1;
        public override void AI()
        {
            //NPC.ai[0] = Xpos
            //NPC.ai[1] = Ypos
            // NPC.ai[2] = Pos choice
            // NPC.ai[3] = Shoottime
            NPC.buffImmune[BuffID.Confused] = true;



            Player player = Main.player[NPC.target]; //Code to move towards player
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.ai[2] == 0) //Top 
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = -150f;


                }
                else if (NPC.ai[2] == 1) // left
                {
                    NPC.ai[0] = -200f;
                    NPC.ai[1] = 0;

                }
                else if (NPC.ai[2] == 2) //  right
                {
                    NPC.ai[0] = 200f;
                    NPC.ai[1] = 0f;

                }
                else if (NPC.ai[2] == 3) //Bottom 
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 150f;

                }
                else if (NPC.ai[2] == 4) //On top of player
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = -20f;

                }
            }
            /*if (player.position.X > NPC.position.X)
            {
                NPC.spriteDirection = 1;
                NPC.direction = 1;
            }
            else if (player.position.X < NPC.position.X)
            {
                NPC.spriteDirection = -1;
                NPC.direction = -1;

            }
            else*/
            {
                NPC.spriteDirection = NPC.direction;
            }
            NPC.TargetClosest();
            Vector2 moveTo = player.Center;
            Vector2 move = moveTo - NPC.Center + new Vector2(NPC.ai[0], NPC.ai[1]); //Postion around player
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > movespeed)
            {
                move *= movespeed / magnitude;
            }
            NPC.velocity = move;

            NPC.rotation = NPC.velocity.X / 12;
            //NPC.velocity.Y *= 0.96f;

            if (player.dead)
            {
                NPC.velocity.Y = -8;
            }
            if (Vector2.Distance(player.Center, NPC.Center) <= 1000f) //&& Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height)
            {
                NPC.ai[3]++;

                if (NPC.ai[3] >= timetoshoot)
                {
                    float projectileSpeed = 13f; // The speed of your projectile (in pixels per second).
                    int damage = 40; // The damage your projectile deals. normal x2, expert x4
                    float knockBack = 2;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.MoonDerpBoltProj>();
                    //int type = ProjectileID.PhantasmalBolt;
                    shootspeed++;
                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Top.Y)) * projectileSpeed;

                    if (shootspeed == timetoshootspeed)
                    {

                        for (int i = 0; i < 1; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10)); // 30 degree spread.
                                                                                                                                        // If you want to randomize the speed to stagger the projectiles
                                float scale = 1f - (Main.rand.NextFloat() * .3f);
                                perturbedSpeed = perturbedSpeed * scale;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Top.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);


                            }
                            SoundEngine.PlaySound(SoundID.Item124, NPC.Center);
                        }
                        shootspeed = 0;
                    }
                    if (NPC.ai[3] >= (timetoshoot + 20))
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.ai[2] = Main.rand.Next(0, 5); //Picks one of the 5 random postions after each shot
                            NPC.netUpdate = true;
                        }
                        //xpostion *= -1f;
                        //ypostion *= -1f;
                        NPC.ai[3] = 0;
                        shootspeed = 0;
                    }

                }
                if (halflife3 && Main.expertMode)
                {
                    eyetime++;
                }
                if (eyetime >= 180)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Top.Y), new Vector2(-3, -4), ModContent.ProjectileType<NPCs.NPCProjs.MoonDerpEyeProj>(), 35, 6);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Top.Y), new Vector2(+3, -4), ModContent.ProjectileType<NPCs.NPCProjs.MoonDerpEyeProj>(), 35, 6);

                    }
                    SoundEngine.PlaySound(SoundID.Zombie103, NPC.Center);

                    eyetime = 0;
                }

            }

            else
            {
                NPC.ai[3] = 40;
                shootspeed = 0;
                eyetime = 100;
            }
            if (halflife3)
            {
                if (Main.rand.Next(10) == 0)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 156);
                }
            }
            if (NPC.life < NPC.lifeMax * 0.5f && halflife3 == false) //Entering phase 2--------------------------------------------------------------------------------------
            {
                NPC.defense = 50;
                timetoshoot = 90;
                timetoshootspeed = 6;
                NPC.velocity *= 0;
                movespeed += 3;
                NPC.ai[3] = 0;

                halflife3 = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                    return;
                }

                SoundEngine.PlaySound(SoundID.Zombie101, NPC.Center);
                Gore.NewGore(null, NPC.Center, NPC.velocity, Mod.Find<ModGore>("MoonDerpGore6").Type, 1f);
                Gore.NewGore(null, NPC.Center, NPC.velocity, Mod.Find<ModGore>("MoonDerpGore6").Type, 1f);
                Gore.NewGore(null, NPC.Center, NPC.velocity, Mod.Find<ModGore>("MoonDerpGore3").Type, 1f);
                Gore.NewGore(null, NPC.Center, NPC.velocity, Mod.Find<ModGore>("MoonDerpGore4").Type, 1f);

                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 156);

                }

                //NPC.damage = NPC.damage / 10 * 14;  //Would be 40% extra damage but too OP

            }
        }
        int npcframe = 0;

        public override void FindFrame(int frameHeight)
        {
            //NPC.spriteDirection = NPC.direction;

            if (halflife3)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 8)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe <= 3 || npcframe >=8) //Cycles through frames 4-7 when below half health
                {
                    npcframe = 4;
                }
            }
            else
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 5)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 4) //Cycles through frames 0-3 when above half health
                {
                    npcframe = 0;
                }
            }

        }
        int choice;
        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 2; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, 265);
                dust.scale = 0.75f;
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MoonDerpGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MoonDerpGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MoonDerpGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MoonDerpGore4").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MoonDerpGore5").Type, 1f);
                choice = Main.rand.Next(4);

                for (int i = 0; i < 25; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 265);
                }
                for (int i = 0; i < 25; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 229);
                }


            }
        }
        //int drops;
        public override void OnKill()
        {
            /*int fragdrop;
            if (Main.expertMode)
            {
                drops = 12 + Main.rand.Next(6); //12-18 on Expert (+1 from bestiary, 13 - 19)
            }
            else
            {
                drops = 10 + Main.rand.Next(4); // 10 - 14 on Normal  (+1 from bestiary, 11- 15)

            }
            for (int i = 0; i < drops; i++) //Drops random fragments until the total is reached
            {
                int choice = Main.rand.Next(4);
                {
                    if (choice == 0)
                    {
                        fragdrop = ItemID.FragmentVortex;
                    }
                    else if (choice == 1)
                    {
                        fragdrop = ItemID.FragmentSolar;
                    }
                    else if (choice == 2)
                    {
                        fragdrop = ItemID.FragmentStardust;
                    }
                    else
                    {
                        fragdrop = ItemID.FragmentNebula;
                    }
                    
                        Item.NewItem(NPC.GetSource_Loot(), (int)NPC.Center.X, (int)NPC.Center.Y, NPC.width, NPC.height, fragdrop);
                    
                   
                }
            }*/
            for (int i = 0; i < 5; i++) 
            {
                Item.NewItem(NPC.GetSource_Loot(), (int)NPC.Center.X, (int)NPC.Center.Y, NPC.width, NPC.height, ItemID.Heart);
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
            LeadingConditionRule isExpert = new LeadingConditionRule(new Conditions.IsExpert());

            /*IItemDropRule[] fragmentdrops = new IItemDropRule[] {
                ItemDropRule.Common(ItemID.FragmentSolar, 1, 6, 12),
                ItemDropRule.Common(ItemID.FragmentVortex, 1, 6, 12),
                ItemDropRule.Common(ItemID.FragmentNebula, 1, 6, 12),
                ItemDropRule.Common(ItemID.FragmentStardust, 1, 6, 12),
            };
            npcLoot.Add(new FewFromRulesRule(4, 1, fragmentdrops));*/
            isExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentSolar, 1, 6, 12));
            notExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentSolar, 1, 5, 10));
            isExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentVortex, 1, 6, 12));
            notExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentVortex, 1, 5, 10));
            isExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentNebula, 1, 6, 12));
            notExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentNebula, 1, 5, 10));
            isExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentStardust, 1, 6, 12));
            notExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentStardust, 1, 5, 10));

            isExpert.OnSuccess(ItemDropRule.Common(ItemID.LunarOre, 1, 20, 30));
            notExpert.OnSuccess(ItemDropRule.Common(ItemID.LunarOre, 1, 15, 25));

            npcLoot.Add(notExpert);
            npcLoot.Add(isExpert);

            //npcLoot.Add(ItemDropRule.OneFromOptions(1, ItemID.FragmentSolar, ItemID.FragmentVortex, ItemID.FragmentNebula, ItemID.FragmentStardust));
 
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/MoonDerp_Glow");
            Vector2 drawPos = new Vector2(0, 2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
          
            Main.instance.LoadProjectile(NPC.type);
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/MoonDerp");

            //Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + NPC.frame.Size() / 2f + new Vector2(-2, 2);
                Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }

            Texture2D texture2 = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/MoonDerp_Glow");
            float speen1 = 9f + 3f * (float)Math.Cos((float)Math.PI * 2f * Main.GlobalTimeWrappedHourly);
            Vector2 spinningpoint5 = Vector2.UnitX * speen1;
            Color color2 = Color.LightSeaGreen * (speen1 / 12f) * 0.8f;
            color2.A /= 3;
            if (halflife3)
            {
                for (float speen2 = 0f; speen2 < (float)Math.PI * 2f; speen2 += (float)Math.PI / 2f)
                {
                    Vector2 finalpos = NPC.position + new Vector2(0, 30) + spinningpoint5.RotatedBy(speen2);
                    spriteBatch.Draw(texture, new Vector2(finalpos.X - screenPos.X + (float)(NPC.width / 2) * NPC.scale, finalpos.Y - screenPos.Y + (float)NPC.height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f * NPC.scale - 16), NPC.frame, color2, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }
            return true;
        }
    }
}