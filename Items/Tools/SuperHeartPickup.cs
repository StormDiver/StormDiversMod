using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.Audio;

namespace StormDiversMod.Items.Tools
{
    public class SuperHeartPickup : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Heart");
            Tooltip.SetDefault("Heals 25 health when picked up");
        
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Orange;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.IgnoresEncumberingStone[Item.type] = true;
        }
        
        public override bool GrabStyle(Player player)
        {
           
            return false;
        }
        public override void GrabRange(Player player, ref int grabRange)
        {
           
            if (player.HasBuff(BuffID.Heartreach))
            {
                grabRange = 300;
                
            }
            else
            {
                grabRange = 150;

            }
        }
        
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
        }
        public override bool CanPickup(Player player)
        {
            return true;
        }
        public override bool OnPickup(Player player)
        {
            SoundEngine.PlaySound(SoundID.Grab, player.Center);

            player.statLife += 25;
            player.HealEffect(25, true);
            return false;
        }
       
        public override bool ItemSpace(Player player)
        {
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}