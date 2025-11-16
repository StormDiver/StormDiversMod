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
    //______________________________________________________________________________________________________
    public class AncientStaffProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ancient Sand Explosion");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = 1;
            Projectile.light = 0.9f;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
           
            Projectile.timeLeft = 40;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;
            Projectile.scale = 1f;
            Projectile.knockBack = 0;
            Projectile.alpha = 255;
            DrawOffsetX = 27;
            DrawOriginOffsetY = 27;

            Projectile.aiStyle = -1;
            //Projectile.usesIDStaticNPCImmunity = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        { 
            Projectile.velocity.X *= 0;
            Projectile.velocity.Y *= 0;
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            if (Projectile.timeLeft > 20)
            {
                if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 138, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1.5f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    //int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
                }
            }
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft == 20)
            {
                Projectile.alpha = 0;

                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                Projectile.scale = 1.5f;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 150;
                Projectile.height = 150;
                Projectile.Center = Projectile.position;
                SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);

                for (int i = 0; i < 25; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 138);
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                    dust.velocity *= 4f;
                    dust.fadeIn = 1f;

                }
                for (int i = 0; i < 50; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(0, -7.5f).RotatedByRandom(MathHelper.ToRadians(360));

                    var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 138, perturbedSpeed.X, perturbedSpeed.Y);
                    dust.noGravity = true;
                    dust.scale = 1.5f;

                }

            }
            if (Projectile.timeLeft <= 20)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
            }
        }
        public override bool? CanDamage()
        {
            if (Projectile.timeLeft > 17 && Projectile.timeLeft <= 20)
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
            //target.AddBuff(mod.BuffType("AridSandDebuff"), 180);
            target.AddBuff(BuffID.OnFire, 180);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(BuffID.OnFire, 180);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            return false;
        }
        public override void OnKill(int timeLeft)
        {                 

        }
    }
    //_______________________________________
    public class AncientKnivesProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ancient Throwing Knife");

        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = 0;


            Projectile.friendly = true;
            Projectile.timeLeft = 35;
            Projectile.penetrate = 1;

            Projectile.tileCollide = true;


            Projectile.DamageType = DamageClass.Melee;
            Projectile.light = 0.1f;

            DrawOffsetX = 0;
            DrawOriginOffsetY = -0;

            Projectile.light = 0.2f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        int spinspeed = 0;
        public override void AI()
        {

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (Projectile.timeLeft == 15)
            {
                Projectile.penetrate = 3;

            }
            if (Projectile.timeLeft <= 15)
            {
                spinspeed++;
                Projectile.rotation = (0.4f * spinspeed) * Projectile.direction;

                DrawOriginOffsetY = -8;

                Projectile.alpha += 17;
                Projectile.light -= 0.01f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
                dust.scale = 0.5f;
            }
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);

            Projectile.damage = (Projectile.damage * 9 / 10);
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
                dust.scale = 0.5f;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(BuffID.OnFire, 180);

                for (int i = 0; i < 5; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
                    dust.scale = 0.5f;
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
                      
        }
    }
    //___________________________________________________________________________
    public class AncientFlameProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ancient Sand Stream");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 0.1f;
            DrawOffsetX = -35;
            DrawOriginOffsetY = -35;
            Projectile.light = 0.8f;
            Projectile.ArmorPenetration = 5;
        }
        
        public override bool? CanDamage()
        {
            if (Projectile.alpha < 100 && Projectile.ai[1] == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            //Projectile.rotation = Main.rand.NextFloat(0, 6.2f); //speen start
        }
        int dustoffset;
        int alphaadd; //add alpha to the trail
        int posadd = 5; //adjust trail position
        public override void AI()
        {
            Projectile.rotation += 0.05f * -Projectile.direction;
            if (dustoffset > 5)
            {
                if (Main.rand.Next(20) == 0) //dust spawn sqaure increases with hurtbox size
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X - (dustoffset / 2), Projectile.position.Y - (dustoffset / 2)), Projectile.width + dustoffset, Projectile.height + dustoffset, 138, Projectile.velocity.X * 1f, -5, 130, default, 0.8f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].fadeIn = 0.8f + (float)Main.rand.Next(5) * 0.1f;

                    //int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
                }
            }
            if (Projectile.scale <= 1f)//increase size until specified amount
            {
                dustoffset += 1;//makes dust expand with projectile, also used for hitbox

                Projectile.scale += 0.016f;
            }
            else
            {
                Projectile.alpha += 2;
                Projectile.velocity *= 0.96f;
                //begin animation
                if (Projectile.frame < 2) //stop at frame 3
                {
                    Projectile.frameCounter++;
                    if (Projectile.frameCounter >= 30)
                    {
                        Projectile.frame++;
                        Projectile.frameCounter = 0;
                    }
                }
            }
            if (Projectile.alpha > 200 || Projectile.wet)//once faded enough or touches water kill projectile
            {
                Projectile.Kill();
            }
            //Trail effect(it works don't judge)
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[2]++;

                if (Projectile.ai[2] % 6 == 0 && Projectile.ai[2] <= 24) //summon a trail projectile every X frames
                {
                    posadd += 5; //add X times velcity to position each time
                    Vector2 velocity = Projectile.velocity * posadd;

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(0);
                    alphaadd += 10; //Add alpha so trail is slighly darker
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X - perturbedSpeed.X, Projectile.Center.Y - perturbedSpeed.Y), Projectile.velocity, ModContent.ProjectileType<AncientFlameProj>(), 0, 0, Projectile.owner);
                    Main.projectile[projID].ai[1] = 1;
                    Main.projectile[projID].alpha += alphaadd;
                }

            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
            if (Projectile.ai[0] == 0) // only on proj deals damage
            {
                hitbox.Width = dustoffset;
                hitbox.Height = dustoffset;
                hitbox.X -= dustoffset / 2 - (Projectile.width / 2);
                hitbox.Y -= dustoffset / 2 - (Projectile.height / 2);
            }
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            
           target.AddBuff(BuffID.OnFire, 180);       
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(BuffID.OnFire, 180);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0;
            //Projectile.Kill();
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.Sienna;
            color.A = (Byte)Projectile.alpha;
            return color;
        }
    }
    //______________________________________________________________________________________________________
    public class AncientArmourProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ancient Sand Explosion");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = 1;
            Projectile.light = 0.1f;
            Projectile.friendly = true;

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

                Projectile.width = 150;
                Projectile.height = 150;
                Projectile.Center = Projectile.position;

                Projectile.knockBack = 6f;

                SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);

                for (int i = 0; i < 40; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 138);
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                    dust.velocity *= 4f;
                    dust.fadeIn = 1f;

                }
                for (int i = 0; i < 40; i++)
                {
                    float speedY = -1.5f;

                    Vector2 perturbedSpeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 55, perturbedSpeed.X, perturbedSpeed.Y);

                    dust.noGravity = true;
                    dust.scale = 1.5f;
                    dust.velocity *= 4f;
                    dust.fadeIn = 1f;
                }
                /*
                for (int i = 0; i < 50; i++)
                {
                    Dust dust;
                    Vector2 position = Projectile.position;
                    dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 55, 0f, 0f, 0, default, 1f)];
                    dust.noGravity = true;
                    dust.scale = 1f;
                }*/
            }
            if (Projectile.timeLeft <= 20)
            {
                Projectile.knockBack = 6f;
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
            }
        }
    
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //target.AddBuff(mod.BuffType("AridSandDebuff"), 180);
            target.AddBuff(BuffID.OnFire, 180);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(BuffID.OnFire, 180);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionAridProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1.6f;
        }
    }
}
