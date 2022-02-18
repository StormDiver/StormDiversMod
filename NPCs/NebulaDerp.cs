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
    public class NebulaDerp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brain Hopper"); // Automatic from .lang files
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

            NPC.damage = 80;
            
            NPC.defense = 20;
            NPC.lifeMax = 1000;

       
            
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath18;
            NPC.knockBackResist = 0.5f;
            Item.buyPrice(0, 0, 0, 0);
           
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.NebulaDerpBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.NebulaPillar,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A Derpling that has been corrupted by Nebula energy, allowing it to possess a fraction of the Nebula Pillar’s power.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!GetInstance<Configurations>().PreventPillarEnemies)
            {
                return SpawnCondition.NebulaTower.Chance * 0.3f;
            }
            else
            {
                return SpawnCondition.NebulaTower.Chance * 0f;

            }
        }

   
        int firerate = 0;
        public override void AI()
        {
         

            Player player = Main.player[NPC.target];
            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
            
            if ((distanceX <= 100f && distanceX >= -100f) && (distanceY <= 0f && distanceY >= -500f) && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                //if (shoottime >= 120 )
                {
                   // float projectileSpeed = 5f; // The speed of your projectile (in pixels per second).
                    int damage = 25; // The damage your projectile deals.
                    float knockBack = 3;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.NebulaFlame>();
                    

                    firerate++;

                    if (firerate >= 5)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), new Vector2(NPC.Center.X, NPC.Top.Y + 10), new Vector2(0, -2.5f), type, damage, knockBack);
                        }
                        SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 34);
                        firerate = 0;

                        if (Main.rand.NextFloat() < 1f)
                        {
                            Dust dust;
                            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                            Vector2 position = NPC.position;
                            dust = Main.dust[Terraria.Dust.NewDust(position, NPC.width, NPC.height, 27, 0f, 0f, 0, new Color(255, 255, 255), 1.5f)];
                            dust.noGravity = true;
                        }
                    }

                   
                }
            }
     
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            firerate = -30;
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 2; i++)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(-2, -2), Main.rand.NextFloat(2, 2));
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, 112);
                dust.scale = 0.5f;
            }

            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("NebulaDerpGore1").Type, 1f);
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("NebulaDerpGore2").Type, 1f);
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("NebulaDerpGore3").Type, 1f);
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.Find<ModGore>("NebulaDerpGore4").Type, 1f);

                
                for (int i = 0; i < 10; i++)
                {
                    Vector2 vel = new Vector2(Main.rand.NextFloat(-2, -2), Main.rand.NextFloat(2, 2));
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 112);
                }
                if (NPC.ShieldStrengthTowerNebula > 0)
                {
                    Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(507));
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/NebulaDerp_Glow");

            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        
        }
        
       

    }

}
