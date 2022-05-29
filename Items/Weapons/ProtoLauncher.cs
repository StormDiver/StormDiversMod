using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
    public class ProtoLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prototype Launcher");
            Tooltip.SetDefault("Fires out impact-exploding grenades that have a small chance to prematurely explode into shrapnel\nRequires Prototype Grenades, purchase more from the Demolitionist");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            //Item.reuseDelay = 30;
            Item.useTurn = false;
            Item.autoReuse = false;

            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<ProtoGrenadeProj>();
            Item.useAmmo = ItemType<Ammo.ProtoGrenade>();
            Item.UseSound = SoundID.Item61;

            Item.damage = 30;
            //Item.crit = 4;
            Item.knockBack = 3f;
            Item.shootSpeed = 10f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
 
            for (int i = 0; i < 1; i++)
            {
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item61, position);

            }

           
            
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddIngredient(ItemID.Bone, 100)
            .AddRecipeGroup(RecipeGroupID.IronBar, 25)
            .AddTile(TileID.Anvils)
            .Register();
            
        }
       
      
   }
}
 