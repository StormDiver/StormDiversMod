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
    public class ConfigurationsGlobal : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Enemy Spawning")]

        [Label("Prevent modded pillar enemies from spawning")]
        [Tooltip("This will prevent the new pillar enemies in this mod from spawning")]
        //[ReloadRequired] //No reload required as it just changes the spawn chance and doesn't disable the enemy itself
        [DefaultValue(false)]
        public bool PreventPillarEnemies { get; set; }

        [Label("Disable Moonlings from naturally spawning")]
        [Tooltip("Prevent Moonlings from spawning naturally in space post-Moon Lord, they can still be summoned via Moonling core")]
        //[ReloadRequired] //No
        [DefaultValue(false)]
        public bool NoMoonling4U { get; set; }

        [Label("Disable Temple Guardians pre Plantera")]
        [Tooltip("Prevent Temple Guardians from spawning when entering the temple pre-Plantera (Not recommended)")]
        //[ReloadRequired] //Sadly no
        [DefaultValue(false)]
        public bool SmellyPlayer { get; set; }

        [Header("Enemy Misc")]

        [Label("Overloaded Scandrone acts as a Plantera Alternative")]
        [Tooltip("This will make defeating Overloaded Scandrone activate everything that defeating the Plantera would")]
        [DefaultValue(false)]
        //[ReloadRequired] //No reload required as it just prevents a single bool being activated
        public bool StormBossSkipsPlant { get; set; }

        [Label("Disable buffed Derplings")]
        [Tooltip("This will prevent Derplings from gaining massively increased stats post-plantera")]
        //[ReloadRequired] //No reload required 
        [DefaultValue(false)]
        public bool PreventBuffedDerps { get; set; }       

        //This will be added if Rho's Playground is ever deleted
        /*[Label("Remove damage variance")]
        [Tooltip("This will remove the random spread in all damage dealt and taken (Requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool NoDamageSpread { get; set; }*/

    }
}