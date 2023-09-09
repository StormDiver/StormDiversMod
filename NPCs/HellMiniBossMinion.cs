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
    public class HellMiniBossMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Soul Cauldron Servant"); 
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;

        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
            
            NPC.width = 26;
            NPC.height = 26;

            NPC.aiStyle = 14; 
            AIType = NPCID.IlluminantBat;
            //animationType = NPCID.CaveBat;

            NPC.damage = 40;
           
            NPC.defense = 10;
            NPC.lifeMax = 250;

            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.9f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            //Banner = Item.NPCtoBanner(ModContent.NPCType<HellMiniBoss>());
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            //Unlock automatically when main derpling is defeated
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPCType<HellMiniBoss>()], quickUnlock: true);
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("The strongest of souls can be separated from the main cauldron, to assist it in battle.")
            });
        }
        
        int shoottime = 0;
        //private float rotation;
        //private float scale;
        bool shooting;
        public override void AI()
        {
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.OnFire3] = true;
            NPC.buffImmune[(BuffType<SuperBurnDebuff>())] = true;
            NPC.buffImmune[(BuffType<HellSoulFireDebuff>())] = true;
            NPC.buffImmune[(BuffType<UltraBurnDebuff>())] = true;

            shoottime++;
            NPC.rotation = NPC.velocity.X / 100;


            Player player = Main.player[NPC.target];           
            
            if (Vector2.Distance(player.Center, NPC.Center) <= 800f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                if (shoottime >=  120)
                {
                  
                    shooting = true;
                    if (Main.rand.Next(5) == 0)
                    {
                        var dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Center.Y - 6), NPC.width, 4, 173, 0, -5);
                        dust2.noGravity = true;
                        dust2.scale = 1.5f;
                    }
                }
                if (shoottime >= 140)
                {
                    NPC.velocity *= 0f;
              
                    float projectileSpeed = 3.5f; // The speed of your projectile (in pixels per second).
                    int damage = 25; // The damage your projectile deals.
                    float knockBack = 3;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.HellMiniBossProj1>();
                    //int type = ProjectileID.PinkLaser;

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;


                    // Projectile.NewProjectile(NPC.Center.X + NPC.width / 2, NPC.Center.Y + NPC.height / 2, velocity.X, velocity.Y, type, damage, knockBack, Main.myPlayer);
                    SoundEngine.PlaySound(SoundID.Item8, NPC.Center);


                    //int numberProjectiles = 4 + Main.rand.Next(2); // 4 or 5 shots

                    for (int i = 0; i < 1; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10)); // 30 degree spread.
                                                                                                                                    // If you want to randomize the speed to stagger the projectiles
                            float scale = 1f - (Main.rand.NextFloat() * .3f);
                            perturbedSpeed = perturbedSpeed * scale;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                        }
                    }
                                      
                    shoottime = 0;
                    shooting = false;
                }
            }
            else
            {
                shoottime = 70;
                shooting = false;

            }
            if (Main.rand.Next(5) == 0)
            {

                var dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Top.Y), NPC.width, NPC.height / 2, 135, 0, -7);
                dust2.scale = 1f;
                dust2.noGravity = true;

            }
            if (Main.rand.Next(7) == 0)
            {

                var dust3 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Bottom.Y - 4), NPC.width, 6, 6, 0, 5);
                dust3.noGravity = false;
                dust3.scale = 0.8f;
            }
        }

        int npcframe = 0;
        int frametime;
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
      
            if (shooting)
            {
                frametime = 3;
            }
            else
            {
                frametime = 6;
            }

                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > frametime)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 3) //Cycles through frames 0-2
                {
                    npcframe = 0;
                }
            

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            
                target.AddBuff(ModContent.BuffType<Buffs.HellSoulFireDebuff>(), 180);
            
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            
            shoottime = 70;
            shooting = false;
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 173);
                dust.scale = 0.5f;

            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossMinionGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossMinionGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellMiniBossMinionGore3").Type, 1f);

                for (int i = 0; i < 10; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 173);

                    dust.noGravity = true;
                    dust.velocity *= 5;
                    dust.scale = 2f;
                }
                for (int i = 0; i < 10; i++)
                {

                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 135);
                    dust2.scale = 1.5f;
                    dust2.noGravity = true;

                }
                for (int i = 0; i < 10; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 31, 0f, 0f, 0, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {          
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Materials.SoulFire>(), 3, 2));          
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture2 = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/HellMiniBossMinion");

            float speen1 = 9f + 3f * (float)Math.Cos((float)Math.PI * 2f * Main.GlobalTimeWrappedHourly);
            Vector2 spinningpoint5 = Vector2.UnitX * speen1;
            Color color = Color.Purple * (speen1 / 12f) * 0.8f;
            color.A /= 3;
            for (float speen2 = 0f; speen2 < (float)Math.PI * 2f; speen2 += (float)Math.PI / 2f)
            {
                Vector2 finalpos = NPC.position + new Vector2(0, 0) + spinningpoint5.RotatedBy(speen2);
                spriteBatch.Draw(texture2, new Vector2(finalpos.X - screenPos.X + (float)(NPC.width / 2) * NPC.scale, finalpos.Y - screenPos.Y + (float)NPC.height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f * NPC.scale), NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/HellMiniBossMinion_Glow");
            Vector2 drawPos = new Vector2(0, -2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 200;
            return color;
        }
    }
}