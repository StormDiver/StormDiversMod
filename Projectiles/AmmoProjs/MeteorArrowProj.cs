using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles.AmmoProjs
{
    public class MeteorArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Meteor Arrow");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.arrow = true;
            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.arrow = true;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX =0;
            DrawOriginOffsetY = 0;
        }
        int hometime;
        public override void OnSpawn(IEntitySource source)
        {
            var player = Main.player[Projectile.owner];

            if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.MeteorBow>())
                Projectile.ai[2] = 1;
        }
        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            hometime++;
            /*if (hometime <= 35)
            {
                Projectile.velocity *= 1.04f;
            }*/
            if ((hometime == 30 && Projectile.ai[2] == 0) || (hometime == 20 && Projectile.ai[2] == 1))
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(0, -4f).RotatedByRandom(MathHelper.ToRadians(360));
                    Dust dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 127, perturbedSpeed.X, perturbedSpeed.Y, 0, new Color(255, 255, 255), 1.5f)];
                    dust.noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);

                Projectile.aiStyle = 0;
                Projectile.extraUpdates = 1;
                Projectile.penetrate = 3;
                Projectile.velocity *= 1.5f;
                Projectile.damage = (Projectile.damage *= 12) / 10; // 20% extra
            }

            if ((hometime >= 30 && Projectile.ai[2] == 0) || (hometime >= 20 && Projectile.ai[2] == 1))
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 0.7f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hometime >= 20)
            {
                target.AddBuff(BuffID.OnFire, 300);
            }
            for (int i = 0; i < 20; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, -4f).RotatedByRandom(MathHelper.ToRadians(360));
                Dust dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 127, perturbedSpeed.X, perturbedSpeed.Y, 0, new Color(255, 255, 255), 1.5f)];
                dust.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 127, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * .3f, 0, new Color(255, 255, 255), 1.5f)];
                //dust.noGravity = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
