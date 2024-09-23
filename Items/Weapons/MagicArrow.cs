using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Common;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Intrinsics.X86;

namespace StormDiversMod.Items.Weapons
{
    public class MagicArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Arrow of Seeking");
            //Tooltip.SetDefault("Hold down left mouse to make the arrow target and seek out any enemy in range\n
            //Returns to you when not in use\n'I'm Mary Poppins Y'all');
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0); //50 Gold to buy
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.reuseDelay = 15;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.damage = 75;
            //Item.crit = 4;
            Item.knockBack = 2f;
            Item.shoot = ModContent.ProjectileType<MagicArrowProj>();
            Item.channel = true;
            Item.shootSpeed = 16f;
            /*if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 12;
            }
            else
            {
                Item.mana = 8;
            }*/
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void HoldItem(Player player)
        {         
        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 10), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        public override void AddRecipes()
        {
          //Purchased from Cyborg
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/MagicArrow_Glow");
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
   
}