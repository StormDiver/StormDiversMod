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
    public class StoneSolar : ModProjectile
    {
      public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Solar Stone Boulder");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.light = 0.8f;
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
         
            //DrawOffsetX = -5;
            //DrawOriginOffsetY = -5;

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
            target.AddBuff(ModContent.BuffType<LunarBoulderDebuff>(), 1000);
           
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 244);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(ModContent.BuffType<LunarBoulderDebuff>(), 500);
                for (int i = 0; i < 10; i++)
                { 
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 244);
                }
            }
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
                    Projectile.velocity.X = -oldVelocity.X * 1f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 1f;
                }
            }
            if (reflect >= 1)
            {

                int numberProjectiles = 20 + Main.rand.Next(5); //This defines how many projectiles to shot.
                for (int i = 0; i < numberProjectiles; i++)
                {

                    float speedX =  Main.rand.NextFloat(-12f, 12f);
                    float speedY =  Main.rand.NextFloat(-12f, 12f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragCelestial>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner, 0, 0);

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

                     
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 244);
                }

            }

          int numberProjectiles = 20 + Main.rand.Next(5); //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {

                float speedX =  Main.rand.NextFloat(-13f, 13f);
                float speedY =  Main.rand.NextFloat(-13f, 13f);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragCelestial>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner,0 , 0);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
    }
    //______________________________________________________________________________________
    public class StoneVortex : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vortex Stone Boulder");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.light = 0.8f;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            AIType = ProjectileID.Bullet;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        
            //DrawOffsetX = -5;
            //DrawOriginOffsetY = -5;

        }

        public override void AI()
        {
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 110, 0f, 0f, 0, new Color(255, 255, 255), 1.2f);
            dust.noGravity = true;
            dust.fadeIn = 1;
            Projectile.rotation += (float)Projectile.direction * -0.2f;
            Projectile.width = 26;
            Projectile.height = 26;
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<LunarBoulderDebuff>(), 1000);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 110);
            }
            int numberProjectiles = 5 + Main.rand.Next(3); //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {

                float speedX = Main.rand.NextFloat(-6f, 6f);
                float speedY = Main.rand.NextFloat(-6f, 6f);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragCelestial>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner, 0, 1);

            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(ModContent.BuffType<LunarBoulderDebuff>(), 500);
                for (int i = 0; i < 10; i++)
                {


                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 110);
                }
            }
        }

         int reflect = 5;

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
                    Projectile.velocity.X = -oldVelocity.X * 1f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 1f;
                }
            }
            if (reflect >= 1)
            {
            int numberProjectiles = 7 + Main.rand.Next(3); //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {

                    float speedX = Main.rand.NextFloat(-8f, 8f);
                    float speedY = Main.rand.NextFloat(-8f, 8f);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragCelestial>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner, 0, 1);

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

                     
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 110);
                }

            }

             int numberProjectiles = 10 + Main.rand.Next(3); //This defines how many projectiles to shot.
             for (int i = 0; i < numberProjectiles; i++)
             {

                float speedX = Main.rand.NextFloat(-10f, 10f);
                float speedY = Main.rand.NextFloat(-10f, 10f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragCelestial>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner, 0, 1);

            }


        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
    }
    
    //_________________________________________________________________
    public class StoneNebula : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nebula Stone Boulder");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.light = 0.8f;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        
            //DrawOffsetX = -5;
            //DrawOriginOffsetY = -5;

        }
       
        public override void AI()
        {
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 112, 0f, 0f, 0, new Color(255, 255, 255), 1.2f);
            dust.noGravity = true;
            dust.fadeIn = 1;
            Projectile.rotation += (float)Projectile.direction * -0.2f;

            Projectile.width = 26;
            Projectile.height = 26;


        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<LunarBoulderDebuff>(), 1000);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 112);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(ModContent.BuffType<LunarBoulderDebuff>(), 500);
                for (int i = 0; i < 10; i++)
                {


                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 112);
                }
            }
        }

        int reflect = 3;

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
                    Projectile.velocity.X = -oldVelocity.X * 0.8f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
                }
            }
            if (reflect >= 1)
            {
                int numberProjectiles = 3 + Main.rand.Next(2); //This defines how many projectiles to shot.
                for (int i = 0; i < numberProjectiles; i++)
                {

                    float speedX = Main.rand.NextFloat(-7f, 7f);
                    float speedY = Main.rand.NextFloat(-7f, 7f);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragCelestial>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner, 0, 2);

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

                     
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 112);
                }

            }

            int numberProjectiles = 5 + Main.rand.Next(2); //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {

                float speedX = Main.rand.NextFloat(-9f, 9f);
                float speedY = Main.rand.NextFloat(-9f, 9f);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragCelestial>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner, 0, 2);
            }


        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
    }
 
    //__________________________________________________________________________________________________
    public class StoneStardust : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stardust Stone Boulder");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.light = 0.8f;
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
           
            //DrawOffsetX = -5;
            //DrawOriginOffsetY = -5;

        }
       
        public override void AI()
        {
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 111, 0f, 0f, 0, new Color(255, 255, 255), 1.2f);
            dust.noGravity = true;
            dust.fadeIn = 1;
            Projectile.rotation += (float)Projectile.direction * -0.2f;

            if (Main.rand.Next(12) == 0)
            {
                int numberProjectiles = 1 + Main.rand.Next(2); //This defines how many projectiles to shot.
                for (int j = 0; j < numberProjectiles; j++)
                {

                    float speedX = Main.rand.NextFloat(-3f, 3f);
                    float speedY = Main.rand.NextFloat(-3f, 3f);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragCelestial>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner, 0, 3);


                }
            }

            Projectile.width = 28;
            Projectile.height = 28;

        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<LunarBoulderDebuff>(), 1000);
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 111);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(ModContent.BuffType<LunarBoulderDebuff>(), 500);
                for (int i = 0; i < 10; i++)
                {


                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 111);
                }
            }
        }

        int reflect = 3;

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
                    Projectile.velocity.X = -oldVelocity.X * 0.8f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
                }
            }
            if (reflect >= 1)
            {
                int numberProjectiles = 3 + Main.rand.Next(2); //This defines how many projectiles to shot.
                for (int i = 0; i < numberProjectiles; i++)
                {

                    float speedX = Main.rand.NextFloat(-7f, 7f);
                    float speedY = Main.rand.NextFloat(-7f, 7f);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragCelestial>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner, 0, 3);
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

                     
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 111);
                }

            }

            int numberProjectiles = 4 + Main.rand.Next(2); //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {

                float speedX = Main.rand.NextFloat(-9f, 9f);
                float speedY = Main.rand.NextFloat(-9f, 9f);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ModContent.ProjectileType<StoneFragCelestial>(), (int)(Projectile.damage * 0.33), 0, Projectile.owner, 0, 3);
            }


        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
    }
 //_________________________________________________________________________________
    public class StoneFragCelestial : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Celestial Fragment");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.light = 0.2f;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 14;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
            Projectile.ArmorPenetration = 50;

        }
        int dusttype;
        public override void AI()
        {
            if (Projectile.ai[1] == 0)//solar
            {
                Projectile.frame = 0;
                dusttype = 244;
            }
            else if (Projectile.ai[1] == 1) //vortex
            {
                Projectile.frame = 1;
                dusttype = 110;
            }
            else if (Projectile.ai[1] == 2) //nebula
            {
                Projectile.frame = 2;
                dusttype = 112;
            }
            else if (Projectile.ai[1] == 3) //Stardust
            {
                Projectile.frame = 3;
                dusttype = 111;
            }
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.Center;
            dust = Terraria.Dust.NewDustPerfect(position, dusttype, new Vector2(0f, 0f), 0, new Color(255, 100, 0), 1f);
            dust.noGravity = true;
            Projectile.rotation += (float)Projectile.direction * -0.2f;

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<LunarBoulderDebuff>(), 500);
            for (int i = 0; i < 3; i++)
            {


                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dusttype);
            }
            Projectile.damage = (Projectile.damage * 9) / 10;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(ModContent.BuffType<LunarBoulderDebuff>(), 240);
                for (int i = 0; i < 3; i++)
                {


                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dusttype);
                }
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
                SoundEngine.PlaySound(SoundID.Tink, Projectile.Center);

                for (int i = 0; i < 3; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dusttype);
                }

            }
        }
        public override bool PreDraw(ref Color lightColor) //trail
        {
            Main.instance.LoadProjectile(Projectile.type);
            //Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/BetsyFlameProj_Trail");
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                    color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }

            return true;
        }
        public override void PostDraw(Color lightColor) //glowmask for animated / more frames)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/StoneFragCelestial_Glow");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
    }
    //______________________________________________________________________________________
}
