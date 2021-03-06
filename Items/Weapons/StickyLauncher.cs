using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
    public class StickyLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiky Bomb Launcher");
            Tooltip.SetDefault("Fires out up to 16 Spiky Bombs that stick to surfaces can be detonated by right clicking while holding the weapon\nRight clicking while holding DOWN will unstick all bombs and make them explode on enemy impact" +
                "\nShoots further depending on your cursor location\nRequires Spiky Bombs, purchase more from the Demolitionist");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            //Item.reuseDelay = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<StickyBombProj>();
            Item.useAmmo = ItemType<Ammo.StickyBomb>();
            Item.UseSound = SoundID.Item61;
        
            Item.damage = 80;
            //Item.crit = 4;
            Item.knockBack = 3f;
            Item.shootSpeed = 10;
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than 16 bombs
            //return player.ownedProjectileCounts[Item.shoot] < 16;
            return true;
        }
        float shootvelo = 1; //Speed multiplers

        public override bool? UseItem(Player player)
        {

            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {          
            float shootToX = Main.MouseWorld.X - player.Center.X;
            float shootToY = Main.MouseWorld.Y - player.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

            shootvelo = distance / 500f + 0.2f; //Faster shoot speed at further distances
            if (shootvelo > 1.5f) //Caps the speed multipler at 1.5x
            {
                shootvelo = 1.5f;
            }
            if (shootvelo < 0.5f) // Caps low end at 0.5x
            {
                shootvelo = 0.5f;
            }

            for (int i = 0; i < 1; i++)
            {
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X * shootvelo, velocity.Y * shootvelo), ModContent.ProjectileType<StickyBombProj>(), damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item61, position);

            }

           
            
            return false;
        }
        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("ProtoLauncher"), 1);
            recipe.AddIngredient(ItemID.SoulofMight, 12);
            recipe.AddIngredient(ItemID.SoulofFright, 12);
            recipe.AddIngredient(ItemID.SoulofSight, 12);
            recipe.AddIngredient(ItemID.Gel, 25);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
       

    }
}
 