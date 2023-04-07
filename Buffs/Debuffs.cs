using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;


//buff effects for npcs are in NPCEffects.cs, effects for player are in NegativeHealthDrain.cs
namespace StormDiversMod.Buffs
{
    public class AridSandDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Burn");
            //Description.SetDefault("The forbidden sand burns you");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen = -16;
            //player.GetModPlayer<NegativeHealthDrain>().sandBurn = true;

            if (Main.rand.Next(4) < 3)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 10, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
                if (Main.rand.NextBool(4))
                {
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].scale *= 0.5f;
                }
            }
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.position, 1f, 0.5f, 0f);

            }

        }
        public override void Update(NPC npc, ref int buffIndex)
        {

            npc.GetGlobalNPC<NPCEffects>().sandBurn = true;
        }
    }
    //___________________________________________________________
    public class SuperFrostBurn : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("CryoBurn");
            //Description.SetDefault("It's like FrostBurn, but it hurts even more");
            Main.debuff[Type] = true;
            // Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.GetModPlayer<NegativeHealthDrain>().superFrost = true;

            player.lifeRegen = -16;


            if (Main.rand.Next(4) < 3)
            {
                int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 156, player.velocity.X, player.velocity.Y, 135, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity

                int dust2 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 156, player.velocity.X, player.velocity.Y, 135, default, .3f);
                Main.dust[dust2].velocity *= 0.5f;
            }
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.position, 0f, 1f, 1f);
            }
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().superFrost = true;

        }
    }

    //_____________________________________________________________
    public class ScanDroneDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Marked for target");
            //Description.SetDefault("All your defense has been taken away");
            Main.debuff[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 500;

        }

    }

    //____________________________________________________________
    public class LunarBoulderDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lunar Bouldered");
            //Description.SetDefault("Inflicts unimaginable pain, reduced movement speed and constant burning");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }
        int particle = 0;
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed = 0.2f;
            player.lifeRegen = -30;

            int choice = Main.rand.Next(4);
            if (choice == 0)
            {
                particle = 244;
            }
            else if (choice == 1)
            {
                particle = 110;
            }
            else if (choice == 2)
            {
                particle = 111; ;
            }
            else if (choice == 3)
            {
                particle = 112;
            }
            if (Main.rand.Next(3) < 3)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, particle, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default, 0.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.8f;
                Main.dust[dust].velocity.Y -= 0.5f;
            }
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.position, 1f, 0.5f, 0.8f);
            }
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().lunarBoulderDB = true;
        }


    }

    //___________________________________________________________
    public class BeetleDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Beetle Swarm");
            //Description.SetDefault("Beetles have greatly reduced your speed");
            Main.debuff[Type] = true;
            // Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {


        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().beetled = true;


        }
    }
    //___________________________________________________________
    public class HeartDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stolen Heart");
            //Description.SetDefault("You cannot live without a heart");
            Main.debuff[Type] = true;
            // Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {


        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().heartDebuff = true;



        }
    }
    //________________________________________________
    public class SuperBurnDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blazing Fire");
            //Description.SetDefault("This is fine");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.GetModPlayer<NegativeHealthDrain>().superBurn = true;
            player.lifeRegen = -8;

            if (Main.rand.Next(4) < 3)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 6, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 0, default, 2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
            }

        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().superburnDebuff = true;



        }
    }
    //________________________________________________
    public class HellSoulFireDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("SoulBurn");
            //Description.SetDefault("This might not be fine");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.GetModPlayer<NegativeHealthDrain>().hellSoulDebuff = true;

            player.lifeRegen = -14;

            if (Main.rand.Next(4) < 3)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 173, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 0, default, 2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;

            }

        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().hellSoulFire = true;



        }
    }
    //________________________________________________
    public class TwilightDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Twilight Warped");
            //Description.SetDefault("You are unable to perform another Twilight Warp");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.GetModPlayer<NegativeHealthDrain>().hellSoulFire = true;

            if (Main.rand.Next(4) < 2)
            {
                if (player.gravDir == 1)
                {
                    var dust = Dust.NewDustDirect(player.position, player.width, player.height, 179, 0, -3);
                    dust.scale = 1.25f;
                    dust.noGravity = true;
                    dust.velocity *= 0.75f;
                }
                else
                {
                    var dust = Dust.NewDustDirect(player.position, player.width, player.height, 179, 0, +3);
                    dust.scale = 1.25f;
                    dust.noGravity = true;
                    dust.velocity *= 0.75f;
                }
            }
            //player.GetModPlayer<NegativeHealthDrain>().hellSoulDebuff = true;

        }
        public override void Update(NPC npc, ref int buffIndex)
        {

        }
    }
    //_______________________________________________
    public class DerpDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Launched");
            //Description.SetDefault("You have been launched into the air by the power of the Derplings");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().derplaunched = true;
        }
    }
    //_______________________________________________
    public class DarkShardDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Essence of Dark");
            //Description.SetDefault("You have been surrounded by the darkness");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {


        }
        public override void Update(NPC npc, ref int buffIndex)
        {

            npc.GetGlobalNPC<NPCEffects>().darknessDebuff = true;



        }
    }
    //_______________________________________________
    public class UltraBurnDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("UltraBurn");
            //Description.SetDefault("This most certainly is NOT fine!!!!");
            Main.debuff[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.GetModPlayer<NegativeHealthDrain>().ultraBurn = true;
            player.lifeRegen = -25;


            int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 6, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 0, default, 2.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 1f;
            Main.dust[dust].velocity.Y -= 2f;



        }
        public override void Update(NPC npc, ref int buffIndex)
        {

            npc.GetGlobalNPC<NPCEffects>().ultraburnDebuff = true;



        }
    }
    //_______________________________________________
    public class UltraFrostDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("UltraFreeze");
            //Description.SetDefault("This really really REALLY hurts!!!!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.GetModPlayer<NegativeHealthDrain>().ultraFrost = true;
            player.lifeRegen = -25;


            int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 135, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 0, default, 2.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 1f;
            Main.dust[dust].velocity.Y -= 2f;



        }
        public override void Update(NPC npc, ref int buffIndex)
        {

            npc.GetGlobalNPC<NPCEffects>().ultrafrostDebuff = true;


        }
    }
    //_________________________________________________________________
    public class SpookedDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spooked");
            //Description.SetDefault("You are overwhelmed by fear");
            Main.debuff[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {



        }
        public override void Update(NPC npc, ref int buffIndex)
        {

            npc.GetGlobalNPC<NPCEffects>().spookedDebuff = true;


        }
    }
    //_________________________________________________________________
    public class WhiptagForbiddenDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Whip tag");
            //Description.SetDefault("You have been tagged by the Forbidden Whip");
            Main.debuff[Type] = true;
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().WhiptagForbidden = true;
            npc.GetGlobalNPC<NPCEffects>().WhiptagCrit += 6;

        }
    }
    //_________________________________________________________________
    public class WhiptagSpaceRockDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Whip tag");
            //Description.SetDefault("You have been tagged by the Asteroid Belt");
            Main.debuff[Type] = true;
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().WhiptagSpaceRock = true;
            npc.GetGlobalNPC<NPCEffects>().WhiptagCrit += 15;

        }
    }
    //_________________________________________________________________
    public class WhiptagBloodDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloody Whip tag");
            //Description.SetDefault("You have been tagged by The Bloody Spine");
            Main.debuff[Type] = true;
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().WhiptagBlood = true;
            npc.GetGlobalNPC<NPCEffects>().WhiptagCrit += 4;

        }
    }
    //_________________________________________________________________
    public class WhiptagWebDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spider Whip tag");
            //Description.SetDefault("You have been tagged by The Spider Whip");
            Main.debuff[Type] = true;
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().WhiptagWeb = true;
        }
    }
    //_________________________________________________________________
    public class WebDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cobwebbed");
            //Description.SetDefault("You are covered in webs which slow you down");
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().webDebuff = true;
        }
    }
    //_________________________________________________________________
    public class AridCoreDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arid Aura");
            //Description.SetDefault("You will take more damage");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCEffects>().aridCoreDebuff = true;
        }
    }
    //_________________________________________________________________
    public class YouCantEscapeDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("You can't escape me!");
            //Description.SetDefault("Even death cannot save you from the pain");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {

        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
    //_________________________________________________________________
    public class PainlessDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pain Cooldown");
            //Description.SetDefault("Your pain will end if you die");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {

        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}