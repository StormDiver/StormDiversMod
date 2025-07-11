using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.NPCs.Banners;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using StormDiversMod.Common;
using Terraria.GameContent.Drawing;

namespace StormDiversMod.NPCs

{
    public class DerpMimic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Perfectly Normal Derpling");
            Main.npcFrameCount[NPC.type] = 12;
            NPCID.Sets.CantTakeLunchMoney[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                //Hide = true // Hides this NPC from the bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
                 NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 8;
            //NPCID.Sets.ShimmerImmunity[NPC.type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.CanHitPastShimmer[NPC.type] = true;
            NPCID.Sets.CantTakeLunchMoney[NPC.type] = true;

            /*NPCDebuffImmunityData debuffData = new()
            {
                ImmuneToAllBuffsThatAreNotWhips = true,
                //ImmuneToWhips = true
            };*/
            NPCID.Sets.ImmuneToRegularBuffs[NPC.type] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 80;

            NPC.aiStyle = 0; 
            //aiType = NPCID.VortexSoldier;

            NPC.damage = 666;

            NPC.defense = 666;
            NPC.lifeMax = 666;
            NPC.noGravity = false;

            NPC.HitSound = SoundID.NPCHit6;
            NPC.DeathSound = SoundID.NPCDeath8;
            NPC.knockBackResist = 0f;
            Item.buyPrice(0, 0, 0, 0);
            NPC.gfxOffY = -2;
            NPC.shimmerMovementSpeed = 1;
            NPC.waterMovementSpeed = 1;
            NPC.lavaMovementSpeed = 1;
            NPC.honeyMovementSpeed = 1;
            NPC.lavaImmune = true;

            NPC.despawnEncouraged = false;            
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Rain,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A perfectly normal Derpling, nothing strange about it at all, you have nothing to worry ab-\nRUN!")
            });
        }
        /* public override float SpawnChance(NPCSpawnInfo spawnInfo)
         {

         }*/
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            if (Main.expertMode && !Main.masterMode)
            {
                NPC.lifeMax = (int)(NPC.lifeMax / 2);
                NPC.damage = (int)(NPC.damage / 2);
            }
            if (Main.masterMode)
            {
                NPC.lifeMax = (int)(NPC.lifeMax / 3);
                NPC.damage = (int)(NPC.damage / 3);
            }
        }
        
        int feartime; //Cooldown before ai starts
       
        bool death; //Victory animation

        bool jump; //If it has jumped

        float distancefear = 500; //Detection ranged

        bool attackmode; //Wheter it is chasing a player

        int dociletime; //cooldown until stops attacking

        float jumpheight; //Jumpy
        float moveatspeed; //How fast it runs
        int projdamage;
        bool onasphalt;
        Player player;
      
        public override bool? CanFallThroughPlatforms()
        {
            if (player.position.Y > NPC.Bottom.Y && Collision.CanHitLine(NPC.Center, 0, 0, player.Center, 0, 0) && attackmode) // fall through platforms is player is below
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public override void AI()
        {
            player = Main.player[NPC.target];
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = Vector2.Distance(player.Center, NPC.Center);
            feartime++;

            if (!player.dead && distance < 1750)
            {
                if (Main.GraveyardVisualIntensity < 1)
                {
                    Main.GraveyardVisualIntensity += 0.02f;
                }
            }
            else
            {
                if (Main.GraveyardVisualIntensity > 0)
                {
                    Main.GraveyardVisualIntensity -= 0.02f;
                }
            }

                //NPC.spriteDirection = NPC.direction;

            int xtilepos = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
            int ytilepos = (int)(NPC.Bottom.Y / 16) + 0;
            var tilePos = NPC.Bottom.ToTileCoordinates16();

            if (Framing.GetTileSafely(tilePos.X, tilePos.Y).TileType == TileID.Asphalt)//When on asphalt
            {
                onasphalt = true;
                moveatspeed = 15 + ((distanceX * distanceX) / 500); //speed increases the further away it is
                if ((NPC.velocity.X > 5 || NPC.velocity.X < -5) && NPC.velocity.Y == 0)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Bottom.Y - 5), NPC.width, 5, 54, 0, -3);
                        dust.scale = 1f;

                    }
                }
                if (moveatspeed > 20) //movement speed cap
                {
                    moveatspeed = 20;
                }
            }
            else
            {
                if (NPC.velocity.Y == 0)
                {
                    onasphalt = false;
                }
                moveatspeed = 10 + ((distanceX * distanceX) / 500); //speed icreases the further away it is

                if (moveatspeed > 10) //movement speed cap
                {
                    moveatspeed = 10;
                }
            }

            jumpheight = distanceY / 70;

            if (jumpheight < -25)//cap jump height
            {
                jumpheight = -25;
            }

            if (feartime > 120)
            {
                if (NPC.velocity.X > 0.1f)
                {
                    NPC.spriteDirection = 1;
                }
                if (NPC.velocity.X < -0.1f)
                {
                    NPC.spriteDirection = -1;

                }
                if (!player.dead)
                {
                    
                    if (distance <= distancefear && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                    {
                        if (!attackmode)
                        {
                            SoundEngine.PlaySound(SoundID.ScaryScream, NPC.Center);
                            //if (!GetInstance<ConfigurationsIndividual>().NoShake)
                            {
                                player.GetModPlayer<MiscFeatures>().screenshaker = true;
                            }
                        }
                        death = false;
                        distancefear = 1000f; //increase detection range once triggered
                        attackmode = true;
                        dociletime = 1200;
                    }
                    else //if cannot detect player begin docile cooldown, shoot projs too
                    {
                        dociletime--;
                    }
                    if (attackmode) //When targetting the player
                    {
                        if (Vector2.Distance(player.Center, NPC.Center) <= 1500f)
                        {
                            NPC.ai[3]++;

                            if ((NPC.ai[3] >= 120 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) && Vector2.Distance(player.Center, NPC.Center) >= 800f)
                                || (NPC.ai[3] >= 60 && !Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height)))
                            {
                                if (Main.masterMode)
                                    projdamage = 50; //300
                                else if (Main.expertMode && !Main.masterMode)
                                    projdamage = 75; //300
                                else
                                    projdamage = 150; //300

                                float knockBack = 1;
                                int type = ModContent.ProjectileType<NPCs.NPCProjs.DerpMimicProj>();

                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                        new Vector2(NPC.Center.X, NPC.Center.Y)) * 10;

                                SoundEngine.PlaySound(SoundID.Zombie87, NPC.Center);


                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + (5 * NPC.spriteDirection), NPC.Center.Y - 8), new Vector2(perturbedSpeed.X + player.velocity.X / 2, perturbedSpeed.Y + player.velocity.Y / 2), type, 333, knockBack);
                                }
                                for (int i = 0; i < 6; i++)
                                {
                                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningHit, new ParticleOrchestraSettings
                                    {
                                        PositionInWorld = new Vector2(NPC.Center.X, NPC.Center.Y),

                                    }, player.whoAmI);
                                }
                                for (int i = 0; i < 50; i++) //Black particles
                                {
                                    Vector2 perturbedSpeed = new Vector2(0, -8f).RotatedByRandom(MathHelper.ToRadians(360));

                                    var dust = Dust.NewDustDirect(NPC.Center, 0, 0, 109, perturbedSpeed.X, perturbedSpeed.Y);
                                    dust.noGravity = true;
                                    dust.scale = 2f;

                                }
                                NPC.ai[3] = 0;
                            }
                        }
                        if (NPC.velocity.Y == 0 || onasphalt)//on ground or shortly after jumping have full movement control
                        {

                            if (distanceX <= -20)
                            {
                                NPC.velocity.X = -moveatspeed + (player.velocity.X * 0.5f);
                            }
                            if (distanceX >= 20)
                            {
                                NPC.velocity.X = +moveatspeed + (player.velocity.X * 0.5f);
                            }
                            if (distanceX < 25 && distanceX > -25)
                            {
                                NPC.velocity.X *= 0.5f;
                            }
                        }
                        else //in air lower its speed control
                        {
                            if (distanceX <= -30)
                            {
                                NPC.velocity.X = -moveatspeed / 2 + (player.velocity.X * 0.5f);
                            }
                            if (distanceX >= 30)
                            {
                                NPC.velocity.X = +moveatspeed / 2 + (player.velocity.X * 0.5f);
                            }
                            if (distanceX < 40 && distanceX > -40)
                            {
                                NPC.velocity.X *= 0.5f;
                            }
                        }

                        if ((distanceX >= -50 && distanceX <= 50) && !jump && NPC.velocity.Y == 0 && player.position.Y + 40 < NPC.position.Y) //jump to attack player
                        {
                            NPC.velocity.Y = -12 + jumpheight;

                            jump = true;
                        }

                        if (!jump && NPC.velocity.Y == 0 && !Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) && player.position.Y - 10 < NPC.position.Y) //Jump if cannot detect player
                        {
                            NPC.velocity.Y = -15;
                            jump = true;
                        }

                        if (NPC.collideX && !jump && NPC.velocity.Y == 0 && (distanceX <= -50 || distanceX >= 50)) //Jump over obstacles in the way
                        {
                            NPC.velocity.Y = -12;
                            jump = true;
                        }

                    }
                }
                if (dociletime <= 0 && NPC.velocity.Y == 0) //After 5 seconds of not being in player range return to docile
                {
                    attackmode = false;
                    distancefear = 500; //reset orignal trigger range
                    npcframe = 0; //Stays on frame 0 if no fear
                    NPC.ai[3] = 0;
                }
                if (NPC.velocity.Y == 0)
                {
                    jump = false;
                }
                if (player.dead) //victory
                {
                    dociletime--;

                    if (NPC.velocity.Y == 0)
                    {
                        attackmode = false;
                        death = true;
                    }
                    NPC.velocity.X = 0;
                    distancefear = 500;
                    NPC.noTileCollide = false;

                }
            }
            else
            {
                if (NPC.velocity.Y < 5)
                {
                    NPC.velocity.Y += 0.5f;
                }
            }
        }
        int npcframe = 0;

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (target.dead)
            {
                SoundEngine.PlaySound(SoundID.ScaryScream with{Volume = 1, Pitch = -1f, MaxInstances = -1}, NPC.Center);
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (attackmode)
            {
                NPC.frame.Y = npcframe * frameHeight;
                if (NPC.velocity.Y == 0)
                {
                    if (NPC.frameCounter > 3)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }
                    if (npcframe <= 0 || npcframe >= 5) //Cycles through frames 1-4 when running on ground
                    {
                        npcframe = 1;
                    }
                }
                else if (NPC.velocity.Y != 0)
                {
                    if (NPC.frameCounter > 4)
                    {
                        npcframe++;
                        NPC.frameCounter = 0;
                    }
                    if (npcframe <= 4 || npcframe >= 9) //Cycles through frames 5-8 when in the air
                    {
                        npcframe = 5;
                    }
                }
            }
            if (death)
            {
                NPC.frame.Y = npcframe * frameHeight;
                if (NPC.frameCounter > 5)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe <= 8 || npcframe >= 12) //Cycles through frames 9-11 when player is dead (dance victory)
                {
                    npcframe = 9;
                }
            }
            if (!attackmode && !death)
            {
                NPC.frame.Y = npcframe * frameHeight;
                npcframe = 0; //Stays on frame 0 if no fear
            }

        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            /*if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }*/
            //attacking make it hostile
            if (!attackmode)
            {
                SoundEngine.PlaySound(SoundID.ScaryScream, NPC.Center);
            }
            feartime = 120; //ignore startup cooldown
            distancefear = 2000; //Grealty increase aggro range
            attackmode = true; //Enable attack mode
            dociletime = 500; //Reset docile time

            //shoottime = 100;
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 20), 20, 20, 5);
               

            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                SoundEngine.PlaySound(SoundID.ScaryScream with { Volume = 2, Pitch = -1f, MaxInstances = -1 }, NPC.Center);
                /* Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("VortCannonGore1"), 1f);   //make sure you put the right folder name where your gores is located and the right name of gores
                 Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("VortCannonGore2"), 1f);     
                 Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("VortCannonGore3"), 1f);
                 Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("VortCannonGore4"), 1f);
                 Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("VortCannonGore5"), 1f);
                */
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

                /*int type = ModContent.NPCType<DerpMimic>();
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)Math.Round(NPC.Center.X), (int)Math.Round(NPC.Center.Y), type);*/

                for (int i = 0; i < 100; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5);
                    dust.scale = 1.5f;
                    dust.velocity *= 2;
                }
               
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Furniture.DerplingTrophyItem>(), 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanitysets.UltimateFearMask>(), 1));

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/DerpMimic");

            //Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            if (jump)
            {
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + NPC.frame.Size() / 2f + new Vector2(-23, -19);
                    Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }

            Texture2D texture2 = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/DerpMimic");
            if (attackmode)
            {
                float speen1 = 9f + 3f * (float)Math.Cos((float)Math.PI * 2f * Main.GlobalTimeWrappedHourly);
                Vector2 spinningpoint = Vector2.UnitX * speen1;
                Color color2 = Color.Black * (speen1 / 12f) * 0.8f;
                color2.A /= 2;
                for (float speen2 = 0f; speen2 < (float)Math.PI * 2f; speen2 += (float)Math.PI / 2f)
                {
                    Vector2 finalpos = NPC.position + new Vector2(0, 20) + spinningpoint.RotatedBy(speen2);
                    spriteBatch.Draw(texture2, new Vector2(finalpos.X - screenPos.X + (float)(NPC.width / 2) * NPC.scale, finalpos.Y - screenPos.Y + (float)NPC.height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f * NPC.scale), NPC.frame, color2, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }
            return true;
        }
    }
}