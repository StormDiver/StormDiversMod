using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Drawing;

namespace StormDiversMod.Projectiles
{
    public class ClaymanProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Clayman Stare");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 2;
            Projectile.light = 0.4f;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.aiStyle = -1;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
           if (Projectile.ai[0] == 1)
            {
                Projectile.position = Projectile.Center;

                Projectile.width = 25;
                Projectile.height = 25;
                Projectile.Center = Projectile.position;
            }
        }
       
        public override bool OnTileCollide(Vector2 oldVelocity)
        {         
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];

            if (Projectile.ai[0] == 0)
            {
                CombatText.NewText(new Rectangle((int)target.Center.X, (int)target.Center.Y, 12, 4), Color.PeachPuff, "Clayman!", false);

                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Excalibur, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(target.Center.X + Main.rand.Next(-target.width / 3, target.width / 3), target.Center.Y + Main.rand.Next(-target.height / 3, target.height / 3)),

                }, player.whoAmI);
            }
            else
                CombatText.NewText(new Rectangle((int)target.Center.X, (int)target.Center.Y, 12, 4), Color.PeachPuff, "Clayman!", true);

        }

        public override void OnKill(int timeLeft)
        {
            //int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionHellSoulProj>(), 0, 0, Projectile.owner);
            //Main.projectile[proj].scale = 1.75f;

            /*for (int i = 0; i < 50; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, -7.5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 173, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.scale = 2f;
                dust.velocity *= 2f;

            }*/
            
        }

    }
}