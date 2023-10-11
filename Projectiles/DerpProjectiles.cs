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
    public class DerpMeleeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Shell Shard");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
           
            Projectile.friendly = true;
            
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.aiStyle = 14;
          
            Projectile.timeLeft = 300;
            DrawOffsetX = -3;
            DrawOriginOffsetY = 0;

            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {

            /*
            int dust = Dust.NewDustPerfect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, 48, 0f, 0f, 200, default, 1.5f);
                Main.dust[dust].velocity *= -1f;
                Main.dust[dust].noGravity = true;
               */

            Projectile.rotation += (float)Projectile.direction * -0.6f;


            Projectile.DamageType = DamageClass.Melee;
        }
        int reflect = 3;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();

            }

            {
                Collision.HitTiles(Projectile.Center + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.8f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
                }
            }
            SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);

            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = Projectile.damage * 8 / 10;
            Projectile.velocity *= 0.5f;
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                //Main.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 22);
                for (int i = 0; i < 5; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68);
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
   
    //__________________________________________________________________________________________________________________
    public class DerpMagicProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Magic Shell");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
           
            Projectile.friendly = true;

            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.aiStyle = 0;
            Projectile.timeLeft = 60;
            Projectile.scale = 0.7f;

        }
        public override void AI()
        {


            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.Center;
            dust = Terraria.Dust.NewDustPerfect(position, 68, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;

            Projectile.rotation += (float)Projectile.direction * -0.6f;



            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 150f;
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
                Projectile.velocity = (10 * Projectile.velocity + move) / 10f;
                AdjustMagnitude(ref Projectile.velocity);
            }

        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 10f)
            {
                vector *= 10f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

           
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        public override void OnKill(int timeLeft)
        {

            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.NPCHit22, Projectile.Center);
            for (int i = 0; i < 5; i++)
            {


                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68);
            }
            int numberProjectiles = 3 + Main.rand.Next(3);
            for (int i = 0; i < numberProjectiles; i++)
            {
                // Calculate new speeds for other projectiles.
                // Rebound at 40% to 70% speed, plus a random amount between -8 and 8
                float speedX = Main.rand.NextFloat(-7f, 7f);
                float speedY = Main.rand.NextFloat(-7f, 7f);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(speedX, speedY), ModContent.ProjectileType<DerpMagicProj2>(), (int)(Projectile.damage * 0.75), 0, Projectile.owner);
            }
        }

    }
    //__________________________________________________________________________________________________________________
    public class DerpMagicProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Derpling Magic Shard");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
           
            Projectile.friendly = true;

            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            //Projectile.extraUpdates = 1;
            //Projectile.CloneDefaults(297);
            Projectile.aiStyle = 2;
            //aiType = 297;
            //Projectile.timeLeft = 240;
            Projectile.timeLeft = 360;
        }
       
        public override void AI()
        {
            Projectile.rotation += (float)Projectile.direction * -0.6f;

            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.Center;
            dust = Terraria.Dust.NewDustPerfect(position, 68, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 0.5f);
            dust.noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            //Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            //Main.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 22);
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68);
            }

        }
    }
}
