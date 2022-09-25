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
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;

namespace StormDiversMod.NPCs

{
    public class StardustMiniDerp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Hopper Minion"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 2; // make sure to set this for your modnpcs.
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;

        }
        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 24;

            NPC.aiStyle = 86; 
            AnimationType = NPCID.DemonEye;

            NPC.damage = 100;
            
            NPC.defense = 0;
            NPC.lifeMax = 100;
            NPC.noTileCollide = true;
               
        
            
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.knockBackResist = -0.1f;
            Item.buyPrice(0, 0, 0, 0);
            NPC.noGravity = true;

            //Banner = Item.BannerToNPC(ModContent.NPCType<StardustDerp>());
            //Banner = Item.NPCtoBanner(ModContent.NPCType<StardustDerp>());


            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            //Unlock automatically when main derpling is defeated
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPCType<StardustDerp>()], quickUnlock: true);

            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.StardustPillar,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Mini Star Hoppers summoned using Stardust energy, very unstable and disappear upon contact.")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //NPC.lifeMax = (int)(NPC.lifeMax * 0.75f);
            //NPC.damage = (int)(NPC.damage * 0.75f);
        }

       
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            NPC.life = 0;
            SoundEngine.PlaySound(SoundID.NPCDeath7, NPC.Center);
            for (int i = 0; i < 10; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X, NPC.Center.Y), 5, 5, 111);
            }
            
        }
        int dustnpc = 0;
        public override void AI()
        {
            NPC.buffImmune[BuffID.Confused] = true;

            dustnpc++;
            if (dustnpc >= 3)
            {
                //var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X, NPC.Center.Y), 5, 5, 111);
                int dust = Dust.NewDust(new Vector2(NPC.Center.X - 3, NPC.Center.Y - 3), 6, 6, 111);
                Main.dust[dust].velocity *= -2f;
                Main.dust[dust].noGravity = true;
                dustnpc = 0;
            }
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
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 111);
                dust.scale = 0.5f;
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StarDerpMiniGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StarDerpMiniGore2").Type, 1f);  
                
                for (int i = 0; i < 10; i++)
                {
                     
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 111);
                }
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/StardustMiniDerp_Glow");
            Vector2 drawPos = new Vector2(0, 2) + NPC.Center - Main.screenPosition;
            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

        }
       
    }
}