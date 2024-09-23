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
using StormDiversMod.Common;
using Terraria.GameContent.Drawing;

namespace StormDiversMod.Items.Weapons
{
    public class SoulStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Soul Storm");
            //Tooltip.SetDefault("Summons 3 damaging souls around the cursor that travel inwards each use");
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;

            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 8;
            Item.useAnimation = 24;
            Item.useTurn = false;
            //Item.channel = true;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item13;

            Item.damage = 50;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<SoulsProj>();
            
            Item.shootSpeed = 1f;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 21;
            }
            else
            {
                Item.mana = 14;
            }

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override bool CanUseItem(Player player)
        {
            /*if (Collision.CanHitLine(Main.MouseWorld, 1, 1, player.Center, 0, 0))
            {
                return true;
            }
            else
            {
                return false;
            }*/
            return true;
        }
        int dusttype;
        float dustscale;
        int choice = 0;
        Vector2 projvelocity;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float projspeed = 20;

            //for (int j = 0; j < 3; j++)
            {
                //For the radius
                double deg = Main.rand.Next(0, 360); //The degrees
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 250; //Distance away from the cursor

                if (Vector2.Distance(player.Center, Main.MouseWorld) <= 800)
                {
                    position.X = Main.MouseWorld.X - (int)(Math.Cos(rad) * dist);
                    position.Y = Main.MouseWorld.Y - (int)(Math.Sin(rad) * dist);
                    projvelocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(position.X, position.Y)) * projspeed;
                }
                else
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X * 800, velocity.Y * 800).RotatedBy(0);
                    position.X = player.Center.X + perturbedSpeed.X - (int)(Math.Cos(rad) * dist);
                    position.Y = player.Center.Y + perturbedSpeed.Y - (int)(Math.Sin(rad) * dist);
                    //var dust = Dust.NewDustDirect(player.Center + perturbedSpeed, 30, 30, 248, 0, 0, 100, default, 5);
                    //dust.noGravity = true;
                    projvelocity = Vector2.Normalize(new Vector2(player.Center.X + perturbedSpeed.X, player.Center.Y + perturbedSpeed.Y) - new Vector2(position.X, position.Y)) * projspeed;

                }

                //position.X = Main.MouseWorld.X - (int)(Math.Cos(rad) * dist);
                //position.Y = Main.MouseWorld.Y - (int)(Math.Sin(rad) * dist);
                //projvelocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(position.X, position.Y)) * projspeed;
                //For the direction

                //int choice = Main.rand.Next(3);
                //Summon all 3 types of soul
                if (choice == 0)
                {
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(projvelocity.X, projvelocity.Y), ModContent.ProjectileType<SoulsProj>(), damage, knockback, player.whoAmI, 0, 2); //Might
                    dusttype = 56;
                    dustscale = 1;                
                    choice++;
                }
                else if (choice == 1)
                {
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(projvelocity.X, projvelocity.Y), ModContent.ProjectileType<SoulsProj>(), damage, knockback, player.whoAmI, 0, 1); //Sight
                    dusttype = 110;
                    dustscale = 0.9f;
                    choice++;
                }
                else if (choice == 2)
                {
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(projvelocity.X, projvelocity.Y), ModContent.ProjectileType<SoulsProj>(), damage, knockback, player.whoAmI, 0, 0); //Fright
                    dusttype = 170;
                    dustscale = 1.1f;
                    choice = 0;
                }

                //SoundEngine.PlaySound(SoundID.Item8 with{Volume = 0.5f, Pitch = 0.5f}, position);

                //For the dust
                Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 66;
                for (int i = 0; i < 10; i++)
                {

                    var dust = Dust.NewDustDirect((player.Center - new Vector2(15, 15)) + muzzleOffset, 30, 30, dusttype, velocity.X * 10, velocity.Y * 10, 100, default, dustscale);
                    dust.noGravity = true;
                    dust.velocity *= 0;
                }
            }
            return false;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        /*public override Vector2? HoldoutOffset()
        {
            return new Vector2(6, 0);
        }*/

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.HallowedBar, 16)
            .AddIngredient(ItemID.SoulofMight, 12)
            .AddIngredient(ItemID.SoulofSight, 12)
            .AddIngredient(ItemID.SoulofFright, 12)
            .AddTile(TileID.MythrilAnvil)
            .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/SoulStaff_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

    }
   
}