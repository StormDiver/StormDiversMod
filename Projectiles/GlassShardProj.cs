using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Buffs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace StormDiversMod.Projectiles
{
    public class GlassShardProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("GlassShard");
            ProjectileID.Sets.DontAttachHideToAlpha[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.knockBack = 0f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ArmorPenetration = 5;
        }
        //bool sticktarget;
        int stickTimer;
        int sticktime = 300;
        public int TargetWhoAmI
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public bool sticktarget
        {
            get => Projectile.ai[2] == 1f;
            set => Projectile.ai[2] = value ? 1f : 0f;
        }
        Vector2 projoffest;

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
                    Projectile.Center = Main.npc[npcTarget].Center + projoffest - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.npc[npcTarget].gfxOffY;
                }
                else
                { // Otherwise, kill the projectile
                    Projectile.Kill();
                }
            }
           // else
             //   Projectile.velocity.Y = 3; //prevent glitch where they can remain in the air
        }
        private const int maxShards = 10; //10 max shards
        private readonly Point[] stickingShards = new Point[maxShards]; 
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Bleeding, sticktime);

            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 149, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);
            }
            if (!sticktarget)//attach to enemy
            {
                target.AddBuff(ModContent.BuffType<GlassShardDebuff>(), sticktime);
                Projectile.timeLeft = sticktime;
                Projectile.damage = 0; 
                projoffest = new Vector2(Main.rand.NextFloat(-target.width / 6, target.width / 6), Main.rand.NextFloat(-target.height / 6, target.height / 6));
                Projectile.extraUpdates = 0;
                TargetWhoAmI = target.whoAmI; // Set the target whoAmI
                Projectile.velocity = (target.Center - Projectile.Center) * 0.75f; // Change velocity based on delta center of targets (difference between entity centers)
                Projectile.netUpdate = true; // netUpdate this shard
                sticktarget = true; // we are sticking to a target
            }                
            Projectile.KillOldestJavelin(Projectile.whoAmI, Type, target.whoAmI, stickingShards);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 149);
                dust.scale = 0.75f;
            }
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 0.5f, Pitch = 0.25f }, Projectile.Center);
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
    }
    public class GlassStaffProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glass orb");
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;
            Projectile.scale = 1f;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        Vector2 mousepos;
        public override void OnSpawn(IEntitySource source)
        {
        }
        public override void AI()
        {
            if (Projectile.ai[0] > 5f)  //this defines where the flames starts
            {
                for (int i = 0; i < 3; i++)
                {
                    float X = Projectile.Center.X - Projectile.velocity.X / 5f * (float)i;
                    float Y = Projectile.Center.Y - Projectile.velocity.Y / 5f * (float)i;

                    int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 156, 0, 0, 100, default, 1f);
                    Main.dust[dust].position.X = X;
                    Main.dust[dust].position.Y = Y;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.4f;
                    Main.dust[dust].noLight = true;
                }
            }
            else
                Projectile.ai[0] += 1f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 156, Projectile.oldVelocity.X, Projectile.oldVelocity.Y);
                dust.scale = 1f;
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.fadeIn = 1.2f;
            }
            SoundEngine.PlaySound(SoundID.Dig with { Volume = 0.75f }, Projectile.Center);
        }
    }
}
