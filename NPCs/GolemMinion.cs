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
    public class GolemMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Temple Guardian");
            Main.npcFrameCount[NPC.type] = 6;


        }
        public override void SetDefaults()
        {
            
            NPC.width = 34;
            NPC.height = 50;

            //NPC.aiStyle = 22;

            //aiType = NPCID.Wraith;
            //animationType = NPCID.FlyingSnake;
            
            NPC.damage = 60;
            NPC.lavaImmune = true;
            NPC.defense = 32;
            NPC.lifeMax = 2000;
            NPC.noGravity = true;
            NPC.rarity = 3;


            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 2, 0, 0);

           Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.GolemMinionBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Fiercely guards the temple to keep out anyone who shouldn’t enter.")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //NPC.lifeMax = (int)(NPC.lifeMax * 0.75f);
            //NPC.damage = (int)(NPC.damage * 0.75f);
        }
        
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (!NPC.downedPlantBoss)
            {
                //Summoning done in EquipmentEffects.cs
                return SpawnCondition.JungleTemple.Chance * 0f;
            }
            else if (NPC.downedPlantBoss && !NPC.AnyNPCs(ModContent.NPCType<GolemMinion>()))
            {
                return SpawnCondition.JungleTemple.Chance * 0.035f;
            }
            else
            {
                return SpawnCondition.JungleTemple.Chance * 0f;
            }
        }
        bool shooting;
     
        bool spawn = true;

        public override void AI()
        {
            //NPC.ai[0] = Xpos
            //NPC.ai[1] = Ypos
            // NPC.ai[2] = Shoottime
            if (spawn)
            {
                
                    NPC.ai[0] = 0;
                    NPC.ai[1] = -150;
                
            }
            NPC.buffImmune[BuffID.Confused] = true;

            NPC.noTileCollide = true;
            Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.6f * Main.essScale);

            NPC.spriteDirection = NPC.direction;


            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            if ((player.ZoneJungle && player.ZoneRockLayerHeight) || NPC.downedPlantBoss)
            {
                Vector2 moveTo = player.Center;
                Vector2 move = moveTo - NPC.Center + new Vector2(NPC.ai[0], NPC.ai[1]);
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                float movespeed = 5f; //Speed of the npc

                if (magnitude > movespeed)
                {
                    move *= movespeed / magnitude;
                }
                NPC.velocity = move;
            }
            NPC.rotation = NPC.velocity.X / 25;
            NPC.spriteDirection = NPC.direction;
            NPC.velocity.Y *= 0.96f;



            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));

            if (player.dead || ((!player.ZoneJungle || !player.ZoneRockLayerHeight) && !NPC.downedPlantBoss)) //Now flees if the player leaves the Underground Jungle pre plant
            {
                NPC.velocity.Y = 8;
            }

            if (distance <= 700f)
            {
                NPC.ai[2]++;

                if (NPC.ai[2] >= 60)//starts the shooting animation
                {
                    //NPC.velocity.X = 0;
                    NPC.velocity.Y *= 0.98f;
                    NPC.velocity.X *= 0.98f;


                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Top.Y + 10), 10, 8, 6, 0, 0);
                    dust2.noGravity = true;
                    dust2.scale = 1.5f;
                    dust2.velocity *= 2;
                    shooting = true;

                }
                else
                {
                    shooting = false;
                }
                if (NPC.ai[2] >= 80)//fires the projectiles
                {
                    spawn = false;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.ai[0] = Main.rand.NextFloat(150f, -150f);
                        NPC.ai[1] = Main.rand.NextFloat(-100f, -200f);
                        NPC.netUpdate = true;

                    }

                    float projectileSpeed = 10f; // The speed of your projectile (in pixels per second).
                    int damage = 35; // The damage your projectile deals. normal x2, expert x4
                    float knockBack = 3;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.GolemMinionProj>();
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {

                        Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                        new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;


                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(12));
                        float scale = 1f - (Main.rand.NextFloat() * .2f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                    }
                    SoundEngine.PlaySound(SoundID.Item33, NPC.Center);


                    NPC.ai[2] = 0;
                    shooting = false;

                }
            }
            else
            {
                NPC.ai[2] = 30;
                shooting = false;
                NPC.ai[0] = 0f;
                NPC.ai[1] = -200f;
            }
            if (Main.rand.Next(4) == 0)
            {
                var dust3 = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Bottom.Y - 10), 10, 20, 6, 0, 10);
                dust3.noGravity = true;
            }
        }
        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            if (shooting)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 4)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe < 4 || npcframe >= 6) //Cycles through frames 4-5 when about to fire
                {
                    npcframe = 4;
                }
            }
            else
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 6)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 4) //Cycles through frames 0-3 when not casting
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
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            for (int i = 0; i < 2; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 25);
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) they will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemMinionGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemMinionGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemMinionGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemMinionGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemMinionGore4").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemMinionGore4").Type, 1f);

                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6);
                    dust.scale = 2;
                    dust.velocity *= 2;
                }
                for (int i = 0; i < 30; i++)
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
            LeadingConditionRule plantDead = new LeadingConditionRule(new Conditions.DownedPlantera());

                plantDead.OnSuccess(ItemDropRule.Common(ItemID.LunarTabletFragment, 1, 5, 8));
                plantDead.OnSuccess(ItemDropRule.Common(ItemID.LihzahrdPowerCell, 1));
           
            npcLoot.Add(plantDead);

        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/GolemMinion_Glow");
            Vector2 drawPos = new Vector2(0, -2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }

    }
}