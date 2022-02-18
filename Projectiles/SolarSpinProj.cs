using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;

namespace StormDiversMod.Projectiles     //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class SolarSpinProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Spinner");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
           
            Projectile.width = 260;     
            Projectile.height = 260;      
            Projectile.friendly = true;    
            Projectile.penetrate = -1;    
            Projectile.tileCollide = false; 
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            //Projectile.scale = 1.2f;


        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            {
                target.AddBuff(BuffID.Daybreak, 600);

            }

            for (int i = 0; i < 10; i++)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(20, 20), Main.rand.NextFloat(-20, -20));
                var dust = Dust.NewDustDirect(target.position, target.width, target.height, 6);
                dust.scale = 2f;
                dust.noGravity = true;
                SoundEngine.PlaySound(SoundID.Item, (int)target.Center.X, (int)target.Center.Y, 74);
            }

        }
        // float hitbox = 300;
        //bool hitboxup;
        //bool hitboxdown;

        public override void AI()
        {

            
            //-------------------------------------------------------------Sound-------------------------------------------------------
            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 116);    
                Projectile.soundDelay = 45;    
            }
            //-----------------------------------------------How the projectile works---------------------------------------------------------------------
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
                    Projectile.Kill();
                }
            }
            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0f);     //this is the projectile light color R, G, B (Red, Green, Blue)
            Projectile.Center = player.MountedCenter;
            Projectile.position.X += player.width / 2 * player.direction;
            
            Projectile.spriteDirection = player.direction;
            
            Projectile.rotation += 0.2f * player.direction; //this is the projectile rotation/spinning speed
           
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = Projectile.rotation;

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);  //this is the dust that this projectile will spawn
            Main.dust[dust].velocity /= 1f;
            Main.dust[dust].scale = 2f;
            Main.dust[dust].noGravity = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
           /* if (hitbox == 300)
            {
                hitboxup = false;
                hitboxdown = true;
            }
            if (hitbox == 270)
            {
                hitboxup = true;
                hitboxdown = false;
            }
            if (hitboxup == true)
            {
                for (int i = 0; i < 10; i++)
                {
                    hitbox++;
                }
            }
            if (hitboxdown == true)
            {
                hitbox--;
            }
            Projectile.width = (int)hitbox;
            Projectile.height = (int)hitbox;*/
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;

        }
   
        public override Color? GetAlpha(Color lightColor)
        {

                Color color = Color.White;
                color.A = 75;
                return color;

        }
    }
    public class SelenianReflect : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        
    
        bool reflected;
        public override void AI(Projectile projectile)
        {
            var player = Main.player[projectile.owner];

            //var projreflect = Main.projectile[mod.ProjectileType("SelenianBladeProj")];

            //if (Main.LocalPlayer.HasBuff(BuffType<SelenianBuff>()))
            if (player.itemAnimation > 1 && Main.mouseLeft && player.HeldItem.type == ModContent.ItemType<Items.Weapons.LunarSolarSpin>())
            {

                if (projectile.hostile)
                {
                    if (
                        projectile.aiStyle == 0 ||
                        projectile.aiStyle == 1 ||
                        projectile.aiStyle == 2


                        )
                    {
                        //Player player = Main.player[npc.target];

                        float distanceX = player.Center.X - projectile.Center.X;
                        float distanceY = player.Center.Y - projectile.Center.Y;
                        float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if (distance <= 100 && !reflected)
                            {

                                int choice = Main.rand.Next(2);
                                if (choice == 0)
                                {
                                    SoundEngine.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 56);
                                    //Projectile.Kill();
                                    projectile.velocity.X *= -1f;


                                    projectile.velocity.Y *= -1f;

                                    projectile.friendly = true;
                                    projectile.hostile = false;

                                    projectile.damage *= 4;
                                    reflected = true;
                                }
                                else
                                {
                                    reflected = true;
                                }
                            }

                        }
                    }
                }

                
            }
        }
    }
}