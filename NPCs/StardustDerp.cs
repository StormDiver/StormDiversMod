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
    public class StardustDerp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Hopper"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 3; // make sure to set this for your modnpcs.
        }
        public override void SetDefaults()
        {
            NPC.width = 68;
            NPC.height = 72;

            NPC.aiStyle = 41; 
            AIType = NPCID.Derpling;
            AnimationType = NPCID.Derpling;
            NPC.alpha = 3;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.damage = 80;
            
            NPC.defense = 10;
            NPC.lifeMax = 800;
          
                NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath18;
            NPC.knockBackResist = 0.5f;
            Item.buyPrice(0, 0, 0, 0);

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.StardustDerpBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A Derpling that has been corrupted by Stardust energy, allowing it to possess a fraction of the Stardust Pillar’s power.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!GetInstance<ConfigurationsGlobal>().PreventPillarEnemies)
            {
                return SpawnCondition.StardustTower.Chance * 0.3f;
            }
            else
            {
                return SpawnCondition.StardustTower.Chance * 0f;

            }
        }

      
        
       
        int shoottime = 0;
        public override void AI()
        {
            shoottime++;
            

            Player player = Main.player[NPC.target];
            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));



            if (distance <= 600f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                if (shoottime >= 180 && NPC.velocity.Y == 0)
                {
                    
                    int type = ModContent.NPCType<StardustMiniDerp>();


                    SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        //NPC.NewNPC((int)Math.Round(NPC.Center.X), (int)Math.Round(NPC.Center.Y), type);
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)Math.Round(NPC.Center.X), (int)Math.Round(NPC.Center.Y), type);
                        for (int i = 0; i < 30; i++)
                        {
                             
                            var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 15, NPC.Center.Y - 15), 30, 30, 111);
                        }
                    }
 

                   

                    shoottime = 0;
                }
            }
            else
            {
                shoottime = 90;
            }
        }
        
        public override void HitEffect(int hitDirection, double damage)
        {
            shoottime = 90;
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, 111);
                dust.scale = 0.5f;
            }

            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StarDerpGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StarDerpGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StarDerpGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StarDerpGore4").Type, 1f);

              
                
                for (int i = 0; i < 50; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 111);
                }
                if (NPC.ShieldStrengthTowerStardust > 0)
                {
                    Projectile.NewProjectile(NPC.GetSource_Death(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(493));
                }
                
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/StardustDerp_Glow");

            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        
        } 

       

    }

}
