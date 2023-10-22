using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using StormDiversMod.Basefiles;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;


namespace StormDiversMod.Items.Weapons
{
	public class BloodSword : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName.SetDefault("Bloody Blade"); 
			//Tooltip.SetDefault("Shoots out a trail of blood every other swing");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() 
		{
			Item.damage = 20;

            Item.DamageType = DamageClass.Melee; 
            Item.width = 35;
			Item.height = 42;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;  
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            //Item.useTurn = true;
            Item.knockBack = 4;
            Item.scale = 1f;
            //Item.shoot = mod.ProjectileType("BloodSwordProj");
            Item.shoot = ModContent.ProjectileType<Projectiles.BloodSwordProj>();

            Item.shootSpeed = 8f;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(4) < 2)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5, 0f, 0f, 100, default, 1f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        int weaponattack = 2;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            weaponattack--;
            if (weaponattack <= 0)
            {    
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0)); // This defines the projectiles random spread . 10 degree spread.
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
                }
                SoundEngine.PlaySound(SoundID.NPCHit9, player.position);
                weaponattack = 2;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("StormDiversMod:EvilBars", 10)
           .AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 5)
           .AddTile(TileID.Anvils)
           .Register();
           
           
        }
   
    }
    //____________________________________________________________________________
    public class BloodSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heart Piercer");
            //Tooltip.SetDefault("Great for stabbing in a hurry");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.Spears[Item.type] = true;

        }
        /*public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (ModLoader.HasMod("TRAEProject"))//DON'T FORGET THIS!!!!!!!
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\nHold right click to charge and release to throw the spear";
                    }
                }

            }
        }*/
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 64;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 4f;
            Item.shoot = ModContent.ProjectileType<Projectiles.BloodSpearProj>();
            Item.shootSpeed = 6f;
            Item.noMelee = true;
            Item.noUseGraphic = true;

        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
         
                SoundEngine.PlaySound(SoundID.NPCHit9, player.position);
                
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddRecipeGroup("StormDiversMod:EvilBars", 10)
          .AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 5)
          .AddTile(TileID.Anvils)
          .Register();

        }
    }
    
    //____________________________________
    public class BloodYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Heart Attack");
            //Tooltip.SetDefault("Leaves behind a trail of damaging blood");
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 25;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            //Item.crit = 0;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 20;
            Item.height = 26;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.useTurn = true;
            Item.knockBack = 4f;
            Item.shoot = ModContent.ProjectileType<Projectiles.BloodYoyoProj>();
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.noUseGraphic = true;

        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {             
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
         .AddRecipeGroup("StormDiversMod:EvilBars", 10)
        .AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 5)
        .AddTile(TileID.Anvils)
        .Register();
        }
    }
    //_______________________
    //____________________________________________________
    public class BloodPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Grenade");
            //Tooltip.SetDefault("Bursts into multiple blood drops on destruction");
            Item.ResearchUnlockCount = 99;

        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Ranged;

            Item.width = 10;
            Item.height = 14;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 5;
            Item.shoot = ModContent.ProjectileType<Projectiles.BloodGrenadeProj>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<BloodPotion>(), 10);
            recipe.AddIngredient(ItemID.Grenade, 10);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

        }
    }
    //________________________________
    public class BloodWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Bloody Tendon");
            //Tooltip.SetDefault("8% summon tag critical strike chance\nYour summons will focus struck enemies\nDamaging blood orbs will orbit the targetted enemy when hit by summons");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.WhipProjs.BloodWhipProj>(), 20, 2, 4.5f, 30);
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.DamageType = DamageClass.SummonMeleeSpeed;

            Item.noMelee = true;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddRecipeGroup("StormDiversMod:EvilBars", 10)
        .AddIngredient(ModContent.ItemType<Items.Materials.BloodDrop>(), 5)
        .AddTile(TileID.Anvils)
        .Register();
        }
      
    }
}