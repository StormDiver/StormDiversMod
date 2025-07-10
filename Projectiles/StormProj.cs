using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Common;

using Terraria.Utilities;
using StormDiversMod.Items.Accessory;
using System.Security.Policy;

namespace StormDiversMod.Projectiles
{
	//______________________________________________________________________________________________________
	public class StormAccessProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Storm Portal");
			Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }
		public override void SetDefaults()
		{

			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.friendly = true;
			Projectile.ignoreWater = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 999999999;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			DrawOffsetX = 0;
			Projectile.alpha = 0;
			Projectile.DamageType = DamageClass.Generic;
		}
		public override bool? CanDamage()
		{
			return false;
		}
		float movespeed = 10; //Speed of the proj

		Vector2 projpos;
		int cooldown;
		Vector2 mousepos;
        Vector2 dustposition;
        double degrees;
        int accesstype = 3; //start at slot 3 (Accessory 1)
        bool hidden;

        public override void AI()
		{
            Player player = Main.player[Projectile.owner];
            bool collision = Collision.CanHit(player.Center, 0, 0, Main.MouseWorld, 0, 0); //Positioning the portal requires this, returning it does not

			//dust:
			if (!hidden || Projectile.ai[0] == 1)
			{
				degrees += 10; //The degrees
				for (int i = 0; i < 3; i++)
				{
					double rad = degrees * (Math.PI / 180); //Convert degrees to radians
					double dist = 18; //Distance away from the player

					dustposition.X = Projectile.Center.X - (int)(Math.Cos(rad) * dist);
					dustposition.Y = Projectile.Center.Y - (int)(Math.Sin(rad) * dist);

					for (int j = 0; j < 5; j++)
					{
						float X = dustposition.X - Projectile.velocity.X / 5f * (float)j;
						float Y = dustposition.Y - Projectile.velocity.Y / 5f * (float)j;

						int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 226, 0, 0, 100, default, 0.75f);

						Main.dust[dust].position.X = X;
						Main.dust[dust].position.Y = Y;
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0;
					}
					degrees += 120; //for dust on other side
				}
			}
            //LocalAI 0  = orbit postion
            //LocalAI 1 = one frame used to get mousepos

            //Ai 0 = What mode proj is in
            //Ai 1 = Shoottime

            Projectile.rotation += 0.1745f;

            if (Projectile.ai[0] == 0) //Passive Mode
			{
				Projectile.ai[1] = 0; //reset shoottime
				Projectile.localAI[1] = 0; //First frame to get postion
				if (Projectile.owner == Main.myPlayer)
				{				
					float distance = Vector2.Distance(player.Center, Projectile.Center);

					Projectile.localAI[0] += 2;  //Rotation

					movespeed = distance / 10 + 0.5f;

					Vector2 moveTo = player.Center;
					Vector2 move = moveTo - Projectile.Center + new Vector2(projpos.X, projpos.Y); //Postion around player
					float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
					if (magnitude > movespeed)
					{
						move *= movespeed / magnitude;
					}
					Projectile.velocity = move;
				}
				//Factors for calculations
				double deg = (Projectile.localAI[0] - 90);
				double rad = deg * (Math.PI / 180);
				double dist = 75; //Distance away from the player

                //position
                projpos.X = (int)(Math.Cos(rad) * dist);
                projpos.Y = (int)(Math.Sin(rad) * dist);

                for (int i = 0; i < 7; i++) //Go through all 7 accessory slots (slot 3 - 8) (only hide in passive mode)
                {
                    if (player.armor[accesstype].type != ModContent.ItemType<StormCoil>()) //is the celestial barrier in the slot?
                    {
                        accesstype++; //if not, check next slot
                    }
                    else if (player.hideVisibleAccessory[accesstype] && player.armor[accesstype].type == ModContent.ItemType<StormCoil>()) //if so, is the slot set to be hidden?
                    {
                        hidden = true; //if so, hide sprite and disable dust
                        Projectile.hide = true;
                    }
                    else if (!player.hideVisibleAccessory[accesstype] && player.armor[accesstype].type == ModContent.ItemType<StormCoil>()) //if not show sprite and dust
                    {
                        hidden = false;
                        Projectile.hide = false;
                    }
                }
                //Main.NewText("Slot: " + accesstype, 220, 63, 139);

                accesstype = 3; //reset to slot 3
            }

            if (Projectile.ai[0] == 1) //Attack mode
			{
                hidden = false;
                Projectile.hide = false;
                if (Projectile.owner == Main.myPlayer)
				{
					//if (Projectile.localAI[1] == 0)
					{
						mousepos = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y); //Set position always
					}

					float distance = Vector2.Distance(mousepos, Projectile.Center);

					Projectile.ai[1]++; //increase shoottime
					Projectile.localAI[0] = -10; //Reset rotation
					Projectile.localAI[1]++; //First frame to get postion

					movespeed = distance / 10 + 1.5f;
				}

				Vector2 moveTo = mousepos;
				Vector2 move = moveTo - Projectile.Center + new Vector2(0, 0); //Postion around player
				float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);

				if (magnitude > movespeed)
				{
					move *= movespeed / magnitude;
				}

				Projectile.velocity = move;
            }
			//return to player if distance is too great
			float speeddistance = (float)System.Math.Sqrt((double)((player.Center.X - Projectile.Center.X) * (player.Center.X - Projectile.Center.X) + (player.Center.Y - Projectile.Center.Y) * (player.Center.Y - Projectile.Center.Y)));
			if (speeddistance > 2000 && Projectile.ai[0] == 1)
			{
				
				Projectile.ai[0] = 0;
				cooldown = 0;
				SoundEngine.PlaySound(SoundID.Item93, player.Center);

			}

			cooldown++;
			if (Projectile.owner == Main.myPlayer)
			{
				/*int xcursor = (int)(Main.MouseWorld.X / 16);
				int ycursor = (int)(Main.MouseWorld.Y / 16);
				Tile tile = Main.tile[xcursor, ycursor];*/

				if (player.controlUseTile && player.noThrow == 0 && ((player.controlUp && Projectile.ai[0] == 0 && collision) || (player.controlDown && Projectile.ai[0] == 1)) && cooldown >= 30)//switch between passive and attack
				{
					for (int i = 0; i < 25; i++)
					{
						int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 226, 0f, 0f, 0, default, 1.5f);
						Main.dust[dustIndex].velocity *= 3;

						Main.dust[dustIndex].noGravity = true;
					}
					SoundEngine.PlaySound(SoundID.Item93, player.Center);

					cooldown = 0;
					Projectile.ai[0]++;//Go to next movement postion
				}

				/*if (player.controlUseTile && player.noThrow == 0 && player.controlUp && Projectile.ai[0] == 1 && collision && cooldown >= 30) //Change position in attack mode
				{
					Projectile.localAI[1] = 0; //reset mouse pos detecter
					for (int i = 0; i < 25; i++)
					{
						int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 226, 0f, 0f, 0, default, 1.5f);
						Main.dust[dustIndex].velocity *= 3;

						Main.dust[dustIndex].noGravity = true;
					}
					SoundEngine.PlaySound(SoundID.Item93, player.Center);
					cooldown = 0;
				}*/

			}

			if (Projectile.ai[0] >= 2) //Reset back to first pos
			{
				Projectile.ai[0] = 0;
			}

			if (Projectile.ai[1] > 40 && Vector2.Distance(Main.MouseWorld, Projectile.Center) <= 200 && collision) //fire lightning if line of sight with player and close to cursor 
			{

				for (int j = 0; j < 30; j++)
				{
					int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 16, Projectile.Center.Y - 16), 32, 32, 226, 0, 0, 130, default, 0.5f);

					Main.dust[dust].noGravity = true; 
					Main.dust[dust].velocity *= 2f;
				}
				for (int k = 0; k < 10; k++)
				{
					Dust dust;

					dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 111, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 2.5f);
					dust.noGravity = true;

				}


				SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
				float numberProjectiles = 12;
				float rotation = MathHelper.ToRadians(180);
				for (int j = 0; j < numberProjectiles; j++) //Lightning is just for visuals
				{

					Vector2 perturbedSpeed = new Vector2(0, 2f).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));

					float ai = Main.rand.Next(100);
					int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
						ModContent.ProjectileType<StormLightningProj>(), 0, 0, Projectile.owner, perturbedSpeed.ToRotation(), ai);

					Main.projectile[projID].tileCollide = false;
                    Main.projectile[projID].timeLeft = 110;

                    Main.projectile[projID].scale = 0.6f;


				}
				//Invisible expanding projectile deals damage

				int projID2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0),
						ModContent.ProjectileType<StormExplosionProj>(), Projectile.damage, .5f, Projectile.owner);

				Main.projectile[projID2].extraUpdates = 2;

				Projectile.ai[1] = 0;

			}

			if (player.GetModPlayer<EquipmentEffects>().stormBossAccess == false || player.dead)
			{
				Projectile.Kill();
				return;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
		public override void OnKill(int timeLeft)
		{

			SoundEngine.PlaySound(SoundID.Item122 with {Volume = 0.5f}, Projectile.Center);
			for (int i = 0; i < 30; i++)
			{
				float speedY = -3f;

				Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

				int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 0.75f);
				Main.dust[dust2].noGravity = true;
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
				Color color = Color.White;
				color.A = 150;
				return color;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
    }
	//____________________________________________________Melee weapon
	public class StormKnifeProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Overloaded Knife");
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.light = 0.6f;
			Projectile.friendly = true;


			Projectile.aiStyle = 2;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.timeLeft = 300;
			DrawOffsetX = 0;
			DrawOriginOffsetY = 0;
			Projectile.extraUpdates = 1;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}


		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

			if (Main.rand.Next(3) == 0)
			{
				var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 226);
				dust.noGravity = true;
				dust.scale = 0.75f;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			SoundEngine.PlaySound(SoundID.Item92, Projectile.Center);

			float xpos = (Main.rand.NextFloat(-50, 50));

			float ai = Main.rand.Next(100);

			Vector2 rotation = -new Vector2(target.Center.X - xpos, target.Center.Y - 500) + target.Center;

			int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(target.Center.X - xpos, target.Center.Y - 500), new Vector2(xpos * 0.02f, 5),
				ModContent.ProjectileType<Projectiles.StormLightningProj>(), Projectile.damage, .5f, Main.myPlayer, rotation.ToRotation(), ai);
			Main.projectile[projID].scale = 1;
			Main.projectile[projID].penetrate = 2;
			Main.projectile[projID].timeLeft = 190;
			Main.projectile[projID].DamageType = DamageClass.Melee;
			Main.projectile[projID].tileCollide = false;


			for (int i = 0; i < 25; i++)
			{
				float speedY = -3f;

				Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

				int dust2 = Dust.NewDust(new Vector2(target.Center.X - xpos, target.Center.Y - 500), 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 1.5f);
				Main.dust[dust2].noGravity = true;
			}

		}
		public override void OnKill(int timeLeft)
		{

			SoundEngine.PlaySound(SoundID.NPCHit53 with {Volume = 0.5f, Pitch = -0.5f, MaxInstances = 0}, Projectile.Center);

			for (int i = 0; i < 20; i++)
			{
				float speedY = -3f;

				Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

				int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 1.5f);
				Main.dust[dust2].noGravity = true;
			}

		}
		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;

		}

	}
	//_______________________________________________ Grenade
	public class StormGrenadeProj : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Overloaded Grenade");
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		}
		public override void SetDefaults()
		{

			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 60;
			Projectile.aiStyle = 14;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}
		int extraTime = 50;
		float lightningSpeed = 1.4f;
		int dustYspeed = 10;
		public override void AI()
		{
			Projectile.localAI[1]++;
			if (Main.rand.Next(1) == 0)
			{
				var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 226);
				dust.noGravity = true;
				dust.scale = 0.75f;
			}
			if (Projectile.localAI[1] >= 10)
			{
				for (int i = 0; i < 20; i++)
				{
					float speedY = -3f;

					Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

					int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 0.75f);
					Main.dust[dust2].noGravity = true;
				}

				Projectile.localAI[1] = 0;
			}

			if (Projectile.ai[1] == 0) //Rockets I/II/Liquids
			{
				extraTime = 50;
				lightningSpeed = 1.2f;
				dustYspeed = 10;
			}
			else if (Projectile.ai[1] == 1) //Rocket III/IV
			{
				extraTime = 0;
				lightningSpeed = 1.5f;
				dustYspeed = 13;

			}
			else if (Projectile.ai[1] == 2) //Mini Nukes/ Cluster I
			{
				extraTime = -50;
				lightningSpeed = 1.75f;
				dustYspeed = 16;

			}
			else if (Projectile.ai[1] == 3) //Mini Nukes/ Cluster II
			{
				extraTime = -100;
				lightningSpeed = 2f;
				dustYspeed = 20; 

			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{


		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return true;
		}
		
		public override void OnKill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
				float numberProjectiles = 12;
				float rotation = MathHelper.ToRadians(180);

				for (int j = 0; j < numberProjectiles; j++) //Lightning is just for visuals
				{

					Vector2 perturbedSpeed = new Vector2(0, lightningSpeed).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));

					float ai = Main.rand.Next(100);
					int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
						ModContent.ProjectileType<StormLightningProj>(), 0, 0, Projectile.owner, perturbedSpeed.ToRotation(), ai); 

					Main.projectile[projID].tileCollide = false;
					Main.projectile[projID].scale = 0.7f;
                    Main.projectile[projID].timeLeft = 130;
                    Main.projectile[projID].DamageType = DamageClass.Ranged;
				}

				
				//Invisible expanding projectile deals damage
				int projID2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0),
						ModContent.ProjectileType<StormExplosionProj>(), Projectile.damage, .5f, Projectile.owner, extraTime); //add 50 onto the timer for rocket I, 0 for rocket III, and -50 for Mini nukes

				Main.projectile[projID2].extraUpdates = 2;
				Main.projectile[projID2].DamageType = DamageClass.Ranged;

				for (int i = 0; i < 50; i++)
				{
					Vector2 dustspeed = new Vector2(0, dustYspeed).RotatedByRandom(MathHelper.ToRadians(360));

					int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 1.5f);
					Main.dust[dust2].noGravity = true;
				}

			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;

		}
	}
	//_____________________________________________ Lightning
	public class StormLightningProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Overloaded Lightning");
			ProjectileID.Sets.SentryShot[Projectile.type] = true;

		}
		public override void SetDefaults()
		{

			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 88;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 4;
			Projectile.timeLeft = 300;
			Projectile.scale = 0.75f;
	
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;

		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.localAI[1] < 1f)
			{
				Projectile.localAI[1] += 2f;
				Projectile.position += Projectile.velocity;
				Projectile.velocity = Vector2.Zero;
			}
			return false;
		}

		public override bool? Colliding(Rectangle myRect, Rectangle targetRect)
		{
			for (int i = 0; i < Projectile.oldPos.Length && (Projectile.oldPos[i].X != 0f || Projectile.oldPos[i].Y != 0f); i++)
			{
				myRect.X = (int)Projectile.oldPos[i].X;
				myRect.Y = (int)Projectile.oldPos[i].Y;
				if (myRect.Intersects(targetRect))
				{
					return true;
				}
			}
			return false;
		}


		public override bool PreDraw(ref Color lightColor)
		{

			Color color = Lighting.GetColor((int)((double)Projectile.position.X + (double)Projectile.width * 0.5) / 16, (int)(((double)Projectile.position.Y + (double)Projectile.height * 0.5) / 16.0));
			Vector2 end = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Projectile.GetAlpha(color);
			Vector2 vector = new Vector2(Projectile.scale) / 2f;
			for (int i = 0; i < 2; i++)
			{
				float num = ((Projectile.localAI[1] == -1f || Projectile.localAI[1] == 1f) ? (-0.2f) : 0f);
				if (i == 0)
				{
					vector = new Vector2(Projectile.scale) * (0.5f + num);
					DelegateMethods.c_1 = new Color(34, 221, 151, 0) * 0.5f;
				}
				else
				{
					vector = new Vector2(Projectile.scale) * (0.3f + num);
					DelegateMethods.c_1 = new Color(255, 255, 255, 0) * 0.5f;
				}
				DelegateMethods.f_1 = 1f;
				for (int j = Projectile.oldPos.Length - 1; j > 0; j--)
				{
					if (!(Projectile.oldPos[j] == Vector2.Zero))
					{
						Vector2 start = Projectile.oldPos[j] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
						Vector2 end2 = Projectile.oldPos[j - 1] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
						Utils.DrawLaser(Main.spriteBatch, tex, start, end2, vector, DelegateMethods.LightningLaserDraw);
					}
				}
				if (Projectile.oldPos[0] != Vector2.Zero)
				{
					DelegateMethods.f_1 = 1f;
					Vector2 start2 = Projectile.oldPos[0] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
					Utils.DrawLaser(Main.spriteBatch, tex, start2, end, vector, DelegateMethods.LightningLaserDraw);
				}
			}
			return false;
		}

		public override void AI()
		{

			if (Projectile.scale > 0.05f)
			{
				Projectile.scale -= 0.005f;
			}
			else
			{
				Projectile.timeLeft = 0;
			}
		
			if (Projectile.localAI[1] == 0f && Projectile.ai[0] >= 900f)
			{
				Projectile.ai[0] -= 1000f;
				Projectile.localAI[1] = -1f;
			}
			int frameCounter = Projectile.frameCounter;
			Projectile.frameCounter = frameCounter + 1;
			if (!Main.dedServ)
			{
				Lighting.AddLight(Projectile.Center, 0.3f, 0.45f, 0.5f);
			}
			if (Projectile.velocity == Vector2.Zero)
			{
				if (Projectile.frameCounter >= Projectile.extraUpdates * 2)
				{
					Projectile.frameCounter = 0;
					bool flag = true;
					for (int i = 1; i < Projectile.oldPos.Length; i++)
					{
						if (Projectile.oldPos[i] != Projectile.oldPos[0])
						{
							flag = false;
						}
					}
					if (flag)
					{
						Projectile.Kill();
						return;
					}
				}
				if (Main.rand.Next(Projectile.extraUpdates) == 0)
				{
					for (int j = 0; j < 2; j++)
					{
						float num = Projectile.rotation + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
						float num2 = (float)Main.rand.NextDouble() * 0.8f + 1f;
						Vector2 vector = new Vector2((float)Math.Cos(num) * num2, (float)Math.Sin(num) * num2);
						int num3 = Dust.NewDust(Projectile.Center, 0, 0, 226, vector.X, vector.Y);
						Main.dust[num3].noGravity = true;
						Main.dust[num3].scale = 1.2f;
					}
					if (Main.rand.Next(5) == 0)
					{
						Vector2 vector2 = Projectile.velocity.RotatedBy(1.5707963705062866) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;
						int num4 = Dust.NewDust(Projectile.Center + vector2 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
						Dust dust = Main.dust[num4];
						dust.velocity *= 0.5f;
						Main.dust[num4].velocity.Y = 0f - Math.Abs(Main.dust[num4].velocity.Y);
					}
				}
			}
			else
			{
				if (Projectile.frameCounter < Projectile.extraUpdates * 2)
				{
					return;
				}
				Projectile.frameCounter = 0;
				float num5 = Projectile.velocity.Length();
				UnifiedRandom unifiedRandom = new UnifiedRandom((int)Projectile.ai[1]);
				int num6 = 0;
				Vector2 spinningpoint = -Vector2.UnitY;
				while (true)
				{
					int num7 = unifiedRandom.Next();
					Projectile.ai[1] = num7;
					num7 %= 100;
					float f = (float)num7 / 100f * ((float)Math.PI * 2f);
					Vector2 vector3 = f.ToRotationVector2();
					if (vector3.Y > 0f)
					{
						vector3.Y *= -1f;
					}
					bool flag2 = false;
					if (vector3.Y > -0.02f)
					{
						flag2 = true;
					}
					if (vector3.X * (float)(Projectile.extraUpdates + 2) * 2f * num5 + Projectile.localAI[0] > 40f)
					{
						flag2 = true;
					}
					if (vector3.X * (float)(Projectile.extraUpdates + 2) * 2f * num5 + Projectile.localAI[0] < -40f)
					{
						flag2 = true;
					}
					if (flag2)
					{
						if (num6++ >= 100)
						{
							Projectile.velocity = Vector2.Zero;
							/*if (Projectile.localAI[1] < 1f)
							{
								Projectile.localAI[1] += 2f;
							}*/
							Projectile.localAI[1] = 1f;
							break;
						}
						continue;
					}
					spinningpoint = vector3;
					break;
				}
				if (Projectile.velocity != Vector2.Zero)
				{

					Projectile.localAI[0] += spinningpoint.X * (float)(Projectile.extraUpdates + 1) * 2f * num5;
					Projectile.velocity = spinningpoint.RotatedBy(Projectile.ai[0] + (float)Math.PI / 2f) * num5;
					Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
				}
			}
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.damage = (Projectile.damage * 8) / 10;
        }
        public override void OnKill(int timeLeft)
		{
			if (Main.rand.Next(50) == 0)
			{
				float num = Projectile.rotation + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
				float num2 = (float)Main.rand.NextDouble() * 0.8f + 1f;
				Vector2 vector = new Vector2((float)Math.Cos(num) * num2, (float)Math.Sin(num) * num2);
				int num3 = Dust.NewDust(Projectile.Center, 0, 0, 226, vector.X, vector.Y);
				Main.dust[num3].noGravity = true;
				Main.dust[num3].scale = 1.2f;
			}
		}
	}
	public class StormExplosionProj : ModProjectile //used for damage instead of lightning in explosions
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Overloaded Lightning Explosion");

		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = -1;
		
			Projectile.friendly = true;
		
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.knockBack = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;

		}

		public override void AI()
		{
			Projectile.velocity.X = 0;
			Projectile.velocity.Y = 0;
			//increases size and keeps it in place
			Projectile.width += 8;
			Projectile.height += 8;
			Projectile.position.X -= 4f;
			Projectile.position.Y -= 4f;
			Projectile.ai[0] += 8;
			//Projectile.scale += 0.1f;
			if (Projectile.ai[0] > 250)//kill when timer is reached, when fired from launcher grenades timer starts at 50/0/-50
            {
                Projectile.Kill();
            }
		}
	
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{

			return false;
		}
		public override void OnKill(int timeLeft)
		{

			


		}

	}
	//_________________________________________________________________
    public class StormStaffProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("StormLightningBolt");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.friendly = true;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;

            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 3;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<NPCEffects>().stormimmunetime > 0 || target.friendly) //Npcs immune to explosion when activating it
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void AI()
        {
            Projectile.localAI[1]++;

            if (Projectile.localAI[1] >= 6)
            {
                for (int i = 0; i < 3; i++)
                {
                    float speedY = -0.75f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 226, dustspeed.X, dustspeed.Y, 229, default, 1f);
                    Main.dust[dust2].noGravity = true;
                }
                Projectile.localAI[1] = 0;
            }
            Dust dust;
            dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 226, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;


            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }
        int targetlimit;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 0.5f, Pitch = -0.1f, MaxInstances = 0 }, Projectile.Center);

            Projectile.ai[2]++;
            if (Projectile.ai[2] <= 1)// one set
            {
                target.GetGlobalNPC<NPCEffects>().stormimmunetime = 5;// makes sure the enemy that summons the proj can't get hit by it
                for (int i = 0; i < Main.maxNPCs; i++) //Shoots sand at one enemy
                {
                    NPC npctarget = Main.npc[i];

                    npctarget.TargetClosest(true);
                    if (Vector2.Distance(Projectile.Center, npctarget.Center) <= 700f && !npctarget.friendly && npctarget.active && !npctarget.dontTakeDamage && npctarget.lifeMax > 5 && npctarget.type != NPCID.TargetDummy
                        && Collision.CanHit(Projectile.Center, 0, 0, npctarget.Center, 0, 0) && npctarget.GetGlobalNPC<NPCEffects>().stormimmunetime == 0 && targetlimit <= 2) //3 limit
                    {
                        targetlimit++;

                        Vector2 velocity = Vector2.Normalize(new Vector2(npctarget.Center.X, npctarget.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * 10;

                        int ProjID = Projectile.NewProjectile(npctarget.GetSource_FromAI(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.StormStaffProj>(), Projectile.damage / 3, 0.5f, Main.myPlayer);
                        Main.projectile[ProjID].ai[2] = Projectile.ai[2];
                        Main.projectile[ProjID].timeLeft = 120;
                        Main.projectile[ProjID].ArmorPenetration = 20;
                        Main.projectile[ProjID].extraUpdates = 10;

                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 0.5f, Pitch = -0.1f, MaxInstances = 0 }, Projectile.Center);

            Projectile.ai[2]++;
            if (Projectile.ai[2] <= 1)// one set
            {
                for (int i = 0; i < Main.maxNPCs; i++) //Shoots sand at one enemy
                {
                    NPC npctarget = Main.npc[i];

                    npctarget.TargetClosest(true);
                    if (Vector2.Distance(Projectile.Center, npctarget.Center) <= 700f && !npctarget.friendly && npctarget.active && !npctarget.dontTakeDamage && npctarget.lifeMax > 5 && npctarget.type != NPCID.TargetDummy
                        && Collision.CanHit(Projectile.Center, 0, 0, npctarget.Center, 0, 0) && npctarget.GetGlobalNPC<NPCEffects>().stormimmunetime == 0 && targetlimit <= 3) //4 limit
                    {
                        targetlimit++;

                        Vector2 velocity = Vector2.Normalize(new Vector2(npctarget.Center.X, npctarget.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * 10;

                        int ProjID = Projectile.NewProjectile(npctarget.GetSource_FromAI(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.StormStaffProj>(), Projectile.damage / 2, 0, Main.myPlayer);
                        Main.projectile[ProjID].ai[2] = Projectile.ai[2];
                        Main.projectile[ProjID].timeLeft = 120;
                        Main.projectile[ProjID].ArmorPenetration = 20;
                        Main.projectile[ProjID].extraUpdates = 10;
                    }
                }
            }
            Projectile.Kill();
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 229, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 100, default, 1.2f);
                dust.noGravity = true;
            }
            for (int i = 0; i < 20; i++)
            {
                Vector2 dustspeed = new Vector2(0, 2).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 1f);
                Main.dust[dust2].noGravity = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
