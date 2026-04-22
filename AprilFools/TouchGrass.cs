using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using ReLogic.Graphics;
using ReLogic.Peripherals.RGB;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using StormDiversMod.Items.Accessory;
using StormDiversMod.Items.Armour;
using StormDiversMod.Items.Furniture;
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
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.WorldBuilding;
using static System.Net.WebRequestMethods;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.AprilFools
{
    //saves
    public class TouchGrassEnabled : ModSystem
    {
        public static bool TouchGrassMode;
        public override void OnWorldLoad()
        {
            TouchGrassMode = false;
        }
        public override void OnWorldUnload()
        {
            TouchGrassMode = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            if (TouchGrassMode)
            {
                tag["TouchGrassMode"] = true;
            }
        }
        public override void LoadWorldData(TagCompound tag)
        {
            TouchGrassMode = tag.ContainsKey("TouchGrassMode");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flagsgrass = new BitsByte(); //Misc (8 max)

            flagsgrass[0] = TouchGrassMode;

            writer.Write(flagsgrass);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flagsgrass = reader.ReadByte();

            TouchGrassMode = flagsgrass[0];
        }
        public override void PreUpdateWorld()
        {
            if (!GetInstance<ConfigurationsGlobal>().AFTouchGrass) //disable mode if config is disabled
                TouchGrassMode = false;
        }
    }
    //mainsystem
    public class TouchGrass : ModPlayer
    {
        int grasstimer = 60 * 300; // 5 minutes
        public override void UpdateDead()//Reset all ints and bools if dead======================
        {
            if (TouchGrassEnabled.TouchGrassMode)
                grasstimer = 60 * 300;
        }
        public override void PostUpdateEquips() //Updates every frame
        {
            //TOUCH GRASS
            if (TouchGrassEnabled.TouchGrassMode)
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
            }
        }
    }

    //items
    public class TouchGrassSeed : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Special Grass Seed");
            //Tooltip.SetDefault("Use to activate Touch Grass mode for this world
            //In this mode you must stand on any grass block every 5 minutes or face the consequences
            //While standing on grass your damage dealt is increased by 20 % and damage taken is reduced by 20 %
            //This cannot be undone, use at your own risk");
            Item.ResearchUnlockCount = 0;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 20;
            Item.height = 32;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useTurn = false;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (GetInstance<ConfigurationsGlobal>().AFTouchGrass)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip3" && ItemSlot.ShiftInUse)
                    {
                        line.Text = "[c/31cb31:50% increased damage]\n[c/31cb31:25% increased crit chance]\n[c/31cb31:33% damage reduction]\n[c/31cb31:Greatly increases move speed and acceleration]\n[c/31cb31:Allows you to dash]";
                    }
                    if (TouchGrassEnabled.TouchGrassMode)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                        {
                            line.Text = "[c/005000:Touch Grass Mode has already been enabled!]";
                        }
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip5")
                    {
                        line.Text = "[c/ff8c00:" + line.Text + "]";
                    }
                }
                else
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "[c/7D7D7D:Touch Grass Disabled via config]";
                    }
                    if (line.Mod == "Terraria" && (line.Name == "Tooltip1" || line.Name == "Tooltip2" || line.Name == "Tooltip3" || line.Name == "Tooltip4"))
                    {
                        line.Text = "";
                    }
                }
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool CanUseItem(Player player)
        {
            if (TouchGrassEnabled.TouchGrassMode == false && GetInstance<ConfigurationsGlobal>().AFTouchGrass)
                return true;
            else
                return false;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 100; i++)
                {
                    float speedX = 0f;
                    float speedY = -5f;

                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(new Vector2(player.Center.X + (5 * player.direction), player.Center.Y - 22 * player.gravDir), 0, 0, 3, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);
                    Main.dust[dust2].noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.Grass, player.Center);

                if (!TouchGrassEnabled.TouchGrassMode)
                {
                    TouchGrassEnabled.TouchGrassMode = true;
                    Main.NewText("Touch Grass mode has been enabled", Color.LimeGreen);
                    CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.LimeGreen, "Touch Grass mode has been enabled!", false);
                    player.QuickSpawnItem(null, ItemID.RecallPotion, 10);
                    player.QuickSpawnItem(null, ItemID.PotionOfReturn, 3);
                }
                else //unused
                {
                    TouchGrassEnabled.TouchGrassMode = false;
                    Main.NewText("Touch Grass mode has been disabled", Color.DarkGreen);
                }
            }
            return true;
        }
        //failsafe incase accidentally activated
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            if (TouchGrassEnabled.TouchGrassMode)
            {
                TouchGrassEnabled.TouchGrassMode = false;
                Main.NewText("Touch Grass mode has been disabled", Color.DarkGreen);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
        public override void AddRecipes()
        {
            if (GetInstance<ConfigurationsGlobal>().AFTouchGrass)
            {
                CreateRecipe()
                .AddTile(TileID.Grass)
                .Register();
            }
        }
    }

    public class ABladeofGrass : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("A Blade of Grass"); 
            //Tooltip.SetDefault("Literally just that, what did you expect\nDoes not count as touching real grass");
            Item.ResearchUnlockCount = 0;
        }
        public override void SetDefaults()
        {
            Item.damage = 1;

            Item.DamageType = DamageClass.Generic;
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Grass;
            Item.autoReuse = true;
            Item.knockBack = 0;
            Item.noMelee = false;
            Item.shootSpeed = 0.1f;
            Item.scale = 0.9f;
            Item.shoot = 1;
        }
        public override void AddRecipes()
        {
            if (GetInstance<ConfigurationsGlobal>().AFTouchGrass)
            {
                CreateRecipe()
                .AddTile(TileID.Grass)
                .Register();
            }
        }
    }
    //stop cheating
    public class Regrowth : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            if (TouchGrassEnabled.TouchGrassMode)
            {
                int xtilepos = (int)(Main.MouseWorld.X) / 16;
                int ytilepos = (int)(Main.MouseWorld.Y) / 16;
                Tile cursornormal = Main.tile[xtilepos, ytilepos];
                Tile cursorsmart = Main.tile[Main.SmartCursorX, Main.SmartCursorY];
                if (TouchGrassEnabled.TouchGrassMode)
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
            }
            return base.CanUseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (TouchGrassEnabled.TouchGrassMode)
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (item.type is ItemID.GrassSeeds or ItemID.CorruptSeeds or ItemID.CrimsonSeeds or ItemID.MushroomGrassSeeds or ItemID.JungleGrassSeeds or ItemID.HallowedSeeds or ItemID.AshGrassSeeds)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Placeable")
                        {
                            line.Text = "[c/31cb31:Nope, can't let you off that easy ;)]";
                        }
                    }
                    if (item.type is ItemID.StaffofRegrowth or ItemID.AcornAxe)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "[c/31cb31:Did you really think I'd allow this?]";
                        }
                    }
                }
            }
            base.ModifyTooltips(item, tooltips);
        }
    }
    //dialogue
    public class NPCGrass : GlobalNPC
    {
        public override void GetChat(NPC npc, ref string chat)
        {
            if (TouchGrassEnabled.TouchGrassMode)
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
        }
    }
    //buffs
    public class TouchGrassBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Touching Grass");
            //Description.SetDefault("See, it's not that bad, damage and defense increased");
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.5f;
            player.GetCritChance(DamageClass.Generic) += 25;
            player.endurance += 0.33f;
            player.dashType = 1;

            player.maxRunSpeed += 1.5f;
            player.runAcceleration *= 1.3f;
            player.runSlowdown = 0.2f;

            if (Main.rand.Next(5) == 0)
            {
                if (player.gravDir == 1)
                {
                    int dust = Dust.NewDust(new Vector2(player.position.X, player.Bottom.Y - 4), player.width, 2, 3, 0, -5, 100, default, 1.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.75f;
                }
                else
                {
                    int dust = Dust.NewDust(new Vector2(player.position.X, player.Top.Y - 2), player.width, 2, 3, 0, 5, 100, default, 1.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.75f;
                }
            }
        }
    }
    public class TouchGrassDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Touch Grass!");
            //Description.SetDefault("This is a threat");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }

    public class TouchGrassItems : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModContent.ItemType<TouchGrassSeed>() || item.type == ModContent.ItemType<ABladeofGrass>())
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "ItemName")
                    {
                        line.Text = line.Text + "\n[c/31cb31:APRIL FOOLS!!!]";
                    }
                }
            }
        }
    }
}