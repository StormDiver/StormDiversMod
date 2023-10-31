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
using StormDiversMod.Items.Vanitysets;

namespace StormDiversMod.NPCs

{
    public class ThePainSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pain Slime"); // Automatic from .lang files
                                                      // make sure to set this for your modnpcs.
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.DontDoHardmodeScaling[Type] = false;
            NPCID.Sets.ShimmerTransformToNPC[Type] = NPCID.ShimmerSlime;
            
        }
        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 20;
            
            NPC.aiStyle = 1; 
            AIType = NPCID.GreenSlime;
            AnimationType = NPCID.BlueSlime;
            
            NPC.damage = 15;
            
            NPC.defense = 6;
            NPC.lifeMax = 40;
                        
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.4f;
            NPC.value = Item.buyPrice(0, 0, 0, 50);
            NPC.alpha = 75;
            NPC.scale = 1.2f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.ThePainSlimeBannerItem>();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPC.rarity = 1;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A slime that is constantly in pain, and wants to share it with you.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Overworld.Chance * 0.03f;
        }
        int shoottime;
        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;

            Player player = Main.player[NPC.target];
            
            if (Vector2.Distance(player.Center, NPC.Center) <= 600f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                if (NPC.life < NPC.lifeMax || !Main.dayTime)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        shoottime++;
                    }
                    if (shoottime > 120)
                    {
                        NPC.velocity.X = 0;
                        NPC.velocity.Y = 0;

                        var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 119, NPC.velocity.X, NPC.velocity.Y);
                        dust.alpha = 100;
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                        if (shoottime >= 180)
                        {
                            SoundEngine.PlaySound(SoundID.NPCHit1 with { Volume = 1f }, NPC.Center);

                            for (int i = 0; i < 5; i++)
                            {
                                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y - 40) - new Vector2(NPC.Center.X, NPC.Center.Y)) * 10;
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.ThePainSlimeProj>(), 5, 1);
                            }
                            shoottime = 0;
                        }
                    }
                }
            }
            else
            {
                shoottime = 90;
            }
        }

        //int npcframe = 0;
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            //shoottime = 80;

            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 119, hit.HitDirection, -2);
                dust.alpha = 100;
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
               
                for (int i = 0; i < 35; i++)
                {
                     
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 119, hit.HitDirection, -2);
                    dust.alpha = 100;

                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.PinkGel, 1, 10, 25));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ThePainMask>(), 50, 1));
        }
    }
    //_________________________________________
    public class TheClaySlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Clay Slime"); // Automatic from .lang files
            // make sure to set this for your modnpcs.
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.DontDoHardmodeScaling[Type] = false;
            NPCID.Sets.ShimmerTransformToNPC[Type] = NPCID.ShimmerSlime;

        }
        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 20;

            NPC.aiStyle = 1;
            AIType = NPCID.GreenSlime;
            AnimationType = NPCID.BlueSlime;

            NPC.damage = 15;

            NPC.defense = 6;
            NPC.lifeMax = 40;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.4f;
            NPC.value = Item.buyPrice(0, 0, 0, 50);
            NPC.alpha = 10;
            NPC.scale = 1.2f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.TheClaySlimeBannerItem>();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPC.rarity = 1;

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A slime made of clay that judges everything it looks at.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Overworld.Chance * 0.03f;
        }
        int shoottime;

        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;

            Player player = Main.player[NPC.target];
            if (Vector2.Distance(player.Center, NPC.Center) <= 600f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                if (NPC.life < NPC.lifeMax || !Main.dayTime)
                {
                    if (NPC.velocity.Y == 0)
                    {
                        shoottime++;
                    }
                    if (shoottime > 120)
                    {
                        NPC.velocity.X = 0;
                        NPC.velocity.Y = 0;
                        var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 236, NPC.velocity.X, NPC.velocity.Y);
                        dust.alpha = 100;
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                        if (shoottime >= 180)
                        {
                            SoundEngine.PlaySound(SoundID.NPCHit1 with { Volume = 1f }, NPC.Center);

                            for (int i = 0; i < 5; i++)
                            {
                                Vector2 perturbedSpeed = new Vector2(0, -8).RotatedByRandom(MathHelper.ToRadians(65));

                                float scale = 1f - (Main.rand.NextFloat() * .3f);
                                perturbedSpeed = perturbedSpeed * scale;

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<NPCs.NPCProjs.TheClaySlimeProj>(), 5, 1);
                            }
                            shoottime = 0;
                        }
                    }
                }
            }
            else
            {
                shoottime = 60;
            }
        }

        //int npcframe = 0;

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {

        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            //shoottime = 80;
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 236, hit.HitDirection, -2);
                dust.alpha = 100;
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {

                for (int i = 0; i < 35; i++)
                {

                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 236, hit.HitDirection, -2);
                    dust.alpha = 100;

                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.ClayBlock, 1, 20, 45));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TheClaymanMask>(), 50, 1));
        }
    }
}