using StormDiversMod.Basefiles;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace StormDiversMod.Emotes
{
    public class ThePainEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.General);
        }
    }
    public class TheEvilEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.General);
        }
    }
    public class TheAngeryEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.General);
        }
    }
    public class TheSadEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.General);
        } 
    }
    public class ClaymanEmote : ModEmoteBubble
    { 
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.General);
        }
    }
    public class AridBossEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.Dangers);
        }
        public override bool IsUnlocked()
        {
            if (StormWorld.aridBossDown == true)
                return true;
            else
                return false;
        }
    }
    public class StormBossEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.Dangers);
        }
        public override bool IsUnlocked()
        {
            if (StormWorld.stormBossDown == true)
                return true;
            else
                return false;
        }
    }
    public class UltimateBossEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.Dangers);
        }
        public override bool IsUnlocked()
        {
            if (StormWorld.ultimateBossDown == true)
                return true;
            else
                return false;
        }
    }
}