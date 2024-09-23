using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;


namespace StormDiversMod.Common
{
    //[Label("Local Configurations")]
    [BackgroundColor(5, 51, 34, 200)]

    public class ConfigurationsIndividual : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Header("Visual")]

        //[Label("Disable screen shake effects")]
        //[Tooltip("Disables the screen shake that a few items in this mod make")]
        //[ReloadRequired] //None required
        //[DefaultValue(false)]
        //[BackgroundColor(27, 130, 114)]
        //public bool NoShake { get; set; }

        //[Label("Screenshake intensity")]
        //[Tooltip("Change how intense the screenshake effect is, set to zero to disable it (defaults to 5)")]
        [Range(0, 10)]
        [Slider]
        [DefaultValue(5)]
        [BackgroundColor(27, 130, 114)]
        public int ShakeAmount { get; set; }

        //[Label("Disable certain chat messages")]
        //[Tooltip("Disables certain chat messages that appear from bosses/certain items, note that Status messages for world events will still be displayed")]
        //[ReloadRequired] //None required
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoMessage { get; set; }

        [Header("Audio")]

        //[Label("Disable text to speech sounds")]
        //[Tooltip("Disables the text to speech sounds some secret items make")]
        //[ReloadRequired] //None required
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoPain { get; set; }
    }
}