using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Buffs;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;

namespace StormDiversMod.Items.Weapons
{
    public class BloodyRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloodshot Rifle");
            Tooltip.SetDefault("Converts regular bullets into blood bullets that drop blood in flight\nRight Click to zoom out");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item40;

            Item.damage = 70;

            Item.knockBack = 2f;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 16f;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true;
        }


        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 1);
        }



        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (type == ProjectileID.Bullet)
            {
                type = ModContent.ProjectileType<Projectiles.BloodyBulletProj>();
            }


            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);


            return false;
        }
        public override void HoldItem(Player player)
        {
            player.scope = true;
        }

        public override void AddRecipes()
        {

            //In new recipes with other hardmode blood moon recipes
          



        }

    }
    public class BloodySentry : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Urchin Staff");
            Tooltip.SetDefault("Summons a blood Urchin Sentry that rapidly spits defense-piercing blood streams at enemies");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            //Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 10;
            //Item.UseSound = SoundID.Item78;

            Item.damage = 18;
            //Item.crit = 4;
            Item.knockBack = 0.25f;

            Item.shoot = ModContent.ProjectileType<Projectiles.SentryProjs.BloodySentryProj>();

            //Item.shootSpeed = 3.5f;



            Item.noMelee = true;
        }
        /* public override Vector2? HoldoutOffset()
         {
             return new Vector2(5, 0);
         }*/
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.NPCHit9, player.position);

            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;
            return false;
        }

        public override void AddRecipes()
        {
            //In new recipes with other hardmode blood moon recipes


        }
    }

}