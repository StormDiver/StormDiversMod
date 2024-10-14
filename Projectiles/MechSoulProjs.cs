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


namespace StormDiversMod.Projectiles
{
    public class SeekerBoltProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Seeker Bolt");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
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
                //if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
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
                Projectile.alpha = 255;

                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 0f;
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionGenericProj>(), 0, 0, Projectile.owner);
                Main.projectile[proj].scale = 1.25f;

                for (int i = 0; i < 30; i++) //orange particles
                {
                    Vector2 perturbedSpeed = new Vector2(0, -4f).RotatedByRandom(MathHelper.ToRadians(360));

                    var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                    dust.noGravity = true;

                    dust.scale = 2f;
                }

                for (int i = 0; i < 20; i++) //Grey dust circle
                {
                    Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));
                    var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                    dust.noGravity = true;
                    dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;

                }
               
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 3)
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
            else
            {
                return false;
            }
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
            //DisplayName.SetDefault("Mechanical ChainSaw");
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
            Projectile.localNPCHitCooldown = 10;
            Player player = Main.player[Projectile.owner];
            if (Main.rand.Next(15) == 0)
            {
                Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(25));
                float scale = 1f - (Main.rand.NextFloat() * .3f);
                perturbedSpeed = perturbedSpeed * scale;
                int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2((perturbedSpeed.X * 0.28f), (float)(perturbedSpeed.Y * 0.28f)), ProjectileID.MolotovFire, (int)(Projectile.damage * 0.4f), 0, Projectile.owner);
                Main.projectile[projID].usesLocalNPCImmunity = true;
                Main.projectile[projID].localNPCHitCooldown = 10;
            }
            AnimateProjectile();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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

        public override void PostDraw(Color lightColor) //glowmask for animated
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/SawBladeChain_Glow");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

    }
    //_______________________________________________________________________________________
    public class DestroyerFlailProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vaporiser Flail");
        }
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;

            //Projectile.aiStyle = 15; // Set the aiStyle to that of a flail.

            Projectile.aiStyle = ProjAIStyleID.Flail;
            //AIType = ProjectileID.BlueMoon;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        int shoottime = 0;
        bool stopspikes = false;//For stopped spinning

        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            //if (player.controlUseItem && !stopspikes)
            {
                shoottime++;
            }
            if (!player.controlUseItem)//When stopped spinning, can no longer fire spikes
            {
                stopspikes = true;
            }
            if ((shoottime >= 25 && !stopspikes) || (shoottime >= 45 && stopspikes && Projectile.tileCollide))//Fire spikes while spinning
            {
                float numberProjectiles = 8;
                float rotation = MathHelper.ToRadians(180);
                //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                for (int j = 0; j < numberProjectiles; j++)
                {
                    float speedX = 0f;
                    float speedY = 11f;
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles)));
                    if (!stopspikes)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(player.Center.X, player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<DestroyerFlailProj3>(), (int)(Projectile.damage * 0.75f), 0.5f, Projectile.owner);
                    else
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<DestroyerFlailProj3>(), (int)(Projectile.damage * 1f), 0.5f, Projectile.owner);

                }


                SoundEngine.PlaySound(SoundID.Item17 with{Volume = 1.5f}, Projectile.Center);

                shoottime = 0;
            }
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
            Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);

            // This fixes a vanilla GetPlayerArmPosition bug causing the chain to draw incorrectly when stepping up slopes. The flail itself still draws incorrectly due to another similar bug. This should be removed once the vanilla bug is fixed.
            playerArmPosition.Y -= Main.player[Projectile.owner].gfxOffY;

            Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>("StormDiversMod/Projectiles/DestroyerFlailChain");

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
        public override void PostDraw(Color lightColor) //glowmask for animated
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/DestroyerFlailProj_Glow");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
    }
    //_______________________________________________________________________________________
    public class DestroyerFlailProj2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Destroyer Flail Unused");
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


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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
        public override void OnKill(int timeLeft)
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
            //DisplayName.SetDefault("Vaporiser Spike");
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
        }

        public override void OnKill(int timeLeft)
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
            //DisplayName.SetDefault("Prime Skull");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

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
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            reflect = true; //Once this projectile has hit an enemy it will home in and bounce off walls
           
            for (int i = 0; i < 10; i++)
            {

                 
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 5);
            }
        }
        public override void OnKill(int timeLeft)
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
