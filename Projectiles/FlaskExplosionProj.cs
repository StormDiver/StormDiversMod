using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using StormDiversMod.Common;
using StormDiversMod.Buffs;
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles
{
    public class FlaskExplosionProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flask of Explosives");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.timeLeft = 4;
            Projectile.penetrate = 3;

            Projectile.tileCollide = true;
            Projectile.scale = 1f;
            Projectile.knockBack = 0;
           
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.alpha = 255;
            DrawOffsetX = 25;
            DrawOriginOffsetY = 25;
        }
        public override bool? CanDamage()
        {         
            return true;     
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<NPCEffects>().flaskexplosivetime > 0 || target.friendly) //Npcs immune to explosion when activating it
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
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft == 3)
            {
                Projectile.alpha = 0;

                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;

                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 60;
                Projectile.height = 60;
                Projectile.Center = Projectile.position;

                Projectile.knockBack = 4f;

                SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);

                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionCompactProj>(), 0, 0, Projectile.owner);
                Main.projectile[proj].scale = 1f;

                for (int i = 0; i < 20; i++) //Orange particles
                {
                    Vector2 perturbedSpeed = new Vector2(0, -3f).RotatedByRandom(MathHelper.ToRadians(360));

                    var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                    dust.noGravity = true;
                    dust.scale = 1.5f;

                }
                for (int i = 0; i < 20; i++) //Grey dust circle
                {
                    Vector2 perturbedSpeed = new Vector2(0, -3f).RotatedByRandom(MathHelper.ToRadians(360));
                    var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                    //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                    dust.noGravity = true;
                    dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                }
            }
        }
    
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
           
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
           
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            return false;
        }
        public override void OnKill(int timeLeft)
        {
           
        }
    }
}
