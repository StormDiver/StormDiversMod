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

namespace StormDiversMod
{
	public class StormDiversMod : Mod //For most important things
	{
        public override void PostSetupContent() //For boss checklist
        {
            if (ModLoader.HasMod("BossChecklist"))//DON'T FORGET THIS!!!!!!!
            {
                Mod bossChecklist = ModLoader.GetMod("BossChecklist");
                if (bossChecklist != null)
                {
                    bossChecklist.Call
                        ("AddBoss", this, "Overloaded Scandrone", ModContent.NPCType<NPCs.StormBoss>(), 11.5f, (Func<bool>)(() => StormWorld.stormBossDown), () => true,
                        new List<int> { ModContent.ItemType<Items.BossTrophy.StormBossTrophy>(), ModContent.ItemType<Items.BossTrophy.StormBossRelic>(), ModContent.ItemType<Items.Pets.StormBossPetItem>(),
                        ModContent.ItemType<Items.BossTrophy.StormBossBag>(), ModContent.ItemType<Items.Accessory.StormCoil>(), //vanity
                        //other
                        //ModContent.ItemType<Items.Weapons.StormKnife>(), ModContent.ItemType<Items.Weapons.StormLauncher>(), ModContent.ItemType<Items.Weapons.StormStaff>(), ModContent.ItemType<Items.Weapons.StormSentryStaff>(),
                        ModContent.ItemType<Items.Vanitysets.BossMaskStormBoss>()}, //ModContent.ItemType<Items.Tools.StormHook>(), ModContent.ItemType<Items.Accessory.StormWings>(),ItemID.TempleKey},

                        ModContent.ItemType<Items.Summons.StormBossSummoner>(), "Spawned by using a Storm Beacon once all 3 mechs have been defeated",
                        "Overloaded Scandrone returns to its home planet (and didn't die on the way home",
                        (SpriteBatch sb, Rectangle rect, Color color) => {
                            Texture2D texture = ModContent.Request<Texture2D>("StormDiversMod/NPCs/StormBoss_Image").Value;
                            Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                            sb.Draw(texture, centered, color);
                        }
                        );
                    // Additional bosses here
                }
            }
        }

        public static ModKeybind ArmourSpecialHotkey;
        public override void Load()
        {
            
            {
                ArmourSpecialHotkey = KeybindLoader.RegisterKeybind(this, "Armor Special Ability", "V");
                //if (StormDiversMod.ArmourSpecialHotkey.JustPressed) 
            }
        }
        public override void Unload()
        {
            ArmourSpecialHotkey = null;
        }
        public StormDiversMod()
		{

		}
        public override void AddRecipeGroups() //Recipe Groups
        {
            //recipe.AddRecipeGroup("StormDiversMod:EvilBars", 10);


            RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil Bar", new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar
            });
            RecipeGroup.RegisterGroup("StormDiversMod:EvilBars", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Golden Bar", new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            });
            RecipeGroup.RegisterGroup("StormDiversMod:GoldBars", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Golden Ore", new int[]
            {
                ItemID.GoldOre,
                ItemID.PlatinumOre
            });
            RecipeGroup.RegisterGroup("StormDiversMod:GoldOres", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil Material", new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample
            });
            RecipeGroup.RegisterGroup("StormDiversMod:EvilMaterial", group);
    
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Running Boots", new int[]
            {
                ItemID.HermesBoots,
                ItemID.FlurryBoots,
                ItemID.SailfishBoots,
                ItemID.SandBoots
            });
            RecipeGroup.RegisterGroup("StormDiversMod:RunBoots", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Anvil", new int[]
            {
                ItemID.IronAnvil,
                ItemID.LeadAnvil,
              
            });
            RecipeGroup.RegisterGroup("StormDiversMod:Anvils", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Tier 2 Hardmode Bar", new int[]
           {
                ItemID.MythrilBar,
                ItemID.OrichalcumBar,

           });
            RecipeGroup.RegisterGroup("StormDiversMod:MidHMBars", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Tier 3 Hardmode Bar", new int[]
           {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar,

           });
            RecipeGroup.RegisterGroup("StormDiversMod:T3HMBars", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Tombstone", new int[]
          {
                ItemID.Tombstone,
                ItemID.GraveMarker,
                ItemID.CrossGraveMarker,
                ItemID.Headstone,
                ItemID.Gravestone,
                ItemID.Obelisk,
                ItemID.RichGravestone1,
                ItemID.RichGravestone2,
                ItemID.RichGravestone3,
                ItemID.RichGravestone4,
                ItemID.RichGravestone5,
          });
            RecipeGroup.RegisterGroup("StormDiversMod:Tombstones", group);


            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Mech Soul", new int[]
           {
                ItemID.SoulofMight,
                ItemID.SoulofSight,
                ItemID.SoulofFright,

           });
            RecipeGroup.RegisterGroup("StormDiversMod:MechSoul", group);
        }
    }
    /*public class immunity : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileID.TerrarianBeam)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 10;
            }
        }
    }*/
}