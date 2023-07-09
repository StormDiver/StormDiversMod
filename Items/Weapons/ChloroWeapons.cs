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
            //DisplayName.SetDefault("Chlorophyte Dart Shotgun");
            //Tooltip.SetDefault("Fires out a burst of darts");
            Item.ResearchUnlockCount = 1;

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
            return new Vector2(-4, 0);
        }
        int accuracy;
       

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 15f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            int numberProjectiles = 3 + Main.rand.Next(2);//This defines how many projectiles to shot.
            if (type == ProjectileID.IchorDart)
            {
                
                numberProjectiles = 3;
                accuracy = 20;

            }
            else if (type == ProjectileID.CrystalDart)
            {
                //damage = (damage * 3);
                //numberProjectiles = 1;
                accuracy = 25;
            }
            else              
            {
                accuracy = 15;
            }

            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(accuracy)); // This defines the projectiles random spread . 10 degree spread.
                int ProjID = Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 2), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
                if (type == ProjectileID.CrystalDart)
                {
                    Main.projectile[ProjID].usesLocalNPCImmunity = true;
                    Main.projectile[ProjID].localNPCHitCooldown = 30;
                }

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
            //DisplayName.SetDefault("Chlorophyte Spore Staff");
            //Tooltip.SetDefault("Fires out a spore that explodes into multiple spore clouds at the cursor's location");
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 30;
            }
            else
            {
                Item.mana = 20;
            }
            Item.UseSound = SoundID.Item21;

            Item.damage = 52;

            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.ChloroStaffProj>();

            Item.shootSpeed = 10;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            /*Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 15;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }*/

            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
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
 