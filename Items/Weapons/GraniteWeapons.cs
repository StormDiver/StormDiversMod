using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Buffs;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;

namespace StormDiversMod.Items.Weapons
{
    public class GraniteRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Granite Rifle");
            //Tooltip.SetDefault("Converts Musket Balls into Granite Bullets that pierce once");

            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
           
            Item.width = 50;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item40;

            Item.damage = 17;
            
            Item.knockBack = 2f;
       
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 11f;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true; 
        }


        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }



        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            if (type == ProjectileID.Bullet)
            {
                type = ModContent.ProjectileType<Projectiles.GraniteBulletProj>();
            }


            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            //Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 40);


            return false;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
          .AddRecipeGroup("StormDiversMod:GoldBars", 10)
          .AddIngredient(ModContent.ItemType<Items.Materials.GraniteCore>(), 3)
          .AddTile(TileID.Anvils)
          .Register();
           


        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/GraniteRifle_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
   
    //__________________________________________________
    public class GraniteStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Granite Scepter");
            //Tooltip.SetDefault("Fires out a bunch of energy bolts");
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
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 9;
            }
            else
            {
                Item.mana = 6;
            }
            Item.UseSound = SoundID.Item8;

            Item.damage = 14;

            Item.knockBack = 3f;

            Item.shoot = ModContent.ProjectileType<Projectiles.GraniteStaffProj>();
           
            Item.shootSpeed = 12f;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 60f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            int numberProjectiles = 2 + Main.rand.Next(2);

            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10));
                float scale = 1f - (Main.rand.NextFloat() * .1f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);

            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
          .AddRecipeGroup("StormDiversMod:GoldBars", 10)
          .AddIngredient(ModContent.ItemType<Items.Materials.GraniteCore>(), 3)
          .AddTile(TileID.Anvils)
          .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/GraniteStaff_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //___________________________
    //___________________________________________________________________________________
    public class GraniteYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Surge");
            ////Tooltip.SetDefault("Attack enemies");
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 30;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            //Item.crit = 0;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 20;
            Item.height = 26;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.useTurn = true;
            Item.knockBack = 2f;
            Item.shoot = ModContent.ProjectileType<Projectiles.GraniteYoyoProj>();
            Item.shootSpeed = 9f;
            Item.noMelee = true;
            Item.noUseGraphic = true;

        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
         .AddRecipeGroup("StormDiversMod:GoldBars", 10)
         .AddIngredient(ModContent.ItemType<Items.Materials.GraniteCore>(), 3)
         .AddTile(TileID.Anvils)
         .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/GraniteYoyo_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}