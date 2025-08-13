using Microsoft.Xna.Framework;
using StormDiversMod.Common;
using StormDiversMod.Items.Weapons;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.Items.Tools
{
	public class GlassPick : ModItem
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName.SetDefault("Glass Pickaxe"); 
			//Tooltip.SetDefault("Fragile");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults() 
		{
			Item.damage = 6;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
			Item.height = 30;
			Item.useTime = 12;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;
            Item.pick = 60;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = true;
            Item.knockBack = 1;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (GetInstance<ConfigurationsGlobal>().GlassSoWeak)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        line.Text = "Shatters on impact with tiles";
                }
            }
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int shardcount = Main.rand.Next(4, 6); //4-5 shards
            for (int i = 0; i < shardcount; i++) //summon dust and projectile
            {
                Projectile.NewProjectile(null, new Vector2(target.position.X + Main.rand.Next(0, target.width), target.position.Y + Main.rand.Next(0, target.height)), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.GlassShardProj>(), Item.damage / 3, 0);
            }
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0f }, target.Center);
            Item.TurnToAir();
            player.QuickSpawnItem(null, ItemType<Items.Tools.GlassHandle>(), 1);

            base.OnHitNPC(player, target, hit, damageDone);

        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Glass, 10)
           .AddTile(TileID.Furnaces)
           .Register();

            CreateRecipe()
           .AddIngredient(ModContent.ItemType<GlassHandle>(), 1)
           .AddIngredient(ItemID.Glass, 5)
           .AddIngredient(ItemID.Gel, 5)
          .Register();
        }
    }
    public class GlassAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glass Axe");
            //Tooltip.SetDefault("Weak");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 14;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;
            Item.axe = 12;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.knockBack = 2;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (GetInstance<ConfigurationsGlobal>().GlassSoWeak)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        line.Text = "Shatters on impact with trees";
                }
            }
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int shardcount = Main.rand.Next(4, 6); //4-5 shards
            for (int i = 0; i < shardcount; i++) //summon dust and projectile
            {
                Projectile.NewProjectile(null, new Vector2(target.position.X + Main.rand.Next(0, target.width), target.position.Y + Main.rand.Next(0, target.height)), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.GlassShardProj>(), Item.damage / 3, 0);
            }
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0f }, target.Center);
            Item.TurnToAir();
            player.QuickSpawnItem(null, ItemType<Items.Tools.GlassHandle>(), 1);

            base.OnHitNPC(player, target, hit, damageDone);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Glass, 8)
           .AddTile(TileID.Furnaces)
           .Register();

            CreateRecipe()
         .AddIngredient(ModContent.ItemType<GlassHandle>(), 1)
         .AddIngredient(ItemID.Glass, 4)
         .AddIngredient(ItemID.Gel, 4)
         .Register();
        }
    }
    public class GlassHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glass Hammer");
            //Tooltip.SetDefault("Smashed");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 9;

            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 18;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;
            Item.hammer = 60;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.knockBack = 3;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (GetInstance<ConfigurationsGlobal>().GlassSoWeak)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        line.Text = "Shatters on impact with walls";
                }
            }
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int shardcount = Main.rand.Next(4, 6); //4-5 shards
            for (int i = 0; i < shardcount; i++) //summon dust and projectile
            {
                Projectile.NewProjectile(null, new Vector2(target.position.X + Main.rand.Next(0, target.width), target.position.Y + Main.rand.Next(0, target.height)), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.GlassShardProj>(), Item.damage / 3, 0);
            }
            SoundEngine.PlaySound(SoundID.Item107 with { Volume = 2f, Pitch = -0f }, target.Center);
            Item.TurnToAir();
            player.QuickSpawnItem(null, ItemType<Items.Tools.GlassHandle>(), 1);

            base.OnHitNPC(player, target, hit, damageDone);
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
         .AddIngredient(ModContent.ItemType<GlassHandle>(), 1)
         .AddIngredient(ItemID.Glass, 4)
         .AddIngredient(ItemID.Gel, 4)
         .Register();
        }
    }
    public class GlassHandle : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Broken Glass Handle"); 
            //Tooltip.SetDefault("What did you expect from a tool made of fucking glass?");
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
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Gray;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            //Item.useTurn = true;
            Item.knockBack = 1;
            Item.scale = 1f;
            Item.noMelee = true;
            Item.noGrabDelay = 0;
        }
        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange *= 3;
        }
        public override bool GrabStyle(Player player)
        {
            Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(Item.Center.X, Item.Center.Y)) * 15;
            Item.velocity = velocity;
            return true;
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
}