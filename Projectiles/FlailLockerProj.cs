using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

using ReLogic.Content;
using Microsoft.CodeAnalysis;


namespace StormDiversMod.Projectiles
{
    public class FlailLockerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Flail Locker Ball");
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;

            //Projectile.aiStyle = 14;
            //Projectile.aiStyle = ProjAIStyleID.Flail;
            //AIType = ProjectileID.BlueMoon;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 158, 0, 0, 100, default, 1f);
            dust.noGravity = true;
            dust.noLight = true;

            var player = Main.player[Projectile.owner];
            // If owner player dies or moves too far away, remove the flail.
            if (player.dead || Vector2.Distance(player.Center, Projectile.Center) > 1500)
            {
                Projectile.Kill();
                return;
            }

            // This prevents the item from being able to be used again prior to this Projectile dying
            player.itemAnimation = 10;
            player.itemTime = 10;
            // Here we turn the player and Projectile based on the relative positioning of the player and Projectile.
            int newDirection = Projectile.Center.X > player.Center.X ? 1 : -1;
            player.ChangeDir(newDirection);
            Projectile.direction = newDirection;

            var vectorToPlayer = player.MountedCenter - Projectile.Center;
            float currentChainLength = vectorToPlayer.Length();

            // Here is what various ai[] values mean in this AI code:
            // ai[0] == 0: Just spawned/being thrown out
            // ai[0] == 1: Flail has hit a tile or has reached maxChainLength, and is now in the swinging mode
            // ai[1] == 1 or !Projectile.tileCollide: Projectile is being forced to retract

