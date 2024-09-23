using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Common;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Items.Weapons;
using SteelSeries.GameSense.DeviceZone;

namespace StormDiversMod.Projectiles
{

    public class StickyBombProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spiky Bomb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 9;
            Projectile.height = 9;
 
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = 2;

            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 99999;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -7;
            DrawOriginOffsetY = -7;
            //Projectile.ContinuouslyUpdateDamage = true;

        }
        bool stick; //wheter bomb has hit a tile
        bool boomed; //when it is exploding
        int boomtime = 0; //How long until you can actually detonate
        bool unstick; //Bool for if you unstick the bombs
        int dettime;
        public override bool? CanDamage()
        {
            if (unstick || boomed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            return true;
        }

        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            /*if (!stick || unstick)
            {
                if (Projectile.velocity.Y < 0)
                {
                    Projectile.velocity.Y *= 0.97f;
                }
                if (Projectile.velocity.Y > -1.5f && Projectile.velocity.Y < 20)
                {
                    Projectile.velocity.Y += 0.4f;
                }
                Projectile.rotation += Projectile.velocity.X / 5;
            }*/

            boomtime++;
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                boomed = true;

                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 200;
                Projectile.height = 200;
                Projectile.Center = Projectile.position;

                Projectile.knockBack = 3f;
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                //Projectile.hostile = true;
  
                //launch player
                float distance = Vector2.Distance(player.Center, Projectile.Center);
                if (distance <= Projectile.width / 2 + 25 && distance >= 1 && !player.mount.Active)
                {
                    //if (Collision.CanHit(player.Center, 0, 0, Projectile.Center, 0, 0))
                    {
                        float launchspeed = 13;
                        Vector2 launchvelocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(player.Center.X, player.Center.Y)) * launchspeed;
                        player.GetModPlayer<MiscFeatures>().explosionfall = true;
                        player.GetModPlayer<MiscFeatures>().explosionflame = 60;

                        player.velocity.X = -launchvelocity.X * 2.5f;
                        player.velocity.Y = -launchvelocity.Y * 2.5f;
                    }
                    SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, player.Center);

                }
                for (int i = 0; i < 200; i++)//for town npcs
                {
                    NPC target = Main.npc[i];

                    float npcdistance = Vector2.Distance(target.Center, Projectile.Center);

                    if (npcdistance <= Projectile.width / 2 + 25 && npcdistance >= 1 && (target.friendly || target.CountsAsACritter) && !target.dontTakeDamage && target.type != NPCID.DD2EterniaCrystal)
                    {
                        //if (Collision.CanHit(target.Center, 0, 0, Projectile.Center, 0, 0))
                        {
                            float npclaunchspeed = 13;
                            Vector2 npclaunchvelocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(target.Center.X, target.Center.Y)) * npclaunchspeed;
                            target.GetGlobalNPC<NPCEffects>().explosionNPCflame = 60;

                            target.velocity.X = -npclaunchvelocity.X * 1.5f;
                            target.velocity.Y = -npclaunchvelocity.Y * 2f;
                        }
                        SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, target.Center);
                    }

                }
            }
            else
            {

                if (!stick)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;

                    dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;

                }
            }

            if (stick && !unstick)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                Projectile.knockBack = 0;

            }

            //Projectile.damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Projectile.originalDamage);
            //^Ignores ammo damage sadly

            //If cursor is near and right click, or if holding right click for 30 frames, and after 30 fraems of fire time and not using a tile for either, or if player dies, explode
            if ((((Vector2.Distance(Main.MouseWorld, Projectile.Center) <= 200 && player.controlUseTile) || dettime > 30) && boomtime > 30 && player.noThrow == 0) || player.dead) //will go BOOM
            {
                if (Projectile.timeLeft > 3)
                {
                    Projectile.timeLeft = 3;
                }
                SoundEngine.PlaySound(SoundID.Item149 with { Volume = 1.5f, Pitch = 0.75f }, player.Center);

                //if (!GetInstance<ConfigurationsIndividual>().NoShake)
                {
                    player.GetModPlayer<MiscFeatures>().screenshaker = true;
                }
            }
            if ((Vector2.Distance(Main.MouseWorld, Projectile.Center ) <= 200 || dettime >= 10) && boomtime > 30)
            {
                if (Projectile.timeLeft > 3)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 235, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].noGravity = true;
                }
            }
            else
            {
            }
            /*if (player.controlUseTile && player.noThrow == 0 && player.controlUp && !unstick && stick) //will unstick
            {
                SoundEngine.PlaySound(SoundID.Item108, Projectile.Center);
                Projectile.velocity.Y = -2;
                unstick = true;
            }*/

            if (player.controlUseTile && player.HeldItem.type == ModContent.ItemType<Items.Weapons.StickyLauncher>())
                dettime++;
            else
                dettime = 0;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!unstick)
            {
                Collision.HitTiles(Projectile.Center + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            }
            if (!stick)
            {
                Projectile.penetrate = -1;
                for (int i = 0; i < 15; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                    dust.noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.NPCHit1 with{Volume = 0.5f, Pitch = 0.2f}, Projectile.Center);

            }
            stick = true;
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                if (unstick)
                {
                    if (Projectile.timeLeft > 3)
                    {
                        Projectile.timeLeft = 3;
                    }
                }
                float launchspeed = 13;
                Vector2 launchvelocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(target.Center.X, target.Center.Y)) * launchspeed;
                target.GetModPlayer<MiscFeatures>().explosionfall = true;
                target.GetModPlayer<MiscFeatures>().explosionflame = 60;

                target.velocity.X = -launchvelocity.X * 2f;
                target.velocity.Y = -launchvelocity.Y * 2f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (unstick)
            {
                if (Projectile.timeLeft > 3)
                {
                    Projectile.timeLeft = 3;
                }
            }
            if (target.knockBackResist != 0 && !target.friendly && target.lifeMax > 5)
            {
                float launchspeed = 13;
                Vector2 launchvelocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(target.Center.X, target.Center.Y)) * launchspeed;
                target.GetGlobalNPC<NPCEffects>().explosionNPCflame = 30;

                target.velocity.X = -launchvelocity.X * 1f;
                target.velocity.Y = -launchvelocity.Y * 1.5f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionGenericProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1.75f;

            for (int i = 0; i < 30; i++) //Orange particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -8f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 2f;

            }
            for (int i = 0; i < 50; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -8f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (dettime > 10)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Utils.DrawLine(Main.spriteBatch,  new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), Color.Red, Color.Transparent, 3);
                }
            }
            return true;
        }
    }
   
}