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
using Terraria.WorldBuilding;
using StormDiversMod.Items.Accessory;

//buff effects for npcs are in NPCEffects.cs, effects for player are in EquipmentEffects.cs
namespace StormDiversMod.Buffs
{
    public class CelestialBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
          
            //DisplayName.SetDefault("Celestial Guardian");
            //Description.SetDefault("The powers of the Celestial spirits rapidly regenerate your life");
        }
        int particle = 10;
        public override void Update(Player player, ref int buffIndex)
        {
            particle--;
            {
                player.lifeRegen += 30;
                if (particle <= 0)
                {
                    particle = 10;
                    Dust.NewDustDirect(new Vector2(player.position.X + 5f, player.position.Y + 20f), 5, 5, 110);
                    Dust.NewDustDirect(new Vector2(player.position.X + 5f, player.position.Y + 20f), 5, 5, 111);
                    Dust.NewDustDirect(new Vector2(player.position.X + 5f, player.position.Y + 20f), 5, 5, 112);
                    Dust.NewDustDirect(new Vector2(player.position.X + 5f, player.position.Y + 20f), 5, 5, 174);

                }
            }
        }
    }
   
    //______________________________________________________________________________
    
    public class TurtleBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shelled");
            //Description.SetDefault("The power of the Turtle shell protects you");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 15;

            if (Main.rand.Next(10) < 3)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 273, player.velocity.X, player.velocity.Y, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
            }
        }
    }
    //_______________________________________________________________________________
    public class ShroomiteBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroomite Enhancement");
            //Description.SetDefault("Increases ranged crit damage by 10%");
        }
        // code in EquipmentEffects.cs
    }
    //_______________________________________________________________________________

    public class SpectreBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spectre Enhancement");
            //Description.SetDefault("Magic damage increased by 15% when below half mana");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.statMana < player.statManaMax2 * 0.5f)
            {
                player.GetDamage(DamageClass.Magic) += 0.2f;
            }
        }
    }
    //_______________________________________________________________________________
    public class SpectreStarBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spectre Star");
            //Description.SetDefault("Magic critical strike chance increased by X");
        }
        //int extracrit;
        float extradamage;
        public override void Update(Player player, ref int buffIndex)
        {
            int buffindex = player.FindBuffIndex(ModContent.BuffType<SpectreStarBuff>());
            if (buffindex > -1)
            {
                extradamage = player.buffTime[buffindex] + 1;
                //Main.NewText("The test = " + (extradamage / 12), 204, 101, 22);
                player.GetDamage(DamageClass.Magic) += (extradamage / 24) / 100; // 300 frames total, 25% damage at 600 frames, divide by 24 and by 100 again to get 1% damage every 24 frames (0.01)
            }
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
                tip = tip + " " + Math.Ceiling(extradamage / 24)  + "%"; //rounds up 
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
        }
    }
    //_______________________________________________________________________________

    public class BeetleBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Beetle Enhancement");
            //Description.SetDefault("20 armor penetration for melee weapons");
        }
        public override void Update(Player player, ref int buffIndex)
        {
                player.GetArmorPenetration(DamageClass.Melee) += 20;
            
        }
    }
    //_______________________________________________________________________________
    public class SpookyBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spooky Enhancement");
            //Description.SetDefault("Increases Minion damage and whip speed by 10%");
        }
        public override void Update(Player player, ref int buffIndex)
        {

            player.GetDamage(DamageClass.Summon) += 0.1f;
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.1f;
        }
    }
    //_______________________________________________________________________________

    public class RainBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sprinting in the rain");
            //Description.SetDefault("Your movement speed is increased");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.20f;
        }
    }
    //_______________________________________________________________________________

    public class FrozenBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Spiked");
            //Description.SetDefault("Your movement speed and critical strike chance are greatly increased");
            //Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

            player.runAcceleration *= 1.5f;
            player.maxRunSpeed += 2f;
            player.GetCritChance(DamageClass.Generic) += 10;
          
            
            
        }
    }
    //___________________________________________________________________
    public class HeartBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heart Collector");
            //Description.SetDefault("Enemies drop hearts more often on death");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            // player.statLifeMax2 += 40;
            player.GetModPlayer<EquipmentEffects>().heartpotion = true;
        }
    }
    //___________________________________________________________________
    public class FruitHeartBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Super Heart Collector");
            //Description.SetDefault("Enemies have a chance to drop a super heart on death");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            //player.statLifeMax2 += 50;
            player.GetModPlayer<EquipmentEffects>().superHeartpotion = true;
        }
    }
    //___________________________________________________________________
    public class HeartBarrierBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Life Barrier");
            //Description.SetDefault("The next incoming attack will be reduced by 25% and will deal no knockback");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.GetModPlayer<EquipmentEffects>().lifeBarrier = true;

            player.endurance += 0.25f;
            player.noKnockback = true;

            if (Main.rand.Next(10) < 3)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 72, player.velocity.X, player.velocity.Y, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
            }
        }
    }
    //_____________________________________________
    public class TeddyBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Teddy Bear Love");
            //Description.SetDefault("The love of the Teddy bear increases life regen");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 6;
            player.loveStruck = true; 
            //player.moveSpeed = 0.1f;
        }
    }
   
    //_____________________________________________
    public class GraniteBuff : ModBuff 
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Granite Barrier");
            //Description.SetDefault("Damage reduction, knockback immunity, and thorns effect, but lower movement and jump speed");
            //Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.5f;
            player.thorns = 1.2f;
            player.noKnockback = true;
            player.velocity.X *= 0.97f;
            player.runAcceleration *= 0.8f;
            // jump speed in armorsetbonuses
            if (Main.rand.Next(4) == 0)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 187, player.velocity.X, player.velocity.Y, 100, default, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
            }
        }
    }
    //_____________________________________________
    public class GraniteAccessBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Granite Surge");
            //Description.SetDefault("30% increased damage");
        }
        bool nodust;
        int accesstype = 3; //start at slot 3 (Accessory 1)
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.3f;

            for (int i = 0; i < 7; i++) //Go through all 7 accessory slots (slot 3 - 8)
            {
                if (player.armor[accesstype].type != ModContent.ItemType<GraniteCoreAccess>() && player.armor[accesstype].type != ModContent.ItemType<BiomeCore>()) //is the accessory in the slot?
                {
                    accesstype++; //if not, check next slot
                }
                else if (player.hideVisibleAccessory[accesstype] && (player.armor[accesstype].type == ModContent.ItemType<GraniteCoreAccess>() || player.armor[accesstype].type == ModContent.ItemType<BiomeCore>())) //if so, is the slot set to be hidden?
                {
                    nodust = true; //if so, hide dust
                }
                else if (!player.hideVisibleAccessory[accesstype] && (player.armor[accesstype].type == ModContent.ItemType<GraniteCoreAccess>() || player.armor[accesstype].type == ModContent.ItemType<BiomeCore>())) //if not show dust
                {
                    nodust = false;
                }
            }
            accesstype = 3; //reset to slot 3

            if (!nodust)
            {
                if (Main.rand.Next(4) == 0)
                {
                    int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 229, player.velocity.X, player.velocity.Y, 100, default, 1.2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y = 1f;
                }
            }
        }
    }
    //_____________________________________________
    public class GladiatorAccessBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Warrior's Gift");
            //Description.SetDefault("15% increased critical strike chance");
            Main.buffNoTimeDisplay[Type] = true;
        }
        bool nodust;
        int accesstype = 3; //start at slot 3 (Accessory 1)
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 15;
            for (int i = 0; i < 7; i++) //Go through all 7 accessory slots (slot 3 - 8)
            {
                if (player.armor[accesstype].type != ModContent.ItemType<GladiatorAccess>() && player.armor[accesstype].type != ModContent.ItemType<BiomeCore>()) //is the accessory in the slot?
                {
                    accesstype++; //if not, check next slot
                }
                else if (player.hideVisibleAccessory[accesstype] && (player.armor[accesstype].type == ModContent.ItemType<GladiatorAccess>() || player.armor[accesstype].type == ModContent.ItemType<BiomeCore>())) //if so, is the slot set to be hidden?
                {
                    nodust = true; //if so, hide dust
                }
                else if (!player.hideVisibleAccessory[accesstype] && (player.armor[accesstype].type == ModContent.ItemType<GladiatorAccess>() || player.armor[accesstype].type == ModContent.ItemType<BiomeCore>())) //if not show dust
                {
                    nodust = false;
                }
            }          
            accesstype = 3; //reset to access 3

            if (!nodust)
            {
                if (Main.rand.Next(5) == 0)
                {
                    int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 57, player.velocity.X, player.velocity.Y, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }
            }
        }
    }
    //_____________________________________________
    public class SpaceRockDefence : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orbital Defense");
            //Description.SetDefault("25% damage reduction from the next attack\nTaking damage summons asteroid boulders from the sky");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

            player.endurance += .25f;
            player.longInvince = true;
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 6, player.velocity.X, player.velocity.Y, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
            }
        }
    }
    //_____________________________________________
    public class SpaceRockOffence : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orbital Strike");
            //Description.SetDefault("Your next attack will cause asteroid boulders to fall upon the attacked enemy");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

            
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 6, player.velocity.X, player.velocity.Y, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
            }
        }
    }
    //_______________________________________________________
    public class DerpBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Power");
            //Description.SetDefault("In Terraria, Derpling launch you");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {


           
        }
    }

    //_______________________________________________________
    //Mushroom Buffs
    public class MushBuff1 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroom Power");
            //Description.SetDefault("Increases damage dealt and reduces damage taken by 1");
            Main.buffNoTimeDisplay[Type] = true;
        }
        bool nodust;
        int accesstype = 3; //start at slot 3 (Accessory 1)
       
        public override void Update(Player player, ref int buffIndex)
        {
            for (int i = 0; i < 7; i++) //Go through all 7 accessory slots (slot 3 - 8)
            {
                if (player.armor[accesstype].type != ModContent.ItemType<SuperMushroom>() && player.armor[accesstype].type != ModContent.ItemType<BiomeCore>()) //is the accessory in the slot?
                {
                    accesstype++; //if not, check next slot
                }
                else if (player.hideVisibleAccessory[accesstype] && (player.armor[accesstype].type == ModContent.ItemType<SuperMushroom>() || player.armor[accesstype].type == ModContent.ItemType<BiomeCore>())) //if so, is the slot set to be hidden?
                {
                    nodust = true; //if so, hide dust
                }
                else if (!player.hideVisibleAccessory[accesstype] && (player.armor[accesstype].type == ModContent.ItemType<SuperMushroom>() || player.armor[accesstype].type == ModContent.ItemType<BiomeCore>())) //if not show dust
                {
                    nodust = false;
                }
            }
            accesstype = 3; //reset to slot 3

            if (!nodust)
            {
                if (Main.rand.Next(10) == 0)
                {
                    int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 113, player.velocity.X, player.velocity.Y, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }
            }
        }
    }
    public class MushBuff2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroom Power");
            //Description.SetDefault("Increases damage dealt and reduces damage taken by 2");
            Main.buffNoTimeDisplay[Type] = true;
        }
        bool nodust;
        int accesstype = 3; //start at slot 3 (Accessory 1)

        public override void Update(Player player, ref int buffIndex)
        {
            for (int i = 0; i < 7; i++) //Go through all 7 accessory slots (slot 3 - 8)
            {
                if (player.armor[accesstype].type != ModContent.ItemType<SuperMushroom>() && player.armor[accesstype].type != ModContent.ItemType<BiomeCore>()) //is the accessory in the slot?
                {
                    accesstype++; //if not, check next slot
                }
                else if (player.hideVisibleAccessory[accesstype] && (player.armor[accesstype].type == ModContent.ItemType<SuperMushroom>() || player.armor[accesstype].type == ModContent.ItemType<BiomeCore>())) //if so, is the slot set to be hidden?
                {
                    nodust = true; //if so, hide dust
                }
                else if (!player.hideVisibleAccessory[accesstype] && (player.armor[accesstype].type == ModContent.ItemType<SuperMushroom>() || player.armor[accesstype].type == ModContent.ItemType<BiomeCore>())) //if not show dust
                {
                    nodust = false;
                }
            }
            accesstype = 3; //reset to slot 3

            if (!nodust)
            {
                if (Main.rand.Next(6) == 0)
                {
                    int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 113, player.velocity.X, player.velocity.Y, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }
            }
        }
    }
    public class MushBuff3 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroom Power");
            //Description.SetDefault("Increases damage dealt and reduces damage taken by 4");
            Main.buffNoTimeDisplay[Type] = true;
        }
        bool nodust;
        int accesstype = 3; //start at slot 3 (Accessory 1)

        public override void Update(Player player, ref int buffIndex)
        {
            for (int i = 0; i < 7; i++) //Go through all 7 accessory slots (slot 3 - 8)
            {
                if (player.armor[accesstype].type != ModContent.ItemType<SuperMushroom>() && player.armor[accesstype].type != ModContent.ItemType<BiomeCore>()) //is the accessory in the slot?
                {
                    accesstype++; //if not, check next slot
                }
                else if (player.hideVisibleAccessory[accesstype] && (player.armor[accesstype].type == ModContent.ItemType<SuperMushroom>() || player.armor[accesstype].type == ModContent.ItemType<BiomeCore>())) //if so, is the slot set to be hidden?
                {
                    nodust = true; //if so, hide dust
                }
                else if (!player.hideVisibleAccessory[accesstype] && (player.armor[accesstype].type == ModContent.ItemType<SuperMushroom>() || player.armor[accesstype].type == ModContent.ItemType<BiomeCore>())) //if not show dust
                {
                    nodust = false;
                }
            }
            accesstype = 3; //reset to slot 3

            if (!nodust)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 113, player.velocity.X, player.velocity.Y, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }
            }
        }
    }
    /*public class MushBuff4 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroom Power");
            //Description.SetDefault("Increases damage dealt and reduces damage taken by 6");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

            if (Main.rand.Next(2) == 0)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 113, player.velocity.X, player.velocity.Y, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
            }

        }
    }*/
    public class SkyKnightSentryBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star Warrior's Sentry");
            //Description.SetDefault("A Star sentry attacks for you");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            
        }
    }
    public class SantankBuff1 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ho-Ho-Homing Missile");
            //Description.SetDefault("Some of your missiles are ready to launch");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

        }
    }
    public class SantankBuff2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ho-Ho-Homing Missile");
            //Description.SetDefault("Most of your missiles are ready to launch");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

        }
    }
    public class SantankBuff3 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ho-Ho-Homing Missile");
            //Description.SetDefault("All of your missiles are ready to launch");
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {

        }
    }
    public class BloodBurstBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Burst");
            //Description.SetDefault("Blood is oozing to burst out of the next struck enemy");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

        }
    }

    public class HellSoulBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Infernal Storm");
            //Description.SetDefault("The infernal flames are ready to be unleashed");
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {

        }
    }

    public class GunBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Marksmanship");
            //Description.SetDefault("15% increased bullet damage");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.bulletDamage *= 1.15f;

        }
    }

    public class WoodenBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forest Empowerment");
            //Description.SetDefault("Reduces damage taken by 4");
        }

        public override void Update(Player player, ref int buffIndex)
        {

            if (Main.rand.Next(6) == 0)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, 3, -player.velocity.X, -player.velocity.Y, 100, default, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }
        }
    }

    public class WoodenBlizzardBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blizzard Empowerment");
            //Description.SetDefault("Creates a small blizzard around you that slows any enemy caught in it");
        }

        public override void Update(Player player, ref int buffIndex)
        {

        }
    }

    public class WoodenDesertBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Desert Empowerment");
            //Description.SetDefault("Striking an enemy has a chance to emit a damaging spark");
        }

        public override void Update(Player player, ref int buffIndex)
        {

            if (Main.rand.Next(6) == 0)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, 138, -player.velocity.X, -player.velocity.Y, 100, default, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }
        }
    }

    //_________________________________________________________________
    public class PainBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Painbringers Curse");
            //Description.SetDefault("You will get to experience pain all over again on death");
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {

        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
    //_________________________________________________________________
    public class SantaReviveBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heart Shock");
            //Description.SetDefault("You are immune to damage and deal mroe damage, but yur heart will soon stop");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.25f;

            if (Main.rand.Next(8) == 0)
            {
                Vector2 dustspeed = new Vector2(0, 2).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(player.Center, 0, 0, 133, dustspeed.X, dustspeed.Y, 100, default, 1f);
            }
            if (player.statLife < player.statLifeMax2 / 3)
            {
                if (Main.rand.Next(5) == 0)
                {
                    Vector2 dustspeed = new Vector2(0, 1).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(player.Center, 0, 0, 5, dustspeed.X, dustspeed.Y, 100, default, 1f);
                }
            }
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
    //_________________________________________________________________
    public class ReflectedBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Reflection");
            //Description.SetDefault("You can reflect projectiles");
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (Main.rand.Next(5) == 0)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld =new Vector2(player.position.X + Main.rand.Next (0, player.width), player.position.Y + Main.rand.Next(0, player.height)),
                }, player.whoAmI);
            }
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }

    //_________________________________________________________________
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
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}