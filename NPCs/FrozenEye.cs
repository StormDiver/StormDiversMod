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
    public class FrozenEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen Eyefish"); 
                                                           
            //NPCID.Sets.DontDoHardmodeScaling[Type] = true;

        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
            
            NPC.width = 30;
            NPC.height = 30;

            NPC.aiStyle = 44; 
            AIType = NPCID.FlyingFish;
            
            NPC.damage = 20;
            
            NPC.defense = 8;
            NPC.lifeMax = 45;
            NPC.noGravity = true;


            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.knockBackResist = 0.7f;
            NPC.value = Item.buyPrice(0, 0, 10, 0);

            NPC.gfxOffY = -2;

           Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.FrozenEyeBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                 BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A Demon Eye fish hybrid that has frozen over due to the harsh blizzards of the snow biome.")
            });
        }
      
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].ZoneSnow && Main.raining)
            {
                return SpawnCondition.Overworld.Chance * 0.25f;
            }
            else
            {
                return SpawnCondition.Overworld.Chance * 0f;
            }

        }
        int shoottime = 0;
        bool shooting = false;

        public override void AI()
        {
            NPC.buffImmune[BuffID.Frostburn] = true;
            NPC.buffImmune[BuffID.Frostburn2] = true;

            NPC.buffImmune[(BuffType<Buffs.SuperFrostBurn>())] = true;
            NPC.buffImmune[(BuffType<Buffs.UltraFrostDebuff>())] = true;

            shoottime++;

            Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.3f * Main.essScale);


            Player player = Main.player[NPC.target];
            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));

            NPC.rotation = (player.MountedCenter - NPC.Center).ToRotation();


            if (distance  <= 600f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                
                if (shoottime >= 130 + Main.rand.Next(60))
                {
                    float projectileSpeed = 8f; // The speed of your projectile (in pixels per second).
                    int damage = 10; // The damage your projectile deals.
                    float knockBack = 1;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.FrozenEyeProj>();

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;


                    
                        
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(7));

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack, Main.myPlayer);
                        NPC.velocity.X = velocity.X * -0.2f;
                        NPC.velocity.Y = velocity.Y * -0.2f;
                        NPC.netUpdate = true;


                    }
                    SoundEngine.PlaySound(SoundID.Item30, NPC.Center);

                    for (int i = 0; i < 20; i++)
                    {

                        var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 135);
                        dust.noGravity = true;
                        dust.velocity *= 3;

                    }
                    shooting = true;
                    shoottime = 0;
                }
            }
            else
            {
                shoottime = 60;
            }
            if (Main.rand.Next (3)== 0)
            {
                var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 135);
                dust.scale = 0.5f;
            }
        }

        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            if (shooting)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 8)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe < 4) //Cycles through frames 4-7 once when firing
                {
                    npcframe = 4;
                }
                if (npcframe >= 8) //Reset when reached end of frames
                {
                    shooting = false;
                }
            }
            else
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 8)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 4) //Cycles through frames 0-3 when not firing
                {
                    npcframe = 0;
                }
            }

        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {

            target.AddBuff(BuffID.Chilled, 300);


        }
        public override void HitEffect(int hitDirection, double damage)
        {
            shoottime = 60;
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            

            for (int i = 0; i < 3; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 80);
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("FrozenFishGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("FrozenFishGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("FrozenFishGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("FrozenFishGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("FrozenFishGore4").Type, 1f);

              
                for (int i = 0; i < 15; i++)
                {
                     
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 80);
                }
              

            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

            npcLoot.Add(ItemDropRule.Common(ItemID.FrostDaggerfish, 1, 10, 20));


        }

       

    }
}