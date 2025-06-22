using System;
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
using StormDiversMod.Common;
using static Terraria.ModLoader.ModContent;
using static System.Formats.Asn1.AsnWriter;
using static Humanizer.In;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;

namespace StormDiversMod.Projectiles
{
    public class CaptainsGunProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Captain Cannon Proj");
            Main.projFrames[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            //Projectile.hide = true; //prevents pre draw from running
            Projectile.alpha = 255; //use instead

            Projectile.ownerHitCheck = true; //so you can't hit enemies through walls
            Projectile.DamageType = DamageClass.Ranged;
            DrawOffsetX = 3;
            DrawOriginOffsetY = 0;
            //Projectile.ContinuouslyUpdateDamage = true;
            
        }
        public override bool? CanDamage() => false;
        int firerate; //counter to shoot
        int firespeed; //current fire speed
        int cannonspeed; //speed to fire cannon
        bool maxspeed;

        int orginaldamage;
        public override void OnSpawn(IEntitySource source)
        {
            var player = Main.player[Projectile.owner];
            //Projectile.damage = player.HeldItem.damage; //starts off at 80, but then ranged buffs are applied after charge, otherwise additon damage is worse at higher charge levels

            firerate = player.HeldItem.useTime; //counter to shoot //20 by default
            firespeed = player.HeldItem.useTime; //current fire speed
            orginaldamage = Projectile.damage;
        }

