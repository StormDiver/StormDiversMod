using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Common;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Buffs;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent;


namespace StormDiversMod.Projectiles
{


    public class SelenianBladeProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Solar Blade");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {

            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            Projectile.CloneDefaults(106);
            AIType = 106;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            DrawOffsetX = -5;
            DrawOriginOffsetY = -5;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
        }

        bool stillspin;
        int stilltime;
        Vector2 dustposition;
        double degrees;
        public override void AI()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.ai[2]++;
            if (Projectile.ai[2] >= 3) //delay dust
            {
                degrees += 20 * Projectile.direction; //The degrees
                for (int i = 0; i < 2; i++)
                {
                    double rad = degrees * (Math.PI / 180); //Convert degrees to radians
                    double dist = 16; //Distance away from the player

                    dustposition.X = Projectile.Center.X - (int)(Math.Cos(rad) * dist);
                    dustposition.Y = Projectile.Center.Y - (int)(Math.Sin(rad) * dist);

                    for (int j = 0; j < 7; j++)
                    {
                        float X = dustposition.X - Projectile.velocity.X / 5f * (float)j;
                        float Y = dustposition.Y - Projectile.velocity.Y / 5f * (float)j;

                        int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 174, 0, 0, 100, default, 1f);

                        Main.dust[dust].position.X = X;
                        Main.dust[dust].position.Y = Y;
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 0.2f;
                    }
                    degrees += 180; //for dust on other side
                }
            }
           /* int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);  //this is the dust that this projectile will spawn
            Main.dust[dust].velocity /= 1f;
            Main.dust[dust].scale = 1f;
            Main.dust[dust].noGravity = true;*/
            Projectile.penetrate = -1;
            if (stillspin)//while active projectile does not move
            {
                Projectile.velocity.Y *= 0f;
                Projectile.velocity.X *= 0f;
                stilltime++; //timer counts up while not moving
            }
            if (stilltime >= 60) //once this time is reached it cannot stay still again
            {
                stillspin = false;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {


            //Main.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 2);
            Player player = Main.player[Projectile.owner];

            target.AddBuff(BuffID.Daybreak, 300);
            for (int i = 0; i < 10; i++)
            {
                 
                var dust = Dust.NewDustDirect(target.position, target.width, target.height, 6);
                dust.scale = 2f;
                dust.noGravity = true;

            }
            if (stilltime < 45) //So the projectile doesn't stay still again
            {
                stillspin = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Main.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 2);
            for (int i = 0; i < 10; i++)
            {               
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                dust.scale = 2f;
                dust.noGravity = true;
            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 1f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 1f;
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(-5f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 75;
            return color;

        }
    }
    
}