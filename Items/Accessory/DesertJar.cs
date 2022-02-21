using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
   
    public class DesertJar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pharaoh's Urn");
            Tooltip.SetDefault("Leaves behind a damaging trail of sand when moving fast enough\nDamaging enemies creates a small sand blast around you");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;


            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }


        int dropdust = 0;
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<StormPlayer>().desertJar = true;

            
            if ((player.velocity.X > 3.5f || player.velocity.X < -3.5f) || (player.velocity.Y > 3.5f || player.velocity.Y < -3.5f))
            {

                dropdust++;
                if (dropdust == 4)
                {

                    //float speedX = 0f;
                    //float speedY = 0f;
                    //Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(180));
                    //float scale = 1f - (Main.rand.NextFloat() * .5f);
                    //perturbedSpeed = perturbedSpeed * scale;
                    Projectile.NewProjectile(player.GetProjectileSource_Accessory(null), new Vector2(player.Center.X, player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.DesertJarProj>(), 35, 0, player.whoAmI);
                        dropdust = 0;

                        //Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 13);
                    
                }
               
               
            }
        }
       
        public override void AddRecipes()
        {
            /*CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.OresandBars.DesertBar>(), 10)
            .AddIngredient(ItemID.AncientBattleArmorMaterial, 2)
            .AddTile(TileID.MythrilAnvil)
            .Register();*/
          
        }
    }
}