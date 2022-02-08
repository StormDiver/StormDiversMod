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


namespace StormDiversMod.NPCs

{
    public class MushroomMiniBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angry Mushroom"); // Automatic from .lang files
                                                      // make sure to set this for your modnpcs.
            Main.npcFrameCount[NPC.type] = 5;

        }
        public override void SetDefaults()
        {
            
            NPC.width = 34;
            NPC.height = 34;

            NPC.aiStyle = 1; 
            AIType = NPCID.Crimslime;
            //animationType = NPCID.BlueSlime;
            
            NPC.damage = 30;
            
            NPC.defense = 5;
            NPC.lifeMax = 175;
            
            NPC.rarity = 2;
            

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.9f;
            NPC.value = Item.buyPrice(0, 0, 50, 0);

           Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.MushroomMiniBossBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A fungi that has gained sentience, however it is not a very fun guy.")
            });
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //NPC.lifeMax = (int)(NPC.lifeMax * 0.75f);
            //NPC.damage = (int)(NPC.damage * 0.75f);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (!NPC.AnyNPCs(ModContent.NPCType<MushroomMiniBoss>()) && NPC.downedBoss1 && Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].ZoneGlowshroom)
            {
                return SpawnCondition.Cavern.Chance * 0.2f;
            }
            else
            {
                return SpawnCondition.Cavern.Chance * 0f;
            }

        }
        int shoottime = 0;

        bool summoning;
        bool jumping;
        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;

            if (NPC.velocity.Y == 0)
            {
                shoottime++;
            }

            Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.3f * Main.essScale);


            Player player = Main.player[NPC.target];
            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
            
            if (distance  <= 600f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                if (shoottime >= 100)
                {
                    if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
                    {
                        

                            var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Top.Y), NPC.width, 3, 113);

                            dust.noGravity = true;
                        

                    }
                    NPC.velocity.X = 0;
                    NPC.velocity.Y = 0;
                    summoning = true;
                }
                if (shoottime >= 120)
                {
                    float projectileSpeed = 10f; // The speed of your projectile (in pixels per second).
                    int damage = 15; // The damage your projectile deals. normal x2, expert x4
                    float knockBack = 1;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.MushroomMiniBossProj>();

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;


                    SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 8);


                    for (int i = 0; i < 1; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0)); 
                          
                            Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), new Vector2(NPC.Center.X, NPC.TopLeft.Y), new Vector2(-3, -6), type, damage, knockBack);
                            Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), new Vector2(NPC.Center.X, NPC.TopRight.Y), new Vector2(+3, -6), type, damage, knockBack);

                        }
                    }
                    for (int i = 0; i < 20; i++)
                    {

                        var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 113);
                        dust.noGravity = true;
                        dust.velocity *= 3;

                    }

                    shoottime = 0;
                    summoning = false;
                }
            }
            else
            {
                shoottime = 70;
                summoning = false;

            }
            if (NPC.velocity.Y != 0)
            {
                jumping = true;
            }
            else
            {
                jumping = false;
            }


            if (Main.rand.Next(6) == 0)
            {
                
                    int dust = Dust.NewDust(NPC.position - new Vector2(2f, 2f), NPC.width + 4, NPC.height + 4, 113, NPC.velocity.X, NPC.velocity.Y);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                
            }
        }

        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            if (jumping)
            {
                NPC.frame.Y = 2 * frameHeight; //Picks frame 2 when in the air
            }
            if (summoning)
            {
                //NPC.frame.Y = 3 * frameHeight; //Cycles bewteen frame 3 and 4 when summoning msuhroom
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 2)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 5 || npcframe <= 2) //Cycles through frames 2-3 when not summoning
                {
                    npcframe = 3;
                }
            }
            if (!jumping && !summoning)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 10)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 2) //Cycles through frames 0-1 when not casting
                {
                    npcframe = 0;
                }
            }

        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
              
            
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            shoottime = 70;
            summoning = false;

            for (int i = 0; i < 3; i++)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(-2, -2), Main.rand.NextFloat(2, 2));
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 31);
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("MushroomMiniBossGore1").Type, 1f);
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("MushroomMiniBossGore2").Type, 1f);
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("MushroomMiniBossGore3").Type, 1f);
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("MushroomMiniBossGore4").Type, 1f);

                for (int i = 0; i < 10; i++)
                {
                    Vector2 vel = new Vector2(Main.rand.NextFloat(-2, -2), Main.rand.NextFloat(2, 2));
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 113);
                }
              

            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Accessory.SuperMushroom>(), 3, 2));

        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/MushroomMiniBoss_Glow");

            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

        }

    }
}