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
    public class MeteorDropper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Meteor Bomber"); 
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffType<SuperBurnDebuff>()] = true;
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
            
            NPC.width = 30;
            NPC.height = 20;

            NPC.aiStyle = -1; 
            //aiType = NPCID.AngryNimbus;

            NPC.damage = 30;
            
            NPC.defense = 10;
            NPC.lifeMax = 60;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.knockBackResist = 0.5f;
            NPC.value = Item.buyPrice(0, 0, 1, 0);

           Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.MeteorDropperBannerItem>();
     
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("These advanced meteor heads can carry tiny burning meteor fragments and drop them onto unsuspecting players.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

 
                return SpawnCondition.Meteor.Chance * 0.35f;
            
            
        }
        int firerate = 0;
        bool firing;
        float ypos = -150;
        bool yassend;
        float xpos = 0;
        bool xassend;
        float speed = 3;
        float inertia = 30;
        public override void AI()
        {

            //Ypos Variance
            if (yassend)
            {
                ypos -= 0.1f;
            }
            else
            {
                ypos += 0.1f;
            }
            if (ypos <= -225f)
            {
                yassend = false;
            }
            if (ypos >= -200f)
            {
                yassend = true;
            }
            //Xpos Variance
            if (xassend)
            {
                xpos -= 0.1f;
            }
            else
            {
                xpos += 0.1f;
            }
            if (xpos <= -20f)
            {
                xassend = false;
            }
            if (xpos >= 20f)
            {
                xassend = true;
            }
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            NPC.rotation = NPC.velocity.X / 25;


            if (!player.dead)
            {
                Vector2 idlePosition = player.Center + new Vector2(0, ypos);
                Vector2 vectorToIdlePosition = idlePosition - NPC.Center;
                float distanceToIdlePosition = vectorToIdlePosition.Length();

                if (distanceToIdlePosition > 10f)
                {
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                }
                else if (NPC.velocity == Vector2.Zero)
                {
                    // If there is a case where it's not moving at all, give it a little "poke"
                    NPC.velocity.X = -0.15f;
                    NPC.velocity.Y = -0.05f;
                }

                NPC.velocity = (NPC.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            }

            if (player.dead)
            {
                NPC.velocity.Y = -5;

            }

            float distanceX = player.Center.X - NPC.Center.X;
            float distanceY = player.Center.Y - NPC.Center.Y;

            if ((distanceX <= 75f && distanceX >= -75f) && (distanceY <= 500f && distanceY >= 0f) && Collision.CanHitLine(NPC.Center, 0, 0, player.Center, 0, 0) && !player.dead)
            {
                int damage = 13; // The damage your projectile deals.
                float knockBack = 1;
                int type = ModContent.ProjectileType<NPCs.NPCProjs.MeteorDropperProj>();

                firerate++;
                float xprojpos = (Main.rand.NextFloat(-10, 10));
                if (firerate >= 25)
                {

                    //xpos = (Main.rand.NextFloat(-30, 30));
                    //ypos = (Main.rand.NextFloat(-180, -150));
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + xprojpos, NPC.Bottom.Y - 5), new Vector2(xprojpos / 15, 8), type, damage, knockBack);
                    }

                    SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                    firerate = 0;
                }

                firing = true;
                speed = 1.5f;

            }
            else
            {
                firing = false;
                speed = 3f;
            }
            if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
            {

                var dust2 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 6);
                //int dust2 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 72, projectile.velocity.X, projectile.velocity.Y, 130, default, 1.5f);
                dust2.noGravity = true;
                dust2.scale = 1.25f;
                dust2.velocity *= 2;

            }
        }
        int npcframe = 0;

        public override void FindFrame(int frameHeight)
        {

            if (!firing)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 5)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe >= 5) //Cycles through frames 0-5 when  in phase 1
                {
                    npcframe = 0;
                }
            }
            if (firing)
            {
                NPC.frame.Y = npcframe * frameHeight;
                NPC.frameCounter++;
                if (NPC.frameCounter > 5)
                {
                    npcframe++;
                    NPC.frameCounter = 0;
                }
                if (npcframe < 6 || npcframe >= 8) //Cycles through frames 6-7 when firing
                {
                    npcframe = 6;
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OnFire, 600);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            firerate = -30;
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }
            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X, NPC.Center.Y), 0, 0, 0);
                dust.scale = 0.8f;
            }
            if (NPC.life <= 0)          //this make so when the npc has 0 life(dead) he will spawn this
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MeteorDropperGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MeteorDropperGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MeteorDropperGore3").Type, 1f);

                for (int i = 0; i < 25; i++)
                {
                    var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6);
                    dust.scale = 1.5f;
                    dust.velocity *= 2f;
                }
              

            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

            npcLoot.Add(ItemDropRule.Common(ItemID.Meteorite, 10, 1, 2));


        }
       
        public override Color? GetAlpha(Color lightColor)
        {

            return Color.White;

        }

    }
}