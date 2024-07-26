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
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;

namespace StormDiversMod.Items.Weapons
{
	public class WoodPointyStick : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName.SetDefault("Pointy Wooden Stick"); 
			//Tooltip.SetDefault("Launches a sharp leaf every stab\n'Poke it with a stick'");
            Item.ResearchUnlockCount = 1;
          
        }

		public override void SetDefaults() 
		{
			Item.damage = 12;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
			Item.height = 30;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Rapier;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Projectiles.WoodPointyStickProj>();
            Item.shootSpeed = 2.2f;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
           
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           int projid = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X * 4f, velocity.Y * 4f), ModContent.ProjectileType<WoodPointyStickProj2>(), (int)(damage * .4f), 0.5f, player.whoAmI);

            return true;
        }

        public override void AddRecipes()
        {
        
        }
       
    }
    //_______________________________________________________________________________
    //_______________________________________________________________________________________________
    public class WoodCrossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wooden Crossbow");
            //Tooltip.SetDefault("Converts all arrows into piercing crossbow bolts\n'Slow but powerful'");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 34;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.White;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            //Item.UseSound = SoundID.Item5;
            Item.damage = 18;
            Item.knockBack = 3f;
            Item.shoot = ModContent.ProjectileType<Projectiles.WoodenBoltProj>();
            Item.shootSpeed = 5f;
            Item.useAmmo = AmmoID.Arrow;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2, -4);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.WoodenBoltProj>(), damage, knockback, player.whoAmI);
            SoundEngine.PlaySound(SoundID.Item5 with{Volume = 1f, Pitch = 0.2f}, player.Center);

            return false;
        }

        public override void AddRecipes()
        {
          

        }
    }
}