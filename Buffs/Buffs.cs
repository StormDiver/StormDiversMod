using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;



//buff effects for npcs are in NPCEffects.cs, effects for player are in EquipmentEffects.cs
namespace StormDiversMod.Buffs
{
    public class CelestialBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
          
            //DisplayName.SetDefault("Celestial Guardian");
            //Description.SetDefault("The powers of the Celestial spirits rapidly regenerate your life and grant additional defense");
        }
        int particle = 10;
        public override void Update(Player player, ref int buffIndex)
        {

            particle--;
           
            {
                player.statDefense += 25;

                player.lifeRegen += 30;
                if (particle <= 0)
                {
                    particle = 10;
                    Dust.NewDustDirect(new Vector2(player.position.X + 5f, player.position.Y + 20f), 5, 5, 110);
                    Dust.NewDustDirect(new Vector2(player.position.X + 5f, player.position.Y + 20f), 5, 5, 111);
                    Dust.NewDustDirect(new Vector2(player.position.X + 5f, player.position.Y + 20f), 5, 5, 112);
                    Dust.NewDustDirect(new Vector2(player.position.X + 5f, player.position.Y + 20f), 5, 5, 244);

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
            //Description.SetDefault("Increases ammo damage by 10%");
        }
        // code in EquipmentEffects.cs
    }
    //_______________________________________________________________________________

    public class SpectreBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spectre Enhancement");
            //Description.SetDefault("Maximum mana increased by 60");
        }
        public override void Update(Player player, ref int buffIndex)
        {

            player.statManaMax2 += 60;
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
            if (ModLoader.HasMod("TRAEProject"))
            {
                player.moveSpeed += 0.15f;
            }
            else
            {
                player.moveSpeed += 0.5f;
            }
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
            //Description.SetDefault("Reduces damage taken by by 15% and grants immunity to knockback");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.velocity.X *= 0.94f;
            player.endurance += 0.15f;
            player.noKnockback = true;
            if (Main.rand.Next(4) == 0)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 65, player.velocity.X, player.velocity.Y, 100, default, 1.5f);
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
            //Description.SetDefault("50% increased damage");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.5f;
           

            if (Main.rand.Next(4) == 0)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 70, player.velocity.X, player.velocity.Y, 100, default, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y = 1f;
            }
        }
    }
    //_____________________________________________
    public class GladiatorAccessBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Warrior's Gift");
            //Description.SetDefault("20% increased critical strike chance");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 20;

           

            if (Main.rand.Next(5) == 0)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 57, player.velocity.X, player.velocity.Y, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
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
            //DisplayName.SetDefault("Derpling Launch");
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

        public override void Update(Player player, ref int buffIndex)
        {

            if (Main.rand.Next(16) == 0)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 113, player.velocity.X, player.velocity.Y, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
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

        public override void Update(Player player, ref int buffIndex)
        {

            if (Main.rand.Next(8) == 0)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 113, player.velocity.X, player.velocity.Y, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
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

        public override void Update(Player player, ref int buffIndex)
        {

            if (Main.rand.Next(4) == 0)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 113, player.velocity.X, player.velocity.Y, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
            }

        }
    }
    public class MushBuff4 : ModBuff
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
    }
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
            //Description.SetDefault("Blood is oozing to burst out of you");
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
            //DisplayName.SetDefault("Inferno Storm");
            //Description.SetDefault("The inferno flames are ready to be unleashed");
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

    //_________________________________________________________________
    public class PainBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pain Extender");
            //Description.SetDefault("You will get to experience pain all over again on death");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {

        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}