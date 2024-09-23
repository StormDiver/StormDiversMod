using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.GameContent.Drawing;
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles
{
    public class PizzaCutterProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pizza Cutter");
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            //Projectile.CloneDefaults(509);
            // aiType = 509;
            Projectile.aiStyle = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true; //so you can't hit enemies through walls
            Projectile.DamageType = DamageClass.Melee;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            //Projectile.ContinuouslyUpdateDamage = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[2] = 45;
        }
        public override void AI()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            var player = Main.player[Projectile.owner];

            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage);
            Projectile.ai[2]++;

            AnimateProjectile();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 11);
                dust.noGravity = true;
                dust.scale = 0.75f;
            }
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(Projectile.Center.X + Main.rand.Next(-Projectile.width / 3, Projectile.width / 3), Projectile.Center.Y + Main.rand.Next(-Projectile.height / 3, Projectile.height / 3)),

            }, player.whoAmI);
            if (Main.rand.Next(1) == 0 && Projectile.ai[2] >= 90 && Collision.CanHit(Projectile.Center, 0, 0, player.Center, 0, 0))
            {
                Vector2 perturbedSpeed = new Vector2(0, -8).RotatedByRandom(MathHelper.ToRadians(120));

                int ProjID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<PizzaCutterProj2>(), (int)(Projectile.damage * 0.66f), 0, Projectile.owner);
                SoundEngine.PlaySound(SoundID.Item71, player.Center);

                for (int i = 0; i < 50; i++) //Black particles
                {
                    Vector2 perturbedSpeed2 = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));

                    var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 11, perturbedSpeed2.X, perturbedSpeed2.Y);
                    dust.noGravity = true;
                    dust.scale = 1.25f;
                }
                Projectile.ai[2] = 0;
            }
        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 2; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }

    }
    //_________________________________________________________________________
    public class PizzaCutterProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magic Water Orb");
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 8;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ArmorPenetration = 15;
        }
        public override bool? CanDamage()
        {
            if (Projectile.ai[1] < 30)
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
            AnimateProjectile();
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(5) == 0)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(Projectile.Center.X + Main.rand.Next(-Projectile.width / 2, Projectile.width / 2), Projectile.Center.Y + Main.rand.Next(-Projectile.height / 2, Projectile.height / 2)),

                }, player.whoAmI);
            }
            Projectile.rotation += 0.4f;
            Projectile.ai[1]++;

            if (Projectile.ai[1] < 30)
            {
                Projectile.velocity *= 0.93f;
            }
                Vector2 move = Vector2.Zero;
            float distance = 400;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
                {
                    if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                    {
                        if (Projectile.ai[1] >= 30)
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
            if (target)
            {
                if (Projectile.ai[1] >= 30)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (6f * Projectile.velocity + move) / 6.5f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 7f)
            {
                vector *= 7f / magnitude;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
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

            SoundEngine.PlaySound(SoundID.Shatter with { Volume = 0.5f, Pitch = 0.25f }, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 11);
                //dust.velocity *= 2;
                dust.scale = 1.2f;
            }

        }
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 2; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
    }
}
