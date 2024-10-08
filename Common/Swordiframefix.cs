using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Items.Weapons;

namespace StormDiversMod.Common
{
    //Credit to Qwerty3.14
    public class BladeImmunePlayer : ModPlayer
    {
        NPC justHit = null;
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (item.type == ModContent.ItemType<BloodSword>() || item.type == ModContent.ItemType<HellSoulSword>() || item.type == ModContent.ItemType<LightDarkSword>() || item.type == ModContent.ItemType<EyeSword>())
            {
                justHit = target;
            }
        }
        public override bool? CanHitNPCWithItem(Item item, NPC target)
        {
            if (item.type == ModContent.ItemType<BloodSword>() || item.type == ModContent.ItemType<HellSoulSword>() || item.type == ModContent.ItemType<LightDarkSword>() || item.type == ModContent.ItemType<EyeSword>())
            {
                if (target.GetGlobalNPC<BladeImmune>().immune[Player.whoAmI] != 0)
                {
                    return false;
                }
            }
            return base.CanHitNPCWithItem(item, target);
        }
        public override void PostItemCheck()
        {

            if (justHit != null)
            {
                justHit.GetGlobalNPC<BladeImmune>().immune[Player.whoAmI] = justHit.immune[Player.whoAmI];
                justHit.immune[Player.whoAmI] = 0;
                justHit = null;
            }
        }
    }
    public class BladeImmune : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int[] immune = new int[256];
        public override bool PreAI(NPC npc)
        {
            for (int j = 0; j < 256; j++)
            {
                if (immune[j] > 0)
                {
                    immune[j]--;
                }
            }
            return base.PreAI(npc);
        }
    }

}