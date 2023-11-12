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
    public class MechanicalRifleProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mechanical Rifle");
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

        int chargetime;
        float sound = -0.5f;
        bool maxcharge;

        int orginaldamage;

        float extralength;

        public override void OnSpawn(IEntitySource source)
        {
            var player = Main.player[Projectile.owner];
            //Projectile.damage = player.HeldItem.damage; //starts off at 80, but then ranged buffs are applied after charge, otherwise additon damage is worse at higher charge levels
            orginaldamage = Projectile.damage;
        }
        public override void AI()
        {   
            var player = Main.player[Projectile.owner];

            //Main.NewText("Damage = " + Projectile.damage, 0, 204, 170); //Inital Scale

            if (!maxcharge) //charge up, lower angle, add velocity to bullets, and increase damage
            {
                chargetime++;
            }
            if (chargetime >= 40 && !maxcharge)//Charge up time is 40 frames
            {
                SoundEngine.PlaySound(SoundID.Item149 with { Volume = 2f, Pitch = 0.5f, MaxInstances = 0 }, base.Projectile.position);
                for (int i = 0; i < 3; i++)
                {
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Projectile.Center.X + (Projectile.velocity.X * .075f), Projectile.Center.Y - 2 + (Projectile.velocity.Y * .075f)),

                    }, player.whoAmI);
                }
                maxcharge = true;
            }
            Projectile.ai[1]++; //noise starts after 10 frames
            if (Projectile.soundDelay <= 0 && !maxcharge && Projectile.ai[1] >= 10) //Charge up sound and effect
            {
                sound += 0.2f;

                SoundEngine.PlaySound(SoundID.Item15 with { Volume = 1f, Pitch = sound, MaxInstances = 0 }, base.Projectile.position);
                for (int i = 0; i < 10; i++)
                {
                    int dust2 = Dust.NewDust(Projectile.position + new Vector2(0, -4), Projectile.width, Projectile.height, 31, 0f, 0f, 80, default(Color), 0.75f);
                    Main.dust[dust2].position.X -= 4f;
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].velocity *= 0.2f;
                    //Main.dust[dust2].velocity.Y = (float)(-Main.rand.Next(7, 13)) * 0.15f;
                }

                Projectile.soundDelay = 10;
            }
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 15f; // Position of end of barrel

           
            if (maxcharge) //At full charge new dust and sound
            {
                Vector2 dustspeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);

                int dust2 = Dust.NewDust(Projectile.Center + new Vector2(-4, -6), 0, 0, 31, dustspeed.X * 0.05f, dustspeed.Y * 0.05f, 100, default, 1.5f);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].scale = 1f;
                if (Projectile.soundDelay <= 0)
                {
                    //SoundEngine.PlaySound(SoundID.Item157 with { Volume = 0.6f, Pitch = 1, MaxInstances = 0, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, base.Projectile.position);
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

            if (!player.controlUseTile)
                extralength = 1;
            else
                extralength = 2.5f;
        }

        public override void OnKill(int timeLeft)
        {
            var player = Main.player[Projectile.owner];
            if (maxcharge) //Different sound at max charge
            {
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
                if (projToShoot == ProjectileID.Bullet && !maxcharge)
                {
                    projToShoot = ProjectileID.BulletHighVelocity;
                }
                else if (projToShoot == ProjectileID.Bullet || projToShoot == ProjectileID.BulletHighVelocity && maxcharge)
                {
                    projToShoot = ModContent.ProjectileType<MechanicalRifleProj2>();

                }

                //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                for (int i = 0; i < 1; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f).RotatedByRandom(MathHelper.ToRadians(0));
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y - 1), new Vector2((perturbedSpeed.X), (float)(perturbedSpeed.Y)), projToShoot, (int)(Projectile.damage), Projectile.knockBack, Projectile.owner);
                   
                    Main.projectile[projID].usesLocalNPCImmunity = true;
                    Main.projectile[projID].localNPCHitCooldown = 10;

                    if (maxcharge) //Extra speed at max charge
                    {
                        Main.projectile[projID].extraUpdates += 1;
                        Main.projectile[projID].knockBack *= 2;
                        Main.projectile[projID].damage = (int)(Projectile.damage * 3); //double damage

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
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Excalibur, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Projectile.Center.X + (Projectile.velocity.X * .1f), Projectile.Center.Y - 4 + (Projectile.velocity.Y * .1f)),

                    }, player.whoAmI);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var player = Main.player[Projectile.owner];
            if (!Main.dedServ)
            {
                Vector2 velocity = Projectile.velocity * 15;

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(0);
                if (maxcharge)
                    Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X - 1, Projectile.Center.Y - 3), new Vector2(Projectile.Center.X - 1 + (perturbedSpeed.X * extralength), Projectile.Center.Y - 3 + (perturbedSpeed.Y * extralength)), Color.Gold, Color.Transparent, 2f);
            }
            return true;
        }
        public override void PostDraw(Color lightColor)
        {
           
        }
    }
    //__________________________________________________________________________________________
    public class MechanicalRifleProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Suepr high Velocity Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;

            Projectile.friendly = true;
            Projectile.timeLeft = 400;
            Projectile.penetrate = 4;

            Projectile.tileCollide = true;
            Projectile.scale = 1.2f;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 8;
            AIType = ProjectileID.Bullet;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 25; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 206);
                dust.scale = 1.2f;
                dust.noGravity = true;
                dust.velocity *= 2;
            }
            return true;
        }
        public override void AI()
        {
            var dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 206);
            dust.scale = 1.2f;
            dust.noGravity = true;
            dust.velocity *= 1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];

            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(Projectile.Center.X + (Projectile.velocity.X * .1f), Projectile.Center.Y - 4 + (Projectile.velocity.Y * .1f)),

            }, player.whoAmI);

            target.AddBuff(BuffID.Confused, Main.rand.Next(60, 181));

            for (int i = 0; i < 25; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 206);
                dust.scale = 1.2f;
                dust.noGravity = true;
                dust.velocity *= 2;
            }
            //Projectile.damage = (Projectile.damage * 19) / 20; //%5 falloff
        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
}
