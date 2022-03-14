using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Projectiles;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Weapons
{
    public class AncientStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arid Sandblast Staff");
            Tooltip.SetDefault("Creates an explosive blast of sand at the cursor's location");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


        }
        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 1 , 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useTurn = false;
            //Item.channel = true;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item78;

            Item.damage = 40;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<AncientStaffProj>();// mod.ProjectileType("AncientStaffProj");
            
            Item.shootSpeed = 1f;

            Item.mana = 10;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override bool CanUseItem(Player player)
        {
            if (Collision.CanHitLine(Main.MouseWorld, 1, 1, player.Center, 0, 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;

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
            .AddIngredient(ItemID.Bone, 40)
            .AddIngredient(ItemID.FossilOre, 15)
            .AddTile(TileID.Anvils)
            .Register();         
        }
      

    }
    //______________________________________________________________________________________________________
    public class AncientKnives : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arid Knives");
            Tooltip.SetDefault("Throw out several knives that pierce after spinning");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


        }
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;
            Item.width = 10;
            Item.height = 10;
            Item.maxStack = 1;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<AncientKnivesProj>();
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 3 + Main.rand.Next(2); ; //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            { 

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, (int) (knockback), player.whoAmI);
                //Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 40);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Bone, 40)
            .AddIngredient(ItemID.FossilOre, 15)
            .AddTile(TileID.Anvils)
            .Register();

        }
    }
    //_______________________________________________________________________________
    public class AncientFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arid Sandblaster");
            Tooltip.SetDefault("Fires out a stream of burning sand\nUses gel for ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {

            Item.width = 40;
            Item.height = 24;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 7;
            Item.useAnimation = 24;

            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item20;

            Item.damage = 16;
            Item.knockBack = 0.2f;
            Item.shoot = ModContent.ProjectileType<AncientFlameProj>(); 

            Item.shootSpeed = 2.5f;

            Item.useAmmo = AmmoID.Gel;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
        public override void HoldItem(Player player)
        {
            player.armorPenetration = 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Bone, 40)
            .AddIngredient(ItemID.FossilOre, 15)
            .AddTile(TileID.Anvils)
            .Register();

        }
        public override bool CanConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() > .50f;
        }
    }
}