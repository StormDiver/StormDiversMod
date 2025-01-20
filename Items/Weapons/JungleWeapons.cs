using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Terraria.GameContent;
using StormDiversMod.Projectiles;

namespace StormDiversMod.Items.Weapons
{
    [LegacyName("MossRepeater")]
    public class JungleRepeater : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mossy Repeater");
            //Tooltip.SetDefault("Seems a little old and neglected, but should still work");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 45;
            Item.height = 24;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            //Item.UseSound = SoundID.Item5;

            Item.damage = 22;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 7f;

            Item.useAmmo = AmmoID.Arrow;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }

        float pitch;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
          
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 15;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(1));
            float scale = 1f - (Main.rand.NextFloat() * .4f);
            perturbedSpeed = perturbedSpeed * scale;

            float scale100 = scale * 100; // times 100 to prevent it rounding to 0 
            int damagemulti = (int)scale100 + 20; //Convert float to int
            damage *= damagemulti; // Multiple damage by damage 2 then divide it by 100 in projectile shoot
            /*if (scale > 0.85f)
            {
                damage = (damage * 20) / 17;

            }
            if (scale < 0.75f)
            {
                damage = (damage * 17) / 20;

            }*/
            //Main.NewText("Tester " + scale, 0, 204, 170); //Inital Scale
            //Main.NewText("Tester " + damage1, 0, 204, 170); //Times 100
            //Main.NewText("Tester " + damage22, 0, 204, 170); //Rounded and extra 20

            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage / 100, knockback, player.whoAmI);
     
            pitch = scale - 1;

            SoundEngine.PlaySound(SoundID.Item5 with { Volume = 1f, Pitch = pitch }, position);
            return false;
        }
        
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
       
    }
    //___________________________________________________________________________________________
    public class JungleSentry : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Jungle Tree Staff");
            //Tooltip.SetDefault("Summons a jungle tree that launches out a bunch of thorn balls");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            //Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 10;
            Item.UseSound = SoundID.Item45;

            Item.damage = 17;
            //Item.crit = 4;
            Item.knockBack = 1.5f;

            Item.shoot = ModContent.ProjectileType<Projectiles.SentryProjs.JungleSentryProj>();

            //Item.shootSpeed = 3.5f;



            Item.noMelee = true;
        }
        public override bool CanUseItem(Player player)
        {
            /*if (Collision.CanHitLine(Main.MouseWorld, 1, 1, player.position, player.width, player.height))
            {
                return true;
            }
            else
            {
                return false;
            }*/
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y - 33), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;
            return false;

            /*position = Main.MouseWorld;
            position.ToTileCoordinates();
            while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[] { new Conditions.IsSolid() }), out _))
            {
                position.Y++;
                position.ToTileCoordinates();
            }
            position.Y -= 32;
            return true;*/

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.RichMahogany, 50)          
            .AddIngredient(ItemID.JungleSpores, 10)
            .AddIngredient(ItemID.Vine, 5)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
    public class JungleSporeGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Jungle Spore Gun");
            //Tooltip.SetDefault("Shoots poisonous spores that explode upon hitting an enemy or tile");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
                Item.mana = 11;
            else
                Item.mana = 7;
            Item.UseSound = SoundID.Item8;
            Item.damage = 25;
            Item.knockBack = 2f;
            Item.shoot = ModContent.ProjectileType<JungleSporeProj>();
            Item.shootSpeed = 12;
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, -0);
        }
        float accuracy = 0; //The amount of spread
        public override void HoldItem(Player player)
        {
           
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(3));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 4), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);

            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            //velocity = velocity.RotatedByRandom(MathHelper.ToRadians(accuracy));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.RichMahogany, 50)
            .AddIngredient(ItemID.JungleSpores, 10)
               .AddRecipeGroup("StormDiversMod:EvilMaterial", 5)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}