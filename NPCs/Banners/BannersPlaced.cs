using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.NPCs.Banners
{
    [LegacyName("BabyDerpBannerPlace")]
    [LegacyName("VineDerpBannerPlace")]
    [LegacyName("ScanDroneBannerPlace")]
    [LegacyName("StormDerpBannerPlace")]
    [LegacyName("VortCannonBannerPlace")]
    [LegacyName("NebulaDerpBannerPlace")]
    [LegacyName("StardustDerpBannerPlace")]
    [LegacyName("SolarDerpBannerPlace")]
    [LegacyName("MoonDerpBannerPlace")]
    [LegacyName("SpaceRockHeadBannerPlace")]
    [LegacyName("SpaceRockHeadLargeBannerPlace")]
    [LegacyName("GladiatorMiniBossBannerPlace")]
    [LegacyName("GraniteMiniBossBannerPlace")]
    [LegacyName("HellSoulBannerPlace")]
    [LegacyName("MushroomMiniBossBannerPlace")]
    [LegacyName("GolemMinionBannerPlace")]
    [LegacyName("HellMiniBossBannerPlace")]
    [LegacyName("IceCoreBannerPlace")]
    [LegacyName("SandCoreBannerPlace")]
    [LegacyName("MeteorDropperBannerPlace")]
    [LegacyName("GolemSentryBannerPlace")]
    [LegacyName("FrozenEyeBannerPlace")]
    [LegacyName("FrozenSoulBannerPlace")]
    [LegacyName("ThePainSlimeBannerPlaced")]
    [LegacyName("TheClaySlimeBannerPlaced")]
    [LegacyName("SnowmanPizzaBannerPlaced")]
    [LegacyName("SnowmanBombBannerPlaced")]
    public class BannersPlaced : ModBannerTile
	{
		// This enum keeps our code clean and readable.
		// Each enum entry has a numerical value (0, 1, ...) which corresponds to the tile style of the placed banner.
		public enum StyleID
		{
            IOU,
			BabyDerp,
            VineDerp,
			ScanDrone,
			StormDerp,
			VortCannon,
			NebulaDerp,
			StardustDerp,
			SolarDerp,
			MoonDerp,
			SpaceRockHead,
			SpaceRockHeadLarge,
			GladiatorMiniBoss,
            GraniteMiniBoss,
            HellSoul,
            MushroomMiniBoss,
            GolemMinion,
            HellMiniBoss,
            IceCore,
            SandCore,
            MeteorDropper,
            GolemSentry,
            FrozenEye,
            FrozenSoul,
            ThePainSlime,
            TheClaySlime,
            SnowmanPizza,
            SnowmanBomb
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }

	// If using this approach, be aware that you'll need to change the following in ModNPC.SetDefaults:
	// BannerItem = ModContent.ItemType<ExampleCustomAISlimeNPCBanner>();
	// to
	// BannerItem = Mod.Find<ModItem>("ExampleCustomAISlimeNPCBanner").Type;

	/*public class EnemyBannerLoader : ILoadable
	{
		public void Load(Mod mod)
		{
			// For each entry in EnemyBanner.StyleID, we dynamically load an AutoloadedBannerItem. 
			foreach (StyleID styleID in Enum.GetValues(typeof(StyleID)))
			{
				mod.AddContent(new AutoloadedBannerItem(styleID.ToString() + "Banner", (int)styleID));
			}
		}

		public void Unload()
		{
		}
	}*/
}
	
