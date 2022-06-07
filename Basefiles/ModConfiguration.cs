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
    public class Configurations : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Enemies")]
        [Label("Overloaded Scandrone acts as a Plantera Alternative")]
        [Tooltip("This will make defeating Overloaded Scandrone activate everything that defeating the Plantera would")]
        [DefaultValue(false)]
        //[ReloadRequired] //No reload required as it just prevents a single bool being activated
        public bool StormBossSkipsPlant { get; set; }
    
        [Label("Prevent modded pillar enemies from spawning")]
        [Tooltip("This will prevent the new pillar enemies in this mod from spawning")]
        //[ReloadRequired] //No reload required as it just changes the spawn chance and doesn't disable the enemy itself
        [DefaultValue(false)]
        public bool PreventPillarEnemies { get; set; }

        [Label("Disable buffed Derplings")]
        [Tooltip("This will prevent Derplings from gaining massively increased stats post-plantera")]
        //[ReloadRequired] //No reload required 
        [DefaultValue(false)]
        public bool PreventBuffedDerps { get; set; }

        [Header("Visual")]
        [Label("Disable screen shake effects")]
        [Tooltip("Disables the screen shake that a few items in this mod make")]
        //[ReloadRequired] //None required
        [DefaultValue(false)]
        public bool NoShake { get; set; }

        [Header("Misc")]
        [Label("Revert modded throwing weapons to throwing class")]
        [Tooltip("This will make all weapons in the mod that were previously throwing deal thrown damage (Requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool ThrowingTryhards { get; set; }

        //This will be added if Rho's Playground is ever deleted
        /*[Label("Remove damage variance")]
        [Tooltip("This will remove the random spread in all damage dealt and taken (Requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool NoDamageSpread { get; set; }*/

    }
}