using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using StormDiversMod.Basefiles;
using System.Collections.Generic;

using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.Items.Weapons
{  
    public class PainSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Pain Giver");
            Tooltip.SetDefault("Inflicts unimaginable pain\nBut not as much pain as I will inflict upon you for having this");
            Item.staff[Item.type] = true;

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 125;

            Item.DamageType = DamageClass.Generic;
            Item.width = 40;
            Item.height = 50;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;  
            Item.value = Item.sellPrice(1, 27, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.scale = 1f;
            Item.knockBack = 20;
            Item.shoot = ModContent.ProjectileType<PainProj>();
            Item.shootSpeed = 30f;
            Item.noMelee = true;
            Item.crit = 46;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(1) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6, 0f, 0f, 100, default, 1.5f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        float posY;
        int shoottype;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!GetInstance<ConfigurationsIndividual>().NoShake)
            {
                player.GetModPlayer<MiscFeatures>().screenshaker = true;
            }
            int shoottype = Main.rand.Next(0, 4);
            int numberProjectiles = 5 + Main.rand.Next(3);
            //SoundEngine.PlaySound(SoundID.ScaryScream with{ Volume = 0.5f, Pitch = 0.5f, PitchVariance = 0.1f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew}, player.Center);
            SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Sounds/ThePainSound" ) with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, player.Center);

            for (int index = 0; index < numberProjectiles; ++index)
            {
                if (shoottype == 0)
                {
                    Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(150) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                    vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-600, 600); //Spawn Spread
                    vector2_1.Y -= (float)(70 * index);
                    float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                    float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                    if ((double)num13 < 0.0) num13 *= -1f;
                    if ((double)num13 < 20.0) num13 = 20f;
                    float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                    float num15 = Item.shootSpeed / num14;
                    float num16 = num12 * num15;
                    float num17 = num13 * num15;
                    float SpeedX = num16 + (float)Main.rand.Next(-10, 10) * 0.05f;  //this defines the projectile X position speed and randomnes
                    float SpeedY = num17 + (float)Main.rand.Next(-10, 10) * 0.05f;  //this defines the projectile Y position speed and randomnes
                    int projid = Projectile.NewProjectile(source, new Vector2(vector2_1.X, vector2_1.Y), new Vector2(SpeedX, SpeedY), type, (int)(damage), 0.5f, player.whoAmI, 0.0f, (float)Main.rand.Next(5));
                }
                if (shoottype == 1)
                {
                    float posX = position.X + Main.rand.NextFloat(100f, -100f);
                    if (player.gravDir == 1)
                    {
                        posY = position.Y + Main.rand.NextFloat(10f, -100f);
                    }
                    else
                    {
                        posY = position.Y - Main.rand.NextFloat(-10f, 100f);
                    }
                    int projid2 = Projectile.NewProjectile(source, new Vector2(posX, posY), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                }
                if (shoottype == 2)
                {
                    Vector2 perturbedSpeed2 = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(360)); // 
                    Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(perturbedSpeed2.X * 0.5f, perturbedSpeed2.Y * 0.5f), type, damage, knockback, player.whoAmI);
                }
                if (shoottype == 3)
                {
                    double deg = Main.rand.Next(0, 360); //The degrees
                    double rad = deg * (Math.PI / 180); //Convert degrees to radians
                    double dist = 500; //Distance away from the cursor


                    position.X = Main.MouseWorld.X - (int)(Math.Cos(rad) * dist);
                    position.Y = Main.MouseWorld.Y - (int)(Math.Sin(rad) * dist);
                    float projspeed = 20;
                    Vector2 projvelocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(position.X, position.Y)) * projspeed;
                    int ProjID = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(projvelocity.X, projvelocity.Y), type, damage, knockback, player.whoAmI, 1, 0);
                    Main.projectile[ProjID].tileCollide = false;
                }
            }
            return false;
        }
        /*public override void AddRecipes()
        {
         CreateRecipe()
         .AddIngredient(ModContent.ItemType<Items.Vanitysets.ThePainMask>(), 50)
         .AddIngredient(ItemID.LunarOre, 1)
         .AddTile(TileID.LunarCraftingStation)
         .Register();
        }*/
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}