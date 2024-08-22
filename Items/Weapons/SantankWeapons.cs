using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;
using StormDiversMod.Items.Materials;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Projectiles;
using Humanizer;
using StormDiversMod.Items.Tools;

namespace StormDiversMod.Items.Weapons
{
    public class SantankMinion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Santa's Staff of Command");
            //Tooltip.SetDefault("Summons a mini flying Santank minion to fight for you");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.knockBack = 2f;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<Projectiles.Minions.SantankMinionBuff>();
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.SantankMinionProj>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);

            // Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position.
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.SantankScrap>(), 18)
            .AddTile(TileID.MythrilAnvil)
            .Register();

        }
    }
    //______________________________________________________________________________
    public class SantaShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Santa's Explosive Boomstick");
            //Tooltip.SetDefault("Each shot creates an explosion at the end of the barrel, damaging any enemy within it
            //\nLeft click to fire each barrel in succession, creating an explosion both times
            //\nRight click to fire both barrels at once, resulting in a larger, more damaging explosion, and a larger spread of bullets
            //\nRight clicking while in the air will also propel you backwards");
            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0,9, 0, 0);
            Item.rare = ItemRarityID.Yellow;

            Item.useStyle = ItemUseStyleID.Shoot;

            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            
            Item.damage = 70;
            Item.knockBack = 8f;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 12f;
            Item.useTime = 15;
            Item.useAnimation = 30;
            Item.reuseDelay = 60;
            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) //right click
            {
                Item.useTime = 30;
                Item.reuseDelay = 60;
                return true;
            }
            else //left click
            {
                Item.useTime = 15;
                Item.reuseDelay = 60;
                return true;
            }
        }
        bool shot;
        public override void HoldItem(Player player)
        {
            if (player.ItemAnimationEndingOrEnded && !shot) //plays after firing
            {
                SoundEngine.PlaySound(SoundID.Item149 with { Volume = 2f, Pitch = 0f, MaxInstances = 1 }, player.Center);
                shot = true;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shot = false;
            //so speed modifers don't change position of explosion
            Vector2 VelocityExplode = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(player.Center.X, player.Center.Y)) * 16;

            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 40;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            if (player.altFunctionUse == 2) //Right Click
            {
                int numberofprojs = 8 + Main.rand.Next(3); //8-10
                for (int i = 0; i < numberofprojs; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20)); // This defines the projectiles random spread . 10 degree spread.
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y - (6 * player.gravDir)), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback * 1.5f, player.whoAmI);
                }
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y - (6 * player.gravDir)), new Vector2(VelocityExplode.X, VelocityExplode.Y), ModContent.ProjectileType<Projectiles.SantaBoomProj2>(), damage * 4, knockback * 1.5f, player.whoAmI);
                if (player.velocity.Y != 0)
                {
                    player.velocity = velocity * -1.25f;
                }
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { Volume = 2f, Pitch = -0.5f, }, player.Center);
            }
            else //left click
            {
                int numberofprojs = 4 + Main.rand.Next(2); //4-5
                for (int i = 0; i < numberofprojs; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10)); // This defines the projectiles random spread . 10 degree spread.
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y - (6 * player.gravDir)), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
                }
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y - (6 * player.gravDir)), new Vector2(VelocityExplode.X, VelocityExplode.Y), ModContent.ProjectileType<Projectiles.SantaBoomProj>(), damage * 2, knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, player.Center);
            }
            if (!GetInstance<ConfigurationsIndividual>().NoShake)
            {
                player.GetModPlayer<MiscFeatures>().screenshaker = true;
            }
            return false;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            //return !(player.itemAnimation < Item.useAnimation - 2);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<SantankScrap>(), 18)
             .AddTile(TileID.MythrilAnvil)
             .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/SantaShotgun_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}