using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using StormDiversMod.Common;


namespace StormDiversMod.Items.Weapons
{
	public class LunarSolarSpin : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName.SetDefault("Blazing Star"); 
			//Tooltip.SetDefault("Spins around with the force of a star, knocking enemies in the direction you're facing\nHas a chance to reflect basic projectiles when spun");
            Item.ResearchUnlockCount = 1;
        }

		public override void SetDefaults() 
		{
			Item.damage = 140;
            Item.crit = 0;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 70;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
            Item.reuseDelay = 10;
			Item.useStyle = 100;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item116;
			Item.autoReuse = true;
           Item.useTurn = false;
            Item.channel = true;
            Item.knockBack = 10f;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<Projectiles.SolarSpinProj>();
           
            Item.noMelee = true; 
            Item.noUseGraphic = true; 
            
        }
        public override void UseItemFrame(Player player)     //this defines what frame the player use when this weapon is used
        {
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ItemID.FragmentSolar, 18)
             .AddTile(TileID.LunarCraftingStation)
             .Register();
         
        }
        public override Color? GetAlpha(Color lightColor)
        {

            Color color = Color.White;
            color.A = 150;
            return color;

        }
        /*public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LunarSolarSpin");


            //spriteBatch.Draw(texture, Item.Center - Main.screenPosition, new Rectangle(0, 0, Item.width, Item.height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw
           (
               texture,
               new Vector2
               (
                   Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                   Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f 
               ),
               new Rectangle(0, 0, texture.Width, texture.Height),
               Color.White,
               rotation,
               texture.Size() * 0.5f,
               scale,
               SpriteEffects.None,
               0f
           );
        }*/
    }
    //________________________________________________________________
    
    public class LunarVortexLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vortex Launcher");
            //Tooltip.SetDefault("Fires out a spread of vortex rockets\nOne will always be fully accurate, with a higher velocity and deals more damage");
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
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            //Item.UseSound = SoundID.Item92;

            Item.damage = 45;

            Item.knockBack = 5f;

            Item.shoot = ProjectileID.RocketI;
            Item.shootSpeed = 18f;

            Item.useAmmo = AmmoID.Rocket;
            Item.useTime = 26;
            Item.useAnimation = 26;

            Item.noMelee = true; //Does the weapon itself inflict damage?

            //Item.glowMask = 194;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
       
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 20f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            int numberProjectiles = 4;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(15));
                float scale = 1f - (Main.rand.NextFloat() * .2f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 3), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.VortexRocketProj>(), damage, knockback, player.whoAmI);

            }
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 3), new Vector2(velocity.X * 1f, velocity.Y) * 1f, ModContent.ProjectileType<Projectiles.VortexRocketProj2>(), (int)(damage * 1.5f), knockback, player.whoAmI);

            SoundEngine.PlaySound(SoundID.Item92, player.Center);

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ItemID.FragmentVortex, 18)
             .AddTile(TileID.LunarCraftingStation)
             .Register();
        }


        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LunarVortexLauncher_Glow");


            //spriteBatch.Draw(texture, Item.Center - Main.screenPosition, new Rectangle(0, 0, Item.width, Item.height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw
           (
               texture,
               new Vector2
               (
                   Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                   Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f 
               ),
               new Rectangle(0, 0, texture.Width, texture.Height),
               Color.White,
               rotation,
               texture.Size() * 0.5f,
               scale,
               SpriteEffects.None,
               0f
           );
        }
        /*public class ItemUseGlow : GlobalItem
        {
            public Texture2D glowTexture = mod.GetTexture("Items/Glowmasks/VortexLauncher_Glow");
            public int glowOffsetY = 0;
            public int glowOffsetX = 0;
            public override bool InstancePerEntity => true;
            public override bool CloneNewInstances => true;
        }*/
    }
    //________________________________________________________________
    public class LunarNebulaStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nebula Storm");
            //Tooltip.SetDefault("Summons nebula projectiles that explode into many homing bolts");
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
            Item.height = 54;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 18;
            }
            else
            {
                Item.mana = 12;
            }
            Item.UseSound = SoundID.Item20;

            Item.damage = 75;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.NebulaStaffProj>();

            Item.shootSpeed = 12.5f;

            //Item.useAmmo = AmmoID.Arrow;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        /* public override Vector2? HoldoutOffset()
         {
             return new Vector2(5, 0);
         }*/
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
      

            /* for (int l = 0; l < Main.projectile.Length; l++)
              {                                                                  //this make so you can only spawn one of this projectile at the time,
                  Projectile proj = Main.projectile[l];
                  if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
                  {
                      proj.active = false;
                  }
              }*/

            return true;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ItemID.FragmentNebula, 18)
             .AddTile(TileID.LunarCraftingStation)
             .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LunarNebulaStaff_Glow");


            //spriteBatch.Draw(texture, Item.Center - Main.screenPosition, new Rectangle(0, 0, Item.width, Item.height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw
           (
               texture,
               new Vector2
               (
                   Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                   Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f 
               ),
               new Rectangle(0, 0, texture.Width, texture.Height),
               Color.White,
               rotation,
               texture.Size() * 0.5f,
               scale,
               SpriteEffects.None,
               0f
           );
        }
    }
    //________________________________________________________________
    public class LunarStardustSentry : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star Invader Staff");
            //Tooltip.SetDefault("Summons a floating Stardust Sentry that launches mini Flow Invaders that home into enemies\nRight click to target a specific enemy");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
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
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.damage = 75;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.SentryProjs.StardustSentryProj>();
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item78;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;
            return false;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FragmentStardust, 18)
            .AddTile(TileID.LunarCraftingStation)
            .Register();


        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LunarStardustSentry_Glow");


            //spriteBatch.Draw(texture, Item.Center - Main.screenPosition, new Rectangle(0, 0, Item.width, Item.height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw
           (
               texture,
               new Vector2
               (
                   Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                   Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f 
               ),
               new Rectangle(0, 0, texture.Width, texture.Height),
               Color.White,
               rotation,
               texture.Size() * 0.5f,
               scale,
               SpriteEffects.None,
               0f
           );
        }
    }
}