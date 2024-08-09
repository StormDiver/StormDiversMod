using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Projectiles;
using static Terraria.ModLoader.ModContent;
using Humanizer;

namespace StormDiversMod.Items.Weapons
{
	public class RoseSickle : ModItem
	{
		public override void SetStaticDefaults() 
		{
            //DisplayName.SetDefault("The Crescent Rifle"); 
            //Left click to swing the scythe creating a large damaging aura that has a chance to destroy projectiles\nRight click to fire the rifle, has a strong recoil when not grounded and requires bullets as ammo
            //\n'A High Caliber Sniper Scythe, turn your enemies into dust''");
            Item.ResearchUnlockCount = 1;
            Item.staff[Item.type] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        int aura = 0;
		public override void SetDefaults() 
		{
			Item.damage = 80;
            Item.DamageType = DamageClass.Melee;
            Item.width = 75;
			Item.height = 75;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Shoot;  
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
			//Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 6;
            Item.shoot = ProjectileID.Bullet;
            aura = ModContent.ProjectileType<Projectiles.RoseAura>();
            Item.shootSpeed = 30;
            Item.scale = 1f;
            Item.noMelee = true;
            //Item.shootsEveryUse = true;
        }
        
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
         public override bool CanUseItem(Player player)
        {

            if (player.altFunctionUse == 2) //gun
            {
                //Item.DamageType = DamageClass.Ranged;
                aura = 0;
                Item.useAmmo = AmmoID.Bullet;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.reuseDelay = 12;
                Item.ArmorPenetration = 0;

            }
            else //scythe
            {
                //Item.DamageType = DamageClass.Melee;
                Item.useStyle = ItemUseStyleID.Swing;
                aura = ModContent.ProjectileType<Projectiles.RoseAura>();
                Item.useAmmo = 0;
                Item.reuseDelay = 0;
                Item.ArmorPenetration = 30;

            }
            return true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
        }
        public override void UseAnimation(Player player)
        {
            player.itemAnimationMax = Item.useAnimation;//seems to fix aura issue on first swing
            if (aura != 0 && !player.ItemAnimationActive)
            {
                Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                if (player.whoAmI == Main.myPlayer)
                {
                    Vector2 velocity = new Vector2(Math.Sign(mousePosition.X - player.Center.X), 0); // determines direction
                    int damage = (int)(player.GetTotalDamage(Item.DamageType).ApplyTo(Item.damage));
                    Projectile spawnedProj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.MountedCenter - velocity * 2, velocity * 5, aura, damage, Item.knockBack, Main.myPlayer,
                            Math.Sign(mousePosition.X - player.Center.X) * player.gravDir, player.itemAnimationMax, player.GetAdjustedItemScale(Item));
                    NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
                }
                return;
            }
        }
        Vector2 muzzleOffset = new Vector2(0 , 0);
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.altFunctionUse == 2)
                player.itemLocation += muzzleOffset * -1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);

            if (player.altFunctionUse == 2) //Right Click
            {

                muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 40;
                if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                {
                    position += muzzleOffset;
                }
                if (type == ProjectileID.Bullet)
                {
                    type = ProjectileID.BulletHighVelocity;
                }
                for (int i = 0; i < 1; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0)); 
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage * 2, knockback * 1.5f, player.whoAmI);
                    if (player.velocity.Y != 0)
                    {
                        player.velocity.X = velocity.X * -0.4f;
                        player.velocity.Y = velocity.Y * -0.4f;
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    int dust2 = Dust.NewDust(position + muzzleOffset * 1f, 0, 0, 218, velocity.X * 0.12f, velocity.Y * 0.12f);
                    Main.dust[dust2].noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.Item41 with { Volume = 1f, Pitch = -0.25f }, player.Center);
            }
            else //left Click
            {
                SoundEngine.PlaySound(SoundID.Item71, player.Center);
            }
            return false;
        }

        public override void AddRecipes()
        {
            /*CreateRecipe()
            .AddIngredient(ItemID.DeathSickle, 1)
            .AddIngredient(ItemID.SniperRifle, 1)
            .AddIngredient(ItemID.SpectreBar, 10)
           .AddTile(TileID.MythrilAnvil)
           .Register();*/
        }
    }
}