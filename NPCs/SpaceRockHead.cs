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
    public class SpaceRockHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Orbiter");
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<SuperBurnDebuff>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<HellSoulFireDebuff>()] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<UltraBurnDebuff>()] = true;
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            
            NPC.width = 34;
            NPC.height = 34;
            NPC.aiStyle = 14; 
            AIType = NPCID.FlyingSnake;
            AnimationType = NPCID.FlyingSnake;

            NPC.damage = 80;
            
            NPC.defense = 15;
            NPC.lifeMax = 750;
            NPC.noGravity = true;

            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.knockBackResist = 0.1f;
            NPC.value = Item.buyPrice(0, 0, 20, 0);
            NPC.gfxOffY = -2;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.SpaceRockHeadBannerItem>();
           
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A sentient asteroid boulder that is infused with strange extra-terrestrial energy, allowing it to telekinetically launch tiny fragments.")
            });
        }
      
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedGolemBoss)
            {
                return SpawnCondition.Sky.Chance * 0.4f;
            }
            return SpawnCondition.Sky.Chance * 0f;            
        }
        int shoottime = 0;

        int movetime;

        public override void AI()
        {
            shoottime++;
 
            Player player = Main.player[NPC.target];
            
            if (Vector2.Distance(player.Center, NPC.Center) <= 600f && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                if (shoottime >= 240)
                {
                    float projectileSpeed = 10f; // The speed of your projectile (in pixels per second).
                    int damage = 40; // The damage your projectile deals.
                    float knockBack = 3;
                    int type = ModContent.ProjectileType<NPCs.NPCProjs.SpaceHeadProj>();

                    Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                    new Vector2(NPC.Center.X, NPC.Center.Y)) * projectileSpeed;

                    SoundEngine.PlaySound(SoundID.Item13, NPC.Center);

                    for (int i = 0; i < 4; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(180)); // 30 degree spread.
                                                                                                                                     // If you want to randomize the speed to stagger the projectiles
                            float scale = 1f - (Main.rand.NextFloat() * .3f);
                            perturbedSpeed = perturbedSpeed * scale;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);
                        }
                    }                   
                    movetime = 30;
                    shoottime = 0;
                }
            }
            else
            {
                shoottime = 150;
            }
            if (movetime > 0)
            {
                NPC.velocity.X = 0;
                NPC.velocity.Y = 0;
                movetime--;
            }
            if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
            {

                var dust2 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 6);
                //int dust2 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 72, projectile.velocity.X, projectile.velocity.Y, 130, default, 1.5f);
                dust2.noGravity = true;
                dust2.scale = 1.5f;
                dust2.velocity *= 2;

            }
            if (Main.rand.Next(2) == 0)
            {

                int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 0, NPC.velocity.X, NPC.velocity.Y, 0, default, 0.5f);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
                                    
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
                 
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 5, NPC.Center.Y - 5), 10, 10, 6);
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadGore4").Type, 1f);

                for (int i = 0; i < 25; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6);
                }             
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
            LeadingConditionRule isExpert = new LeadingConditionRule(new Conditions.IsExpert());

            isExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SpaceRock>(), 1, 1, 4));
            notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SpaceRock>(), 1, 1, 3));

            npcLoot.Add(notExpert);
            npcLoot.Add(isExpert);

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/SpaceRockHead");
            Color color = new Color(179, 151, 238, 40);
            float scaleFactor13 = 0.5f + (NPC.GetAlpha(color).ToVector3() - new Vector3(0.5f)).Length() * 0.5f;
            for (int num149 = 0; num149 < 4; num149++)
            {
                spriteBatch.Draw(texture, NPC.position - screenPos + new Vector2((float)(NPC.width) * NPC.scale / 2f * NPC.scale, (float)(NPC.height) * NPC.scale / Main.npcFrameCount[NPC.type] + 4f * NPC.scale + 5) + NPC.velocity.RotatedBy((float)num149 * ((float)Math.PI / 2f)) * scaleFactor13, NPC.frame, new Microsoft.Xna.Framework.Color(64, 64, 64, 0), NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}