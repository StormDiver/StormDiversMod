using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;


namespace StormDiversMod.NPCs.NPCProjs
{
    public class SnowmanPizzaProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Red Soul");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.coldDamage = true;

            Projectile.width = 26;
            Projectile.height = 26;

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = 1;

            Projectile.tileCollide = true;
            Projectile.scale = 1f;

            Projectile.ignoreWater = true;

            Projectile.timeLeft = 360;
            //aiType = ProjectileID.LostSoulHostile;
            Projectile.aiStyle = 14;
            // Projectile.CloneDefaults(452);
            //aiType = 452;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }

        //float speed = 8f;
        //float inertia = 5;
        //Vector2 idlePosition;
        public override void AI()
        {
            if (Main.rand.Next(5) == 0)
            {
                var dust3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 124, 0, 0);
                dust3.scale = 0.75f;
                dust3.noGravity = true;
            }
            Projectile.ai[0]++;

            Projectile.rotation += 0.15f * Projectile.direction;

            /*idlePosition = Main.LocalPlayer.Center;
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Projectile.ai[0] >= 30 && Projectile.ai[0] <= 60)
            //if (Collision.CanHit(Projectile.Center, 0, 0, Main.MouseWorld, 0, 0))
            {
                if (distanceToIdlePosition > 10f)
                {
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                }
                if (Vector2.Distance(idlePosition, Projectile.Center) > 30)
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            }*/

            /*if (Projectile.ai[0] >= 30 && Projectile.ai[0] < 60)
            {
                Projectile.velocity.X *= 0.98f;
                Projectile.velocity.Y *= 0.98f;
            }
            if (Projectile.ai[0] == 60)
            {
                for (int i = 0; i < 200; i++)
                {
                    Player target = Main.player[i];

                    if (Vector2.Distance(target.Center, Projectile.Center) < 2000f && target.active)
                    {
                        float projspeed = 3f;
                        Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;

                        Projectile.velocity.X = velocity.X;
                        Projectile.velocity.Y = velocity.Y;
                    }
                }
            }
            if (Projectile.ai[0] >= 60 && Projectile.ai[0] < 90)
            {
                Projectile.velocity.X *= 1.04f;
                Projectile.velocity.Y *= 1.04f;
            }*/
        }

        int reflect = 4;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            reflect--;
            if (reflect <= 0)
                Projectile.Kill();
            else
            {
                SoundEngine.PlaySound(SoundID.NPCHit1, Projectile.Center);
                for (int i = 0; i < 20; i++)
                {
                    int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5, 0f, 0f, 50, default, 1f);
                    int dust3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 124, 0f, 0f, 50, default, 0.5f);
                }
            }
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            Projectile.velocity *= 0.9f;
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {
                int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5, 0f, 0f, 50, default, 1f);
                int dust3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 124, 0f, 0f, 50, default, 0.5f);
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }
    //____________________________________
    //___________________________________________________________
    public class SnowmanExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Snowman Explosion");
        }

        public override void SetDefaults()
        {
            Projectile.coldDamage = true;

            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.friendly = true;
            Projectile.hostile = true;

            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;

            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.hide = true;
        }
        public override void AI()
        {
           
            if (Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 150;
                Projectile.height = 150;
                Projectile.Center = Projectile.position;

                Projectile.knockBack = 3f;
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
            }
        }
       
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionGenericProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1.5f;
            for (int i = 0; i < 10; i++) //Snowballs
            {
                float speedX = 0f;
                float speedY = -13f;
                Vector2 perturbedSpeed2 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(80));
                float scale = 1f - (Main.rand.NextFloat() * .5f);
                perturbedSpeed2 = perturbedSpeed2 * scale;
                //Projectile.NewProjectile(player.Center.X, player.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("FrostAccessProj"), 50, 3f, player.whoAmI);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed2.X, perturbedSpeed2.Y), ModContent.ProjectileType<SnowmanSnowball>(), Projectile.damage / 2, Projectile.knockBack);
            }

            for (int i = 0; i < 30; i++) //Orange particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -6f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 2f;

            }
            for (int i = 0; i < 40; i++) //Grey dust circle7
            {
                Vector2 perturbedSpeed = new Vector2(0, -4f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
            }

        }
    }
    public class SnowmanSnowball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Snowball");
        }

        public override void SetDefaults()
        {
            Projectile.coldDamage = true;

            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;

            Projectile.tileCollide = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = 14;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f * Projectile.direction;
            if (Main.rand.Next(7) == 0)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 76, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }
           if (Projectile.ai[2] == 1) //for Snow balla
            {
                Projectile.aiStyle = 0;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Projectile.ai[2] ==0)
            {
                Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit11, Projectile.Center);

            for (int i = 0; i < 15; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 76);
                dust.noGravity = true;
                dust.velocity *= 2;
                //Main.PlaySound(SoundID.NPCHit, (int)Projectile.Center.X, (int)Projectile.Center.Y, 3);
            }

        }
    }
}
