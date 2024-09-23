using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework.Graphics;

using Terraria.GameContent.Drawing;
using Terraria.DataStructures;
using System.Linq;

namespace StormDiversMod.Projectiles
{
    public class CoralBoneProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Coral Riptide");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            //Projectile.melee = true;
            Projectile.timeLeft = 480;
            Projectile.light = 0.2f;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.aiStyle = 0;
            DrawOffsetX = -10;
            DrawOriginOffsetY = -10;   
            Projectile.extraUpdates = 1;
            Projectile.ArmorPenetration = 10;
        }

        Vector2 dustposition;
        int hometime;
        bool stophome;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(10 * Projectile.direction);
            var player = Main.player[Projectile.owner];
            Projectile.ai[2]++;

            //for dust:
            if (Projectile.ai[2] > 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    double deg = Main.rand.Next(0, 360); //The degrees
                    double rad = deg * (Math.PI / 180); //Convert degrees to radians
                    double dist = 24; //Distance away from the player
                    dustposition.X = Projectile.Center.X - (int)(Math.Cos(rad) * dist);
                    dustposition.Y = Projectile.Center.Y - (int)(Math.Sin(rad) * dist);

                    float X = dustposition.X - Projectile.velocity.X / 5f * (float)i;
                    float Y = dustposition.Y - Projectile.velocity.Y / 5f * (float)i;

                    int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 33, 0, 0, 100, default, 1f);

                    Main.dust[dust].position.X = X;
                    Main.dust[dust].position.Y = Y;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.1f;
                }
            }
            //projpostion

            if (!stophome)
            {
                Vector2 move = Vector2.Zero;
                float distance = 500;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
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
                    Projectile.velocity = (15 * Projectile.velocity + move) / 15.5f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }
            if (hometime <= 0)
            {
                stophome = false;
            }
            else
                hometime--;

            if (Projectile.damage < 1)
                Projectile.Kill();
            AnimateProjectile();
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 8f)
            {
                vector *= 8f / magnitude;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 33, Projectile.velocity.X, Projectile.velocity.Y);
                dust.scale = 1f;
            }
            SoundEngine.PlaySound(SoundID.Item85 with { Volume = 1f, Pitch = 0.25f}, Projectile.Center);
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 33, Projectile.velocity.X, Projectile.velocity.Y);
                dust.scale = 1f;
            }
            //Projectile.velocity.Y = -5;

            SoundEngine.PlaySound(SoundID.Item85 with { Volume = 1f, Pitch = 0.25f }, Projectile.Center);

            Projectile.damage = (Projectile.damage * 7) / 10;

            stophome = true; //stop homing for 15 frames (7.5)
            hometime = 15;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item85 with { Volume = 1.25f, Pitch = -0f}, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 25, Projectile.Center.Y - 25), 50, 50, 33, Projectile.velocity.X, Projectile.velocity.Y);
                dust.scale = 1.5f;
            }
            for (int i = 0; i < 30; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 25, Projectile.Center.Y - 25), 50, 50, 176, Projectile.velocity.X, Projectile.velocity.Y);
                dust.scale = 1f;
            }
        }
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
        public override bool PreDraw(ref Color lightColor) //trial
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/CoralBoneProj");
             
            /*Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0, 0);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]), color, Projectile.rotation, drawOrigin, Projectile.scale * 0.8f, SpriteEffects.None, 0);
            }*/

            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 200;
            return color;
        }
    }
}