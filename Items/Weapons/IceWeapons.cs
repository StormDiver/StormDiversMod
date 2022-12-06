using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;

namespace StormDiversMod.Items.Weapons
{
    public class IceGrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Grenade");
            Tooltip.SetDefault("Inflicts frostburn on enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            
            Item.damage = 60;         
            Item.DamageType = DamageClass.Ranged;         
            Item.width = 10;
            Item.height = 14;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;  
            Item.knockBack = 8;
            Item.value = Item.sellPrice(0, 0, 0, 16);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 5f;
            Item.shoot = ModContent.ProjectileType < Projectiles.IceGrenadeProj>();
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.noMelee = true;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
    
            SoundEngine.PlaySound(SoundID.Item1, position);
           
                return true;
            
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<IceGrenade>(), 10);
            recipe.AddIngredient(ItemID.Grenade, 10);
            recipe.AddIngredient(ItemID.IceTorch, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

        }
    }
    //_____________________________
    //__________________________________________________
    public class IceStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Icicle Staff");
            Tooltip.SetDefault("Rapidly shoots out damaging icicles");
            Item.staff[Item.type] = true;

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
         
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 15;
            Item.useAnimation = 15;
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
            Item.UseSound = SoundID.Item30 with { Volume = 0.5f};

            Item.damage = 14;

            Item.knockBack = 3f;

            Item.shoot = ModContent.ProjectileType<Projectiles.IceStaffProj>();

            Item.shootSpeed = 13f;


            Item.noMelee = true; //Does the weapon itself inflict damage?
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
            int numberProjectiles = 1 + Main.rand.Next(2); ; //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(6));
                float scale = 1f - (Main.rand.NextFloat() * .1f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            

            return false;
        }

        public override void AddRecipes()
        {
         

        }
       
    }
}