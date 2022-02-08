using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;


namespace StormDiversMod.Items.Weapons
{
    public class MechDestroyerFlail : ModItem
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Vaporiser");
            Tooltip.SetDefault("Fires out 8 spikes upon impacting an enemy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
    
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.crit = 4;
           
            Item.knockBack = 6f;
            Item.damage = 65;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.shoot = ModContent.ProjectileType<Projectiles.DestroyerFlailProj>();
            Item.shootSpeed = 27f;
            Item.channel = true;
            Item.noUseGraphic = true;
        }


        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.HallowedBar, 15)
            .AddIngredient(ItemID.SoulofMight, 20)
            .AddTile(TileID.MythrilAnvil)
            .Register();
           
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/MechDestroyerFlail_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }

    //_______________________________________________________________________________
    public class MechSawBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Shredder");
            Tooltip.SetDefault("Shreds any enemy that it comes into contact with\nEmits sparks that linger on the ground");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.crit = 10;
            Item.DamageType = DamageClass.Melee;
            Item.width = 60;
            Item.height = 26;
            Item.useTime = 10;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.knockBack = 1.5f;

            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.SawBladeChain>();
            Item.shootSpeed = 50f;
            Item.axe = 30;
            Item.tileBoost = 2;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;


        }



        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Chain, 20)
           .AddIngredient(ItemID.HallowedBar, 10)
           .AddIngredient(ItemID.SoulofFright, 20)
           .AddTile(TileID.MythrilAnvil)
           .Register();
           
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/MechSawBlade_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //_______________________________________________________________________________
    public class MechTheSeeker : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Seeker");
            Tooltip.SetDefault("Summons explosive bolts that can be guided towards the cursor when holding right click\nRequires Seeker Bolts, craft more with Souls of Sight");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 24;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<Projectiles.SeekerBoltProj>();
            Item.useAmmo = ItemType<Ammo.SeekerBolt>();
            Item.UseSound = SoundID.Item11;

            Item.damage = 45;
            //Item.crit = 0;
            Item.knockBack = 6f;

            Item.shootSpeed = 10f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10)); // This defines the projectiles random spread . 10 degree spread.
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            return false;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.ExplosivePowder, 20)
          .AddIngredient(ItemID.HallowedBar, 10)
          .AddIngredient(ItemID.SoulofSight, 20)
          .AddTile(TileID.MythrilAnvil)
          .Register();
          
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/MechTheSeeker_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //_______________________________________________________________________________
    public class MechPrimeStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Obliterator");
            Tooltip.SetDefault("Fires out spinning skulls that will home onto any enemy they touch");
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
            Item.width = 40;
            Item.height = 52;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.autoReuse = true;
            Item.UseSound = SoundID.Item43;

            Item.DamageType = DamageClass.Magic;


            Item.damage = 40;
            Item.knockBack = 4f;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.mana = 12;
            Item.shoot = ModContent.ProjectileType<Projectiles.SkullSeek>();
            Item.shootSpeed = 16f;

            Item.useStyle = ItemUseStyleID.Shoot;

            Item.noMelee = true;

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 48f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0)); // This defines the projectiles random spread . 10 degree spread.
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);

            }
            return false;


        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.CrystalShard, 20)
          .AddIngredient(ItemID.UnicornHorn, 2)
          .AddIngredient(ItemID.SoulofFright, 20)
          .AddTile(TileID.MythrilAnvil)
          .Register();
           

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/MechPrimeStaff_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}