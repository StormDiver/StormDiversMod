using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.Enums;
using System.Transactions;

namespace StormDiversMod.Projectiles
{
    public class EyeSwordProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eye Sword Ball");

        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.light = 0.3f;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 2;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -1;
            DrawOriginOffsetY = -1;
        }
        int rotate;

        public override void AI()
        {
            rotate += 1;
            Projectile.rotation = rotate * (0.1f * Projectile.direction);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5);
            Projectile.spriteDirection = Projectile.direction;


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {



        }

        bool reflect = false;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 1f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.75f;
            }

            if (!reflect)
            {
                SoundEngine.PlaySound(SoundID.NPCHit1, Projectile.Center);
                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);

                }
            }
            else
            {
                Projectile.Kill();
            }
            reflect = true;

            return false;

        }


        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);

            }

        }

    }
    //_______________________________________________________________________
    public class EyeStaffProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bouncing Eye");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.light = 0.3f;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 200;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -1;
            DrawOriginOffsetY = -1;
        }

        public override void AI()
        {
            Projectile.rotation += (float)Projectile.direction * -0.4f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5);
            Projectile.spriteDirection = Projectile.direction;


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        bool reflect = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Player player = Main.player[Projectile.owner];

            if (Projectile.owner == Main.myPlayer)
            {         
                float projspeed = 10;
                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));

                Projectile.velocity.X = perturbedSpeed.X;
                Projectile.velocity.Y = perturbedSpeed.Y;

            }
            
            if (!reflect)
            {
                SoundEngine.PlaySound(SoundID.NPCHit1, Projectile.Center);
                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);

                }
            }
            else
            {
                Projectile.Kill();
            }
            reflect = true;

            return false;


        }


        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);

            }

        }

    }
}
