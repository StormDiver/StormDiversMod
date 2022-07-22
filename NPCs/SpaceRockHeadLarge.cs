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
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;

namespace StormDiversMod.NPCs

{
    public class SpaceRockHeadLarge : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asteroid Charger"); // Automatic from .lang files
                                                 // make sure to set this for your modnpcs.
       
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            
            NPC.width = 40;
            NPC.height = 40;

            NPC.aiStyle = 74; 
            AIType = NPCID.SolarCorite;
            AnimationType = NPCID.FlyingSnake;
            NPC.noGravity = true;
            NPC.damage = 75;
            
            NPC.defense = 25;
            NPC.lifeMax = 900;
            

            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.knockBackResist = 0.3f;
            NPC.value = Item.buyPrice(0, 0, 25, 0);

            Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.SpaceRockHeadLargeBannerItem>();

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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A sentient asteroid boulder that is infused with strange extra-terrestrial energy, allowing it to charge towards whatever it chooses.")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //NPC.lifeMax = (int)(NPC.lifeMax * 0.75f);
            //NPC.damage = (int)(NPC.damage * 0.75f);
        }
        public override bool? CanFallThroughPlatforms()
        {         
                return true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (NPC.downedGolemBoss)
            {
                return SpawnCondition.Sky.Chance * 0.25f;
            }
            return SpawnCondition.Sky.Chance * 0f;
            
        }

        public override void AI()
        {

            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.OnFire3] = true;

            NPC.buffImmune[(BuffType<SuperBurnDebuff>())] = true;
            NPC.buffImmune[(BuffType<HellSoulFireDebuff>())] = true;
            NPC.buffImmune[(BuffType<UltraBurnDebuff>())] = true;
            NPC.buffImmune[BuffID.Confused] = true;


            Player player = Main.player[NPC.target];
            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
            NPC.velocity.X *= 0.97f;
            NPC.velocity.Y *= 0.97f;

            if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
            {

                var dust2 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 6);
                //int dust2 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 72, projectile.velocity.X, projectile.velocity.Y, 130, default, 1.5f);
                dust2.noGravity = true;
                dust2.scale = 1.5f;
                dust2.velocity *= 2;

            }
            if (Main.rand.Next(2) == 0)
            {

                int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 0, NPC.velocity.X, NPC.velocity.Y, 0, default, 0.5f);
            }
        }


        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
               
            
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 6);
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                for (int i = 0; i < 4; i++)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadLargeGore1").Type, 1f);
                }

                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadLargeGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadLargeGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadLargeGore4").Type, 1f);

               
                for (int i = 0; i < 30; i++)
                {
                     
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6);
                }


            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
            LeadingConditionRule isExpert = new LeadingConditionRule(new Conditions.IsExpert());

            isExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SpaceRock>(), 1, 1, 4));
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SpaceRock>(), 1, 1, 3));

            npcLoot.Add(notExpert);
            npcLoot.Add(isExpert);

        }
        public override Color? GetAlpha(Color lightColor)
        {

            return Color.White;

        }

    }
}