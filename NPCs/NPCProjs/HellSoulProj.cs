using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;


namespace StormDiversMod.NPCs.NPCProjs
{
   
    public class HellSoulProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Soul");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;

        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.light = 0.4f;

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = 1;


            Projectile.tileCollide = false;
            Projectile.scale = 1.1f;

            Projectile.alpha = 150;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 600;
            //aiType = ProjectileID.LostSoulHostile;
            Projectile.aiStyle = -1;
            // Projectile.CloneDefaults(452);
            //aiType = 452;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
        }


        readonly int homerandom = Main.rand.Next(180, 300);
        public override void AI()
        {
            var dust3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0);
            dust3.noGravity = true;
            Projectile.ai[0]++;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Projectile.ai[0] <= homerandom)
            {
                Projectile.velocity.X *= 0.98f;
                Projectile.velocity.Y *= 0.98f;
            }

            if (Projectile.ai[0] == homerandom)
            {
                for (int i = 0; i < 20; i++)
                {

                    int dust2 = Dust.NewDust(Projectile.Center - Projectile.velocity, Projectile.width, Projectile.height, 5, 0f, 0f, 50, default, 1f);
                    Main.dust[dust2].noGravity = true;

                }
            }
            if (Projectile.ai[0] >= homerandom && Projectile.ai[0] <= homerandom + 45)
            {
                for (int i = 0; i < 200; i++)
                {
                    Player target = Main.player[i];
                    //If the npc is hostile

                    //Get the shoot trajectory from the Projectile and target
                    float shootToX = target.Center.X - Projectile.Center.X;
                    float shootToY = target.Center.Y - Projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    //If the distance between the live targeted npc and the Projectile is less than 480 pixels
                    if (distance < 2000f && target.active)
                    {

                        distance = 0.5f / distance;

                        //Multiply the distance by a multiplier proj faster
                        shootToX *= distance * 5f;
                        shootToY *= distance * 5f;

                        //Set the velocities to the shoot values
                        Projectile.velocity.X = shootToX;
                        Projectile.velocity.Y = shootToY;
                    }

                }
                

            }

        }
     
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.SuperBurnDebuff>(), 300);

            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {

                int dust2 = Dust.NewDust(Projectile.Center - Projectile.velocity, Projectile.width, Projectile.height, 5, 0f, 0f, 50, default, 1f);

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
        /*public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }*/

    }
}
