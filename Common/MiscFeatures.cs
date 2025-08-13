using ExampleMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using ReLogic.Graphics;
using ReLogic.Peripherals.RGB;
using StormDiversMod.Buffs;
using StormDiversMod.Items.Accessory;
using StormDiversMod.Items.Armour;
using StormDiversMod.Items.Furniture;
using StormDiversMod.Items.Tools;
using StormDiversMod.Items.Vanitysets;
using StormDiversMod.Items.Weapons;
using StormDiversMod.NPCs;
using StormDiversMod.NPCs.Boss;
using StormDiversMod.Projectiles;
using StormDiversMod.Projectiles.Minions;
using StormDiversMod.Projectiles.Petprojs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security.Policy;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;
using Terraria.WorldBuilding;
using static System.Net.WebRequestMethods;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.Common
{
    public class MiscFeatures : ModPlayer
    {
        /*public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            return new[] {
                new Item(ModContent.ItemType<ThePainMask>()),
                new Item(ModContent.ItemType<TheClaymanMask>()),
                new Item(ModContent.ItemType<PainMusicBoxitem>())
            };
        }*/

        public bool screenshaker; //Weapons that shake the screen
        public int shaketimer; //How long to shake the screen for

        public int templeWarning; //Warning until Temple Guardians spawn

        public int playerimmunetime; //makes player immune to damage

        //public int ninelives; //how many kills with the sickle, up to 9
        //public int ninelivescooldown; //cooldown to remove a soul

        public bool explosionfall; //Player has been launched by a stickybomb
        public int explosionflame; //How long to have flames under the player's feet after being launched

        public bool cursedplayer;
        public bool soulpickup;
        public override void ResetEffects() //Resets bools if the item is unequipped
        {
            screenshaker = false;
            //soulpickup = false;
        }
        public override void UpdateDead()//Reset all ints and bools if dead======================
        {
            templeWarning = 0;
            shaketimer = 0;
            //ninelives = 0;
            //ninelivescooldown = 0;
            explosionfall = false;

            cursedplayer = false;
        }

        //===============================================================================================================
        public override bool PreItemCheck()
        {
            return base.PreItemCheck();
        }
        public override void ModifyScreenPosition()//screenshaker
        {
            int screenpos = GetInstance<ConfigurationsIndividual>().ShakeAmount; //5 default
            if (screenshaker)
            {
                shaketimer = screenpos * 2; //10 default
                screenshaker = false;
            }
            if (shaketimer > 0)
            {
                Main.screenPosition += new Vector2(Main.rand.Next(-screenpos, screenpos), Main.rand.Next(-screenpos, screenpos));
                shaketimer--;
            }

        }
        //int bufftimer;
        
        public override void PostUpdateEquips() //Updates every frame
        {
            if (Player.HasBuff(BuffID.PotionSickness))
            {
                /*int buffindex = Player.FindBuffIndex(BuffID.PotionSickness);
                if (buffindex > -1)
                {
                    bufftimer = Player.buffTime[buffindex] + 1;
                }
                Main.NewText("Potion time: " + (bufftimer / 60 + 1), Color.Crimson);*/
            }
            //If player holds forbidden item summon up to 6 Guardians after 5 seconds
            if (!NPC.downedPlantBoss && Player.GetModPlayer<PlayerUpgrades>().NoTempleCurse == false && !GetInstance<ConfigurationsGlobal>().NoScaryCurse)
            {
                if ((Player.HeldItem.type == ModContent.ItemType<LizardSpinner>() || Player.HeldItem.type == ModContent.ItemType<LizardFlame>() || Player.HeldItem.type == ModContent.ItemType<LizardSpell>() || Player.HeldItem.type == ModContent.ItemType<LizardMinion>())
                    && Player.controlUseItem)
                {
                    Player.AddBuff(ModContent.BuffType<TempleDebuff>(), Player.HeldItem.useAnimation);
                }
                if (Player.armor[0].type == ModContent.ItemType<TempleBMask>() || Player.armor[1].type == ModContent.ItemType<TempleChest>() || Player.armor[2].type == ModContent.ItemType<TempleLegs>())
                {
                    Player.AddBuff(ModContent.BuffType<TempleDebuff>(), 2);
                }

                if (Player.HasItemInAnyInventory(ModContent.ItemType<TempleBMask>()) || Player.HasItemInAnyInventory(ModContent.ItemType<TempleChest>()) || Player.HasItemInAnyInventory(ModContent.ItemType<TempleLegs>())
                || Player.HasItemInAnyInventory(ModContent.ItemType<LizardSpinner>()) || Player.HasItemInAnyInventory(ModContent.ItemType<LizardFlame>()) || Player.HasItemInAnyInventory(ModContent.ItemType<LizardSpell>())
                || Player.HasItemInAnyInventory(ModContent.ItemType<LizardMinion>()) || Player.HasBuff(ModContent.BuffType<LizardMinionBuff>()))
                {
                    templeWarning++;
                    string templecursetext = "The curse of the temple item in your possession activates!!!";
                    if (templeWarning == 1)
                    {
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(templecursetext), new Color(204, 101, 22));
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText(templecursetext, 204, 101, 22);
                        }
                        CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.SaddleBrown, templecursetext, false);

                    }

                    if (templeWarning >= 480 && NPC.CountNPCS(ModContent.NPCType<GolemMinion>()) < 6) //spawn up to 6
                    {
                        if (Main.rand.Next(30) == 0)
                        {
                            NPC.SpawnOnPlayer(Player.whoAmI, ModContent.NPCType<NPCs.GolemMinion>());
                        }
                    }

                    Player.buffImmune[BuffID.Darkness] = false;
                    Player.buffImmune[BuffID.Blackout] = false;
                    Player.buffImmune[BuffID.Obstructed] = false;
                    Player.buffImmune[BuffID.Slow] = false;
                    Player.buffImmune[BuffID.Bleeding] = false;
                    Player.buffImmune[ModContent.BuffType<TempleDebuff>()] = false;

                    //don't give debuffs, but give effects of them
                    Player.bleed = true;
                    Player.blind = true;

                    if (templeWarning > 0 && templeWarning < 240)
                    {
                        //Player.AddBuff(BuffID.Darkness, 2);
                        //Player.AddBuff(BuffID.Bleeding, 2);
                    }

                    else if (templeWarning >= 240 && templeWarning < 480)
                    {
                        Player.blackout = true;
                        Player.slow = true;
                        //Player.AddBuff(BuffID.Blackout, 2);
                        //Player.AddBuff(BuffID.Slow, 2);
                    }
                    else if (templeWarning >= 480)
                    {
                        Player.slow = true;
                        Player.AddBuff(BuffID.Obstructed, 2);
                        //Player.AddBuff(BuffID.Bleeding, 2);
                        //Player.AddBuff(BuffID.Slow, 2);
                    }
                    cursedplayer = true;
                }
                else
                {
                    cursedplayer = false;

                    templeWarning = 0;
                }
            }
            else
            {
                cursedplayer = false;

                templeWarning = 0;
            }

            
            /*if (Player.ZoneJungle && !NPC.downedPlantBoss) //This code is only active when certain criteia is met, sadly the zonelizardtemple doesn't work
            {
                int xtilepos = (int)(Player.position.X + (float)(Player.width / 2)) / 16;
                int ytilepos = (int)(Player.position.Y + (float)(Player.height / 2)) / 16;

                if (Main.tile[xtilepos, ytilepos].WallType == WallID.LihzahrdBrickUnsafe)
                {
                    if (!GetInstance<ConfigurationsGlobal>().SmellyPlayer)
                    {

                        if (NPC.CountNPCS(ModContent.NPCType<GolemMinion>()) < 3)
                        {
                            templeWarning++;
                            if (templeWarning == 1 && !NPC.AnyNPCs(ModContent.NPCType<GolemMinion>()))
                            {
                                if (Main.netMode == 2) // Server
                                {
                                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("The ancient temple defenses begin to wake up!!!"), new Color(204, 101, 22));
                                }
                                else if (Main.netMode == 0) // Single Player
                                {
                                    Main.NewText("The ancient temple defenses begin to wake up!!!", 204, 101, 22);
                                }
                            }

                            if (templeWarning >= 300)
                            {
                                if (Main.rand.Next(30) == 0)
                                {
                                    NPC.SpawnOnPlayer(Player.whoAmI, ModContent.NPCType<NPCs.GolemMinion>());
                                }
                            }
                        }
                        if (templeWarning >= 300)
                        {
                            Player.AddBuff(BuffID.Obstructed, 2);
                        }
                    }
                    if (GetInstance<ConfigurationsGlobal>().SmellyPlayer) //For noobs who can't handle the temple
                    {
                        Player.AddBuff(BuffID.Stinky, 2);
                        //Player.AddBuff(BuffID.Darkness, 2);
                    }
                }
            }
            else
            {
                templeWarning = 0;
            }*/
            if (playerimmunetime > 0)
            {
                playerimmunetime--;
            }
            if (Player.GetModPlayer<EquipmentEffects>().bootFall == false)
            {
                playerimmunetime = 0;
            }

            if (explosionflame > 0)
            {
                if (Player.gravDir == 1)
                {
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.FlameWaders, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Player.Center.X, Player.Bottom.Y)
                    }, Player.whoAmI);
                }
                else
                {
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.FlameWaders, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Player.Center.X, Player.Top.Y)
                    }, Player.whoAmI);
                }
                explosionflame--;
            }
            if (explosionfall)//Correct fall damage when launched via sticky bomb
            {

                if (Player.velocity.Y > 0)
                {
                    Player.fallStart = (int)Player.tileTargetY;
                    //Main.NewText("plswork" + Player.tileTargetY, 204, 101, 22);

                    explosionfall = false;
                }
            }

            if (NPC.CountNPCS(ModContent.NPCType<NPCs.Boss.ThePainBoss>()) == 0)
            {
                Player.ClearBuff(ModContent.BuffType<YouCantEscapeDebuff>());
            }
            //Main.NewText("Pain is " + paintime, 220, 63, 139);

            //Achievements
            if (Player.HasBuff(ModContent.BuffType<TwilightPetBuff>()) && Player.HasBuff(ModContent.BuffType<StormLightBuff>()))
            {
                if (ModLoader.TryGetMod("TMLAchievements", out Mod mod))
                {
                    mod.Call("Event", "ThePets");
                }
            }
        }
        int whackcount;
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.lifeMax > 5 && target.type != NPCID.TargetDummy)
            {
                if (item.type == ModContent.ItemType<PainStaff>() || item.type == ModContent.ItemType<PainSword>())
                {
                    if (ModLoader.TryGetMod("TMLAchievements", out Mod mod))
                    {
                        mod.Call("ValueEvent", "Whack", 1f);
                    }
                    for (int i = 0; i < 20; i++) //Pain dust
                    {
                        Vector2 perturbedSpeed = new Vector2(0, -3f).RotatedByRandom(MathHelper.ToRadians(360));
                        if (item.type == ModContent.ItemType<PainStaff>())
                        {
                            var dust = Dust.NewDustDirect(target.Center, 0, 0, 115, perturbedSpeed.X, perturbedSpeed.Y);
                            dust.noGravity = true;
                        }
                        if (item.type == ModContent.ItemType<PainSword>())
                        {
                            var dust = Dust.NewDustDirect(target.Center, 0, 0, 72, perturbedSpeed.X, perturbedSpeed.Y);
                            dust.noGravity = true;
                        }
                    }
                    SoundEngine.PlaySound(SoundID.Item175 with { Volume = 0.5f, Pitch = 0f, MaxInstances = 0 }, target.Center);
                    whackcount += 1;
                    if (whackcount == 15)
                    {
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Seriously, you have a weapon that summons powerful homing projectiles..."), Color.IndianRed);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText("Seriously, you have a weapon that summons powerful homing projectiles...", Color.IndianRed);
                        }
                    }
                    if (whackcount == 30)
                    {
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Yet you choose to whack enemies over the head with it..."), Color.IndianRed);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText("Yet you choose to whack enemies over the head with it...", Color.IndianRed);
                        }
                    }
                    if (whackcount == 45)
                    {
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Like come on, are you some kind of true-melee player or something?"), Color.IndianRed);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText("Like come on, are you some kind of true-melee player or something?", Color.IndianRed);
                        }
                    }
                    if (whackcount == 60)
                    {
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("You do know you're only dealing a fraction of the weapon's damage like this right??"), Color.IndianRed);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText("You do know you're only dealing a fraction of the weapon's damage like this right??", Color.IndianRed);
                        }
                    }
                    if (whackcount == 75)
                    {
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Just left click already!!!"), Color.IndianRed);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText("Just left click already!!!", Color.IndianRed);
                        }
                    }
                    if (whackcount == 90)
                    {
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("!$%!£&%!"), Color.IndianRed);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText("!$%!£&%!", Color.IndianRed);
                        }
                    }
                    if (whackcount >= 175) // + 15 gives 100 hits bfore restarting text
                    {
                        whackcount = 0;
                    }
                    //CombatText.NewText(new Rectangle((int)target.Center.X, (int)target.Center.Y, 12, 4), Color.DeepPink, "Whacks = " + whackcount, false);
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.lifeMax > 5 && target.type != NPCID.TargetDummy)
            {
                if (proj.type == ModContent.ProjectileType<PainProj>() || proj.type == ModContent.ProjectileType<PainStaffProj>())
                {
                    if (whackcount >= 15)
                    {
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("See that's better now isn't it?"), Color.IndianRed);
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText("See that's better now isn't it?", Color.IndianRed);
                        }
                        whackcount = 0;
                    }
                }
            }
            if (proj.type == ModContent.ProjectileType<StompBootProj2>() && target.type != NPCID.TargetDummy) //10 frames of immunity
            {
                playerimmunetime = 10;
            }
            //nine lives
            /*if (proj.type == ModContent.ProjectileType<TheSickleProj>() || proj.type == ModContent.ProjectileType<TheSickleProj2>())
            {
                //Main.NewText("" + ninelives + " " + ninelivescooldown, 204, 101, 22);
                //regular enemies 1 soul 
                if (!target.SpawnedFromStatue && !target.dontTakeDamage && !target.friendly && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
                {
                    if (ninelivescooldown < 540 && ninelives > 0)
                    {
                        if (proj.type == ModContent.ProjectileType<TheSickleProj>())
                            ninelivescooldown += 5; //add some small time if an enemy is attacked
                        if (proj.type == ModContent.ProjectileType<TheSickleProj2>())
                            ninelivescooldown += 9; //add some small time if an enemy is attacked
                    }
                    if (target.life <= 0)
                    {
                        ninelivescooldown = 540; //Reset cooldown to 9 seconds, even at max amount
                        if (ninelives < 9) //Spawn up to 9
                        {
                            ninelives++;//increase counter

                            int nineproj = Projectile.NewProjectile(target.GetSource_FromAI(), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<TheSickleProj3>(), 0, 0, Player.whoAmI, 0, ninelives - 1);//changes ai[1] field for different angles

                            SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1f, Pitch = -1f, MaxInstances = 0 }, target.Center); ;

                            for (int i = 0; i < 3; i++)
                            {
                                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningHit, new ParticleOrchestraSettings
                                {
                                    PositionInWorld = new Vector2(target.Center.X, target.Center.Y),

                                }, Player.whoAmI);
                            }
                            //Main.NewText("" + ninelives, 204, 101, 22);
                        }
                    }
                }
                if ((target.type == ModContent.NPCType<NPCs.HellMiniBoss>()) && target.life <= 0) //Soul Cauldron give all souls
                {
                    ninelivescooldown = 540; //Reset cooldown to 9 seconds, even at max amount
                    for (int i = 0; i < 8; i++) //Because you get one from normal means
                    {
                        if (ninelives < 9)
                        {
                            ninelives++;//increase counter
                            //Main.NewText("" + ninelives, 204, 101, 22);
                            Projectile.NewProjectile(target.GetSource_FromAI(), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<TheSickleProj3>(), 0, 0, Player.whoAmI, 0, ninelives - 1);//changes ai[1] field for different angles

                            for (int j = 0; j < 20; j++)
                            {
                                var dust = Dust.NewDustDirect(target.position, target.width, target.height, 31);
                                dust.noGravity = true;
                                dust.velocity *= 3;
                                dust.scale = 1.5f;

                            }
                        }
                    }
                }
            }*/
        }
        String Paintext = "";
        //public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if ((proj.type == ModContent.ProjectileType<Projectiles.AmmoProjs.ProtoGrenadeProj>() ||
                proj.type == ModContent.ProjectileType<Projectiles.AmmoProjs.ProtoGrenadeProj2>() || proj.type == ModContent.ProjectileType<Projectiles.AmmoProjs.C4Proj>()
                ) && !Player.immune) //100% for shrapnel
            {
                modifiers.FinalDamage /= 2;
            }

            /*if (proj.type == ModContent.ProjectileType<Projectiles.BazookaProj2>()) //50% damage for bazooka
            {
                modifiers.FinalDamage /= 4;

            }*/
            if (proj.type == ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>() && !Player.immune)
            {
                //paintime = 3600;
                //Player.statDefense = 0; //ignores all DR
                //Player.endurance = 0;
                //damage = (Player.statLife / 3); //Deals 1/3 the player's
                if (Player.statLife < Player.statLifeMax2 / 2)//Deals 66% damage below 50% life
                {
                    modifiers.FinalDamage *= 0.66f;
                }
            }
        }
        Color textcolour = Color.DeepPink;
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) //taunt messages when harmed by the final boss
        {
            if (!GetInstance<ConfigurationsIndividual>().NoMessage)
            {
                if (proj.type == ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProj>() || proj.type == ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjCharge>() ||
                    proj.type == ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjGravity>() || proj.type == ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjHome>()
                    || proj.type == ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjShard>())
                {
                    if (Player.statLife < Player.statLifeMax2 && Player.statLife > 0) //No message if dead or revived
                    {
                        if (Main.rand.Next(3) == 0 || proj.type == ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjShard>())
                        {                           //Final phase always says second phase each hit                               
                            int choice = Main.rand.Next(0, 5);
                            if (proj.type != ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjShard>())
                            {
                                if (choice == 0)
                                    Paintext = "That looked very Painful!";
                                else if (choice == 1)
                                    Paintext = "Enjoy the pain!";
                                else if (choice == 2)
                                    Paintext = "How does the pain feel?";
                                else if (choice == 3)
                                    Paintext = "Skill issue!";
                                else if (choice == 4)
                                    Paintext = "You seem to be in a lot of pain!";
                            }
                            else //final phase has different text
                            {
                                if (choice == 0)
                                    Paintext = "How are you still alive?";
                                if (choice == 1)
                                    Paintext = "Just give up!";
                                else if (choice == 2)
                                    Paintext = "You can't win!";
                                else if (choice == 3)
                                    Paintext = "I hope that hurt!";
                                else if (choice == 4)
                                    Paintext = "You can't handle any more pain!";
                            }
                            for (int i = 0; i < 200; i++)//message also appears from boss
                            {
                                NPC painTarget = Main.npc[i];
                                if ((painTarget.type == ModContent.NPCType<NPCs.Boss.TheUltimateBoss>() && !Main.zenithWorld))
                                {
                                    CombatText.NewText(new Rectangle((int)painTarget.Center.X, (int)painTarget.Center.Y, 12, 4), Color.IndianRed, Paintext, false);
                                }
                            }
                            if (Main.netMode == 2) // Server
                                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.IndianRed);
                            else if (Main.netMode == 0) // Single Player
                                Main.NewText(Paintext, Color.IndianRed);
                        }
                        //Player.QuickSpawnItem(null, ModContent.ItemType<ThePainMask>(), 1);
                    }
                }
                if (proj.type == ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>())
                {

                    int choice = Main.rand.Next(0, 5);
                    if (proj.type != ModContent.ProjectileType<NPCs.NPCProjs.TheUltimateBossProjShard>())
                    {
                        if (choice == 0)
                            Paintext = "That looked very Painful!";
                        else if (choice == 1)
                            Paintext = "Enjoy the pain!";
                        else if (choice == 2)
                            Paintext = "How does the pain feel?";
                        else if (choice == 3)
                            Paintext = "Skill issue!";
                        else if (choice == 4)
                            Paintext = "You seem to be in a lot of pain!";

                        for (int i = 0; i < 200; i++)//message also appears from boss
                        {
                            NPC painTarget = Main.npc[i];
                            if ((painTarget.type == ModContent.NPCType<NPCs.Boss.TheUltimateBoss>() && !Main.zenithWorld))
                            {
                                CombatText.NewText(new Rectangle((int)painTarget.Center.X, (int)painTarget.Center.Y, 12, 4), Color.DeepPink, Paintext, false);
                            }
                        }
                        if (Main.netMode == 2) // Server
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), Color.DeepPink);
                        else if (Main.netMode == 0) // Single Player
                            Main.NewText(Paintext, Color.DeepPink);
                    }

                }
            }
        }
        public override void PostHurt(Player.HurtInfo info)
        {
           
        }
        String Suffertext;
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) //revived with Ultimate pain
        {
            /*if (NPC.CountNPCS(ModContent.NPCType<TheUltimateBoss>()) >= 1 && Player.respawnTimer < 5)
            {
                Main.NewText("Stop cheating please, set your respawn timer to 5 or more seconds", 220, 63, 139);

                Player.respawnTimer = 5;
            }*/

            if (Player.HasBuff(ModContent.BuffType<YouCantEscapeDebuff>()) && Player.GetModPlayer<EquipmentEffects>().DeathCore == false && Player.GetModPlayer<EquipmentEffects>().SantaCore == false) //Save you from death once, won't activate if accessory does
            {
                Suffertext = "HOW CAN YOU SUFFER IF YOU'RE DEAD???";
                for (int i = 0; i < 200; i++)//message also appears from boss
                {
                    NPC painTarget = Main.npc[i];
                    if (painTarget.type == ModContent.NPCType<NPCs.Boss.ThePainBoss>())
                    {
                        CombatText.NewText(new Rectangle((int)painTarget.Center.X, (int)painTarget.Center.Y, 12, 4), Color.DeepPink, Suffertext, true);
                    }
                }
                int proj = Projectile.NewProjectile(Player.GetSource_FromAI(), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj2>(), 0, 0, Main.myPlayer);
                Main.projectile[proj].scale = 2.5f;
                SoundEngine.PlaySound(SoundID.Item74 with { Volume = 2f, Pitch = 0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Player.Center);
                SoundEngine.PlaySound(SoundID.Item109 with { Volume = 1f, Pitch = 0f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Player.Center);

                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Suffertext), Color.DeepPink);
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText(Suffertext, Color.DeepPink);
                }

                for (int i = 0; i < 100; i++)
                {
                    float speedY = -8f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(Player.Center, 0, 0, 72, dustspeed.X, dustspeed.Y, 100, default, 1.5f);
                    //Main.dust[dust2].noGravity = true;
                }
                for (int i = 0; i < 6; i++)
                {
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.StellarTune, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Player.Center.X, Player.Center.Y)
                    }, Player.whoAmI);

                }
                Player.HealEffect(Player.statLifeMax2, true);
                Player.statLife = Player.statLifeMax2;
                Player.immuneTime = 120;
                Player.ClearBuff(ModContent.BuffType<YouCantEscapeDebuff>());
                return false;
            }
            return true;
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            base.ModifyHurt(ref modifiers);
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (playerimmunetime > 0)
            {
                return true;
            }
            else
                return false;
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            //glass armour breaking
            if (Player.armor[0].type == ModContent.ItemType<GlassBHelmet>()) //first helmet
            {
                //remove armour and armour sprite
                Player.armor[0].TurnToAir();
                Player.armor[0].UpdateItem(0);
                Player.armor[0].headSlot = 0;
                Player.QuickSpawnItem(null, ItemType<Items.Materials.GlassArmourShard>(), 1); //add armour shard to inventory
                SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0f }, Player.Center); //sound
                int shardcount = Main.rand.Next(7, 9); //7-8 shards
                for (int i = 0; i < shardcount; i++) //summon dust and projectile
                {
                    var dust = Dust.NewDustDirect(new Vector2(Player.position.X, Player.position.Y), Player.width, 15, 149, 0, 0);
                    Vector2 perturbedSpeed = new Vector2(0, -5).RotatedByRandom(MathHelper.ToRadians(150));
                    float scale = 1f - (Main.rand.NextFloat() * .5f);
                    perturbedSpeed = perturbedSpeed * scale;
                    Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y-15), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<GlassShardProj>(), 10, 0, Player.whoAmI);
                }
            }
            else if (Player.armor[2].type == ModContent.ItemType<GlassGreaves>() && Player.armor[0].type != ModContent.ItemType<GlassBHelmet>()) //second leggings
            {
                Player.armor[2].TurnToAir();
                Player.armor[2].UpdateItem(0);
                Player.armor[2].legSlot = 0;
                Player.QuickSpawnItem(null, ItemType<Items.Materials.GlassArmourShard>(), 1);
                SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0f }, Player.Center);
                int shardcount = Main.rand.Next(10, 12); //10-11 shards
                for (int i = 0; i < shardcount; i++) //summon dust and projectile
                {
                    var dust = Dust.NewDustDirect(new Vector2(Player.position.X, Player.Center.Y + 20), Player.width, 15, 149, 0, 0);
                    Vector2 perturbedSpeed = new Vector2(0, -5).RotatedByRandom(MathHelper.ToRadians(150));
                    float scale = 1f - (Main.rand.NextFloat() * .5f);
                    perturbedSpeed = perturbedSpeed * scale;
                    Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y+15), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<GlassShardProj>(), 10, 0, Player.whoAmI);
                }
            }
            else if (Player.armor[1].type == ModContent.ItemType<GlassChestplate>() && Player.armor[1].type != ModContent.ItemType<GlassGreaves>() && Player.armor[0].type != ModContent.ItemType<GlassBHelmet>()) //finally chesplate
            {
                Player.armor[1].TurnToAir();
                Player.armor[1].UpdateItem(0);
                Player.armor[1].bodySlot = 0;
                Player.QuickSpawnItem(null, ItemType<Items.Materials.GlassArmourShard>(), 1);
                SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0f }, Player.Center);
                int shardcount = Main.rand.Next(13, 15); //13-14 shards
                for (int i = 0; i < shardcount; i++) //summon dust and projectile
                {
                    var dust = Dust.NewDustDirect(new Vector2(Player.position.X, Player.Center.Y + 10), Player.width, 15, 149, 0, 0);
                    Vector2 perturbedSpeed = new Vector2(0, -5).RotatedByRandom(MathHelper.ToRadians(150));
                    float scale = 1f - (Main.rand.NextFloat() * .5f);
                    perturbedSpeed = perturbedSpeed * scale;
                    Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<GlassShardProj>(), 10, 0, Player.whoAmI);
                }
            }
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            
        }
    }
    public class Itemchanges : GlobalItem
    {
        public override void ArmorSetShadows(Player player, string set)
        {
            
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            /*if (item.type == ItemID.MagnetSphere)
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "UseMana")
                    {
                        line.Text = line.Text + "\nRIP Leinfors";
                    }
                }
            }*/
            if (item.type is ItemID.LeinforsHat or ItemID.LeinforsShirt or ItemID.LeinforsPants or ItemID.LeinforsAccessory)
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = line.Text + "\n[c/8a00d1: Rest in Piece Leinfors]";
                    }
                }
            }
            if (item.type is ItemID.LeinforsWings)
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        line.Text = line.Text + "\n[c/8a00d1:Rest in Piece Leinfors]";
                    }
                }
            }
        }

        /*public override bool CanUseItem(Item item, Player player) //use this to disable the RoD if you want 
        {
            if (item.type == ItemID.RodofDiscord)
            {
                if (Player.HasBuff(ModContent.BuffType<TwilightDebuff")))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }*/
        public override void SetStaticDefaults() //allows the Cultist bag to be researched
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[ItemID.CultistBossBag] = 3;
            base.SetStaticDefaults();
        }
        public override void SetDefaults(Item item)
        {
            /*if (item.type == ItemID.Tombstone)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.GraveMarker;
            if (item.type == ItemID.GraveMarker)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.CrossGraveMarker;
            if (item.type == ItemID.CrossGraveMarker)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Headstone;
            if (item.type == ItemID.Headstone)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Gravestone;
            if (item.type == ItemID.Gravestone)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Obelisk;
            if (item.type == ItemID.Obelisk)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Tombstone;*/

            if (item.type == ItemID.RichGravestone1)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.GraveMarker;
            if (item.type == ItemID.RichGravestone2)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Tombstone;
            if (item.type == ItemID.RichGravestone3)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.CrossGraveMarker;
            if (item.type == ItemID.RichGravestone4)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Gravestone;
            if (item.type == ItemID.RichGravestone5)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Headstone;
        }
        public override void PostUpdate(Item item)
        {
            var player = Main.LocalPlayer;
            if (item.type is ItemID.RichGravestone1 or ItemID.RichGravestone2 or ItemID.RichGravestone3 or ItemID.RichGravestone4 or ItemID.RichGravestone5)
            {
                if (item.shimmerTime > 0.89)//one tick hopefully
                {
                    player.coinLuck += 100000 * item.stack; //10 gold coins worth

                    if (player.coinLuck > 1000000) //cap at 1M
                        player.coinLuck = 1000000;
                    //Main.NewText("Shimmer time is : " + item.shimmerTime, 204, 101, 22);
                }
                if (!Main.dedServ)
                {
                    Lighting.AddLight(item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
                }
            }
        }
    }
    public class Tiles : GlobalTile
    {
        //Vector2 tilepos;
        int breakchance;
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            //seed drops for flaming seed launcher
            var player = Main.LocalPlayer;
            if (type == TileID.Plants)
            {
                if (player.HasItem(ModContent.ItemType<MoltenSeedLauncher>()))
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Seed, Main.rand.Next(2, 5));
            }

            /*if (!Main.SmartCursorIsUsed)
                tilepos = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y);
            else
                tilepos = new Vector2(Main.SmartCursorX / 16, Main.SmartCursorY / 16);*/

            //break glass tools

            if (!GetInstance<ConfigurationsGlobal>().GlassSoWeak)
                breakchance = (int)(64 * (1 + player.luck));
            else
                breakchance = 1;
            //Main.NewText("Test " + breakchance, Color.Orange);
            if ((player.HeldItem.type == ModContent.ItemType<GlassPick>() && Main.tileSolid[type]) || //only break on solid tiles
                (player.HeldItem.type == ModContent.ItemType<GlassHammer>() && Main.tileSolid[type]) || //only break on sloped blocks
                (player.HeldItem.type == ModContent.ItemType<GlassAxe>() && Main.tileAxe[type])) //only break on trees
            {
                if (Main.rand.Next(breakchance) == 0)//1 in 64 for each hit
                {
                    SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0f }, player.Center);

                    player.HeldItem.TurnToAir(); //remove item
                    player.QuickSpawnItem(null, ItemType<Items.Tools.GlassHandle>(), 1); //spawn broken handle
                    int shardcount = Main.rand.Next(5, 7); //5-6 shards
                    for (int k = 0; k < shardcount; k++) //summon dust and projectile
                    {
                        var dust = Dust.NewDustDirect(new Vector2(player.position.X, player.position.Y), player.width, player.height, 149, 0, 0);

                        Vector2 perturbedSpeed = new Vector2(0, -3).RotatedByRandom(MathHelper.ToRadians(150));
                        float scale = 1f - (Main.rand.NextFloat() * .5f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(null, new Vector2(player.Center.X, player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<GlassShardProj>(), player.HeldItem.damage / 3, 0, player.whoAmI);
                    }
                }
            }
        }
    }
    public class Walls : GlobalWall
    {
        //break glass hammer on walls
        int breakchance;
        public override void KillWall(int i, int j, int type, ref bool fail)
        {
            var player = Main.LocalPlayer;

            if (!GetInstance<ConfigurationsGlobal>().GlassSoWeak)
                breakchance = (int)(64 * (1 + player.luck));
            else
                breakchance = 1;

            if (player.HeldItem.type == ModContent.ItemType<GlassHammer>())
            {
                if (Main.rand.Next(breakchance) == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0f }, player.Center);

                    player.HeldItem.TurnToAir();
                    player.QuickSpawnItem(null, ItemType<Items.Tools.GlassHandle>(), 1);
                    int shardcount = Main.rand.Next(5, 7); //5-6 shards
                    for (int k = 0; k < shardcount; k++) //summon dust and projectile
                    {
                        var dust = Dust.NewDustDirect(new Vector2(player.position.X, player.position.Y), player.width, player.height, 149, 0, 0);

                        Vector2 perturbedSpeed = new Vector2(0, -3).RotatedByRandom(MathHelper.ToRadians(150));
                        float scale = 1f - (Main.rand.NextFloat() * .5f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(null, new Vector2(player.Center.X, player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<GlassShardProj>(), player.HeldItem.damage / 3, 0, player.whoAmI);
                    }
                }
            }
        }
    }
    //_____________________________________________________
    public class SpinnerReflect : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool reflected;
        int distance;
        public override void AI(Projectile projectile)
        {
            var player = Main.LocalPlayer;
            //var projreflect = Main.projectile[mod.ProjectileType("SelenianBladeProj")];

            //if (Main.LocalPlayer.HasBuff(BuffType<SelenianBuff>()))
            if (projectile.aiStyle != 20)
            {

                if (player.controlUseItem && player.HasBuff(ModContent.BuffType<ReflectedBuff>()))
                {
                    if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.TheSickle>()) //15 frame parry, 2.5 second cooldown
                        distance = 60;
                    if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.FrostSpinner>()) //20 frame parry, 2 second cooldown
                        distance = 80;
                    if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.TheScythe>()) //25 frame parry, 1.5 second cooldown
                        distance = 100;
                    if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.LunarSolarSpin>()) //30 frame parry, 0.5 second cooldown
                        distance = 120;

                    if (projectile.hostile)
                    {
                        if (projectile.aiStyle is 0 or 1 or 2 or 3 or 4 or 5 or 8 or 10 or 13 or 14 or 18 or 21 or 23 or 25)
                        {
                            //Player player = Main.player[npc.target];
                            for (int i = 0; i < 1; i++)
                            {
                                var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 66, projectile.velocity.X, projectile.velocity.Y);
                                dust.scale = 0.8f;
                                dust.noGravity = true;
                            }

                            if (Vector2.Distance(player.Center, projectile.Center) <= distance && !reflected)
                            {
                                int choice = Main.rand.Next(0, 1);
                                if (choice == 0)
                                {
                                    SoundEngine.PlaySound(SoundID.Item56, projectile.Center);
                                    //Projectile.Kill();
                                    if (!Main.dedServ)
                                    {
                                        projectile.owner = Main.myPlayer;
                                        projectile.velocity.X *= -1f;


                                        projectile.velocity.Y *= -1f;
                                        for (int i = 0; i < 1; i++)
                                        {
                                            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
                                            {
                                                PositionInWorld = projectile.Center,
                                            }, projectile.whoAmI);
                                        }

                                        projectile.friendly = true;
                                        projectile.hostile = false;
                                        //damage is in npceffects
                                        projectile.netUpdate = true;

                                    }
                                    reflected = true;
                                }
                                else
                                {
                                    reflected = true;
                                }
                            }

                        }

                    }
                }

            }
        }

    }
}