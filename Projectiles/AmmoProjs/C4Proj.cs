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
using Terraria.DataStructures;
using Steamworks;

namespace StormDiversMod.Projectiles.AmmoProjs
{
    public class C4Proj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sacthel Charge");
            //ProjectileID.Sets.Explosive[Type] = true;
            ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
 
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.aiStyle = 2;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 999999;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = -6;
            Projectile.light = 0.1f;
            //Projectile.ContinuouslyUpdateDamage = true;
        }
        bool stick; //wheter bomb has hit a tile
        bool boomed; //when it is exploding
        int boomtime = 0; //How long until you can actually detonate
        bool helddet = false; //is the detenator being held?
        int dist = 240; //Distance of dust warning radius
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
            //warning dust radius
            if (boomtime > 30)
            {
                if (dist < 240) //max size
                    dist += 5;
                else
                {
                    SoundEngine.PlaySound(SoundID.MaxMana with { Volume = 2f, Pitch = .25f }, Projectile.Center);
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0; //also set frame counter to  0 so it lights up
                    dist = 0;
                }
                for (int i = 0; i < 15; i++) //dust
                {
                    double deg = Main.rand.Next(0, 360); //The degrees
                    double rad = deg * (Math.PI / 180); //Convert degrees to radians
                    //double dist = 240; //Distance away from the proj
                    float dustx = Projectile.Center.X -2 - (int)(Math.Cos(rad) * dist);
                    float dusty = Projectile.Center.Y -2 - (int)(Math.Sin(rad) * dist);

                    var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 235, 0, 0);
                    dust.noGravity = true;
                    dust.scale = 1f;
                    dust.noLight = true;
                    dust.velocity *= 0;

                    float dustx2 = Projectile.Center.X -2 - (int)(Math.Cos(rad) * 240);
                    float dusty2 = Projectile.Center.Y -2 - (int)(Math.Sin(rad) * 240);

                    var dust2 = Dust.NewDustDirect(new Vector2(dustx2, dusty2), 1, 1, 235, 0, 0);
                    dust2.noGravity = true;
                    dust2.scale = 1.25f;
                    dust2.noLight = true;
                    dust2.velocity *= 0;
                }
            }
           
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                boomed = true;

                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.Size = new Vector2(425);
                Projectile.Center = Projectile.position;

                Projectile.knockBack = 1f;
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
            }
            else
            {
            }

            if (stick)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                Projectile.knockBack = 0;
            }
            if (player.itemTime == (player.HeldItem.useTime - 1))
            {
                if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.MineDetonateC4>() && !player.controlUp && player.controlUseTile && player.noThrow == 0 && boomtime > 30) //will go BOOM
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
                if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.MineDetonateC4>() && player.controlUp && player.controlUseTile && player.noThrow == 0 && boomtime > 30) //disarm bomb
                {
                    SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

                    for (int i = 0; i < 30; i++) //Grey dust circle
                    {
                        Vector2 perturbedSpeed = new Vector2(0, -3f).RotatedByRandom(MathHelper.ToRadians(360));
                        var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                        //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                        dust.noGravity = true;
                        dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                        dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    }
                    Projectile.active = false;
                    Item.NewItem(new EntitySource_Loot(player), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ItemType<Items.Ammo.C4Ammo>());
                }
            }

            if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.MineDetonateC4>() && boomtime > 30)
                helddet = true;
            else
                helddet = false;

            AnimateProjectile();
        }
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            if (boomtime < 50) //start off with light off
                Projectile.frame = 1;
            else
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 12 && Projectile.frame == 0) //light on for 12 frames
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
            }
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
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.knockBackResist != 0 && !target.friendly && target.lifeMax > 5)
            {
                float launchspeed = 14;
                Vector2 launchvelocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(target.Center.X, target.Center.Y)) * launchspeed;
                target.GetGlobalNPC<NPCEffects>().explosionNPCflame = 30;

                target.velocity.X = -launchvelocity.X * 1f;
                target.velocity.Y = -launchvelocity.Y * 1.5f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62 with { Volume = 2f, Pitch = -0.5f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion with { Volume = 2f, Pitch = 0f }, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionGenericProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 2f;
            for (int i = 0; i < 20; i++)
            {
                double deg = Main.rand.Next(0, 360); //The degrees
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 150; //Distance away from the proj
                float dustx = Projectile.Center.X - (int)(Math.Cos(rad) * dist);
                float dusty = Projectile.Center.Y - (int)(Math.Sin(rad) * dist);

                var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 235, 0, 0);
                int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(dustx, dusty), new Vector2(0, 0), ModContent.ProjectileType<ExplosionGenericProj>(), 0, 0, Projectile.owner);
                Main.projectile[proj2].scale = 1.25f;

                dust.noGravity = true;
                dust.scale = 1.5f;
            }

            for (int i = 0; i < 150; i++) //Orange particles
            {
                Vector2 perturbedSpeed = new Vector2(0, Main.rand.NextFloat(-22f, -12f)).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 2.5f;

            }

            for (int i = 0; i < 100; i++) //Orange particles no grav
            {
                Vector2 perturbedSpeed = new Vector2(0, Main.rand.NextFloat(-8f, -6f)).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                dust.scale = 1.5f;

            }
            for (int i = 0; i < 150; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, Main.rand.NextFloat(-22f, -12f)).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
            }
            for (int k = 0; k < 100; k++)
            {
                float smokevelocity = 5;
                Gore smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center, new Vector2(Main.rand.NextFloat(-smokevelocity, smokevelocity), Main.rand.NextFloat(-smokevelocity + 1.5f, smokevelocity + 1.5f)), Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));               
            }
            if (Projectile.owner == Main.myPlayer)
            {
                int explosionRadius = 15; // Bomb: 4, Dynamite: 7, Explosives & TNT Barrel: 10
                int minTileX = (int)(Projectile.Center.X / 16f - explosionRadius);
                int maxTileX = (int)(Projectile.Center.X / 16f + explosionRadius);
                int minTileY = (int)(Projectile.Center.Y / 16f - explosionRadius);
                int maxTileY = (int)(Projectile.Center.Y / 16f + explosionRadius);

                // Ensure that all tile coordinates are within the world bounds
                Utils.ClampWithinWorld(ref minTileX, ref minTileY, ref maxTileX, ref maxTileY);

                // These 2 methods handle actually mining the tiles and walls while honoring tile explosion conditions
                //bool explodeWalls = Projectile.ShouldWallExplode(Projectile.Center, explosionRadius, minTileX, maxTileX, minTileY, maxTileY); //only if you want explosions from the edge of walls
                Projectile.ExplodeTiles(Projectile.Center, explosionRadius, minTileX, maxTileX, minTileY, maxTileY, true);
            }
        }
        Color colorline = Color.Red;
        public override void PostDraw(Color lightColor) //glowmask for animated
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/AmmoProjs/C4Proj_Glow");
            //Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f + 6);

            Main.EntitySpriteDraw(texture, (Projectile.Center - Main.screenPosition), new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
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
                    Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(player.Center.X, player.Center.Y), colorline, Color.Transparent, 5);
                }
            }
            return true;
        }
    }
   
}