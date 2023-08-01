using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace StormDiversMod.Projectiles       //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class ChloroStaffProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Chlorophyte Stream");
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        Vector2 mousepos;
        public override void OnSpawn(IEntitySource source)
        {
            mousepos = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y); //Set position for 1 frame
        }
        public override void AI()
        {
            
            if (Projectile.ai[0] > 7f)  //this defines where the flames starts
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 273, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1.5f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= -0.3f;
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 44);
                dust2.noGravity = true;
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            if (Vector2.Distance(Projectile.Center, mousepos) <= 7)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;

                Projectile.Kill();
            }
            
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Venom, 300);
            Projectile.damage = (Projectile.damage * 9) / 10;

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(BuffID.Venom, 300);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.5f, Pitch = 0.5f }, Projectile.Center);

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionChloroProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1.25f;

            if (Projectile.owner == Main.myPlayer)
            {
                int numberProjectiles = 6 + Main.rand.Next(3);
                for (int i = 0; i < numberProjectiles; i++)
                {
                    float speedX = Main.rand.NextFloat(.4f, .7f) + Main.rand.NextFloat(-3.5f, 3.5f);
                    float speedY = Main.rand.NextFloat(.4f, .7f) + Main.rand.NextFloat(-3.5f, 3.5f);

                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(speedX, speedY),
                         ProjectileID.SporeCloud, (int)(Projectile.damage * 0.8f), 0.5f, Projectile.owner);

                    Main.projectile[projID].usesIDStaticNPCImmunity = true;
                    Main.projectile[projID].idStaticNPCHitCooldown = 10;
                    Main.projectile[projID].DamageType = DamageClass.Magic;
                    Main.projectile[projID].extraUpdates = 1;

                }
            }
            for (int i = 0; i < 60; i++) 
            {
                Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 273, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 1f + (float)Main.rand.Next(5) * 0.2f;
                dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
            }
        }
    }
    
}