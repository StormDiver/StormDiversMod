using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles
{
    public class StoneProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stone Boulder");
        }
        public override void SetDefaults()
        {

            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14; 
            AIType = ProjectileID.WoodenArrowFriendly;
           
        }

        public override void AI()
        {
            if (Main.rand.NextFloat() < 1f)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 1, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
            
        {
            Projectile.Kill();
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 1);
                }

            }
            {

                int numberProjectiles = 2 + Main.rand.Next(2); //This defines how many projectiles to shot.
                for (int i = 0; i < numberProjectiles; i++)
                {

                    float speedX = Main.rand.NextFloat(-4f, 4f);
                    float speedY = Main.rand.NextFloat(-4f, 4f);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragProj>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner);
                }
            }
        }
    }
    //________________________________________________________________________
    public class StoneHardProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hard Stone Boulder");
        }
        public override void SetDefaults()
        {

            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.scale = 0.75f;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            //drawOffsetX = -5;
            //drawOriginOffsetY = -5;
        }

        public override void AI()
        {
            if (Main.rand.NextFloat() < 1f)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 1, 0f, 0f, 0, new Color(255, 255, 255), 1.2f)];
                dust.noGravity = true;
            }

            Projectile.width = 28;
            Projectile.height = 28;
        }




        int reflect = 2;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();

            }

            {
                Collision.HitTiles(Projectile.Center + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.7f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.7f;
                }
            }
            if (reflect >= 1)
            {
                int numberProjectiles = 4 + Main.rand.Next(2); //This defines how many projectiles to shot.
                for (int i = 0; i < numberProjectiles; i++)
                {

                    float speedX = Main.rand.NextFloat(-5f, 5f);
                    float speedY = Main.rand.NextFloat(-5f, 5f);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragProj>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner);

                }
            }
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 1);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                for (int i = 0; i < 10; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 1);
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 1);
                }

            }

            int numberProjectiles = 6 + Main.rand.Next(2); //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {

                float speedX = Main.rand.NextFloat(-6f, 6f);
                float speedY = Main.rand.NextFloat(-6f, 6f);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragProj>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner);

            }

        }
    }
    //_____________________________________________________________________________________________________
    public class StoneSuperProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flaming Stone Boulder");
        }
        public override void SetDefaults()
        {
            Projectile.light = 0.8f;
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 180;
            //drawOffsetX = -5;
            //drawOriginOffsetY = -5;

        }
       
        public override void AI()
        {
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 174, 0f, 0f, 0, new Color(255, 255, 255), 1.2f);
            dust.noGravity = true;
            dust.fadeIn = 1;


            Projectile.rotation += (float)Projectile.direction * -0.2f;

            Projectile.width = 28;
            Projectile.height = 28;
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 600);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 55);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
            target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 800);
                for (int i = 0; i < 10; i++)
                {


                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 55);

                }
            }
        }

        int reflect = 2;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();

            }

            {
                Collision.HitTiles(Projectile.Center + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.9f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
                }
            }
            if (reflect >= 1)
            {
                int numberProjectiles = 3 + Main.rand.Next(2); //This defines how many projectiles to shot.
                for (int i = 0; i < numberProjectiles; i++)
                {

                    float speedX = Main.rand.NextFloat(-7f, 7f);
                    float speedY = Main.rand.NextFloat(-7f, 7f);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragSuperProj>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner);

                }
            }
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 55);
                }

            }

            int numberProjectiles = 3 + Main.rand.Next(2); //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {

                float speedX = Main.rand.NextFloat(-8f, 8f);
                float speedY = Main.rand.NextFloat(-8f, 8f);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragSuperProj>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner);
            }


        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //______________________________________________________________________________________
    public class StoneFragProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stone Fragment");
        }
        public override void SetDefaults()
        {

            Projectile.width = 9;
            Projectile.height = 9;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
            Projectile.ArmorPenetration = 10;

        }

        public override void AI()
        {
            if (Main.rand.NextFloat() < 1f)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 1, 0f, 0f, 0, new Color(255, 255, 255), 0.8f)];
                dust.noGravity = true;
            }
            Projectile.rotation += (float)Projectile.direction * -0.2f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Tink, Projectile.Center);

                for (int i = 0; i < 5; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 1);
                }

            }
        }
    }
    //_________________________________________________________________________________________________
    public class StoneFragSuperProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flaming Fragment");
        }
        public override void SetDefaults()
        {
            Projectile.light = 0.2f;
            Projectile.width = 9;
            Projectile.height = 9;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 180;
            DrawOffsetX = -3;
            DrawOriginOffsetY = -3;

            Projectile.ArmorPenetration = 30;

        }

        public override void AI()
        {
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.Center;
            dust = Terraria.Dust.NewDustPerfect(position, 174, new Vector2(0f, 0f), 0, new Color(255, 100, 0), 1f);
            dust.noGravity = true;
            Projectile.rotation += (float)Projectile.direction * -0.2f;


        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 180);
            Projectile.damage = (Projectile.damage * 9) / 10;

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 400);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Tink, Projectile.Center);

                for (int i = 0; i < 3; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 55);
                }

            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
