using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Tools
{
    public class Quack : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rubber Duck");
            Tooltip.SetDefault("'QUACK!'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.White;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 70;
            Item.useAnimation = 70;
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.holdStyle = 0;
            Item.noMelee = true; 
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        float pitch;

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                float shootToX = Main.MouseWorld.X - player.Center.X;
                float shootToY = Main.MouseWorld.Y - player.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                

                pitch = distance / 500 - 0.6f; //Lowest possible pitch is -0.6f;
                if (pitch > 0.8f) //Caps the pitch at 0.8f;
                {
                    pitch = 0.8f;
                }
                //SoundEngine.PlaySound(SoundID.Zombie, (int)player.Center.X, (int)player.Center.Y, 12, 1, pitch);*/
                SoundEngine.PlaySound(SoundID.Zombie12 with { Volume = 1f, Pitch = pitch}, player.Center);
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
        
        
       
    }
}