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
using StormDiversMod.Buffs;
using StormDiversMod.Items.Tools;
using StormDiversMod.Items.Weapons;
using System.Transactions;
using StormDiversMod.Items.Vanitysets;

namespace StormDiversMod.NPCs
{
    public class SnowmanPizza : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pizza Delivery Snowman"); // Automatic from .lang files
                                                      // make sure to set this for your modnpcs.
            Main.npcFrameCount[NPC.type] = 7;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<SuperFrostBurn>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<UltraFrostDebuff>()] = true;

            //NPCID.Sets.BelongsToInvasionFrostLegion[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.coldDamage = true;

            NPC.width = 30;
            NPC.height = 42;

            NPC.aiStyle = 38; 
            //AIType = NPCID.MisterStabby;
            
            NPC.damage = 40;
            
            NPC.defense = 15;
            NPC.lifeMax = 200;
            
            NPC.HitSound = SoundID.NPCHit11;
            NPC.DeathSound = SoundID.NPCDeath15;
            NPC.knockBackResist = 0.6f;
            NPC.value = Item.buyPrice(0, 0, 4, 0);

           Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.SnowmanPizzaBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("This poor snowman has had to deliver pizza in the worst of conditions all its life, now all it wants to deliver is pain.")
        });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].ZoneSnow && Main.raining && Main.hardMode)
            {
                return SpawnCondition.Overworld.Chance * 0.2f;
            }
            else
            {
                return SpawnCondition.Overworld.Chance * 0f;
            }

        }
        int shoottime = 0;

        bool summoning;
        public override void AI()
        {
            if (NPC.aiStyle == 38)//shoottime pauses when in air and not firing
            {
                NPC.spriteDirection = NPC.direction;
                shoottime++;
            }
            
            if (!Main.dedServ)
            {
                Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.3f * Main.essScale);
            }
            //Main.NewText("Shoottime is: " + shoottime, 204, 101, 22);

            Player player = Main.player[NPC.target];
           
            if (Vector2.Distance(player.Center, NPC.Center) <= 800f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                if (shoottime >= 210)
                {
                    if (player.Center.X < NPC.Center.X)
                        NPC.spriteDirection = -1;
                    else
                        NPC.spriteDirection = 1;

                    NPC.aiStyle = -1;
                    if (NPC.velocity.Y == 0) //wait until on the ground before attacking
                    {
                        NPC.knockBackResist = 0; //prevent innteruption
                        NPC.velocity.X *= 0.95f;
                        NPC.velocity.Y = 0;
                        summoning = true; //begins summon animation
                        shoottime++;
                    }
                }
                if (shoottime == 210 + 24) //3 animation frames into attack
                {
                    float projectileSpeed = 7f; // The speed of your projectile (in pixels per second).
                    int damage = 25; // The damage your projectile deals. normal x2, expert x4 (50, 100, 150)
                    float knockBack = 1;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.SnowmanPizzaProj>();

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;

                    SoundEngine.PlaySound(SoundID.Item7 with {  Volume = 2}, NPC.Center);

                    for (int i = 0; i < 1; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + (5 * NPC.spriteDirection), NPC.Center.Y - 8), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);

                        }
                    }
                }
            }
            else
            {
                NPC.aiStyle = 38;
                shoottime = 110;
                summoning = false;
                NPC.knockBackResist = 0.6f;
            }
        }

        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (summoning)
            {
                NPC.frame.Y = npcframe * frameHeight;
                if (NPC.frameCounter > 8) 
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe <= 2) //Cycles through frames 3-6 when summoning
                {
                    npcframe = 3;
                }
                if (npcframe >= 7) //back to normal
                {
                    NPC.knockBackResist = 0.6f;
                    shoottime = 0;
                    NPC.aiStyle = 38;
                    summoning = false;
                }
            }
            else
            {
                NPC.frame.Y = npcframe * frameHeight;
                if (NPC.frameCounter > 10)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 2) //Cycles through frames 0-2 when not
                {
                    npcframe = 0;
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            //shoottime = 70;
            //summoning = false;
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 76);
                dust.noGravity = true;
                dust.velocity *= 2;
                dust.scale = 1.25f;

            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                for (int i = 0; i < 100; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 76);
                    dust.noGravity = true;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<PizzaCutter>(), 12, 8));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.SnowGlobe, 12, 8));
            npcLoot.Add(ItemDropRule.Common(ItemID.SnowBlock, 1, 5, 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.Pizza, 5, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PizzaCap>(), 25, 1, 1));

        }
    }
    //________________________________________
    public class SnowmanBomb : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pizza Delivery Snowman"); // Automatic from .lang files
            // make sure to set this for your modnpcs.
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<SuperFrostBurn>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<UltraFrostDebuff>()] = true;

            NPCID.Sets.BelongsToInvasionFrostLegion[Type] = true;

            NPCID.Sets.CantTakeLunchMoney[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.coldDamage = true;

            NPC.width = 40;
            NPC.height = 42;

            NPC.aiStyle = 38;
            //AIType = NPCID.MisterStabby;

            NPC.damage = 50;

            NPC.defense = 15;
            NPC.lifeMax = 300;

            NPC.HitSound = SoundID.NPCHit11;
            NPC.DeathSound = SoundID.NPCDeath15;
            NPC.knockBackResist = 0.4f;
            NPC.value = Item.buyPrice(0, 0, 4, 0);

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.SnowmanBombBannerItem>();
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostLegion,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A mad snowman that rushes into battle carryin' a bomb, both a danger to unfortunate adventurers, but also its own kind!")
        });
        }

        /*public override float SpawnChance(NPCSpawnInfo spawnInfo) //spawn code in npc effects
        {
            if (Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].ZoneSnow && Main.raining && Main.hardMode)
            {
                return SpawnCondition.Overworld.Chance * 0.14f;
            }
            else
            {
                return SpawnCondition.Overworld.Chance * 0f;
            }
        }*/
        bool bombprimed;
        int bombtime;
        int bombtime2;
        public override void AI()
        {
           
           if (NPC.aiStyle == 38)
                NPC.spriteDirection = NPC.direction;

            if (!Main.dedServ)
            {
                Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 0.3f * Main.essScale);
            }
            //Main.NewText("Shoottime is: " + shoottime, 204, 101, 22);

            Player player = Main.player[NPC.target];

            if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height)) //after line of sight for 15 seconds explode
                bombtime2++;

            if ((Vector2.Distance(player.Center, NPC.Center) <= 200 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height)) || bombtime2 > 1200)
            {
                bombprimed = true;
            }

            if (bombprimed)
            {
                if (player.Center.X < NPC.Center.X)
                    NPC.spriteDirection = -1;
                else
                    NPC.spriteDirection = 1;

                NPC.aiStyle = -1;
                if (NPC.velocity.Y == 0) //wait until on the ground before attacking
                {
                    NPC.velocity.X *= 0.8f;
                    NPC.velocity.Y = 0;
                }
                bombtime++;
                if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                {
                    int dustIndex = Dust.NewDust(new Vector2((NPC.Center.X - 4) + (12 * NPC.spriteDirection), NPC.Center.Y - 20), 0, -6, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
                if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
                {
                    int dustIndex = Dust.NewDust(new Vector2((NPC.Center.X - 4) + (12 * NPC.spriteDirection), NPC.Center.Y - 20), 0, 0, 6, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].noGravity = true;
                }
                if (bombtime % 10 == 0)
                SoundEngine.PlaySound(SoundID.Item13 with { MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, NPC.Center);
            }
            if (bombtime > 150)
            {
                int damage = 60; // The damage your projectile deals. normal x2, expert x4 (120, 240, 360) (snowballs deal half)
                float knockBack = 1;
                int type = ModContent.ProjectileType<NPCs.NPCProjs.SnowmanExplosion>();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2((NPC.Center.X - 4) + (12 * NPC.spriteDirection), NPC.Center.Y - 10), new Vector2(0, 0), type, damage, knockBack);
                    Main.projectile[proj].timeLeft = 3;
                    Main.projectile[proj].hostile = true;
                }
                for (int i = 0; i < 50; i++) //Orange particles
                {
                    Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));

                    var dust = Dust.NewDustDirect(new Vector2((NPC.Center.X - 4) + (12 * NPC.spriteDirection), NPC.Center.Y - 10), 0, 0, 76, perturbedSpeed.X, perturbedSpeed.Y);
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                }
                NPC.life = 0;
            }
        }

        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (bombprimed)
            {
                NPC.frame.Y = npcframe * frameHeight;
                if (NPC.frameCounter > 6)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe <= 2 || npcframe >= 5) //Cycles through frames 3-4 when bomb
                {
                    npcframe = 3;
                }
            }
            else
            {
                NPC.frame.Y = npcframe * frameHeight;
                if (NPC.frameCounter > 10)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 2) //Cycles through frames 0-2 when not
                {
                    npcframe = 0;
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {

        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            //shoottime = 70;
            //summoning = false;
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 76);
                dust.noGravity = true;
                dust.velocity *= 2;
                dust.scale = 1.25f;

            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                for (int i = 0; i < 100; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 76);
                    dust.noGravity = true;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.SnowBlock, 1, 5, 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.Bomb, 1, 5, 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FlatCap>(), 50, 1, 1));
        }
    }
}