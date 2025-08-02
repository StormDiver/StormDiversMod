using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Common;
using Terraria.Audio;



namespace StormDiversMod.NPCs.NPCProjs
{
    public class FrozenEyeProj: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mini Snowflake");
        }
        public override void SetDefaults()
        {
            Projectile.coldDamage = true;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
           
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            //drawOffsetX = -2;
            //drawOriginOffsetY = -2;
        }
        public override void AI()
        {
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 0.7f);
            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex].noGravity = true;

            Projectile.ai[0]++;
            Projectile.rotation++;

            if (Projectile.ai[0] == 1)
            {
                for (int i = 0; i < 15; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
                    dust.velocity *= 2;
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Frostburn, 180);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
                dust.velocity *= 2;
            }
        }
    }
    //___________________________________________________________________________________________________________________________________
    public class FrozenSoulProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Giant Snowflake");
        }
        public override void SetDefaults()
        {
            Projectile.coldDamage = true;

            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 80;
            Projectile.aiStyle = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            //drawOffsetX = -2;
            //drawOriginOffsetY = -2;
        }
        public override void AI()
        {
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 0.7f);
            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex].noGravity = true;

            Projectile.ai[0]++;
            Projectile.rotation = Projectile.ai[0] / 3 * Projectile.direction;

            if (Projectile.ai[0] == 1)
            {
                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
                    dust.velocity *= 2;
                }
            }
            Projectile.ai[1]++;

            if (Projectile.ai[1] >= 20)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 15; i++)
                    {

                        var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
                        dust.velocity *= 2;
                    }
                    float numberProjectiles = 6;
                    float rotation = MathHelper.ToRadians(180);
                    for (int j = 0; j < numberProjectiles; j++)
                    {
                        Vector2 perturbedSpeed = new Vector2(0, 2.5f).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                        int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<FrozenEyeProj>(), Projectile.damage, Projectile.knockBack);
                        Main.projectile[projID].timeLeft = 30;
                    }
                }
                SoundEngine.PlaySound(SoundID.Item28 with{Volume = 1f, Pitch = 0.5f}, Projectile.Center);

                Projectile.ai[1] = 0;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Buffs.SuperFrostBurn>(), 180);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27 with{Volume = 1f, Pitch = 0.5f}, Projectile.Center);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                //SoundEngine.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 56, 0.5f);
                float numberProjectiles = 8;
                float rotation = MathHelper.ToRadians(180);
                for (int j = 0; j < numberProjectiles; j++)
                {
                    Vector2 perturbedSpeed = new Vector2(0, 7).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<FrozenEyeProj>(), Projectile.damage, Projectile.knockBack);
                    Main.projectile[projID].timeLeft = 60;
                }
            }
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
                dust.velocity *= 2;
            }
        }
    }
}