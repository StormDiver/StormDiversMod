using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Basefiles;


namespace StormDiversMod.Items.Vanitysets
{
    [AutoloadEquip(EquipType.Head)]
    public class BossMaskStormBoss : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Scandrone Mask");
            //Tooltip.SetDefault("Boss Mask?");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeadLayer.RegisterData(Item.headSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Head_Glow")
            });

            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
            
        }
      
      
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Vanitysets/BossMaskStormBoss_Glow");

            //spriteBatch.Draw(texture, Item.Center - Main.screenPosition, new Rectangle(0, 0, Item.width, Item.height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
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