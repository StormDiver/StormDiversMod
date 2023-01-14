using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Basefiles;

namespace StormDiversMod.Projectiles
{

    public class BeetleSpearProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beetle Spear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 19;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }
        protected virtual float HoldoutRangeMin => 50f;
        protected virtual float HoldoutRangeMax => 175f;

        bool beetled;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner]; // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            int duration = player.itemAnimationMax; // Define the duration the projectile will exist in frames

            player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

            // Reset projectile time left if necessary
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }

            Projectile.velocity = Vector2.Normalize(Projectile.velocity); // Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

            float halfDuration = duration * 0.5f;
            float progress;

            // Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation.
            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }


                // Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
                Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

            // Apply proper rotation to the sprite.
            if (Projectile.spriteDirection == -1)
            {
                // If sprite is facing left, rotate 45 degrees
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                // If sprite is facing right, rotate 135 degrees
                Projectile.rotation += MathHelper.ToRadians(135f);
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, 186, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 1, Scale: 1.2f);
                dust.noGravity = true;
                dust.velocity += Projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;
            }

            bool lineOfSight = Collision.CanHitLine(Projectile.Center, 0, 0, player.position, player.width, player.height);

            if (Projectile.timeLeft < halfDuration && lineOfSight)
            {
                if (!beetled)
                {
                    SoundEngine.PlaySound(SoundID.Zombie50 with { Volume = 1f, Pitch = 1.3f, MaxInstances = 0 }, Projectile.Center);

                    Vector2 perturbedSpeed = new Vector2(0, -7).RotatedByRandom(MathHelper.ToRadians(360));

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BeetleProj>(), (int)(Projectile.damage * 0.5f), 0, Projectile.owner);
                    beetled = true;
                }

            }

        }


    }
    //________________________________________________________________________________________________________________
    public class BeetleYoyoProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beetle Yoyo");
            //ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 15f;

            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;

            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 16f;
        }
        public override void SetDefaults()
        {

            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = -1;

            Projectile.scale = 1f;
            Projectile.aiStyle = 99;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            // drawOffsetX = -3;
            // drawOriginOffsetY = 1;
        }
        int shoottime = 0;
        
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage); //update damage

            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.position;
            dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 186, 0f, 0f, 0, new Color(255, 255, 255), 1f);
            dust.noGravity = true;

            shoottime++;
            if (shoottime >= 20)
            {

                SoundEngine.PlaySound(SoundID.Zombie50 with{Volume = 1f, Pitch = 1.3f, MaxInstances = 0 }, Projectile.Center);

                Vector2 perturbedSpeed = new Vector2(0, -4).RotatedByRandom(MathHelper.ToRadians(360));

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BeetleProj>(), (int)(Projectile.damage * 0.5f), 0, Projectile.owner);



                shoottime = 0;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

        }
    }
    //________________________________________________________________________________________________________________
    public class BeetleShellProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beetle Shell");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void SetDefaults()
        {

            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 400;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 2;
        }

        public override void AI()
        {
            

            Projectile.rotation += (float)Projectile.direction * -0.6f;

            DrawOffsetX = 0;
            DrawOriginOffsetY = 2;
            Projectile.width = 26;
            Projectile.height = 26;
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;

            SoundEngine.PlaySound(SoundID.Item7, Projectile.Center);


            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 186);
                dust.noGravity = true;
            }
            if (Main.rand.Next(4) == 0)
            {
                Vector2 perturbedSpeed = new Vector2(0, -7).RotatedByRandom(MathHelper.ToRadians(360));

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BeetleProj>(), (int)(Projectile.damage * 0.5f), 0, Projectile.owner);
                SoundEngine.PlaySound(SoundID.Zombie50 with{Volume = 1f, Pitch = 1.3f, MaxInstances = 0 }, Projectile.Center);

            }
        }
        int reflect = 4;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();

            }
            SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);


            if (Main.rand.Next(4) == 0)
            {
                Vector2 perturbedSpeed = new Vector2(0, -7).RotatedByRandom(MathHelper.ToRadians(360));

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BeetleProj>(), (int)(Projectile.damage * 0.5f), 0, Projectile.owner);
                SoundEngine.PlaySound(SoundID.Zombie50 with{Volume = 1f, Pitch = 1.3f, MaxInstances = 0 }, Projectile.Center);

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
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 186);
                dust.noGravity = true;
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Item89, Projectile.Center);


                for (int i = 0; i < 25; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 186);
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

    }

    //________________________________________________________________________________________________________________
    public class BeetleProj : ModProjectile //For beetle Weapons
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beetle");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.friendly = true;
            Projectile.penetrate = 3;

            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.scale = 1f;
            Projectile.extraUpdates = 1;


            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

        }
        int damagetime = 0;
        public override bool? CanDamage()
        {
            if (damagetime <= 30)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void AI()
        {
            damagetime++;
            if (Projectile.velocity.X < 0)
            {
                Projectile.spriteDirection = -1;
            }
            else
            {
                Projectile.spriteDirection = 1;

            }
            Projectile.rotation = Projectile.velocity.X / 20;

            if (damagetime > 30)
            {
                if (Projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;
                float distance = 700f;
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
          
            AnimateProjectile();
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            if (damagetime > 30)
            {
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                if (magnitude > 6f)
                {
                    vector *= 6f / magnitude;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.8f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(ModContent.BuffType<Buffs.BeetleDebuff>(), 300);
                //Main.PlaySound(SoundID.Zombie, (int)Projectile.Center.X, (int)Projectile.Center.Y, 50);
            }
            Projectile.damage = (Projectile.damage * 9) / 10;
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {


                for (int i = 0; i < 7; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width = 10, Projectile.height = 10, 186);

                    dust.noGravity = true;

                }

            }
        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }


    }
    //________________________________________________________________________________________________________________
    public class BeetleGloveProj : ModProjectile //For Gauntlet
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Beetle");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;

            Projectile.scale = 1f;
            Projectile.extraUpdates = 1;

            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.ArmorPenetration = 15;

        }
        int damagetime = 0;
        public override bool? CanDamage()
        {
            if (damagetime <= 30)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void AI()
        {
            damagetime++;
            if (Projectile.velocity.X < 0)
            {
                Projectile.spriteDirection = -1;
            }
            else
            {
                Projectile.spriteDirection = 1;

            }
            Projectile.rotation = Projectile.velocity.X / 20;
            if (damagetime < 60)
            {
                Projectile.velocity *= 0.98f;
            }
            if (damagetime > 60)
            {
                if (Projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;
                float distance = 500f;
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
                    Projectile.velocity = (10 * Projectile.velocity + move) / 10f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }

            AnimateProjectile();
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            if (damagetime > 60)
            {
                float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                if (magnitude > 5f)
                {
                    vector *= 5f / magnitude;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.8f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(4) == 0)
            {
                target.AddBuff(ModContent.BuffType<Buffs.BeetleDebuff>(), 180);
                //Main.PlaySound(SoundID.Zombie, (int)Projectile.Center.X, (int)Projectile.Center.Y, 50);
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {


                for (int i = 0; i < 3; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.Center, Projectile.width = 10, Projectile.height = 10, 186);

                    dust.noGravity = true;

                }

            }
        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }


    }




}