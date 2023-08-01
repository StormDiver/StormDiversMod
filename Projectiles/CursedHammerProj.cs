using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{
    public class CursedHammerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cursed Hammer");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.light = 0.6f;
            Projectile.friendly = true;
            Projectile.penetrate = 5;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
          
            DrawOffsetX = -3;
            DrawOriginOffsetY = -10;
        }
        int speedup = 0;
        int shoottime = 0;
        public override void AI()
        {
            
            /*Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);*/
            Projectile.spriteDirection = Projectile.direction;

            speedup++;
            shoottime++;
            if (speedup < 60)
            {
                Projectile.rotation = (0.4f * speedup);
            }
            if (speedup == 60)
            {


                SoundEngine.PlaySound(SoundID.Item34, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {

                     
                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 107);
                    dust2.noGravity = true;
                }
               
                
                    Projectile.velocity.X *= 12f;
                    Projectile.velocity.Y *= 12f;
                
                Projectile.penetrate = 2;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
            }

            if (speedup >= 60)
            {


                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;


                if (shoottime >= 17)
                {
                    float speedX = 0f;
                    float speedY = -4f;
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(90));
                    float scale = 1f - (Main.rand.NextFloat() * .5f);
                    perturbedSpeed = perturbedSpeed * scale;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<CursedHammerProj2>(), (int)(Projectile.damage * 0.7f), Projectile.knockBack, Projectile.owner);
                    for (int i = 0; i < 10; i++)
                    {

                         
                        var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 107);
                        dust2.noGravity = true;
                    }
                    shoottime = 0;
                }

            }
           



        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 107);
                dust.noGravity = true;
                //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 74);
            }

           
            {
                target.AddBuff(BuffID.CursedInferno, 600);

            }
            if (speedup < 60)
            {
                Projectile.damage = (Projectile.damage * 8) / 10;
            }
            else
            {
                Projectile.damage = (Projectile.damage * 9) / 10;

            }

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            
            return true;
        }

        public override void Kill(int timeLeft)
        {


            SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 107);
                dust.noGravity = true;
            }

        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool PreDraw(ref Color lightColor)
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

            return true;

        }
    }
    //___________________________________________________________________________________________
    public class CursedHammerProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cursed Hammer");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.light = 0.3f;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 200;
            //aiType = ProjectileID.WoodenArrowFriendly;
            Projectile.aiStyle = 1;
            Projectile.scale = 0.8f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            DrawOffsetX = -6;
            DrawOriginOffsetY = -15;
        }
        int spin = 0;
        public override void AI()
        {
            spin++;
            /*Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);*/
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation = (0.3f * spin);
           
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 107);
               dust.noGravity = true;
                
            }

           
            
                target.AddBuff(BuffID.CursedInferno, 400);


            Projectile.damage = (Projectile.damage * 9) / 10;


        }


        public override void Kill(int timeLeft)
        {


            
           SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 107);
               dust.noGravity = true;
            }

        }
        

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;

        }
    }
}