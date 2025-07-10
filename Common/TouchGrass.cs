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
using ReLogic.Peripherals.RGB;
using StormDiversMod.Items.Furniture;
using StormDiversMod.Items.Weapons;
using StormDiversMod.Projectiles.Minions;
using StormDiversMod.Items.Armour;
using StormDiversMod.Projectiles.Petprojs;
using StormDiversMod.NPCs.Boss;
using Terraria.GameContent.Creative;
using Terraria.ModLoader.Config;
using System.Drawing.Drawing2D;
using StormDiversMod.Items.Accessory;
using static System.Net.WebRequestMethods;

namespace StormDiversMod.Common
{
    public class TouchGrass : ModPlayer
    {
        //int grasstimer = 60 * 300; // 5 minutes
        public override void ResetEffects() //Resets bools if the item is unequipped
        {
           
        }
        public override void UpdateDead()//Reset all ints and bools if dead======================
        {
            //grasstimer = 60 * 300;
        }
        //===============================================================================================================
        public override bool PreItemCheck()
        {
            return base.PreItemCheck();
        }
        public override void ModifyScreenPosition()//screenshaker
        {
           
        }
        public override void PostUpdateEquips() //Updates every frame
        {
            /*if (StormWorld.TouchGrassMode)
            {
                var tilePos = Player.Bottom.ToTileCoordinates16();
                var tileposgrav = Player.Top.ToTileCoordinates16();
                //if (Player.controlUseTile)
                //    grasstimer = 60 * 12;

                if ((Framing.GetTileSafely(tilePos.X, tilePos.Y).TileType is TileID.Grass or TileID.GolfGrass or TileID.HallowedGrass or TileID.GolfGrassHallowed or TileID.AshGrass or TileID.CorruptGrass or TileID.CrimsonGrass or TileID.JungleGrass or TileID.MushroomGrass
                    or TileID.CorruptJungleGrass or TileID.CrimsonJungleGrass)
                    || (Player.gravDir == -1 && Framing.GetTileSafely(tileposgrav.X, tileposgrav.Y - 1).TileType is TileID.Grass or TileID.GolfGrass or TileID.HallowedGrass or TileID.GolfGrassHallowed or TileID.AshGrass or TileID.CorruptGrass or TileID.CrimsonGrass
                    or TileID.JungleGrass or TileID.MushroomGrass or TileID.CorruptJungleGrass or TileID.CrimsonJungleGrass))//When on grass 
                {
                    //Main.NewText("Touching grass " + grasstimer, Color.Green);
                    grasstimer = 60 * 300;
                    Player.AddBuff(ModContent.BuffType<TouchGrassBuff>(), 2);
                    Player.ClearBuff(ModContent.BuffType<TouchGrassDebuff>());
                }
                else
                {
                    //Main.NewText("Not touching grass " + (grasstimer / 60 + 1), Color.Red);
                    Player.AddBuff(ModContent.BuffType<TouchGrassDebuff>(), grasstimer);
                    Player.ClearBuff(ModContent.BuffType<TouchGrassBuff>());

                    if (grasstimer > 0)
                        grasstimer--;
                }
                if (grasstimer == 60 * 120) //2 minutes left
                {
                    CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.Green, "Might wanna touch grass soon!", true);
                    SoundEngine.PlaySound(SoundID.Item140 with { Volume = 1, Pitch = -0f }, Player.Center);
                }
                if (grasstimer == 60 * 60) //1 minute left
                {
                    CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.Yellow, "I really need to touch grass!", true);
                    SoundEngine.PlaySound(SoundID.Item140 with { Volume = 1, Pitch = -0f }, Player.Center);
                }
                if (grasstimer == 60 * 30) //30 seconds left
                {
                    CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.Orange, "If I don't touch grass ASAP bad things will happen!", true);
                    SoundEngine.PlaySound(SoundID.Item140 with { Volume = 1, Pitch = -0f }, Player.Center);

                }
                if (grasstimer == 60 * 10) //10 seconds left
                {
                    CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.Red, "HURRY!", true);
                    SoundEngine.PlaySound(SoundID.Item144 with { Volume = 1, Pitch = -0f }, Player.Center);
                }
                if (grasstimer > 0 && grasstimer <= 60 * 9 && grasstimer % 60 == 0) //10 second count down
                {
                    CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.Red, grasstimer / 60, true);
                    SoundEngine.PlaySound(SoundID.Item144 with { Volume = 1, Pitch = -0f }, Player.Center);
                }
                if (grasstimer == 1)
                    SoundEngine.PlaySound(SoundID.Item145 with { Volume = 1f, Pitch = -0f }, Player.Center);

                if (grasstimer == 0)
                {
                    Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " didn't touch grass"), 99999, 0, false);
                }
            }
            else
            {
                grasstimer = 60 * 300;
                Player.ClearBuff(ModContent.BuffType<TouchGrassDebuff>());
            }*/
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            base.ModifyHurt(ref modifiers);
        }
      
