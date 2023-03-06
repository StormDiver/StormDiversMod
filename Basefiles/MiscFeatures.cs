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

            if (explosionfall)//Coorect fall damage when launched via sticky bomb
            {
                if (Player.velocity.Y > 0)
                {
                    Player.fallStart = (int)Player.tileTargetY;
                    //Main.NewText("plswork" + Player.tileTargetY, 204, 101, 22);

                    explosionfall = false;
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
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
                    ninelivescooldown = 600; //Reset cooldown to 10 seconds, even at max amount
                    if (ninelives < 9) //Spawn up to 9
                    {
                        ninelives++;//increase counter

                        int nineproj = Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<TheSickleProj3>(), 0, 0, Player.whoAmI, 0, ninelives - 1);//changes ai[1] field for different angles
                       
                        SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1f, Pitch = -1f, MaxInstances = 0 }, target.Center); ;

                        for (int i = 0; i < 20; i++)
                        {
                            var dust = Dust.NewDustDirect(target.position, target.width, target.height, 31);
                            dust.noGravity = true;
                            dust.velocity *= 3;
                            dust.scale = 1.5f;

                        }
                        //Main.NewText("" + ninelives, 204, 101, 22);
                    }
                }
                if ((target.type == ModContent.NPCType<NPCs.HellMiniBoss>()) && target.life <= 0) //Soul Cauldron give all souls
                {
                    ninelivescooldown = 600; //Reset cooldown to 10 seconds, even at max amount
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
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (playerimmunetime > 0)
            {
                return false;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
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