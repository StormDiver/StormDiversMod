using Microsoft.Xna.Framework;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using StormDiversMod.Items.Weapons;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace StormDiversMod.Projectiles
{
    public class RoseSickleExplosionProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cresent Rifle Explosion");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;
            Projectile.friendly = true;

            Projectile.timeLeft = 4;
            Projectile.penetrate = 1;

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
            if (target.GetGlobalNPC<NPCEffects>().aridimmunetime > 0 || target.friendly) //Npcs immune to explosion when activating it
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

                //Projectile.scale = 1.7f;
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;

                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 100;
                Projectile.height = 100;
                Projectile.Center = Projectile.position;
            }
            if (Projectile.timeLeft <= 20)
            {
                Projectile.knockBack = 6f;
            }
        }
    
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Venom, 300); //replace with custom debuff
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
            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 12);
                dust.scale = 1f;
                dust.velocity *= 2f;
                dust.fadeIn = 1f;
            }
            for (int i = 0; i < 20; i++)
            {
                float speedY = -1.5f;
                Vector2 perturbedSpeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 12, perturbedSpeed.X, perturbedSpeed.Y);
                dust.scale = 1f;
                dust.velocity *= 2f;
                dust.fadeIn = 1f;
            }
            SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionPainNofaceProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1f;
        }
    }
    //_________________________________________________________________________________________________
    public class RoseSickleBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rose Sickle Bullet");      
        }
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 90;
            AIType = ProjectileID.Bullet;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.extraUpdates = 3;
            Projectile.light = 0.2f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[2] == 1) //enhanced bullet
            {
                SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
                //Main.NewText("STRONK");
                Projectile.damage *= 3;
            }
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 3 && Projectile.ai[2] == 1)
            {
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 12, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                dust2.noGravity = true;
            }
            if (Projectile.ai[0] == 3 && Projectile.ai[2] == 1)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionPainNofaceProj>(), 0, 0);
                Main.projectile[proj].scale = 1f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[2] == 1)
            {
                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(target.Center, 0, 0, 12);
                    dust.scale = 1f;
                    dust.velocity *= 2f;
                    dust.fadeIn = 1f;
                }
                for (int i = 0; i < 20; i++)
                {
                    float speedY = -1.5f;
                    Vector2 perturbedSpeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    var dust = Dust.NewDustDirect(target.Center, 0, 0, 12, perturbedSpeed.X, perturbedSpeed.Y);
                    dust.scale = 1f;
                    dust.velocity *= 2f;
                    dust.fadeIn = 1f;
                }
                SoundEngine.PlaySound(SoundID.Item74, target.Center);
                int proj = Projectile.NewProjectile(target.GetSource_FromThis(), new Vector2(target.Center.X, target.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionPainNofaceProj>(), 0, 0);
                Main.projectile[proj].scale = 1f;
                //npc.AddBuff(ModContent.BuffType<RoseDebuff>(), Math.Min(600, Math.Max(0, rosehitcount * 30)));
                target.AddBuff(ModContent.BuffType<RoseDebuff>(), 600);
            }
            else
            {
                for (int i = 0; i < 30; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 12);
                    dust.noGravity = true;
                }
                Projectile.damage = (Projectile.damage * 9) / 10;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 12);
                dust2.noGravity = true;
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
