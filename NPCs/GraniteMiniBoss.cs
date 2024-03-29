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
    public class GraniteMiniBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Surged Granite Core"); 
                                                           
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            
            NPC.width = 38;
            NPC.height = 38;

            NPC.aiStyle = 5; 
            AIType = NPCID.Parrot;
            AnimationType = NPCID.FlyingSnake;
            
            NPC.damage = 25;
            
            NPC.defense = 12;
            NPC.lifeMax = 220;
            NPC.noGravity = true;
            NPC.rarity = 2;


            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath44;
            NPC.knockBackResist = 0.5f;
            NPC.value = Item.buyPrice(0, 0, 50, 0);
            NPC.gfxOffY = -2;
           Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.GraniteMiniBossBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A floating orb of pure granite energy that guards the granite biomes.")
            });
        }
    
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.remixWorld)
            {
                if (spawnInfo.Granite && !NPC.AnyNPCs(ModContent.NPCType<GraniteMiniBoss>()) && NPC.downedBoss1)
                {
                    return SpawnCondition.Cavern.Chance * 0.12f;
                }
                else
                    return SpawnCondition.Cavern.Chance * 0f;
            }
            else
            {
                if (spawnInfo.Granite && !NPC.AnyNPCs(ModContent.NPCType<GraniteMiniBoss>()) && NPC.downedBoss1)
                {
                    return SpawnCondition.Underground.Chance * 0.12f;
                }
                else
                    return SpawnCondition.Underground.Chance * 0f;
            }
        }
        int shoottime = 0;


        public override void AI()
        {
            shoottime++;
            if (!Main.dedServ)
            {
                Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.3f * Main.essScale);
            }

            Player player = Main.player[NPC.target];
          
            if (Vector2.Distance(player.Center, NPC.Center) <= 800f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                if (shoottime >= 100)
                {
                    if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                    {
                        var dust2 = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y-10), 20, 20, 229);
                        dust2.noGravity = true;
                        dust2.scale = 1.5f;

                    }
                    NPC.velocity.X = 0;
                    NPC.velocity.Y = 0;
                }
                if (shoottime >= 160)
                {
                    float projectileSpeed = 15f; // The speed of your projectile (in pixels per second).
                    int damage = 15; // The damage your projectile deals.
                    float knockBack = 1;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.GraniteMiniBossProj>();

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;

                    SoundEngine.PlaySound(SoundID.Item12, NPC.Center);

                    for (int i = 0; i < 1; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0)); 
                                                                                                                                  
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                        }
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 229);
                        dust.noGravity = true;
                        dust.velocity *= 3;
                    }

                    shoottime = 0;
                }
            }
            else
            {
                shoottime = 60;
            }

            if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                NPC.noTileCollide = false;
                NPC.velocity.X *= 0.96f;
                NPC.velocity.Y *= 0.96f;
            }
            else
            {
                NPC.noTileCollide = true;
            }
          
            if (Main.rand.Next(2) == 0)
            {
                int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 187, NPC.velocity.X, NPC.velocity.Y, 0, default, 1.2f);
                Main.dust[dust2].noGravity = true;

            }
        }


        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            
               
            
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
           // shoottime = 80;
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            

            for (int i = 0; i < 3; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 229);
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GraniteMiniBossGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GraniteMiniBossGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GraniteMiniBossGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GraniteMiniBossGore4").Type, 1f);

                for (int i = 0; i < 25; i++)
                {
                     
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 229);
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Accessory.GraniteCoreAccess>(), 3, 2));
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/GraniteMiniBoss_Glow");
            Vector2 drawPos = new Vector2(0, 2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

    }
}