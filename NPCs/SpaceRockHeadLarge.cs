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
    public class SpaceRockHeadLarge : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Charger"); // Automatic from .lang files
                                                        // make sure to set this for your modnpcs.
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 7;
            
            NPC.width = 40;
            NPC.height = 40;

            NPC.aiStyle = 74; 
            AIType = NPCID.SolarCorite;
            //AnimationType = NPCID.FlyingSnake;
            NPC.noGravity = true;
            NPC.damage = 75;
            
            NPC.defense = 25;
            NPC.lifeMax = 900;
            

            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.knockBackResist = 0.3f;
            NPC.value = Item.buyPrice(0, 0, 25, 0);

            Banner = NPC.type;
           BannerItem = ModContent.ItemType<Banners.SpaceRockHeadLargeBannerItem>();

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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A sentient asteroid boulder that is infused with strange extra-terrestrial energy, allowing it to charge towards whatever it chooses.")
            });
        }
     
        public override bool? CanFallThroughPlatforms()
        {         
                return true;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (NPC.downedGolemBoss)
            {
                return SpawnCondition.Sky.Chance * 0.25f;
            }
            return SpawnCondition.Sky.Chance * 0f;
            
        }
        bool charging;
        bool playedsound;
        public override void AI()
        {

            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.OnFire3] = true;

            NPC.buffImmune[(BuffType<SuperBurnDebuff>())] = true;
            NPC.buffImmune[(BuffType<HellSoulFireDebuff>())] = true;
            NPC.buffImmune[(BuffType<UltraBurnDebuff>())] = true;
            NPC.buffImmune[BuffID.Confused] = true;

            Player player = Main.player[NPC.target];
           
            NPC.velocity.X *= 0.98f;
            NPC.velocity.Y *= 0.98f;
            if (NPC.velocity.X > 0.1f)
            {
                NPC.spriteDirection = -1;
            }
            if (NPC.velocity.X < -0.1f)
            {
                NPC.spriteDirection = 1;

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
            if (NPC.velocity.X > 5 || NPC.velocity.X < -5 || NPC.velocity.Y > 5 || NPC.velocity.Y < -5) //When moving fast frame 0
            {
                charging = true;
                if (!playedsound)
                {
                    SoundEngine.PlaySound(SoundID.Item45 with { Volume = 2f, Pitch = 0f }, NPC.Center);
                    playedsound = true;
                }
            }
            else
            {
                playedsound = false;
                charging = false;
                NPC.rotation = NPC.velocity.X / 10;
            }

        }
        int npcframe = 0;

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = npcframe * frameHeight;

            if (charging) 
            {
                if (NPC.frameCounter > 4)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe <= 3 ||npcframe >= 7) //Cycles through frames 4-6 when dashing
                {
                    npcframe = 4;
                }
            }
            else
            {
                if (NPC.frameCounter > 6)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 4) //Cycles through frames 0-3 when not dash
                {
                    npcframe = 0;
                }
            }
            NPC.frameCounter++;

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
                for (int i = 0; i < 4; i++)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadLargeGore1").Type, 1f);
                }

                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadLargeGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadLargeGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpaceRockHeadLargeGore4").Type, 1f);

               
                for (int i = 0; i < 30; i++)
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
        public override Color? GetAlpha(Color lightColor)
        {

            return Color.White;

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Main.instance.LoadProjectile(NPC.type);
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/SpaceRockHeadLarge");
            if (charging) //When moving fast, ie. charging, have trail
            {
                //Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + NPC.frame.Size() / 2f + new Vector2(-4, -6);
                    Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }
            Color color2 = new Color(179, 151, 238, 40);
            float scaleFactor13 = 0.5f + (NPC.GetAlpha(color2).ToVector3() - new Vector3(0.5f)).Length() * 0.5f;
            for (int num149 = 0; num149 < 4; num149++)
            {
                spriteBatch.Draw(texture, NPC.position - screenPos + new Vector2((float)(NPC.width) * NPC.scale / 2f * NPC.scale, (float)(NPC.height) * NPC.scale / Main.npcFrameCount[NPC.type] + 4f * NPC.scale + 9) + NPC.velocity.RotatedBy((float)num149 * ((float)Math.PI / 2f)) * scaleFactor13, NPC.frame, new Microsoft.Xna.Framework.Color(64, 64, 64, 0), NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);     
        }
    }
}