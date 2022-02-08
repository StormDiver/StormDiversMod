using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Buffs;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent;


namespace StormDiversMod.Projectiles
{


    public class SelenianBladeProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Blade");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {

            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            Projectile.CloneDefaults(106);
            AIType = 106;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.penetrate = -1;
        }

        bool stillspin;
        int stilltime;
        public override void AI()
        {


            Projectile.width = 36;
            Projectile.height = 36;

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);  //this is the dust that this projectile will spawn
            Main.dust[dust].velocity /= 1f;
            Main.dust[dust].scale = 1f;
            Main.dust[dust].noGravity = true;
            Projectile.penetrate = -1;
            if (stillspin)//while active projectile does not move
            {
                Projectile.velocity.Y *= 0f;
                Projectile.velocity.X *= 0f;
                stilltime++; //timer counts up while not moving
            }
            if (stilltime >= 45) //once this time is reached it cannot stay still again
            {
                stillspin = false;
            }

            
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {


            //Main.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 2);
            Player player = Main.player[Projectile.owner];

            target.AddBuff(BuffID.Daybreak, 300);
            for (int i = 0; i < 10; i++)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(20, 20), Main.rand.NextFloat(-20, -20));
                var dust = Dust.NewDustDirect(target.position, target.width, target.height, 6);
                dust.scale = 2f;
                dust.noGravity = true;

            }
            if (stilltime < 45) //So the projectile doesn't stay still again
            {
                stillspin = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            //Main.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 2);
            for (int i = 0; i < 10; i++)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(20, 20), Main.rand.NextFloat(-20, -20));
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                dust.scale = 2f;
                dust.noGravity = true;

            }
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);



            }



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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 75;
            return color;

        }
    }
    
}