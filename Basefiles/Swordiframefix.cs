using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Items.Weapons;

namespace StormDiversMod.Basefiles
{
    //Credit to Qwerty3.14
    public class BladeImmunePlayer : ModPlayer
    {
        NPC justHit = null;
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (item.type == ModContent.ItemType<BloodSword>() || item.type == ModContent.ItemType<HellSoulSword>() || item.type == ModContent.ItemType<LightDarkSword>())
            {
                justHit = target;
            }
        }
        public override bool? CanHitNPC(Item item, NPC target)
        {
            if (item.type == ModContent.ItemType<BloodSword>() || item.type == ModContent.ItemType<HellSoulSword>() || item.type == ModContent.ItemType<LightDarkSword>())
            {
                if (target.GetGlobalNPC<BladeImmune>().immune[Player.whoAmI] != 0)
                {
                    return false;
                }
            }
            return base.CanHitNPC(item, target);
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