using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.NPCs.Banners     
{
    public class BabyDerpBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Baby Derpling Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*/*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<BabyDerpBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)    //so if a player is close to the banner
            {          
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.BabyDerp>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
    public class VineDerpBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Camouflaged Derpling Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<VineDerpBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)      //so if a player is close to the banner
            {
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.VineDerp>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
        public class ScanDroneBannerPlace : ModTile
        {
            public override void SetStaticDefaults()
            {
                Main.tileFrameImportant[Type] = true;
                Main.tileNoAttach[Type] = true;
                Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
                TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
                TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
                TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
                TileObjectData.newTile.StyleHorizontal = true;
                TileObjectData.newTile.StyleWrapLimit = 111;
                TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
                TileObjectData.addTile(Type);

                LocalizedText name = CreateMapEntryName();
                //name.SetDefault("ScanDrone Banner");
                AddMapEntry(new Color(13, 88, 130), name);
            }

            /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<ScanDroneBannerItem>());//this defines what to drop when this tile is destroyed
            }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)     //so if a player is close to the banner
            {
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.ScanDrone>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
        }
    public class StormDerpBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Storm Hopper Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<StormDerpBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.StormDerp>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class VortCannonBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Vortexian Cannon Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<VortCannonBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.VortexCannon>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class NebulaDerpBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Brain Hopper Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<NebulaDerpBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.NebulaDerp>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class StardustDerpBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Star Hopper Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<StardustDerpBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.StardustDerp>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class SolarDerpBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Blazing Hopper Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<SolarDerpBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {     
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.SolarDerp>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class MoonDerpBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Moonling Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<MoonDerpBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {     
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.MoonDerp>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class SpaceRockHeadBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Asteroid Orbiter Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<SpaceRockHeadBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {           
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.SpaceRockHead>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
       
      
    }
    public class SpaceRockHeadLargeBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Asteroid Charger Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<SpaceRockHeadLargeBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {             
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.SpaceRockHeadLarge>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class GladiatorMiniBossBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Fallen Champion Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<GladiatorMiniBossBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.GladiatorMiniBoss>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class GraniteMiniBossBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Surged Granite Core Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<GraniteMiniBossBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {
                
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.GraniteMiniBoss>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class HellSoulBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Heartless Soul Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<HellSoulBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {        
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.HellSoul>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class MushroomMiniBossBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Angry Mushroom Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<MushroomMiniBossBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {  
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.MushroomMiniBoss>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class GolemMinionBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Temple Guardian Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<GolemMinionBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {       
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.GolemMinion>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class HellMiniBossBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Soul Cauldron Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<HellMiniBossBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {      
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.HellMiniBoss>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class IceCoreBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Frigid Snowflake Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<IceCoreBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {        
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.IceCore>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class SandCoreBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);        
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Dune Blaster Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }
        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<SandCoreBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {   
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.SandCore>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class MeteorDropperBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Meteor Bomber Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<MeteorDropperBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {       
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.MeteorDropper>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }

    public class GolemSentryBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            
            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Lihzahrd Flametrap Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<GolemSentryBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {     
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.GolemSentry>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class FrozenEyeBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Frozen Eyefish Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<FrozenEyeBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.FrozenEye>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class FrozenSoulBannerPlace : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;  //This defines if the tile is destroyed by lava
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);//
            TileObjectData.newTile.Height = 3;  //this is how many parts the sprite is devided (height)
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };  //this is how many pixels are in each devided part(pink square) (height)   so there are 3 parts with 16 x 16
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Frozen Spirit Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<FrozenSoulBannerItem>());//this defines what to drop when this tile is destroyed
        }*/

        public override void NearbyEffects(int i, int j, bool closer)   //this make so the banner give an effect to nearby players
        {
            if (closer)  //so if a player is close to the banner
            {
                Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<NPCs.FrozenSoul>()] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
}