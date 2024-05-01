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
	public class EyeSword : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName.SetDefault("Eye Sored"); 
			//Tooltip.SetDefault("Launches a bouncing eyeball every swing");

            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EyeGun>();

        }

        public override void SetDefaults() 
		{
			Item.damage = 25;

            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
			Item.height = 50;
			Item.useTime = 28;
			Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = false;
            Item.noUseGraphic = false;
            Item.noMelee = false;

            Item.knockBack = 3;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Projectiles.EyeSwordProj>();
            Item.shootSpeed = 11f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
            SoundEngine.PlaySound(SoundID.NPCHit9, player.Center);
            return false;
        }
       

    }
    //_______________________________________________________________________
    public class EyeGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eye Rifle");
            //Tooltip.SetDefault("Fires 2 bullets in rapid succession\nOnly the first shot consumes ammo");

            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EyeStaff>();

        }
        public override void SetDefaults()
        {

            Item.width = 50;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.reuseDelay = 35;
            Item.useTurn = false;
            Item.autoReuse = true;
            //Item.UseSound = SoundID.Item11;
            Item.DamageType = DamageClass.Ranged;


            Item.damage = 26;
            Item.knockBack = 5f;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 10f;

            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }


        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        //int secondfire = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item11, position);

            Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 4), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
            return false;

        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
          
            return !(player.itemAnimation < Item.useAnimation - 2);

        }

    }
    //_________________________________________________________________________
    public class EyeStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Eyestalk");
            //Tooltip.SetDefault("Summons a piercing eyeball that ricochets back towards you");
            Item.staff[Item.type] = true;

            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EyeMinion>();

        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 54;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item8;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 28;
            Item.knockBack = 2f;
            Item.shoot = ModContent.ProjectileType<Projectiles.EyeStaffProj>();
            Item.shootSpeed = 12f;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 12;
            }
            else
            {
                Item.mana = 8;
            }
            Item.noMelee = true; //Does the weapon itself inflict damage?

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }


            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);

            return false;

        }
       
    }
    //_________________________________________________________
    public class EyeMinion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eyeball Staff");
            //Tooltip.SetDefault("Summons a Servant of Cthulhu minion to fight for you");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EyeSword>();

        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 46;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.autoReuse = true;
            // Item.UseSound = SoundID.Item43;

            Item.damage = 12;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item43;

            Item.mana = 10;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;

            Item.UseSound = SoundID.Item44;

            Item.buffType = ModContent.BuffType<Projectiles.Minions.EyeMinionBuff>();
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.EyeMinionProj>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(8, 8);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            player.AddBuff(Item.buffType, 2);

            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);

            return false;



        }

       
    }
}