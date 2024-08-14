using Microsoft.Xna.Framework;

using Terraria;

using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using StormDiversMod.Items.Weapons;

using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using StormDiversMod.Items.Ammo;
using StormDiversMod.Items.Accessory;
using StormDiversMod.Items.Tools;
using StormDiversMod.Items.Vanitysets;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using StormDiversMod.Projectiles;
using static Humanizer.In;
using static Terraria.ModLoader.PlayerDrawLayer;
using StormDiversMod.NPCs;
using Terraria.GameContent.Drawing;
using StormDiversMod.NPCs.Boss;
using System.Reflection.Metadata.Ecma335;
using Terraria.WorldBuilding;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using StormDiversMod.NPCs.NPCProjs;

namespace StormDiversMod.Basefiles
{
    public class NPCEffects : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        // npc.GetGlobalNPC<NPCEffects>().boulderDB = true; in debuff.cs
        // target.AddBuff(mod.BuffType("BoulderDebuff"), 1200)

        public bool lunarBoulderDB; //Lunar Bouldered

        public bool sandBurn; //Forbidden Burn 

        public bool beetled; //beetle Swarm

        public bool heartDebuff; //Stolen Heart
         
        public bool superFrost; //Glacial burn

        public bool superburnDebuff; //Blazing fire

        public bool hellSoulFire; //Soulburn
         
        public bool darknessDebuff; //Escence of darkness

        public bool ultraburnDebuff; //Ultra Burn

        public bool ultrafrostDebuff; //Ultra freeze

        public bool spookedDebuff; //Spooked

        public bool aridCoreDebuff; //Arid Aura

        public bool webDebuff; //Cobwebbed

        public bool painDebuff; //For painbosses final phase

        public bool BlizzardDebuff;


        //All this for a speen----------------------------------------------

        public bool derplaunched; //If the npc has been launched by the Derpling armour

        public float direction; //records the direction prior to speen

        public bool spun; //if enemy has been spun

        //For projectile immunity immune

        public int aridimmunetime; //prevent arid armour explosion from hitting triggered enemy

        public int bloodimmunetime; //prevent blood drop proj from damaging hit enemy

        public int forbiddenimmunetime; //Prevent forbidden sand from bitten targetted enemy

        //For Heart Emblem

        public bool heartStolen; //If the npc has dropped below 50% life

        //Whip tags

        public bool WhiptagWeb; //Spider Whip
        int webwhipcooldown;

        public bool WhiptagBlood; //Bloody Whip
        int bloodwhipcooldown;

        public bool WhiptagForbidden; //Forbidden Whip
        int forbiddenwhipcooldown;

        public bool WhiptagSpaceRock; //Asteroid Whip
        int spacerockwhipcooldown;


        public int arrowcooldown; //Cooldown for magic Arrow

        public int explosionNPCflame; //How long to have flames under the enemy feet after being launched

        public int bosssoulcooldown; //cooldown for whena boss can drop a soul again


        //------------------------------------------------------------------
        public override void ResetEffects(NPC npc)
        {
            lunarBoulderDB = false;
            sandBurn = false;
            beetled = false;
            heartDebuff = false;
            superFrost = false;
            superburnDebuff = false;
            hellSoulFire = false;
            //derplaunched = false;
            darknessDebuff = false;
            ultraburnDebuff = false;
            ultrafrostDebuff = false;
            spookedDebuff = false;
            aridCoreDebuff = false;
            webDebuff = false;
            painDebuff = false;
            BlizzardDebuff = false;

            WhiptagWeb = false;
            WhiptagBlood = false;
            WhiptagForbidden = false;
            WhiptagSpaceRock = false;
        }

