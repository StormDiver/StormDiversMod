using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Common;
using Terraria.GameContent.Drawing;
using StormDiversMod.Items.Potions;



//buff effects for npcs are in NPCEffects.cs, effects for player are in EquipmentEffects.cs
namespace StormDiversMod.Buffs
{
    public class FlaskFrostImbue : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Weapon Imbue: Frostburn");
            //Description.SetDefault("Melee and whip attacks inflict enemies with Frostburn");
            BuffID.Sets.IsAFlaskBuff[Type] = true;
            Main.meleeBuff[Type] = true;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<FlaskEffects>().flaskFrost = true;

        }
    }
    public class FlaskExplosiveImbue : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Weapon Imbue: Explosive");
            //Description.SetDefault("Melee and whip attacks create a small explosion");
            BuffID.Sets.IsAFlaskBuff[Type] = true;
            Main.meleeBuff[Type] = true;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<FlaskEffects>().flaskExplosive = true;
            player.GetModPlayer<FlaskEffects>().FlaskExplosive = "true";

        }
    }
}