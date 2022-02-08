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
    public class SeekerBoltProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seeker Bolt");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;

            
            Projectile.light = 0.5f;
            Projectile.friendly = true;

            // Projectile.CloneDefaults(338);
            // aiType = 338;
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            //drawOffsetX = -4;
            //drawOriginOffsetY = 0;

        }
        //int homed;
        int dusttime;
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            dusttime++;

            /* Dust dust;
             // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
             Vector2 position = Projectile.Center;
             dust = Terraria.Dust.NewDustPerfect(position, 182, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
             dust.noGravity = true;
             dust.fadeIn = 1;*/
            if (dusttime > 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    float X = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                    float Y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;


                    int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, 6, 0, 0, 100, default, 1f);
                    Main.dust[dust].position.X = X;
                    Main.dust[dust].position.Y = Y;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0f;
                }
            }

            var player = Main.player[Projectile.owner];

            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 750f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                //if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
                if (player.controlUseTile && player.HeldItem.type == ModContent.ItemType<Items.Weapons.MechTheSeeker>() && !player.dead && Projectile.timeLeft > 60 && Projectile.owner == Main.myPlayer)
                {
                    if (Collision.CanHit(Projectile.Center, 0, 0, Main.MouseWorld, 0, 0))
                    {


                        //Projectile.timeLeft = 120;
                        Vector2 newMove = Main.MouseWorld - Projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            move = newMove;
                            distance = distanceTo;
                            target = true;
                        }
                    }

                }


            }
          
            if (target)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (11 * Projectile.velocity + move) / 11f;
                AdjustMagnitude(ref Projectile.velocity);
            }
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 100;
                Projectile.height = 100;
                Projectile.Center = Projectile.position;


                Projectile.knockBack = 6;
            }

        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 15f)
            {
                vector *= 15f / magnitude;
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
            


        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 14);
                
                for (int i = 0; i < 50; i++)
                {
                    Dust dust;
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Vector2 position = Projectile.position;
                    dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                    dust.noGravity = true;
                    dust.scale = 2f;


                }
                for (int i = 0; i < 30; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
                for (int i = 0; i < 40; i++)
                {

                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);

                    dust2.scale = 1.5f;
                    dust2.velocity *= 2;
                }



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
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
    //_______________________________________________________________________________________
    public class SawBladeChain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical ChainSaw");
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            //Projectile.CloneDefaults(509);
            // aiType = 509;
            Projectile.aiStyle = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true; //so you can't hit enemies through walls
            Projectile.DamageType = DamageClass.Melee;

            DrawOffsetX = 5;
            DrawOriginOffsetY = 0;

            
        }

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.9f);
            Main.dust[dust].noGravity = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Player player = Main.player[Projectile.owner];
            if (Main.rand.Next(16) == 0)
            {
                
                    Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(25));
                    float scale = 1f - (Main.rand.NextFloat() * .3f);
                    perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2((perturbedSpeed.X * 0.22f), (float)(perturbedSpeed.Y * 0.22f)), ProjectileID.MolotovFire, (int)(Projectile.damage * 0.4f), 0, Projectile.owner);
            }

           
            AnimateProjectile();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

          
        }


        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 2; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
       
        /*public override void PostDraw(Color drawColor)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/ShroomArrowProj_Glow");

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Center, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }*/

    }
    //_______________________________________________________________________________________
    
    public class DestroyerFlailProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Destroyer Flail");
        }
        public override void SetDefaults()
        {

            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.aiStyle = 15; // Set the aiStyle to that of a flail.
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        int shoottime = 60;
        bool firedspike = false;

        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            shoottime++;
            if (!player.controlUseItem)
            {
                firedspike = true;
            }
            // Spawn some dust visuals
           /* var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 100, default, 1.5f);
            dust.noGravity = true;
            dust.velocity /= 2f;

            var player = Main.player[Projectile.owner];

            if (!player.controlUseItem)
            {
                firedspike = true;

            }
            shoottime++;
            if (shoottime == 14 && player.controlUseItem)
            {
                if (!firedspike)
                {
                    float numberProjectiles = 8;
                    float rotation = MathHelper.ToRadians(180);
                    //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                    for (int j = 0; j < numberProjectiles; j++)
                    {
                        float speedX = 0f;
                        float speedY = 11f;
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                        Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<DestroyerFlailProj3>(), (int)(Projectile.damage * 0.6f), Projectile.knockBack, Projectile.owner);
                    }
                    SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 17, 1.5f);

                    firedspike = true;
                }

            }
            // If owner player dies, remove the flail.
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            // This prevents the item from being able to be used again prior to this projectile dying
            player.itemAnimation = 10;
            player.itemTime = 10;

            // Here we turn the player and projectile based on the relative positioning of the player and Projectile.
            int newDirection = Projectile.Center.X > player.Center.X ? 1 : -1;
            player.ChangeDir(newDirection);
            Projectile.direction = newDirection;

            var vectorToPlayer = player.MountedCenter - Projectile.Center;
            float currentChainLength = vectorToPlayer.Length();

            // Here is what various ai[] values mean in this AI code:
            // ai[0] == 0: Just spawned/being thrown out
            // ai[0] == 1: Flail has hit a tile or has reached maxChainLength, and is now in the swinging mode
            // ai[1] == 1 or !Projectile.tileCollide: projectile is being forced to retract

            // ai[0] == 0 means the projectile has neither hit any tiles yet or reached maxChainLength
            if (Projectile.ai[0] == 0f)
            {
                // This is how far the chain would go measured in pixels
                float maxChainLength = 1000f;
                Projectile.tileCollide = true;
               
                if (currentChainLength > maxChainLength)
                {
                    // If we reach maxChainLength, we change behavior.
                    Projectile.ai[0] = 1f;
                    Projectile.netUpdate = true;

                    //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;



                }
                else if (!player.channel)
                {
                    // Once player lets go of the use button, let gravity take over and let air friction slow down the projectile
                    if (Projectile.velocity.Y < 0f)
                        Projectile.velocity.Y *= 0.9f;

                    Projectile.velocity.Y += 1f;
                    Projectile.velocity.X *= 0.9f;
                }
            }
            else if (Projectile.ai[0] == 1f)
            {

               
                // When ai[0] == 1f, the projectile has either hit a tile or has reached maxChainLength, so now we retract the projectile
                float elasticFactorA = 14f / player.meleeSpeed;
                float elasticFactorB = 0.9f / player.meleeSpeed;
                float maxStretchLength = 1100f; // This is the furthest the flail can stretch before being forced to retract. Make sure that this is a bit more than maxChainLength so you don't accidentally reach maxStretchLength on the initial throw.

                if (Projectile.ai[1] == 1f)
                    Projectile.tileCollide = false;

                // If the user lets go of the use button, or if the projectile is stuck behind some tiles as the player moves away, the projectile goes into a mode where it is forced to retract and no longer collides with tiles.
                if (!player.channel || currentChainLength > maxStretchLength || !Projectile.tileCollide)
                {
                    Projectile.ai[1] = 1f;

                    if (Projectile.tileCollide)
                        Projectile.netUpdate = true;

                    Projectile.tileCollide = false;

                    if (currentChainLength < 20f)
                        Projectile.Kill();
                }

                if (!Projectile.tileCollide)
                    elasticFactorB *= 2f;

                int restingChainLength = 60;

                // If there is tension in the chain, or if the projectile is being forced to retract, give the projectile some velocity towards the player
                if (currentChainLength > restingChainLength || !Projectile.tileCollide)
                {
                    var elasticAcceleration = vectorToPlayer * elasticFactorA / currentChainLength - Projectile.velocity;
                    elasticAcceleration *= elasticFactorB / elasticAcceleration.Length();
                    Projectile.velocity *= 0.98f;
                    Projectile.velocity += elasticAcceleration;
                }
                else
                {
                    // Otherwise, friction and gravity allow the projectile to rest.
                    if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < 6f)
                    {
                        Projectile.velocity.X *= 0.96f;
                        Projectile.velocity.Y += 0.2f;
                    }
                    if (player.velocity.X == 0f)
                        Projectile.velocity.X *= 0.96f;
                }
            }

            // Here we set the rotation based off of the direction to the player tweaked by the velocity, giving it a little spin as the flail turns around each swing 
            Projectile.rotation = vectorToPlayer.ToRotation() - Projectile.velocity.X * 0.1f;

            // Here is where a flail like Flower Pow could spawn additional projectiles or other custom behaviors
           */
        }
           
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
           
            // This custom OnTileCollide code makes the projectile bounce off tiles at 1/5th the original speed, and plays sound and spawns dust if the projectile was going fast enough.
           /* bool shouldMakeSound = false;

            if (oldVelocity.X != Projectile.velocity.X)
            {
                if (Math.Abs(oldVelocity.X) > 4f)
                {
                    shouldMakeSound = true;
                }

                Projectile.position.X += Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X * 0.2f;
            }

            if (oldVelocity.Y != Projectile.velocity.Y)
            {
                if (Math.Abs(oldVelocity.Y) > 4f)
                {
                    shouldMakeSound = true;
                }

                Projectile.position.Y += Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y * 0.2f;
            }

            // ai[0] == 1 is used in AI to represent that the projectile has hit a tile since spawning
            Projectile.ai[0] = 1f;

            if (shouldMakeSound)
            {
                // if we should play the sound..
                Projectile.netUpdate = true;
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                // Play the sound
                SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y);
            }
           */
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            
            /*var player = Main.player[Projectile.owner];

            Vector2 mountedCenter = player.MountedCenter;
            Texture2D chainTexture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/DestroyerFlailChain");

            var drawPosition = Projectile.Center;
            var remainingVectorToPlayer = mountedCenter - drawPosition;

            float rotation = remainingVectorToPlayer.ToRotation() - MathHelper.PiOver2;

            if (Projectile.alpha == 0)
            {
                int direction = -1;

                if (Projectile.Center.X < mountedCenter.X)
                    direction = 1;

                player.itemRotation = (float)Math.Atan2(remainingVectorToPlayer.Y * direction, remainingVectorToPlayer.X * direction);
            }

            // This while loop draws the chain texture from the projectile to the player, looping to draw the chain texture along the path
            while (true)
            {
                float length = remainingVectorToPlayer.Length();

                // Once the remaining length is small enough, we terminate the loop
                if (length < 25f || float.IsNaN(length))
                    break;

                // drawPosition is advanced along the vector back to the player by 12 pixels
                // 12 comes from the height of ExampleFlailProjectileChain.png and the spacing that we desired between links
                drawPosition += remainingVectorToPlayer * 12 / length;
                remainingVectorToPlayer = mountedCenter - drawPosition;

                // Finally, we draw the texture at the coordinates using the lighting information of the tile coordinates of the chain section
                Color color = Lighting.GetColor((int)drawPosition.X / 16, (int)(drawPosition.Y / 16f));
                Main.EntitySpriteDraw(chainTexture, drawPosition - Main.screenPosition, null, color, rotation, chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }*/

            return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (firedspike == true && shoottime >= 60)
            {

                float numberProjectiles = 8;
                float rotation = MathHelper.ToRadians(180);
                //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                for (int j = 0; j < numberProjectiles; j++)
                {
                    float speedX = 0f;
                    float speedY = 11f;
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<DestroyerFlailProj3>(), (int)(Projectile.damage * 0.3f), Projectile.knockBack, Projectile.owner);
                }

                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 17, 1.5f);

                shoottime = 0;
            }
        }
        public override void PostDraw(Color lightColor)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/DestroyerFlailProj_Glow");

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Center, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }
    }
    //_______________________________________________________________________________________
    public class DestroyerFlailProj2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Destroyer Flail");
        }
        public override void SetDefaults()
        {

            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            //aiType = ProjectileID.Meteor1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
            dust.noGravity = true;
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
           

        }
        int bouncesound = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            bouncesound++;
            if (Projectile.velocity.X >= 0.3f || Projectile.velocity.Y >= 0.3f)
            {
                if (bouncesound <= 5)
                {
                    SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
                }
            
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

                for (int i = 0; i < 20; i++)
                {

                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                    
                    dust2.scale = 1.5f;
                    dust2.velocity *= 2;
                }
                for (int i = 0; i < 25; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }

            }
        }
        public override void PostDraw(Color lightColor)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/DestroyerFlailProj_Glow");

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Center, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }
    }
    //_____________________________________________________________________________________________________________________________________________________
    public class DestroyerFlailProj3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vaporiser Spike");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;

            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.penetrate = 3;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;


            Projectile.DamageType = DamageClass.Melee;



        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

            return true;
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            Projectile.alpha += 15;
            if (Projectile.alpha > 250)
            {
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
        }

        public override void Kill(int timeLeft)
        {

            //Main.PlaySound(SoundID.Item10, Projectile.position);
          

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

    //_______________________________________________________________________________________
    public class SkullSeek : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prime Skull");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            //Projectile.aiStyle = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.light = 0.5f;
        }
        bool reflect = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            if (!reflect) //Projectile will be destroyed if it hits a wall before an enemy, but once it hits an enemy it can bounce off walls
            {
                return true;
            }
            else
            {
                {
                    Collision.HitTiles(Projectile.Center + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

                    if (Projectile.velocity.X != oldVelocity.X)
                    {
                        Projectile.velocity.X = -oldVelocity.X * 1;
                    }
                    if (Projectile.velocity.Y != oldVelocity.Y)
                    {
                        Projectile.velocity.Y = -oldVelocity.Y * 1f;
                    }

                }
                return false;
            }
        }
        public override void AI()
        {
            Projectile.rotation += (float)Projectile.direction * -0.4f;

            //Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 6, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
            Projectile.spriteDirection = Projectile.direction;

            if (reflect == true)
            {
                if (Projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;
                float distance = 400f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
                    {
                        if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                        {
                            Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                            if (distanceTo < distance)
                            {
                                move = newMove;
                                distance = distanceTo;
                                target = true;
                            }
                        }
                    }
                }

                if (target)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }
        }
    
         private void AdjustMagnitude(ref Vector2 vector)
         {
            if (reflect == true)
            {
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
               if (magnitude > 10f)
                {
                   vector *= 10f / magnitude;
              }
            }
         }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            reflect = true; //Once this projectile has hit an enemy it will home in and bounce off walls
           
            for (int i = 0; i < 10; i++)
            {

                Vector2 vel = new Vector2(Main.rand.NextFloat(20, 20), Main.rand.NextFloat(-20, -20));
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
            }
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

                for (int i = 0; i < 15; i++)
                {

                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);

                    dust2.scale = 1.25f;
                    dust2.velocity *= 2;
                }
                for (int i = 0; i < 20; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 0, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }
        }
        public override void PostDraw(Color lightColor)
        {
           
            if (reflect)
            {
                Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/SkullSeek_Glow");

                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Center, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

            }
            else return;

        }
    }
   
}
