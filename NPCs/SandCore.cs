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
    public class SandCore : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dune Blaster");
            Main.npcFrameCount[NPC.type] = 9;


        }
        public override void SetDefaults()
        {
            
            NPC.width = 40;
            NPC.height = 40;
            
            
            NPC.damage = 40;
            NPC.lavaImmune = true;
            NPC.defense = 12;
            NPC.lifeMax = 1000;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.aiStyle = -1;


            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.5f;
            NPC.value = Item.buyPrice(0, 1, 0, 0);

           Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.SandCoreBannerItem>();


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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A husk of hardened sand, capable of spewing out sand hot enough to melt anything it touches.")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //NPC.lifeMax = (int)(NPC.lifeMax * 0.75f);
            //NPC.damage = (int)(NPC.damage * 0.75f);
        }
        
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (!NPC.AnyNPCs(ModContent.NPCType<SandCore>()) && Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].ZoneUndergroundDesert && Main.hardMode)
            {
                {
                    return SpawnCondition.DesertCave.Chance * 0.015f;
                }
            }
            
            else
            {
                return SpawnCondition.DesertCave.Chance * 0f;
            }
        }
        bool attacking;
        int sounddelay;
        int ypos = -150;
        float distance;
        float speed = 3;
        float inertia = 50;
        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        public override void AI()
        {
            NPC.buffImmune[(BuffType<AridSandDebuff>())] = true;
            NPC.buffImmune[BuffID.Confused] = true;

            NPC.spriteDirection = NPC.direction;


            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            /*Vector2 moveTo = player.Center;
            Vector2 move = moveTo - NPC.Center + new Vector2(0, ypos);
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);

            if (magnitude > movespeed)
            {
                move *= movespeed / magnitude;
            }
            NPC.velocity = move;
            */
            distance = Vector2.Distance(player.Center, NPC.Center);
            if (!player.dead)
            {             
                Vector2 idlePosition = player.Center + new Vector2(0, ypos);
                Vector2 vectorToIdlePosition = idlePosition - NPC.Center;

                vectorToIdlePosition.Normalize();
                vectorToIdlePosition *= speed;

                NPC.velocity = (NPC.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                NPC.rotation = NPC.velocity.X / 50;
            }
            else
            {
                NPC.velocity.Y = -0.5f;
            }

            NPC.spriteDirection = NPC.direction;         
               
            //NPC.ai[2] = staggertime;

            int xtilepos = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
            int ytilepos = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
            Tile tile = Main.tile[xtilepos, ytilepos];
            if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) || (distance < 500 && !Main.tileSolid[tile.TileType]))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    speed = 2f;
                    NPC.noTileCollide = false;
                    ypos = -75;
                    NPC.netUpdate = true;

                }
            }         
            if (distance > 500 && !Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height)) //if far away pass through tiles
            { 
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.noTileCollide = true;
                    speed = 3f;
                    ypos = 0;
                    NPC.netUpdate = true;

                }
            }

            if (distance <= 350f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {             
                NPC.ai[0]++;//shootime          
                NPC.ai[1]++;//shootduration

                sounddelay ++;

                if (NPC.ai[0] >= 5 && !player.dead && NPC.ai[1] < 120)//fires the projectiles, once ever 5 frames for 2 seconds
                {
                    attacking = true;

                    int damage = 15;
                    float knockBack = 0;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.SandCoreProj>();

                    float projectileSpeed = 3f;
                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X + -player.velocity.X, player.Center.Y + -player.velocity.X) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(5));
                        float scale = 1f - (Main.rand.NextFloat() * .2f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                    }


                    NPC.ai[0] = 0;

                }

                if (sounddelay > 10 && NPC.ai[1] < 240)
                {
                    SoundEngine.PlaySound(SoundID.Item34 with{Volume = 1f, Pitch = 0.25f}, NPC.Center);
                    sounddelay = 0;
                }
                if (NPC.ai[1] > 120) //stop firing after 2 seconds
                {
                    attacking = false;
                    sounddelay = 0;
                    NPC.ai[0] = 0;

                }
                if (NPC.ai[1] > 300) // reset cycle after 3 seconds
                {
                    NPC.ai[1] = 0;
                }
            }
            else
            {
                NPC.ai[0] = 0;
                attacking = false;              
            }          
            if (Main.rand.Next(4) == 0) //Dust effects
            {
                var dust3 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Bottom.Y - 15), NPC.width, 20, 10, 0, 10);
                dust3.noGravity = true;
            }
      
        }
        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
           
            if (!attacking)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 5)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 5) //Cycles through frames 0-4 when not attacking
                {
                    npcframe = 0;
                }
            }
            if (attacking)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 3)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe < 5 || npcframe >= 9) //Cycles through frames 5-8 when attacking
                {
                    npcframe = 5;
                }
            }
        }
    
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffType<AridSandDebuff>(), 300);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            NPC.ai[0] -= 30;
            attacking = false;
            NPC.ai[1] = 0;
            sounddelay = 0;
            NPC.ai[2] = 0;


            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 2; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 129);
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) they will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SandCoreGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SandCoreGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SandCoreGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SandCoreGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SandCoreGore4").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SandCoreGore4").Type, 1f);

                for (int i = 0; i < 40; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 129);
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

            isExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.DesertOre>(), 1, 12, 18));
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.DesertOre>(), 1, 10, 15));

            npcLoot.Add(ItemDropRule.OneFromOptionsWithNumerator(3, 2, ModContent.ItemType<Items.Accessory.DesertJar>(), ItemID.AncientBattleArmorMaterial));

            npcLoot.Add(notExpert);
            npcLoot.Add(isExpert);


        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/SandCore_Glow");
            Vector2 drawPos = new Vector2(0, -2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }
    }
}