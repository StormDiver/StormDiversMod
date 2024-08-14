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

namespace StormDiversMod.Projectiles
{
    public class BetsyFlameProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Betsy's Flame");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            //Projectile.melee = true;
            Projectile.timeLeft = 300;
            Projectile.light = 0.5f;
            Projectile.scale = 1f;
            
            Projectile.aiStyle = 0;
            //drawOffsetX = -9;
            //drawOriginOffsetY = -9;   
            Projectile.extraUpdates = 1;
        }

        Vector2 dustposition;
        double degrees;
        int hometime; //distance
        public override void OnSpawn(IEntitySource source)
        {
            hometime = Main.rand.Next(30, 61);

            base.OnSpawn(source);
        }
        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            Projectile.ai[2]++;

            //for dust:
            if (Projectile.ai[2] > 2)
            {
                degrees += 10 * Projectile.direction; //The degrees
                for (int i = 0; i < 2; i++)
                {
                    double rad = degrees * (Math.PI / 180); //Convert degrees to radians
                    double dist = 6; //Distance away from the player

                    dustposition.X = Projectile.Center.X - (int)(Math.Cos(rad) * dist);
                    dustposition.Y = Projectile.Center.Y - (int)(Math.Sin(rad) * dist);

                    for (int j = 0; j < 3; j++)
                    {
                        float X = dustposition.X - Projectile.velocity.X / 5f * (float)j;
                        float Y = dustposition.Y - Projectile.velocity.Y / 5f * (float)j;

                        int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 174, 0, 0, 100, default, 1f);

                        Main.dust[dust].position.X = X;
                        Main.dust[dust].position.Y = Y;
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 0.1f;
                    }
                    degrees += 180; //for dust on other side
                }
            }
            //projpostion

                AnimateProjectile();
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.ai[2] > hometime)
            {
                Vector2 move = Vector2.Zero;
                float distance = 1200f;
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
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 300);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 174);

                dust.noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.75f, Pitch = 0.5f }, Projectile.Center);
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
            //Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/BetsyFlameProj_Trail");
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                    color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }

            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.Moccasin;
            color.A = 150;
            return color;
        }
    }
}