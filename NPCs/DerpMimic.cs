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


namespace StormDiversMod.NPCs

{
    public class DerpMimic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Perfectly Normal Derpling");
            Main.npcFrameCount[NPC.type] = 18;


            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true // Hides this NPC from the bestiary
            };
                 NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
            
         }
        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 80;

            //NPC.aiStyle = 3; 
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
            
        }
        
       
        /* public override float SpawnChance(NPCSpawnInfo spawnInfo)
         {

         }*/
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f);
            NPC.damage = (int)(NPC.damage * 0.5f);
            
        }
        
        int feartime; //Cooldown before ai starts

        bool fear;// running aniamtion 
       
        bool death; //Vctory animation

        bool jump; //If it has jumped

        float distancefear = 500; //Detection ranged

        bool attackmode; //Wheter it is chasing a player

        int dociletime; //cooldown until stops attacking

        float jumpheight; //Jumpy
        float moveatspeed; //How fast it runs
        public override void AI()
        {
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            Player player = Main.player[NPC.target];
            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
            feartime ++;
            NPC.spriteDirection = NPC.direction;

            int xtilepos = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
            int ytilepos = (int)(NPC.Bottom.Y / 16) + 0;
            var tilePos = NPC.Bottom.ToTileCoordinates16();



            if (Framing.GetTileSafely(tilePos.X, tilePos.Y).TileType == TileID.Asphalt)//When on asphalt 
            {
                moveatspeed = 15 + (distance / 50);
                if ((NPC.velocity.X > 5 || NPC.velocity.X < -5) && NPC.velocity.Y == 0)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Bottom.Y - 5), NPC.width, 5, 54, 0, -3);
                        dust.scale = 1f;

                    }
                }
                if (moveatspeed > 25)
                {
                    moveatspeed = 25;
                }
            }
            else
            {
                moveatspeed = 10 + (distance / 50);

                if (moveatspeed > 18)
                {
                    moveatspeed = 18;
                }
            }

            jumpheight = distanceY / 70;

            if (jumpheight < -20)
            {
                jumpheight = -20;
            }

            if (feartime > 120)
            {
                if (!player.dead)
                {
                    if (distance <= distancefear && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                    {
                        if (!attackmode)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 2);
                        }
                        death = false;
                        distancefear = 1000f; //increase detection range once triggered
                        attackmode = true;
                        dociletime = 300;


                    }
                    else //if cannot detect player begin docile cooldown
                    {
                      
                        dociletime--;
                    }
                    if (attackmode) //When taregting the player
                    {
                       
                        fear = true;


                        if (distanceX <= -5)
                        {
                            NPC.velocity.X = -moveatspeed;
                        }
                        if (distanceX >= 5)
                        {
                            NPC.velocity.X = +moveatspeed;
                        }
                        if (distanceX < 25 && distanceX > -25)
                        {
                            NPC.velocity.X *= 0.5f;
                        }

                        if ((distanceX >= -50 && distanceX <= 50) && !jump && NPC.velocity.Y == 0 && player.position.Y + 50 < NPC.position.Y) //jump to attack player
                        {
                            NPC.velocity.Y = -12 + jumpheight;
                            jump = true;
                        }
                        if (player.position.Y > NPC.Bottom.Y && Collision.CanHitLine(NPC.Center, 0, 0, player.Center, 0, 0)) // fall through platforms is player is below
                        {
                            NPC.noTileCollide = true;
                        }
                        else
                        {
                            NPC.noTileCollide = false;
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
                    fear = false;
                    distancefear = 500; //reset orignal trigger range
                    npcframe = 0; //Stays on frame 0 if no fear

                }
                if (NPC.velocity.Y == 0)
                {
                    jump = false;
                }
                if (player.dead) //victory
                {
                    dociletime--;

                    fear = false;
                    death = true;
                    NPC.velocity.X = 0;
                    distancefear = 500;
                    NPC.noTileCollide = false;

                }
            }
        }
        int npcframe = 0;
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (target.dead)
            {
                SoundEngine.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 2, 1, -1f);
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if (fear)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 2)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe <= 7 || npcframe >= 18) //Cycles through frames 8-17 when FEAR (frame 8 maeks it twitch)
                {
                    npcframe = 8;
                }
            }
            if (death)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 5)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe <= 0 || npcframe >= 9) //Cycles through frames 1-8 when DEAD
                {
                    npcframe = 1;
                }
            }
            if (!fear && !death)
            {
                NPC.frame.Y = npcframe * frameHeight;
                npcframe = 0; //Stays on frame 0 if no fear
            }

        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            //attacking make it hostile
            if (!attackmode)
            {
                SoundEngine.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 2);
            }
            feartime = 120; //ignore startup cooldown
            distancefear = 2000; //Grealty increase aggro range
            attackmode = true; //Enable attack mode
            dociletime = 300; //Reset docile time

            //shoottime = 100;
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 20), 20, 20, 5);
               

            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {

                /* Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("VortCannonGore1"), 1f);   //make sure you put the right folder name where your gores is located and the right name of gores
                 Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("VortCannonGore2"), 1f);     
                 Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("VortCannonGore3"), 1f);
                 Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("VortCannonGore4"), 1f);
                 Gore.NewGore(NPC.position, NPC.velocity, mod.GetGoreSlot("VortCannonGore5"), 1f);
                */


                for (int i = 0; i < 100; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5);
                    dust.scale = 1.5f;
                    dust.velocity *= 2;
                }
               
            }
        }
        

    }
}