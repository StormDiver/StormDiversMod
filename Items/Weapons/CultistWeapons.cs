using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Common;
using System.Collections.Generic;


namespace StormDiversMod.Items.Weapons
{
    public class CultistSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lunatic Spear of Fire");
            //Tooltip.SetDefault("Launches a spread of 5 wall piercing fireballs each stab");

            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });

            ItemID.Sets.Spears[Item.type] = true;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CultistBow>();

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
            Item.damage = 85;
            Item.crit = 16;
            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 5f;
            Item.shoot = ModContent.ProjectileType <Projectiles.CultistSpearProj>();
            Item.shootSpeed = 7.5f;
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
            return true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/CultistSpear_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
        //Drop rate in NPCEffects/ Luantic Cultist treasure bag
    }
    //________________________________________________________________________________
    public class CultistBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lunatic Bow of Ice");
            //Tooltip.SetDefault("Fires out an ice arrow alongside 2 to 3 regular arrows which rains down icicles on impact\nThe ice arrow inflicts Ultrafreeze that deals high damage over time and slows enemies
            //");

            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CultistTome>();
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 46;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item5;

            Item.damage = 65;
            //Item.crit = 4;
            Item.knockBack = 2f;

            Item.shoot = ProjectileID.WoodenArrowFriendly;

            Item.shootSpeed = 10f;

            Item.useAmmo = AmmoID.Arrow;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
         

            SoundEngine.PlaySound(SoundID.Item30, player.Center);


            int numberProjectiles = 2 + Main.rand.Next(2); ; //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed2 = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(15)); // 
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed2.X, perturbedSpeed2.Y), type, damage, knockback, player.whoAmI);
            }
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(5));

            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X * 1.2f, perturbedSpeed.Y * 1.2f), ModContent.ProjectileType<Projectiles.CultistBowProj>(), damage, knockback, player.whoAmI);

            return false;

        }
        //Drop rate in NPCEffects/ Luantic Cultist treasure bag
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/CultistBow_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

    }
    //__________________________________________________________________________________________________________
    //__________________________________________________
    public class CultistTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lunatic Spell of Ancient Light");
            //Tooltip.SetDefault("Summons multiple ancient lights that seek out enemies");

            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CultistStaff>();

        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 23;
            }
            else
            {
                Item.mana = 15;
            }
            Item.UseSound = SoundID.Item9;

            Item.damage = 40;

            Item.knockBack = 4f;

            Item.shoot = ModContent.ProjectileType<Projectiles.CultistTomeProj>(); 

            Item.shootSpeed = 10f;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            for (int i = 0; i < 20; i++) 
            {
                int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 135, 0, 0, 120, default, 1.5f);   
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 2.5f;
            }
            int numberProjectiles = 3 + Main.rand.Next(2); ; //3-4
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed2 = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(25)); // 
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed2.X, perturbedSpeed2.Y), type, damage, knockback, player.whoAmI);
            }
            return false;
           
        }
        //Drop rate in NPCEffects/ Luantic Cultist treasure bag
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/CultistTome_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //_______________________________________________________________________
    public class CultistStaff : ModItem 
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lunatic Staff of Lightning");
            //Tooltip.SetDefault("Summons a lightning orb sentry that rapidly fires lightning bolts at enemies\nRight click to target a specific enemy"); 
            Item.staff[Item.type] = true;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;


            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CultistSpear>();

        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 10;
            Item.UseSound = SoundID.Item122;

            Item.damage = 70;

            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.SentryProjs.CultistSentryProj>();

            Item.shootSpeed = 1f;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;
            return false;
          
        }
        //Drop rate in NPCEffects/ Luantic Cultist treasure bag
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/CultistStaff_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}