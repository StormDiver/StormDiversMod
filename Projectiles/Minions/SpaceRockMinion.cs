using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Items.Pets;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Projectiles.Minions
{
    public class SpaceRockMinionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asteroid Minion");
            Description.SetDefault("A mini Asteroid will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<SpaceRockMinionProj>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
    

    //CCOPY Spazmini MINION AI
    public class SpaceRockMinionProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asteroid Minion");
            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[Projectile.type] = 4;
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            // These below are needed for a minion
            // Denotes that this Projectile is a pet or minion
            Main.projPet[Projectile.type] = true;
            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;


            Projectile.DamageType = DamageClass.Summon;

        }

        public sealed override void SetDefaults()
        {
            // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.friendly = true;
            // Only determines the damage type
            Projectile.minion = true;
            // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.minionSlots = 1f;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.penetrate = -1;

            Projectile.CloneDefaults(388);
            AIType = 388;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        int dustspeed;
        int projspeed;
        public override void AI()
        {
            projspeed++;
            Projectile.minionSlots = 1f;
            Player player = Main.player[Projectile.owner];
            // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<SpaceRockMinionBuff>());
            }
            if (player.HasBuff(BuffType<SpaceRockMinionBuff>()))
            {
                Projectile.timeLeft = 2;
            }
            Projectile.width = 38;
            Projectile.height = 22;
            Projectile.Opacity = 1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;


           
            if (Projectile.velocity.X > 6 || Projectile.velocity.X < -6 || Projectile.velocity.Y > 6 || Projectile.velocity.Y < -6)
            {
                dustspeed = 1;
            }
            else
            {
                dustspeed = 5;
            }
            if (Main.rand.Next(dustspeed) == 0)     //this defines how many dust to spawn
            {

                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6);
                //int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1.5f);
                dust2.noGravity = true;
                dust2.scale = 1.5f;
                dust2.velocity *= 1;

            }
     
            int frameSpeed = 20;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //NPC.immuneTime = 10;

            //Projectile.velocity.X *= 0.5f;
            //Projectile.velocity.Y *= 0.5f;
            for (int i = 0; i < 20; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 130, default, 1.5f);
                dust.noGravity = true;
            }
            if (projspeed >= 90)
            {
                if (Main.rand.Next(2) == 0)
                {
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(target.Right.X + 200, target.Center.Y - 200), new Vector2(-15, 15f), ModContent.ProjectileType<SpaceRockMinionProj2>(), (int)(Projectile.damage * 0.5f), 0, Main.myPlayer);

                }
                else
                {
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(target.Right.X - 200, target.Center.Y - 200), new Vector2(15, 15f), ModContent.ProjectileType<SpaceRockMinionProj2>(), (int)(Projectile.damage * 0.5f), 0, Main.myPlayer);

                }
                projspeed = 0;
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 0, 0, 0, 130, default, 0.5f);
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 130, default, 1f);
            }
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 62, 0.5f, 0.2f);

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //__________________________________________________________________________________________________________________________________________________
    public class SpaceRockMinionProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asteroid Minion Fragment");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;

        }
        public override void SetDefaults()
        {

            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 14;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            Projectile.light = 0.4f;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = true;
            DrawOffsetX = 0;
            DrawOriginOffsetY = -6;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            

        }
        int rotate;
        public override void AI()
        {

            var player = Main.player[Projectile.owner];

            if (Projectile.position.Y > (player.position.Y - 200))
            {
                Projectile.tileCollide = true;
            }
            else
            {
                Projectile.tileCollide = false;

            }
            rotate += 2;
            Projectile.rotation = rotate * 0.1f;
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f, ((255 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors
            if (Projectile.timeLeft > 125)
            {
                Projectile.timeLeft = 125;
            }
            if (Projectile.ai[0] > 0f)  //this defines where the flames starts
            {
                if (Main.rand.Next(2) == 0)     //this defines how many dust to spawn
                {


                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].velocity *= -0.3f;
                    int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 0, Projectile.velocity.X, Projectile.velocity.Y, 130, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust2].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust2].velocity *= -0.3f;

                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }

        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        public override void Kill(int timeLeft)
        {

            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 0, 0, 0, 130, default, 0.5f);
                var dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 130, default, 1f);
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
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}