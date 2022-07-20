using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles.SentryProjs
{
    //______________________________________________________________________________________________________
    public class StargazerCoreProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stargazer Sentry");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

        }
        public override void SetDefaults()
        {
       
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            //Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.timeLeft = 36000;
            Projectile.light = 0.6f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanDamage()
        {

            return false;
        }
        
        int firerate = 0;
        int stopfire = 0;
        int opacity = 255;
        NPC target;


        public override void AI()
        {
            if (opacity > 150)
            {
                opacity -= 10;
                for (int i = 0; i < 20; i++)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 111, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);

                    Main.dust[dust].velocity *= 0.5f;
                }
            }
            Projectile.alpha = opacity;
            Projectile.ai[0] += 1; // rotate
            Projectile.rotation = Projectile.ai[0] * 0.1f;
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            Main.player[Projectile.owner].UpdateMaxTurrets();


            {
                if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 111, 0, 10, 130, default, 1f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 111, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
                }
            }


            Player player = Main.player[Projectile.owner];
            Projectile.ai[1]++;//shootime
            firerate++;
            if (Projectile.ai[1] > 90)
            {
                stopfire++;
            }
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

                //Getting the shooting trajectory
                float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y + (float)target.height * 0.5f - Projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                //bool lineOfSight = Collision.CanHitLine(Projectile.Center, 1, 1, target.Center, 1, 1);
                //If the distance between the projectile and the live target is active

                if (distance < 750f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
                {

                    if (Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                    {

                        if (Projectile.ai[1] > 90)
                        {

                            
                            //Dividing the factor of 2f which is the desired velocity by distance
                            distance = 1.6f / distance;

                            //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                            shootToX *= distance * 15f;
                            shootToY *= distance * 15f;

                            
                            if (firerate >= 5)
                            {
                                Vector2 perturbedSpeed = new Vector2(shootToX, shootToY).RotatedByRandom(MathHelper.ToRadians(0));

                                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StargazerCoreProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                                for (int j = 0; j < 20; j++)     //this defines how many dust to spawn
                                {
                                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 111, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);

                                    Main.dust[dust].velocity *= 0.5f;
                                }
                                firerate = 0;
                            }
                            //Main.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 60);
                            if (stopfire >= 25)
                            {
                                Projectile.ai[1] = 0;
                                stopfire = 0;
                                firerate = 0;

                            }
                        }
                    }
                }
               
            }
           



        }
       


      
        public override void Kill(int timeLeft)
        {

            //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 45);
            for (int i = 0; i < 50; i++)
            {

          
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 111);
                dust2.noGravity = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (opacity <= 150)
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
    //_____________________________________________________________________________________________________
    public class StargazerCoreProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stargazer Laser");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {

            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.light = 0.6f;
            Projectile.DamageType = DamageClass.Summon;
        }
        int visible;
        public override void AI()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            visible++;
            
                if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 111, 0, 0, 130, default, 1.5f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                }
            
           
            if (visible <= 3)
            {
                Projectile.alpha = 255;
            }
           
            return;
        }

        

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage -= (Projectile.damage / 10);
            for (int i = 0; i < 20; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 120, default, 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);

                for (int i = 0; i < 20; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0, 0, 120, default, 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 2.5f;
                }

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
        public override Color? GetAlpha(Color lightColor)
        {


            if (visible > 3)
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

}
