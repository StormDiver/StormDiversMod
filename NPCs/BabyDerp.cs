using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.NPCs.Banners;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;


namespace StormDiversMod.NPCs
{
    public class BabyDerp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Derpling"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 3; // make sure to set this for your modNPCs.
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 24;

            NPC.aiStyle = 41; 
            AIType = NPCID.Derpling;
            AnimationType = NPCID.Derpling;

            NPC.damage = 20;
            
            NPC.defense = 5;
            NPC.lifeMax = 100;
            
            //NPC.HitSound = SoundID.NPCHit22;
            //NPC.DeathSound = SoundID.NPCDeath25;
            NPC.knockBackResist = 1f;
            NPC.value = Item.buyPrice(0, 0, 0, 1);

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.BabyDerpBannerItem>();

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("It's a Baby Derpling, how cute.")
            });
        }
        
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f);
            NPC.damage = (int)(NPC.damage * 0.5f);

        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
            {

                return SpawnCondition.SurfaceJungle.Chance * 0.8f;
            }
            return SpawnCondition.SurfaceJungle.Chance * 0f;
        }
       
        public override void HitEffect(int hitDirection, double damage)
        {
            SoundEngine.PlaySound(SoundID.NPCHit22 with {Volume = 0.7f, Pitch = 0.4f}, NPC.Center);
            if (NPC.life <= 0)          //this make so when the NPC has 0 life(dead) he will spawn this
            {
                SoundEngine.PlaySound(SoundID.NPCDeath25 with{Volume = 0.7f, Pitch = 0.5f}, NPC.Center);

            }

            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 68);
                dust.scale = 0.5f;
            }

            if (NPC.life <= 0)          //this make so when the NPC has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("BabyDerpGore1").Type, 1f);   
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("BabyDerpGore2").Type, 1f);   
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("BabyDerpGore3").Type, 1f);   
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("BabyDerpGore4").Type, 1f);  
                for (int i = 0; i < 10; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 68);
                }
            }
        }
       
    }
}