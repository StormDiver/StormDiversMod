using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Common;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace StormDiversMod.Projectiles.Petprojs
{ 
    public class AridBossPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {           
            //DisplayName.SetDefault("Liberated Husk");
            //Description.SetDefault("Free at last, but it decides to stay with you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<EquipmentEffects>().aridBossPet = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<AridBossPetProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), new Vector2(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2)), new Vector2(0, 0), ProjectileType<AridBossPetProj>(), 0, 0, player.whoAmI);

            }
        }
    }
    public class AridBossPetProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Liberated Husk");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.LightPet[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {         
            Projectile.aiStyle = -1;
            Projectile.width = 28;
           Projectile.height = 50;
            Projectile.scale = 1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.timeLeft *= 5;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            //player.petFlagDD2Gato = false; 
            return true;
        }

        float speed = 16f;
        float inertia = 25f;

        Vector2 projPosition;
        int angle;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.8f / 255f, (255 - Projectile.alpha) * 0.7f / 255f, (255 - Projectile.alpha) * .6f / 255f); //light
            }
            EquipmentEffects modPlayer = player.GetModPlayer<EquipmentEffects>();
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (player.dead)
            {
                modPlayer.aridBossPet = false;
            }
            if (modPlayer.aridBossPet)
            {
                Projectile.timeLeft = 2;
            }
            /*if (Projectile.position.X >= player.position.X)
                Projectile.spriteDirection = -1;

            if (Projectile.position.X < player.position.X)
                Projectile.spriteDirection = 1;*/

            Projectile.spriteDirection = player.direction;
            Projectile.rotation = Projectile.velocity.X / 50;

            angle += 2 * player.direction;

            double deg = (angle);
            double rad = deg * (Math.PI / 180);
            double dist = 75; //Distance away from the player

            //position
            projPosition.X = (int)(Math.Cos(rad) * dist); //X pos
            projPosition.Y = (int)(Math.Sin(rad) * dist); //Y pos

            Vector2 idlePosition = player.Center + new Vector2(projPosition.X, projPosition.Y);

            //var dust5 = Dust.NewDustDirect(new Vector2(idlePosition.X, idlePosition.Y), 0, 0, 0, 0, 0.5f);
            //dust5.noGravity = true;

            // Teleport to player if distance is too big
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
            {
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }
            {
                if (distanceToIdlePosition > 50)
                {
                    // Speed up the pet if it's away from the player
                    speed = 15f;
                    inertia = 20f;
                }
                else
                {
                    // Slow down the pet if closer to the player
                    speed = 4.5f;
                    inertia = 20f;
                }
                if (distanceToIdlePosition > 0f)
                {
                    // The immediate range around the player (when it passively floats about)

                    // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
                else if (Projectile.velocity == Vector2.Zero)
                {
                    // If there is a case where it's not moving at all, give it a little "poke"
                    //Projectile.velocity.X = -0.15f;
                    //Projectile.velocity.Y = -0.05f;
                }
            }
            if (!Main.dedServ)
            {           
                if (Main.rand.Next(6) == 0)
                {

                    var dust3 = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.Bottom.Y - 10), Projectile.width, 6, 138, 0, 3);
                    dust3.noGravity = false;
                    dust3.scale = 0.5f;
                }
            }
            // animate         
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 4)
            {
                Projectile.frame = 0;

            }

        }
        public override void OnKill(int timeLeft)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 30; i++)
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 138);
                    dust.scale = 1.1f;
                    dust.velocity *= 2;
                    dust.noGravity = true;

                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;
        }

        public override bool PreDraw(ref Color lightColor) //trail
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, 0);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * (texture.Height / Main.projFrames[Projectile.type]), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                    color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }

            return true;
        }
    }
}

