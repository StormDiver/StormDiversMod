using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{

    public class SantankMissleProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ho-Ho-Homing missile");
            Main.projFrames[Projectile.type] = 4;

        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

           
            Projectile.friendly = true;

            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        int timer;
        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            /*if (Projectile.position.Y > (player.position.Y - 250) && Projectile.position.Y < player.position.Y)
            {
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.tileCollide = true;

            }*/
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            timer++;
            if (timer > 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    float X = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                    float Y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;


                    int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 6, 0, 0, 100, default, 1f);
                    Main.dust[dust].position.X = X;
                    Main.dust[dust].position.Y = Y;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;
                }
            }
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 160;
                Projectile.height = 160;
                Projectile.Center = Projectile.position;
                

                Projectile.knockBack = 3f;
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                
                
            }
            if (timer > 45)
            {
                if (Projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;
                float distance = 1000f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
                    {
                        if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                        {
                            Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                            if (distanceTo < distance)
                            {
                                move = newMove;
                                distance = distanceTo;
                                target = true;
                            }
                        }
                    }
                }
                if (target)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            if (timer > 45)
            {
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                if (magnitude > 15f)
                {
                    vector *= 15f / magnitude;
                }
            }
        }
        public override bool? CanDamage()
        {
            if (timer < 45)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (timer < 45)
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.8f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
                }
            }
            if (Projectile.timeLeft > 3 && timer >= 45)
            {
                Projectile.timeLeft = 3;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);



            for (int i = 0; i < 60; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 4f;
                
            }
            for (int i = 0; i < 50; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.Center;
                dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 2f;
                dust.velocity *= 3;


            }
            for (int i = 0; i < 50; i++)
            {

                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default, 1f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }

    }
    
}