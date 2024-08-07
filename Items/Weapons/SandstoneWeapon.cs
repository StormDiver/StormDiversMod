using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
	
    public class SandstoneGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ancient Revolver");
            //Tooltip.SetDefault("Fires a burst of 6 bullets with only the first shot consuming ammo\nNeeds time to reload between bursts");

            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {

            Item.width = 50;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 6;
            Item.useAnimation = 32;
            Item.reuseDelay = 50;
            Item.useTurn = false;
            Item.autoReuse = true;
            //Item.UseSound = SoundID.Item41;
            Item.DamageType = DamageClass.Ranged;


            Item.damage = 9;
            Item.knockBack = 2f;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 10f;

            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }


        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, 0);
        }

        //int secondfire = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            SoundEngine.PlaySound(SoundID.Item41, position);
            {
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 4), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
            }

            return false;

        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(7));
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return !(player.itemAnimation < Item.useAnimation - 2);

        }
       
        public override void AddRecipes()
        {
           
        }
    }
   
}