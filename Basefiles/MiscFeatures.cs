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
using StormDiversMod.Dusts;
using Terraria.DataStructures;
using Terraria.Audio;

namespace StormDiversMod.Basefiles
{

    public class MiscFeatures : ModPlayer
    {
        public bool screenshaker; //Weapons that shake the screen
        public int shaketimer; //How long to shake the screen for

        public int templeWarning; //Warning until Temple Guardians spawn
       
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
                Main.screenPosition += new Vector2(Main.rand.Next(-7, 7), Main.rand.Next(-7, 7));
                shaketimer--;
            }

        }
        public override void PostUpdateEquips() //Updates every frame
        {
            //Detect if player is in Temple and immediatly summon up to 12 Guardians
            if (Player.ZoneJungle && Player.ZoneRockLayerHeight && !NPC.downedPlantBoss) //This code is onyl active when certain cirteia is met, sadly the zonelizardtemple doesn't work
            {
                int xtilepos = (int)(Player.position.X + (float)(Player.width / 2)) / 16;
                int ytilepos = (int)(Player.position.Y + (float)(Player.height / 2)) / 16;

                if (Main.tile[xtilepos, ytilepos].WallType == WallID.LihzahrdBrickUnsafe)
                {
                    if (NPC.CountNPCS(ModContent.NPCType<GolemMinion>()) < 12)
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
                }
            }
            else
            {
                templeWarning = 0;
            }
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
    }
}