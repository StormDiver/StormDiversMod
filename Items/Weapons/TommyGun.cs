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
using StormDiversMod.Common;
using Humanizer;
using System.Net;

namespace StormDiversMod.Items.Weapons
{
    public class TommyGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Snowman's Tommy Gun");
            //Tooltip.SetDefault("Can only be fired left and right
            //Hold UP/ DOWN to aim upwards / downwards at a 45 degree angle respectively
            //'Wit' dis weapon I be a me a real gangsta'");
            //Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 10;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.EatFood;
            
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item40;

            Item.damage = 28;
            
            Item.knockBack = 0.5f;
       
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 11f;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true;

            Item.noUseGraphic = false;
        }
        
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            base.UseStyle(player, heldItemFrame);
            if (player.controlUp && !player.controlDown && player.direction == 1) //up right
            {
                player.itemRotation = 5.497787f; //315 degrees
                player.itemLocation += new Vector2(10, 10);

            }
            else if (player.controlUp && !player.controlDown && player.direction == -1) //up left
            {
                player.itemRotation = 0.7853982f; //45 degrees
                player.itemLocation += new Vector2(-10, 10);
            }
            else if (player.controlDown && !player.controlUp && player.direction == 1) //down right
            {
                player.itemRotation = 0.7853982f;  //45 degrees
                player.itemLocation += new Vector2(-5, -15);
            }
            else if (player.controlDown && !player.controlUp && player.direction == -1) //down left
            {
                player.itemRotation = 5.497787f; //315 degrees
                player.itemLocation += new Vector2(5, -15);
            }
            else //stright ahead
            {
                player.itemRotation = 6.283185f; //0 degrees
                player.itemLocation += new Vector2(-3 * player.direction, 4);

            }
        }
        public override void HoldItemFrame(Player player)
        {
            base.HoldItemFrame(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = ProjectileID.BulletHighVelocity;
            int spread = 2;
            //if (player.direction == 1)
            if (player.controlUp && !player.controlDown)
            {
                Vector2 perturbedSpeed = new Vector2(Item.shootSpeed * player.direction * 0.71f, -Item.shootSpeed * 0.71f).RotatedByRandom(MathHelper.ToRadians(spread));

                Projectile.NewProjectile(source, new Vector2(position.X, position.Y - (4 * player.gravDir)), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            else if (player.controlDown && !player.controlUp)
            {
                Vector2 perturbedSpeed = new Vector2(Item.shootSpeed * player.direction * 0.71f, Item.shootSpeed * 0.71f).RotatedByRandom(MathHelper.ToRadians(spread));

                Projectile.NewProjectile(source, new Vector2(position.X, position.Y - (12 * player.gravDir)), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            else
            {
                Vector2 perturbedSpeed = new Vector2(Item.shootSpeed * player.direction, 0).RotatedByRandom(MathHelper.ToRadians(spread));

                Projectile.NewProjectile(source, new Vector2(position.X, position.Y - (4 * player.gravDir)), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            //Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 40);
            return false;
        }

        
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
           
        }
    }
}