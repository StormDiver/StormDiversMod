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
using StormDiversMod.Common;

namespace StormDiversMod.Items.Weapons
{
    public class ShotgunBlaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Overlocked Shotgun");
            //Tooltip.SetDefault("Very fast but conseutive shots greatly reduce accuracy");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 45;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            //Item.UseSound = SoundID.Item38;
            Item.damage = 20;
            Item.knockBack = 5f;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 12f;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true;
        }
        float accuracy = 5; //The amount of spread
        public override void UpdateInventory(Player player)
        {
            //if (player.controlUseTile)
            //   accuracy -= 0.1f;
            if (!player.controlUseItem && ((player.itemAnimation == 0 && player.HeldItem.type == ModContent.ItemType<ShotgunBlaster>()) || player.HeldItem.type != ModContent.ItemType<ShotgunBlaster>()))
            {
                if (accuracy > 5) //increase accuracy when not shooting
                    accuracy -= 0.5f; //1 degree every 2 frames, restores 50 degrees over 100 frames (1.66 second to full accuracy)
            }
            //Main.NewText("" + accuracy, 175, 17, 96);
            accuracy = (Math.Min(55, Math.Max(5f, accuracy))); //clamp between 5 and 55

            if (accuracy > 5)
                Item.SetNameOverride("Overclocked Shotgun - Weapon Stability: " + Math.Round(100 - ((accuracy - 5) * 2)) + "%");// for each %age lost reduce stability by 2% 
            else
                Item.SetNameOverride("Overclocked Shotgun - " + "Fully Stable!");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 20;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            if (type == ProjectileID.ChlorophyteBullet)
            {
                damage = (damage * 8) / 10;
            }
            int numberProjectiles = 4 + Main.rand.Next(1);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(accuracy));
                float scale = 1f - (Main.rand.NextFloat() * .25f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            //Main.NewText("" + accuracy, 175, 17, 96);
            //if (accuracy <= 20)
                SoundEngine.PlaySound(SoundID.Item36 with { Volume = 1f, Pitch = -0.05f + accuracy / 50 }, player.Center);
            if (accuracy > 25) //knock back and screenshake at low stability (<60%)
            {
                player.grappling[0] = -1; //Remove grapple hooks
                player.grapCount = 0;
                for (int p = 0; p < 1000; p++)
                {
                    if (Main.projectile[p].active && Main.projectile[p].owner == player.whoAmI && Main.projectile[p].aiStyle == 7)
                    {
                        Main.projectile[p].Kill();
                    }
                }
                player.GetModPlayer<MiscFeatures>().screenshaker = true;
                player.velocity = velocity * -0.8f * (accuracy / 100);

                //Main.NewText("" + velocity * -0.8f * (accuracy / 100), 175, 17, 96);
                //dust effects
                Vector2 muzzleOffset2 = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 40;
                if (Collision.CanHit(position, 0, 0, position + muzzleOffset2, 0, 0))
                    position += muzzleOffset2;
                for (int i = 0; i < accuracy / 5; i++)
                {
                    int dust1 = Dust.NewDust(new Vector2(position.X, position.Y - 6), 0, 0, 31, velocity.X * 0.25f, velocity.Y * 0.25f, 0);
                    Main.dust[dust1].noGravity = true;
                    Main.dust[dust1].scale = 1.25f;
                    int dust2 = Dust.NewDust(new Vector2(position.X, position.Y - 6), 0, 0, 174, velocity.X * 0.25f, velocity.Y * 0.25f, 0);
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].scale = 1f;
                }
            }
            if (accuracy >= 55)
            {
              //eh no more negative effects
            }
            accuracy += 5f; //5 degrees per shot (10% stability)
            //CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.IndianRed, Math.Min(100, Math.Max(0, Math.Round(100 - ((accuracy - 5) * 2)))) + "%", false);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ItemID.Shotgun, 1)
         .AddIngredient(ItemID.IllegalGunParts, 1)
         .AddIngredient(ItemID.ExplosivePowder, 15)
         .AddTile(TileID.MythrilAnvil)
         .Register();
        }
    }
}