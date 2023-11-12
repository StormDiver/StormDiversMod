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
using static Terraria.Main.CurrentFrameFlags;

namespace StormDiversMod.Items.Weapons
{
    public class QuackStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Duck Fracture");
            //Tooltip.SetDefault("Summons a group of 3 mini ducks each use\n'May be prone to quacking'");
            //Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            //Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 3));
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 12;
            }
            else
            {
                Item.mana = 8;
            }

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

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.direction == 1)
                player.itemRotation = 0.7853982f; //45 degrees

            else
                player.itemRotation = 5.497787f; //315 degrees

            player.itemLocation += new Vector2(-15 * player.direction, -20);
        }
        float posY;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            for (int i = 0; i < 3; i++)
            {
                float posX = position.X + Main.rand.NextFloat(35f, -35f);
                
                    posY = position.Y + Main.rand.NextFloat(10f, -40f) * player.gravDir;
                
                Projectile.NewProjectile(source, new Vector2(posX, posY), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
            }
            SoundEngine.PlaySound(SoundID.Duck, player.Center);

            return false;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Tools.Quack>(), 1)
            .AddIngredient(ItemID.HallowedBar, 15)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
      
    }
    //____________________________________________________________________________________________________
    public class QuackStaffSuper : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Duck Quackture");
            //Tooltip.SetDefault("Rapidly summons mini ducks enhanced by lunar energy
            //\nDucks can have one of four different effects\n'Incredibly prone to quacking'");
            //Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 6;
            }
            else
            {
                Item.mana = 4;
            }

            Item.damage = 100;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<QuackProj>();

            Item.shootSpeed = 15;

            //Item.useAmmo = AmmoID.Arrow;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.direction == 1)
                player.itemRotation = 0.7853982f; //45 degrees

            else
                player.itemRotation = 5.497787f; //315 degrees

            player.itemLocation += new Vector2(-24 * player.direction, -20);
        }
        float posY;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 1f;
           
            int choice = Main.rand.Next(4);
            if (choice == 0)
            {
                type = ModContent.ProjectileType<QuackSolarProj>();
            }
            else if (choice == 1)
            {
                type = ModContent.ProjectileType<QuackVortexProj>();
            }
            else if (choice == 2)
            {
                type = ModContent.ProjectileType<QuackNebulaProj>();
            }
            else if (choice == 3)
            {
                type = ModContent.ProjectileType<QuackStardustProj>();
            }
           
                float posX = position.X + Main.rand.NextFloat(50f, -50f);
            
                posY = position.Y + Main.rand.NextFloat(10f, -50f) * player.gravDir;
            
            if (Collision.CanHitLine(position, 0, 0, player.Center, 0, 0))
            {

                Projectile.NewProjectile(source, new Vector2(posX, posY), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Duck with{Volume = 0.75f, Pitch = -0.25f}, player.Center);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<QuackStaff>(), 1)
            .AddIngredient(ItemID.FragmentSolar, 12)
            .AddIngredient(ItemID.FragmentVortex, 12)
            .AddIngredient(ItemID.FragmentNebula, 12)
            .AddIngredient(ItemID.FragmentStardust, 12)

            .AddTile(TileID.LunarCraftingStation)
            .Register();


        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}