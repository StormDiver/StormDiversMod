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
 
    public class FrostSentryProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frostspike Sentry");
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
      
            Projectile.width = 30;
            Projectile.height = 76;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Summon;
            DrawOffsetX = -2;
            DrawOriginOffsetY = 4;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            return true;
        }
        bool animate = false; //Animate every shot
        NPC target;
        public override void AI()
        {
            if (Projectile.velocity.Y < 20)
            {
                Projectile.velocity.Y += 0.5f;
            }         

            Projectile.ai[0]++;//spawntime
            if (Projectile.ai[0] <= 3)
            {
                for (int i = 0; i < 50; i++)
                {

                    int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 180, 0f, 0f, 0, default, 1.5f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                    Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }

            }
            Projectile.rotation = 0;

            Main.player[Projectile.owner].UpdateMaxTurrets();

            if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 20, Projectile.Center.Y - 20 - 18), 40, 40, 180, 0, 0, 130, default, 1f);

                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.5f;
            }
        
            Projectile.ai[1]++;//Shoottime

            Player player = Main.player[Projectile.owner];

            //Getting the npc to fire at
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (player.HasMinionAttackTargetNPC)
                {
                    target = Main.npc[player.MinionAttackTargetNPC];
                }
                else
                {
                    target = Main.npc[i];
                }      

                if (Vector2.Distance(Projectile.Center, target.Center) <= 750f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.CanBeChasedBy() && target.type != NPCID.TargetDummy && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                {
                    target.TargetClosest(true);
                    float projspeed = 15;
                    Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y - 16)) * projspeed;

                    if (Projectile.ai[1] == 57)
                    {
                        animate = true;
                    }

                    if (Projectile.ai[1] > 75)
                    {                
                        for (int j = 0; j < 60; j++)
                        {
                            float speedY = -1.5f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 18), 0, 0, 180, dustspeed.X, dustspeed.Y);
                            Main.dust[dust2].scale = 0.8f;

                            Main.dust[dust2].noGravity = true;
                        }
                        for (int k = 0; k < 10; k++)
                        {
                            Dust dust;

                            dust = Terraria.Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y - 17), 180, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1.5f);
                            dust.noGravity = true;

                        }

                        SoundEngine.PlaySound(SoundID.Item48, Projectile.Center);

                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X - 6, Projectile.Center.Y - 16), new Vector2(velocity.X, velocity.Y),
                            ModContent.ProjectileType<FrostSentryProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                        Projectile.ai[1] = 0;
                    }

                }
               
            }        

            Projectile.frameCounter++;
         
            if (animate) //frames 4-7 when firing
            {
                if (Projectile.frameCounter >= 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame <= 3)
                {
                    Projectile.frame = 4;
                }
                if (Projectile.frame >= 8)
                {
                    animate = false;
                }
            }
            if (!animate)//frames 0-3 when idle
            {
                if (Projectile.frame >= 4 || Projectile.frame < 0)
                {
                    Projectile.frame = 0;
                }
                if (Projectile.frameCounter >= 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
       
        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            for (int i = 0; i < 50; i++)
            {

                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 180, 0f, 0f, 0, default, 1.5f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.4f;
                Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
     
    }
    //_____________________________________________ Proj
    public class FrostSentryProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Sentry Proj");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.78f;

            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X, Projectile.velocity.Y);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= -0.3f;
            }
        }
    
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            target.AddBuff(BuffID.Frostburn2, 180);

            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 135, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 135);
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
            return Color.White;
        }
    }
}
