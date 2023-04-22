using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Buffs;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;


namespace StormDiversMod.Items.Weapons
{
    
    public class WyvernBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bow of the Wyverns");
            //Tooltip.SetDefault("Fires 3 arrows in an even spread\nGrants additional speed to all arrows");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 46;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item5;

            Item.damage = 32;
            Item.knockBack = 2f;

            Item.shoot = ProjectileID.WoodenArrowFriendly;

            Item.shootSpeed = 12f;

            Item.useAmmo = AmmoID.Arrow;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Breaks a couple of arrows :thesad:
            /*int projID = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
            Main.projectile[projID].aiStyle = 0;
            Main.projectile[projID].rotation = (float)Math.Atan2(Main.projectile[projID].velocity.Y, Main.projectile[projID].velocity.X) + 1.57f;*/

            float numberProjectiles = 3;
            float rotation = MathHelper.ToRadians(7);
            //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
            for (int j = 0; j < numberProjectiles; j++)
            {
         
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles - 1)));
                int proj = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
                Main.projectile[proj].extraUpdates += 1;
            }

            return false;
        }

     
       
    }
   
}