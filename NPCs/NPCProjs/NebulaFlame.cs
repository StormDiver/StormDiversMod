using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StormDiversMod.NPCs.NPCProjs      
{
    public class NebulaFlame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nebula Flame");
            Main.projFrames[Projectile.type] = 4;

        }
        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.extraUpdates = 2;
            Projectile.scale = 0.1f;
            DrawOffsetX = -35;
            DrawOriginOffsetY = -35;
            Projectile.light = 0.8f;
        }

        public override bool? CanDamage()
        {
            if (Projectile.alpha < 45 && Projectile.ai[1] == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int dustoffset;
        int alphaadd; //add alpha to the trail
        int posadd; //adjust trail position
        public override void AI()
        {
            Projectile.rotation += 0.1f;

            if (Main.rand.Next(10) == 0) //dust spawn sqaure increases with hurtbox size
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X - (dustoffset / 2), Projectile.position.Y - (dustoffset / 2)), Projectile.width + dustoffset, Projectile.height + dustoffset, 27, Projectile.velocity.X * 1f, -5, 130, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
                //int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 0.5f);
            }

            if (Projectile.scale <= 1f)//increase size until specified amount
            {
                dustoffset++;//makes dust expand with projectile, also used for hitbox

                Projectile.scale += 0.012f;
            }
            else//once the size has been reached begin to fade out and slow down
            {
                Projectile.alpha += 5;

                Projectile.velocity.X *= 0.99f;
                Projectile.velocity.Y *= 0.99f;
                //begin animation
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 10) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
            }
            if (Projectile.alpha > 150 || Projectile.wet)//once faded enough or touches water kill projectile
            {
                Projectile.Kill();
            }

            //Trail effect(it works don't judge)
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[2]++;

                if (Projectile.ai[2] % 5 == 0 && Projectile.ai[2] <= 40) //summon a trail projectile every 5 frames
                {
                    posadd += 3; //add 3 times velcity to position each time
                    Vector2 velocity = Projectile.velocity * posadd;

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(0);
                    alphaadd += 5; //Add alpha so it fades out at the same time
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X - perturbedSpeed.X, Projectile.Center.Y - perturbedSpeed.Y), Projectile.velocity, ModContent.ProjectileType<NebulaFlame>(), 0, 0, Projectile.owner);
                    Main.projectile[projID].ai[1] = 1;
                    Main.projectile[projID].alpha += alphaadd;
                }
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
            hitbox.Width = dustoffset;
            hitbox.Height = dustoffset;
            hitbox.X -= dustoffset / 2 - (Projectile.width / 2);
            hitbox.Y -= dustoffset / 2 - (Projectile.height / 2);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            //target.AddBuff(BuffID.OnFire, 180);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.HotPink;
            color.A = (Byte)Projectile.alpha;
            return color;
        }
    }
    /*public class NebulaFlame2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nebula Flame");
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            if (Main.rand.Next(3) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, 27, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 2f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= -0.3f;
                int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1.5f);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity *= -1f;
            }
           
            return;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
                        
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }*/
}