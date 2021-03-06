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


namespace StormDiversMod.Basefiles
{
    public class ConfigurationsIndividual : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [DisplayName("Local Configurations")]
        [Header("Visual")]

        [Label("Disable screen shake effects")]
        [Tooltip("Disables the screen shake that a few items in this mod make")]
        //[ReloadRequired] //None required
        [DefaultValue(false)]
        public bool NoShake { get; set; }
       
    }
}