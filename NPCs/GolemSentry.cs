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
    public class GolemSentry : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lihzahrd Flametrap");
            Main.npcFrameCount[NPC.type] = 4;


        }
        public override void SetDefaults()
        {
            
            NPC.width = 20;
            NPC.height = 66;

            //NPC.aiStyle = 22;

            //aiType = NPCID.Wraith;
            //animationType = NPCID.FlyingSnake;
            NPC.noTileCollide = false;

            NPC.damage = 50;
            NPC.lavaImmune = true;
            NPC.defense = 40;
            NPC.lifeMax = 1000;
            NPC.noGravity = false;
            
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 25, 0);
            NPC.gfxOffY = 0;
           Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.GolemSentryBannerItem>();

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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Simple flame turrets designed to protect the secrets of the temple.")
            });
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //NPC.lifeMax = (int)(NPC.lifeMax * 0.75f);
            //NPC.damage = (int)(NPC.damage * 0.75f);
        }
       
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (NPC.downedPlantBoss)
            {
                return SpawnCondition.JungleTemple.Chance * 0.35f;
            }
           
            else
            {
                return SpawnCondition.JungleTemple.Chance * 0f;
            }
        }
        int shoottime = 0;

        int animatespeed;

        public override void AI()
        {
            NPC.buffImmune[BuffID.Confused] = true;

            Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.6f * Main.essScale);

            //NPC.velocity.Y = 3;
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
           


            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));

           

            if (distance <= 700f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {

                shoottime++;

                if (shoottime >= 90)//starts the shooting animation
                {
                 


                    var dust2 = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Top.Y + 15), 10, 10, 6, 0, 0);
                    dust2.noGravity = true;
                    dust2.scale = 1.5f;
                    dust2.velocity *= 2;
                    animatespeed = 4;
                }
                else
                {
                    animatespeed = 10;
                }
                if (shoottime >= 120)//fires the projectiles
                {



                    float projectileSpeed = 7.5f; // The speed of your projectile (in pixels per second).
                    int damage = 30; // The damage your projectile deals. normal x2, expert x4
                    float knockBack = 3;
                    //int type = mod.ProjectileType("GolemMinionProj");
                    int type = ProjectileID.Fireball;

                    SoundEngine.PlaySound(SoundID.Item20, NPC.Center);

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;



                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {


                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(5));
                        float scale = 1f - (Main.rand.NextFloat() * .2f);
                        perturbedSpeed = perturbedSpeed * scale;
                        int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y - 13), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                        //Main.projectile[proj].timeLeft = 240;

                    }


                    shoottime = 0;

                }
            }
            else
            {
                shoottime = 45;
                animatespeed = 10;


            }
            if (Main.rand.Next(4) == 0)
            {
                var dust3 = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Top.Y + 15), 10, 10, 6, 0, 10);
                dust3.noGravity = true;
            }
        }
        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {

            NPC.frame.Y = npcframe * frameHeight;
            NPC.frameCounter++;
            if (NPC.frameCounter > animatespeed) //Animation speeds up when about to fire
            {
                npcframe++;
                NPC.frameCounter = 0;
            }
            if ( npcframe >= 4) 
            {
                npcframe = 0;
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

            for (int i = 0; i < 2; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 15), 10, 10, 25);
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) they will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemSentryGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemSentryGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemSentryGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemSentryGore4").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemSentryGore5").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GolemSentryGore5").Type, 1f);

                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6);
                    dust.scale = 2;
                    dust.velocity *= 2;
                }
                for (int i = 0; i < 30; i++)
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

            npcLoot.Add(ItemDropRule.Common(ItemID.LunarTabletFragment, 1, 2, 5));
            npcLoot.Add(ItemDropRule.Common(ItemID.LihzahrdPowerCell, 5));


        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/GolemSentry_Glow");
            Vector2 drawPos = new Vector2(0, 2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }

    }
}