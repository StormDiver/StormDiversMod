using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;


namespace StormDiversMod.Projectiles.AmmoProjs
{
   
    public class IronShotProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Iron Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.aiStyle = 1;
         

            Projectile.friendly = true;
            Projectile.timeLeft = 400;
            Projectile.penetrate = 1;

            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;


            Projectile.DamageType = DamageClass.Ranged;
            Projectile.knockBack = 3f;
            AIType = ProjectileID.WoodenArrowFriendly;
            DrawOffsetX = 2;
            DrawOriginOffsetY = -0;
            Projectile.light = 0.2f;
        }



        public override void AI()
        {


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

        }
        public override Color? GetAlpha(Color lightColor)
        {



            return Color.White;

        }


    }
    public class LeadShotProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lead Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.aiStyle = 1;
            

            Projectile.friendly = true;
            Projectile.timeLeft = 400;
            Projectile.penetrate = 1;
            Projectile.knockBack = 3f;

            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;

            Projectile.DamageType = DamageClass.Ranged;

            AIType = ProjectileID.WoodenArrowFriendly;

            DrawOffsetX = 2;
            DrawOriginOffsetY = -0;

            Projectile.light = 0.2f;
        }



        public override void AI()
        {



           
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

        }

        public override Color? GetAlpha(Color lightColor)
        {



            return Color.White;

        }

    }

}