        public override void AI()
        {
            Projectile.timeLeft = 60;
            var player = Main.player[Projectile.owner];
            //Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage ); //update damage

            //Main.NewText("Speed = " + firespeed, 0, 204, 170); //Inital Scale
            firerate++;

            Projectile.ai[2]++;
            if (Projectile.ai[2] >= 25 && !maxspeed) //speed up every 25 frames, from 20 to 6, 5.83 seconds to reach max speed
            {
                 firespeed--;
                Projectile.ai[2] = 0;

            }
            if (firespeed <= 6 && !maxspeed) //charge up, increase speed until 6
            {
                //SoundEngine.PlaySound(SoundID.Item28 with { Volume = 0.5f, Pitch = 0f }, player.Center);

                maxspeed = true;
            }
           
            Projectile.ai[1]++; //1 frame delay so particles always appear at end
           
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 15f; // Position of end of barrel

            if (Collision.CanHit(Projectile.position, 0, 0, Projectile.position + muzzleOffset, 0, 0))
            {
                Projectile.position += muzzleOffset;
            }
            
            //To fire the bullets
           
            if (firerate >= firespeed)
            {
                bool canShoot = player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed; //checks for ammo and also removes ammo each charge
                int projToShoot = player.HeldItem.shoot; //doesn't work with rockets other than I-IV, so custom rocket
                int usedAmmoItemID = player.HeldItem.useAmmo;
                float speed = player.HeldItem.shootSpeed;
                int Damage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
                float KnockBack = player.inventory[player.selectedItem].knockBack;

                if (canShoot && Collision.CanHit(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, new Vector2(player.Center.X, player.Center.Y), 0, 0))
                {
                    SoundEngine.PlaySound(SoundID.Item36 with { Volume = 0.5f, Pitch = 0f }, player.Center);

                    if (firespeed < player.HeldItem.useTime) //don't consume ammo on first shot
                    player.PickAmmo(player.inventory[player.selectedItem], out projToShoot, out speed, out Damage, out KnockBack, out usedAmmoItemID, false);
                    else
                        player.PickAmmo(player.inventory[player.selectedItem], out projToShoot, out speed, out Damage, out KnockBack, out usedAmmoItemID, true);

                    //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                    for (int i = 0; i < 1; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X * 0.75f, Projectile.velocity.Y * 0.75f).RotatedByRandom(MathHelper.ToRadians(7));
                        int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + (-Projectile.velocity.X), Projectile.Center.Y + 3 + (-Projectile.velocity.Y)), new Vector2((perturbedSpeed.X), (float)(perturbedSpeed.Y)), projToShoot, (int)(Projectile.damage), Projectile.knockBack, Projectile.owner);

                        Main.projectile[projID].usesLocalNPCImmunity = true;
                        Main.projectile[projID].localNPCHitCooldown = 10;

                    }
                    Vector2 dustspeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
                    for (int i = 0; i < 10; i++)
                    {
                        int dust2 = Dust.NewDust(Projectile.Center + new Vector2(-4, -1), 0, 0, 170, dustspeed.X * 0.12f, dustspeed.Y * 0.12f, 100, default, 0.75f);
                        Main.dust[dust2].noGravity = true;
                    }
                   
                }

                firerate = 0;
            }
            //Cannon balls

            if (player.HasItem(ItemID.Cannonball))
            {
                cannonspeed++;
            }
            if (cannonspeed == player.HeldItem.useTime * 3 && !player.controlUseTile && player.HasItem(ItemID.Cannonball))
            {
                SoundEngine.PlaySound(SoundID.Item149 with { Volume = 2f, Pitch = -0.2f, MaxInstances = 0 }, base.Projectile.position);
            }

            if (cannonspeed > player.HeldItem.useTime * 3 && player.controlUseTile) // 20 x 3 = 60
            {
                //bool canShoot = player.HasAmmo(player.inventory[ItemID.Cannonball]) && !player.noItems && !player.CCed; //checks for ammo and also removes ammo each charge
                bool canShoot = player.HasItem(ItemID.Cannonball) && !player.noItems && !player.CCed;
                //int projToShoot = ProjectileID.CannonballFriendly; //doesn't work with rockets other than I-IV, so custom rocket
                //int usedAmmoItemID = ItemID.Cannonball;
                float speed = player.HeldItem.shootSpeed;
                int Damage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
                float KnockBack = player.inventory[player.selectedItem].knockBack;
                if (canShoot && Collision.CanHit(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, new Vector2(player.Center.X, player.Center.Y), 0, 0))
                {
                    //player.PickAmmo(player.inventory[player.selectedItem], out projToShoot, out speed, out Damage, out KnockBack, out usedAmmoItemID, false);
                    player.ConsumeItem(ItemID.Cannonball);

                    SoundEngine.PlaySound(SoundID.Item38 with { Volume = 1f, Pitch = 0f }, player.Center);
                    //Main.NewText("EXCUSE ME! = ", 0, 204, 170); //Inital Scale

                    Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X * 0.6f, Projectile.velocity.Y * 0.6f).RotatedByRandom(MathHelper.ToRadians(15));
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + (-Projectile.velocity.X), Projectile.Center.Y + 13 - (Projectile.velocity.Y)), new Vector2((perturbedSpeed.X), (float)(perturbedSpeed.Y - 1.5f)), ProjectileID.CannonballFriendly, (int)(Projectile.damage) * 4, Projectile.knockBack, Projectile.owner);
                    Main.projectile[projID].usesLocalNPCImmunity = true;
                    Main.projectile[projID].localNPCHitCooldown = -1;
                    Main.projectile[projID].DamageType = DamageClass.Ranged;
                    Main.projectile[projID].ai[0] = 2; //start smoke as soon as spawned

                }

                cannonspeed = 0;
            }

            //=============================================================================================Code for movement of weapon projectile====================================================================================================================================================
            Vector2 vector13 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                if (Main.player[Projectile.owner].channel && player.HasAmmo(player.inventory[player.selectedItem]))
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
            //if (base.Projectile.velocity.X > 0f)
            //{
            //    Main.player[Projectile.owner].ChangeDir(1);
            //}
            //else if (base.Projectile.velocity.X < 0f)
            //{
            //    Main.player[Projectile.owner].ChangeDir(-1);
            //}
            Projectile.spriteDirection = Projectile.direction;
            player.ChangeDir(Projectile.direction);
            //Main.player[Projectile.owner].heldProj = player.whoAmI; //<-- causes issues
            player.SetDummyItemTime(2);
            base.Projectile.position.X = vector13.X - (float)(base.Projectile.width / 2);
            base.Projectile.position.Y = vector13.Y - (float)(base.Projectile.height / 2);
            Projectile.rotation = (float)(Math.Atan2(base.Projectile.velocity.Y, base.Projectile.velocity.X) + 1.5700000524520874);
            if (player.direction == 1)
            {
                player.itemRotation = (float)Math.Atan2(base.Projectile.velocity.Y * (float)Projectile.direction, base.Projectile.velocity.X * (float)Projectile.direction);
            }
            else
            {
                player.itemRotation = (float)Math.Atan2(base.Projectile.velocity.Y * (float)Projectile.direction, base.Projectile.velocity.X * (float)Projectile.direction);
            }
            //base.Projectile.velocity.X *= 1f + (float)Main.rand.Next(-3, 4) * 0.01f;        
            //================================================================================================================================================================================================================================================
        }
        
        public override void OnKill(int timeLeft)
        {
           
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
        public override void PostDraw(Color lightColor)
        {
           
        }
    }
}
