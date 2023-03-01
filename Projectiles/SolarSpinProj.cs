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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
           
            Projectile.width = 200;     
            Projectile.height = 200;      
            Projectile.friendly = true;    
            Projectile.penetrate = -1;    
            Projectile.tileCollide = false; 
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
            //Projectile.ContinuouslyUpdateDamage = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 9999999;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            {
                target.AddBuff(BuffID.Daybreak, 600);

            }

            for (int i = 0; i < 10; i++)
            {
                 
                var dust = Dust.NewDustDirect(target.position, target.width, target.height, 6);
                dust.scale = 2f;
                dust.noGravity = true;
                SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
            }

        }
       

        public override void AI()
        {

            //-------------------------------------------------------------Sound-------------------------------------------------------
            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item116, Projectile.Center);    
                Projectile.soundDelay = 60;    
            }
            //-----------------------------------------------How the projectile works---------------------------------------------------------------------
            Player player = Main.player[Projectile.owner];

            Projectile.damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(Projectile.originalDamage); //update damage


            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.dead)
                {
                    Projectile.Kill();
                }
            }
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0f);     //this is the projectile light color R, G, B (Red, Green, Blue)
            }
            if (Projectile.owner == Main.myPlayer)
            {
                if (Main.MouseWorld.X >= player.Center.X)
                {
                    Projectile.velocity.X = 1;
                    player.direction = 1;
                }
                else if (Main.MouseWorld.X < player.Center.X)
                {
                    Projectile.velocity.X = -1;
                    player.direction = -1;

                }
            }

            Projectile.Center = player.MountedCenter;
            Projectile.position.X += player.width / 2 * player.direction;
            
            //Projectile.spriteDirection = player.direction;
            
            Projectile.rotation += 0.15f * player.direction; //this is the projectile rotation/spinning speed
           
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = Projectile.rotation;

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);  //this is the dust that this projectile will spawn
            Main.dust[dust].velocity /= 1f;
            Main.dust[dust].scale = 2f;
            Main.dust[dust].noGravity = true;
            
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
            hitbox.Width = 260;
            hitbox.Height = 260;
            hitbox.X -= (hitbox.Width - Projectile.width) / 2;
            hitbox.Y -= (hitbox.Height - Projectile.height) / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }

        public override void PostDraw(Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            var player = Main.player[Projectile.owner];

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {

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


        public bool reflected;
        public override void AI(Projectile projectile)
        {
            var player = Main.LocalPlayer;

            //var projreflect = Main.projectile[mod.ProjectileType("SelenianBladeProj")];

            //if (Main.LocalPlayer.HasBuff(BuffType<SelenianBuff>()))
            if (projectile.aiStyle != 20)
            {

                if (player.itemAnimation > 1 && player.controlUseItem && player.HeldItem.type == ModContent.ItemType<Items.Weapons.LunarSolarSpin>())
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

                            if (Vector2.Distance(player.Center, projectile.Center) <= 100 && !reflected)
                            {
                                int choice = Main.rand.Next(2);
                                if (choice == 0)
                                {
                                    SoundEngine.PlaySound(SoundID.Item56, projectile.Center);
                                    //Projectile.Kill();
                                    if (!Main.dedServ)
                                    {
                                        projectile.owner = Main.myPlayer;
                                        projectile.velocity.X *= -1f;


                                        projectile.velocity.Y *= -1f;

                                        projectile.friendly = true;
                                        projectile.hostile = false;
                                        //damage is in npceffects
                                        projectile.netUpdate = true;

                                    }
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