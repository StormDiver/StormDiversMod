using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace StormDiversMod.Items.Weapons
{
    public class VoidSuitcase : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Void Suitcase");
            //Tooltip.SetDefault("Fire void pull enemies ect...");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Magic;
            //Item.UseSound = SoundID.Item13;
            Item.channel = true;
            Item.damage = 60;
            Item.knockBack = 2f;
            //Item.mana = 10;
            Item.shoot = ModContent.ProjectileType<Projectiles.VoidSuitcaseProj>();
            Item.shootSpeed = 20f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.statMana >= 60 && player.ownedProjectileCounts[Item.shoot] < 1) //requres 60 mana to fire
                return true;
            else
                return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Item.shootSpeed = 20f;
            if (player.ownedProjectileCounts[Item.shoot] < 1)
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.VoidSuitcaseProj>(), damage, knockback, player.whoAmI, 0, 0);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, -4);
        }

    }
}