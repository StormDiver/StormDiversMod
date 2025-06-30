using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles.SentryProjs
{
    
    
   
    //______________________________________________________________________________________________________
    public class DesertStaffProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Sentry");
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;   
        }
        public override bool? CanDamage()
        {
            return false;
        }
        int opacity = 255;
        bool animate = false;

        bool floatup = true;
        public override void AI()
        {
            if (opacity > 0)
            {
                opacity -= 10;
            }
            Projectile.alpha = opacity;
            //Projectile.rotation += (float)Projectile.direction * -0.1f;
            Main.player[Projectile.owner].UpdateMaxTurrets();
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            {
                if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 10, 0, 10, 130, default, 1f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 54, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
                }
            }
            Projectile.ai[1]++; //shoottime
            //Getting the npc to fire at
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if (Vector2.Distance(Projectile.Center, target.Center) <= 260f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.CanBeChasedBy() && target.type != NPCID.TargetDummy)
                {
                    if (Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                    {
                        if (Projectile.ai[1] > 40)
                        {
                            AnimateProjectile();

                            float numberProjectiles = 12;
                            float rotation = MathHelper.ToRadians(180);
                            //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                            for (int j = 0; j < numberProjectiles; j++) //visual effect
                            {
                                float speedX = 0f;
                                float speedY = 3.5f;
                                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<DesertStaffProj2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                            }

                            //Invisible expanding projectile deals damage
                            int projID2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0),
                                    ModContent.ProjectileType<DesertStaffProj3>(), Projectile.damage, .5f, Projectile.owner, 0);

                            SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);
                            animate = true;
                            Projectile.ai[1] = 0;
                        }
                    }
                }
            }
            if (animate)
            {
                AnimateProjectile();
            }
            if (floatup) //Floating upwards
            {
                Projectile.localAI[0]++; //Floattime
                if (Projectile.localAI[0] <= 30)
                {
                    Projectile.velocity.Y -= 0.01f;

                }
                else //Halfway through it slows down
                {
                    Projectile.velocity.Y += 0.01f;

                }
                if (Projectile.localAI[0] >= 60)
                {
                    floatup = false;
                    Projectile.localAI[0] = 0;
                }
            }
            if (!floatup) //Floating downwards
            {
                Projectile.localAI[0]++;

                if (Projectile.localAI[0] <= 30)
                {
                    Projectile.velocity.Y += 0.01f;
                }
                else //Halfway through it slows down
                {
                    Projectile.velocity.Y -= 0.01f;

                }
                if (Projectile.localAI[0] >= 60)
                {
                    floatup = true;
                    Projectile.localAI[0] = 0;
                }
            }
        }
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
                Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                //Projectile.frame %= 6; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame == 8)
            {

                Projectile.frame = 0;
                animate = false;
            }   
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 300);
        }
        public override void OnKill(int timeLeft)
        {

            //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 45);
            for (int i = 0; i < 50; i++)
            {

                 
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 54);
                dust2.noGravity = true;
            }
        }

    }
    //_____________________________________________________________________________________________________
    public class DesertStaffProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Sand");
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            if (Projectile.ai[0] > 5f)  //this defines where the flames starts
            {
                if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 10, Projectile.Center.Y - 10), 20, 20, 10, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.25f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 10, 10, 54, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            return;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;

            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 180);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 180);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
    //_____________________________________________________________________________________________________
    public class DesertStaffProj3 : ModProjectile
    { 
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Sentry Explosion");
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;
            //increases size and keeps it in place
            Projectile.width += 8;
            Projectile.height += 8;
            Projectile.position.X -= 4f;
            Projectile.position.Y -= 4f;
            Projectile.ai[0] += 8;
            //Projectile.scale += 0.1f;
            if (Projectile.ai[0] > 450)//kill when timer is reached,
            {
                Projectile.Kill();
            }
            var player = Main.player[Projectile.owner];
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Collision.CanHitLine(Projectile.Center, 0, 0, target.position, target.width, target.height) && !target.friendly)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AridSandDebuff>(), 180);       
            Projectile.damage = (Projectile.damage * 20) / 19;
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
