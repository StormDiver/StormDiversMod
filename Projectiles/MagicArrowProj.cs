using System;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{
    public class MagicArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magic Arrow");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.light = 0.4f;
            Projectile.friendly = true;
            
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 99999999;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = -20;
            Projectile.penetrate = -1;          
        }
        bool returnsound = false;

        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            //player.Center = Projectile.Center; //Funni follow
            //player.velocity.Y = -0.1f;
            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage); //update damage

            for (int i = 0; i < 10; i++)
            {
                float X = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                float Y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;

                int dust = Dust.NewDust(new Vector2(X, Y), 0, 0, 90, 0, 0, 100, default, 1f);
                Main.dust[dust].position.X = X;
                Main.dust[dust].position.Y = Y;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0f;

            }
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
   
            Vector2 playerdistance = new Vector2(player.Center.X, player.Center.Y - 10) - Projectile.Center;
            float distanceToplayer = playerdistance.Length();

            if (player.channel && !player.dead) //if holding down button target enemies
            {
                Vector2 move = Vector2.Zero;
                float distance = 1000f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy() && Main.npc[k].GetGlobalNPC<NPCEffects>().arrowcooldown == 0)
                    {
                        if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                        {
                            Vector2 newMove = Main.npc[k].Center - Projectile.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20));
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
                if (target) //player sounds when targetting enemies
                {
                    if (Projectile.soundDelay == 0)
                    {
                        //SoundEngine.PlaySound(SoundID.Item1 with { Volume = 1f, Pitch = 0.5f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Projectile.position);
                        Projectile.soundDelay = Main.rand.Next(10, 10);
                    }
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
                    AdjustMagnitude(ref Projectile.velocity);
                }

                if (distanceToplayer > 1000 && target == false)
                {
                    SoundEngine.PlaySound(SoundID.Item1 with { Volume = 1f, Pitch = -0.5f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Projectile.position);

                    player.channel = false;
                }
            }
            if (distanceToplayer > 2000)
            {
                Projectile.Kill();
            }
           
            /*player.manaRegenDelay = 60; //prevent mana from regenerating
            
            if (player.statMana > 0)
            {
                if (Main.rand.Next(100) <= 20)
                {
                    if (ModLoader.HasMod("TRAEProject"))//bool if TRAE
                    {
                        player.statMana -= 2;
                    }
                    else
                    {
                        player.statMana -= 1;
                    }
                }
            }
            if (player.statMana <= 0)
           {
               player.channel = false;
           }*/
            if (!player.channel) //if release return to player
            {
                if (!returnsound)
                {
                    SoundEngine.PlaySound(SoundID.Item1 with { Volume = 1.5f, Pitch = -0.5f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Projectile.position);
                    returnsound = true;
                }
                Main.player[Projectile.owner].SetDummyItemTime(10);

                Projectile.tileCollide = false;
                Player target = Main.player[Projectile.owner];
             
                if (distanceToplayer < 15)
                {
                    Main.player[Projectile.owner].SetDummyItemTime(0);

                    Projectile.Kill();
                }

                //If the distance between the live targeted npc and the Projectile is less than 480 pixels
                if (target.active && distanceToplayer >= 15)
                {
                    float projspeed = 25;
                    Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y - 14) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                    //Set the velocities to the shoot values
                    Projectile.velocity.X = velocity.X;
                    Projectile.velocity.Y = velocity.Y;
                }
                //Main.NewText("please??????!" + distanceToplayer, 0, 204, 170);             
            }
            if (player.dead)
            {
                Projectile.Kill();
            }

            Projectile.spriteDirection = Projectile.direction;
            //Main.player[Projectile.owner].ChangeDir(Projectile.direction);
            //Main.player[Projectile.owner].heldProj = player.whoAmI;
          

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item1 with { Volume = 0.75f, Pitch = 0f, MaxInstances = 0, }, Projectile.position);

            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 1f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 1f;
            }
            return false;
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
             float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
             if (magnitude > 50f)
             {
                 vector *= 50f / magnitude;
             }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item1 with { Volume = 1f, Pitch = 0.5f, MaxInstances = 3, }, Projectile.position);

            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 90, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);
                dust.noGravity = true;
            }
            //target.AddBuff(ModContent.BuffType<WebDebuff>(), 15);
            target.GetGlobalNPC<NPCEffects>().arrowcooldown = 7;

            //Projectile.damage += 12;
        }

        public override void Kill(int timeLeft)
        {
            //Main.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 6);
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 90);
                dust.noGravity = true;
            }       
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            /*for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }*/

            return true;

        }    
    }
   
}
