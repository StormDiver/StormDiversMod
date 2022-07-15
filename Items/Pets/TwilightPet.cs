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
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Pets
{

    public class TwilightPetItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strange Twilight Hood");
            Tooltip.SetDefault("Summons a mysterious figure to light your way");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;


            Item.width = 24;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            
            Item.shoot = ProjectileType<TwilightPetProj>();
            Item.buffType = BuffType<TwilightPetBuff>();
            Item.rare = ItemRarityID.Orange;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {


            player.AddBuff(Item.buffType, 2); // The item applies the buff, the buff spawns the projectile

            return false;

        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
        
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Pets/TwilightPetItem_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

    }

    public class TwilightPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Twilight Figure");
            Description.SetDefault("A strange hooded figure lights your way");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<EquipmentEffects>().twilightPet = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<TwilightPetProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {

                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), new Vector2(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2)), new Vector2(0, 0), ProjectileType<TwilightPetProj>(), 0, 0, player.whoAmI);

            }
        }
    }
    public class TwilightPetProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twilight light");
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.LightPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.DD2PetGato);
            //aiType = ProjectileID.DD2PetGato;
            Projectile.aiStyle = -1;
            Projectile.width = 22;
           Projectile.height = 42;
            Projectile.scale = 1;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.ignoreWater = true;

        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            //player.petFlagDD2Gato = false; 
            return true;
        }


        float movespeed = 12f; //Speed of the pet

        float xpostion = 50; // The picked x postion
        float ypostion = -60f;
        bool teleport;
        bool yAssend;
        bool teleanimation;

       


        public override void AI()
        {

            Player player = Main.player[Projectile.owner];
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0.3f / 255f); //light
            }
            EquipmentEffects modPlayer = player.GetModPlayer<EquipmentEffects>();
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (player.dead)
            {
                modPlayer.twilightPet = false;
            }
            if (modPlayer.twilightPet)
            {
                Projectile.timeLeft = 2;
            }
            


            float distanceX = player.Center.X - Projectile.Center.X;
            float distanceY = player.Center.Y - Projectile.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));

            Projectile.rotation = Projectile.velocity.X / 13;


            xpostion = 50 * player.direction; //Moves to the front of the player
            Projectile.spriteDirection = player.direction; //Flips sprite
            movespeed = distance / 20 + 0.5f; //Moves faster the further away it is

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

            if (distance <= 250) //Movement
            {
                Vector2 moveTo = player.Center;
                Vector2 move = moveTo - Projectile.Center + new Vector2(xpostion, ypostion); //Postion around player
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > movespeed)
                {
                    move *= movespeed / magnitude;
                }
                Projectile.velocity = move;

                if (teleport) //Post teleport
                {
                    if (!Main.dedServ)
                    {
                        SoundEngine.PlaySound(SoundID.Item8 with { Volume = 2f, Pitch = 0.5f }, Projectile.Center);

                        for (int i = 0; i < 30; i++) //Dust post-teleport
                        {
                            var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 62);
                            dust.scale = 1.1f;
                            dust.velocity *= 2;
                            dust.noGravity = true;

                        }
                    }
                    teleport = false;
                }
            }
            else //teleports if too far away from player
            {
                if (!Main.dedServ)
                {
                    for (int i = 0; i < 30; i++) //Dust pre-teleport
                    {
                        var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 62);
                        dust.scale = 1.1f;
                        dust.velocity *= 2;
                        dust.noGravity = true;

                    }
                }
                teleanimation = true;
                Projectile.frame = 4;

                teleport = true;
                Projectile.position.X = player.Center.X + xpostion - Projectile.width / 2;
                Projectile.position.Y = player.Center.Y + ypostion - Projectile.height / 2;
               
            }
            //dust
            if (!Main.dedServ)
            {
                if (Main.rand.Next(4) == 0)
                {
                    var dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y + 6), 10, 6, 58, 0, 3);
                    dust.scale = 1f;
                    dust.velocity.X *= 0.5f;
                    dust.fadeIn = 0.5f;
                    dust.noGravity = true;
                    dust.noLight = true;
                }

            }


            AnimateProjectile();

        }
         public void AnimateProjectile() // Call this every frame, for example in the AI method.
         {
            if (!teleanimation)
            {
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
            if (teleanimation)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 8)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame >= 8)
                {
                    teleanimation = false;
                    Projectile.frame = 0;

                }
            }
        }


        public override void Kill(int timeLeft)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 30; i++) //Dust post-teleport
                {
                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 62);
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

