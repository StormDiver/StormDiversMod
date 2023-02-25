using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;

using Terraria.Utilities;

namespace StormDiversMod.Projectiles.SentryProjs
{

    public class CultistSentryProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cultist Sentry Orb");
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
      
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanDamage()
        {

            return false;
        }
        NPC target;
        public override void AI()
        {
            if (Projectile.alpha == 255)
            {
                SoundEngine.PlaySound(SoundID.Item121, Projectile.Center);

            }
            if (Projectile.alpha > 5)
            {
                Projectile.alpha -= 5;
                
            }
            if (Projectile.alpha == 10)
            {
                for (int i = 0; i < 25; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 226, 0f, 0f, 0, default, 1.5f);
                    Main.dust[dustIndex].velocity *= 3;

                    Main.dust[dustIndex].noGravity = true;
                }
            }

            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;
            
           
            Main.player[Projectile.owner].UpdateMaxTurrets();
			if (!Main.dedServ)
			{
				Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
			}
            
                if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 24, Projectile.Center.Y - 24), 48, 48, 226, 0, 0, 130, default, 1f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                }
            
            //else
            {
                //Projectile.ai[0] += 1f;
            }
            if (Projectile.alpha < 10)
            {
                Projectile.ai[1]++; //Shoottime
            }
            Player player = Main.player[Projectile.owner];
            //Getting the npc to fire at
            for (int i = 0; i < 200; i++)
            {

                if (player.HasMinionAttackTargetNPC)
                {
                    target = Main.npc[player.MinionAttackTargetNPC];
                }
                else
                {
                    target = Main.npc[i];

                }

                if (Vector2.Distance(Projectile.Center, target.Center) <= 900f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.CanBeChasedBy() && target.type != NPCID.TargetDummy)
                {
                    if (Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                    {
                        target.TargetClosest(true);
                        
                        if (Projectile.ai[1] > 35)
                        {
                            float projspeed = 7;
                            Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                           
                            for (int j = 0; j < 30; j++)
                            {
                                int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 16, Projectile.Center.Y - 16), 32, 32, 226, 0, 0, 130, default, 0.5f);

                                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                                Main.dust[dust].velocity *= 2f;
                            }
                            for (int k = 0; k < 10; k++)
                            {
                                Dust dust;
                                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                                
                                dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 111, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 2.5f);
                                dust.noGravity = true;

                            }                         
                                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                                //Projectile.NewProjectile(Projectile.Center.X, Projectile.Top.Y + 14, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("MagmaSentryProj2"), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f);
                                SoundEngine.PlaySound(SoundID.Item122 with { Volume = 0.5f, Pitch = 0f }, Projectile.Center);

                                float ai = Main.rand.Next(100);
                                int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), 
									ModContent.ProjectileType<CultistSentryProj2>(), Projectile.damage, .5f, Projectile.owner, perturbedSpeed.ToRotation(), ai);

                               /* Main.projectile[projID].hostile = false;
                                Main.projectile[projID].friendly = true;
                                Main.projectile[projID].penetrate = 10;
                                Main.projectile[projID].usesLocalNPCImmunity = true;
                                Main.projectile[projID].localNPCHitCooldown = -1;
                                Main.projectile[projID].scale = 0.75f;
                                Main.projectile[projID].timeLeft = 180;
                                Main.projectile[projID].DamageType = DamageClass.Summon;*/

                            Projectile.ai[1] = 0;
                        }
                    }

                }
            }


            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;

            }
            if (Projectile.frame >= 3)
            {

                Projectile.frame = 0;
            }


        }
        

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
       
        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
            for (int i = 0; i < 50; i++)
            {

                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 226, 0f, 0f, 0, default, 1.5f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.alpha <= 5)
            {
                Color color = Color.White;
                color.A = 150;
                return color;
            }
            else
            {
                return null;
            }
        }
    }
	//_____________________________________________ Lightning
	public class CultistSentryProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunatic Lightning");
			ProjectileID.Sets.SentryShot[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

		}
		public override void SetDefaults()
		{

			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 4;
			Projectile.timeLeft = 300;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
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
					DelegateMethods.c_1 = new Color(113, 251, 255, 0) * 0.5f;
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
			if (Projectile.scale > 0.01f)
			{
				Projectile.scale -= 0.005f;
			}
			else
			{
				Projectile.Kill();
			}
			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[0] = Projectile.velocity.ToRotation();
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
				/*if (Main.rand.Next(4) == 0 && Main.netMode != 1 && Projectile.localAI[1] == 0f)
				{
					float num8 = (float)Main.rand.Next(-3, 4) * ((float)Math.PI / 3f) / 3f;
					Vector2 vector4 = Projectile.ai[0].ToRotationVector2().RotatedBy(num8) * Projectile.velocity.Length();
					if (!Collision.CanHitLine(Projectile.Center, 0, 0, Projectile.Center + vector4 * 80f, 0, 0))
					{
						Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X - vector4.X, Projectile.Center.Y - vector4.Y), new Vector2(vector4.X, vector4.Y), Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, vector4.ToRotation() + 1000f, Projectile.ai[1]);
					}
				}*/
			}

		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.damage = (Projectile.damage * 9) / 10;

		}
	}
}
	
