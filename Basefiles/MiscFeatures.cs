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

namespace StormDiversMod.Basefiles
{

    public class MiscFeatures : ModPlayer
    {
        public bool screenshaker; //Weapons that shake the screen
        public int shaketimer; //How long to shake the screen for

        public int templeWarning; //Warning until Temple Guardians spawn

        public int playerimmunetime; //makes player immune to damage
        public override void ResetEffects() //Resets bools if the item is unequipped
        { 
            screenshaker = false;
        }
        public override void UpdateDead()//Reset all ints and bools if dead======================
        { 
            templeWarning = 0;
            shaketimer = 0;
        }

        //===============================================================================================================
        public override bool PreItemCheck()
        {
            //Alt fire autouse
            if ((
                Player.HeldItem.type == ModContent.ItemType<Items.Weapons.LightDarkSword>() ||
                Player.HeldItem.type == ModContent.ItemType<Items.Weapons.LunarVortexLauncher>() ||
                Player.HeldItem.type == ModContent.ItemType<Items.Weapons.LunarVortexShotgun>() ||
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
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (proj.type == ModContent.ProjectileType<StompBootProj2>() && target.type != NPCID.TargetDummy) //10 frames of immunity
            {
                playerimmunetime = 10;
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
      //Drill speeds
        public override void HoldItem(Item item, Player player)
        {
            if (GetInstance<ConfigurationsGlobal>().FastDrill4U)
            {
                
                float drillspeed = player.pickSpeed * 100;
                int drillspeed2 = (int)drillspeed;
                if (item.type == ItemID.CobaltDrill || item.type == ItemID.PalladiumDrill)
                {
                    item.useTime = 7 * drillspeed2 / 100;
                    //Main.NewText("Pick speed = " + 7 * drillspeed2 / 100, 47, 86, 146);
                }
                if (item.type == ItemID.MythrilDrill)
                {
                    item.useTime = 6 * drillspeed2 / 100;
                }
                if (item.type == ItemID.OrichalcumDrill)
                {
                    item.useTime = 5 * drillspeed2 / 100;
                }
                if (item.type == ItemID.AdamantiteDrill || item.type == ItemID.TitaniumDrill || item.type == ItemID.Drax || item.type == ItemID.ChlorophyteDrill)
                {
                    item.useTime = 4 * drillspeed2 / 100;
                }
                if (item.type == ItemID.LaserDrill)
                {
                    item.useTime = 6 * drillspeed2 / 100;
                }
                if (item.type == ItemID.SolarFlareDrill || item.type == ItemID.VortexDrill || item.type == ItemID.NebulaDrill || item.type == ItemID.StardustDrill)
                {
                    item.useTime = 2 * drillspeed2 / 100;
                }
                //mine
                if (item.type == ModContent.ItemType<Items.Tools.FastDrill>())
                {
                    item.useTime = 5 * drillspeed2 / 100;
                }
                if (item.type == ModContent.ItemType<Items.Tools.FastDrill2>())
                {
                    item.useTime = 4 * drillspeed2 / 100;
                }
                if (item.type == ModContent.ItemType<Items.Tools.DerplingDrill>() || item.type == ModContent.ItemType<Items.Tools.SpaceRockDrillSaw>())
                {
                    item.useTime = 3 * drillspeed2 / 100;
                }
                if (item.type == ModContent.ItemType<Items.Tools.SantankDrill>())
                {
                    item.useTime = 2 * drillspeed2 / 100;
                }
            }
        }
    }
}