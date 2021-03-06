using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Weapons
{
    public class ChloroDartGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Dart Shotgun");
            Tooltip.SetDefault("Fires out a burst of darts\nMerges Crystal Darts into a single much more damaging dart");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ProjectileID.Seed;
            Item.useAmmo = AmmoID.Dart;
            //Item.UseSound = SoundID.Item99;

            Item.damage = 18;
            Item.knockBack = 3f;
            Item.shootSpeed = 13f;
            Item.noMelee = true; 
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
        int accuracy;
       

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            int numberProjectiles = 3 + Main.rand.Next(2);//This defines how many projectiles to shot.
            if (type == ProjectileID.IchorDart)
            {
                
                numberProjectiles = 3;
                accuracy = 15;

            }
            else if (type == ProjectileID.CrystalDart)
            {
                damage = (damage * 3);
                numberProjectiles = 1;
                accuracy = 0;
            }
            else
               
            {
                accuracy = 10;
            }

            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(accuracy)); // This defines the projectiles random spread . 10 degree spread.
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 2), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            SoundEngine.PlaySound(SoundID.Item98 with{Volume = 1.5f, Pitch = -0.4f}, player.Center);

            return false;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.ChlorophyteBar, 12)
          .AddTile(TileID.MythrilAnvil)
          .Register();
           

        }
        
    }
    //___________________________________________________________________________________________
    public class ChloroStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Scepter");
            Tooltip.SetDefault("Fires out a stream of damaging spore dust");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.UseSound = SoundID.Item13;

            Item.damage = 48;

            Item.knockBack = 3f;

            Item.shoot = ModContent.ProjectileType<Projectiles.ChloroStaffProj>();

            Item.shootSpeed = 10f;



            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 15;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            float scale = 1f - (Main.rand.NextFloat() * .1f);
            perturbedSpeed = perturbedSpeed * scale;
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
        .AddIngredient(ItemID.ChlorophyteBar, 12)
        .AddTile(TileID.MythrilAnvil)
        .Register();

        }

    }
}
 