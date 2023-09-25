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
using Terraria.DataStructures;
using StormDiversMod.Projectiles.SentryProjs;
using Terraria.GameContent.Drawing;

namespace StormDiversMod.Projectiles
{

    public class TheSickleProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nine Lives Sickle Spinner");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 86;
            Projectile.height = 86;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 9999999;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item71 with { Volume = 1f, Pitch = 0.5f }, Projectile.Center);

        }
        public override void AI()
        {
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                Projectile.soundDelay = 25;
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
            Projectile.position.Y += player.height / 10;

            Projectile.spriteDirection = player.direction;

            Projectile.rotation += 0.15f * player.direction; //this is the projectile rotation/spinning speed

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = Projectile.rotation;
           
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
            hitbox.Width = 100;
            hitbox.Height = 100;
            hitbox.X -= (hitbox.Width - Projectile.width) / 2;
            hitbox.Y -= (hitbox.Height - Projectile.height) / 2;
            base.ModifyDamageHitbox(ref hitbox);
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
                 Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
             }

            return true;
        }
    }
    //_________________________________________________________________________________________________________________
    public class TheSickleProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nine Lives Sickle Thrown");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            Projectile.CloneDefaults(106);
            AIType = 106;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
            Projectile.penetrate = -1;
        }

        bool stillspin;
        int stilltime;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[2]++;
            //Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage); //update damage

            if (Projectile.velocity.X >= 0)
            {
                Projectile.spriteDirection = 1;
            }
            else
            {
                Projectile.spriteDirection = -1;
            }                      
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.light = 0;
            Projectile.penetrate = -1;
            if (stillspin)//while active projectile does not move
            {
                Projectile.velocity.Y *= 0f;
                Projectile.velocity.X *= 0f;
                stilltime++; //timer counts up while not moving
            }
            if (stilltime >= 15) //once this time is reached it cannot stay still again
            {
                stillspin = false;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item71 with { Volume = 1f, Pitch = 0.5f }, Projectile.Center);

            if (stilltime < 15) //So the projectile doesn't stay still again
            {
                stillspin = true;
            }         
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                dust.scale = 0.75f;
                dust.noGravity = true;
            }
            if (Projectile.ai[2] < 2)
            {
                return false;
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(-2.5f, -2.5f);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
    }
    //_________________________________________________________________________________________________________________
    public class TheSickleProj3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nine Lives Soul");
            //Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.knockBack = 0;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 99999999;
            Projectile.tileCollide = false;
            Projectile.MaxUpdates = 1;
        }
        float degrees;
        bool scaleup;
        //int shoottime;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        bool escaped;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.ai[0] == 1)
            {
                /*for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                    dust.noGravity = true;
                    dust.scale = 1f;

                }*/
                for (int i = 0; i < 1; i++)
                {
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningHit, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

                    }, player.whoAmI);
                }
            }

            Projectile.ai[0]++;

            if (Projectile.ai[1] == 0)          
                degrees = 0;
            
            if (Projectile.ai[1] == 1)           
                degrees = 40;
            
            if (Projectile.ai[1] == 2)
                degrees = 80;

            if (Projectile.ai[1] == 3)
                degrees = 120;
            
            if (Projectile.ai[1] == 4)
                degrees = 160;
            
            if (Projectile.ai[1] == 5)
                degrees = 200;
            
            if (Projectile.ai[1] == 6)
                degrees = 240;
            
            if (Projectile.ai[1] == 7)
                degrees = 280;
            
            if (Projectile.ai[1] == 8)
                degrees = 320;

            if (player.GetModPlayer<MiscFeatures>().ninelives <= Projectile.ai[1] || player.dead)
            {
                if (!escaped)//mark soul as escaped
                {
                    Projectile.timeLeft = 300;
                    
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.StellarTune, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Projectile.Center.X, Projectile.Center.Y),

                    }, player.whoAmI);
                    SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 1f, Pitch = 0.5f, MaxInstances = 0 }, Projectile.Center);
                    escaped = true;
                }        
                //Projectile.Kill();
            }

            if (!escaped)//prevent escaped souls returning
            {
                Projectile.rotation = player.velocity.X / 40;

                //Factors for calculations
                double deg = (degrees + 90); //The degrees, you can multiply Projectile.ai[1] to make it orbit faster, may be choppy depending on the value
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 75; //Distance away from the player

                Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
                Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

                /*shoottime++;
                for (int i = 0; i < 200; i++)
                {               
                    NPC target = Main.npc[i];
                    if (Vector2.Distance(Projectile.Center, target.Center) <= 500f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy && target.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0) && Collision.CanHitLine(Projectile.Center, 0, 0, player.Center, 0, 0))
                    {
                        float projspeed = 6;
                        Vector2 velocity = Vector2.Normalize(new Vector2(target.Center.X, target.Center.Y) - new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projspeed;
                        if (shoottime > 180)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(8));
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ProjectileID.LostSoulFriendly, 40, Projectile.knockBack, Projectile.owner);
                            SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
                            shoottime = 0;
                        }
                    }
                }*/
            }
            
            if (escaped)//fly up to freedom
            {
                if (Projectile.velocity.Y >= -15)
                {
                    Projectile.velocity.Y -= 0.2f;
                }
                Projectile.rotation = 0;
            }
            if (Main.rand.Next(10) == 0)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                dust.noGravity = true;
                dust.velocity *= 0.75f;
                dust.scale = 0.75f;
            }
            if (scaleup)
            {
                Projectile.scale += 0.01f;
            }
            else
            {
                Projectile.scale -= 0.01f;
            }
            if (Projectile.scale >= 1f)
            {
                scaleup = false;
            }
            if (Projectile.scale <= 0.75f)
            {
                scaleup = true;
            }
            // AnimateProjectile();
        }
        public override bool? CanDamage()
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 10; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 31);
                    dust.noGravity = true;
                }
            }
        }
        Texture2D texture;
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/TheSickleProj3");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);
        }
        /* public void AnimateProjectile() // Call this every frame, for example in the AI method.
         {
             Projectile.frameCounter++;
             if (Projectile.frameCounter >= 5) // This will change the sprite every 5 frames
             {
                 Projectile.frame++;
                 Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                 Projectile.frameCounter = 0;
             }
         }*/
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }
    }

}