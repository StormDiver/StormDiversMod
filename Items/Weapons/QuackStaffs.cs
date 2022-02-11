using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
    public class QuackStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duck Fracture");
            Tooltip.SetDefault("Fires out mini ducks to attack your foes\n'May be prone to quacking'");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 54;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            

            Item.damage = 37;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<QuackProj>();

            Item.shootSpeed = 5f;
            
            //Item.useAmmo = AmmoID.Arrow;
                

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        /* public override Vector2? HoldoutOffset()
         {
             return new Vector2(5, 0);
         }*/
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            for (int i = 0; i < 3; i++)
            {
                float posX = position.X + Main.rand.NextFloat(35f, -35f);
                float posY = position.Y + Main.rand.NextFloat(10f, -40f);

                Projectile.NewProjectile(source, new Vector2(posX, posY), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
            }
            SoundEngine.PlaySound(SoundID.Duck, (int)player.position.X, (int)player.position.Y);

            return false;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Tools.Quack>(), 1)
            .AddIngredient(ItemID.HallowedBar, 25)
            .AddTile(TileID.MythrilAnvil)
            .Register();


        }
      
    }
    //____________________________________________________________________________________________________
    public class QuackStaffSuper : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duck Quackture");
            Tooltip.SetDefault("Summons a bunch of ducks enhanced by lunar energy\n'Incredibly prone to quacking'");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 54;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
        

            Item.damage = 100;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<QuackProj>();

            Item.shootSpeed = 15f;

            //Item.useAmmo = AmmoID.Arrow;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
         public override Vector2? HoldoutOffset()
         {
             return new Vector2(5, 0);
         }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 1f;
           
            int choice = Main.rand.Next(4);
            if (choice == 0)
                {
                type = ModContent.ProjectileType<QuackSolarProj>();
                }
            else if (choice == 1)
            {
                type = ModContent.ProjectileType < QuackVortexProj>();
                damage = damage / 10 * 12;
            }
            else if (choice == 2)
            {
                type = ModContent.ProjectileType < QuackNebulaProj>();
            }
            else if (choice == 3)
            {
                type = ModContent.ProjectileType < QuackStardustProj>();
            }
           
                float posX = position.X + Main.rand.NextFloat(50f, -50f);
                float posY = position.Y + Main.rand.NextFloat(10f, -50f);
            if (Collision.CanHitLine(position, 0, 0, player.Center, 0, 0))
            {

                Projectile.NewProjectile(source, new Vector2(posX, posY), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Duck, (int)player.Center.X, (int)player.Center.Y, 0, 0.5f, -0.25f);

            }


            return false;
        
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<QuackStaff>(), 1)
            .AddIngredient(ItemID.FragmentSolar, 15)
            .AddIngredient(ItemID.FragmentVortex, 15)
            .AddIngredient(ItemID.FragmentNebula, 15)
            .AddIngredient(ItemID.FragmentStardust, 15)

            .AddTile(TileID.LunarCraftingStation)
            .Register();


        }

    }
}