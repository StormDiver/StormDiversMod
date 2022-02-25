using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Tools
{
    public class ShieldKiller : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celestial Globe");
            Tooltip.SetDefault("Removes the shields from all active pillars");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useTurn = false;
            Item.autoReuse = false;
                Item.shoot = ProjectileID.TowerDamageBolt;
                Item.shootSpeed = 0f;

            Item.noMelee = true; 
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, -10);
        }
        public override void UseAnimation(Player player)
        {
           
            base.UseAnimation(player);
        }
        
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
        
           // Main.PlaySound(SoundID.NPCKilled, (int)position.X, (int)position.Y, 59);
            //for (int i = 0; i < 15; i++)
            {
                if (NPC.ShieldStrengthTowerVortex > 0 || NPC.ShieldStrengthTowerSolar > 0 || NPC.ShieldStrengthTowerNebula > 0 || NPC.ShieldStrengthTowerStardust > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 122);
                    //Projectile.NewProjectile(position.X, position.Y, 0, 0, 629, damage, knockBack, player.whoAmI, NPC.FindFirstNPC(422));

                    NPC.ShieldStrengthTowerVortex = 0;
                    
                    NPC.ShieldStrengthTowerSolar = 0;
                    NPC.ShieldStrengthTowerNebula = 0;
                    NPC.ShieldStrengthTowerStardust = 0;
                    //Projectile.NewProjectile(player.Center.X, player.Bottom.Y, 0, 0, ProjectileID.DD2DarkMageHeal, 0, 0, player.whoAmI);


                    Main.NewText("The Shields guarding the Celestial pillars have been stripped away!", 0, 204, 170);

                    for (int i = 0; i < 50; i++)
                    {

                        float speedX = 0f;
                        float speedY = -10f;

                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                        int dust2 = Dust.NewDust(new Vector2(player.Center.X + (20 * player.direction), player.Top.Y - 6), 0, 0, 229, perturbedSpeed.X, perturbedSpeed.Y, 200, default, 1.5f);
                        Main.dust[dust2].noGravity = true;
                    }
                  
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.NPCKilled, (int)position.X, (int)position.Y, 6);
                    for (int i = 0; i < 50; i++)
                    {
                        float speedX = 0f;
                        float speedY = -5f;

                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                        int dustIndex = Dust.NewDust(new Vector2(player.Center.X + (20 * player.direction), player.Top.Y - 6), 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);
                        Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].noGravity = true;
                    }

                    Main.NewText("There are no active shields...", 150, 75, 76);
                }
                /*if (NPC.ShieldStrengthTowerSolar > 0)
                {
                    //Projectile.NewProjectile(position.X, position.Y, 0, 0, 629, damage, knockBack, player.whoAmI, NPC.FindFirstNPC(517));
                   
                }
                if ()
                {
                    //Projectile.NewProjectile(position.X, position.Y, 0, 0, 629, damage, knockBack, player.whoAmI, NPC.FindFirstNPC(507));
                    
                }
                if ()
                {
                   //Projectile.NewProjectile(position.X, position.Y, 0, 0, 629, damage, knockBack, player.whoAmI, NPC.FindFirstNPC(493));
                    
                }*/
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FragmentSolar, 50)
            .AddIngredient(ItemID.FragmentVortex, 50)
            .AddIngredient(ItemID.FragmentNebula, 50)
            .AddIngredient(ItemID.FragmentStardust, 50)
         .AddTile(TileID.LunarCraftingStation)
         .Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}