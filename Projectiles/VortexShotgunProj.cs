﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;
using StormDiversMod.Basefiles;
using static Terraria.ModLoader.ModContent;
using static System.Formats.Asn1.AsnWriter;
using static Humanizer.In;

namespace StormDiversMod.Projectiles
{
    public class VortexShotgunGun : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Diver Shotgun");
            Main.projFrames[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            //Projectile.CloneDefaults(509);
            // aiType = 509;
            //Projectile.aiStyle = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true; //so you can't hit enemies through walls
            Projectile.DamageType = DamageClass.Ranged;
            DrawOffsetX = 3;
            DrawOriginOffsetY = 0;
            //Projectile.ContinuouslyUpdateDamage = true;
            Projectile.alpha = 255;
        }
        public override bool? CanDamage() => false;

        float angle = 45;
        float extravel = 1f;
        float sound = -0.5f;
        bool maxcharge;
        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            if (!maxcharge) //charge up, lower angle, add velocity to bullets, and increase damage
            {
                angle -= 0.5f; //takes 90 frames to charge
                extravel += 0.01f;
                Projectile.damage += 5; //Extra 450 damage from base, + additonal 25% at max charge (100 + 450 = 550 * 1.2 = 660),
                                        //4.5 bullets on average, add addition 30 frame cooldown everytime, fewer bullet will likely hit at lower charges
                                        //(base damage X Shots per second X 4.5)
                                        //~900dps  at no charge, (2 shots per second, 100 base damage)
                                        //~1125dps at half second charge (1 shot per second, 250 base damage)
                                        //~1188dps at second charge (0.66 shots per second, 400 base damage)
                                        //~1238dps almost full charge, (0.5 shots per second, 550 base damage)
                                        //~1485dps at max charge (~0.5 shots per second, 660 base damage)
            }
            if (angle <= 0.5f)//Charge up time is 100 frames as angle starts at 50
            {
                maxcharge = true;
            }
            //Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0 && !maxcharge) //Charge up sound and effect
            {
                sound += 0.2f;

                SoundEngine.PlaySound(SoundID.Item33 with { Volume = 0.2f, Pitch = sound, MaxInstances = 1 }, base.Projectile.position);
                for (int i = 0; i < 10; i++)
                {
                    int dust2 = Dust.NewDust(Projectile.position + Projectile.velocity * Main.rand.Next(6, 10) * 0.1f, Projectile.width, Projectile.height, 229, 0f, 0f, 80, default(Color), 0.75f);
                    Main.dust[dust2].position.X -= 4f;
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].velocity *= 0.2f;
                    //Main.dust[dust2].velocity.Y = (float)(-Main.rand.Next(7, 13)) * 0.15f;
                }
                Projectile.soundDelay = 15;
            }

            //Main.NewText("Tester " + angle, 0, 204, 170); //Inital Scale

            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 10f; // Position of end of barrel

            if (Collision.CanHit(Projectile.position, 0, 0, Projectile.position + muzzleOffset, 0, 0))
            {
                Projectile.position += muzzleOffset;
            }
            if (maxcharge) //At full charge new dust and sound
            {
                if (Projectile.soundDelay <= 0)
                {
                    SoundEngine.PlaySound(SoundID.Item33 with { Volume = 0.2f, Pitch = 1, MaxInstances = 0, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, base.Projectile.position);
                    Projectile.soundDelay = 8;
                }
                Vector2 dustspeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);

                int dust2 = Dust.NewDust(Projectile.Center + new Vector2(-5, -5), 0, 0, 229, dustspeed.X * 0.25f, dustspeed.Y * 0.25f, 229, default, 1.5f);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].scale = 1f;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 1) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 2; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
            //=============================================================================================Code for movement of weapon projectile====================================================================================================================================================
            Vector2 vector13 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                if (Main.player[Projectile.owner].channel)
                {
                    float num171 = Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].shootSpeed * Projectile.scale;
                    Vector2 vector14 = vector13;
                    float num172 = (float)Main.mouseX + Main.screenPosition.X - vector14.X;
                    float num173 = (float)Main.mouseY + Main.screenPosition.Y - vector14.Y;
                    if (Main.player[Projectile.owner].gravDir == -1f)
                    {
                        num173 = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector14.Y;
                    }
                    float num174 = (float)Math.Sqrt(num172 * num172 + num173 * num173);
                    num174 = (float)Math.Sqrt(num172 * num172 + num173 * num173);
                    num174 = num171 / num174;
                    num172 *= num174;
                    num173 *= num174;
                    if (num172 != base.Projectile.velocity.X || num173 != base.Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }
                    base.Projectile.velocity.X = num172;
                    base.Projectile.velocity.Y = num173;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            /*if (base.Projectile.velocity.X > 0f)
            {
                Main.player[Projectile.owner].ChangeDir(1);
            }
            else if (base.Projectile.velocity.X < 0f)
            {
                Main.player[Projectile.owner].ChangeDir(-1);
            }*/
            Projectile.spriteDirection = Projectile.direction;
            Main.player[Projectile.owner].ChangeDir(Projectile.direction);
            Main.player[Projectile.owner].heldProj = player.whoAmI;
            Main.player[Projectile.owner].SetDummyItemTime(2);
            base.Projectile.position.X = vector13.X - (float)(base.Projectile.width / 2);
            base.Projectile.position.Y = vector13.Y - (float)(base.Projectile.height / 2);
            Projectile.rotation = (float)(Math.Atan2(base.Projectile.velocity.Y, base.Projectile.velocity.X) + 1.5700000524520874);
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.player[Projectile.owner].itemRotation = (float)Math.Atan2(base.Projectile.velocity.Y * (float)Projectile.direction, base.Projectile.velocity.X * (float)Projectile.direction);
            }
            else
            {
                Main.player[Projectile.owner].itemRotation = (float)Math.Atan2(base.Projectile.velocity.Y * (float)Projectile.direction, base.Projectile.velocity.X * (float)Projectile.direction);
            }
            //base.Projectile.velocity.X *= 1f + (float)Main.rand.Next(-3, 4) * 0.01f;        
            //================================================================================================================================================================================================================================================
        }
        public override void Kill(int timeLeft)
        {
            var player = Main.player[Projectile.owner];
            if (maxcharge) //Different sound and screenshake at max charge
            {
                //Projectile.damage *= 2;
                if (!GetInstance<ConfigurationsIndividual>().NoShake)
                {
                    player.GetModPlayer<MiscFeatures>().screenshaker = true;
                }
                SoundEngine.PlaySound(SoundID.Item38 with { Volume = 1f, Pitch = 0f }, player.Center);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item36 with { Volume = 0.5f, Pitch = 0f }, player.Center);
            }
            //To fire the bullets
            bool canShoot = player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed; 
            int projToShoot = 14;
            int usedAmmoItemID = 0;
            float speed = 14f;
            int Damage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
            float KnockBack = player.inventory[player.selectedItem].knockBack;
            if (canShoot)
            {             
                player.PickAmmo(player.inventory[player.selectedItem], out projToShoot, out speed, out Damage, out KnockBack, out usedAmmoItemID, true);
                if (projToShoot == ProjectileID.Bullet && maxcharge)
                {
                    projToShoot = ProjectileID.MoonlordBullet;
                }
                int numberProjectiles = 4 + Main.rand.Next(2); ; //This defines how many projectiles to shot.
                for (int i = 0; i < numberProjectiles; i++)
                {            
                    Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f).RotatedByRandom(MathHelper.ToRadians(angle));    
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2((perturbedSpeed.X * extravel), (float)(perturbedSpeed.Y * extravel)), projToShoot, (int)(Projectile.damage), Projectile.knockBack, Projectile.owner);
                    Main.projectile[projID].usesLocalNPCImmunity = true;
                    Main.projectile[projID].localNPCHitCooldown = 10;

                    if (maxcharge) //Extra speed at max charge
                    {
                        Main.projectile[projID].extraUpdates += 1;
                        Main.projectile[projID].knockBack *= 2;
                        Main.projectile[projID].damage = Projectile.damage + (int)(Projectile.damage / 5); //20% extra damage

                    }
                }           
                if (maxcharge) //extra dust for maxcharge
                {
                    for (int i = 0; i < 30; i++)
                    {
                        float speedY = -1.5f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 229, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 229, default, 1.5f);
                        Main.dust[dust2].noGravity = true;

                    }
                }
            }
        }
    }
}