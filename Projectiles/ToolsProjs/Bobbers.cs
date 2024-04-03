using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
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
            // These are copied through the CloneDefaults method
            // Projectile.width = 14;
            // Projectile.height = 14;
            // Projectile.aiStyle = 61;
            // Projectile.bobber = true;
            // Projectile.penetrate = -1;
            // Projectile.netImportant = true;
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

        /*public override void ModifyFishingLine(ref Vector2 lineOriginOffset, ref Color lineColor)
        {
            // Change these two values in order to change the origin of where the line is being drawn.
            // This will make it draw 47 pixels right and 31 pixels up from the player's center, while they are looking right and in normal gravity.
            lineOriginOffset = new Vector2(43, -25);
            // Sets the fishing line's color. Note that this will be overridden by the colored string accessories.
            lineColor = FishingLineColor;
        }*/

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
			// These are copied through the CloneDefaults method
			// Projectile.width = 14;
			// Projectile.height = 14;
			// Projectile.aiStyle = 61;
			// Projectile.bobber = true;
			// Projectile.penetrate = -1;
			// Projectile.netImportant = true;
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

		/*public override void ModifyFishingLine(ref Vector2 lineOriginOffset, ref Color lineColor)
		{
			// Change these two values in order to change the origin of where the line is being drawn.
			// This will make it draw 47 pixels right and 31 pixels up from the player's center, while they are looking right and in normal gravity.
			lineOriginOffset = new Vector2(47, -25);
			// Sets the fishing line's color. Note that this will be overridden by the colored string accessories.
			lineColor = FishingLineColor;
		}*/

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
            // Projectile.width = 14;
            // Projectile.height = 14;
            // Projectile.aiStyle = 61;
            // Projectile.bobber = true;
            // Projectile.penetrate = -1;
            // Projectile.netImportant = true;
            Projectile.CloneDefaults(ProjectileID.BobberWooden);

            DrawOriginOffsetY = -8; // Adjusts the draw position
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
            //Main.NewText("Ai 0:" + Projectile.ai[0] + " Ai 1:" + Projectile.ai[1] + " Ai 2:" + Projectile.ai[2], 204, 101, 22);
            // Always ensure that graphics-related code doesn't run on dedicated servers via this check.
            if (!Main.dedServ)
            {
                // Create some light based on the color of the line.
                //Lighting.AddLight(Projectile.Center, FishingLineColor.ToVector3());
            }
        }

        /*public override void ModifyFishingLine(ref Vector2 lineOriginOffset, ref Color lineColor)
        {
            // Change these two values in order to change the origin of where the line is being drawn.
            // This will make it draw 47 pixels right and 31 pixels up from the player's center, while they are looking right and in normal gravity.
            lineOriginOffset = new Vector2(47, -25);
            // Sets the fishing line's color. Note that this will be overridden by the colored string accessories.
            lineColor = FishingLineColor;

            if (Projectile.ai[2] == 0)//solar
                lineColor = new Color(225, 154, 0);
            else if (Projectile.ai[2] == 1) //vortex
                lineColor = new Color(0, 238, 161);
            else if (Projectile.ai[2] == 2) //nebula
                lineColor = new Color(255, 96, 232);
            else  //Stardust
                lineColor = new Color(0, 173, 246);

            lineColor.A = 255;
        }*/

        // 225, 154, 0
        // 0, 238, 161
        // 255, 96, 232
        // 0, 173, 246

        // These last two methods are required so the line color is properly synced in multiplayer.
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
            return Color.White;
        }
    }
}
