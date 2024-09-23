using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using StormDiversMod.Common;
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
using StormDiversMod.Items.Tools;

using Terraria.GameContent.ItemDropRules;
using StormDiversMod.NPCs.Boss;
using StormDiversMod.Items.Summons;
using System.Security.Policy;
using StormDiversMod.Items.Armour;
using StormDiversMod.NPCs.NPCProjs;
using Terraria.Achievements;
using StormDiversMod.Items.Vanitysets;
using StormDiversMod.NPCs;
using StormDiversMod.Items.Accessory;

namespace StormDiversMod
{
	public class StormDiversMod : Mod //For most important things
	{
        public static int TwilightRobe;

        public override void PostSetupContent()
        {
            //For boss checklist
            if (ModLoader.HasMod("BossChecklist"))//DON'T FORGET THIS!!!!!!!
            {
                Mod bossChecklist = ModLoader.GetMod("BossChecklist");
                if (bossChecklist != null)
                {
                    bossChecklist.Call
                       ("LogBoss",
                        this,
                        nameof(AridBoss),
                        6.5f,
                        () => StormWorld.aridBossDown,
                        ModContent.NPCType<NPCs.Boss.AridBoss>(),
                         new Dictionary<string, object>()
                         {
                             ["spawnItems"] = ModContent.ItemType<AridBossSummon>(),
                             ["collectibles"] = ItemID.MusicBoxBoss5,
                             ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                             {
                                 Texture2D texture = ModContent.Request<Texture2D>("StormDiversMod/NPCs/Boss/AridBoss_Image").Value;
                                 Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                                 sb.Draw(texture, centered, color);
                             }
                         }

                       );

                    bossChecklist.Call
                        ("LogBoss",
                        this,
                        nameof(StormBoss),
                        11.9f,
                        () => StormWorld.stormBossDown,
                        ModContent.NPCType<NPCs.Boss.StormBoss>(),
                        new Dictionary<string, object>()
                        {
                            ["spawnItems"] = ModContent.ItemType<StormBossSummoner>(),
                            ["collectibles"] = ItemID.MusicBoxBoss4,
                            ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                            {
                                Texture2D texture = ModContent.Request<Texture2D>("StormDiversMod/NPCs/Boss/StormBoss_Image").Value;
                                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                                sb.Draw(texture, centered, color);
                            }
                        }

                        );

                    bossChecklist.Call
                       ("LogBoss",
                        this,
                        nameof(TheUltimateBoss),
                        19f,
                        () => StormWorld.ultimateBossDown,
                        ModContent.NPCType<NPCs.Boss.TheUltimateBoss>(),
                         new Dictionary<string, object>()
                         {
                             ["spawnItems"] = ModContent.ItemType<UltimateBossSummoner>(),
                             ["collectibles"] = ItemID.MusicBoxLunarBoss,
                             ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                             {
                                 Texture2D texture = ModContent.Request<Texture2D>("StormDiversMod/NPCs/Boss/TheUltimateBoss_Image").Value;
                                 Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                                 sb.Draw(texture, centered, color);
                             }
                         }

                       );
                }
            }
            //4eum
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod))
            {
                thoriumMod.Call("AddMartianItemID", ModContent.ItemType<SuperDartLauncher>());
                thoriumMod.Call("AddFlailProjectileID", ModContent.ProjectileType<Projectiles.DestroyerFlailProj>());
                thoriumMod.Call("AddFlailProjectileID", ModContent.ProjectileType<Projectiles.FlailLockerProj>());

            }
            //Bosses as NPCs
            if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs))
            {
                bossesAsNPCs.Call("AddToShop", "WithDiv", "Dreadnautilus", ModContent.ItemType<Items.Weapons.BloodySentry>(), new List<Condition>(), 0.125f);
            }

            //for achievements
            if (ModLoader.TryGetMod("TMLAchievements", out Mod mod))
            {
                //mod.Call("AddAchievement", this, "TestAchievement", AchievementCategory.Collector, "StormDiversMod/Assets/Achievements/TestAchievement", null, false, false, 2.5f, new string[] { "Kill_" + ModContent.NPCType<AridBoss>() });
                //Boss kills
                mod.Call("AddAchievement", this, "AchievementAridBoss", AchievementCategory.Slayer, "StormDiversMod/Assets/Achievements/AchievementAridBoss",
                    "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Kill_" + ModContent.NPCType<AridBoss>() });
                mod.Call("AddAchievement", this, "AchievementStormBoss", AchievementCategory.Slayer, "StormDiversMod/Assets/Achievements/AchievementStormBoss",
                    "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Kill_" + ModContent.NPCType<StormBoss>() });
                mod.Call("AddAchievement", this, "AchievementUltimateBoss", AchievementCategory.Slayer, "StormDiversMod/Assets/Achievements/AchievementUltimateBoss",
                    "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Kill_" + ModContent.NPCType<TheUltimateBoss>() });

                //Misc kills
                mod.Call("AddAchievement", this, "AchievementNoPizza", AchievementCategory.Slayer, "StormDiversMod/Assets/Achievements/AchievementNoPizza",
                   "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Kill_" + ModContent.NPCType<SnowmanPizza>() });

                mod.Call("AddAchievement", this, "AchievementMoonling", AchievementCategory.Slayer, "StormDiversMod/Assets/Achievements/AchievementMoonling",
                  "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Kill_" + ModContent.NPCType<MoonDerp>() });

                mod.Call("AddAchievement", this, "AchievementScaryDerp", AchievementCategory.Slayer, "StormDiversMod/Assets/Achievements/AchievementScaryDerp",
                    "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Kill_" + ModContent.NPCType<DerpMimic>() });

                //Item crafts (just make collect)
                mod.Call("AddAchievement", this, "AchievementBiomeCore", AchievementCategory.Collector, "StormDiversMod/Assets/Achievements/AchievementBiomeCore",
                 "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Collect_" + ModContent.ItemType<BiomeCore>() });
                mod.Call("AddAchievement", this, "AchievementFastDrill", AchievementCategory.Collector, "StormDiversMod/Assets/Achievements/AchievementFastDrill",
                "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Collect_" + ModContent.ItemType<FastDrill2>() });
                mod.Call("AddAchievement", this, "AchievementEquinox", AchievementCategory.Collector, "StormDiversMod/Assets/Achievements/AchievementEquinox",
                    "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Collect_" + ModContent.ItemType<LightDarkSword>() });
                mod.Call("AddAchievement", this, "AchievementStoneCannon4", AchievementCategory.Collector, "StormDiversMod/Assets/Achievements/AchievementStoneCannon4",
                    "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Collect_" + ModContent.ItemType<StoneThrowerSuperLunar>() });
                //Item Collects
                mod.Call("AddAchievement", this, "AchievementThePain", AchievementCategory.Collector, "StormDiversMod/Assets/Achievements/AchievementThePain",
                    "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Collect_" + ModContent.ItemType<ThePainMask>() });
                mod.Call("AddAchievement", this, "AchievementClayman", AchievementCategory.Collector, "StormDiversMod/Assets/Achievements/AchievementClayman",
                    "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Collect_" + ModContent.ItemType<TheClaymanMask>() });

                mod.Call("AddAchievement", this, "AchievementDerpKing", AchievementCategory.Collector, "StormDiversMod/Assets/Achievements/AchievementDerpKing",
                   "StormDiversMod/Assets/Achievements/AchievementBorder", true, false, 2.5f, new string[] { "Collect_" + ModContent.ItemType<DerplingBMask>(), "Collect_" + ModContent.ItemType<DerplingBreastplate>(),
                       "Collect_" + ModContent.ItemType<DerplingGreaves>(), "Collect_" + ModContent.ItemType<DerplingSword>(), "Collect_" + ModContent.ItemType<DerplingGun>(),
                   "Collect_" + ModContent.ItemType<DerplingStaff>(), "Collect_" + ModContent.ItemType<DerplingMinion>(),
                       "Collect_" + ModContent.ItemType<DerplingDrill>() , "Collect_" + ModContent.ItemType<DerplingChainsaw>(), "Collect_" + ModContent.ItemType<DerplingJackhammer>(),
                       "Collect_" + ModContent.ItemType<FishingRodDerpling>(), "Collect_" + ModContent.ItemType<DerpHook>() , "Collect_" + ModContent.ItemType<DerpEye>()});

                mod.Call("AddAchievement", this, "AchievementLunarCosplay", AchievementCategory.Collector, "StormDiversMod/Assets/Achievements/AchievementLunarCosplay",
                    "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Collect_" + ModContent.ItemType<SelenianBMask>(), "Collect_" + ModContent.ItemType<SelenianBody>(), "Collect_" + ModContent.ItemType<SelenianLegs>(),
                    "Collect_" + ModContent.ItemType<StormDiverBMask>(), "Collect_" + ModContent.ItemType<StormDiverBody>(), "Collect_" + ModContent.ItemType<StormDiverLegs>(),
                    "Collect_" + ModContent.ItemType<PredictorBMask>(), "Collect_" + ModContent.ItemType<PredictorLegs>(), "Collect_" + ModContent.ItemType<PredictorLegs>(),
                    "Collect_" + ModContent.ItemType<StargazerBMask>(), "Collect_" + ModContent.ItemType<StargazerBody>(), "Collect_" + ModContent.ItemType<StargazerLegs>()});

                //Doing things

                mod.Call("AddAchievement", this, "AchievementGnomed", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementGnomed",
             "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Event_Gnomed" });

                mod.Call("AddAchievement", this, "AchievementPendants", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementPendants",
              "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Event_Pendants" });

                mod.Call("AddAchievement", this, "AchievementStompBounce", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementStompBounce",
               "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Event_StompBounce" });

                mod.Call("AddAchievement", this, "AchievementQuack", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementQuack",
               "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "ValueEvent_Quack_5" });

                mod.Call("AddAchievement", this, "AchievementHugBear", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementHugBear",
                 "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Event_HugBear" });

                mod.Call("AddAchievement", this, "AchievementHeartSteal", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementHeartSteal",
             "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Event_HeartSteal" });

                mod.Call("AddAchievement", this, "AchievementTwilight", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementTwilight",
                   "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Event_TwilightWarp" });

                mod.Call("AddAchievement", this, "AchievementSuperHeart", AchievementCategory.Collector, "StormDiversMod/Assets/Achievements/AchievementSuperHeart",
               "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Event_SuperHeart" });

                mod.Call("AddAchievement", this, "AchievementNineLives", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementNineLives",
            "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Event_NineLives" });

                mod.Call("AddAchievement", this, "AchievementSantanked", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementSantanked",
              "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Event_Santanked" });

                mod.Call("AddAchievement", this, "AchievementNoShield", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementNoShield",
                  "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Event_NoShield" });

                mod.Call("AddAchievement", this, "AchievementThePets", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementThePets",
                 "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "Event_ThePets" });

                mod.Call("AddAchievement", this, "AchievementWhack", AchievementCategory.Challenger, "StormDiversMod/Assets/Achievements/AchievementWhack",
             "StormDiversMod/Assets/Achievements/AchievementBorder", false, false, 2.5f, new string[] { "ValueEvent_Whack_100" });
                //place where activated
                /*if (ModLoader.TryGetMod("TMLAchievements", out Mod mod)) 
                {
                    mod.Call("Event", "ExampleEvent");
                }*/
            }
        }
      
        public static ModKeybind ArmourSpecialHotkey;

        public override void Load()
        {
            ArmourSpecialHotkey = KeybindLoader.RegisterKeybind(this, "Armor Special Ability", "V");

            //Wikithis
            ModLoader.TryGetMod("Wikithis", out Mod wikithis);
            if (wikithis != null && !Main.dedServ)
            {
                // wikithis.Call("AddModURL", this, "terrariamods.wiki.gg$Storm's_Additions_Mod");
                wikithis.Call(0, this, "https://terrariamods.wiki.gg/wiki/Storm's_Additions_Mod/{}");
            }

            if (!Main.dedServ)
            {
                TwilightRobe = EquipLoader.AddEquipTexture(this, "StormDiversMod/Items/Armour/NightsChainmail_Robe", EquipType.Front, GetModItem(ItemType<NightsChainmail>()));
            }
        }
        public override void Unload()
        {
            ArmourSpecialHotkey = null;
        }

    }
    public class Musicdisplay : ModSystem
    {
        public override void PostAddRecipes()
        {
            if (ModLoader.TryGetMod("MusicDisplay", out Mod display))
            {
                string author = "MicelTheGuy";

                display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "Assets/Music/AridBossMusic"), "Desert Fatigue", author, "The Possesed Ancient Armour Set");
                display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "Assets/Music/StormBossIntro"), "Dance of the Tempest (Intro)", author, "");
                display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "Assets/Music/StormBossMusic"), "Dance of the Tempest", author, "The Failed Experiment");
                display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "Assets/Music/UltimateBossMusic"), "False Fright", author, "The Pain Begins");
                display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "Assets/Music/UltimateBossMusic2"), "Raw Trama", author, "The Real Pain Begins");
                display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "Assets/Music/UltimatePainMusic"), "Brain Malfunction", author, "This is the TRUE Pain");
            }
        }
    }
    public class Miscprojeffects : GlobalProjectile //Unused
    {
        public override void AI(Projectile projectile)
        {
            /*if (projectile.type == ProjectileID.SnowBallHostile) //make Snow Balla projectile not grief
            {
                int projid = Projectile.NewProjectile(projectile.GetSource_FromThis(), new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(projectile.velocity.X, projectile.velocity.Y), 
                    ModContent.ProjectileType<SnowmanSnowball>(), projectile.damage, projectile.knockBack);
                Main.projectile[projid].ai[2] = 1;
                //projectile.Kill();
                projectile.active = false;
            }*/

            }
            /*if (projectile.type == ProjectileID.VampireKnife) //homing vampire knives
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
            /*if (projectile.type == ProjectileID.ChlorophyteBullet) //Infinite pierce Chlorophyte bullets
            {
                projectile.penetrate = -1;
            }*/
        }
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (projectile.type == ProjectileID.SnowBallHostile)
            {
                //return false;
            }
                //Generic for Rocket, Grenade, Mine, and Snowman Cannon
                //Cluster frags x0.75, using frost colours
                /*if (projectile.type is ProjectileID.ClusterFragmentsI or ProjectileID.ClusterFragmentsII
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
                }*/

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
}