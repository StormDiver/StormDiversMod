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
using Terraria.DataStructures;

namespace StormDiversMod.NPCs

{
    public class ScanDrone : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("ScanDrone"); // Automatic from .lang files
                                                 // make sure to set this for your modnpcs.
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;

        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
            
            NPC.width = 40;
            NPC.height = 20;

            NPC.aiStyle = -1;
            //aiType = NPCID.GiantFlyingFox;
            //animationType = NPCID.CaveBat;

            NPC.damage = 50;
            
            NPC.defense = 15;
            NPC.lifeMax = 300;
            NPC.alpha = 3;
            

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.ScanDroneBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.VortexPillar,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A small but fast creature used to find weaknesses in enemy defences and to exploit them.")
            });
        }
      
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

        
            if (!GetInstance<ConfigurationsGlobal>().PreventPillarEnemies)
            {
                return SpawnCondition.VortexTower.Chance * 0.14f;
            }
            else
            {
                return SpawnCondition.VortexTower.Chance * 0f;
            }
        }
        int shoottime = 0;
        //private float rotation;
        //private float scale;
        bool shooting;
        float movespeed = 10f; //Speed of the npc

        float xpostion = 0; // The picked x postion
        float ypostion = 0f;
        public override void OnSpawn(IEntitySource source)
        {
            NPC.ai[1] = 90;
        }
        public override void AI()
        {
            NPC.buffImmune[BuffID.Confused] = true;

            Player player = Main.player[NPC.target]; //Code to move towards player
            NPC.TargetClosest();

            double deg = (NPC.ai[1]);
            double rad = deg * (Math.PI / 180);
            double dist = 100; //Distance away from the player

            //position
            xpostion = (int)(Math.Cos(rad) * dist);
            ypostion = (int)(Math.Sin(rad) * dist);

            Vector2 moveTo = player.Center;
            Vector2 move = moveTo - NPC.Center + new Vector2(xpostion, ypostion); //Postion around player
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > movespeed)
            {
                move *= movespeed / magnitude;
            }
            NPC.velocity = move;
            NPC.velocity.X *= 0.9f;

            //xpostion = 150 * -player.direction;
            if (player.direction == 1 && NPC.ai[1] <= 180)
            {
                NPC.ai[1] += 5;
            }
            else if (player.direction == -1 && NPC.ai[1] >= 0)
            {
                NPC.ai[1] -= 5;
            }

            shoottime++;
            NPC.noTileCollide = true;

            if (player.dead)
            {
                NPC.velocity.Y = -10;
            }
            if (player.position.X > NPC.position.X)
            {
                NPC.spriteDirection = 1;
                NPC.direction = 1;
            }
            else
            {
                NPC.spriteDirection = -1;
                NPC.direction = -1;

            }
            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;          
            
            if ((distanceX <= 600f && distanceX >= -600f) && (distanceY <= 200f && distanceY >= -200f))
            {
                if (shoottime >= 50)
                {
                    shooting = true;
                    //NPC.velocity.X = 0f;
                    //NPC.velocity.Y = 0f;


                    if (Main.rand.Next(3) == 0)
                    {
                        Dust dust;
                        // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                        dust = Terraria.Dust.NewDustPerfect(new Vector2(NPC.Center.X + 12 * NPC.direction, NPC.Center.Y), 156, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                    }
                }
                if (shoottime >= 60)
                {
                    float projectileSpeed = 8f; // The speed of your projectile (in pixels per second).
                    int damage = 25; // The damage your projectile deals.
                    float knockBack = 3;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.ScanDroneProj>();
                    //int type = ProjectileID.PinkLaser;

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;


                    SoundEngine.PlaySound(SoundID.Item17, NPC.Center);


                    //int numberProjectiles = 4 + Main.rand.Next(2); // 4 or 5 shots

                    for (int i = 0; i < 1; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10)); // 10 degree spread.
                                                                                                                                    
                            float scale = 1f - (Main.rand.NextFloat() * .3f);
                            perturbedSpeed = perturbedSpeed * scale;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + 10 * NPC.direction, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                        }
                    }
                    //xpostion *= -1;
                    shoottime = 0;
                    shooting = false;
                }
            }
            else
            {
                shoottime = 0;
                shooting = false;

            }
        }

        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            //NPC.spriteDirection = NPC.direction;
            if (shooting)
            {
                NPC.frame.Y = 4 * frameHeight; //Picks frame 4 when shooting
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
            
                target.AddBuff(ModContent.BuffType<Buffs.ScanDroneDebuff>(), 800);
            
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            shoottime = 0;
            shooting = false;
            NPC.velocity.X = 0f;
            NPC.velocity.Y = 0f;

            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 229);
                dust.scale = 0.5f;

            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("ScanDroneGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("ScanDroneGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("ScanDroneGore3").Type, 1f);
                
                for (int i = 0; i < 10; i++)
                {
                     
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 229);
                }
                //NPC.ShieldStrengthTowerVortex = (int)MathHelper.Clamp(NPC.ShieldStrengthTowerVortex - 1, 0f, NPC.ShieldStrengthTowerMax);
                if (NPC.ShieldStrengthTowerVortex > 0)
                {
                    Projectile.NewProjectile(NPC.GetSource_Death(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(422));
                }

            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)

        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/ScanDrone_Glow");
            Vector2 drawPos = new Vector2(0, 2) + NPC.Center - Main.screenPosition;

            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.instance.LoadProjectile(NPC.type);
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/ScanDrone");

            //Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + NPC.frame.Size() / 2f + new Vector2(0, 0);
                Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
    }
}