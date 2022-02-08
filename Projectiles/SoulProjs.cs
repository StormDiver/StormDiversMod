using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;


namespace StormDiversMod.Projectiles
{
    
    public class SoulFrightProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Projectile of Fright");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 24;
            Projectile.light = 0.5f;
            
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.scale = 1.5f;
            DrawOffsetX = -0;
            DrawOriginOffsetY = -0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

        }
       
  
        public override void AI()
        {
           
            AnimateProjectile();

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 259);
                dust.velocity *= 0.5f;
                dust.noGravity = true;
            }

            if (Projectile.timeLeft >= 30)
            {

                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 259);
                    dust.velocity *= 2;
                    dust.noGravity = true;

                }
            }
            var player = Main.player[Projectile.owner];

        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            if (Projectile.timeLeft >= 35)
            {
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                if (magnitude > 60f)
                {
                    vector *= 60f / magnitude;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 259, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                dust.scale = 1.5f;
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {


            for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 259);
                    dust.velocity *= 2;
                    dust.noGravity = true;

                }

        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }

    }
    //______________________________
    public class SoulSightProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Projectile of Sight");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 24;
            Projectile.light = 0.5f;
           
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.scale = 1.5f;
            DrawOffsetX = -0;
            DrawOriginOffsetY = -0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

        }

        public override void AI()
        {

            AnimateProjectile();

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
          
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 110);
                dust.velocity *= 0.5f;
                dust.noGravity = true;
            }

            if (Projectile.timeLeft >= 30)
            {

                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 110);
                    dust.velocity *= 2;
                    dust.noGravity = true;

                }
            }
            var player = Main.player[Projectile.owner];

          

        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            if (Projectile.timeLeft >= 35)
            {
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                if (magnitude > 60f)
                {
                    vector *= 60f / magnitude;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 110, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
                dust.scale = 0.75f;
            }
        }

        public override void Kill(int timeLeft)
        {


            for (int i = 0; i < 15; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 110);
                dust.velocity *= 2;
                dust.noGravity = true;

            }


        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }

    }
    //_____________________
    public class SoulMightProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Projectile of Might");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 24;
            Projectile.light = 0.5f;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.scale = 1.5f;
            DrawOffsetX = -0;
            DrawOriginOffsetY = -0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }


        public override void AI()
        {

            AnimateProjectile();

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
           
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 56);
                dust.velocity *= 0.5f;
                dust.noGravity = true;
            }
            if (Projectile.timeLeft >= 30)
            {

                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 56);
                    dust.velocity *= 2;
                    dust.noGravity = true;

                }
            }

            var player = Main.player[Projectile.owner];

           

        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            if (Projectile.timeLeft >= 35)
            {
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                if (magnitude > 60f)
                {
                    vector *= 60f / magnitude;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 56, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);

            }

        }

        public override void Kill(int timeLeft)
        {


            for (int i = 0; i < 15; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 56);
                dust.velocity *= 2;
                dust.noGravity = true;

            }


        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }

    }

}