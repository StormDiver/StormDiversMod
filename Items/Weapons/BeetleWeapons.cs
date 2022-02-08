using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Weapons
{
    public class BeetleShellWeapon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Beetle Shell");
            Tooltip.SetDefault("Summons beetles on impact that attack and swarm your foes");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 67;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            
            Item.damage = 70;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 22;
           
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.knockBack = 8;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<Projectiles.BeetleShellProj>();
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;

        }
        /* public override bool AltFunctionUse(Player player)
         {
             return true;
         }
         public override bool CanUseItem(Player player)       
         {

             if (player.altFunctionUse == 2)
             {
                 Item.useTime = 30;
                 Item.useAnimation = 30;

             }
             else
             {
                 Item.useTime = 12;
                 Item.useAnimation = 12;

             }
             return player.ownedProjectileCounts[Item.shoot] < 8;
         }*/
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           

            SoundEngine.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 1);
           
                return true;
            
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<TurtleShellWeapon>(), 1)
            .AddIngredient(ItemID.BeetleHusk, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
          
        }
    }
    //__________________________________________________________________
    public class BeetleSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beetle Lance");
            Tooltip.SetDefault("Summons beetles that attack and swarm your foes");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 67;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            //Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 60;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 5f;
            Item.shoot = ModContent.ProjectileType < Projectiles.BeetleSpearProj>();
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.noUseGraphic = true;

        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<TurtleSpear>(), 1)
           .AddIngredient(ItemID.BeetleHusk, 10)
           .AddTile(TileID.MythrilAnvil)
           .Register();
           
        }
    }
    //___________________________________________________________________________________
    public class BeetleYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Thorax");
            Tooltip.SetDefault("Summons beetles that attack and swarm your foes");
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 30;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 67;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 85;
            //Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 26;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.useTurn = true;
            Item.knockBack = 4f;
            Item.shoot = ModContent.ProjectileType < Projectiles.BeetleYoyoProj>();
            // Item.shootSpeed = 9f;
            Item.noMelee = true;
            Item.noUseGraphic = true;

        }


        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<TurtleYoyo>(), 1)
            .AddIngredient(ItemID.BeetleHusk, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}