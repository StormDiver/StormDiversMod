using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Common;
using ReLogic.Content;
using Terraria.GameContent.Creative;
using Terraria.Utilities;
using System.IO;
using Terraria.DataStructures;
using static Terraria.ModLoader.PlayerDrawLayer;


namespace StormDiversMod.Projectiles.ToolsProjs
{
    internal class BobberCoral : ModProjectile
    {
        public static readonly Color[] PossibleLineColors = new Color[] {
            new Color(128, 95, 74) // A brown color
		};

        // This holds the index of the fishing line color in the PossibleLineColors array.
        private int fishingLineColorIndex;

		public Color FishingLineColor => PossibleLineColors[fishingLineColorIndex];

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Bobber");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BobberWooden);

            DrawOriginOffsetY = -8; // Adjusts the draw position
        }

        public override void OnSpawn(IEntitySource source)
        {
            // Decide color of the pole by getting the index of a random entry from the PossibleLineColors array.
            fishingLineColorIndex = (byte)Main.rand.Next(PossibleLineColors.Length);
        }

        // What if we want to randomize the line color
        public override void AI()
        {
            // Always ensure that graphics-related code doesn't run on dedicated servers via this check.
            if (!Main.dedServ)
            {
                // Create some light based on the color of the line.
                //Lighting.AddLight(Projectile.Center, FishingLineColor.ToVector3());
            }
        }
        // These last two methods are required so the line color is properly synced in multiplayer.
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)fishingLineColorIndex);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            fishingLineColorIndex = reader.ReadByte();
        }
    }
    //__________________________________________________________________________________________
    internal class BobberDerpling : ModProjectile
	{
		public static readonly Color[] PossibleLineColors = new Color[] {
			new Color(0, 191, 255) // A blue color
		};

		// This holds the index of the fishing line color in the PossibleLineColors array.
		private int fishingLineColorIndex;

        public Color FishingLineColor => PossibleLineColors[fishingLineColorIndex];

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Derpling Bobber");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BobberWooden);

			DrawOriginOffsetY = -8; // Adjusts the draw position
		}

		public override void OnSpawn(IEntitySource source)
		{
			// Decide color of the pole by getting the index of a random entry from the PossibleLineColors array.
			fishingLineColorIndex = (byte)Main.rand.Next(PossibleLineColors.Length);
		}

        // What if we want to randomize the line color
        public override void AI()
        {
            // Always ensure that graphics-related code doesn't run on dedicated servers via this check.
            if (!Main.dedServ)
            {
                // Create some light based on the color of the line.
                //Lighting.AddLight(Projectile.Center, FishingLineColor.ToVector3());
            }
        }
		// These last two methods are required so the line color is properly synced in multiplayer.
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((byte)fishingLineColorIndex);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			fishingLineColorIndex = reader.ReadByte();
		}
	}
    //__________________________________________________________________________________________
    internal class BobberDerplingGold : ModProjectile
    {
        public static readonly Color[] PossibleLineColors = new Color[] {
            new Color(240,221,125) // A gold color
		};

        // This holds the index of the fishing line color in the PossibleLineColors array.
        private int fishingLineColorIndex;

        public Color FishingLineColor => PossibleLineColors[fishingLineColorIndex];

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Bobber");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BobberWooden);

            DrawOriginOffsetY = -8; // Adjusts the draw position
        }

        public override void OnSpawn(IEntitySource source)
        {
            // Decide color of the pole by getting the index of a random entry from the PossibleLineColors array.
            fishingLineColorIndex = (byte)Main.rand.Next(PossibleLineColors.Length);
        }

        public override void AI()
        {
            // Always ensure that graphics-related code doesn't run on dedicated servers via this check.
            if (!Main.dedServ)
            {
                if (Main.rand.Next(15) == 0)
                {
                    var dust = Dust.NewDustDirect(Projectile.Top, Projectile.width, Projectile.height, 124);

                    dust.scale = 0.5f;
                    dust.velocity *= 0;
                }
            }
        }
        // These last two methods are required so the line color is properly synced in multiplayer.
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)fishingLineColorIndex);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            fishingLineColorIndex = reader.ReadByte();
        }
    }
    //__________________________________________________________________________________________
    internal class BobberLunar : ModProjectile
    {
        public static readonly Color[] PossibleLineColors = new Color[] {
            new Color(0, 238, 161) // A green color
		};
       
        // This holds the index of the fishing line color in the PossibleLineColors array.
        private int fishingLineColorIndex;

        public Color FishingLineColor => PossibleLineColors[fishingLineColorIndex];

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lunar Bobber");
            //Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            // These are copied through the CloneDefaults method
             Projectile.width = 15;
             Projectile.height = 15;
             Projectile.aiStyle = 61;
             Projectile.bobber = true;
             Projectile.penetrate = -1;
             Projectile.netImportant = true;
            //Projectile.CloneDefaults(ProjectileID.BobberWooden);

            DrawOffsetX = 0; //Fishing rod hitboxes are very off
            DrawOriginOffsetY = -7;
        }
        public override void OnSpawn(IEntitySource source)
        {
            // Decide color of the pole by getting the index of a random entry from the PossibleLineColors array.
            fishingLineColorIndex = (byte)Main.rand.Next(PossibleLineColors.Length);

            /*if (Projectile.ai[2] == 0)//solar
                Projectile.frame = 0;
            else if (Projectile.ai[2] == 1) //vortex
                Projectile.frame = 1;
            else if (Projectile.ai[2] == 2) //nebula
                Projectile.frame = 2;
            else if (Projectile.ai[2] == 3) //Stardust
                Projectile.frame = 3;*/
        }

        // What if we want to randomize the line color
        int dusttype;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.alpha = 255; //hide the main sprite without hiding the line
           
            if (!Main.dedServ)
            {
                if (Projectile.ai[2] == 0)//solar
                    dusttype = 244;
                else if (Projectile.ai[2] == 1) //vortex
                    dusttype = 110;
                else if (Projectile.ai[2] == 2) //nebula
                    dusttype = 112;
                else //Stardust
                    dusttype = 111;

                if (Main.rand.Next(10) == 0)
                {
                    Dust dust;
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    dust = Terraria.Dust.NewDustDirect(new Vector2(Projectile.position.X + 8, Projectile.position.Y), Projectile.width, Projectile.height, dusttype, 0f, 0f, 0, new Color(255, 255, 255), 0.6f);
                    dust.noGravity = true;
                    dust.fadeIn = 1;
                }
                Lighting.AddLight(Projectile.Center, FishingLineColor.ToVector3());
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)fishingLineColorIndex);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            fishingLineColorIndex = reader.ReadByte();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return null;
        }
        Texture2D texture;
        public override void PostDraw(Color lightColor) //This works I guess
        {
            if (Projectile.ai[2] == 0)
                texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/ToolsProjs/BobberLunarSolar");
            else if (Projectile.ai[2] == 1)
                texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/ToolsProjs/BobberLunarVortex");
            else if (Projectile.ai[2] == 2)
                texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/ToolsProjs/BobberLunarNebula");
            else 
                texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/ToolsProjs/BobberLunarStardust");

                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f + 7); //Seems to fit just about right
            if (Projectile.ai[1] <= 0) //This is set above 0 when pulling in an item
            {                                                                //V Have to put vertical offset here or it messes up
                Main.EntitySpriteDraw(texture, Projectile.Center + new Vector2(7, 0) - Main.screenPosition, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                    Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
        }
    }
}
