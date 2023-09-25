using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using StormDiversMod.Buffs;
using StormDiversMod.Items.Accessory;

namespace StormDiversMod.Projectiles
{
    public class CelestialShieldProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Celestial Guardian");
            Main.projFrames[Projectile.type] = 4;

        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.knockBack = 0;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 99999999;
            Projectile.tileCollide = false;
            Projectile.MaxUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        int rotatespeed;
        int particle;
        bool hidden;
        public override void AI()
        {
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
            }

            if (player.HasBuff(ModContent.BuffType<CelestialBuff>()))
            {
                rotatespeed = 10;
            }
            else
            {
                rotatespeed = 2;
            }
           
            Projectile.rotation += (float)Projectile.direction * -0.2f;
            //Making player variable "p" set as the projectile's owner

            //Factors for calculations
            double deg = ((double)Projectile.ai[1] + 90); //The degrees, you can multiply Projectile.ai[1] to make it orbit faster, may be choppy depending on the value
            double rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = 50; //Distance away from the player

            /*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */

            Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
            Projectile.ai[1] += -rotatespeed;

            int[] array = { 3, 4, 5, 6, 7, 8, 9 }; //idk how to do array stuff

            if (!hidden)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int choice = Main.rand.Next(4);
                    if (choice == 0)
                    {
                        particle = 244;
                    }
                    else if (choice == 1)
                    {
                        particle = 110;
                    }
                    else if (choice == 2)
                    {
                        particle = 111; ;
                    }
                    else if (choice == 3)
                    {
                        particle = 112;
                    }
                    var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, particle);
                    dust.noGravity = true;

                }
            }
            if (player.GetModPlayer<EquipmentEffects>().lunarBarrier == false || player.dead)

            {
                if (!hidden)
                {
                    for (int i = 0; i < 10; i++)
                    {

                        var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 110);
                        var dust2 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 111);
                        dust2.noGravity = true;

                        var dust3 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 112);
                        dust3.noGravity = true;

                        var dust4 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 244);
                        dust4.noGravity = true;

                    }
                }
                Projectile.Kill();
                return;
            }
            
            AnimateProjectile();
        
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
        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) // This will change the sprite every 5 frames
            {
                Projectile.frame++;
                Projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                Projectile.frameCounter = 0;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }
    }
    
}
