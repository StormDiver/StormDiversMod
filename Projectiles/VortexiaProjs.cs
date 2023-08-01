using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using StormDiversMod.Buffs;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using Terraria.GameContent;

namespace StormDiversMod.Projectiles
{
    public class VortexiaProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Large Vortex");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 70;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.light = 0.5f;
        }

        int rotate;
        public override void AI()
        {
            rotate++;
            Projectile.rotation = rotate * 0.1f;
           
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 229, 0, 0, 130, default, 1f);

                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.5f;
            
            Projectile.ai[1]++;
            if (Projectile.ai[1] >= 15)
            {
                SoundEngine.PlaySound(SoundID.Item20 with { Volume = 1f, Pitch = 0.5f }, Projectile.Center);

                Vector2 perturbedSpeed = new Vector2(0, -2.5f).RotatedByRandom(MathHelper.ToRadians(180));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<VortexiaProj2>(), (int)(Projectile.damage * 0.7f), Projectile.knockBack, Projectile.owner);

                for (int i = 0; i < 25; i++)
                {
                    Vector2 perturbeddustSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));
                    int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 229, perturbeddustSpeed.X, perturbeddustSpeed.Y, 200, default, 1.5f);
                    Main.dust[dust2].noGravity = true;
                }
                Projectile.ai[1] = 0;
            }
        }
      
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            return true;

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 229, 0, 0, 120, default, 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1f, Pitch = -0.5f }, Projectile.Center);

                float numberProjectiles = Main.rand.Next(4, 6); //4 to 5
                float rotation = MathHelper.ToRadians(180);
                for (int j = 0; j < numberProjectiles; j++)
                {

                    Vector2 perturbedSpeed = new Vector2(0, 4f).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<VortexiaProj2>(), (int)(Projectile.damage * 0.7f), Projectile.knockBack, Projectile.owner);

                }

                for (int i = 0; i < 50; i++)
                {
                    Vector2 perturbeddustSpeed = new Vector2(0, -10f).RotatedByRandom(MathHelper.ToRadians(360));
                    int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 229, perturbeddustSpeed.X, perturbeddustSpeed.Y, 200, default, 1.5f);
                    Main.dust[dust2].noGravity = true;
                }
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            }

            return true;

        }
       
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //____________________________________________________________________________________________________________________________________________________________________
    public class VortexiaProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Homing Vortex");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.light = 0.5f;
        }

        int rotate;
        public override void AI()
        {
            rotate++;
            Projectile.rotation = rotate * 0.1f;
            if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 229, 0, 0, 130, default, 1f);

                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.5f;
            }

            if (rotate >= 30)
            {
                if (Projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;
                float distance = 400f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
                    {
                        if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                        {
                            Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                            if (distanceTo < distance)
                            {
                                move = newMove;
                                distance = distanceTo;
                                target = true;
                            }
                        }
                    }
                }

                if (target)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (10f * Projectile.velocity + move) / 10f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            if (rotate >= 25)
            {
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                if (magnitude > 10f)
                {
                    vector *= 10f / magnitude;
                }
            }
        }
        public override bool? CanDamage()
        {
            if (rotate <= 25)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            if (rotate <= 40)
            {

                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 1;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 1f;
                }

                return false;
            }
            else
            {
                return true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0, 0, 120, default, 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath7 with {Volume = 0.5f, Pitch = 0.5f}, Projectile.Center);

                float speedX = 0f;
                float speedY = -4f;
                for (int i = 0; i < 25; i++)
                {
                    Vector2 perturbeddustSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 229, perturbeddustSpeed.X, perturbeddustSpeed.Y, 200, default, 1.5f);
                    Main.dust[dust2].noGravity = true;
                }

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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            }

            return true;

        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}

