using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Common;
using StormDiversMod.Items.Vanitysets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.NPCs
{
    public class XSentryTarget : ModNPC //Ok this is Jank AF but it works so shut
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the bestiary
            };
            NPCID.Sets.ImmuneToAllBuffs[NPC.type] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 30;
            NPC.aiStyle = -1;
            AIType = 0;

            NPC.damage = 0;
            NPC.noGravity = true;
            NPC.defense = 999999;
            NPC.lifeMax = 999999;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0;
            NPC.scale = 1f;
            NPC.alpha = 100;
            NPC.gfxOffY = -4;
        }
        public override void AI()
        {
            NPC.ShowNameOnHover = false; //Otherwise name will be visible
            Player player = Main.LocalPlayer;
            //NPC.immune[player.whoAmI] = 99999999;
            NPC.rotation += 0.1f;
            NPC.Center = Main.MouseWorld;

            if (!player.GetModPlayer<MiscFeatures>().sentryControl) //remove if player is not manually targetting
                NPC.life -= 2147483647;

            Main.player[Main.myPlayer].MinionAttackTargetNPC = NPC.whoAmI; //Sets the manual target to be this NPC

            if (Main.rand.Next(5) == 0)
            {
                Vector2 perturbedSpeed = new Vector2(0, -1f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X -2, NPC.Center.Y - 2), 0, 0, 182, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 0.8f;
            }
            Dust dust2;
            dust2 = Dust.NewDustPerfect(NPC.Center, 182, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust2.noGravity = true;
        }
        public override void UpdateLifeRegen(ref int damage) //if the player is somehow able to damage it...
        {
            NPC.lifeRegen += 999999;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

        //Makes it immune to damage but it can still be targetted
        public override bool? CanBeHitByProjectile(Projectile projectile) => false;

        public override bool? CanBeHitByItem(Player player, Item item) => false;
    }
}