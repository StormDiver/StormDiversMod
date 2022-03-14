using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Buffs;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;

namespace StormDiversMod.Items.Weapons
{
    public class BloodyRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Rifle");
            Tooltip.SetDefault("Converts regular bullets into blood bullets that drop blood in flight\nRight Click to zoom out");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item40;

            Item.damage = 70;

            Item.knockBack = 2f;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 16f;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true;
        }


        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 1);
        }



        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (type == ProjectileID.Bullet)
            {
                type = ModContent.ProjectileType<Projectiles.BloodyBulletProj>();
            }


            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);


            return false;
        }
        public override void HoldItem(Player player)
        {
            player.scope = true;
        }

        public override void AddRecipes()
        {

            //In new recipes with other hardmode blood moon recipes
          



        }

    }
   
    
}