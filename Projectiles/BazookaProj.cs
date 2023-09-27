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
using Terraria.ModLoader.Default;

namespace StormDiversMod.Projectiles
{
    public class BazookaProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The bazooka");
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
        int chargetime = 0; //15 frames before weapon actually fires
        int rocketcharge = 0; 
        float sound = -0.6f;
        //Item ammotype; //custom ammo
        //int shoottype; //
        bool released;
        int firetime;

        float extradmg = 1;
        public override void OnSpawn(IEntitySource source)
        {
            var player = Main.player[Projectile.owner];
            //Projectile.damage = player.HeldItem.damage; //starts off at 90, but then ranged buffs are applied after charge
            //ammotype = player.ChooseAmmo(player.HeldItem);
        }
        public override void AI()
        {           
            var player = Main.player[Projectile.owner];

            /*Vector2 muzzleOffset = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 10f; // Position of end of barrel

            if (Collision.CanHit(Projectile.position, 0, 0, Projectile.position + muzzleOffset, 0, 0))
            {
                Projectile.position += muzzleOffset;
            }*/

            bool canShoot = player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed; //checks for ammo and also removes ammo each charge
            int projToShoot = player.HeldItem.shoot; //doesn't work with rockets other than I-IV, so custom rocket
            int usedAmmoItemID = player.HeldItem.useAmmo;
            float speed = player.HeldItem.shootSpeed;
            int Damage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
            float KnockBack = player.inventory[player.selectedItem].knockBack;

            if (!released)
            chargetime++;

            if (chargetime >= player.HeldItem.useTime && canShoot && !released) //Charges up the rockets every 15 frames 
            {
                //if (rocketcharge >= 0) //first ammo is consumed as soon as the weapon is fired
                {
                    player.PickAmmo(player.inventory[player.selectedItem], out projToShoot, out speed, out Damage, out KnockBack, out usedAmmoItemID, false);
                    //Projectile.damage = (Projectile.damage * 20) / 19; //extra 5% per additional rocket
                }
                //for overloading mechanisim, remove dust effects as max charge to activate
                if (rocketcharge <= 5)
                    rocketcharge++;
                else
                {
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, -8), Projectile.velocity * 0, ModContent.ProjectileType<BazookaProj2>(), (int)(Projectile.damage), Projectile.knockBack, Projectile.owner);

                    Main.projectile[projID].timeLeft = 1;
                }
                if (rocketcharge >= 6)
                {
                    CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.Red, "MAX", false);
                    SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.5f, Pitch = 0f}, Projectile.Center);

                }
                else
                    CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.White, rocketcharge, false);

                for (int i = 0; i < 30; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.Center + new Vector2(-4, -11), 0, 0, 6, 0f, 0f, 50, default, 1.5f);
                    Main.dust[dustIndex].velocity *= 2;
                    Main.dust[dustIndex].noGravity = true;

                    float speedX = 0f;
                    float speedY = -1f;

                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dustIndex2 = Dust.NewDust(Projectile.Center + new Vector2(-4, -11), 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);
                    Main.dust[dustIndex2].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex2].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex2].noGravity = true;
                }
                sound += 0.1f;

                SoundEngine.PlaySound(SoundID.Item61 with { Volume = 1f, Pitch = sound }, Projectile.Center);

                chargetime = 0;
            }
            Projectile.ai[1]++; 

            if (rocketcharge > 5 || !canShoot) //dust effects when at full charge
            {
                Vector2 perturbedSpeed = new Vector2(0, -1).RotatedByRandom(MathHelper.ToRadians(360));

                int dustIndex = Dust.NewDust(Projectile.Center + new Vector2(-4, -11), 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;

                Vector2 dustspeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);

                int dust2 = Dust.NewDust(Projectile.Center + new Vector2(-4, -11), 0, 0, 31, dustspeed.X * 0.125f, dustspeed.Y * 0.125f, 100, default, 1.5f);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].scale = 1f;

                if (Projectile.ai[1] >= 10)
                {
                    SoundEngine.PlaySound(SoundID.Item13 with { Volume = 2f, Pitch = -0.5f }, Projectile.Center);
                    Projectile.ai[1] = 0;
                }
                chargetime = 0;
            }
            if (!player.channel && !released) //player releases left click
            {
                //if (rocketcharge < 1) //so one rocket still fires
                  //  rocketcharge = 1;
                released = true;
            }
            if (released) //fire rockets, 5 frame delay, 30 frames total to fire
            {
                firetime++;

                if (firetime >= player.HeldItem.useTime / 3) //15 / 3 = 5
                {
                    //For custom ammo
                    /*if (ammotype.type == ItemID.RocketI)
                        shoottype = ProjectileID.RocketI;
                    else if (ammotype.type == ItemID.RocketII)
                        shoottype = ProjectileID.RocketII;
                    else if (ammotype.type == ItemID.RocketIII)
                        shoottype = ProjectileID.RocketIII;
                    else if (ammotype.type == ItemID.RocketIV)
                        shoottype = ProjectileID.RocketIV;
                    else if (ammotype.type == ItemID.ClusterRocketI)
                        shoottype = ProjectileID.ClusterRocketI;
                    else if (ammotype.type == ItemID.ClusterRocketII)
                        shoottype = ProjectileID.ClusterRocketII;
                    else if (ammotype.type == ItemID.MiniNukeI)
                        shoottype = ProjectileID.MiniNukeRocketI;
                    else if (ammotype.type == ItemID.MiniNukeII)
                        shoottype = ProjectileID.MiniNukeRocketII;
                    else if (ammotype.type == ItemID.WetRocket)
                        shoottype = ProjectileID.WetRocket;
                    else if (ammotype.type == ItemID.LavaRocket)
                        shoottype = ProjectileID.LavaRocket;
                    else if (ammotype.type == ItemID.HoneyRocket)
                        shoottype = ProjectileID.HoneyRocket;
                    else if (ammotype.type == ItemID.DryRocket)
                        shoottype = ProjectileID.DryRocket;*/

                    //float extradmg = orginaldamage / 20 * rocketcharge; //extra 6% per rocket loaded
                    extradmg += 0.06f;
                    //Main.NewText("extradmg " + extradmg, 0, 204, 170); //Inital Scale

                    Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X * 0.66f, Projectile.velocity.Y * 0.66f).RotatedByRandom(MathHelper.ToRadians(6));
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, -8), new Vector2((perturbedSpeed.X), (float)(perturbedSpeed.Y)), ModContent.ProjectileType<BazookaProj2>(), (int)(Projectile.damage * extradmg), Projectile.knockBack, Projectile.owner);

                    Main.projectile[projID].usesLocalNPCImmunity = true;
                    Main.projectile[projID].localNPCHitCooldown = 10;
                    //If projectile is blocked then explode 
                    if (!Collision.CanHitLine(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, new Vector2(player.Center.X, player.Center.Y), 0, 0))
                    {
                        Main.projectile[projID].timeLeft = 3;
                        Main.projectile[projID].position.X = Projectile.Center.X - 7;
                        Main.projectile[projID].position.Y = Projectile.Center.Y - 7;

                    }
                    SoundEngine.PlaySound(SoundID.Item92 with { Volume = 1f, Pitch = 0, MaxInstances = -1 }, Projectile.Center);
                    for (int i = 0; i < 30; i++)
                    {
                        float speedX = 0f;
                        float speedY = -1f;

                        Vector2 perturbedSpeed2 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dustIndex = Dust.NewDust(Projectile.Center + new Vector2(-4, -11), 0, 0, 31, perturbedSpeed2.X, perturbedSpeed2.Y, 100, default, 1f);
                        Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].noGravity = true;

                        Vector2 dustspeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);

                        int dust2 = Dust.NewDust(Projectile.Center + new Vector2(-4, -11), 0, 0, 31, dustspeed.X * 0.125f, dustspeed.Y * 0.125f, 100, default, 1.5f);
                        Main.dust[dust2].noGravity = true;
                        Main.dust[dust2].scale = 1f;
                    }
                    rocketcharge -= 1;

                    firetime = 0;
                }
                //for (int i = 0; i < rocketcharge; i++)
            }

            if (rocketcharge <= 0 && released || player.dead) //kill projectile once all rockets are fired
                Projectile.Kill();
           
            //Main.NewText("charge " + chargetime, 0, 204, 170); //Inital Scale
            //Main.NewText("rocket " + rocketcharge, 0, 204, 170); //Inital Scale

            //=============================================================================================Code for movement of weapon projectile====================================================================================================================================================
            Vector2 vector13 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                //if (Main.player[Projectile.owner].channel) //projectil ewill be killed when all rockets are fired
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
                //else
                {
                    /*if (player.HasAmmo(player.HeldItem)) //if no ammo is left it'll cause a crash, so just add 50 to the weapon
                        Projectile.damage += ammodmg.damage;
                    else
                        Projectile.damage += 50;

                    Projectile.damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Projectile.damage); //make sure damage updates are applied*/
                    //Projectile.Kill();
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


            //if (Collision.CanHitLine(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, new Vector2(player.Center.X, player.Center.Y), 0, 0))
            {
                //player.PickAmmo(player.inventory[player.selectedItem], out projToShoot, out speed, out Damage, out KnockBack, out usedAmmoItemID, true);

                //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                /*for (int i = 0; i < rocketcharge; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X * 0.66f, Projectile.velocity.Y * 0.66f).RotatedByRandom(MathHelper.ToRadians(1 + rocketcharge)); //Start at 2 for 1, up to 7 for 6
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, -8), new Vector2((perturbedSpeed.X), (float)(perturbedSpeed.Y)), ModContent.ProjectileType<BazookaProj2>(), (int)(Projectile.damage), Projectile.knockBack, Projectile.owner);

                    Main.projectile[projID].usesLocalNPCImmunity = true;
                    Main.projectile[projID].localNPCHitCooldown = 10;
                    //If projectile is blocked then explode 
                    if (!Collision.CanHitLine(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, new Vector2(player.Center.X, player.Center.Y), 0, 0))
                    {
                        Main.projectile[projID].timeLeft = 3;
                        Main.projectile[projID].position.X = Projectile.Center.X - 7;
                        Main.projectile[projID].position.Y = Projectile.Center.Y - 7;

                    }
                }
                //for (int i = 0; i < rocketcharge; i++)
                SoundEngine.PlaySound(SoundID.Item92 with { Volume = 1f, Pitch = 0, MaxInstances = -1}, Projectile.Center);*/
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
        
            return true;
        }
        public override void PostDraw(Color lightColor)
        {
           
        }
    }
    //___________________________________________________________
    public class BazookaProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("bazooka Missile");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;

        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;


            Projectile.friendly = true;

            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.hide = true;
        }
        public override void AI()
        {

            var player = Main.player[Projectile.owner];
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.ai[1]++;
            if (Projectile.ai[1] < 40)
            {
                Projectile.velocity *= 1.02f;
            }
            if (Projectile.ai[1] > 1 && Projectile.timeLeft > 3)
            {
                Projectile.hide = false; //hides a weired visual glitch

                for (int i = 0; i < 10; i++)
                {
                    float X = Projectile.Center.X - Projectile.velocity.X / 8f * (float)i;
                    float Y = Projectile.Center.Y - Projectile.velocity.Y / 8f * (float)i;

                    int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 6, 0, 0, 100, default, 2f);
                    Main.dust[dust].position.X = X;
                    Main.dust[dust].position.Y = Y;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;
                }
                for (int i = 0; i < 3; i++)
                {
                    var dust2 = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, 0, 0, 100);

                    //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                    dust2.noGravity = true;
                    dust2.scale = 1.5f;
                }
            }
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 125;
                Projectile.height = 125;
                Projectile.Center = Projectile.position;


                Projectile.knockBack = 3f;
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;

            }

            if (Projectile.shimmerWet && Projectile.velocity.Y > 0)
            {
                Projectile.velocity.Y *= -1;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

            if (Projectile.timeLeft > 3)
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
            Main.projectile[proj].scale = 1.1f;

            for (int i = 0; i < 30; i++) //Orange particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -6f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 2f;

            }
            for (int i = 0; i < 40; i++) //Grey dust circle7
            {
                Vector2 perturbedSpeed = new Vector2(0, -4f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
            }
            var player = Main.player[Projectile.owner];

            /*float distance = Vector2.Distance(player.Center, Projectile.Center);
            if (distance <= Projectile.width / 2 + 25 && distance >= 1 && !player.mount.Active)
            {
                //if (Collision.CanHit(player.Center, 0, 0, Projectile.Center, 0, 0))
                {
                    float launchspeed = 11;
                    Vector2 launchvelocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(player.Center.X, player.Center.Y)) * launchspeed;
                    player.GetModPlayer<MiscFeatures>().explosionfall = true;
                    player.GetModPlayer<MiscFeatures>().explosionflame = 60;

                    player.velocity.X = -launchvelocity.X * 2.5f;
                    player.velocity.Y = -launchvelocity.Y * 2.5f;
                }
            }*/

            /*for (int i = 0; i < 200; i++) //test for now rocket damage system
            {
                NPC target = Main.npc[i];
                var player = Main.LocalPlayer;

                if (Vector2.Distance(Projectile.Center, target.Center) <= 75 && !target.friendly && !target.dontTakeDamage && target.active)
                {
                    int prjdamage = (Math.Min(1, Math.Max(9999999, Projectile.damage - target.defense / 2)));
                    target.life -= prjdamage;

                    CombatText.NewText(new Rectangle((int)target.Center.X, (int)target.Center.Y, 12, 4), Color.Orange, "" + prjdamage, false);
                    //how to detect crit?
                    //needs KB
                }

            }*/
        }
    }
}
