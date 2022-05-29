using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class ShroomBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomite Bullet");
            Tooltip.SetDefault("Ricochets off walls and pierces enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 16);
            Item.rare = ItemRarityID.Yellow;


            Item.DamageType = DamageClass.Ranged;
            

            Item.damage = 15;
            Item.crit = 6;
            Item.knockBack = 2f;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.ShroomBulletProj>();
            Item.shootSpeed = 2f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void OnConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.NextBool(10))
            {
               
                
            }
        }

        public override void AddRecipes()
        {

            Recipe recipe = Mod.CreateRecipe(ModContent.ItemType<ShroomBullet>(), 150);
            recipe.AddIngredient(ItemID.ShroomiteBar, 1);
            recipe.AddIngredient(ItemID.MusketBall, 150);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
           
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
