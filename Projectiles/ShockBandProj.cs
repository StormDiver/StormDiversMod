using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Common;

using Terraria.Utilities;
using StormDiversMod.Items.Accessory;
using System.Security.Policy;

namespace StormDiversMod.Projectiles
{
    public class ShockBandProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shock band Lightning");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.friendly = true;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;

            Projectile.timeLeft = 120;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 10;
            Projectile.ArmorPenetration = 20;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<NPCEffects>().shockbandtime > 0 || target.friendly) //Npcs immune to explosion when activating it
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void AI()
        {
            Projectile.localAI[1]++;

            if (Projectile.localAI[1] >= 6)
            {
                for (int i = 0; i < 3; i++)
                {
                    float speedY = -0.75f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 226, dustspeed.X, dustspeed.Y, 229, default, 1f);
                    Main.dust[dust2].noGravity = true;
                }
                Projectile.localAI[1] = 0;
            }
            Dust dust;
            dust = Terraria.Dust.NewDustPerfect(Projectile.Center, 226, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;


            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }
        int targetlimit;
        bool targethit;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 0.5f, Pitch = -0.1f, MaxInstances = 0 }, Projectile.Center);

            Projectile.ai[2]++;
            if (Projectile.ai[2] <= 3)// Hit 5 enemies
            {
                target.GetGlobalNPC<NPCEffects>().shockbandtime = 10;// makes sure the enemy that summons the proj can't get hit by it
                for (int i = 0; i < Main.maxNPCs; i++) //Shoots sand at one enemy
                {
                    NPC npctarget = Main.npc[i];

                    npctarget.TargetClosest(true);
                    if (Vector2.Distance(Projectile.Center, npctarget.Center) <= 700f && !npctarget.friendly && npctarget.active && !npctarget.dontTakeDamage && npctarget.lifeMax > 5 && npctarget.type != NPCID.TargetDummy
                        && Collision.CanHitLine(Projectile.Center, 0, 0, npctarget.Center, 0, 0) && npctarget.GetGlobalNPC<NPCEffects>().shockbandtime == 0 && !targethit)
                    {
                        targethit = true;
                        Vector2 velocity = Vector2.Normalize(new Vector2(npctarget.Center.X, npctarget.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * 10;

                        int ProjID = Projectile.NewProjectile(npctarget.GetSource_FromAI(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.ShockBandProj>(), Projectile.damage / 2, 0, Main.myPlayer);
                        Main.projectile[ProjID].ai[2] = Projectile.ai[2]; //adds on to this ai for limited amount of hits
                    }
                }
            }
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 229, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 100, default, 1.2f);
                dust.noGravity = true;
            }
            for (int i = 0; i < 20; i++)
            {
                Vector2 dustspeed = new Vector2(0, 2).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(Projectile.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 100, default, 1f);
                Main.dust[dust2].noGravity = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
