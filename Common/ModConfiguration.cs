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
    //[Label("Global Configurations")]
    [BackgroundColor(5, 51, 34, 200)]

    public class ConfigurationsGlobal : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        [Header("EnemySpawning")]

        //[Label("Prevent Frost Legion enemies spawning in hardmode Blizzards")]
        //[Tooltip("This will prevent Frost legion enemeis from spawning during Blizzards in Hardmode")]
        //[ReloadRequired] //TBA
        //[DefaultValue(false)]
        //[BackgroundColor(27, 130, 114)]
        //public bool PreventSnowmenEnemies { get; set; }

        //[Label("Prevent modded pillar enemies from spawning")]
        //[Tooltip("This will prevent the new pillar enemies in this mod from spawning")]
        //[ReloadRequired] //No reload required as it just changes the spawn chance and doesn't disable the enemy itself
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool PreventPillarEnemies { get; set; }

        //[Label("Disable Moonlings from naturally spawning")]
        //[Tooltip("Prevent Moonlings from spawning naturally in space post-Moon Lord, they can still be summoned via Moonling core")]
        //[ReloadRequired] //No
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoMoonling4U { get; set; }

        //[Label("Disable Temple Guardians pre Plantera")]
        //[Tooltip("Prevent Temple Guardians from spawning when entering the temple pre-Plantera (Not recommended)")]
        //[ReloadRequired] //Sadly no
        //[DefaultValue(false)]
        //[BackgroundColor(27, 130, 114)]
        //public bool SmellyPlayer { get; set; }

        [Header("EnemyMisc")]

        //[Label("Overloaded Scandrone acts as a Plantera alternative")]
        //[Tooltip("This will make defeating Overloaded Scandrone activate everything that defeating the Plantera would")]
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        //[ReloadRequired] //No reload required as it just prevents a single bool being activated
        public bool StormBossSkipsPlant { get; set; }

        //[Label("Disable buffed Derplings")]
        //[Tooltip("This will prevent Derplings from gaining massively increased stats post-plantera")]
        //[ReloadRequired] //No reload required 
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool PreventBuffedDerps { get; set; }

        //[Label("Prevent Boss weather changes")]
        //[Tooltip("This will prevent bosses in the mod from changing the weather when alive")]
        //[ReloadRequired] //No reload required 
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool PreventBossStorm { get; set; }

        [Header("GenMisc")]

        //[Label("Prevent Extra Granite, Marble, and Mushroom chests from generating")]
        //[Tooltip("This will prevent extra chests from generating in the Granite, Marble, and Mushroom biomes in case of mod conflicts")]
        [ReloadRequired] 
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoChestforu { get; set; }

        //[Label("Prevent the Temple Curse for occuring")]
        //[Tooltip("This will prevent the Temple curse from activating if a Temple weapon is in your inventory, but doesn't prevent the damage when trying to use them")]
        [ReloadRequired] //No reload required 
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoScaryCurse { get; set; }
    }
}