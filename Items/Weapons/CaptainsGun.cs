using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Humanizer;

namespace StormDiversMod.Items.Weapons
{
    public class CaptainsGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Captains Cannon");
            //Tooltip.SetDefault("Rapidly fires out bullets, has a slow start up speed but builds up to full speed after firing for a few seconds
            //Right click while firing to launch a cannonball, requires cannonballs as ammo");

            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.reuseDelay = 20;
            Item.useTurn = false;
            Item.autoReuse = true;
            //Item.UseSound = SoundID.Item41;
            Item.DamageType = DamageClass.Ranged;

            Item.damage = 23;
            Item.knockBack = 2f;

            Item.shoot = ModContent.ProjectileType<Projectiles.CaptainsGunProj>();
            Item.shootSpeed = 20f;

            Item.useAmmo = AmmoID.Bullet;
            Item.channel = true;
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 5);
        }

        //int secondfire = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Item.shootSpeed = 20f;

            if (player.ownedProjectileCounts[Item.shoot] < 1)
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.CaptainsGunProj>(), damage, knockback, player.whoAmI);

            return false;
        }
        /*public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(27));
        }*/
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }      
        public override void AddRecipes()
        {
           
        }
    }
}