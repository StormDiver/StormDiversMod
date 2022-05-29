using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;

namespace StormDiversMod.Items.Weapons
{
	public class MoltenDagger : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Molten Dagger"); 
			Tooltip.SetDefault("Rapidly stab your foes");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }

		public override void SetDefaults() 
		{
			Item.damage = 30;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
			Item.height = 30;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Rapier;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.scale = 1.2f;
            Item.shoot = ModContent.ProjectileType<Projectiles.MoltenDaggerProj>();
            Item.shootSpeed = 3.5f;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) < 2)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 127, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
       /* public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.SuperBurnDebuff>(), 300);
        }
        public override void OnHitPvp(Player player, Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType < Buffs.SuperBurnDebuff>(), 300);
        }*/
       
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.HellstoneBar, 16)
          .AddTile(TileID.Anvils)
          .Register();

           
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/MoltenDagger_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //_______________________________________________________________________________
    public class MoltenSeedLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flaming Seed Launcher");
            Tooltip.SetDefault("Sets seeds ablaze\nObtain more from the Witch Doctor");
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
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 22;
            Item.useAnimation = 22;
            //Item.reuseDelay = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            //Item.shoot = mod.ProjectileType("ProtoGrenadeProj");
            Item.shoot = ProjectileID.Seed;
            Item.useAmmo = AmmoID.Dart;
            Item.UseSound = SoundID.Item64;

            Item.damage = 18;
            //Item.crit = 4;
            Item.knockBack = 3f;
            Item.shootSpeed = 13f;
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void HoldItem(Player player)
        {

        }
       
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            if (type == ProjectileID.Seed)
            {
                type = ModContent.ProjectileType<FlamingSeed>();
                damage += (damage / 2);
            }

            for (int i = 0; i < 1; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(3));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ItemID.HellstoneBar, 16)
         .AddTile(TileID.Anvils)
         .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/MoltenSeedLauncher_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
        public class SeedShop : GlobalNPC
        {
            public override void SetupShop(int type, Chest shop, ref int nextSlot)
            {
                switch (type)
                {
                    case NPCID.WitchDoctor:

                        if (Main.LocalPlayer.HasItem(ModContent.ItemType<MoltenSeedLauncher>()))
                        {
                            shop.item[nextSlot].SetDefaults(ItemID.Seed);
                            nextSlot++;

                        }

                        break;
                }
            }
        }
    }
    //_______________________________________________________________________________
    public class MoltenSpell : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Blast");
            Tooltip.SetDefault("Summons an orb of lava that splashes on impact");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            Item.mana = 13;
            Item.UseSound = SoundID.Item20;

            Item.damage = 24;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<LavaSpellProj>();
            Item.scale = 0.9f;
            Item.shootSpeed = 14f;

            //Item.useAmmo = AmmoID.Arrow;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.HellstoneBar, 16)
          .AddTile(TileID.Anvils)
          .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/MoltenSpell_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //_______________________________________________________________
    public class MoltenSentry : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Orb Staff");
            Tooltip.SetDefault("Summons a magma orb sentry that launches bouncing fireballs at enemies\nRight click to target a specific enemy");
            //Item.staff[Item.type] = true;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
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
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 10;
            Item.UseSound = SoundID.Item45;

            Item.damage = 32;
            //Item.crit = 4;
            Item.knockBack = 1.5f;

            Item.shoot = ModContent.ProjectileType<Projectiles.SentryProjs.MagmaSentryProj>();

            //Item.shootSpeed = 3.5f;



            Item.noMelee = true;
        }
        public override bool CanUseItem(Player player)
        {
            /*if (Collision.CanHitLine(Main.MouseWorld, 1, 1, player.position, player.width, player.height))
            {
                return true;
            }
            else
            {
                return false;
            }*/
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y - 35), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;
            return false;
            /*position = Main.MouseWorld;
            position.ToTileCoordinates();
            while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[] { new Conditions.IsSolid() }), out _))
            {
                position.Y++;
                position.ToTileCoordinates();
            }
            position.Y -= 34;
            return true;*/

        }

        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ItemID.HellstoneBar, 16)
         .AddTile(TileID.Anvils)
         .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/MoltenSentry_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}