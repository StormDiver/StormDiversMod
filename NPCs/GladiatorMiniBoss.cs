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
    public class GladiatorMiniBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fallen Warrior"); // Automatic from .lang files
                                                 // make sure to set this for your modNPCs.
           
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            
            NPC.width = 28;
            NPC.height = 48;

            NPC.aiStyle = 22; 
            AIType = NPCID.Wraith;
            AnimationType = NPCID.FlyingSnake;

            NPC.damage = 30;
           
            NPC.defense = 10;
            NPC.lifeMax = 180;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.85f;
            NPC.value = Item.buyPrice(0, 0, 50, 0);
            NPC.rarity = 2;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.GladiatorMiniBossBannerItem>();

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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Once was a mighty warrior, now simply floats around the granite caves waiting for someone to haunt.")
            });
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            //NPC.lifeMax = (int)(NPC.lifeMax * 0.75f);
            //NPC.damage = (int)(NPC.damage * 0.75f);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (spawnInfo.marble && !NPC.AnyNPCs(ModContent.NPCType<GladiatorMiniBoss>()) && NPC.downedBoss1)
            {
                return SpawnCondition.Cavern.Chance * 0.2f;
            }
            else
            return SpawnCondition.Cavern.Chance * 0f;
    
            
        }
        int shoottime = 0;


        public override void AI()
        {
            shoottime++;

            Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.3f * Main.essScale);

            Player player = Main.player[NPC.target];
            Vector2 target = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));

            if (distance <= 800f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                
            {

                if (shoottime >= 300)
                {
                    float projectileSpeed = 1f; // The speed of your projectile (in pixels per second).
                    int damage = 15; // The damage your projectile deals. normal x2, expert x4
                    float knockBack = 1;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.GladiatorMiniBossProj>();

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;


                    SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 8);
                    
                    for (int i = 0; i < 3; i++)
                    {
                        float posX = NPC.position.X + Main.rand.NextFloat(60f, -60f);
                        float posY = NPC.position.Y + Main.rand.NextFloat(60f, -60f);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {

                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                            Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), new Vector2(posX, posY), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);

                        }
                    }
                    for (int i = 0; i < 20; i++)
                    {

                        var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 57);
                        dust.noGravity = true;
                        dust.velocity *= 3;
                        dust.scale = 2;

                    }
                    shoottime = 0;

                }
               
            }
            else
            {
                shoottime = 160;


            }


            if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
            {

                var dust2 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 57);
                //int dust2 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 72, projectile.velocity.X, projectile.velocity.Y, 130, default, 1.5f);
                dust2.noGravity = true;
                dust2.scale = 1f;
                

            }
            if (Main.rand.Next(2) == 0)
            {

                int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + NPC.height / 2), NPC.width, NPC.height / 2, 5, 0, 2, 150, default, 1f);
            }
        }


        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
               
            
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            shoottime = 160;
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            

            for (int i = 0; i < 3; i++)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(-2, -2), Main.rand.NextFloat(2, 2));
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, 5);
                dust.alpha = 150;
            }
            if (NPC.life <= 0)          //this make so when the NPC has 0 life(dead) he will spawn this
            {
                
                for (int i = 0; i < 10; i++)
                {
                    Vector2 vel = new Vector2(Main.rand.NextFloat(-2, -2), Main.rand.NextFloat(2, 2));
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5);
                    dust.alpha = 150;
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
        
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Accessory.GladiatorAccess>(), 3, 2));

            
        }
       
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;
        }
    }
}