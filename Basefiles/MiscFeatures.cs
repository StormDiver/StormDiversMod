using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

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
using StormDiversMod.Buffs;
using StormDiversMod.NPCs;
using StormDiversMod.Projectiles;

using Terraria.DataStructures;
using Terraria.Audio;
using System.Configuration;
using Terraria.GameContent.Drawing;
using StormDiversMod.Items.Vanitysets;
using Terraria.GameContent.ItemDropRules;
using rail;
using Terraria.WorldBuilding;

namespace StormDiversMod.Basefiles
{

    public class MiscFeatures : ModPlayer
    {
        public bool screenshaker; //Weapons that shake the screen
        public int shaketimer; //How long to shake the screen for

        public int templeWarning; //Warning until Temple Guardians spawn

        public int playerimmunetime; //makes player immune to damage

        public int ninelives; //how many kills with the sickle, up to 9
        public int ninelivescooldown; //cooldown to remove a soul
        public float ninedmg;  //increase in melee damage
        public bool explosionfall; //Player has been launched by a stickybomb
        public int explosionflame; //How long to have flames under the player's feet after being launched

        public override void ResetEffects() //Resets bools if the item is unequipped
        {
            screenshaker = false;
        }
        public override void UpdateDead()//Reset all ints and bools if dead======================
        {
            templeWarning = 0;
            shaketimer = 0;
            ninelives = 0;
            ninelivescooldown = 0;
            ninedmg = 0;
            explosionfall = false;
        }

