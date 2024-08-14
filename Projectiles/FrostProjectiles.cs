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
using Microsoft.CodeAnalysis;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;


namespace StormDiversMod.Projectiles
{

    public class FrostGrenadeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Grenade");
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            

            if (Projectile.timeLeft > 3)
            {
                Projectile.timeLeft = 3;
            }
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(1) == 0) // the chance
            {
               
                    target.AddBuff(BuffID.Frostburn2, 300);

                

            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(BuffID.Frostburn2, 300);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionFrostProj>(), 0, 0, Projectile.owner);
            //Main.projectile[proj].scale = 1;
         
            for (int i = 0; i < 30; i++) //Frost dust
            {
                Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 156, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 1.5f;

            }
            for (int i = 0; i < 30; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;

            }
          

        }

    }
    //___________________________________________________________________________________________________________________________________
    public class FrostSpinProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frozen Polestar");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {

            Projectile.width = 126;
            Projectile.height = 126;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 26;
            Projectile.timeLeft = 9999999;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(1) == 0) // the chance
            {
                target.AddBuff(BuffID.Frostburn2, 300);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(BuffID.Frostburn2, 300);
        }
        bool parry;
        int parrytime = 120; //count down timer
        int parrycooldown = 120; //the cooldown amount

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            // 20 frame parry, 120 frame cooldown
            if (parrytime > 0 && player.releaseUseTile) //don't count down if player is still holding rmb
                parrytime--;

            if (parrytime == 1) //sound when can parry again
            {
                SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing with { Volume = 1f, Pitch = 0.5f, MaxInstances = 1 }, Projectile.Center);
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(player.Center.X, player.Center.Y),
                }, player.whoAmI);
            }

            if (player.controlUseTile && !parry && parrytime <= 0) //add parry buff
            {
                player.AddBuff(ModContent.BuffType<ReflectedBuff>(), 20);
                parrytime = parrycooldown + 20;
                parry = true;
            }
            if (!player.HasBuff(ModContent.BuffType<ReflectedBuff>()))
                parry = false;

            if (parry)
                Projectile.scale = 1.1f;
            else
                Projectile.scale = 1;

            //Main.NewText("parrytime: " + parrytime, 220, 63, 139);

            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage); //update damage

            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.dead)
                {
                    Projectile.Kill();
                    player.ClearBuff(ModContent.BuffType<ReflectedBuff>());

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
            Projectile.position.Y += player.height / 10;

            Projectile.spriteDirection = player.direction;

            if (parry)
                Projectile.rotation += MathHelper.ToRadians(12.5f) * player.direction; //this is the projectile rotation/spinning speed
            else
                Projectile.rotation += MathHelper.ToRadians(8.5f) * player.direction; //this is the projectile rotation/spinning speed

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = Projectile.rotation;


            //-------------------------------------------------------------Sound-------------------------------------------------------
            if (!parry)
                Projectile.soundDelay--;
            else
                Projectile.soundDelay -= 3;

            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item1 with { Volume = 2f, Pitch = -0.5f, MaxInstances = 5 }, Projectile.Center);
                for (int i = 0; i < 20; i++) //Frost particles
                {
                    Vector2 perturbedSpeed = new Vector2(0, -7f).RotatedByRandom(MathHelper.ToRadians(360));

                    var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 180, perturbedSpeed.X + player.velocity.X, perturbedSpeed.Y + player.velocity.Y);
                    dust.noGravity = true;
                    dust.scale = 1.5f;

                }
                Projectile.soundDelay = 30;
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
            hitbox.Width = 160;
            hitbox.Height = 160;
            hitbox.X -= (hitbox.Width - Projectile.width) / 2;
            hitbox.Y -= (hitbox.Height - Projectile.height) / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool PreDraw(ref Color lightColor)
        {           
            Main.instance.LoadProjectile(Projectile.type);

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/FrostSpinProj_Trail");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.position - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }

    }
    //___________________________________________________________________________________________________________________________________
    public class FrostStarProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Frizbee");
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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
                   
                        target.AddBuff(BuffID.Frostburn2, 300);

                    

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
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(BuffID.Frostburn2, 300);
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
       
        public override void OnKill(int timeLeft)
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
            //DisplayName.SetDefault("Frost");
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
            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 0.1f;
            DrawOffsetX = -35;
            DrawOriginOffsetY = -35;
            Projectile.light = 0.9f;
            Projectile.ArmorPenetration = 15;
        }
        public override bool? CanDamage()
        {
            if (Projectile.alpha < 125 && Projectile.ai[1] == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            //Projectile.rotation = Main.rand.NextFloat(0, 6.2f); //speen start
        }
        int dustoffset;
        int alphaadd; //add alpha to the trail
        int posadd = 5; //adjust trail position
        public override void AI()
        {
            Projectile.rotation += 0.05f * -Projectile.direction;
            if (dustoffset > 5)
            {
                if (Main.rand.Next(25) == 0) //dust spawn sqaure increases with hurtbox size
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X - (dustoffset / 2), Projectile.position.Y - (dustoffset / 2)), Projectile.width + dustoffset, Projectile.height + dustoffset, 156, Projectile.velocity.X, -5, 130, default, 1.25f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;

                    //int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f); //this defines the flames dust and color parcticles, like when they fall thru ground, change DustID to wat dust you want from Terraria
                }
            }

            if (Projectile.scale <= 1f)//increase size until specified amount
            {
                dustoffset += 1; //makes dust expand with projectile, also used for hitbox

                Projectile.scale += 0.014f;
            }
            else
            {
                Projectile.alpha += 2;
                Projectile.velocity *= 0.96f;

                //begin animation
                if (Projectile.frame < 2) //stop at frame 3
                {
                    Projectile.frameCounter++;
                    if (Projectile.frameCounter >= 40)
                    {
                        Projectile.frame++;
                        Projectile.frameCounter = 0;
                    }
                }
            }
            if (Projectile.alpha > 200 || Projectile.wet)//once faded enough or touches water kill projectile
            {
                Projectile.Kill();
            }

            //Trail effect(it works don't judge)
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[2]++;
               
                if (Projectile.ai[2] % 6 == 0 && Projectile.ai[2] <= 30) //summon a trail projectile every X frames
                {
                    posadd += 5; //add X times velcity to position each time
                    Vector2 velocity = Projectile.velocity * posadd;

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(0);
                    alphaadd += 8; //Add alpha so it fades out at the same time
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X - perturbedSpeed.X, Projectile.Center.Y - perturbedSpeed.Y), Projectile.velocity, ModContent.ProjectileType<Frostthrowerproj>(), 0, 0, Projectile.owner);
                    Main.projectile[projID].ai[1] = 1;
                    Main.projectile[projID].alpha += alphaadd;
                }
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
            if (Projectile.ai[0] == 0) // only on proj deals damage
            {
                hitbox.Width = dustoffset;
                hitbox.Height = dustoffset;
                hitbox.X -= dustoffset / 2 - (Projectile.width / 2);
                hitbox.Y -= dustoffset / 2 - (Projectile.height / 2);
            }
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(1) == 0) // the chance
            {
                target.AddBuff(BuffID.Frostburn2, 300);

            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(BuffID.Frostburn2, 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0;
            //Projectile.Kill();
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.LightBlue;
            color.A = (Byte)Projectile.alpha;
            return color;

        }
        public override void PostDraw(Color lightColor)
        {
            /*Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = new Vector2(0, 0) + Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(texture, drawPos, null, Color.White, Projectile.rotation, Projectile.Center, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);*/
            //spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, projectile.Center, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
    }
    //___________________________________________________________________________________________________________________________________
    public class FrostAccessProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frost Fragment");
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            if (Projectile.ai[2] == 0)
            target.AddBuff(BuffID.Frostburn2, 180);
            else if (Projectile.ai[2] == 1)
                target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 150);

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(BuffID.Frostburn2, 180);
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
                SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width = 10, Projectile.height = 10, 135);
                }

            }
        }
       
    }
    //___________________________
    public class FrostCryoArmourProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cryo Cloud");
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
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
            Player player = Main.player[Projectile.owner];

            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(Projectile.originalDamage); //update damage

            if (Main.rand.Next(7) == 0)
            {
                float xpos = (Main.rand.NextFloat(-40, 40));
                //float ypos = (Main.rand.NextFloat(200, 250));

                int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X - xpos, Projectile.Center.Y), new Vector2(xpos * -0.05f, 10), ModContent.ProjectileType<FrostAccessProj>(), Projectile.damage, 0, Projectile.owner, 0, 0, 1);

                Main.projectile[projID].DamageType = DamageClass.Magic;
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
            if (player.dead)
            {
                Projectile.Kill();
            }
        }

  

        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
          
        }
    }

}