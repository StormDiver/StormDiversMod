using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using StormDiversMod.Common;
using Terraria.DataStructures;
namespace StormDiversMod.Projectiles       
{
   
    public class StompBootProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shockwave");
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            //Projectile.magic = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            Projectile.knockBack = 8f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Generic;
        }

        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            Projectile.damage = (Projectile.damage * 100) / 101;

            if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
            {
                Vector2 position = Projectile.position;
                if (player.gravDir == 1)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.Bottom.Y - 10), Projectile.width, 12, 31, 0f, 0f, 100, default, 100f);
                    Main.dust[dustIndex].scale = 0.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
                else
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.Top.Y + 10), Projectile.width, 12, 31, 0f, 0f, 100, default, 100f);
                    Main.dust[dustIndex].scale = 0.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 203);

                dust2.noGravity = true;
            }
            if (Projectile.damage == 0)
            {
                Projectile.Kill();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
    public class StompBootProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stompwave");
        }
        public override void SetDefaults()
        {

            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            //Projectile.magic = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 9999;
            Projectile.extraUpdates = 0;
            //Projectile.knockBack = 2f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 9;
            Projectile.DamageType = DamageClass.Generic;
        }

        public override void AI()
        {
            if (Projectile.damage < 60) //10-60 damage
            {
                Projectile.damage += 1;
            }
            //Main.NewText("The stomp damage is: " + Projectile.damage, 110, 255, 100);

            var player = Main.player[Projectile.owner];
            Projectile.position.X = player.Center.X - 20;
            if (player.gravDir == 1)
            {
                Projectile.position.Y = player.Center.Y + 0;
            }
            else
            {
                Projectile.position.Y = player.Center.Y - 40;
            }
            Projectile.knockBack = 6;
            Projectile.velocity.X = player.direction; //knocks enemies in the direction facing
            if (player.GetModPlayer<EquipmentEffects>().falling == false || player.dead || (!player.controlDown))
            {
                Projectile.Kill();
            }
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.Center.Y - 20 * player.gravDir), Projectile.width, 20, 31, 0f, 0f, 100, default, 1.5f);
            Main.dust[dustIndex].noGravity = true;
        } 

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
    }
    public class StompBootDrillProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stompwave");
            Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            //Projectile.magic = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 9999;
            Projectile.extraUpdates = 0;
            //Projectile.knockBack = 2f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 9;
            Projectile.DamageType = DamageClass.Generic;
        }
        public override void OnSpawn(IEntitySource source)
        {
            var player = Main.player[Projectile.owner];

            for (int i = 0; i < 30; i++) //Grey dust circle7
            {
                Vector2 perturbedSpeed = new Vector2(0, -2f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X + player.velocity.X, perturbedSpeed.Y + player.velocity.Y);
                dust.noGravity = true;
                dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
            }
        }
        bool isretracting;
        Vector2 playerBL;
        Vector2 playerBM;
        Vector2 playerBR;
        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            if (player.gravDir == 1)
            {
                playerBL = new Vector2(player.BottomLeft.X - 1, player.BottomLeft.Y + 1); //need to make it one to the left

                playerBM = new Vector2(player.Bottom.X - 0, player.Bottom.Y + 1);

                playerBR = new Vector2(player.BottomRight.X - 0, player.BottomRight.Y + 1);
            }
            else
            {
                playerBL = new Vector2(player.TopLeft.X - 1, player.TopLeft.Y - 1); //need to make it one to the left

                playerBM = new Vector2(player.Top.X - 0, player.Top.Y - 1);

                playerBR = new Vector2(player.TopRight.X - 0, player.TopRight.Y - 1);
            }

            var tilePosleft = playerBL.ToTileCoordinates16();//get perfect cooridantes
            var tilePosmiddle = playerBM.ToTileCoordinates16();
            var tilePosright = playerBR.ToTileCoordinates16();

            if (!isretracting)
            {
                Projectile.ai[1]++;
                Projectile.ai[2]++;
                if (player.gravDir == 1)
                    Projectile.rotation = MathHelper.ToRadians(0);
                else
                    Projectile.rotation = MathHelper.ToRadians(180);

                if (Projectile.ai[2] >= 0)
                {
                    for (int i = 0; i < 2; i++)//hit twice so grass doesn't stop you
                    {
                        if (Main.tile[tilePosleft].HasTile && !Main.tileAxe[Main.tile[tilePosleft].TileType] && Main.tileSolid[Main.tile[tilePosleft].TileType])
                        {
                            player.PickTile(tilePosleft.X, tilePosleft.Y, 100);
                            player.GetModPlayer<EquipmentEffects>().bootdrillmining = true; //slow down player descent speed if mining
                            Projectile.ai[1] = 0;
                        }
                        if (Main.tile[tilePosmiddle].HasTile && !Main.tileAxe[Main.tile[tilePosmiddle].TileType] && Main.tileSolid[Main.tile[tilePosmiddle].TileType])
                        {
                                player.PickTile(tilePosmiddle.X, tilePosmiddle.Y, 100);
                            player.GetModPlayer<EquipmentEffects>().bootdrillmining = true; //slow down player descent speed if mining
                            Projectile.ai[1] = 0;
                        }
                        if (Main.tile[tilePosright].HasTile && !Main.tileAxe[Main.tile[tilePosright].TileType] && Main.tileSolid[Main.tile[tilePosright].TileType])
                        {
                                player.PickTile(tilePosright.X, tilePosright.Y, 100);
                            player.GetModPlayer<EquipmentEffects>().bootdrillmining = true; //slow down player descent speed if mining
                            Projectile.ai[1] = 0;
                        }
                        //Main.NewText("LEFT: " + tilePosleft, Color.Green);
                        //Main.NewText("MIDDLE: " + tilePosmiddle, Color.Yellow);
                        //Main.NewText("RIGHT: " + tilePosmiddle, Color.Red);

                        Projectile.ai[2] = 0;
                    }
                }
                if (Projectile.ai[1] > 5)//when not maining bring speed back to full
                    player.GetModPlayer<EquipmentEffects>().bootdrillmining = false;

                //Main.NewText("ffs " + Projectile.ai[1], Color.Red);

                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 2)
                {
                    Projectile.frame++;
                    if (Projectile.frame == 6)
                        Projectile.frame = 4;

                    Projectile.frameCounter = 0;
                }

                if (Projectile.damage < 60) //10-60 damage
                {
                    Projectile.damage += 1;
                }
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, (Projectile.Center.Y - 10) + (20 * player.gravDir)), Projectile.width, 20, 31, 0f, 0f, 100, default, 1.5f);
                Main.dust[dustIndex].noGravity = true;
            }
            Projectile.position.X = player.Center.X - 20;
            if (player.gravDir == 1)
            {
                Projectile.position.Y = player.Center.Y - 4;
            }
            else
            {
                Projectile.position.Y = player.Center.Y - 48;
            }
            Projectile.knockBack = 6;
            Projectile.velocity.X = player.direction; //knocks enemies in the direction facing
            if (player.dead)
            {
                Projectile.Kill();
            }
            if (player.GetModPlayer<EquipmentEffects>().falling == false || player.dead || (!player.controlDown))
            {
                isretracting = true;
            }
            if (isretracting)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 2)
                {
                    Projectile.frame--;
                    if (Projectile.frame == -1)
                        Projectile.Kill();
                    Projectile.frameCounter = 0;
                }
                Projectile.damage = 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
    }
}