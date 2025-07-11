﻿using System;
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
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles.AmmoProjs
{

    public class MineBombProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mine");
            ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
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
        bool helddet = false;
        public override bool? CanDamage()
        {
            if (boomed)
                return true;
            else
                return false;
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

                Projectile.width = 150;
                Projectile.height = 150;
                Projectile.Center = Projectile.position;

                Projectile.knockBack = 1f;
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                //Projectile.hostile = true;
  
                //launch player
                float distance = Vector2.Distance(player.Center, Projectile.Center);
                if (distance <= Projectile.width / 2 + 25 && distance >= 1 && !player.mount.Active)
                {
                    //if (Collision.CanHit(player.Center, 0, 0, Projectile.Center, 0, 0))
                    {
                        float launchspeed = 8.5f;
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
                            float npclaunchspeed = 8.5f;
                            Vector2 npclaunchvelocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(target.Center.X, target.Center.Y)) * npclaunchspeed;
                            target.GetGlobalNPC<NPCEffects>().explosionNPCflame = 60;

                            target.velocity.X = -npclaunchvelocity.X * 1.5f;
                            target.velocity.Y = -npclaunchvelocity.Y * 2f;
                        }
                        SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, player.Center);

                    }
                }
            }
            else
            {
                if (!stick)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }

            if (stick)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                Projectile.knockBack = 0;
            }
            if (player.itemTime == (player.HeldItem.useTime - 1) && !player.controlUseItem)
            {
                if ((player.HeldItem.type == ModContent.ItemType<Items.Weapons.MineDetonate>() && !player.controlUp && player.controlUseTile && player.noThrow == 0 && boomtime > 30) || player.dead) //will go BOOM
                {
                    if (Projectile.timeLeft > 3)
                    {
                        Projectile.timeLeft = 3;
                    }

                    //if (!GetInstance<ConfigurationsIndividual>().NoShake)
                    {
                        player.GetModPlayer<MiscFeatures>().screenshaker = true;
                    }
                }
                if ((player.HeldItem.type == ModContent.ItemType<Items.Weapons.MineDetonate>() && player.controlUp && player.controlUseTile && player.noThrow == 0 && boomtime > 30)) //disarm bomb
                {
                    SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

                    for (int i = 0; i < 15; i++) //Grey dust circle
                    {
                        Vector2 perturbedSpeed = new Vector2(0, -1.5f).RotatedByRandom(MathHelper.ToRadians(360));
                        var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                        //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                        dust.noGravity = true;
                        dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                        dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                    }
                    Projectile.active = false;
                    Item.NewItem(new EntitySource_Loot(player), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ItemType<Items.Ammo.MineBomb>());
                }
            }
            if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.MineDetonate>() && boomtime > 30)
                helddet = true;
            else
                helddet = false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
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
                float launchspeed = 7;
                Vector2 launchvelocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(target.Center.X, target.Center.Y)) * launchspeed;
                target.GetModPlayer<MiscFeatures>().explosionfall = true;
                target.GetModPlayer<MiscFeatures>().explosionflame = 60;

                target.velocity.X = -launchvelocity.X * 2f;
                target.velocity.Y = -launchvelocity.Y * 2f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.knockBackResist != 0 && !target.friendly && target.lifeMax > 5)
            {
                float launchspeed = 7;
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
            Main.projectile[proj].scale = 1f;

            for (int i = 0; i < 20; i++) //Orange particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 2f;

            }
            for (int i = 0; i < 30; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
            }
        }
        Color colorline = Color.Red;
        public override bool PreDraw(ref Color lightColor)
        {
            var player = Main.player[Projectile.owner];

            if (helddet)
            {
                if (player.controlUp)
                    colorline = Color.Green;
                else
                    colorline = Color.Red;

                if (Main.netMode != NetmodeID.Server)
                {
                    Utils.DrawLine(Main.spriteBatch,  new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(player.Center.X, player.Center.Y), colorline, Color.Transparent, 3);
                }
            }
            return true;
        }
    }
   
}