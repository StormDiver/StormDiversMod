using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using StormDiversMod.Basefiles;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent;
using Terraria.Enums;
using ReLogic.Content;
using StormDiversMod.Items.Weapons;
using Terraria.GameContent.ItemDropRules;

namespace StormDiversMod
{
	public class StormDiversMod : Mod //For most important things
	{
        public override void PostSetupContent()
        {
            //For boss checklist
            if (ModLoader.HasMod("BossChecklist"))//DON'T FORGET THIS!!!!!!!
            {
                Mod bossChecklist = ModLoader.GetMod("BossChecklist");
                if (bossChecklist != null)
                {
                    bossChecklist.Call
                        ("AddBoss", this, "Overloaded Scandrone", ModContent.NPCType<NPCs.Boss.StormBoss>(), 12f, (Func<bool>)(() => StormWorld.stormBossDown), () => true,
                        new List<int> { ModContent.ItemType<Items.BossTrophy.StormBossTrophy>(), ModContent.ItemType<Items.BossTrophy.StormBossRelic>(), ModContent.ItemType<Items.Pets.StormBossPetItem>(),
                        ModContent.ItemType<Items.BossTrophy.StormBossBag>(), ModContent.ItemType<Items.Accessory.StormCoil>(), //vanity
                        //other
                        //ModContent.ItemType<Items.Weapons.StormKnife>(), ModContent.ItemType<Items.Weapons.StormLauncher>(), ModContent.ItemType<Items.Weapons.StormStaff>(), ModContent.ItemType<Items.Weapons.StormSentryStaff>(),
                        ModContent.ItemType<Items.Vanitysets.BossMaskStormBoss>()}, //ModContent.ItemType<Items.Tools.StormHook>(), ModContent.ItemType<Items.Accessory.StormWings>(),ItemID.TempleKey},

                        ModContent.ItemType<Items.Summons.StormBossSummoner>(), "Summoned by using a Storm Beacon once all 3 mechs have been defeated",
                        "Overloaded Scandrone returns to its home planet (and didn't die on the way home)",
                        (SpriteBatch sb, Rectangle rect, Color color) =>
                        {
                            Texture2D texture = ModContent.Request<Texture2D>("StormDiversMod/NPCs/Boss/StormBoss_Image").Value;
                            Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                            sb.Draw(texture, centered, color);
                        }
                        );
                    // Additional bosses here

                    bossChecklist.Call
                       ("AddBoss", this, "Ancient Husk", ModContent.NPCType<NPCs.Boss.AridBoss>(), 6.5f, (Func<bool>)(() => StormWorld.aridBossDown), () => true,
                       new List<int> { ModContent.ItemType<Items.BossTrophy.AridBossTrophy>(), ModContent.ItemType<Items.BossTrophy.AridBossRelic>(), ModContent.ItemType<Items.Pets.AridBossPetItem>(),
                        ModContent.ItemType<Items.BossTrophy.AridBossBag>(), ModContent.ItemType<Items.Accessory.AridCore>(), //vanity
                       
                        ModContent.ItemType<Items.Vanitysets.BossMaskAridBoss>()},

                       ModContent.ItemType<Items.Summons.AridBossSummon>(), "Summoned by using a Cracked Horn at any time while in a desert",
                       "The Ancient Husk returns to the depths of the desert",
                       (SpriteBatch sb, Rectangle rect, Color color) =>
                       {
                           Texture2D texture = ModContent.Request<Texture2D>("StormDiversMod/NPCs/Boss/AridBoss_Image").Value;
                           Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                           sb.Draw(texture, centered, color);
                       }
                       );

                    /*bossChecklist.Call
                       ("AddBoss", this, "Ultimate Pain", ModContent.NPCType<NPCs.Boss.ThePainBoss>(), 19f, (Func<bool>)(() => StormWorld.painBossDown), () => true,
                       new List<int> { ModContent.ItemType<Items.BossTrophy.UltimateBossTrophy>(), ModContent.ItemType<Items.BossTrophy.UltimateBossRelic>(), ModContent.ItemType<Items.Pets.UltimateBossPetItem>(),
                        ModContent.ItemType<Items.BossTrophy.UltimateBossBag>(), ModContent.ItemType<Items.Accessory.DeathCore>(), //vanity
                       
                        ModContent.ItemType<Items.Vanitysets.ThePainMask>()},

                       ModContent.ItemType<Items.Vanitysets.ThePainMask>(), "Summoned by using Thepain post Moon Lord",
                       "You copuldn;t handle the pain huh?",
                       (SpriteBatch sb, Rectangle rect, Color color) => {
                           Texture2D texture = ModContent.Request<Texture2D>("StormDiversMod/NPCs/Boss/ThePainBoss_Image").Value;
                           Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                           sb.Draw(texture, centered, color);
                       }
                       );*/
                }
            }
            //4eum
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod))
            {
                thoriumMod.Call("AddMartianItemID", ModContent.ItemType<SuperDartLauncher>());
                thoriumMod.Call("AddFlailProjectileID", ModContent.ProjectileType<Projectiles.DestroyerFlailProj>());

            }
            //Bosses as NPCs
            if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs))
            {
                bossesAsNPCs.Call("AddToShop", "WithDiv", "Dreadnautilus", ModContent.ItemType<Items.Weapons.BloodySentry>(), () => true, 0.125f);
            }
        }
        public static ModKeybind ArmourSpecialHotkey;
        public override void Load()
        {
            ArmourSpecialHotkey = KeybindLoader.RegisterKeybind(this, "Armor Special Ability", "V");
            /*if (GetInstance<ConfigurationsGlobal>().NoDamageSpread)
            {
                //!!All credit goes to Kojo's mod called Rho's Playground!!
                On.Terraria.Main.DamageVar += (orig, damage, luck) => (int)Math.Round(damage * Main.rand.NextFloat(1, 1)); //No damage variance
            }*/

            //Wikithis
            ModLoader.TryGetMod("Wikithis", out Mod wikithis);
            if (wikithis != null && !Main.dedServ)
            {
                wikithis.Call("AddModURL", this, "terrariamods.wiki.gg$Storm's_Additions_Mod");
            }
        }
        public override void Unload()
        {
            ArmourSpecialHotkey = null;
        }
      
    }
  
    public class explosioneffects : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
           
        }
            /*if (projectile.type == ProjectileID.VampireKnife)
            {
                if (projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref projectile.velocity);
                    projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;
                float distance = 500f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
                    {
                        if (Collision.CanHit(projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                        {
                            Vector2 newMove = Main.npc[k].Center - projectile.Center;
                            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                            if (distanceTo < distance)
                            {
                                move = newMove;
                                distance = distanceTo;
                                target = true;
                            }
                        }
                    }
                }
                if (target)
                {
                    AdjustMagnitude(ref move);
                    projectile.velocity = (10 * projectile.velocity + move) / 10f;
                    AdjustMagnitude(ref projectile.velocity);
                }
            }
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 14f)
            {
                vector *= 14f / magnitude;
            }
        }*/
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.timeLeft == Projectile.SentryLifeTime)
            {
                projectile.timeLeft = 36000;
            }
            /*if (projectile.type == ProjectileID.ChlorophyteBullet)
            {
                projectile.penetrate = -1;
            }*/
        }
        public override void Kill(Projectile projectile, int timeLeft)
        {
            //Generic for Rocket, Grenade, Mine, and Snowman Cannon
            //Cluster frags x0.75, using frost colours
            if (projectile.type is ProjectileID.ClusterFragmentsI or ProjectileID.ClusterFragmentsII
                or ProjectileID.ClusterSnowmanFragmentsI or ProjectileID.ClusterSnowmanFragmentsII)
            {
                int proj = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionFrostProj>(), 0, 0, projectile.owner);
                Main.projectile[proj].scale = 0.75f;
            }
            //Cluster Rockets, use frost colours at 1.25x
            if (projectile.type is ProjectileID.ClusterRocketI or ProjectileID.ClusterRocketII
               or ProjectileID.ClusterGrenadeI or ProjectileID.ClusterGrenadeII
               or ProjectileID.ClusterMineI or ProjectileID.ClusterMineII
               or ProjectileID.ClusterSnowmanRocketI or ProjectileID.ClusterSnowmanRocketI)
            {
                int proj = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionFrostProj>(), 0, 0, projectile.owner);
                Main.projectile[proj].scale = 1.25f;
            }
            //Rocket Is and IIs, and Cluster Rockets x1.25
            if (projectile.type is ProjectileID.RocketI or ProjectileID.RocketII
                or ProjectileID.GrenadeI or ProjectileID.GrenadeII
                or ProjectileID.ProximityMineI or ProjectileID.ProximityMineI
                or ProjectileID.RocketSnowmanI or ProjectileID.RocketSnowmanII)
            {
                int proj = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionGenericProj>(), 0, 0, projectile.owner);
                Main.projectile[proj].scale = 1.25f;
            }
            //Rocket IIIs and IVs 1.5x
            if (projectile.type is ProjectileID.RocketIII or ProjectileID.RocketIV 
                or ProjectileID.GrenadeIII or ProjectileID.GrenadeIV 
                or ProjectileID.ProximityMineIII or ProjectileID.ProximityMineIV
                or ProjectileID.RocketSnowmanIII or ProjectileID.RocketSnowmanIV)
            {
                int proj = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionGenericProj>(), 0, 0, projectile.owner);
                Main.projectile[proj].scale = 1.5f;
            }
            //Mini Nukes x1.75
            if (projectile.type is ProjectileID.MiniNukeRocketI or ProjectileID.MiniNukeRocketII 
                or ProjectileID.MiniNukeGrenadeI or ProjectileID.MiniNukeGrenadeII 
                or ProjectileID.MiniNukeMineI or ProjectileID.MiniNukeMineII
                or ProjectileID.MiniNukeSnowmanRocketI or ProjectileID.MiniNukeSnowmanRocketII)
            {
                int proj = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionGenericProj>(), 0, 0, projectile.owner);
                Main.projectile[proj].scale = 1.75f;
            }

            base.Kill(projectile, timeLeft);
        }
        /*public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileID.TerrarianBeam)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 10;
            }
        }*/

    }
    public class treasurebagresearch : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[ItemID.CultistBossBag] = 3;

            base.SetStaticDefaults();
        }
    }
}