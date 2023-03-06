using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Basefiles;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.Projectiles
{

    public class StickyBombProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiky Bomb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
 
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = 2;

            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 99999;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -1;
            DrawOriginOffsetY = -1;
            //Projectile.ContinuouslyUpdateDamage = true;

        }
        bool stick; //wheter bomb has hit a tile
        bool boomed; //when it is exploding
        int boomtime = 0; //How long until you can actually detonate
        bool unstick; //Bool for if you unstick the bombs
        public override bool? CanDamage()
        {
            if (unstick)
            {
                return true;
            }
            if (!boomed)
            {
                return false;

            }
            else
            {
                return true;
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
                if (distance <= Projectile.width / 2 + 25 && !player.mount.Active)
                {
                    float launchspeed = 12;
                    Vector2 launchvelocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(player.Center.X, player.Center.Y)) * launchspeed;
                    player.GetModPlayer<MiscFeatures>().explosionfall = true;

                    player.velocity.X = -launchvelocity.X * 2.5f;
                    player.velocity.Y = -launchvelocity.Y * 2.5f;
                    //player.fallStart = 0;
                }
                for (int i = 0; i < 200; i++)//for town npcs
                {
                    NPC target = Main.npc[i];

                    float npcdistance = Vector2.Distance(target.Center, Projectile.Center);

                    if (npcdistance <= Projectile.width / 2 + 25 && (target.friendly || target.CountsAsACritter))
                    {
                        float npclaunchspeed = 12;
                        Vector2 npclaunchvelocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(target.Center.X, target.Center.Y)) * npclaunchspeed;

                        target.velocity.X = -npclaunchvelocity.X * 1.5f;
                        target.velocity.Y = -npclaunchvelocity.Y * 2f;
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

            if ((player.controlUseTile && !player.controlUp && player.HeldItem.type == ModContent.ItemType<Items.Weapons.StickyLauncher>() && boomtime > 30) || player.dead) //will go BOOM
            {
                if (Projectile.timeLeft > 3)
                {
                    Projectile.timeLeft = 3;
                }
                if (!GetInstance<ConfigurationsIndividual>().NoShake)
                {
                    player.GetModPlayer<MiscFeatures>().screenshaker = true;
                }
            }
            if ((player.controlUseTile && player.controlUp && !unstick && stick)) //will unstick
            {
                SoundEngine.PlaySound(SoundID.Item108, Projectile.Center);
                Projectile.velocity.Y = -2;
                unstick = true;
            }
        }
            
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            damage /= 4;
            target.velocity.Y = -15;
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


                }
                SoundEngine.PlaySound(SoundID.NPCHit1 with{Volume = 0.5f, Pitch = 0.2f}, Projectile.Center);

            }
            stick = true;
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (unstick)
            {
                if (Projectile.timeLeft > 3)
                {
                    Projectile.timeLeft = 3;
                }
            }
            if (target.knockBackResist != 0)
            {
                float launchspeed = 12;
                Vector2 launchvelocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(target.Center.X, target.Center.Y)) * launchspeed;
                target.velocity.X = -launchvelocity.X * 0.75f;
                target.velocity.Y = -launchvelocity.Y * 1.5f;
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionGenericProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1.75f;

            for (int i = 0; i < 30; i++) //Orange particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -7f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;

                dust.scale = 1.5f;
                dust.fadeIn = 1.5f;

            }
            for (int i = 0; i < 35; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -2.5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 2f;
                dust.velocity *= 2.5f;

            }
           
        }
    }
   
}