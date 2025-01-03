using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using StormDiversMod.Common;


namespace StormDiversMod.Projectiles
{
	public class JungleSporeProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            //DisplayName.SetDefault("Jungle Spore");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
            Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.scale = 1f;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;

			Projectile.timeLeft = 300;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.extraUpdates = 0;
        }
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 15; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 44, 0, 0, 100, default, 1.2f);
                dust.noGravity = true;
            }
        }
        public override void AI()
		{
			Projectile.rotation += 0.2f * Projectile.direction;

            var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 44, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 100, default, 0.8f);
			dust.noGravity = true;
      
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;

                Projectile.velocity *= 0;
                Projectile.alpha = 255;

                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;

                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 60;
                Projectile.height = 60;
                Projectile.Center = new Vector2(Projectile.position.X, Projectile.position.Y);

                Projectile.knockBack = 0f;
            }
           
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.Next(2) == 0)
                target.AddBuff(BuffID.Poisoned, 300);

            for (int i = 0; i < 15; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 44, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 100, default, 1.2f);
                dust.noGravity = true;
            }

            if (Projectile.timeLeft > 3)
                Projectile.timeLeft = 3;

            Projectile.damage = (Projectile.damage * 8) / 10;
            /*Vector2 perturbedSpeed = new Vector2(Main.rand.NextFloat(-1, 1), 7).RotatedByRandom(MathHelper.ToRadians(12));
            Projectile.velocity = -perturbedSpeed * 0.75f;
            Projectile.damage = (Projectile.damage * 5) / 10;
            Projectile.scale = .75f;
            Projectile.aiStyle = 1;*/
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 3)
                Projectile.timeLeft = 3;
            Projectile.velocity *= 0f;

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74 with { Volume = 0.5f, Pitch = 1f }, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionSporeProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1f;

            for (int i = 0; i < 50; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, -20f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust2 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 0, 0, 44, perturbedSpeed.X, perturbedSpeed.Y);
                dust2.noGravity = true;
                dust2.scale = 1.5f;
            }
        }
        public override bool PreDraw(ref Color lightColor) //trial
        {
            Main.instance.LoadProjectile(Projectile.type);
            //Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/BetsyFlameProj_Trail");
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            if (Projectile.timeLeft > 3)
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                        color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
		{
            if (Projectile.timeLeft > 3)
            {
                Color color = Color.White;
                color.A = 150;
                return color;
            }
            else
                return null;
        }
    }
}