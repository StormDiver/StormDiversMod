using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;


namespace StormDiversMod.Projectiles
{
    
    public class BoneAcProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spinning bone");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.CloneDefaults(131);
            AIType = 131;
            Projectile.friendly = true;
            Projectile.penetrate = 4;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = true;

            //drawOffsetX = 2;
            //drawOriginOffsetY = 2;

        }
        float distance = 200;
        public override void AI()
        {
            Projectile.light = 0;
            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
           
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
                {
                    if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                    {
                        distance = 200;
                    }
                    else
                    {
                        distance = 100;

                    }
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
            if (target)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 7f;
                AdjustMagnitude(ref Projectile.velocity);
            }
         

        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 5f)
            {
                vector *= 5f / magnitude;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 1;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 1f;
            }
            return false;
        }

    }
   

}
