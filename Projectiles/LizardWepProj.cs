using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
using StormDiversMod.Buffs;
using Terraria.GameContent;
using Terraria.Audio;


namespace StormDiversMod.Projectiles
{

    public class LizardSpinnerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lihzahrd Spinner");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {

            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 3600;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.light = 0.4f;
        }
     
        float movespeed = 25f; //Speed of the proj

        Vector2 mousepos;

        float projsleft = 6.5f; //extra count for when spawned

        int proj = ModContent.ProjectileType<Projectiles.LizardSpinnerProj>();
        public override void AI()
        {
            Projectile.ai[1]++;

            if (Main.rand.Next(5) == 0)
            {
                Vector2 perturbedSpeed = new Vector2(0, -2.5f).RotatedByRandom(MathHelper.ToRadians(360));

                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 170, perturbedSpeed.X, perturbedSpeed.Y, 255, default, 1f);
                Main.dust[dustIndex].noGravity = true;
            }

            Projectile.rotation = Projectile.ai[1] * 0.08f;


            //movement
            if (Projectile.ai[1] == 1)
            {
                mousepos = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y); //Set position for 1 frame
            }

            if (Projectile.ai[1] > 60)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;

            }

            float distanceX = mousepos.X - Projectile.Center.X;
            float distanceY = mousepos.Y - Projectile.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));

            movespeed = distance / 30;
            
            Vector2 moveTo = mousepos;
            Vector2 move = moveTo - Projectile.Center + new Vector2(0, 0); //Postion around cursor
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);

            if (magnitude > movespeed)
            {
                move *= movespeed / magnitude;
            }

            Projectile.velocity = move;

            var player = Main.player[Projectile.owner];

            if (Projectile.timeLeft < 60) //Destroy aniamtion
            {
               

                Projectile.position = Projectile.Center;
                Projectile.Center = Projectile.position;
                if (Projectile.scale > 0)
                {
                    Projectile.scale -= 0.02f;
                }
                //Projectile.velocity.X = 0;
                //Projectile.velocity.Y = 0;
            }

            /*if (player.ownedProjectileCounts[proj] >= 10 && player.controlUseItem && Projectile.ai[1] > 60)
            {

                Projectile.Kill();

            }*/

            if (player.controlUseTile && Projectile.ai[1] > 60 && player.HeldItem.type == ModContent.ItemType<Items.Weapons.LizardSpinner>() || player.dead) 
            {
                if (Projectile.timeLeft > 60)
                {
                    Projectile.timeLeft = 60;
                }
            }
            if (player.itemAnimation == 1 && player.HeldItem.type == ModContent.ItemType<Items.Weapons.LizardSpinner>()) 
            {          
                projsleft -= 0.5f; //half because extra updates
            }
            if (projsleft == 0)
            {
                Projectile.timeLeft = 60;
            }
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.velocity.X *= 0.1f;
            Projectile.velocity.Y *= 0.1f;

            Projectile.damage = (Projectile.damage * 9) / 10;

            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(target.position, target.width, target.height, 170);
                dust.scale = 1f;
                dust.noGravity = true;

            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;
            return false;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74 with { Volume = 0.5f, Pitch = 0.5f }, Projectile.Center);

            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 170);
                dust.scale = 1.5f;
                
                dust.noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);

            Texture2D texture2 = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture2.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture2, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }

    }
    //____________________________________________________
    public class LizardFlameProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lihzahrd Flame");
        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 125;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            
            if (Projectile.ai[0] > 8f)  //this defines where the flames starts
            {
                if (Main.rand.Next(1) == 0)     //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f, 130, default, 3f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= 2.5f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f); //this defines the flames dust and color parcticles, like when they fall thru ground, change DustID to wat dust you want from Terraria
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            return;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(1) == 0) // the chance
            {
                
                    target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 300);

            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 300);
        }
        int reflect = 3;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();
            }
            {

                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 1f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 1f;
                }
            }
            return false;
        }
    }
    //_____________________________________________
    public class LizardSpellProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lihzahrd Fire Orb");
        }
        public override void SetDefaults()
        {

            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            //aiType = ProjectileID.Meteor1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 1;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Main.rand.Next(1) == 0)
            {

                Dust dust;
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 0, new Color(255, 255, 255), .8f);
                dust.noGravity = true;
                dust.scale = 1.5f;

            }
            Projectile.ai[1]++;

            Projectile.rotation = Projectile.ai[1] * 0.2f;
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            Projectile.damage = (Projectile.damage * 19) / 20;
            target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 300);

        }
        public override void OnHitPvp(Player target, int damage, bool crit)

        {
            target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 300);
        }
        int reflect = 4;
        public override bool OnTileCollide(Vector2 oldVelocity)

        {
            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();

            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.8f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
            }
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10 with { Volume = 1f, Pitch = -0.5f }, Projectile.Center);


            return false;
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {


                for (int i = 0; i < 25; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 0, default, 2f);

                    Main.dust[dustIndex].noGravity = false;
                    Main.dust[dustIndex].velocity *= 2;

                }

            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }

}