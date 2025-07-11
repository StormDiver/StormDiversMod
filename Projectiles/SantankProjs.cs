﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles
{
    public class SantankMissleProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ho-Ho-Homing missile");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
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
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        int timer;
        public override void AI()
        {
            var player = Main.player[Projectile.owner];

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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionGenericProj>(), 0, 0, Projectile.owner);
            //Main.projectile[proj].scale = 1;

            for (int i = 0; i < 30; i++) //Orange particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -8f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 2f;

            }
            for (int i = 0; i < 50; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -7f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
            }
        }
    }
    //________________________________________________________________________________________
    public class SantaBoomProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Santa's Boomstick Explosion");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(100); //12 smaller than width
            DrawOffsetX = -6; //half of difference
            DrawOriginOffsetY = -12; //all of difference

            Projectile.light = 0.3f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = -1;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ownerHitCheck = true;
        }
        public override bool? CanDamage()
        {
            if (Projectile.timeLeft < 27)
                return false;
            else
                return true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            var player = Main.player[Projectile.owner];
           
        }
        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            Projectile.ai[2]++;

            if (Projectile.ai[2] == 1)
            {
                for (int i = 0; i < 30; i++) //Orange particles
                {
                    Vector2 perturbedSpeed = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(35));
                    float scale = 1f - (Main.rand.NextFloat() * .7f);
                    perturbedSpeed = perturbedSpeed * scale;

                    var dust = Dust.NewDustDirect(Projectile.Center + perturbedSpeed * 2, 0, 0, 174, perturbedSpeed.X * .6f + player.velocity.X, perturbedSpeed.Y * .6f + player.velocity.Y);
                    dust.noGravity = true;
                    dust.scale = 2f;

                    var dust2 = Dust.NewDustDirect(Projectile.Center + perturbedSpeed * 2, 0, 0, 31, perturbedSpeed.X * .6f + player.velocity.X, perturbedSpeed.Y * .6f + player.velocity.Y);
                    dust2.noGravity = true;
                    dust2.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    dust2.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                }
            }

            player.heldProj = Projectile.whoAmI; //appear in front of player but behind hand

            Vector2 velocity = Projectile.velocity * 6f; //keep at 6

            Projectile.Center = player.RotatedRelativePoint(new Vector2(player.MountedCenter.X + velocity.X, player.MountedCenter.Y - (6 * player.gravDir) + velocity.Y));
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 4)
                Projectile.Kill();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }

    //________________________________________________________________________________________
    public class SantaBoomProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Santa's Boomstick Large Explosion");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(140); //36 smaller than width
            DrawOffsetX = -18; //half of difference
            DrawOriginOffsetY = -36; //all of difference

            Projectile.light = 0.3f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = -1;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
           
        }
        public override bool? CanDamage()
        {
            if (Projectile.timeLeft < 25)
                return false;
            else
                return true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            var player = Main.player[Projectile.owner];
            //consume 1 extra ammo
            int projToShoot = player.HeldItem.shoot; //need all this otherwise it has a fit
            int usedAmmoItemID = player.HeldItem.useAmmo;
            float speed = player.HeldItem.shootSpeed;
            int Damage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
            float KnockBack = player.inventory[player.selectedItem].knockBack;

            player.PickAmmo(player.inventory[player.selectedItem], out projToShoot, out speed, out Damage, out KnockBack, out usedAmmoItemID, false);
        }
            
        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            Projectile.ai[2]++;

            if (Projectile.ai[2] == 1)
            {
                for (int i = 0; i < 50; i++) //Orange particles
                {
                    Vector2 perturbedSpeed = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(50));
                    float scale = 1f - (Main.rand.NextFloat() * .7f);
                    perturbedSpeed = perturbedSpeed * scale;

                    var dust = Dust.NewDustDirect(Projectile.Center + perturbedSpeed * 2, 0, 0, 174, perturbedSpeed.X * .8f + player.velocity.X, perturbedSpeed.Y * .8f + player.velocity.Y);
                    dust.noGravity = true;
                    dust.scale = 2f;

                    var dust2 = Dust.NewDustDirect(Projectile.Center + perturbedSpeed * 2, 0, 0, 31, perturbedSpeed.X * .8f + player.velocity.X, perturbedSpeed.Y * .8f + player.velocity.Y);
                    dust2.noGravity = true;
                    dust2.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    dust2.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                }
            }

            player.heldProj = Projectile.whoAmI; //appear in front of player but behind hand

            Vector2 velocity = Projectile.velocity * 7.2f; //keep at 7.2f

            Projectile.Center = player.RotatedRelativePoint(new Vector2(player.MountedCenter.X + velocity.X, player.MountedCenter.Y - (6 * player.gravDir) + velocity.Y));
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 4)
                Projectile.Kill();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}