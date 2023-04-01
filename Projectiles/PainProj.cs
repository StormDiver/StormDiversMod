using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles
{
    public class PainProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Painful Projectile");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.light = 0f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 300;
            //aiType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 1;
            
            //drawOffsetX = 2;
            //drawOriginOffsetY = -10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 72, 0f, 0f, 0, default, 1.5f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
            base.OnSpawn(source);
        }
        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            if (Projectile.position.Y > Main.MouseWorld.Y && Projectile.ai[0] == 0)
            {
                Projectile.tileCollide = true;
            }
            else
            {
                Projectile.tileCollide = false;

            }

            Projectile.ai[1]++;
            Projectile.rotation = Projectile.ai[1] / 5;
            //Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, 175);
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X * -0.2f, Projectile.velocity.Y * -0.2f, 0, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
            Main.dust[dust].noGravity = true; //this make so the dust has no gravity
           
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 120;
                Projectile.height = 120;
                Projectile.Center = Projectile.position;


                Projectile.knockBack = 6;
            }
            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 1000f;
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

        private void AdjustMagnitude(ref Vector2 vector)
        {

            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 15f)
            {
                vector *= 15f / magnitude;
            }

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            return false;

        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath6 with{Volume = 1.5f, Pitch = 0.5f}, Projectile.Center);
            //SoundEngine.PlaySound(SoundID.ScaryScream with{ Volume = 0.5f, Pitch = 1f}, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionPainProj>(), 0, 0, Projectile.owner);
            //Main.projectile[proj].scale = 1;

            /*for (int i = 0; i < 30; i++)
            {

                int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 72, 0f, 0f, 0, default, 1.5f);
                //Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                //Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 3;

            }*/
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft > 3)
            {
                return Color.White;
            }
            else
            {
                return null;
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
    public class PainProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Painful Core");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.light = 0f;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
        }
        float xPos;
        float yPos;
        public override bool? CanDamage()
        {
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            var player = Main.player[Projectile.owner];

            xPos = Projectile.Center.X - player.Center.X;
            yPos = Projectile.Center.Y - player.Center.Y;
        }
        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 1)
            {
                Projectile.timeLeft = 50;
                Projectile.rotation += 0.05f;
            }
            else
            {
                Projectile.position.X = player.Center.X + xPos;
                Projectile.position.Y = player.Center.Y + yPos;

            }
        }
        public override void Kill(int timeLeft)
        {
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 150;
            return color;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] == 1)
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
            }
            return true;
        }
    }
}