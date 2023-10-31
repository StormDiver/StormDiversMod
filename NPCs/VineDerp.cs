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
using Terraria.GameContent.ItemDropRules;


namespace StormDiversMod.NPCs

{
    public class VineDerp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Camouflaged Derpling"); 
            Main.npcFrameCount[NPC.type] = 24; 
        }
        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 64;

            NPC.aiStyle = 25; 
            AIType = NPCID.Mimic;
            AnimationType = NPCID.Mimic;

            NPC.damage = 80;
            
            NPC.defense = 25;
            NPC.lifeMax = 1500;


          
            NPC.HitSound = SoundID.NPCHit22;
            NPC.DeathSound = SoundID.NPCDeath25;
            NPC.knockBackResist = 0.3f;
            NPC.value = Item.buyPrice(0, 0, 50, 0);
            //NPC.rarity = 2;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.VineDerpBannerItem>();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("An elder Derpling that disguises itself as jungle terrain, ready to ambush nearby prey.")
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedPlantBoss && !NPC.AnyNPCs(ModContent.NPCType<VineDerp>()))
            { 
                return SpawnCondition.UndergroundJungle.Chance * 0.6f;
            }
            return SpawnCondition.UndergroundJungle.Chance * 0f;
        }
        public override void AI()
        {
           
        }


        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, 3);
               

            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("VineDerpGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("VineDerpGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("VineDerpGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("VineDerpGore4").Type, 1f);

                
                for (int i = 0; i < 30; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 3);
                    dust.scale = 1.5f;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.ChlorophyteOre, 1, 10, 16));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Pets.DerplingVine>(), 3, 2));
        }
       
    }
}