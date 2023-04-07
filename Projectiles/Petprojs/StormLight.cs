using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles.Petprojs
{

    public class StormLightBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            
            //DisplayName.SetDefault("Baby Storm Diver");
            //Description.SetDefault("It's not cute, it's not!!!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<EquipmentEffects>().stormHelmet = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<StormLightProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), new Vector2(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2)), new Vector2(0, 0), ProjectileType<StormLightProj>(), 0, 0, player.whoAmI);

            }
        }
    }
    public class StormLightProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Baby Storm Diver");
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.DD2PetGato);
            //aiType = ProjectileID.DD2PetGato;
            Projectile.aiStyle = -1;
            Projectile.width = 34;
           Projectile.height = 40;
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
        float movespeed = 10f; //Speed of the pet

        float xpostion = 50; // The picked x postion
        float ypostion = -60f;
        bool yAssend;
        bool animatefast;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            EquipmentEffects modPlayer = player.GetModPlayer<EquipmentEffects>();
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (player.dead)
            {
                modPlayer.stormHelmet = false;
            }
            if (modPlayer.stormHelmet)
            {
                Projectile.timeLeft = 2;
            }
            Projectile.rotation = Projectile.velocity.X / 20;

            xpostion = 50 * -player.direction; //Moves to the front of the player
            Projectile.spriteDirection = player.direction; //Flips sprite

            float distance = Vector2.Distance(player.Center, Projectile.Center);

            movespeed = distance / 15 + 0.5f; //Moves faster the further away it is

            //To make bop up and down
            if (yAssend)
            {
                ypostion -= 0.1f;
            }
            else
            {
                ypostion += 0.1f;
            }
            if (ypostion <= -62f)
            {
                yAssend = false;
            }
            if (ypostion >= -58f)
            {
                yAssend = true;
            }

            if (distance < 1000)
            {
                Vector2 moveTo = player.Center;
                Vector2 move = moveTo - Projectile.Center + new Vector2(xpostion, ypostion); //Postion around player
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > movespeed)
                {
                    move *= movespeed / magnitude;
                }
                Projectile.velocity = move;
            }
            if (distance >= 1000) //Teleports if too far
            {
                Projectile.position.X = player.position.X;
                Projectile.position.Y = player.position.Y;

            }
            if (distance > 100 && Projectile.velocity.Y < 5) //Change aniamtion and speed and faster dust
            {
                //if (Projectile.velocity.Y < 5)
                if (!Main.dedServ)
                {
                    {
                        Dust dust;
                        Vector2 position = (new Vector2(Projectile.Center.X - (7 * Projectile.spriteDirection) - 4, Projectile.Center.Y - 3));
                        dust = Terraria.Dust.NewDustDirect(position, 0, 0, 206, -Projectile.spriteDirection * 3f, 3, 0, new Color(255, 255, 255), 1f);
                    }
                }
                animatefast = true;
            }
            else
            {
                if (!Main.dedServ)
                {
                    if (Main.rand.Next(6) == 0)
                    {
                        Dust dust;
                        Vector2 position = (new Vector2(Projectile.Center.X - (7 * Projectile.spriteDirection) - 4, Projectile.Center.Y - 3));
                        dust = Terraria.Dust.NewDustDirect(position, 0, 0, 206, -Projectile.spriteDirection * 2f, 2, 0, new Color(255, 255, 255), 1f);
                    }
                }
                animatefast = false;
            }


            // animate

            if (!animatefast)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;

                }
            }
            if (animatefast)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 2)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame >= 8 || Projectile.frame <= 3)
                {
                    Projectile.frame = 4;

                }
            }
        }
        public override void Kill(int timeLeft)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 30; i++) //Dust post-teleport
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 206);
                    dust.scale = 1.1f;
                    dust.velocity *= 2;
                    dust.noGravity = true;

                }
            }
        }

        /*public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)//Doesn't work >:(
        {
            Texture2D texture = mod.GetTexture("Pets/StormLightProj_Glowmask");

            spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * (Main.ProjectileTexture[Projectile.type].Height / Main.projFrames[Projectile.type]), Main.ProjectileTexture[Projectile.type].Width, Main.ProjectileTexture[Projectile.type].Height)
           , Color.White, Projectile.rotation, Projectile.Center, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 100);
           

        }*/

        
    }
}

