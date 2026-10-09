using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Common;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;


namespace StormDiversMod.Projectiles
{
    public class SpikeBombProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spiky Bomb");
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.aiStyle = 14;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 180;
        }
        public override void AI()
        {
            if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Projectile.Kill();
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.4f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.4f;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }
        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionCompactProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1f;
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            float numberProjectiles = 6 + Main.rand.Next(2);

            for (int i = 0; i < numberProjectiles; i++)
            {
                float speedX = 0f;
                float speedY = -5f;
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(180));
                float scale = 1f - (Main.rand.NextFloat() * .25f);
                perturbedSpeed = perturbedSpeed * scale;

                int projID = Projectile.NewProjectile(null, new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<SpikeBombProj2>(), Projectile.damage / 3, 1, Projectile.owner);
            }
     
            for (int i = 0; i < 20; i++) 
            {
                Vector2 perturbedSpeed = new Vector2(0, -1f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.velocity *= 2f;
            }
            for (int i = 0; i < 20; i++) //Grey dust fade
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default, 1f);
                Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
    }
    //_____________________________________________________________________________________________________________________________________________________
    public class SpikeBombProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spiky Bomb Spike");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Ranged;
            //DrawOffsetX = -2;
        }
        int stickTimer;
        int sticktime = 600;
        public int TargetWhoAmI
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public bool sticktarget
        {
            get => Projectile.ai[0] == 1f;//this has to be set to 0 otherwise there's no limit, create custom gravity for Ai or smth
            set => Projectile.ai[0] = value ? 1f : 0f;
        }
        Vector2 projoffest;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 595)
            {
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X * 0.9f;
                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
                return false;
            }
            else
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                return true;
            }
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (sticktarget)
            {
                Projectile.aiStyle = 0;
                Projectile.penetrate = 1;
                //Projectile.alpha = 255;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                stickTimer += 1;
                int npcTarget = TargetWhoAmI;
                if (stickTimer >= sticktime || npcTarget < 0 || npcTarget >= 200)
                {
                    Projectile.Kill();
                }
                else if (Main.npc[npcTarget].active && !Main.npc[npcTarget].dontTakeDamage)
                {
                    // If the target is active and can take damage
                    // Set the projectile's position relative to the target's center
                    Projectile.Center = Main.npc[npcTarget].Center + projoffest - Projectile.velocity * 1.75f;
                    Projectile.gfxOffY = Main.npc[npcTarget].gfxOffY;
                }
                else
                { // Otherwise, kill the projectile
                    Projectile.Kill();
                }
            }
            else
            {
                if (Projectile.velocity.Y < 10)
                    Projectile.velocity.Y += 0.07f;
            }
        }
        private const int maxSpikes = 10; //10 max spikes
        private readonly Point[] stickingSpikes = new Point[maxSpikes];

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 149, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);
            }
            if (!sticktarget)//attach to enemy
            {
                target.AddBuff(ModContent.BuffType<SpikedDebuff>(), sticktime);
                Projectile.timeLeft = sticktime;
                Projectile.damage = 0;
                projoffest = new Vector2(Main.rand.NextFloat(-target.width / 6, target.width / 6), Main.rand.NextFloat(-target.height / 6, target.height / 6));
                Projectile.extraUpdates = 0;
                TargetWhoAmI = target.whoAmI; // Set the target whoAmI
                Projectile.velocity = (target.Center - Projectile.Center) * 0.75f; // Change velocity based on delta center of targets (difference between entity centers)
                Projectile.netUpdate = true; // netUpdate this spikes
                sticktarget = true; // we are sticking to a target
            }
            Projectile.KillOldestJavelin(Projectile.whoAmI, Type, target.whoAmI, stickingSpikes);
        }
        public override void OnKill(int timeLeft)
        {
            //Main.PlaySound(SoundID.Item10, Projectile.position);
            SoundEngine.PlaySound(SoundID.Tink with { Volume = 0.5f }, Projectile.Center);
            for (int i = 0; i < 6; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 11);
                dust.scale = 0.8f;
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (sticktarget)
            {
                int npcIndex = TargetWhoAmI;
                if (npcIndex >= 0 && npcIndex < 200 && Main.npc[npcIndex].active)
                {
                    if (Main.npc[npcIndex].behindTiles) //this works I guess
                    {
                        Projectile.hide = true;
                        behindNPCsAndTiles.Add(index);
                    }
                    else
                    {
                        Projectile.hide = false;
                        behindNPCsAndTiles.Add(index);
                    }
                    return;
                }
            }
            // Since we aren't attached, add to this list
            behindNPCsAndTiles.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!sticktarget)
            {
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                }
            }
            return true;
        }
    }
}