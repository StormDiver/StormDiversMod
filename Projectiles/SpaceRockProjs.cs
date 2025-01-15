using System;
using System.Collections.Generic;
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
    public class SpaceGlobeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Boulder");
        }
        public override void SetDefaults()
        {

            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 2;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.light = 0.4f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        int rotate;
        int opacity = 255;
        public override void AI()
        {
            rotate += 1;
            Projectile.rotation = rotate * 0.1f;
            opacity -= 10;
            Projectile.alpha = opacity;

            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            if (Projectile.ai[0] > 12f)  //this defines where the flames starts
            {
                if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
                {
                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                    //int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1.5f);
                    dust2.noGravity = true;
                    dust2.scale = 1.5f;
                    dust2.velocity *= 2;

                }
                if (Main.rand.Next(2) == 0)
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 0, 0, 10, 150, default, 0.5f);

                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 0.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 0, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 0.5f);
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            float numberProjectiles = 6 + Main.rand.Next(3);
            float rotation = MathHelper.ToRadians(180);
            //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                float speedX = 0f;
                float speedY = -8f;
                
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(180));
                float scale = 1f - (Main.rand.NextFloat() * .5f);
                perturbedSpeed = perturbedSpeed * scale;
                int projid = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<SpaceFragment>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[projid].DamageType = DamageClass.Magic;

            }
            SoundEngine.PlaySound(SoundID.Item62 with {Volume = 0.5f}, Projectile.Center);
            for (int i = 0; i < 30; i++) //Flame particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));

                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1.5f);
                Main.dust[dustIndex].noGravity = true;
            }
            for (int i = 0; i < 30; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 130, default, 1.5f);
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 0, 0, 0, 130, default, 1f);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //__________________________________________________________________________________________________________________________________________________
  
    public class SpaceArmourProj : ModProjectile
    { //For the armour set bonus
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Homing Boulder");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

        }
        public override void SetDefaults()
        {

            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.extraUpdates = 3;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.light = 0.4f;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Generic;

        }

        int rotate;
        int opacity = 255;
        public override void AI()
        {
            rotate += 1;
            Projectile.rotation = rotate * 0.1f;
            opacity -= 10;
            Projectile.alpha = opacity;
            
            var player = Main.player[Projectile.owner];

            if (Projectile.position.Y > player.position.Y)
            {
                Projectile.tileCollide = true;
            }
            else
            {
                Projectile.tileCollide = false;

            }
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            if (Projectile.ai[0] > 12f)  //this defines where the flames starts
            {
                if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
                {

                    var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                    //int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1.5f);
                    dust2.noGravity = true;
                    dust2.scale = 1.5f;
                    dust2.velocity *= 2;

                }
                if (Main.rand.Next(2) == 0)
                {
                   
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 0, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 0.5f);
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            
            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 150;
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
                Projectile.velocity = (10 * Projectile.velocity + move) / 7f;
                AdjustMagnitude(ref Projectile.velocity);
            }

        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 5f)
            {
                vector *= 5f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }
     
        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.5f }, Projectile.Center);
            for (int i = 0; i < 30; i++)
            {
                float speedX = -Projectile.velocity.X * Main.rand.NextFloat(.4f, .7f) + Main.rand.NextFloat(-16f, 16f);
                float speedY = -Projectile.velocity.Y * Main.rand.NextFloat(.4f, .7f) + Main.rand.NextFloat(-16f, 16f);
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, speedX, speedY, 130, default, 1.5f);
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 0, 0, 0, 130, default, 1f);
            }

            float numberProjectiles = 2 + Main.rand.Next(2);
            float rotation = MathHelper.ToRadians(180);
            //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                float speedX = 0f;
                float speedY = -8f;

                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(180));
                float scale = 1f - (Main.rand.NextFloat() * .5f);
                perturbedSpeed = perturbedSpeed * scale;
                int projid = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<SpaceFragment>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
                Main.projectile[projid].DamageType = DamageClass.Generic;
            }
            for (int i = 0; i < 30; i++) //Flame particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));

                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1.5f);
                Main.dust[dustIndex].noGravity = true;


            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //___________________________________________________________________
    public class SpaceFragment : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Fragment");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.aiStyle = 14;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 200;
            Projectile.light = 0.4f;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = true;
            DrawOffsetX = 0;
            DrawOriginOffsetY = -6;
        }
        int rotate;
        public override void AI()
        {
            rotate += 2;
            Projectile.rotation = rotate * 0.1f;
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            var player = Main.player[Projectile.owner];
           
            if (Projectile.ai[0] > 0f) 
            {
                if (Main.rand.Next(2) == 0) 
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; 
                    Main.dust[dust].velocity *= -0.3f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 0, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust2].noGravity = true; 
                    Main.dust[dust2].velocity *= -0.3f;
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 0, 0, 0, 130, default, 0.5f);
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 130, default, 1f);
            }
            SoundEngine.PlaySound(SoundID.Tink with { Volume = 0.5f }, Projectile.Center);
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
    //______________________________________________
    //___________________________________________________________________
    public class SpaceSwordProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asteroid Fragment");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = 0;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 200;
            Projectile.light = 0.4f;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = true;
            DrawOffsetX = 0;
            DrawOriginOffsetY = -6;
            Projectile.extraUpdates = 1;

        }
        int rotate;
        public override void AI()
        {
            rotate += 2;
            Projectile.rotation = rotate * 0.1f;
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            }
            var player = Main.player[Projectile.owner];
            if (Projectile.position.Y > (Main.MouseWorld.Y - 100))
            {
                Projectile.tileCollide = true;
            }
            else
            {
                Projectile.tileCollide = false;
            }

            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 250;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].CanBeChasedBy())
                {
                    if (Collision.CanHit(Projectile.Center, 0, 0, Main.npc[k].Center, 0, 0))
                    {
                        Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance && Projectile.position.Y < Main.npc[k].position.Y && Projectile.position.X > Main.npc[k].position.X - 60 && Projectile.position.X < Main.npc[k].position.X + 60)
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
                Projectile.tileCollide = true;

                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 9f;
                AdjustMagnitude(ref Projectile.velocity);
            }


            if (Main.rand.Next(2) == 0)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= -0.3f;
                int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 0, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity *= -0.3f;

            }

        }
        private void AdjustMagnitude(ref Vector2 vector)
        {

            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 10f)
            {
                vector *= 11f / magnitude;
            }

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = Projectile.damage * 6 / 10;

            for (int i = 0; i < 10; i++) //Flame particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -2.5f).RotatedByRandom(MathHelper.ToRadians(360));

                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1.25f);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                float speedX = -Projectile.velocity.X * Main.rand.NextFloat(.4f, .7f) + Main.rand.NextFloat(-8f, 8f);
                float speedY = -Projectile.velocity.Y * Main.rand.NextFloat(.4f, .7f) + Main.rand.NextFloat(-8f, 8f);
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, speedX, speedY, 130, default, 1.5f);
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 0, 0, 0, 130, default, 1f);
            }

            for (int i = 0; i < 20; i++) //Flame particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));

                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1.5f);
                Main.dust[dustIndex].noGravity = true;
            }
            
            SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.5f, Pitch = 0.2f }, Projectile.Center);
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
}