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
    public class GraniteBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;

            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 3;


            Projectile.tileCollide = true;


            Projectile.DamageType = DamageClass.Ranged;

            AIType = ProjectileID.Bullet;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

        }


        int dusttime;
        public override void AI()
        {

            dusttime++;

            if (dusttime > 5)
            {
                for (int i = 0; i < 10; i++)
                {
                    float X = Projectile.Center.X - Projectile.velocity.X / 5f * (float)i;
                    float Y = Projectile.Center.Y - Projectile.velocity.Y / 5f * (float)i;


                    int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 70, 0, 0, 100, default, 1f);
                    Main.dust[dust].position.X = X;
                    Main.dust[dust].position.Y = Y;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;

                }
            }

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 15; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 70, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 130, default, 1.2f);
                dust.noGravity = true;
            }
            Projectile.damage = (Projectile.damage * 9) / 10;
        }

        public override void Kill(int timeLeft)
        {

            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 70);
                dust.noGravity = true;

            }

        }

        public override Color? GetAlpha(Color lightColor)
        {

            return Color.White;
        }


    }
    //______________________________________________________________________________________________________________________
    public class GraniteSpearProj : ModProjectile
    {
        //unused
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Spear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 19;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }


        public float MovementFactor // Change this value to alter how fast the spear moves
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            // Sadly, Projectile/ModProjectile does not have its own
            Player projOwner = Main.player[Projectile.owner];
            // Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
            // As long as the player isn't frozen, the spear can move
            if (!projOwner.frozen)
            {
                if (MovementFactor == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    MovementFactor = 1.2f; // Make sure the spear moves forward when initially thrown out
                    Projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 2) // Somewhere along the item animation, make sure the spear moves back
                {
                    MovementFactor -= 1f;
                }
                else // Otherwise, increase the movement factor
                {
                    MovementFactor += 1.2f;
                }
            }
            // Change the spear position based off of the velocity and the movementFactor
            Projectile.position += Projectile.velocity * MovementFactor;
            // When we reach the end of the animation, we can kill the spear projectile
            if (projOwner.itemAnimation == 0)
            {
                Projectile.Kill();
            }
            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            // Offset by 90 degrees here
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(90f);
            }

            // These dusts are added later, for the 'ExampleMod' effect
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, 70, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 1, Scale: 1.2f);
                dust.noGravity = true;
                dust.velocity += Projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;
            }

        }


    }

    //___________________________________
    public class GraniteSpearProj2 : ModProjectile
    {
        //unused
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Spear Orb");


        }
        public override void SetDefaults()
        {

            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 25;
            Projectile.aiStyle = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        int dusttime;
        public override void AI()
        {

            dusttime++;

            if (dusttime > 10)
            {

                for (int i = 0; i < 10; i++)
                {
                    float X = Projectile.Center.X - Projectile.velocity.X / 5f * (float)i;
                    float Y = Projectile.Center.Y - Projectile.velocity.Y / 5f * (float)i;


                    int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 70, 0, 0, 100, default, 1);
                    Main.dust[dust].position.X = X;
                    Main.dust[dust].position.Y = Y;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;

                }
            }
            Dust dust2;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position2 = Projectile.Center;
            dust2 = Terraria.Dust.NewDustPerfect(position2, 65, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1.5f);
            dust2.noGravity = true;
            dust2.noLight = true;


        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {

        }

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 70);
                dust.noGravity = true;
            }
        }

    }
    //___________________________________
    public class GraniteStaffProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Energy Orb");


        }
        public override void SetDefaults()
        {

            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;


        }

        public override void AI()
        {


            for (int i = 0; i < 10; i++)
            {
                float X = Projectile.Center.X - Projectile.velocity.X / 5f * (float)i;
                float Y = Projectile.Center.Y - Projectile.velocity.Y / 5f * (float)i;


                int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 70, 0, 0, 100, default, 1f);
                Main.dust[dust].position.X = X;
                Main.dust[dust].position.Y = Y;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].scale *= 1f;
            }
            Dust dust2;
            Vector2 position2 = Projectile.Center;
            dust2 = Terraria.Dust.NewDustPerfect(position2, 65, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1.5f);
            dust2.noGravity = true;
            dust2.noLight = true;


        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {

        }

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 70);
                dust.noGravity = true;
            }
        }

    }
    //_______________________________________________________
    public class GraniteYoyoProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Yoyo");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 4f;

            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 160f;

            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 12.5f;

            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void SetDefaults()
        {

            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 240;

            Projectile.scale = 1f;
            Projectile.aiStyle = 99;

            // drawOffsetX = -3;
            // drawOriginOffsetY = 1;
        }
        public override void AI()
        {
          
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/GraniteYoyoProj_Glow");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
        public override void PostDraw(Color drawColor)
        {
            
                Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/GraniteYoyoProj_Glow");

                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Center, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            

        }
    }
}
