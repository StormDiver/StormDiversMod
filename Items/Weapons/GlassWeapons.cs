using ExampleMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Common;
using StormDiversMod.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;
using static Terraria.ModLoader.ModContent;


namespace StormDiversMod.Items.Weapons
{
	public class GlassSword : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName.SetDefault("Glass Sword"); 
			//Tooltip.SetDefault("Might be fragile");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() 
		{
			Item.damage = 15;
            Item.DamageType = DamageClass.Melee; 
            Item.width = 35;
			Item.height = 35;
			Item.useTime = 15;
			Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
            //Item.useTurn = true;
            Item.knockBack = 3.5f;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Projectiles.GlassShardProj>();
            Item.shootSpeed = 1f;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (GetInstance<ConfigurationsGlobal>().GlassSoWeak)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        line.Text = "Shatters into damaging shards on impact with enemies";
                }
            }
        }
        int breakchance;
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!GetInstance<ConfigurationsGlobal>().GlassSoWeak)
                breakchance = 32;
            else
                breakchance = 1;
            if (Main.rand.Next(breakchance) == 0)
            {
                int shardcount = Main.rand.Next(5, 7); //5-6 shards
                for (int i = 0; i < shardcount; i++) //summon dust and projectile
                {
                    var dust = Dust.NewDustDirect(new Vector2(player.position.X, player.position.Y), player.width, player.height, 149, 0, 0);
                    Projectile.NewProjectile(null, new Vector2(target.position.X + Main.rand.Next(0, target.width), target.position.Y + Main.rand.Next(0, target.height)), new Vector2(0, 0), Item.shoot, Item.damage / 3, 0);
                }
                SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0f }, target.Center);
                Item.TurnToAir();
                player.QuickSpawnItem(null, ItemType<Items.Weapons.GlassSwordBroken>(), 1);
            }
            base.OnHitNPC(player, target, hit, damageDone);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Glass, 8)
           .AddTile(TileID.Furnaces)
           .Register();

            CreateRecipe()
           .AddIngredient(ModContent.ItemType<GlassSwordBroken>(), 1)
           .AddIngredient(ItemID.Glass, 4)
           .AddIngredient(ItemID.Gel, 4)
           .Register();
        }
    }
    public class GlassSwordBroken : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Broken Glass Sword"); 
            //Tooltip.SetDefault("What did you expect from a sword made of fucking glass?");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.IgnoresEncumberingStone[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Gray;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.knockBack = 1;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Projectiles.GlassShardProj>();

            Item.shootSpeed = 1f;
        }
        public override bool AllowPrefix(int pre)
        {
            return false;
        }
        public override bool WeaponPrefix()
        {
            return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
    //______________________________________
    public class GlassBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glass Bow");
            //Tooltip.SetDefault("Seriosuly?");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useTurn = false;
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Ranged;
            Item.UseSound = SoundID.Item5;
            Item.damage = 12;
            Item.knockBack = 3f;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 6f;
            Item.useAmmo = AmmoID.Arrow;
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (GetInstance<ConfigurationsGlobal>().GlassSoWeak)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        line.Text = "Shatters into damaging shards on use";
                }
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        int breakchance;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!GetInstance<ConfigurationsGlobal>().GlassSoWeak)
                breakchance = 32;
            else
                breakchance = 1;
            if (Main.rand.Next(breakchance) == 0)
            {
                int shardcount = Main.rand.Next(5, 7); //5-6 shards
                for (int i = 0; i < shardcount; i++) //summon dust and projectile
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(8) * 0.33f); // 
                    float scale = 1f - (Main.rand.NextFloat() * .1f);
                    perturbedSpeed = perturbedSpeed * scale;
                    var dust = Dust.NewDustDirect(new Vector2(player.position.X, player.position.Y), player.width, player.height, 149, 0, 0);
                    Projectile.NewProjectile(null, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.GlassShardProj>(), Item.damage / 3, 0);
                }
                SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0f }, player.Center);
                Item.TurnToAir();
                player.QuickSpawnItem(null, ItemType<Items.Weapons.GlassBowBroken>(), 1);
                return false;
            }
            else
                return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Glass, 7)
           .AddTile(TileID.Furnaces)
           .Register();

            CreateRecipe()
           .AddIngredient(ModContent.ItemType<GlassBowBroken>(), 1)
           .AddIngredient(ItemID.Glass, 4)
           .AddIngredient(ItemID.Gel, 4)
           .Register();
        }
    }
    public class GlassBowBroken : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Broken Glass Bow"); 
            //Tooltip.SetDefault("What did you expect from a bow made of fucking glass?");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.IgnoresEncumberingStone[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Gray;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.knockBack = 0;
            Item.scale = 1f;
            Item.noMelee = true;
        }
        public override bool AllowPrefix(int pre)
        {
            return false;
        }
        public override bool WeaponPrefix()
        {
            return false;
        }
    }
    //__________________________________________________
    public class GlassStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glass Staff");
            //Tooltip.SetDefault("Still weak");
            Item.staff[Item.type] = true;

            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useTurn = false;
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
                Item.mana = 8;
            else
                Item.mana = 5;
            Item.UseSound = SoundID.Item43;
            Item.damage = 15;
            Item.knockBack = 3f;
            Item.shoot = ModContent.ProjectileType<Projectiles.GlassStaffProj>();
            Item.shootSpeed = 9.5f;
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (GetInstance<ConfigurationsGlobal>().GlassSoWeak)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        line.Text = "Shatters into damaging shards on use";
                }
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        int breakchance;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 60f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            if (!GetInstance<ConfigurationsGlobal>().GlassSoWeak)
                breakchance = 32;
            else
                breakchance = 1;
            if (Main.rand.Next(breakchance) == 0)
            {
                int shardcount = Main.rand.Next(5, 7); //5-6 shards
                for (int i = 0; i < shardcount; i++) //summon dust and projectile
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(8) * 0.33f); // 
                    float scale = 1f - (Main.rand.NextFloat() * .1f);
                    perturbedSpeed = perturbedSpeed * scale;
                    var dust = Dust.NewDustDirect(new Vector2(position.X, position.Y), 0, 0, 149, 0, 0);
                    Projectile.NewProjectile(null, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.GlassShardProj>(), Item.damage / 3, 0);
                }
                SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0f }, player.Center);
                Item.TurnToAir();
                player.QuickSpawnItem(null, ItemType<Items.Weapons.GlassStaffBroken>(), 1);
                return false;
            }
            else
                return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Glass, 10)
           .AddTile(TileID.Furnaces)
           .Register();

            CreateRecipe()
           .AddIngredient(ModContent.ItemType<GlassStaffBroken>(), 1)
           .AddIngredient(ItemID.Glass, 5)
           .AddIngredient(ItemID.Gel, 5)
           .Register();
        }
    }
    public class GlassStaffBroken : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Broken Glass Staff"); 
            //Tooltip.SetDefault("What did you expect from a staff made of fucking glass?");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.IgnoresEncumberingStone[Item.type] = true;
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Gray;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.knockBack = 0;
            Item.scale = 1f;
            Item.noMelee = true;
        }
        public override bool AllowPrefix(int pre)
        {
            return false;
        }
        public override bool WeaponPrefix()
        {
            return false;
        }
    }
}