using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using StormDiversMod.Basefiles;

namespace StormDiversMod.NPCs.NPCProjs
{
  
    //______________________________________________________________________________________________________
    public class AridBossSandProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Husk Sand");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = 1;
            Projectile.light = 0.9f;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 70;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.knockBack = 0;
            Projectile.alpha = 255;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        { 
 
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            if (Projectile.timeLeft > 20 || Projectile.ai[1] == 0)
            {
                if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 138, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;

                    int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 10, 10, 162, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1.5f);
                    Main.dust[dust2].velocity *= 0.5f;
                }
            }
            if (Projectile.timeLeft == 20 && Projectile.ai[1] == 1) //Change to first attack           
            {
                Projectile.alpha = 0;

                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                Projectile.scale = 1f;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 100;
                Projectile.height = 100;
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
                Projectile.netUpdate = true;
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

    }
    //_______________________________________
    public class AridBossShardProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Husk Shard");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = true; 
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.light = 0.1f;

            DrawOffsetX = 0;
            DrawOriginOffsetY = -0;
            Projectile.light = 0.2f;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
            dust.scale = 0.5f;
            dust.velocity *= 0;
        }
            
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {         
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
                dust.scale = 0.5f;
            }
        }


        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
                dust.scale = 0.75f;
            }

        }
    }
    //___________________________________________________________________________
    public class AridBossFlameProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Husk Sand Flame");
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
            if (Projectile.alpha < 45)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int dustoffset;
        public override void AI()
        {
            Projectile.rotation += 0.1f;

            if (Main.rand.Next(10) == 0) //dust spawn sqaure increases with hurtbox size
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X - (dustoffset / 2), Projectile.position.Y - (dustoffset / 2)), Projectile.width + dustoffset, Projectile.height + dustoffset, 138, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 130, default, 1f);
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
                Projectile.alpha += 3;

                Projectile.velocity.X *= 0.98f;
                Projectile.velocity.Y *= 0.98f;
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
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
            hitbox.Width = dustoffset;
            hitbox.Height = dustoffset;
            hitbox.X -= dustoffset / 2 - (Projectile.width / 2);
            hitbox.Y -= dustoffset / 2 - (Projectile.height / 2);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {         
            target.AddBuff(BuffID.OnFire, 180);
        }
    }  
}
