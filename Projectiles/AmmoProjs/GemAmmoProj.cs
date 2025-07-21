using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.DataStructures;
using StormDiversMod.Items.Ammo;

namespace StormDiversMod.Projectiles.AmmoProjs
{
    //__________________________________________________________________________________
    public class GemAmmoBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gem Bullet");
            //ProjectileID.Sets.Explosive[Type] = true;
            Main.projFrames[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;

            Projectile.aiStyle = 1;
            Projectile.light = 0.2f;

            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            DrawOffsetX = -5;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 1;
        }
        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        int mineamount;
        int totalmine;
        int pickpower;
        int dusttype;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            switch (Projectile.ai[2])
            {
                case 0: //amey
                    totalmine = 2;
                    pickpower = 40;
                    dusttype = 27;
                    break;
                case 1: //topz
                    totalmine = 2;
                    pickpower = 50; 
                    dusttype = 246;
                    break;
                case 2: //sapp
                    totalmine = 3;
                    pickpower = 50; 
                    dusttype = 172;
                    break;
                case 3: //emem
                    totalmine = 3;
                    pickpower = 55;
                    dusttype = 299;
                    break;
                case 4: //ruby
                    totalmine = 4;
                    pickpower = 50;
                    dusttype = 60;
                    break;
                case 5: //ambr
                    totalmine = 4;
                    pickpower = 55;
                    dusttype = 55;
                    break;
                case 6: //diam
                    totalmine = 4;
                    pickpower = 55;
                    dusttype = 247;
                    break;
            }
            Projectile.frame = (int)Projectile.ai[2];

            int xtilepos = (int)(Projectile.Center.X) / 16;
            int ytilepos = (int)(Projectile.Center.Y) / 16;

            Tile tilepos = Main.tile[xtilepos, ytilepos];

            if (tilepos.HasTile /*&& Main.tileSolid[tilepos.TileType]*/ && !Main.tileAxe[tilepos.TileType] && !Main.tileHammer[tilepos.TileType]) //once it passes over a tile that isn't a tree or wall, break it
            {
                Projectile.velocity *= 0.5f;
                player.PickTile((int)(xtilepos), (int)(ytilepos), pickpower);

                mineamount += 1;
            }
            if (mineamount >= totalmine)
                Projectile.Kill();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dusttype);
                dust.noGravity = true;
                if (dusttype == 60)
                    dust.scale = 1.5f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 0.6f, Pitch = 0.5f }, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dusttype);
                dust.noGravity = true;
                if (dusttype == 60)
                    dust.scale = 1.5f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(-5, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                    color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
    //__________________________________________________________________________________
    //Ammo shoot code ignores the shoot hook so this method works to have them all share the same projectile
    public class GemAmethystBulletProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 1;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            int projid = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Projectile.velocity.X, Projectile.velocity.Y), 
                ModContent.ProjectileType<GemAmmoBulletProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 0);
            Projectile.Kill();
        }
    }
    //__________________________________________________________________________________
    public class GemTopazBulletProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 1;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            int projid = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Projectile.velocity.X, Projectile.velocity.Y),
                ModContent.ProjectileType<GemAmmoBulletProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 1);
            Projectile.Kill();
        }
    }
    //__________________________________________________________________________________
    public class GemSapphireBulletProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 1;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            int projid = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Projectile.velocity.X, Projectile.velocity.Y),
                ModContent.ProjectileType<GemAmmoBulletProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 2);
            Projectile.Kill();
        }
    }
    //__________________________________________________________________________________
    public class GemEmeraldBulletProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 1;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            int projid = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Projectile.velocity.X, Projectile.velocity.Y),
                ModContent.ProjectileType<GemAmmoBulletProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 3);
            Projectile.Kill();
        }
    }
    //__________________________________________________________________________________
    public class GemRubyBulletProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 1;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            int projid = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Projectile.velocity.X, Projectile.velocity.Y),
                ModContent.ProjectileType<GemAmmoBulletProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 4);
            Projectile.Kill();
        }
    }
    //__________________________________________________________________________________
    public class GemAmberBulletProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 1;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            int projid = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Projectile.velocity.X, Projectile.velocity.Y),
                ModContent.ProjectileType<GemAmmoBulletProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 5);
            Projectile.Kill();
        }
    }
    //__________________________________________________________________________________
    public class GemDiamondBulletProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 1;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            int projid = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Projectile.velocity.X, Projectile.velocity.Y),
                ModContent.ProjectileType<GemAmmoBulletProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 6);
            Projectile.Kill();
        }
    }
}