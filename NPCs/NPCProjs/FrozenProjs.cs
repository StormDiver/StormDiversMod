using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
using Terraria.Audio;



namespace StormDiversMod.NPCs.NPCProjs
{

    
    public class FrozenEyeProj: ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Snowflake");
        }
        public override void SetDefaults()
        {

            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
           
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            //drawOffsetX = -2;
            //drawOriginOffsetY = -2;
        }
        public override void AI()
        {
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 0.7f);
            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex].noGravity = true;

            Projectile.ai[0]++;
            Projectile.rotation = Projectile.ai[0] *= 3;

            if (Projectile.ai[0] == 1)
            {
                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
                    dust.velocity *= 2;
                }
            }
        }

       
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 180);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void Kill(int timeLeft)
        {
           
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);

                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
                dust.velocity *= 2;
                }

            
        }
    }
    //___________________________________________________________________________________________________________________________________
    public class FrozenSoulProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Snowflake");
        }
        public override void SetDefaults()
        {

            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 45 + +Main.rand.Next(30);
            Projectile.aiStyle = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            //drawOffsetX = -2;
            //drawOriginOffsetY = -2;
        }
        public override void AI()
        {
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 0.7f);
            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex].noGravity = true;

            Projectile.ai[0]++;
            Projectile.rotation = Projectile.ai[0] / 10;

            if (Projectile.ai[0] == 1)
            {
                for (int i = 0; i < 15; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
                    dust.velocity *= 2;
                }
            }
        }


        public override void OnHitPvp(Player target, int damage, bool crit)

        {
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.SuperFrostBurn>(), 180);
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            return false;
        }
        public override void Kill(int timeLeft)
        {

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27, 1, -0.5f);
                //SoundEngine.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 56, 0.5f);


                float numberProjectiles = 6;
                float rotation = MathHelper.ToRadians(180);
                for (int j = 0; j < numberProjectiles; j++)
                {
                  
                    Vector2 perturbedSpeed = new Vector2(0, 6).RotatedBy(MathHelper.Lerp(-rotation -Main.rand.Next(10), rotation + Main.rand.Next(10), j / (numberProjectiles)));
                    int projID = Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<FrozenEyeProj>(), Projectile.damage, Projectile.knockBack);
                    Main.projectile[projID].tileCollide = false;
                    Main.projectile[projID].timeLeft = 60;


                }
            }
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135);
                dust.velocity *= 2;
            }


        }
    }
    //___________________________________________________________________________________________________________________________________
    //___________________________________________________________________________________________________________________________________

}