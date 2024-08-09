using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using StormDiversMod.Buffs;
using StormDiversMod.Items.Accessory;
using System.Linq;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Utilities;

namespace StormDiversMod.Projectiles
{
    public class CelestialShieldProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Celestial Guardian");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.knockBack = 0;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 99999999;
            Projectile.tileCollide = false;
            Projectile.MaxUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.scale = 1;
        }
        int rotatespeed;
        int particle;
        bool hidden;
        int accesstype = 3; //start at slot 3 (Accessory 1)
        Vector2 dustposition;
        double degrees;
        int dustchoice;
        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            Projectile.velocity = player.velocity;
            for (int i = 0; i < 7; i++) //Go through all 7 accessory slots (slot 3 - 8)
            {
                if (player.armor[accesstype].type != ModContent.ItemType<Celestialshield>()) //is the celestial barrier in the slot?
                {
                    accesstype++; //if not, check next slot
                }
                else if (player.hideVisibleAccessory[accesstype] && player.armor[accesstype].type == ModContent.ItemType<Celestialshield>()) //if so, is the slot set to be hidden?
                {
                    hidden = true; //if so, hide sprite and disable dust
                    Projectile.hide = true;
                }
                else if (!player.hideVisibleAccessory[accesstype] && player.armor[accesstype].type == ModContent.ItemType<Celestialshield>()) //if not show sprite and dust
                {
                    hidden = false;
                    Projectile.hide = false;
                }
            }
            if (accesstype >= 9) //after slot 7 go back to slot 1
                accesstype = 3;
            //Main.NewText("Slot: " + accesstype, 220, 63, 139);

            /*
             var player = Main.player[Projectile.owner];
             if ((player.hideVisibleAccessory[3] && player.armor[3].type == ModContent.ItemType<Celestialshield>()) ||
             (player.hideVisibleAccessory[4] && player.armor[4].type == ModContent.ItemType<Celestialshield>()) ||
             (player.hideVisibleAccessory[5] && player.armor[5].type == ModContent.ItemType<Celestialshield>()) ||
             (player.hideVisibleAccessory[6] && player.armor[6].type == ModContent.ItemType<Celestialshield>()) ||
             (player.hideVisibleAccessory[7] && player.armor[7].type == ModContent.ItemType<Celestialshield>()) ||
             (player.hideVisibleAccessory[8] && player.armor[8].type == ModContent.ItemType<Celestialshield>()) ||
             (player.hideVisibleAccessory[9] && player.armor[9].type == ModContent.ItemType<Celestialshield>()))
             {
                 hidden = true;
                 Projectile.hide = true;
             }
             else
             {
                 hidden = false;
                 Projectile.hide = false;
             }*/

            if (player.HasBuff(ModContent.BuffType<CelestialBuff>()))
            {
                rotatespeed = 10;
            }
            else
            {
                rotatespeed = 2;
            }

            Projectile.rotation -= 0.1745f * Projectile.direction;

            //Factors for orbiting player
            double deg = ((double)Projectile.ai[1] + 90); //The degrees, you can multiply Projectile.ai[1] to make it orbit faster, may be choppy depending on the value
            double rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = 75; //Distance away from the player

            Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

            Projectile.ai[1] += -rotatespeed;
            //dust effects            

            degrees -= 10;
            for (int i = 0; i < 4; i++)
            {
                if (!hidden)
                {
                    if (dustchoice >= 4)//reset chocie back to 1 when limit reached
                        dustchoice = 0;
                    //Main.NewText("Dust: " + dustchoice, 220, 63, 139);

                    double rad2 = degrees * (Math.PI / 180); //Convert degrees to radians
                    double dist2 = 15; //Distance away from the center of proj

                    dustposition.X = Projectile.Center.X - (int)(Math.Cos(rad2) * dist2);
                    dustposition.Y = Projectile.Center.Y - (int)(Math.Sin(rad2) * dist2);

                    for (int j = 0; j < 3; j++)
                    {
                        if (dustchoice == 0) //Stardust
                            particle = 111;
                        else if (dustchoice == 1) //Solar
                            particle = 174;
                        else if (dustchoice == 2) //Nebula
                            particle = 112;
                        else if (dustchoice == 3) //Vortex
                            particle = 110;

                        float X = dustposition.X - Projectile.velocity.X / 5f * (float)j;
                        float Y = dustposition.Y - Projectile.velocity.Y / 5f * (float)j;

                        int dust = Dust.NewDust(new Vector2(X, Y), 1, 1, particle, 0, 0, 100, default, 0.9f);

                        Main.dust[dust].position.X = X;
                        Main.dust[dust].position.Y = Y;
                        Main.dust[dust].noGravity = true;

                        Main.dust[dust].velocity *= 0;
                    }
                    dustchoice++; //each side gets its own dust
                    degrees += 90; //for dust on other side
                }
            }
          
            if (player.GetModPlayer<EquipmentEffects>().lunarBarrier == false || player.dead)
            {
                if (!hidden)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 110);
                        dust.noGravity = true;

                        var dust2 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 111);
                        dust2.noGravity = true;

                        var dust3 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 112);
                        dust3.noGravity = true;

                        var dust4 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 174);
                        dust4.noGravity = true;
                    }
                }
                Projectile.Kill();
                return;
            }
                    
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                //Main.PlaySound(SoundID.Item14, Projectile.position);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.White;
            color.A = 175;
            return color;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }

    }
    
}
