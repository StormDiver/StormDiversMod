using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
namespace StormDiversMod.Common
{
    public class PlayerUpgrades : ModPlayer
    {
        public bool ZephyrFeatherUpgrade;
        public bool NoTempleCurse;


        //gacah stuff
        public int Gacha5Pity; //Pity for 5 star
        public int Gacha4Pity; //Pity for 4 star
        public bool Gacha5050; //5050 Guaranteed?
        public int GachaCoinDropCount; //How many coins have been dropped by enemies
        public int GachaCoinDropCooldown; //Cooldown for when coin drops reset
        public int GachaCoinDropBossCount; //Cooldown for when coin drops reset
        public int GachaCoinDropBossCooldown; //Colldown for when bosses can drop more coins
        //add cooldown timer
        /*public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            
        }*/
        public override void PostUpdateEquips()
        {
            if (!GetInstance<ConfigurationsGlobal>().NoGacha4U)
            {
                if (GachaCoinDropCooldown > 0)
                    GachaCoinDropCooldown--;
                else
                {
                    GachaCoinDropCooldown = 3600; //every minute reset the count (limit of 30)
                    GachaCoinDropCount = 0;
                }

                //Main.NewText("Drop Reset = " + GachaCoinDropCooldown, Color.Red);
                //Main.NewText("Drop Amount limit = " + GachaCoinDropCount + "/30", Color.Red);

                if (GachaCoinDropBossCooldown > 0)
                    GachaCoinDropBossCooldown--;
                else
                {
                    GachaCoinDropBossCooldown = 3600 * 4; //every 4 minutes reset the count (limit of 3 * 60) (45 per minute)
                    GachaCoinDropBossCount = 0;
                }

                //Main.NewText("Boss Cooldown = " + GachaCoinDropBossCooldown, Color.Orange);
                //Main.NewText("Boss Amount Limit = " + GachaCoinDropBossCount + "/3", Color.Orange);
            }
            if (ZephyrFeatherUpgrade) //this works I guess
            {
                Player.maxRunSpeed *= 1.1f;
                Player.runAcceleration *= 1.1f;
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag["ZephyrFeatherUpgrade"] = ZephyrFeatherUpgrade;
            tag["NoTempleCurse"] = NoTempleCurse;
            tag["Gacha5Pity"] = Gacha5Pity;
            tag["Gacha4Pity"] = Gacha4Pity;
            tag["Gacha5050"] = Gacha5050;
            tag["GachaCoinDropCount"] = GachaCoinDropCount;
            tag["GachaCoinDropCooldown"] = GachaCoinDropCooldown;
            tag["GachaCoinDropBossCount"] = GachaCoinDropBossCount;
            tag["GachaCoinDropBossCooldown"] = GachaCoinDropBossCooldown;

        }
        public override void LoadData(TagCompound tag)
        {
            ZephyrFeatherUpgrade = tag.GetBool("ZephyrFeatherUpgrade");
            NoTempleCurse = tag.GetBool("NoTempleCurse");
            Gacha5Pity = tag.GetInt("Gacha5Pity");
            Gacha4Pity = tag.GetInt("Gacha4Pity");
            Gacha5050 = tag.GetBool("Gacha5050");
            GachaCoinDropCount = tag.GetInt("GachaCoinDropCount");
            GachaCoinDropCooldown = tag.GetInt("GachaCoinDropCooldown");
            GachaCoinDropBossCount = tag.GetInt("GachaCoinDropBossCount");
            GachaCoinDropBossCooldown = tag.GetInt("GachaCoinDropBossCooldown");
        }

        //idc about multiplayer
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
           
            base.SendClientChanges(clientPlayer);
        }
        public override void CopyClientState(ModPlayer targetCopy)
        {
          
            base.CopyClientState(targetCopy);
        }
    }
}