        //===============================================================================================================
        public override bool PreItemCheck()
        {
            //Alt fire autouse
            if ((
                Player.HeldItem.type == ModContent.ItemType<Items.Weapons.LightDarkSword>() ||
                Player.HeldItem.type == ModContent.ItemType<Items.Weapons.LunarVortexLauncher>() ||
                //Player.HeldItem.type == ModContent.ItemType<Items.Weapons.LunarVortexShotgun>() ||
                Player.HeldItem.type == ModContent.ItemType<Items.Weapons.ShroomiteLauncher>()
                )
                && Player.altFunctionUse == 2 && Player.controlUseTile && Player.itemAnimation == 1)
            {
                Player.itemAnimationMax = Player.itemAnimation = Player.HeldItem.useAnimation;
            }
            return base.PreItemCheck();
        }
        public override void ModifyScreenPosition()//screenshaker
        {
            if (screenshaker)
            {
                shaketimer = 10;
                screenshaker = false;
            }
            if (shaketimer > 0)
            {
                Main.screenPosition += new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5));
                shaketimer--;
            }
        }
        public override void PostUpdateEquips() //Updates every frame
        {
            //Detect if player is in Temple and immediatly summon up to 12 Guardians

            if (Player.ZoneJungle && Player.ZoneRockLayerHeight && !NPC.downedPlantBoss) //This code is only active when certain criteia is met, sadly the zonelizardtemple doesn't work
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
            }
            if (playerimmunetime > 0)
            {
                playerimmunetime--;
            }
            if (Player.GetModPlayer<EquipmentEffects>().bootFall == false)
            {
                playerimmunetime = 0;
            }

            if (ninelivescooldown > 0)
            {
                ninelivescooldown--;
            }
            if (ninelivescooldown == 0)
            {
                if (ninelives > 0) //once cooldown is met remove 1 soul
                {
                    ninelives--;
                    ninelivescooldown = 90; //  1.5 seconds per soul

                    //Main.NewText("" + ninelives, 204, 101, 22);
                }
            }
            //Main.NewText("ninelivescooldown" + ninelivescooldown, 204, 101, 22);
            if (Player.HeldItem.type == ModContent.ItemType<Items.Weapons.TheSickle>())
            {
                Player.GetDamage(DamageClass.Melee) += 0.05f * ninelives; //increase damage from 5-45%
                Player.GetCritChance(DamageClass.Melee) += 2 * ninelives; //increase crit from 2-18%

            }
            /*if (ninelives == 9)
            {
                Player.GetDamage(DamageClass.Melee) += 0.1f; //extra 10% damage (55% total)
                Player.GetCritChance(DamageClass.Melee) += 5; //extra 5% crit (32% total)
            }*/
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
            //Main.NewText("Pain = " + (100 - (Player.statDefense * 0.75f) * (1 - Player.endurance)), 204, 101, 22);
            
            if (NPC.CountNPCS(ModContent.NPCType<NPCs.Boss.ThePainBoss>()) == 0)
            {
                Player.ClearBuff(ModContent.BuffType<YouCantEscapeDebuff>()); 
            }
            //Main.NewText("Pain is " + paintime, 220, 63, 139);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        { 
        //public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        
            if (proj.type == ModContent.ProjectileType<StompBootProj2>() && target.type != NPCID.TargetDummy) //10 frames of immunity
            {
                playerimmunetime = 10;
            }
            //nine lives
            if (proj.type == ModContent.ProjectileType<TheSickleProj>() || proj.type == ModContent.ProjectileType<TheSickleProj2>())
            {          
                //regular enemies 1 soul 
                if (!target.SpawnedFromStatue && !target.dontTakeDamage && !target.friendly && target.lifeMax > 5 && target.type != NPCID.TargetDummy && target.life <= 0)
                {
                    ninelivescooldown = 540; //Reset cooldown to 9 seconds, even at max amount
                    if (ninelives < 9) //Spawn up to 9
                    {
                        ninelives++;//increase counter

                        int nineproj = Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<TheSickleProj3>(), 0, 0, Player.whoAmI, 0, ninelives - 1);//changes ai[1] field for different angles
                       
                        SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1f, Pitch = -1f, MaxInstances = 0 }, target.Center); ;

                        /*for (int i = 0; i < 20; i++)
                        {
                            var dust = Dust.NewDustDirect(target.position, target.width, target.height, 31);
                            dust.noGravity = true;
                            dust.velocity *= 3;
                            dust.scale = 1.5f;

                        }*/
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
                if ((target.type == ModContent.NPCType<NPCs.HellMiniBoss>()) && target.life <= 0) //Soul Cauldron give all souls
                {
                    ninelivescooldown = 540; //Reset cooldown to 9 seconds, even at max amount
                    for (int i = 0; i < 8; i++) //Because you get one from normal means
                    {
                        if (ninelives < 9)
                        {
                            ninelives++;//increase counter
                            //Main.NewText("" + ninelives, 204, 101, 22);
                            Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<TheSickleProj3>(), 0, 0, Player.whoAmI, 0, ninelives - 1);//changes ai[1] field for different angles

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
            }
        }
        String Paintext = "";
        //public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
                
            if (proj.type == ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>() && !Player.immune)
            {
                //paintime = 3600;
                //Player.statDefense = 0; //ignores all DR
                //Player.endurance = 0;
                //damage = (Player.statLife / 3); //Deals 1/3 the player's
                if (Player.statLife < Player.statLifeMax2 / 3)//Deals 75% damage below 33% life
                {
                    modifiers.FinalDamage *= 0.75f;      
                }
            }
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            /*if (proj.type == ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj>())
            {
                if (Player.statLife < Player.statLifeMax2 && Player.statLife > 0) //No message if dead or revived
                {
                    int choice = Main.rand.Next(0, 6);

                    if (Main.rand.Next(1) == 0)
                    {
                        if (choice == 0)
                            Paintext = "That looked very Painful!";
                        else if (choice == 1)
                            Paintext = "Enjoy the pain!";
                        else if (choice == 2)
                            Paintext = "Are you enjoying this?";
                        else if (choice == 3)
                            Paintext = "How does the pain feel?";
                        else if (choice == 4)
                            Paintext = "Skill issue!";
                        else if (choice == 5)
                            Paintext = "You seem to be in a lot of pain!";
                        
                        for (int i = 0; i < 200; i++)//message also appears from boss
                        {
                            NPC painTarget = Main.npc[i];
                            if (painTarget.type == ModContent.NPCType<NPCs.Boss.ThePainBoss>())
                            {
                                CombatText.NewText(new Rectangle((int)painTarget.Center.X, (int)painTarget.Center.Y, 12, 4), Color.DeepPink, Paintext, true);
                            }
                        }
                        if (Main.netMode == 2) // Server
                        {
                            Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Paintext), new Color(175, 17, 96));
                        }
                        else if (Main.netMode == 0) // Single Player
                        {
                            Main.NewText(Paintext, 175, 17, 96);
                        }

                        //Player.QuickSpawnItem(null, ModContent.ItemType<ThePainMask>(), 1);

                    }
                }    
            }*/
        }
        String Suffertext;
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Player.HasBuff(ModContent.BuffType<YouCantEscapeDebuff>()) && !Player.HasBuff(ModContent.BuffType<PainBuff>())) //Save you from death once, won't activate if accessory does
            {
                Suffertext = "HOW CAN YOU SUFFER IF YOU'RE DEAD???";
                for (int i = 0; i < 200; i++)//message also appears from boss
                {
                    NPC painTarget = Main.npc[i];
                    if (painTarget.type == ModContent.NPCType<NPCs.Boss.ThePainBoss>())
                    {
                        CombatText.NewText(new Rectangle((int)painTarget.Center.X, (int)painTarget.Center.Y, 12, 4), Color.HotPink, Suffertext, true);
                    }
                }
                int proj = Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<NPCs.NPCProjs.ThePainBossProj2>(), 0, 0, Main.myPlayer);
                Main.projectile[proj].scale = 2.5f;
                SoundEngine.PlaySound(SoundID.Item74 with { Volume = 2f, Pitch = 0.5f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Player.Center);
                SoundEngine.PlaySound(SoundID.Item109 with { Volume = 1f, Pitch = 0f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Player.Center);

                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Suffertext), new Color(220, 63, 139));
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText(Suffertext, 220, 63, 139);
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
            if (playerimmunetime > 0)
            {
            }
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
            base.OnHurt(info);
        }        
    }
    public class Itemchanges : GlobalItem
    {
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

    }
}