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
using StormDiversMod.Common;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace StormDiversMod.NPCs

{
    public class SolarDerp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blazling"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 3; // make sure to set this for your modnpcs.

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<SuperBurnDebuff>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<HellSoulFireDebuff>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<UltraBurnDebuff>()] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 68;
            NPC.height = 72;

            NPC.aiStyle = 41; 
            AIType = NPCID.Derpling;
            AnimationType = NPCID.Derpling;
            NPC.alpha = 3;

            NPC.damage = 120;
            
            NPC.defense = 25;
            NPC.lifeMax = 1200;


            
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath18;
            NPC.knockBackResist = 0.5f;
            Item.buyPrice(0, 0, 0, 0);
            
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.SolarDerpBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A Derpling that has been corrupted by Solar energy, allowing it to possess a fraction of the Solar Pillar�s power.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (!GetInstance<ConfigurationsGlobal>().PreventPillarEnemies)
            {
                return SpawnCondition.SolarTower.Chance * 0.2f;
            }
            else
            {
                return SpawnCondition.SolarTower.Chance * 0f;
            }
        }      
       
        int shoottime = 0;
        public override void AI()
        {
            if (Main.rand.NextFloat() < 0.8f)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = NPC.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, NPC.width, NPC.height, 127, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }

            shoottime++;
            
            Player player = Main.player[NPC.target];
          
            if (Vector2.Distance(player.Center, NPC.Center) <= 500f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))    
            {
                if (shoottime >= 180)
                {
                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, 244);
                    //dust2.noGravity = true;
                    if (shoottime >= 240 && NPC.velocity.Y == 0)
                    {
                        float projectileSpeed = 2f; // The speed of your projectile (in pixels per second).
                        int damage = 30; // The damage your projectile deals.
                        float knockBack = 3;
                        int type = ModContent.ProjectileType<NPCs.NPCProjs.SolarDerpProj>();


                        Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                        new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                Vector2 perturbedSpeed = new Vector2(0, -8).RotatedByRandom(MathHelper.ToRadians(45));

                                float scale = 1f - (Main.rand.NextFloat() * .3f);
                                perturbedSpeed = perturbedSpeed * scale;

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                            }
                        }
                        SoundEngine.PlaySound(SoundID.Item74, NPC.Center);

                        for (int i = 0; i < 10; i++)
                        {
                            var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X, NPC.Center.Y), 0, 0, 244);
                        }

                        shoottime = 0;
                    }
                }
            }
            else
            {
                shoottime = 60;
            }
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
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, 244);
                dust.scale = 0.5f;
            }

            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SolarDerpGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SolarDerpGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SolarDerpGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SolarDerpGore4").Type, 1f);
             

                for (int i = 0; i < 50; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 244);
                    dust.scale = 1.5f;
                }
                if (NPC.ShieldStrengthTowerSolar > 0)
                {
                    Projectile.NewProjectile(NPC.GetSource_Death(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(517));
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/SolarDerp_Glow");
            Color color = new Color(220, 40, 30, 40);
            float scaleFactor = 0.5f + (NPC.GetAlpha(color).ToVector3() - new Vector3(0.5f)).Length() * 0.5f;
            for (int i = 0; i < 4; i++)
            {
                spriteBatch.Draw(texture, NPC.position - screenPos + new Vector2((float)(NPC.width) * NPC.scale / 2f * NPC.scale, (float)(NPC.height) * NPC.scale / Main.npcFrameCount[NPC.type] + 4f * NPC.scale + 8) + NPC.velocity.RotatedBy((float)i * ((float)Math.PI / 2f)) * scaleFactor, NPC.frame, new Microsoft.Xna.Framework.Color(64, 64, 64, 0), NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            } 
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/SolarDerp_Glow");

            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);     
        }   
    }
}
