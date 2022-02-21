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

namespace StormDiversMod
{
	public class StormDiversMod : Mod
	{
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
                ItemID.SailfishBoots
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
        }
    }
}