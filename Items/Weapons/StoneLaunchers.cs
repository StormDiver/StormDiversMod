using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;
using System.Collections.Generic;

namespace StormDiversMod.Items.Weapons
{
    public class StoneAmmo : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.StoneBlock || item.type == ItemID.EbonstoneBlock || item.type == ItemID.CrimstoneBlock || item.type == ItemID.PearlstoneBlock)
            {
                item.ammo = ItemID.StoneBlock;
                item.notAmmo = true;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            /*foreach (TooltipLine line in tooltips)
            {
                if (item.type == ItemID.StoneBlock || item.type == ItemID.EbonstoneBlock || item.type == ItemID.CrimstoneBlock || item.type == ItemID.PearlstoneBlock)
                {
                    if (line.Mod == "Terraria" && line.Name == "Material")
                    {
                        line.Text = "Ammo\n" + line.Text;
                    }
                }
            }*/
        }
      
    }
    public class StoneThrower : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Cannon");
            Tooltip.SetDefault("Fire out all your unwanted stone at your foes");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.damage = 32;
            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<StoneProj>();
            //Item.useAmmo = ItemType<Ammo.StoneShot>();
            if (ModLoader.HasMod("ThoriumMod"))
            {
                Item.useAmmo = ItemType<StoneShot>();
            }
            else
            {
                Item.useAmmo = ItemID.StoneBlock;
            }
            Item.UseSound = SoundID.Item61;


            //Item.crit = 0;
            Item.knockBack = 5f;

            Item.shootSpeed = 11f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (ModLoader.HasMod("ThoriumMod"))
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\nRequires Compact Boulders, craft more with stone";
                    }
                }
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, -2);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 35f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            //velocity.X = velocity.X + player.velocity.X;
            //velocity.Y = velocity.Y + player.velocity.Y;
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 4), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StoneProj>(), damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.IllegalGunParts, 1)
         .AddIngredient(ItemID.StoneBlock, 250)
         .AddRecipeGroup("StormDiversMod:EvilMaterial", 25)
         .AddRecipeGroup(RecipeGroupID.IronBar, 25)
         .AddTile(TileID.Anvils)
         .Register();



        }

    }
    //_______________________________________________________________________________
    public class StoneThrowerHard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mega Stone Cannon");
            Tooltip.SetDefault("An upgraded stone cannon which makes stone far more deadly");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 7, 50, 0);
            Item.rare = ItemRarityID.LightPurple;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.damage = 64;
            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<StoneProj>();
            //Item.useAmmo = ItemType<Ammo.StoneShot>();
            if (ModLoader.HasMod("ThoriumMod"))
            {
                Item.useAmmo = ItemType<StoneShot>();
            }
            else
            {
                Item.useAmmo = ItemID.StoneBlock;
            }
            Item.UseSound = SoundID.Item61;


            //Item.crit = 0;
            Item.knockBack = 8f;

            Item.shootSpeed = 13f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (ModLoader.HasMod("ThoriumMod"))
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\nRequires Compact Boulders, craft more with stone";
                    }

                }

            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-14, -4);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
           
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 6), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StoneHardProj>(), damage, knockback, player.whoAmI);

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<StoneThrower>(), 1)
           .AddIngredient(ItemID.SoulofMight, 10)
           .AddIngredient(ItemID.SoulofSight, 10)
           .AddIngredient(ItemID.SoulofFright, 10)
           .AddIngredient(ItemID.HallowedBar, 15)
           .AddTile(TileID.MythrilAnvil)
           .Register();

        }

    }
    //_______________________________________________________________________________
    public class StoneThrowerSuper : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flaming Stone Cannon");
            Tooltip.SetDefault("Superheats your stone into flaming stone boulders");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.damage = 82;
            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<StoneProj>();
            //Item.useAmmo = ItemType<Ammo.StoneShot>();
            if (ModLoader.HasMod("ThoriumMod"))
            {
                Item.useAmmo = ItemType<StoneShot>();
            }
            else
            {
                Item.useAmmo = ItemID.StoneBlock;
            }
            Item.UseSound = SoundID.Item38;

            //Item.crit = 0;
            Item.knockBack = 8f;

            Item.shootSpeed = 14f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (ModLoader.HasMod("ThoriumMod"))
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\nRequires Compact Boulders, craft more with stone";
                    }

                }

            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-18, -4);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!GetInstance<ConfigurationsIndividual>().NoShake)
            {
                player.GetModPlayer<MiscFeatures>().screenshaker = true;
            }

            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 55f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            int numberProjectiles = 2; //This defines how many projectiles to shot.
        
            for (int i = 0; i < numberProjectiles; i++)
            {

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 6), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StoneSuperProj>(), damage, knockback, player.whoAmI);
            }

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ModContent.ItemType<StoneThrowerHard>(), 1)
          .AddIngredient(ItemID.ShroomiteBar, 15)
          .AddIngredient(ItemID.SpectreBar, 15)
          .AddIngredient(ItemID.LunarTabletFragment, 10)
          .AddIngredient(ItemID.BeetleHusk, 10)
          .AddTile(TileID.MythrilAnvil)
          .Register();

        }

    }

    //_______________________________________________________________________________
    public class StoneThrowerSuperLunar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celestial Stone Cannon");
            Tooltip.SetDefault("Empowers your stone with the power of the celestial fragments");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.damage = 140;
            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<StoneProj>();
            //Item.useAmmo = ItemType<Ammo.StoneShot>();
            if (ModLoader.HasMod("ThoriumMod"))
            {
                Item.useAmmo = ItemType<StoneShot>();
            }
            else
            {
                Item.useAmmo = ItemID.StoneBlock;
            }
            Item.UseSound = SoundID.Item38;

            Item.crit = 12;
            Item.knockBack = 8f;

            Item.shootSpeed = 17f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (ModLoader.HasMod("ThoriumMod"))
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\nRequires Compact Boulders, craft more with stone";
                    }

                }

            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-18, -4);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!GetInstance<ConfigurationsIndividual>().NoShake)
            {
                player.GetModPlayer<MiscFeatures>().screenshaker = true;
            }
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 55f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            int choice = Main.rand.Next(4);
            if (choice == 0)
            {
                {

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(8));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 6), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StoneSolar>(), damage, knockback, player.whoAmI);
                }
            }
            else if (choice == 1)
            {
                {

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 6), new Vector2(perturbedSpeed.X * 1f, perturbedSpeed.Y * 1f), ModContent.ProjectileType<StoneVortex>(), damage, knockback, player.whoAmI);

                }
            }
            else if (choice == 2)
            {
                for (int i = 0; i < 3; i++)
                {

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 6), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StoneNebula>(), damage, knockback, player.whoAmI);

                }
            }
            else if (choice == 3)
            {

                {

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10));
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 6), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<StoneStardust>(), damage, knockback, player.whoAmI);

                }
            }
            /*for (int i = 0; i < 3; i++)
            {

                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                Projectile.NewProjectile(position.X, position.Y, (float)(perturbedSpeed.X), (float)(perturbedSpeed.Y), mod.ProjectileType("StoneSuperProj"), damage, knockBack, player.whoAmI);
            }*/

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<StoneThrowerSuper>(), 1)
         .AddIngredient(ItemID.FragmentSolar, 15)
         .AddIngredient(ItemID.FragmentVortex, 15)
         .AddIngredient(ItemID.FragmentNebula, 15)
         .AddIngredient(ItemID.FragmentStardust, 15)
         .AddIngredient(ItemID.LunarBar, 15)
         .AddTile(TileID.LunarCraftingStation)
         .Register();

           
        }
       
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/StoneThrowerSuperLunar_Glow");


            //spriteBatch.Draw(texture, Item.Center - Main.screenPosition, new Rectangle(0, 0, Item.width, Item.height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw
           (
               texture,
               new Vector2
               (
                   Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                   Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f
               ),
               new Rectangle(0, 0, texture.Width, texture.Height),
               Color.White,
               rotation,
               texture.Size() * 0.5f,
               scale,
               SpriteEffects.None,
               0f
           );
        }
    }
    public class Bouldershake : ModPlayer
    {
       /* int shaketimer;
        public override void ModifyScreenPosition()
        {
            if (Player.itemTime == Player.HeldItem.useTime - 1 && Player.HeldItem.type == ModContent.ItemType<Items.Weapons.StoneThrowerSuperLunar>())
            {
                shaketimer = 10;
               
            }
            if (shaketimer > 0)
            {
                Main.screenPosition += new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
                shaketimer--;
            }
            if (Player.HeldItem.type != ModContent.ItemType<Items.Weapons.StoneThrowerSuperLunar>())
            {
                shaketimer = 0;
            }
        }*/
    }
}