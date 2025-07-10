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
    //Would've had all these as once proj but sadly ammo shoot code ignores the shoot hook //_-)
    //__________________________________________________________________________________
    public class GemAmethystBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Amethyst Bullet");
            //ProjectileID.Sets.Explosive[Type] = true;
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
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        int mineamount;

        int totalmine = 2;
        int pickpower = 40;
        int dustype = 27;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        public override void AI()
        {
            /*
            switch (Projectile.ai[2])
            {
                case 0: //amey
                    totalmine = 1;
                    pickpower = 40;
                    dusttype = 27;
                    break;
                case 1: //topz
                    totalmine = 1;
                    pickpower = 50; 
                    dusttype = 246;
                    break;
                case 2: //sapp
                    totalmine = 2;
                    pickpower = 50; 
                    dusttype = 172;
                    break;
                case 3: //emem
                    totalmine = 2;
                    pickpower = 55;
                    dusttype = 299;
                    break;
                case 4: //ruby
                    totalmine = 3;
                    pickpower = 50;
                    dusttype = 60;
                    break;
                case 5: //ambr
                    totalmine = 3;
                    pickpower = 55;
                    dusttype = 55;
                    break;
                case 6: //diam
                    totalmine = 3;
                    pickpower = 55;
                    dusttype = 247;
                    break;
            }
            */
            Player player = Main.player[Projectile.owner];
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
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 0.6f, Pitch = 0.5f }, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //__________________________________________________________________________________
    public class GemTopazBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Topaz Bullet");
            //ProjectileID.Sets.Explosive[Type] = true;
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
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        int mineamount;

        int totalmine = 2;
        int pickpower = 50;
        int dustype = 246;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
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
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 0.6f, Pitch = 0.5f }, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //__________________________________________________________________________________
    public class GemSapphireBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sapphire Bullet");
            //ProjectileID.Sets.Explosive[Type] = true;
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
        int mineamount;

        int totalmine = 3;
        int pickpower = 45;
        int dustype = 172;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
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
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 0.6f, Pitch = 0.5f }, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //__________________________________________________________________________________
    public class GemEmeraldBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Emerald Bullet");
            //ProjectileID.Sets.Explosive[Type] = true;
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
        int mineamount;

        int totalmine = 3;
        int pickpower = 55;
        int dustype = 299;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
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
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 0.6f, Pitch = 0.5f }, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //__________________________________________________________________________________
    public class GemRubyBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ruby Bullet");
            //ProjectileID.Sets.Explosive[Type] = true;
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
        int mineamount;

        int totalmine = 4;
        int pickpower = 50;
        int dustype = 60;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
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
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
                dust.scale = 1.5f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 0.6f, Pitch = 0.5f }, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //__________________________________________________________________________________
    public class GemAmberBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Amber Bullet");
            //ProjectileID.Sets.Explosive[Type] = true;
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
        int mineamount;

        int totalmine = 4;
        int pickpower = 55;
        int dustype = 55;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
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
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 0.6f, Pitch = 0.5f }, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //__________________________________________________________________________________
    public class GemDiamondBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Diamond Bullet");
            //ProjectileID.Sets.Explosive[Type] = true;
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
        int mineamount;

        int totalmine = 4;
        int pickpower = 55;
        int dustype = 247;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            int xtilepos = (int)(Projectile.Center.X) / 16;
            int ytilepos = (int)(Projectile.Center.Y) / 16;

            Tile tilepos = Main.tile[xtilepos, ytilepos];

            if (tilepos.HasTile /*&& Main.tileSolid[tilepos.TileType]*/ && !Main.tileAxe[tilepos.TileType] && !Main.tileHammer[tilepos.TileType]) //once it passes over a tile that isn't a tree or wall, break it
            {
                Projectile.velocity *= 0.5f; //slow down after first hit
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
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 0.6f, Pitch = 0.5f }, Projectile.position);
            for (int i = 0; i < 10; i++)
            {    
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustype);
                dust.noGravity = true;
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
    