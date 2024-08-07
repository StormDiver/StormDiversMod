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
using StormDiversMod.Items.Weapons;

namespace StormDiversMod.NPCs

{
    public class HellSoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heartless Soul");
            Main.npcFrameCount[NPC.type] = 5;

            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<SuperBurnDebuff>()] = true;
        }
        public override void SetDefaults()
        {
            
            NPC.width = 22;
            NPC.height = 45;

            NPC.aiStyle = 22;

            AIType = NPCID.Wraith;
            //animationType = NPCID.FlyingSnake;
            
            NPC.damage = 35;
            NPC.lavaImmune = true;
            NPC.defense = 12;
            NPC.lifeMax = 350;
            NPC.noGravity = true;
           
            NPC.rarity = 1;

            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.4f;
            NPC.value = Item.buyPrice(0, 0, 25, 0);

           Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.HellSoulBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A soul that has spent so long in the depths of hell that it has lost all hope, and more importantly, its heart.")
            });
        }
        
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
           if (Main.remixWorld && NPC.downedBoss3 || !Main.remixWorld)
                return SpawnCondition.Underworld.Chance * 0.06f;
           else
                 return SpawnCondition.Underworld.Chance * 0f;

        }
        int shoottime = 0;

        bool casting;
        public override void AI()
        {           
            NPC.rotation = NPC.velocity.X / 15;

            shoottime++;

            Player player = Main.player[NPC.target];
           
            bool lineofsight = Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height);
            if ((Vector2.Distance(player.Center, NPC.Center) <= 1000f && lineofsight))
            {
                if (shoottime >= 200)//starts the casting animation
                {
                    //NPC.velocity.X = 0;
                    NPC.velocity.Y *= 0f;
                    NPC.velocity.X *= 0.8f;

                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Top.Y), NPC.width, 2, 6, 0, -5);
                    dust2.noGravity = true;
                    dust2.scale = 1.5f;

                    casting = true;
                }
                else
                {
                    casting = false;
                }
                if (shoottime >= 240)//fires the projectiles
                {
                    float projectileSpeed = 7f; // The speed of your projectile (in pixels per second).
                    int damage = 20; // The damage your projectile deals.
                    float knockBack = 3;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.HellSoulEnemyProj>();
                    SoundEngine.PlaySound(SoundID.Item8, NPC.Center);

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;

                    for (int i = 0; i < 3; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            float speedX = 0f;
                            float speedY = -4f;
                            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(70));
                            float scale = 1f - (Main.rand.NextFloat() * .2f);
                            perturbedSpeed = perturbedSpeed * scale;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                        }
                    }

                    shoottime = 0;
                }
            }
            else
            {
                shoottime = 140;
                casting = false;
            }
            var dust3 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Bottom.Y - 2), NPC.width, 2, 6, 0, 5);
            dust3.noGravity = true;
        }
        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
            if (casting)
            {
                NPC.frame.Y = 4 * frameHeight; //Picks frame 4 when casting
            }
            else
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 10)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe == 4) //Cycles through frames 0-3 when not casting
                {
                    npcframe = 0;
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Buffs.SuperBurnDebuff>(), 600);
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
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 5);
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                for (int i = 0; i < 2; i++)
                {
                    int goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                    goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + (float)(NPC.width / 2) - 24f, NPC.position.Y + (float)(NPC.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1f;
                }

                /*Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellSoulGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellSoulGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellSoulGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("HellSoulGore4").Type, 1f);*/

                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5);
                    dust.scale = 1f;
                }
                for (int i = 0; i < 30; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Materials.CrackedHeart>(), 2, 1));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<CrackedDagger>(), 20, 15));

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/HellSoul");
            Color color = new Color(251, 79, 3, 40);
            float scaleFactor13 = 0.5f + (NPC.GetAlpha(color).ToVector3() - new Vector3(0.5f)).Length() * 0.5f;
            for (int num149 = 0; num149 < 4; num149++)
            {
                spriteBatch.Draw(texture, NPC.position - screenPos + new Vector2((float)(NPC.width) * NPC.scale / 2f * NPC.scale, (float)(NPC.height) * NPC.scale / Main.npcFrameCount[NPC.type] * NPC.scale + 16) + NPC.velocity.RotatedBy((float)num149 * ((float)Math.PI / 2f)) * scaleFactor13, NPC.frame, new Microsoft.Xna.Framework.Color(64, 64, 64, 0), NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        /*public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/GraniteMiniBoss_Glow");
            Vector2 drawPos = new Vector2(0, 2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

        }*/
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }
    }
}