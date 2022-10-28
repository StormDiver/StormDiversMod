using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Tools
{
    public class Aircan : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Compressed Air Can");
            Tooltip.SetDefault("Fires out a blast of air that blows enemies away\n'Great for clearing out cobwebs'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
            
        }
        public override void SetDefaults()
        {
            
            Item.width = 15;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(0, 0, 0, 10);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.consumable = true;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item13;

            Item.damage = 5;
            
            Item.knockBack = 16;
            Item.shoot = ModContent.ProjectileType<Projectiles.ToolsProjs.AircanProj>();
            
            Item.shootSpeed = 7f;

            //Item.useAmmo = AmmoID.Gel;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void HoldItem(Player player)
        {
            //player.armorPenetration = 0;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 8);
        }
      
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 20f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            for (int i = 0; i < 2; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);

            }

            return false;
        }
        
        

    }
}