        public override void SetStaticDefaults()
        {

        }
        public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<GladiatorMiniBoss>()] = 3;
            ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<GraniteMiniBoss>()] = 3;
            ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<MushroomMiniBoss>()] = 3;

            ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<HellSoul>()] = 3;

            ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<SnowmanPizza>()] = 2;
            ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<VineDerp>()] = 3;

        }
        public override void AI(NPC npc)
        {

            /*if (npc.type == NPCID.SnowmanGangsta && Main.invasionType == 0)
            {
                if (!Main.dedServ)
                {
                    NPC.GetNPCInvasionGroup(NPCID.SnowmanGangsta);
                   
                    
                    Main.newMusic = 0;

                    //Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/TheUltimateBossTheme");
                }

            }*/
            //Don't forget to remove
            //npc.dontTakeDamage = false;

            //Debuff immunities
            if (npc.boss || NPCID.Sets.ShouldBeCountedAsBoss[npc.type] == true)
            {
                npc.buffImmune[(BuffType<BeetleDebuff>())] = true;
                npc.buffImmune[(BuffType<BlizzardDebuff>())] = true;
                npc.buffImmune[(BuffType<WebDebuff>())] = true;

            }
            if (npc.buffImmune[BuffID.Frostburn] == true) //all enemies immune to frost burn are immune to the Glacial Burn and Ultra Freeze
            {
                npc.buffImmune[BuffType<SuperFrostBurn>()] = true; //Glacial Burn
                npc.buffImmune[BuffType<UltraFrostDebuff>()] = true; //Ultra Freeze
            }
            if (npc.buffImmune[BuffID.OnFire] == true) //all enemies immune to on fire are immune to the fire debuffs
            {
                npc.buffImmune[BuffType<SuperBurnDebuff>()] = true; //Blazing Fire
                npc.buffImmune[BuffType<HellSoulFireDebuff>()] = true; //Soul Fire
                npc.buffImmune[BuffType<UltraBurnDebuff>()] = true; //Ultra Burn
            }
            //All underground desert and sandstorm enemies are immune to Forbidden burn
            if (npc.type is NPCID.SandSlime or NPCID.Antlion or NPCID.WalkingAntlion or NPCID.GiantWalkingAntlion or NPCID.FlyingAntlion or NPCID.GiantFlyingAntlion or
                NPCID.LarvaeAntlion or NPCID.DesertBeast or NPCID.DesertScorpionWalk or NPCID.DesertScorpionWall or NPCID.DesertLamiaDark or NPCID.DesertLamiaLight or
                NPCID.DesertDjinn or NPCID.DesertGhoul or NPCID.DesertGhoulCorruption or NPCID.DesertGhoulCrimson or NPCID.DesertGhoulHallow or
                NPCID.TombCrawlerHead or NPCID.TombCrawlerBody or NPCID.TombCrawlerTail or NPCID.DuneSplicerHead or NPCID.DuneSplicerBody or NPCID.DuneSplicerTail or
                NPCID.SandElemental or NPCID.SandShark or NPCID.SandsharkCorrupt or NPCID.SandsharkCrimson or NPCID.SandsharkHallow or NPCID.Tumbleweed
                )
            {
                npc.buffImmune[BuffType<AridSandDebuff>()] = true;
            }
            
            //slowdown enemies
            if ((beetled) && (!npc.boss && NPCID.Sets.ShouldBeCountedAsBoss[npc.type] == false))
            {
                npc.velocity.X *= 0.92f;
                npc.velocity.Y *= 0.92f;
            }
            if ((BlizzardDebuff) && (!npc.boss && NPCID.Sets.ShouldBeCountedAsBoss[npc.type] == false))
            {
                npc.velocity.X *= 0.92f;
                npc.velocity.Y *= 0.92f;
            }
            if (webDebuff && (!npc.boss && NPCID.Sets.ShouldBeCountedAsBoss[npc.type] == false))
            {
                npc.velocity.X *= 0.92f;

            }
            if (ultrafrostDebuff && (!npc.boss && NPCID.Sets.ShouldBeCountedAsBoss[npc.type] == false))
            {
                npc.velocity.X *= 0.93f;
            }
            
            //summon projectiles for shield killer
            var player = Main.LocalPlayer;         

            if (player.HeldItem.type == ModContent.ItemType<Items.Tools.ShieldKiller>() && player.itemAnimation == player.itemAnimationMax - 1)
            {

                if (npc.type == NPCID.LunarTowerVortex)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(422));
                }
                if (npc.type == NPCID.LunarTowerSolar)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(517));
                }
                if (npc.type == NPCID.LunarTowerNebula)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(507));
                }
                if (npc.type == NPCID.LunarTowerStardust)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(493));
                }

            }
            //speen________________________________________________

            if (derplaunched)
            {
                if ((npc.aiStyle is 1 or 3 or 8 or 25 or 26 or 38 or 39 or 41 or 42 || npc.type == ModContent.NPCType<SuperPainDummy>()) && npc.knockBackResist != 0)
                {
                    if (!spun)
                    {
                        if (npc.velocity.X > 0)
                        {
                            direction = 1;
                        }
                        else
                        {
                            direction = -1;
                        }
                        spun = true;
                    }
                    npc.rotation += (0.2f * direction); //Speen speed and direction

                    if (npc.velocity.Y == 0)
                    {
                        npc.rotation = 0; //reset the rotation to 0 for 1 frame
                        derplaunched = false;
                    }
                }
            }
            else
                spun = false;

            
            if (!npc.friendly)
            {
                //------------Projectile immune
                if (aridimmunetime > 0)
                {
                    //Main.NewText("PLEASE WORK::::::" + aridimmunetime, 204, 101, 22);
                    aridimmunetime--;
                }
                if (forbiddenimmunetime > 0)
                {
                    forbiddenimmunetime--;
                }
                if (bloodimmunetime > 0)
                {
                    bloodimmunetime--;
                }
            }
            //______________
            int chance;
            if (Main.invasionType != 0 || Main.eclipse || Main.bloodMoon || Main.slimeRain ||Main.pumpkinMoon || Main.snowMoon
                || Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerSolar || Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerVortex
                || Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerNebula || Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneTowerStardust)
                chance = 7;
            else
                chance = 4;
           
            if (player.GetModPlayer<EquipmentEffects>().heartSteal && !player.dead) //For the Jar of hearts
            {
                if (!npc.SpawnedFromStatue && npc.life <= (npc.lifeMax * 0.50f) && !npc.friendly && npc.lifeMax > 5 && !npc.buffImmune[(BuffType<HeartDebuff>())]) //Rolls to see the outcome when firts hit under 50% life
                {
                    if (!npc.GetGlobalNPC<NPCEffects>().heartStolen)//Makes sure this only happens once
                    {
                        if (!npc.boss) //non bosses
                        {
                            if (npc.type != ModContent.NPCType<AridBossMinion>() && npc.type != ModContent.NPCType<TheUltimateBossMinion>() && npc.type != NPCID.EaterofWorldsBody && npc.type != ModContent.NPCType<SuperPainDummy>())
                            {
                                //Main.NewText("Invasion is= " + chance, 204, 101, 22);

                                if (Main.rand.Next(chance) == 0) //1 in 4 chance to have the debuff applied and to heal player, 1 in 7 for invasions
                                {
                                    if (ModLoader.TryGetMod("TMLAchievements", out Mod mod))
                                    {
                                        mod.Call("Event", "HeartSteal");
                                    }
                                    //Item.NewItem(new EntitySource_Loot(npc), new Vector2(npc.Center.X, npc.Center.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Tools.SuperHeartPickup>());                            
                                    //Main.LocalPlayer.statLife += 20;
                                    //Main.LocalPlayer.HealEffect(20, true);                               

                                    //Main.player[Main.myPlayer].lifeSteal -= 20;
                                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0f, 0f, 305, 0, 0f, player.whoAmI, Main.myPlayer, 20); //Damage

                                    //Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("TESTER."), new Color(224, 141, 255));

                                    SoundEngine.PlaySound(SoundID.NPCDeath7, npc.Center);
                                    for (int i = 0; i < 15; i++)
                                    {
                                        var dust = Dust.NewDustDirect(new Vector2(npc.Center.X, npc.Center.Y), 5, 5, 72);
                                        //dust.noGravity = true;
                                    }
                                    npc.AddBuff(ModContent.BuffType<HeartDebuff>(), 3600);
                                    npc.GetGlobalNPC<NPCEffects>().heartStolen = true; //prevents more hearts from being dropped

                                }
                                else //Otherwise it just prevents the roll from happening again
                                {
                                    npc.GetGlobalNPC<NPCEffects>().heartStolen = true;
                                }
                            }
                        }
                        else //for bosses
                        {
                            //Main.player[Main.myPlayer].lifeSteal -= 75;
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0f, 0f, 305, 0, 0f, player.whoAmI, Main.myPlayer, 75); //Damage

                            SoundEngine.PlaySound(SoundID.NPCDeath7, npc.Center);
                            for (int i = 0; i < 15; i++)
                            {
                                var dust = Dust.NewDustDirect(new Vector2(npc.Center.X, npc.Center.Y), 5, 5, 72);
                                //dust.noGravity = true;
                            }
                            npc.AddBuff(ModContent.BuffType<HeartDebuff>(), 3600);
                            npc.GetGlobalNPC<NPCEffects>().heartStolen = true;
                        }
                    }
                }
            }
            /*
            bool lineOfSight = Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height);
            if (Main.LocalPlayer.HasBuff(BuffType<BloodBuff>()) && !npc.friendly && npc.lifeMax > 5) //If the player has taken a blood potion and the NPC is within a certian radius of the player
            {

                if (Vector2.Distance(Player.Center, target.Center) <= 140 && lineOfSight)
                {

                    npc.AddBuff(mod.BuffType("BloodDebuff"), 2);
                }
            }*/

            //COVER YOURSELF IN OIL
            /*if (npc.HasBuff(BuffID.Oiled) && Main.raining && !npc.boss)
            {
                npc.velocity.Y = -10;
            }*/
            if (webwhipcooldown < 60)
            {
                webwhipcooldown++;
            }
            if (bloodwhipcooldown < 60)
            {
                bloodwhipcooldown++;
            }
            if (forbiddenwhipcooldown < 60)
            {
                forbiddenwhipcooldown++;
            }
            if (spacerockwhipcooldown < 60)
            {
                spacerockwhipcooldown++;
            }

            if (arrowcooldown > 0)
            {
                arrowcooldown--;
            }

            //explode
            if (explosionNPCflame > 0)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.FlameWaders, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(npc.Center.X, npc.Bottom.Y)
                }, player.whoAmI);
                explosionNPCflame--;
            }

            if (bosssoulcooldown > 0)
                bosssoulcooldown--;
        }
        public override void SetDefaults(NPC npc)
        {
            /*if (npc.type == NPCID.BloodNautilus)
            {
                npc.GivenName = "Micheal";
            }*/
        }
        public override void OnKill(NPC npc)
        {
            var player = Main.LocalPlayer;
            if (player.GetModPlayer<EquipmentEffects>().heartpotion) //For the Heart Potion
            {
                if (!npc.SpawnedFromStatue && !npc.friendly && npc.lifeMax > 5 && !npc.boss && player.statLife < player.statLifeMax2)
                {
                    if (Main.rand.Next(5) == 0)
                    {
                        Item.NewItem(new EntitySource_Loot(npc), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ItemID.Heart);
                    }
                }
                if (npc.boss)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Item.NewItem(new EntitySource_Loot(npc), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ItemID.Heart);
                    }
                }
            }
            if (player.GetModPlayer<EquipmentEffects>().superHeartpotion) //For the Super Heart Potion
            {

                if (!npc.SpawnedFromStatue && !npc.friendly && npc.lifeMax > 5 && !npc.boss && player.statLife < player.statLifeMax2)
                {
                    if (Main.rand.Next(8) == 0)
                    {
                        Item.NewItem(new EntitySource_Loot(npc), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Tools.SuperHeartPickup>());
                    }
                }
                if (npc.boss)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Item.NewItem(new EntitySource_Loot(npc), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Tools.SuperHeartPickup>());
                    }
                }
            }
            if (player.GetModPlayer<EquipmentEffects>().deathList) //enemy drop souls
            {
                if (!npc.SpawnedFromStatue && !npc.friendly && npc.lifeMax > 5 && !npc.boss)
                {
                    Item.NewItem(new EntitySource_Loot(npc), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Tools.SoulDeathPickup>());
                    if (npc.type == ModContent.NPCType<NPCs.HellMiniBoss>())
                    {
                        for (int i = 0; i < 8; i++) //Because you get one from normal means
                            Item.NewItem(new EntitySource_Loot(npc), new Vector2(npc.Center.X - 150, npc.Center.Y - 150), new Vector2(300, 300), ModContent.ItemType<Items.Tools.SoulDeathPickup>());
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningHit, new ParticleOrchestraSettings
                        {
                            PositionInWorld = new Vector2(npc.Center.X, npc.Center.Y),

                        }, player.whoAmI);
                    }
                }
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
           
            if (heartDebuff)
            {
                npc.lifeRegen -= 50;

                damage = 5;

            }
            if (superburnDebuff)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 80;
                    damage = 10;
                }
                else
                {
                    npc.lifeRegen -= 30;
                    damage = 5;
                }
            }
            if (sandBurn)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 100;
                    damage = 12;
                }
                else
                {
                    npc.lifeRegen -= 50;
                    damage = 5;
                }
            }
            if (superFrost)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 82;
                    damage = 12;
                }
                else
                {
                    npc.lifeRegen -= 32;
                    damage = 5;
                }
            }
            if (darknessDebuff)
            {
                npc.lifeRegen -= 60;

                damage = 6;

            }
            if (hellSoulFire)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 130;
                    damage = 15;
                }
                else
                {
                    npc.lifeRegen -= 80;
                    damage = 10;
                }
            }
       
            if (ultraburnDebuff)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 150;
                    damage = 15;
                }
                else
                {
                    npc.lifeRegen -= 100;
                    damage = 10;
                }
            }
            if (ultrafrostDebuff)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 150;
                    damage = 15;
                }
                else
                {
                    npc.lifeRegen -= 100;
                    damage = 10;
                }
            }
            var player = Main.LocalPlayer;

            if (spookedDebuff)
            {
                if (player.GetModPlayer<EquipmentEffects>().spooked == true && player.GetModPlayer<EquipmentEffects>().spookyClaws == true)
                {
                    npc.lifeRegen -= 800;
                    damage = 400;
                }
                else
                {
                    npc.lifeRegen -= 400;
                    damage = 200;
                }
            }
            if (lunarBoulderDB)
            {
                npc.lifeRegen -= 300;

                damage = 30;

            }
        }
        int particle = 0;
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {  
            if (lunarBoulderDB)
            {
                int choice = Main.rand.Next(4);
                if (choice == 0)
                {
                    particle = 244;
                }
                else if (choice == 1)
                {
                    particle = 110;
                }
                else if (choice == 2)
                {
                    particle = 111; ;
                }
                else if (choice == 3)
                {
                    particle = 112;
                }
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, particle, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                if (!Main.dedServ)
                {
                    Lighting.AddLight(npc.position, 1f, 0.5f, 0f);
                }
            }           
            if (sandBurn)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 10, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 1.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                if (!Main.dedServ)
                {
                    Lighting.AddLight(npc.position, 0.1f, 0.2f, 0.7f);
                }
            }
          
            if (beetled)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 186, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

            }
            if (heartDebuff)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, 115, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
                if (Main.rand.NextBool(4))
                {
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].scale *= 0.5f;
                }
                int dust2 = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 115, 0, 2, 0, default, 1f);
                Main.dust[dust2].noGravity = true;

            }
            if (superFrost)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 135, npc.velocity.X * 1.2f, npc.velocity.Y * 1.2f, 130, default, 2f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity

                    int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 135, npc.velocity.X, npc.velocity.Y, 130, default, 1f);
                    Main.dust[dust].velocity *= 0.5f;
                }

            }           
            if (superburnDebuff)
            {

                int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, npc.velocity.X * 1.2f, npc.velocity.Y * 1.2f, 0, default, 2f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity

                int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, npc.velocity.X, npc.velocity.Y, 0, default, 1f);
                Main.dust[dust].velocity *= 0.5f;


            }
            if (hellSoulFire)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173, npc.velocity.X * 1.2f, npc.velocity.Y * 1.2f, 0, default, 2f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity


                }

            }
            if (darknessDebuff)
            {

                int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 54, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, default, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
                if (Main.rand.NextBool(4))
                {
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].scale *= 0.5f;
                }

            }
            if (ultraburnDebuff)
            {

                int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, npc.velocity.X * 1.2f, npc.velocity.Y * 1.2f, 0, default, 2.5f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity

                int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, npc.velocity.X, -1, 0, default, 1.5f);
                Main.dust[dust2].noGravity = true; //this make so the dust has no gravity


            }
            if (ultrafrostDebuff)
            {

                int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 135, npc.velocity.X * 1.2f, npc.velocity.Y * 1.2f, 0, default, 2.5f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity

                int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 135, npc.velocity.X, -1, 0, default, 1.5f);
                Main.dust[dust2].noGravity = true; //this make so the dust has no gravity


            }
            if (spookedDebuff)
            {
                if (Main.rand.Next(5) == 0)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 200, npc.velocity.X * 1.2f, -3, 0, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].fadeIn = 0.5f; //this make so the dust has no gravity

                }
                drawColor = new Color(255, 68, 0);

            }
            if (aridCoreDebuff)
            {
                if (Main.rand.Next(5) == 0)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 138, npc.velocity.X * 1.2f, -3, 0, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].fadeIn = 0.5f; //this make so the dust has no gravity

                }
                drawColor = new Color(180, 150, 15);
            }
            if (BlizzardDebuff)
            {
                int dust = Dust.NewDust(new Vector2(npc.position.X - 10, npc.position.Y - 10), npc.width + 20, npc.height + 20, 156, npc.velocity.X, npc.velocity.Y, 0, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                drawColor = new Color(102, 255, 255);

            }
            if (webDebuff)
            {
                if (Main.rand.Next(4) < 2)
                {
                    int dust = Dust.NewDust(npc.Center, 0, 0, 31, 0, 0, 50, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.6f;

                    Main.dust[dust].fadeIn = 2;
                }
                /*if (Main.rand.Next(4) < 1)
                {
                    double deg = Main.rand.Next(0, 360); //The degrees
                    double rad = deg * (Math.PI / 180); //Convert degrees to radians
                    double dist = 50; //Distance away from the enemy
                    float dustx = npc.Center.X - (int)(Math.Cos(rad) * dist);
                    float dusty = npc.Center.Y - (int)(Math.Sin(rad) * dist);

                    Vector2 velocity = Vector2.Normalize(new Vector2(npc.Center.X, npc.Center.Y) - new Vector2(dustx, dusty)) * 6;

                    var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 0, 0, 31, velocity.X, velocity.Y);
                    dust.noGravity = true;
                    dust.scale = 1f;
                }*/

            }
            if (WhiptagWeb)
            {
                if (Main.rand.Next(4) < 1)
                {
                    int dust = Dust.NewDust(npc.Center - new Vector2(10f, 10f), 20, 20, 31, 0, 0, 50, default, 0.8f);
                    Main.dust[dust].noGravity = true;

                }

            }
            if (WhiptagBlood)
            {
                if (Main.rand.Next(4) < 1)
                {
                    int dust = Dust.NewDust(npc.Center - new Vector2(10f, 10f), 20, 20, 115, 0, 4, 50, default, 1.2f);
                    Main.dust[dust].noGravity = true;
                }
            }
            if (WhiptagForbidden)
            {
                if (Main.rand.Next(4) < 2)
                {
                    int dust = Dust.NewDust(npc.Center - new Vector2(5f, 5f), 10, 10, 10, 0, 0, 100, default, 1.25f);
                    Main.dust[dust].noGravity = true;
                }
            }
            if (WhiptagSpaceRock)
            {
                if (Main.rand.Next(4) < 1)
                {
                    int dust = Dust.NewDust(npc.Center - new Vector2(10f, 10f), 20, 20, 0, 0, -4, 100, default, 1f);
                    Main.dust[dust].noGravity = true;

                }
            }
        }
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            
            base.ModifyIncomingHit(npc, ref modifiers);
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            var player = Main.player[projectile.owner];

            if (player.GetModPlayer<ArmourSetBonuses>().shadowflameSet == true)
            {
                if (npc.HasBuff(BuffID.ShadowFlame))
                {
                    modifiers.FlatBonusDamage += 5;
                }
            }

            if (projectile.type == ModContent.ProjectileType<Projectiles.AncientArmourProj>()) //No crit from arid explosion
            {
                modifiers.DisableCrit();
            }
            if (projectile.type == ModContent.ProjectileType<Projectiles.HellSoulArmourProj>() || projectile.type == ModContent.ProjectileType<Projectiles.AncientArmourProj>()) //No crit from hellsoul explosion
            {
                if (npc.aiStyle == 6 || npc.type == NPCID.TheDestroyerBody) //Worms take reduced damage
                {
                    modifiers.FinalDamage *= 0.5f;
                }
                modifiers.DisableCrit();
            }
            if (npc.type == NPCType<NPCs.DerpMimic>()) //Takes 666 less damage
            {
                modifiers.FinalDamage.Flat -= 666;

            }
            if (npc.type == NPCType<NPCs.Boss.StormBoss>()) //75% damage from homing projectiles
            {
                if (ProjectileID.Sets.CultistIsResistantTo[projectile.type])
                {
                    modifiers.FinalDamage *= 0.75f;
                }
            }

            if (projectile.GetGlobalProjectile<Basefiles.SpinnerReflect>().reflected || projectile.type == ModContent.ProjectileType<SnowmanExplosion>()) //update damage to be the same as if it hit the player
            {
                if (Main.masterMode)
                {
                    modifiers.FinalDamage *= 6;
                }
                else if (Main.expertMode && !Main.masterMode)
                {
                    modifiers.FinalDamage *= 4;
                }
                else
                {
                    modifiers.FinalDamage *= 2;

                }
            }

            if (player.GetModPlayer<EquipmentEffects>().aridBossAccess == true && aridCoreDebuff) //Ancient Emblem extra damage
            {
                float extradmg;
                int buffindex = npc.FindBuffIndex(ModContent.BuffType<AridCoreDebuff>());
                if (buffindex > -1)
                {
                    extradmg = npc.buffTime[buffindex] + 1;
                    //Main.NewText("The test = " + (extradmg / 1000), 204, 101, 22);
                    modifiers.FinalDamage *= 1 + (extradmg / 1000); //1% for every 10 frames, starts at 20%, 200 frames / 1000 = 0.02, plus 1 is 1.02
                }
                //damage = damage + ((damage * 20) / 17);
                //modifiers.FinalDamage *= 1.15f; //15% extra damage
                //modifiers.FinalDamage *= 1 + (extradmg / 10); //15% extra damage
            }

            //Whips
            if (!projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type] == true || projectile.sentry))
            {
                //Spider Whip
                if (WhiptagWeb)
                {
                    modifiers.FlatBonusDamage += 3; //tag damage

                    if (player.HasMinionAttackTargetNPC && npc == Main.npc[player.MinionAttackTargetNPC]) //summon projectile
                    {
                        if (Main.rand.Next(100) <= 20) //20% chance
                        {
                            npc.AddBuff(ModContent.BuffType<WebDebuff>(), 120);
                        }
                    }

                }
                //Blood Whip
                if (WhiptagBlood)
                {
                    //modifiers.FlatBonusDamage += 2; //tag damage
                    if (Main.rand.Next(100) <= 8) //crit
                    {
                        modifiers.SetCrit();
                    }
                    if (player.HasMinionAttackTargetNPC && npc == Main.npc[player.MinionAttackTargetNPC]) //summon projectile
                    {
                        if (bloodwhipcooldown >= 60)
                        {

                            Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X, npc.Center.Y - (40 - (npc.width / 2))), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.WhipProjs.BloodWhipProj2>(), 15, 0, Main.myPlayer, 0, Main.rand.Next(0, 359));
                            bloodwhipcooldown = 0;
                        }

                    }
                }
                //Forbidden whip
                if (WhiptagForbidden)
                {
                    if (Main.rand.Next(100) <= 12)
                    {
                        modifiers.SetCrit();
                    }

                    if (player.HasMinionAttackTargetNPC && npc == Main.npc[player.MinionAttackTargetNPC])
                    {
                        npc.GetGlobalNPC<NPCEffects>().forbiddenimmunetime = 20;// makes sure the enemy that summons the sand can't get hit by it

                        for (int i = 0; i < Main.maxNPCs; i++) //Shoots sand at one enemy
                        {
                            NPC target = Main.npc[i];

                            target.TargetClosest(true);

                            if (Vector2.Distance(npc.Center, target.Center) <= 400f && Vector2.Distance(npc.Center, target.Center) > 15f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy && Collision.CanHit(npc.Center, 0, 0, target.Center, 0, 0))
                            {
                                if (forbiddenwhipcooldown >= 10)
                                {
                                    float projspeed = 5;
                                    Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(npc.Center.X, npc.Center.Y)) * projspeed;

                                    int ProjID = Projectile.NewProjectile(target.GetSource_FromAI(), new Vector2(npc.Center.X, npc.Center.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.WhipProjs.DesertWhipProj2>(), 10, 0, Main.myPlayer);

                                    forbiddenwhipcooldown = 0;
                                }
                            }
                        }
                    }
                }
                if (WhiptagSpaceRock)
                {
                    modifiers.FlatBonusDamage += 8; //tag damage
                    if (Main.rand.Next(100) <= 18)
                    {
                        modifiers.SetCrit();
                    }

                    if (player.HasMinionAttackTargetNPC && npc == Main.npc[player.MinionAttackTargetNPC]) //summon projectile
                    {
                        if (spacerockwhipcooldown >= 25)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X + 200, npc.Center.Y - 300), new Vector2(-15, 23f), ModContent.ProjectileType<Projectiles.WhipProjs.SpaceRockWhipProj2>(), 60, 0, Main.myPlayer);
                            }
                            else
                            {
                                Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X - 200, npc.Center.Y - 300), new Vector2(15, 23f), ModContent.ProjectileType<Projectiles.WhipProjs.SpaceRockWhipProj2>(), 60, 0, Main.myPlayer);
                            }
                            spacerockwhipcooldown = 0;
                        }

                    }
                }
            }

            if (player.GetModPlayer<EquipmentEffects>().aridCritChest == true)
            {
                //damage = (int)(damage * 1.1f);
                modifiers.CritDamage *= 1.1f;
            }
            if (player.GetModPlayer<EquipmentEffects>().derpEye == true)
            {
                //damage = (int)(damage * 1.15f);
                modifiers.CritDamage *= 1.15f;
            }
            if (player.GetModPlayer<EquipmentEffects>().derpEyeGolem == true)
            {
                //damage = (int)(damage * 1.15f);
                modifiers.CritDamage *= 1.15f;
            }
            if (player.HasBuff(BuffType<ShroomiteBuff>()))//If the player has the shroomite potion then 10% increase ammo damage
            {
                if (projectile.CountsAsClass(DamageClass.Ranged))
                modifiers.CritDamage *= 1.10f;
            }

            if (player.GetModPlayer<EquipmentEffects>().deathList) //hitting a boss with the Reaper's List Equipped
            {
                if (Main.rand.Next(25) == 0 && npc.boss && bosssoulcooldown == 0)
                {
                    Item.NewItem(new EntitySource_Loot(npc), new Vector2(npc.position.X, npc.position.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Tools.SoulDeathPickup>());
                    bosssoulcooldown = 60;
                }
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitByItem(npc, player, item, ref modifiers);
            
            if (player.GetModPlayer<ArmourSetBonuses>().shadowflameSet == true)
            {
                if (npc.HasBuff(BuffID.ShadowFlame))
                {
                    modifiers.FlatBonusDamage += 5;
                }
            }
            if (npc.type == NPCType<NPCs.DerpMimic>()) //Takes 666 less damage
            {
                modifiers.FinalDamage.Flat -= 666;

            }

            if (player.GetModPlayer<EquipmentEffects>().aridCritChest == true)
            {
                //damage = (int)(damage * 1.1f);
                modifiers.CritDamage *= 1.1f;
            }
            if (player.GetModPlayer<EquipmentEffects>().derpEye == true)
            {
                //damage = (int)(damage * 1.15f);
                modifiers.CritDamage *= 1.15f;
            }
            if (player.GetModPlayer<EquipmentEffects>().derpEyeGolem == true)
            {
                //damage = (int)(damage * 1.15f);
                modifiers.CritDamage *= 1.15f;
            }
            if (player.GetModPlayer<EquipmentEffects>().aridBossAccess == true && aridCoreDebuff) //Ancient Emblem extra damage
            {
                float extradmg;
                int buffindex = npc.FindBuffIndex(ModContent.BuffType<AridCoreDebuff>());
                if (buffindex > -1)
                {
                    extradmg = npc.buffTime[buffindex] + 1;
                    modifiers.FinalDamage *= 1 + (extradmg / 1000); //1% for every 10 frames, starts at 20%, 200 frames / 1000 = 0.02, plus 1 is 1.02
                }
            }
        }
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            //if (npc.type == NPCID.Gnome) //secret
              //  SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/Gnomed") with { Volume = 1.5f, MaxInstances = -1 }, npc.Center);

            base.OnHitPlayer(npc, target, hurtInfo);
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitByItem(npc, player, item, hit, damageDone);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitByProjectile(npc, projectile, hit, damageDone);
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            //if ()
            if (Main.LocalPlayer.armor[0].type == ModContent.ItemType<Items.Vanitysets.ThePainMask>() || Main.LocalPlayer.armor[10].type == ModContent.ItemType<Items.Vanitysets.ThePainMask>())
            {
                if (Main.rand.Next(4) == 0)
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            chat = "You seem to be in a lot of pain.";
                            break;
                        case 1:
                            chat = "ThePain!!";
                            break;
                        case 2:
                            chat = "How do you handle all that pain?";
                            break;

                    }
                }
            }
            if (Main.LocalPlayer.armor[0].type == ModContent.ItemType<Items.Vanitysets.TheClaymanMask>() || Main.LocalPlayer.armor[10].type == ModContent.ItemType<Items.Vanitysets.TheClaymanMask>())
            {
                if (Main.rand.Next(4) == 0)
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            chat = "What's with the judgmental stare?";
                            break;
                        case 1:
                            chat = "Clayman!!";
                            break;
                        case 2:
                            chat = "Are you claymanning me?";
                            break;
                    }
                }
            }
            if (Main.LocalPlayer.armor[0].type == ModContent.ItemType<Items.Vanitysets.UltimateFearMask>() || Main.LocalPlayer.armor[10].type == ModContent.ItemType<Items.Vanitysets.UltimateFearMask>())
            {
                if (Main.rand.Next(4) == 0)
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            chat = "What the heck is that thing on your head?";
                            break;
                        case 1:
                            chat = "Wh..what did you have to kill to get that?";
                            break;
                        case 2:
                            chat = "Your mask is scaring me!";
                            break;
                    }
                }
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (Main.invasionType == 2 && Main.invasionProgress > 0) //don't spawn until even has progressed
            {
                pool.Add(ModContent.NPCType<SnowmanBomb>(), .2f);
            }
            /*if (!GetInstance<ConfigurationsGlobal>().PreventSnowmenEnemies)
            {
                Player player = spawnInfo.Player;
                if (player.ZoneSnow && Main.raining && Main.hardMode && player.ZoneOverworldHeight)
                {
                    pool.Add(NPCID.SnowmanGangsta, .25f);
                    pool.Add(NPCID.SnowBalla, .25f);
                    pool.Add(NPCID.MisterStabby, .25f);
                }
            }*/
        }
    }

}

        
