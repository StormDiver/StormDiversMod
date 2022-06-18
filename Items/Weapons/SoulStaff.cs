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


namespace StormDiversMod.Items.Weapons
{
    public class SoulStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Storm");
            Tooltip.SetDefault("Summons damaging souls around the cursor");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

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
            Item.useTime = 10;
            Item.useAnimation = 30;
            Item.useTurn = false;
            //Item.channel = true;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item8 with {Volume = 0.75f, Pitch = -0.2f};

            Item.damage = 50;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<SoulFrightProj>();
            
            Item.shootSpeed = 1f;

            Item.mana = 14;

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

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            position = Main.MouseWorld;

            int choice = Main.rand.Next(3);
            if (choice == 0)
            {
                type = ModContent.ProjectileType<SoulFrightProj>();
                dusttype = 259;
                dustscale = 1.1f;
            }
            else if (choice == 1)
            {
                type = ModContent.ProjectileType<SoulSightProj>();
                dusttype = 110;
                dustscale = 0.9f;

            }
            else if (choice == 2)
            {
                type = ModContent.ProjectileType<SoulMightProj>();
                dusttype = 56;
                dustscale = 1;

            }

            //For the radius
            double deg = Main.rand.Next(0, 360); //The degrees
            double rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = 250; //Distance away from the player


            position.X = Main.MouseWorld.X - (int)(Math.Cos(rad) * dist);
            position.Y = Main.MouseWorld.Y - (int)(Math.Sin(rad) * dist);

            //For the direction

            float shootToX = Main.MouseWorld.X - position.X;
            float shootToY = Main.MouseWorld.Y - position.Y;
            float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

            distance = 3f / distance;
            shootToX *= distance * 7;
            shootToY *= distance * 7;
            int proj = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(shootToX, shootToY), type, damage, knockback, player.whoAmI, 0 ,0);
            //SoundEngine.PlaySound(SoundID.Item8 with{Volume = 0.5f, Pitch = 0.5f}, position);

            //For the dust
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 66; 
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect((player.Center - new Vector2(15, 15)) + muzzleOffset, 30, 30, dusttype, velocity.X * 10, velocity.Y * 10, 100, default, dustscale);
                dust.noGravity = true;
                dust.velocity *= 0;
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
            .AddIngredient(ItemID.SoulofMight, 15)
            .AddIngredient(ItemID.SoulofSight, 15)
            .AddIngredient(ItemID.SoulofFright, 15)
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