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
using static Terraria.GameContent.Bestiary.BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;

namespace StormDiversMod.Projectiles.AmmoProjs
{
    public class FrostBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Bullet");
            ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;

            Projectile.aiStyle = 1;
            Projectile.light = 0.2f;

            Projectile.friendly = true;
            Projectile.timeLeft = 9999;
            Projectile.penetrate = -1; //sometimes it can hit 2 at once for no reason

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Ranged;
            AIType = ProjectileID.Bullet;
        }
        public int TargetWhoAmI
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        int basedamage;
        bool sticktarget;
        int StickTimer;
        
        public override void OnSpawn(IEntitySource source)
        {
            basedamage = Projectile.damage;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            return true;
        }
        Vector2 projoffest;
        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            //Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            if (sticktarget)
            {
                Projectile.penetrate = 1;
                Projectile.alpha = 255;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                StickTimer += 1;
                for (int i = 0; i < 1; i++) //projectiles when embedded
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
                    dust.velocity *= 0.5f;
                }
                int npcTarget = TargetWhoAmI;
                if (StickTimer >= 180 || npcTarget < 0 || npcTarget >= 200) //after 3 seconds "explode"
                {
                    Projectile.timeLeft = 1;
                    Projectile.damage = (int)(basedamage * 0.3f); //second hit 30% damage
                    Projectile.ArmorPenetration = 10; //10 AP
                    for (int i = 0; i < 15; i++)
                    {
                        var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
                        dust.scale *= 2f;
                        dust.velocity *= 2f;
                        dust.noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
                    //Projectile.Kill();
                }
                else if (Main.npc[npcTarget].active && !Main.npc[npcTarget].dontTakeDamage)
                {
                    // If the target is active and can take damage
                    // Set the projectile's position relative to the target's center
                    Projectile.Center = Main.npc[npcTarget].Center + projoffest - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.npc[npcTarget].gfxOffY;
                }
                else
                { // Otherwise, kill the projectile
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.damage = (int)(basedamage * 0.85f); //first hit deals 85% damage
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            if (!sticktarget)//attach to enemy
            {
                Projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore
                projoffest = new Vector2(Main.rand.NextFloat(-target.width / 6, target.width / 6), Main.rand.NextFloat(-target.height / 6, target.height / 6));
                Projectile.extraUpdates = 0;
                TargetWhoAmI = target.whoAmI; // Set the target whoAmI
                Projectile.velocity = (target.Center - Projectile.Center) * 0.75f; // Change velocity based on delta center of targets (difference between entity centers)
                Projectile.netUpdate = true; // netUpdate this javelin
                sticktarget = true; // we are sticking to a target
                Projectile.knockBack /= 2;
            }
            else if (sticktarget && Projectile.timeLeft == 1)
                target.AddBuff(BuffID.Frostburn2, 180);
        }
        public override bool? CanDamage()
        {
            if (!sticktarget || (sticktarget && Projectile.timeLeft <= 1))
                return true;
            else
                return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // By shrinking target hitboxes by a small amount, this projectile only hits if it more directly hits the target.
            // This helps the javelin stick in a visually appealing place within the target sprite.
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            // Return if the hitboxes intersects, which means the javelin collides or not
            return projHitbox.Intersects(targetHitbox);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (!sticktarget)
                return Color.White;
            else
                return null;
        }
    }
    //_____________________________________________________________________________________________________________________________________________________

    public class FrostArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Arrow");
            ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.aiStyle = 1;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.arrow = true;
            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.arrow = true;
            AIType = ProjectileID.WoodenArrowFriendly;
            //Creates no immunity frames
            DrawOffsetX = -4;
            DrawOriginOffsetY = 0;
        }
        public override void AI()
        {  
               /* int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 187, 0f, 0f, 100, default, 0.7f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;*/
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 92, 0f, 0f, 0, new Color(255, 255, 255), 0.7f)];
                dust.noGravity = true;
            }

            int numberProjectiles = 2; //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {
                float speedX = -Projectile.velocity.X;
                float speedY = -Projectile.velocity.Y;
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20)) * 1f;
                float scale = 1f - (Main.rand.NextFloat() * .5f);
                perturbedSpeed = perturbedSpeed * scale;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ProjectileID.CrystalShard, (int)(Projectile.damage * 0.25f), 0, Projectile.owner);
            }
        }  
    }
}
