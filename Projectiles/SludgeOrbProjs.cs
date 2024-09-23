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
    public class SludgeOrbProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Large Sludge Orb");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.ignoreWater = true;
            Projectile.alpha = 100;
            Projectile.light = 0.3f;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation += 0.01f * Projectile.direction;

            AnimateProjectile();

            Dust dust = Terraria.Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 39, Projectile.velocity.X * 1, Projectile.velocity.Y * 1, 150, new Color(255, 255, 255), 1f);
            dust.noGravity = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SludgedDebuff>(), 240);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<SludgedDebuff>(), 240);
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
                SoundEngine.PlaySound(SoundID.Item86, Projectile.Center);
                for (int i = 0; i < 25; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 39);
                    dust.alpha = 150;
                }
            }

            int numberProjectiles = 4 + Main.rand.Next(2); //This defines how many projectiles to shot (4-5)
            for (int i = 0; i < numberProjectiles; i++)
            {
                float speedX = 0f;
                float speedY = -6f;

                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(170));
                float scale = 1f - (Main.rand.NextFloat() * .7f);
                perturbedSpeed = perturbedSpeed * scale;
                int ProjID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<SludgeOrbProj2>(), (int)(Projectile.damage * 0.33f), 0, Projectile.owner);
            }
        }
        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor) => null;
    }
    //_____________________________________________
    public class SludgeOrbProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Venom Sludge");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 360;
            Projectile.aiStyle = 2;
            Projectile.ignoreWater = true;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
            Projectile.usesIDStaticNPCImmunity = true; //static iframes at first
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.ArmorPenetration = 8;
            Projectile.alpha = 100;
            Projectile.light = 0.2f;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
        public override bool? CanDamage() => true;
       
        bool stick;
        public override void AI()
        {
            if (!stick)
            {
                Projectile.rotation += 0.01f * Projectile.direction;
                if (Main.rand.Next(4) == 0)
                {
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Dust dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 39, 0f, 0f, 150, new Color(255, 255, 255), 1f)];
                    dust.noGravity = true;
                }
            }
            if (stick)
            {
                //Projectile.usesIDStaticNPCImmunity = false;
                //Projectile.usesLocalNPCImmunity = true; //switch to local
                //Projectile.localNPCHitCooldown = 15;
                Projectile.idStaticNPCHitCooldown = 5;

                if (Main.rand.Next(10) == 0)
                {
                    Dust dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 39, 0f, 0f, 150, new Color(255, 255, 255), 0.75f)];
                    dust.velocity *= 0;
                }
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                Projectile.knockBack = 0;
            }
            AnimateProjectile();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //Projectile.damage = (Projectile.damage * 85) / 100;
            target.AddBuff(ModContent.BuffType<SludgedDebuff>(), 60);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<SludgedDebuff>(), 60);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!stick)
            {
                Projectile.penetrate = 3;
                for (int i = 0; i < 5; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 39);
                    dust.alpha = 150;
                    SoundEngine.PlaySound(SoundID.NPCDeath1 with { Volume = 0.5f, Pitch = 0.2f }, Projectile.Center);
                }
            }
            stick = true;
            return false;
        }
        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6) 
            {
                Projectile.frame++;
                Projectile.frame %= 4; 
                Projectile.frameCounter = 0;
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 5; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 39);
                    dust.alpha = 150;
                }
            }
        }
    }
    //==============================================================================================================================
    public class SludgeVenomProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Large Sludge Orb");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.ignoreWater = true;
            Projectile.alpha = 50;
            Projectile.light = 0.3f;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation += 0.01f * Projectile.direction;

            AnimateProjectile();

            Dust dust = Terraria.Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 118, Projectile.velocity.X * 1, Projectile.velocity.Y * 1, 150, new Color(255, 255, 255), 1f);
            dust.noGravity = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SludgedVenomDebuff>(), 240);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<SludgedVenomDebuff>(), 240);
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
                SoundEngine.PlaySound(SoundID.Item86, Projectile.Center);
                for (int i = 0; i < 25; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 118);
                    dust.alpha = 150;
                }
            }

            int numberProjectiles = 6 + Main.rand.Next(2); //This defines how many projectiles to shot (6-7)
            for (int i = 0; i < numberProjectiles; i++)
            {
                float speedX = 0f;
                float speedY = -7f;

                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(170));
                float scale = 1f - (Main.rand.NextFloat() * .7f);
                perturbedSpeed = perturbedSpeed * scale;
                int ProjID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<SludgeVenomProj2>(), (int)(Projectile.damage * 0.33f), 0, Projectile.owner);
            }
        }
        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor) => null;
    }
    //_____________________________________________
    public class SludgeVenomProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Venom Sludge");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 360;
            Projectile.aiStyle = 2;
            Projectile.ignoreWater = true;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
            Projectile.usesIDStaticNPCImmunity = true; //static iframes at first
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.ArmorPenetration = 8;
            Projectile.alpha = 50;
            Projectile.light = 0.2f;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
        public override bool? CanDamage() => true;

        bool stick;
        public override void AI()
        {
            if (!stick)
            {
                Projectile.rotation += 0.01f * Projectile.direction;
                if (Main.rand.Next(4) == 0)
                {
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Dust dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 118, 0f, 0f, 150, new Color(255, 255, 255), 1f)];
                    dust.noGravity = true;
                }
            }
            if (stick)
            {
                //Projectile.usesIDStaticNPCImmunity = false;
                //Projectile.usesLocalNPCImmunity = true; //switch to local
                //Projectile.localNPCHitCooldown = 15;
                Projectile.idStaticNPCHitCooldown = 5;
                if (Main.rand.Next(10) == 0)
                {
                    Dust dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 118, 0f, 0f, 150, new Color(255, 255, 255), 0.75f)];
                    dust.velocity *= 0;
                }
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
                Projectile.knockBack = 0;
            }
            AnimateProjectile();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //stick = true;
            //Projectile.damage = (Projectile.damage * 85) / 100;
            target.AddBuff(ModContent.BuffType<SludgedVenomDebuff>(), 60);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<SludgedVenomDebuff>(), 60);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!stick)
            {
                Projectile.penetrate = 3;
                for (int i = 0; i < 5; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 118);
                    dust.alpha = 150;
                    SoundEngine.PlaySound(SoundID.NPCDeath1 with { Volume = 0.5f, Pitch = 0.2f }, Projectile.Center);
                }
            }
            stick = true;
            return false;
        }
        public void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.frameCounter = 0;
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 5; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 118);
                    dust.alpha = 150;
                }
            }
        }
    }
}
