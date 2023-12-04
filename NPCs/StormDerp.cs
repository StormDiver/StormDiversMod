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
    public class StormDerp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Storm Hopper"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 3; // make sure to set this for your modnpcs.
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
        }
        public override void SetDefaults()
        {
            NPC.width = 68;
            NPC.height = 72;

            NPC.aiStyle = 41; 
            AIType = NPCID.Derpling;
            AnimationType = NPCID.Derpling;
            NPC.alpha = 3;

            NPC.damage = 90;
            
            NPC.defense = 20;
            NPC.lifeMax = 1000;


            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath18;
            NPC.knockBackResist = 0.5f;
            Item.buyPrice(0, 0, 0, 0);

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.StormDerpBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.VortexPillar,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A Derpling that has been corrupted by Vortex energy, allowing it to possess a fraction of the Vortex Pillar’s power.")
            });
        }
       
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!GetInstance<ConfigurationsGlobal>().PreventPillarEnemies)
            {
                return SpawnCondition.VortexTower.Chance * 0.3f;
            }
            else
            {
                return SpawnCondition.VortexTower.Chance * 0f;
            }
        }

        int shoottime = 0;
        public override void AI()
        {
            shoottime++;        

            Player player = Main.player[NPC.target];
                   
            if (Vector2.Distance(player.Center, NPC.Center) <= 600f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                if (shoottime >= 120)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 20, NPC.position.Y + 10), 40, 1, 229);
                    dust.noGravity = true;
                    if (shoottime > 180 && NPC.velocity.Y == 0)
                    {
                        float projectileSpeed = 10f; // The speed of your projectile (in pixels per second).
                        int damage = 40; // The damage your projectile deals.
                        float knockBack = 3;
                        int type = ModContent.ProjectileType<NPCs.NPCProjs.StormDerpProj>();
                        SoundEngine.PlaySound(SoundID.Item17, NPC.Center);

                        Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                        new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;


                        for (int i = 0; i < 5; i++)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(30)); // 30 degree spread.
                                                                                                                                        // If you want to randomize the speed to stagger the projectiles
                                float scale = 1f - (Main.rand.NextFloat() * .3f);
                                perturbedSpeed = perturbedSpeed * scale;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + Main.rand.Next(-20, 20), NPC.Top.Y + 20), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                            }
                        }

                        shoottime = 0;
                    }
                }
            }
            else
            {
                shoottime = 90;
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
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, 229);
                dust.scale = 0.5f;
            }

            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StormDerpGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StormDerpGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StormDerpGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("StormDerpGore4").Type, 1f);


                for (int i = 0; i < 50; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 229);
                }
                if (NPC.ShieldStrengthTowerVortex > 0)
                {
                    Projectile.NewProjectile(NPC.GetSource_Death(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(422));
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/StormDerp_Glow");

            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.instance.LoadProjectile(NPC.type);
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/StormDerp");

            //Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + NPC.frame.Size() / 2f + new Vector2(-4, 0);
                Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
    }

}
