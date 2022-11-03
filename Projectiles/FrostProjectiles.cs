using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

using System.Linq;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;


namespace StormDiversMod.Projectiles
{

    public class FrostGrenadeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Grenade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;


            Projectile.light = 0.1f;
            Projectile.friendly = true;

            Projectile.aiStyle = 2;

            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.timeLeft = 200;

        }
       
       
        public override void AI()
        {

            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 100;
                Projectile.height = 100;
                Projectile.Center = Projectile.position;

                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                Projectile.knockBack = 3f;
                
            }
            else
            {
                
                if (Main.rand.NextBool())
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                    
                    dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                   
                }
            }
            }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            

            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(1) == 0) // the chance
            {
               
                    target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);

                

            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionFrostProj>(), 0, 0, Projectile.owner);
            //Main.projectile[proj].scale = 1;

            /*for (int i = 0; i < 50; i++) //Frost particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -6f).RotatedByRandom(MathHelper.ToRadians(360));

                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 156, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 2f);
                Main.dust[dustIndex].noGravity = true;


            }*/
            for (int i = 0; i < 30; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, -7f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 156, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;

                dust.scale = 1.5f;
                dust.fadeIn = 1.5f;

            }
            for (int i = 0; i < 20; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -2.5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 2f;
                dust.velocity *= 2f;

            }
          

        }

    }
    //___________________________________________________________________________________________________________________________________
    public class FrostSpinProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen Polestar");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {

            Projectile.width = 158;
            Projectile.height = 158;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.timeLeft = 9999999;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(1) == 0) // the chance
            {

                target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);

            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);
        }
        // float hitbox = 150;
        // bool hitboxup;
        // bool hitboxdown;
        public override void AI()
        {
            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item7, Projectile.Center);
                Projectile.soundDelay = 60;
            }

            Player player = Main.player[Projectile.owner];
            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage); //update damage

            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.dead)
                {
                    Projectile.Kill();
                }
            }
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, 0f, 0.5f, 0.5f);
            }
            if (Projectile.owner == Main.myPlayer)
            {
                if (Main.MouseWorld.X >= player.Center.X)
                {
                    Projectile.velocity.X = 1;
                    player.direction = 1;
                }
                else if (Main.MouseWorld.X < player.Center.X)
                {
                    Projectile.velocity.X = -1;
                    player.direction = -1;

                }
            }

            Projectile.Center = player.MountedCenter;
            Projectile.position.X += player.width / 2 * player.direction;
            
            //Projectile.spriteDirection = player.direction;
            
            Projectile.rotation += 0.15f * player.direction; //this is the projectile rotation/spinning speed
           
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = Projectile.rotation;


            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135);  //this is the dust that this projectile will spawn
            Main.dust[dust].velocity /= 1f;
           
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/FrostSpinProj");
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            var player = Main.player[Projectile.owner];

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
        }

    }
    //___________________________________________________________________________________________________________________________________
    public class FrostStarProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Frizbee");
        }
        public override void SetDefaults()
        {

            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
           Projectile.CloneDefaults(272);
           AIType = 272;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -6;
            DrawOriginOffsetY = -6;
        }

        
        public override void AI()
        {
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 0.7f);
            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex].noGravity = true;


            Projectile.rotation += (float)Projectile.direction * -0.6f;


            Projectile.tileCollide = true;

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {

                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 92, 0f, 0f, 0, new Color(255, 255, 255), 0.7f)];


            }
            {
                var player = Main.player[Projectile.owner];
                if (Main.rand.Next(1) == 0) // the chance
                {
                   
                        target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);

                    

                }
            }
            for (int i = 0; i < 3; i++)
            {

                float speedX = Main.rand.NextFloat(-5f, 5f);
                float speedY = Main.rand.NextFloat(-5f, 5f);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ProjectileID.CrystalShard, (int)(Projectile.damage * 0.33f), 0, Projectile.owner);
            }
            Projectile.Kill();

        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            
                {
                    target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);

                }
        
            
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {

                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 92, 0f, 0f, 0, new Color(255, 255, 255), 0.7f)];


            }
            for (int i = 0; i < 3; i++)
            {

                float speedX = Main.rand.NextFloat(-3f, 3f);
                float speedY = Main.rand.NextFloat(-3f, 3f);

                int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + speedX, Projectile.Center.Y + speedY), new Vector2(speedX, speedY), ProjectileID.CrystalShard, (int)(Projectile.damage * 0.33f), 0, Projectile.owner);
                Main.projectile[projID].DamageType = DamageClass.Melee;
            }
            Projectile.Kill();
            return false;
        }
       
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);

               

            }



        }
    }
    //___________________________________________________________________________________________________________________________________
    public class Frostthrowerproj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost");
            Main.projFrames[Projectile.type] = 4;

            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {

            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 125;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 0.1f;
            DrawOffsetX = -35;
            DrawOriginOffsetY = -35;
            Projectile.light = 0.9f;
            Projectile.ArmorPenetration = 10;
        }
        public override bool? CanDamage()
        {
            if (Projectile.ai[1] == 0 && Projectile.alpha < 30) // if dust is faded don't deal damage
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
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X - (dustoffset / 2), Projectile.position.Y - (dustoffset / 2)), Projectile.width + dustoffset, Projectile.height + dustoffset, 135, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 2f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; 
                    Main.dust[dust].velocity *= 2.5f;
                    //int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f); //this defines the flames dust and color parcticles, like when they fall thru ground, change DustID to wat dust you want from Terraria
                }
           
            if (Projectile.scale <= 1f)//increase size until specified amount
            {
                dustoffset++; //makes dust expand with projectile, also used for hitbox

                Projectile.scale += 0.01f;
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(1) == 0) // the chance
            {              
                    target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);
              
            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0;
            //Projectile.Kill();
            return false;
        }
        /*public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/Frostthrowerproj_Trial");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(-40, 0);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }*/
    }
    //___________________________________________________________________________________________________________________________________
    public class FrostAccessProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Fragment");
        }
        public override void SetDefaults()
        {

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
           
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 14;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }

        public override void AI()
        {
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 0.7f);
            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
            Main.dust[dustIndex].noGravity = true;


            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 180);


        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 180);
        }


        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            Projectile.Kill();
            return true;
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width = 10, Projectile.height = 10, 135);
                }

            }
        }
    }
    //___________________________
    public class FrostCryoArmourProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cryo Cloud");
        }
        public override void SetDefaults()
        {

            Projectile.width = 100;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;

            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }
        public override bool? CanDamage()
        {

            return false;
        }
        public override void AI()
        {
            if (Main.rand.Next(5) == 0)
            {
                float xpos = (Main.rand.NextFloat(-50, 50));
                //float ypos = (Main.rand.NextFloat(200, 250));

                int projID = Projectile.NewProjectile(null, new Vector2(Projectile.Center.X - xpos, Projectile.Center.Y), new Vector2(xpos * -0.05f, 10), ModContent.ProjectileType<FrostAccessProj>(), Projectile.damage, 0, Projectile.owner);

                Main.projectile[projID].DamageType = DamageClass.Summon;
                Main.projectile[projID].aiStyle = 0;
                Main.projectile[projID].timeLeft = 60;
                Main.projectile[projID].penetrate = 1;

                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X - xpos, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionFrostProj>(), 0, 0, Projectile.owner);
                Main.projectile[proj].scale = 0.5f;

            }
            for (int i = 0; i < 2; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, -15f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 135, perturbedSpeed.X, perturbedSpeed.Y);
                dust.scale = 1.5f;
                dust.velocity *= 2f;
                dust.noGravity = true;
            }
            for (int i = 0; i < 1; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.Center.Y), Projectile.width, 0, 180, 0 , 10);
                dust.scale = 1.5f;
                dust.noGravity = true;
            }
            for (int i = 0; i < 1; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.Center.Y), Projectile.width, 0, 180, 0, -5);
                dust.scale = 1.5f;
                dust.noGravity = true;
            }
            Player player = Main.player[Projectile.owner];
            if (player.dead)
            {
                Projectile.Kill();
            }
        }

  

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            return false;
        }
        public override void Kill(int timeLeft)
        {
          
        }
    }

}