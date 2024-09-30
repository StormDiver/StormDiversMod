using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ExampleMod.Common.Players
{
    public class PlayerUpgrades : ModPlayer
    {
        public bool ZephyrFeatherUpgrade;

        /*public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            
        }*/
        public override void PostUpdateEquips()
        {
            if (ZephyrFeatherUpgrade) //this works I guess
            {
                Player.maxRunSpeed *= 1.1f;
                Player.runAcceleration *= 1.1f;
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag["ZephyrFeatherUpgrade"] = ZephyrFeatherUpgrade;
        }
        public override void LoadData(TagCompound tag)
        {
            ZephyrFeatherUpgrade = tag.GetBool("ZephyrFeatherUpgrade");
        }

        //idc about multiplayer
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            //???
            base.SendClientChanges(clientPlayer);
        }
        public override void CopyClientState(ModPlayer targetCopy)
        {
            //???
            base.CopyClientState(targetCopy);
        }
    }
}