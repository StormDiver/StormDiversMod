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
    public class Dasher : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TestNPC"); // Automatic from .lang files
                                                 // make sure to set this for your modnpcs.
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true // Hides this NPC from the bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
        }
        public override void SetDefaults()
        {
            //Main.npcFrameCount[NPC.type] = 5;
            
            NPC.width = 32;
            NPC.height = 32;

            NPC.aiStyle = -1;
            //aiType = NPCID.GiantFlyingFox;
            //animationType = NPCID.CaveBat;
            NPC.noGravity = true;
            NPC.damage = 50;
            
            NPC.defense = 10;
            NPC.lifeMax = 250;
            
            NPC.gfxOffY = 10;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 10, 0);

            //Banner = NPC.type;
            //BannerItem = ModContent.ItemType<Banners.ScanDroneBannerItem>();
            /*NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };*/
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        /*public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.VortexPillar,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Test")
            });
        }*/
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //NPC.lifeMax = (int)(NPC.lifeMax * 0.75f);
            //NPC.damage = (int)(NPC.damage * 0.75f);
        }
       /* public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
   
            if (!NPC.AnyNPCs(ModContent.NPCType<Dasher>()) && Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].ZoneSnow && Main.hardMode)
            {
                return SpawnCondition.Overworld.Chance * 0.05f;
            }
            else
            {
                return SpawnCondition.Overworld.Chance * 0f;
            }
        }*/
        int shoottime = 0;
 
        //bool shooting;
        float movespeed = 1f; //Speed of the npc

        bool rotateright = false;

        bool charge = false;

        int posX = 0;
        int posY = -100;
        public override void AI()
        {
            NPC.buffImmune[BuffID.Confused] = true;

            //NPC.ai[0] = Rotation
            //NPC.ai[1] = Oribit time
            //NPC.ai[2] = Charge time
            //NPC.ai[3] = Phase?

            Player player = Main.player[NPC.target]; //Code to move towards player
                                                    


            NPC.TargetClosest(true);
            if (!charge)
            {
                Vector2 moveTo = player.Center;
                Vector2 move = moveTo - NPC.Center + new Vector2(posX, posY); //Postion around player
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > movespeed)
                {
                    move *= movespeed / magnitude;
                }
                NPC.velocity = move;
                NPC.velocity.X *= 0.9f;
            }
            shoottime++;
            NPC.noTileCollide = true;

           
            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));

            if (!player.dead)
            {
                if (!charge) //Not charging
                {
                    NPC.rotation = (player.MountedCenter - NPC.Center).ToRotation();
                    NPC.ai[2] = 0;
                    if (distance > 350) //Travelling towards player
                    {
                        NPC.ai[0] = 270; //Rotation

                        movespeed = 0.6f * distance / 50;

                        if (NPC.position.X < player.position.X)
                        {
                            rotateright = true; //Spins one way
                        }
                        else
                        {
                            rotateright = false; //Spins the other
                        }

                    }
                    else //Oribiting player
                    {
                        movespeed = 6.5f;
                        //Factors for calculations
                        double deg = (NPC.ai[0]);
                        double rad = deg * (Math.PI / 180);
                        double dist = 300; //Distance away from the player

                        //position
                        posX = (int)(Math.Cos(rad) * dist);
                        posY = (int)(Math.Sin(rad) * dist);

                        //angle
                        if (rotateright)
                        {
                            NPC.ai[0] += 1f;
                        }
                        else
                        {
                            NPC.ai[0] -= 1f;

                        }
                        NPC.ai[1]++;
                        if (NPC.ai[1] > (120 + Main.rand.Next(400)))
                        {
                            charge = true;
                        }
                    }
                }
                if (charge) //Charging
                {
                    NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 0f;



                    NPC.ai[1] = 0;

                    NPC.ai[2]++;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * 2;

                        if (NPC.ai[2] == 1)
                        {
                            NPC.velocity.X = velocity.X;
                            NPC.velocity.Y = velocity.Y;
                            NPC.ai[0] += 180; //New target is opposite where charged
                        }
                        if (NPC.ai[2] > 1 && NPC.ai[2] < 15)
                        {
                            NPC.velocity *= 1.15f;
                        }
                        if (NPC.ai[2] > 35)
                        {
                            NPC.velocity *= 0.96f;
                        }
                    }
                    if (NPC.ai[2] >= 120)
                    {
                        charge = false;
                    }
                }

            }
            else
            {
                if (NPC.velocity.Y > -10)
                {
                    NPC.velocity.Y -= 0.5f;
                }
                NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 0f;
                NPC.despawnEncouraged = true;
            }
            /*if ((distanceX <= 600f && distanceX >= -600f) && (distanceY <= 200f && distanceY >= -200f))
            {
                
                if (shoottime >= 60)
                {
                    float projectileSpeed = 8f; // The speed of your projectile (in pixels per second).
                    int damage = 25; // The damage your projectile deals.
                    float knockBack = 3;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.ScanDroneProj>();
                    //int type = ProjectileID.PinkLaser;

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;


                   // Projectile.NewProjectile(NPC.Center.X + NPC.width / 2, NPC.Center.Y + NPC.height / 2, velocity.X, velocity.Y, type, damage, knockBack, Main.myPlayer);
                    SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 17);


                    //int numberProjectiles = 4 + Main.rand.Next(2); // 4 or 5 shots

                    for (int i = 0; i < 1; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10)); // 30 degree spread.
                                                                                                                                    // If you want to randomize the speed to stagger the projectiles
                            float scale = 1f - (Main.rand.NextFloat() * .3f);
                            perturbedSpeed = perturbedSpeed * scale;
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                        }
                    }
                    //xpostion *= -1;
                    shoottime = 0;
                    shooting = false;
                }
            }
            else
            {
                shoottime = 0;
                shooting = false;

            }*/
        }

        /*int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            //NPC.spriteDirection = NPC.direction;
            if (shooting)
            {
                NPC.frame.Y = 4 * frameHeight; //Picks frame 4 when shooting
            }
            else
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 10)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe == 4) //Cycles through frames 0-3 when not casting
                {
                    npcframe = 0;
                }
            }

        }*/
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
                //target.AddBuff(ModContent.BuffType<Buffs.ScanDroneDebuff>(), 800);
            
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            /*shoottime = 0;
            shooting = false;
            NPC.velocity.X *= 0.5f;
            NPC.velocity.Y *= 0.5f;

            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 229);
                dust.scale = 0.5f;

            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("ScanDroneGore1").Type, 1f);
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("ScanDroneGore2").Type, 1f);
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("ScanDroneGore3").Type, 1f);
                
                for (int i = 0; i < 10; i++)
                {
                     
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 229);
                }
             

            }*/
        }
       /* public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/ScanDrone_Glow");
            Vector2 drawPos = new Vector2(0, 2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }*/
       

    }
}