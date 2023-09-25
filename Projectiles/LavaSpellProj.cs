using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{
    public class LavaSpellProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magic Lava Orb");
            Main.projFrames[Projectile.type] = 5;

        }
        public override void SetDefaults()
        {

            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            AnimateProjectile();

            if (Main.rand.NextFloat() < 1f)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 259, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 0, new Color(255, 255, 255), .8f);
                dust.noGravity = true;
                /*Dust dust2;
                dust2 = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 127, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust2.noGravity = true;*/
               

            }
            Dust dust3;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position2 = Projectile.Center;
            dust3 = Terraria.Dust.NewDustPerfect(position2, 127, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust3.noGravity = true;
            dust3.noLight = true;


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SuperBurnDebuff>(), 300);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<SuperBurnDebuff>(), 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.Item86 with{Volume = 1f, Pitch = -2}, Projectile.Center);
                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width = 10, Projectile.height = 10, 259);
                }

            }
            {

                int numberProjectiles = 4 + Main.rand.Next(2); //This defines how many projectiles to shot.


                for (int i = 0; i < numberProjectiles; i++)
                {


                    float speedX = 0f;
                    float speedY = -7f;

                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(170));
                    float scale = 1f - (Main.rand.NextFloat() * .2f);
                    perturbedSpeed = perturbedSpeed * scale;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<LavaSmallProj>(), (int)(Projectile.damage * 0.3f), 1, Projectile.owner);
                }
            }
        }
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 5; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //_____________________________________________________
    public class LavaSmallProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Small Lava Bolt");
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = 14;
            Projectile.DamageType = DamageClass.Magic;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            //drawOffsetX = -2;
            //drawOriginOffsetY = -2;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Main.rand.Next(2) == 0)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 259, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 0, new Color(255, 255, 255), .8f);
                dust.noGravity = true;
                Dust dust2;
                dust2 = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 127, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust2.noGravity = true;


            }
            Projectile.rotation += (float)Projectile.direction * -0.2f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SuperBurnDebuff>(), 300);

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<SuperBurnDebuff>(), 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Item86 with{Volume = 1f, Pitch = -2}, Projectile.Center);

                for (int i = 0; i < 5; i++)
                {

                    
                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width = 10, Projectile.height = 10, 259);
                    dust.scale = 0.5f;

                }

            }
        }
    }
   
}
