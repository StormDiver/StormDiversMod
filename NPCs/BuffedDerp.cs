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
using Terraria.Audio;
using StormDiversMod.Common;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;

namespace StormDiversMod.NPCs
{
    public class BuffDerps : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC npc)
        {
            if (!GetInstance<ConfigurationsGlobal>().PreventBuffedDerps)
            {
                if (npc.type == NPCID.Derpling && NPC.downedPlantBoss)
                {
                    npc.life = npc.lifeMax = 1000;
                    npc.damage = 100;
                    npc.defense = 40;
                    npc.knockBackResist = 0.1f;
                }
            }
            if (npc.type == NPCID.Derpling)
            {
                NPCID.Sets.ShimmerTransformToNPC[npc.type] = ModContent.NPCType<DerpMimic>();
            }
        }
        int shoottime = 0;
        //private float rotation;
        //private float scale;
        public override void AI(NPC npc)
        {
           
            if (!GetInstance<ConfigurationsGlobal>().PreventBuffedDerps)
            {
                if (npc.type == NPCID.Derpling && NPC.downedPlantBoss)
                {

                    if (npc.velocity.Y == 0)
                    {
                        shoottime++;
                    }
                    //npc.rotation = npc.velocity.X / 15;

                    Player player = Main.player[npc.target];                   

                    if (Vector2.Distance(player.Center, npc.Center) <= 800f && Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                    {
                        if (shoottime >= 90)
                        {
                            npc.velocity.X = 0;
                            npc.velocity.Y = 0;

                            if (Main.rand.Next(10) == 0)
                            {
                                var dust2 = Dust.NewDustDirect(new Vector2(npc.Center.X - 15, npc.Top.Y - 6), 30, 8, 68, 0, 0);

                            }
                        }
                        if (shoottime >= 120)
                        {
                            npc.velocity *= 0f;

                            float projectileSpeed = 13f; // The speed of your projectile (in pixels per second).
                            int damage = 40; // The damage your projectile deals.
                            float knockBack = 3;
                            int type = ModContent.ProjectileType<NPCs.NPCProjs.DerplingEnemyShellProj>();

                            Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) -
                            new Vector2(npc.Center.X, npc.Center.Y)) * projectileSpeed;

                            SoundEngine.PlaySound(SoundID.Item50, npc.Center);


                            int numberProjectiles = 2 + Main.rand.Next(3);

                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10)); // 30 degree spread.
                                                                                                                                            // If you want to randomize the speed to stagger the projectiles
                                    float scale = 1f - (Main.rand.NextFloat() * .3f);
                                    perturbedSpeed = perturbedSpeed * scale;
                                   Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X, npc.Top.Y - 6), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockBack);

                                }
                            }
                            for (int i = 0; i < 25; i++)
                            {
                                var dust3 = Dust.NewDustDirect(new Vector2(npc.Center.X - 15, npc.Top.Y - 6), 30, 8, 68, 0, 0);
                                dust3.velocity *= 3;
                            }
                            shoottime = 0;
                        }
                    }
                    else
                    {
                        shoottime = 45;

                    }
                }
            }
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if (!GetInstance<ConfigurationsGlobal>().PreventBuffedDerps)
            {
                if (npc.type == NPCID.Derpling)
                {
                    //shoottime = 45;
                }
            }
        }
    }
}