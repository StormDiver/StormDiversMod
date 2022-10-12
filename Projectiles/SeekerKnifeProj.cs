using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Buffs;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent;


namespace StormDiversMod.Projectiles
{


    public class SeekerKnifeProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seeeker Knife");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }
        public override void SetDefaults()
        {


            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            //Projectile.gfxOffY = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;

            Projectile.light = 0.5f;
        }

        int hometime;
        bool homed;
        public override void AI()
        {


            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 250f;
            bool target = false;

            if (hometime <= 60)
            {
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
            }
            if (target && hometime <= 60)
            {
                homed = true;
                hometime++;
                AdjustMagnitude(ref move);
                Projectile.velocity = (15 * Projectile.velocity + move) / 15.5f;
                AdjustMagnitude(ref Projectile.velocity);
            }
            if (homed)
            {
                Projectile.aiStyle = 0;
                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.78f;

               if (Main.rand.Next(5) == 0)
                {
                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, 220);
                    dust.noGravity = true;
                    dust.scale = 0.75f;
                }
            }
            else
            {
                Projectile.aiStyle = 14;

            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {

            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 9f)
            {
                vector *= 9f / magnitude;
            }

        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Projectile.Kill();
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
                for (int i = 0; i < 15; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 220);
                    dust.noGravity = true;
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
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, 0);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
    
    }
    
}