        public override void OnHurt(Player.HurtInfo info)
        {
            base.OnHurt(info);
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            
        }
    }
    public class Regrowth : GlobalItem
    {
        /*public override bool CanUseItem(Item item, Player player)
        {
            int xtilepos = (int)(Main.MouseWorld.X) / 16;
            int ytilepos = (int)(Main.MouseWorld.Y) / 16;
            Tile cursornormal = Main.tile[xtilepos, ytilepos];
            Tile cursorsmart = Main.tile[Main.SmartCursorX, Main.SmartCursorY];
            if (StormWorld.TouchGrassMode)
            {
                if (item.type is ItemID.GrassSeeds or ItemID.CorruptSeeds or ItemID.CrimsonSeeds or ItemID.MushroomGrassSeeds or ItemID.JungleGrassSeeds or ItemID.HallowedSeeds or ItemID.AshGrassSeeds)
                    return false;

                if (item.type is ItemID.StaffofRegrowth or ItemID.AcornAxe && 
                    ((!Main.tileAxe[cursornormal.TileType] && Main.tileSolid[cursornormal.TileType]) ||
                    (Main.SmartCursorIsUsed && !Main.tileAxe[cursorsmart.TileType] && Main.tileSolid[cursorsmart.TileType])))
                    return false;
                else
                    return true;
            }
                return base.CanUseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (item.type is ItemID.GrassSeeds or ItemID.CorruptSeeds or ItemID.CrimsonSeeds or ItemID.MushroomGrassSeeds or ItemID.JungleGrassSeeds or ItemID.HallowedSeeds or ItemID.AshGrassSeeds && StormWorld.TouchGrassMode)
                {
                    if (line.Mod == "Terraria" && line.Name == "Placeable")
                    {
                        line.Text = "[c/31cb31:Nope, can't let you off that easy ;)]";
                    }
                }
                if (item.type is ItemID.StaffofRegrowth or ItemID.AcornAxe && StormWorld.TouchGrassMode)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "[c/31cb31:Did you really think I'd allow this?]";
                    }
                }
            }
            base.ModifyTooltips(item, tooltips);
        }*/
    }
    public class NPCGrass : GlobalNPC
    {
        /*public override void GetChat(NPC npc, ref string chat)
        {
            if (StormWorld.TouchGrassMode)
            {
                switch (Main.rand.Next(5))
                {
                    case 0:
                        chat = "Touch grass!";
                        break;
                    case 1:
                        chat = "Have you touched grass recently?";
                        break;
                    case 2:
                        chat = "There's this thing called grass, you should touch it!";
                        break;
                    case 3:
                        chat = "Haven't touched grass yet? Skill issue!";
                        break;
                    case 4:
                        chat = "Will you ever touch grass? I mean in real life...";
                        break;
                }
            }
        }*/
    }
}