using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
    public class SpookyStaff : ModItem
    {
        

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spooky Staff");
            //Tooltip.SetDefault("Rains homing fire from the sky");
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 24;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 23;
            }
            else
            {
                Item.mana = 15;
            }
            Item.UseSound = SoundID.Item13;

            Item.damage = 74;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<SpookyGlobeProj>();

            Item.shootSpeed = 25f;
         
                

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            for (int index = 0; index < 3; ++index)
            {
                Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
                vector2_1.Y -= (float)(100 * index);
                float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if ((double)num13 < 0.0) num13 *= -1f;
                if ((double)num13 < 20.0) num13 = 20f;
                float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                float num15 = Item.shootSpeed / num14;
                float num16 = num12 * num15;
                float num17 = num13 * num15;
                float SpeedX = num16 + (float)Main.rand.Next(-20, 20) * 0.02f;  //this defines the projectile X position speed and randomnes
                float SpeedY = num17 + (float)Main.rand.Next(-20, 20) * 0.02f;  //this defines the projectile Y position speed and randomnes
                Projectile.NewProjectile(source, new Vector2(vector2_1.X, vector2_1.Y), new Vector2(SpeedX, SpeedY), type, damage, knockback, player.whoAmI, 0.0f, (float)Main.rand.Next(5));
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.SpookyWood, 350)
            .AddTile(TileID.WorkBenches)
            .Register();

        }
        
    }
}