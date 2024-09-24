using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Common;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using StormDiversMod.Items.Vanitysets;
using Microsoft.Build.Evaluation;
using static Terraria.GameContent.Bestiary.BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;
using System.Collections;

namespace StormDiversMod.Items.Weapons
{
    public class SludgeLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Toxic Sludge Cannon");
            //Tooltip.SetDefault("	Fires out a projectile of Toxic Sludge that explodes into smaller sludge blobs on tile or enemy impact
            //The Smaller sludge blobs stick to tiles and hit multiple times after doing so
            //All sludge projectiles inflict the Sludged Debuff, causing damage over time and slowing down enemies
            //Uses gel for ammo");
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
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<Projectiles.SludgeOrbProj>();
            Item.useAmmo = AmmoID.Gel;

            Item.UseSound = SoundID.Item95;
            Item.damage = 45;
            //Item.crit = 4;
            Item.knockBack = 3f;
            Item.shootSpeed = 10;
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 10);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(5));
          
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y + 8), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.SludgeOrbProj>(), damage, knockback, player.whoAmI);
            for (int i = 0; i < 15; i++)
            {
                int dust2 = Dust.NewDust(new Vector2(position.X, position.Y + 8)  * 1f, 0, 0, 39, perturbedSpeed.X / 8, perturbedSpeed.Y / 8, 150, default, 1f);
            }
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/SludgeLauncher_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    public class SludgeVenomLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Acid Venom Sludge Cannon");
            //Tooltip.SetDefault("Fires out a projectile of Acidic Venom Sludge that explodes into smaller sludge blobs on tile or enemy impact
            //The Smaller sludge blobs stick to tiles and hit multiple times after doing so
            //All sludge projectiles inflict the Sludged Debuff, causing damage over time and slowing down enemies
            //Uses gel for ammo");
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
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<Projectiles.SludgeVenomProj>();
            Item.useAmmo = AmmoID.Gel;

            Item.UseSound = SoundID.Item95;
            Item.damage = 60;
            Item.knockBack = 4f;
            Item.shootSpeed = 13;
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 10);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 50;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(2));

            Projectile.NewProjectile(source, new Vector2(position.X, position.Y + 8), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.SludgeVenomProj>(), damage, knockback, player.whoAmI);
            for (int j = 0; j < 15; j++)
            {
                int dust2 = Dust.NewDust(new Vector2(position.X, position.Y + 8) * 1f, 0, 0, 118, perturbedSpeed.X / 8, perturbedSpeed.Y / 8, 150, default, 1f);
            }
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/SludgeVenomLauncher_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<SludgeLauncher>(), 1)
            .AddIngredient(ItemID.VialofVenom, 20)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}