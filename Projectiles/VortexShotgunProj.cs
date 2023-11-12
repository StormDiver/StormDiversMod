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
using StormDiversMod.Basefiles;
using static Terraria.ModLoader.ModContent;
using static System.Formats.Asn1.AsnWriter;
using static Humanizer.In;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;


namespace StormDiversMod.Projectiles
{
    public class VortexShotgunGun : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Storm Diver Shotgun");
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
            //Projectile.hide = true; //prevents pre draw from running
            Projectile.alpha = 255; //use instead

            Projectile.ownerHitCheck = true; //so you can't hit enemies through walls
            Projectile.DamageType = DamageClass.Ranged;
            DrawOffsetX = 3;
            DrawOriginOffsetY = 0;
            //Projectile.ContinuouslyUpdateDamage = true;
            
        }
        public override bool? CanDamage() => false;

        float angle = 18f;
        float extravel = 1f;
        float sound = -0.5f;
        bool maxcharge;

        float linewidth = 1.5f;
        float numberlines = 2;
        float linerotation;
        int orginaldamage;
        public override void OnSpawn(IEntitySource source)
        {
            var player = Main.player[Projectile.owner];
            //Projectile.damage = player.HeldItem.damage; //starts off at 80, but then ranged buffs are applied after charge, otherwise additon damage is worse at higher charge levels
            orginaldamage = Projectile.damage;
        }
        public override void AI()
        {   
            var player = Main.player[Projectile.owner];

            //for the lines
            if (angle == 0)
                linewidth = 2.5f;
            else
                linewidth = 1.5f;

            linerotation = MathHelper.ToRadians(angle);

            //Main.NewText("Damage = " + Projectile.damage, 0, 204, 170); //Inital Scale

            if (!maxcharge) //charge up, lower angle, add velocity to bullets, and increase damage
            {
                angle -= 0.3f; //takes 60 frames to charge
                extravel += 0.01f;
                Projectile.damage += orginaldamage / 20; //extra 5% of orignal damage, so roughly same as 4 damage per second
                //Projectile.damage += 4; //Extra 240 damage from base, (90 + 240 = 330 + extra 15% at max charge for 380),                                
                                        //~900 dps at  0 frame charge (2    shots per second, 90  base damage)
                                        //~997 dps at 15 frame charge (1.33 shots per second, 150 base damage)
                                        //~1050dps at 30 frame charge (1    shot  per second, 210 base damage)
                                        //~1080dps at 45 frame charge (0.8  shots per second, 270 base damage)
                                        //~1089dps at 60 frame charge (0.66 shots per second, 330 base damage)                                     
                                        //~1250dps at 60 frame charge (0.66  shots per second, 380 base damage) (Bonus 15% damage (+1))
                                        //ranged damage buffs are applied afterwards
            }
            if (angle < 0.2f)//Charge up time is 60 frames as angle starts at 18
            {
                angle = 0; //full accuracy
                maxcharge = true;
            }
            Projectile.ai[1]++; //1 frame delay so particles always appear at end
            if (Projectile.soundDelay <= 0 && !maxcharge && Projectile.ai[1] > 1) //Charge up sound and effect
            {
                sound += 0.2f;

                SoundEngine.PlaySound(SoundID.Item157 with { Volume = 0.5f, Pitch = sound, MaxInstances = 0 }, base.Projectile.position);
                for (int i = 0; i < 10; i++)
                {
                    int dust2 = Dust.NewDust(Projectile.position + new Vector2(0, -2) + Projectile.velocity * Main.rand.Next(6, 10) * 0.1f, Projectile.width, Projectile.height, 229, 0f, 0f, 80, default(Color), 0.75f);
                    Main.dust[dust2].position.X -= 4f;
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].velocity *= 0.2f;
                    //Main.dust[dust2].velocity.Y = (float)(-Main.rand.Next(7, 13)) * 0.15f;
                }

                Projectile.soundDelay = 10;
            }
            if (maxcharge)
            {
                //Main.NewText("TesterMax " + Projectile.damage * 1.15f, 0, 204, 170); //Inital Scale
            }
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 15f; // Position of end of barrel

            if (Collision.CanHit(Projectile.position, 0, 0, Projectile.position + muzzleOffset, 0, 0))
            {
                Projectile.position += muzzleOffset;
            }
            if (maxcharge) //At full charge new dust and sound
            {
                if (Projectile.soundDelay <= 0)
                {
                    SoundEngine.PlaySound(SoundID.Item157 with { Volume = 0.6f, Pitch = 1, MaxInstances = 0, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, base.Projectile.position);

                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.TrueNightsEdge, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y - 2),

                    }, player.whoAmI);
                  
                    Projectile.soundDelay = 7;
                }
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
            var player = Main.player[Projectile.owner];
            if (maxcharge) //Different sound at max charge
            {
                if (!GetInstance<ConfigurationsIndividual>().NoShake)
                {
                    player.GetModPlayer<MiscFeatures>().screenshaker = true;
                }
                //Projectile.damage *= 2;
                SoundEngine.PlaySound(SoundID.Item38 with { Volume = 1f, Pitch = 0f }, player.Center);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item36 with { Volume = 0.5f, Pitch = 0f }, player.Center);
            }
            //To fire the bullets
            bool canShoot = player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed; 
            int projToShoot = player.HeldItem.shoot;
            int usedAmmoItemID = player.HeldItem.useAmmo;
            float speed = player.HeldItem.shootSpeed;
            int Damage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
            float KnockBack = player.inventory[player.selectedItem].knockBack;
            if (canShoot && Collision.CanHit(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, new Vector2(player.Center.X, player.Center.Y), 0, 0))
            {             
                player.PickAmmo(player.inventory[player.selectedItem], out projToShoot, out speed, out Damage, out KnockBack, out usedAmmoItemID, false);
                if (projToShoot == ProjectileID.Bullet && maxcharge)
                {
                    projToShoot = ProjectileID.MoonlordBullet;
                }

                float numberProjectiles = 5;
                float rotation = MathHelper.ToRadians(angle);
                //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    float speedX = Projectile.velocity.X * 10f;
                    float speedY = Projectile.velocity.Y * 10f;
                    Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y - 2), new Vector2((perturbedSpeed.X * extravel), (float)(perturbedSpeed.Y * extravel)), projToShoot, (int)(Projectile.damage), Projectile.knockBack, Projectile.owner);
                   
                    Main.projectile[projID].usesLocalNPCImmunity = true;
                    Main.projectile[projID].localNPCHitCooldown = 10;

                    if (maxcharge) //Extra speed at max charge
                    {
                        Main.projectile[projID].extraUpdates += 1;
                        Main.projectile[projID].knockBack *= 2;
                        Main.projectile[projID].damage = (int)(Projectile.damage * 1.15f) + 1; //15% extra damage

                        //player.velocity.X += perturbedSpeed.X * -0.25f;
                        //player.velocity.Y += perturbedSpeed.Y * -0.25f;
                    }
                }           
                if (maxcharge) //extra dust for maxcharge
                {
                    /*for (int i = 0; i < 30; i++)
                    {
                        float speedY = -1.5f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y - 2), Projectile.width, Projectile.height, 229, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 229, default, 1.5f);
                        Main.dust[dust2].noGravity = true;
                      
                    }*/
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.TerraBlade, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y - 2),

                    }, player.whoAmI);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var player = Main.player[Projectile.owner];
            if (!Main.dedServ)
            {
                for (int i = 0; i < numberlines; i++)
                {
                    //Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X - 1, player.Center.Y - 2) - new Vector2(Main.MouseWorld.X - 1, Main.MouseWorld.Y - 2)) * -500;
                    Vector2 velocity = Projectile.velocity * 30;

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-linerotation, linerotation, i / (numberlines - 1)));
                    //Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X * 2, perturbedSpeed.Y * 2), ModContent.ProjectileType<CrimsonAxeProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X - 1, Projectile.Center.Y - 2), new Vector2(Projectile.Center.X - 1 + perturbedSpeed.X, Projectile.Center.Y - 2 + perturbedSpeed.Y), Color.Aquamarine, Color.Transparent, linewidth);

                }
            }
            return true;
        }
        public override void PostDraw(Color lightColor)
        {
           
        }
    }
}