            // ai[0] == 0 means the Projectile has neither hit any tiles yet or reached maxChainLength
            if (Projectile.ai[0] == 0f)
            {
                // This is how far the chain would go measured in pixels
                float maxChainLength = 250;
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
                    // Once player lets go of the use button, let gravity take over and let air friction slow down the Projectile
                    if (Projectile.velocity.Y < 0f)
                        Projectile.velocity.Y *= 0.9f;

                    Projectile.velocity.Y += 1f;
                    Projectile.velocity.X *= 0.9f;
                }
            }
            else if (Projectile.ai[0] == 1f)
            {
                // When ai[0] == 1f, the Projectile has either hit a tile or has reached maxChainLength, so now we retract the Projectile
                float elasticFactorA = 14f / player.GetAttackSpeed(DamageClass.MeleeNoSpeed);
                float elasticFactorB = 0.9f / player.GetAttackSpeed(DamageClass.MeleeNoSpeed);
                float maxStretchLength = 450; // This is the furthest the flail can stretch before being forced to retract. Make sure that this is a bit more than maxChainLength so you don't accidentally reach maxStretchLength on the initial throw.

                if (Projectile.ai[1] == 1f)
                    Projectile.tileCollide = false;

                // If the user lets go of the use button, or if the Projectile is stuck behind some tiles as the player moves away, the Projectile goes into a mode where it is forced to retract and no longer collides with tiles.
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

                // If there is tension in the chain, or if the Projectile is being forced to retract, give the Projectile some velocity towards the player
                if (currentChainLength > restingChainLength || !Projectile.tileCollide)
                {
                    var elasticAcceleration = vectorToPlayer * elasticFactorA / currentChainLength - Projectile.velocity;
                    elasticAcceleration *= elasticFactorB / elasticAcceleration.Length();
                    Projectile.velocity *= 0.98f;
                    Projectile.velocity += elasticAcceleration;
                }
                else
                {
                    // Otherwise, friction and gravity allow the Projectile to rest.
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

            if (Projectile.shimmerWet) //shimmer interaction
            {
                Projectile.ai[0] = 1;
                if (Projectile.velocity.Y > -25f)
                Projectile.velocity.Y -= 1f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            // This custom OnTileCollide code makes the Projectile bounce off tiles at 1/5th the original speed, and plays sound and spawns dust if the Projectile was going fast enough.
            bool shouldMakeSound = false;

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

            // ai[0] == 1 is used in AI to represent that the Projectile has hit a tile since spawning
            Projectile.ai[0] = 1f;

            if (shouldMakeSound)
            {
                // if we should play the sound..
                Projectile.netUpdate = true;
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                // Play the sound
                SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
                
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);

            // This fixes a vanilla GetPlayerArmPosition bug causing the chain to draw incorrectly when stepping up slopes. The flail itself still draws incorrectly due to another similar bug. This should be removed once the vanilla bug is fixed.
            playerArmPosition.Y -= Main.player[Projectile.owner].gfxOffY;

            Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>("StormDiversMod/Projectiles/FlailLockerChain");

            Rectangle? chainSourceRectangle = null;
            // Drippler Crippler customizes sourceRectangle to cycle through sprite frames: sourceRectangle = asset.Frame(1, 6);
            float chainHeightAdjustment = 0f; // Use this to adjust the chain overlap. 

            Vector2 chainOrigin = chainSourceRectangle.HasValue ? (chainSourceRectangle.Value.Size() / 2f) : (chainTexture.Size() / 2f);
            Vector2 chainDrawPosition = Projectile.Center;
            Vector2 vectorFromProjectileToPlayerArms = playerArmPosition.MoveTowards(chainDrawPosition, 4f) - chainDrawPosition;
            Vector2 unitVectorFromProjectileToPlayerArms = vectorFromProjectileToPlayerArms.SafeNormalize(Vector2.Zero);
            float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : chainTexture.Height()) + chainHeightAdjustment;
            if (chainSegmentLength == 0)
            {
                chainSegmentLength = 10; // When the chain texture is being loaded, the height is 0 which would cause infinite loops.
            }
            float chainRotation = unitVectorFromProjectileToPlayerArms.ToRotation() + MathHelper.PiOver2;
            int chainCount = 0;
            float chainLengthRemainingToDraw = vectorFromProjectileToPlayerArms.Length() + chainSegmentLength / 2f;

            // This while loop draws the chain texture from the projectile to the player, looping to draw the chain texture along the path
            while (chainLengthRemainingToDraw > 0f)
            {
                // This code gets the lighting at the current tile coordinates
                Color chainDrawColor = Lighting.GetColor((int)chainDrawPosition.X / 16, (int)(chainDrawPosition.Y / 16f));

                // Flaming Mace and Drippler Crippler use code here to draw custom sprite frames with custom lighting.
                // Cycling through frames: sourceRectangle = asset.Frame(1, 6, 0, chainCount % 6);
                // This example shows how Flaming Mace works. It checks chainCount and changes chainTexture and draw color at different values

                var chainTextureToDraw = chainTexture;
                /*if (chainCount >= 4)
                {
                    // Use normal chainTexture and lighting, no changes
                }
                else if (chainCount >= 2)
                {
                    // Near to the ball, we draw a custom chain texture and slightly make it glow if unlit.
                    chainTextureToDraw = chainTextureExtra;
                    byte minValue = 140;
                    if (chainDrawColor.R < minValue)
                        chainDrawColor.R = minValue;

                    if (chainDrawColor.G < minValue)
                        chainDrawColor.G = minValue;

                    if (chainDrawColor.B < minValue)
                        chainDrawColor.B = minValue;
                }
                else
                {
                    // Close to the ball, we draw a custom chain texture and draw it at full brightness glow.
                    chainTextureToDraw = chainTextureExtra;
                    chainDrawColor = Color.White;
                }*/

                // Here, we draw the chain texture at the coordinates
                Main.spriteBatch.Draw(chainTextureToDraw.Value, chainDrawPosition - Main.screenPosition, chainSourceRectangle, chainDrawColor, chainRotation, chainOrigin, 1f, SpriteEffects.None, 0f);

                // chainDrawPosition is advanced along the vector back to the player by the chainSegmentLength
                chainDrawPosition += unitVectorFromProjectileToPlayerArms * chainSegmentLength;
                chainCount++;
                chainLengthRemainingToDraw -= chainSegmentLength;
            }

            
            return true;
        }
        /*public override bool PreDraw(ref Color lightColor)
        {
            
            var player = Main.player[Projectile.owner];

            Vector2 mountedCenter = player.MountedCenter + new Vector2(4 * player.direction, 6);
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
            }
           
            return true;
        }*/
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];

            /*if (firedspike == false && !player.controlUseItem)//Create spikes once when fail is launched
            {

                float numberProjectiles = 8;
                float rotation = MathHelper.ToRadians(180);
                //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                for (int j = 0; j < numberProjectiles; j++)
                {
                    float speedX = 0f;
                    float speedY = 11f;
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<DestroyerFlailProj3>(), (int)(Projectile.damage * 0.4f), 0.5f, Projectile.owner);
                }

                SoundEngine.PlaySound(SoundID.Item17 with{Volume = 1.5f}, Projectile.Center);

                firedspike = true;
            }*/
        }
        public override void PostDraw(Color lightColor)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/DestroyerFlailProj_Glow");

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Center, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


        }
    }
    
}
