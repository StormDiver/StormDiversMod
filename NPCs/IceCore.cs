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
using StormDiversMod.Basefiles;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;


namespace StormDiversMod.NPCs

{
    public class IceCore : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frigid Snowflake");
            Main.npcFrameCount[NPC.type] = 9;


        }
        public override void SetDefaults()
        {
            NPC.coldDamage = true;

            NPC.width = 40;
            NPC.height = 40;

            //NPC.aiStyle = 22;

            //aiType = NPCID.Wraith;
            //animationType = NPCID.FlyingSnake;
            
            NPC.damage = 50;
            NPC.lavaImmune = true;
            NPC.defense = 10;
            NPC.lifeMax = 1250;
            NPC.noGravity = true;
            NPC.noTileCollide = false;


            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 1, 0, 0);
            NPC.gfxOffY = 0;
           Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.IceCoreBannerItem>();

           


            NPC.rarity = 2;
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A frozen creature that lurks in the underground ice biome, waiting to rain down sharp icicles upon any unsuspecting adventurer.")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //NPC.lifeMax = (int)(NPC.lifeMax * 0.75f);
            //NPC.damage = (int)(NPC.damage * 0.75f);
        }
        
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (!NPC.AnyNPCs(ModContent.NPCType<IceCore>()) && Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].ZoneSnow && Main.hardMode)
            {
                {
                    return SpawnCondition.Cavern.Chance * 0.05f;
                }
            }
            
            else
            {
                return SpawnCondition.Cavern.Chance * 0f;
            }
        }
        
      
        float ypos = -150;
        float movespeed = 3f; //Speed of the npc
        bool staggered;
        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        public override void AI()
        {
            NPC.buffImmune[BuffID.Frostburn] = true;
            NPC.buffImmune[BuffID.Frostburn2] = true;
            NPC.buffImmune[(BuffType<SuperFrostBurn>())] = true;
            NPC.buffImmune[(BuffType<UltraFrostDebuff>())] = true;
            NPC.buffImmune[BuffID.Confused] = true;
            if (!Main.dedServ)
            {
                Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.6f * Main.essScale);
            }
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            Vector2 moveTo = player.Center;
            Vector2 move = moveTo - NPC.Center + new Vector2(0, ypos);
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);

            if (magnitude > movespeed)
            {
                move *= movespeed / magnitude;
            }
            NPC.velocity = move;

            NPC.spriteDirection = NPC.direction;
            NPC.velocity.Y *= 0.96f;
            


            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
            if (!Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    movespeed = 1.5f;
                    ypos = 0;
                    NPC.noTileCollide = true;
                    NPC.netUpdate = true;

                }
            }
            else
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (!staggered)
                    {
                        movespeed = 2.5f;
                    }
                    ypos = -150;
                    NPC.noTileCollide = false;
                    NPC.netUpdate = true;

                }

            }
            if (player.dead)
            {
                NPC.velocity.Y = -0.5f;
            }
            if (NPC.ai[3] == 0)//phase
            {
                NPC.rotation = NPC.velocity.X / 50;

                if (distance <= 500f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                {
                    NPC.ai[0]++; //shootime
                    NPC.ai[1]++; //phasetime

                    if (NPC.ai[0] >= 15 && (NPC.position.Y < player.position.Y - 100))//fires the projectiles
                    {

                        int damage = 15; 
                        float knockBack = 3;
                        int type = ModContent.ProjectileType<NPCs.NPCProjs.IceCoreProj>();

                        SoundEngine.PlaySound(SoundID.Item30 with{Volume = 0.5f, Pitch = 0.25f}, NPC.Center);

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {


                            Vector2 perturbedSpeed = new Vector2(0, 10).RotatedByRandom(MathHelper.ToRadians(12));
                            float scale = 1f - (Main.rand.NextFloat() * .2f);
                            perturbedSpeed = perturbedSpeed * scale;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + Main.rand.NextFloat(-30f, 30f), NPC.Center.Y + Main.rand.NextFloat(-30f, 30f)), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                        }


                        NPC.ai[0] = 0;

                    }
                }
                else
                {
                    NPC.ai[0] = 0;
                   
                }

                if (NPC.ai[1] >= 360 && Main.netMode != NetmodeID.MultiplayerClient) //Phase 1 to 2
                {
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    NPC.ai[3] = 1;
                    NPC.netUpdate = true;


                }
            }
            if (NPC.ai[3] == 1)//phase
            {
                NPC.rotation += (float)NPC.direction * -0.5f;
                NPC.velocity.X *= 0.5f;
                NPC.velocity.Y *= 0.5f;
                NPC.ai[1]++;//Phasetime
                if (distance <= 500f)
                {
                    NPC.ai[0]++; //shootime
                    if (NPC.ai[0] >= 80 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                    {
                        SoundEngine.PlaySound(SoundID.Item30, NPC.Center);

                        float numberProjectiles = 10 + Main.rand.Next(4);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < numberProjectiles; i++)
                            {

                                int damage = 20; 
                                float knockBack = 3;
                                int type = ModContent.ProjectileType<NPCs.NPCProjs.IceCoreProj>();

                                float speedX = 0f;
                                float speedY = -8f;
                                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(150));
                                float scale = 1f - (Main.rand.NextFloat() * .5f);
                                perturbedSpeed = perturbedSpeed * scale;
                                //Projectile.NewProjectile(player.Center.X, player.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("FrostAccessProj"), 50, 3f, player.whoAmI);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);


                            }
                            for (int i = 0; i < 30; i++)
                            {

                                Dust dust;
                                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                                Vector2 position = NPC.position;
                                dust = Main.dust[Terraria.Dust.NewDust(position, NPC.width, NPC.height, 92, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                                dust.noGravity = true;
                            }
                        }
                        NPC.ai[0] = 0;
                    }
                }
                    
                if (NPC.ai[1] >= 250 && Main.netMode != NetmodeID.MultiplayerClient) //Phase 2 to 1
                {                  
                    NPC.ai[0] = 0;                  
                    NPC.ai[1] = 0;
                    NPC.ai[3] = 0;
                    NPC.netUpdate = true;

                }
            }

            if (Main.rand.Next(4) == 0) //Dust effects
            {
                var dust3 = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Bottom.Y - 10), 10, 20, 135, 0, 10);
                dust3.noGravity = true;
            }

            if (staggered)//when hit greatly slowdown
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[2]++;

                    movespeed = 0.4f;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.ai[2] > 15)
            {
                staggered = false;
                NPC.ai[2] = 0;
            }
            /*if (NPC.collideX)
            {
                NPC.velocity.X = NPC.velocity.X * -1;
            }
            if (NPC.collideY)
            {
                NPC.velocity.Y = NPC.velocity.Y * -1;
            }*/
        }
        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
          
            if (NPC.ai[3] == 0)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 5)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 6) //Cycles through frames 0-6 when  in phase 1
                {
                    npcframe = 0;
                }
            }
            if (NPC.ai[3] == 1)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 5)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe < 7 || npcframe >= 9) //Cycles through frames 7-8 when in phase 2
                {
                    npcframe = 7;
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Chilled, 600);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            staggered = true;
            NPC.ai[2] = 0;

            if (NPC.ai[3] == 0)
            {
                NPC.ai[0] = -5;
            }
            if (NPC.ai[3] == 1)
            {
                NPC.ai[0] = 50;
            }
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 2; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 135);
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) they will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("IceCoreGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("IceCoreGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("IceCoreGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("IceCoreGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("IceCoreGore4").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("IceCoreGore4").Type, 1f);


                for (int i = 0; i < 40; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 135);
                    dust.scale = 2;
                    dust.velocity *= 3;
                    dust.noGravity = true;
                }
                

            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
            LeadingConditionRule isExpert = new LeadingConditionRule(new Conditions.IsExpert());

            isExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.IceOre>(), 1, 12, 18));
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.IceOre>(), 1, 10, 15));

            npcLoot.Add(ItemDropRule.OneFromOptionsWithNumerator(3, 2, ModContent.ItemType<Items.Accessory.FrostAccess>(), ItemID.FrostCore));

            npcLoot.Add(notExpert);
            npcLoot.Add(isExpert);

        }


        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}