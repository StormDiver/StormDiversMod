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

namespace StormDiversMod.Items.Weapons
{
    public class TombStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tombstone Staff");
            Tooltip.SetDefault("Fires out magical tombstones that bounce around\nTombstones spawn a miniature ghost upon hitting an enemy");
            Item.staff[Item.type] = true;

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 34;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic; 
            Item.mana = 10;
            Item.UseSound = SoundID.Item8;

            Item.damage = 17;
         
            Item.knockBack = 4f;

            Item.shoot = ModContent.ProjectileType<TombProj>();

            Item.shootSpeed = 16f;
   
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
          
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            
            {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(3));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
           
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddRecipeGroup("StormDiversMod:Tombstones", 5)
           .AddRecipeGroup("StormDiversMod:EvilMaterial", 20)   
           .AddTile(TileID.Anvils)
           .Register();
           
        }
       
    }
    //_____________________________________________________________________________________________________________________________
   
}