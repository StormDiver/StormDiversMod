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
    public class FrozenSoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frozen Spirit");
            Main.npcFrameCount[NPC.type] = 8;

            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<SuperFrostBurn>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<UltraFrostDebuff>()] = true;
        }
        public override void SetDefaults()
        {
            NPC.coldDamage = true;

            NPC.width = 22;
            NPC.height = 45;

            NPC.aiStyle = 22;

            AIType = NPCID.Poltergeist;
            
            NPC.damage = 50;
            NPC.lavaImmune = true;
            NPC.defense = 15;
            NPC.lifeMax = 600;
            NPC.noGravity = true;
           
            NPC.noTileCollide = false;

            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.25f;
            NPC.value = Item.buyPrice(0, 0, 50, 0);

           Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.FrozenSoulBannerItem>();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { 
                Velocity = 0f // 
            };
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A lost spirit that has wandered into the ice biome and frozen over, making it unable to phase through solid walls. " +
                "During harsh blizzards it reveals itself, and begins to seek out and summon deadly cold attacks on any unfortunate player it encounters")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].ZoneSnow && Main.raining && Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<FrozenSoul>()))
            {
                return SpawnCondition.Overworld.Chance * 0.17f;
            }
            else
            {
                return SpawnCondition.Overworld.Chance * 0f;
            }

        }
        int shoottime = 0;

        bool shooting;
        public override void AI()
        {
            NPC.rotation = NPC.velocity.X / 15;

            shoottime++;

            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);
           
            bool lineofsight = Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height);
            NPC.velocity.X *= 0.95f;
            NPC.timeLeft = 60;
            if ((Vector2.Distance(player.Center, NPC.Center) <= 1000f && lineofsight))
            {
                if (shoottime >= 210)//warning
                {
                    shooting = true;

                    Vector2 perturbedSpeed = new Vector2(0, -1.5f).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(new Vector2(NPC.Center.X + (15 * NPC.direction), NPC.Center.Y), 0, 0, 111, perturbedSpeed.X, perturbedSpeed.Y, 0, default, 1.2f);
                    Main.dust[dust2].noGravity = true;
                }

                if (shoottime >= 240)//fires the projectiles
                {
                    float projectileSpeed = 7f; // The speed of your projectile (in pixels per second).
                    int damage = 25; // The damage your projectile deals.
                    float knockBack = 2;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.FrozenSoulProj>();

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(3));

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + (15 * NPC.direction), NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                    }
                    SoundEngine.PlaySound(SoundID.Item28, NPC.Center);

                    for (int i = 0; i < 35; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(0, -2).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(new Vector2(NPC.Center.X + (15 * NPC.direction), NPC.Center.Y), 0, 0, 111, perturbedSpeed.X, perturbedSpeed.Y, 0, default, 1.2f);
                        Main.dust[dust2].noGravity = true;
                    }
                    shoottime = 0;
                }
            }
            else
            {
                shoottime = 125;
            }
            if (shooting)
            {
                NPC.velocity *= 0.5f;

            }
            var dust3 = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.Bottom.Y - 6), NPC.width, 2, 135, 0, 5);
            dust3.noGravity = true;
        }
        int npcframe = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
            if (shooting)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 8)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe < 4) //Cycles through frames 4-7 once when firing
                {
                    npcframe = 4;
                }
                if (npcframe >= 8) //Reset when reached end of frames
                {
                    shooting = false;
                }
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
                if (npcframe >= 4) //Cycles through frames 0-3 when not casting
                {
                    npcframe = 0;
                }
            }

        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {

            target.AddBuff(ModContent.BuffType<Buffs.SuperFrostBurn>(), 300);


        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            //shoottime = 80;
            shooting = false;
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 135);
                dust.velocity *= 2;

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
                /*Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("FrozenSoulGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("FrozenSoulGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("FrozenSoulGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("FrozenSoulGore4").Type, 1f);*/

                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 135);
                    dust.velocity *= 2;
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
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Items.Weapons.SpiritStaff>(), 4, 3));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/FrozenSoul");
            Color color = new Color(175, 242, 243, 40);
            float scaleFactor13 = 0.5f + (NPC.GetAlpha(color).ToVector3() - new Vector3(0.5f)).Length() * 0.5f;
            for (int num149 = 0; num149 < 4; num149++)
            {
                spriteBatch.Draw(texture, NPC.position - screenPos + new Vector2((float)(NPC.width) * NPC.scale / 2f * NPC.scale, (float)(NPC.height) * NPC.scale / Main.npcFrameCount[NPC.type] * NPC.scale + 18) + NPC.velocity.RotatedBy((float)num149 * ((float)Math.PI / 2f)) * scaleFactor13, NPC.frame, new Microsoft.Xna.Framework.Color(64, 64, 64, 0), NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.LightBlue;
            color.A = 150;
            return color;

        }
    }
}