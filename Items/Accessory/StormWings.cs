using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;


namespace StormDiversMod.Items.Accessory
{

    [AutoloadEquip(EquipType.Wings)]
    public class StormWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm Wings");
            Tooltip.SetDefault("Allows flight and slow fall\nHold DOWN and JUMP to hover");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            if (!Main.dedServ)
            {
                WingsLayer.RegisterData(Item.wingSlot, new DrawLayerData()
                {
                    Texture = ModContent.Request<Texture2D>(Texture + "_Wings_Glow"),
                    Color = () => Color.White * 0.8f
                });
            }
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(168, 6.5f, 1.66f);

        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;

            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
           
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.controlDown && player.controlJump && player.wingTime > 0 && player.velocity.Y != 0) //slow down to hover
            {
                if ((player.velocity.Y > 0.1f && player.gravDir == 1) || (player.velocity.Y < -0.1f && player.gravDir == -1))
                {
                    player.velocity.Y *= 0.6f;
                }
                else
                {
                   
                    if (player.gravDir == 1)
                    {
                        player.velocity.Y = 0.001f; //(mostly) prevent bottles from being reactivated
                    }
                    else
                    {
                        player.velocity.Y = -0.001f; //(mostly0 prevent bottles from being reactivated

                    }
                }
            }
            if (player.controlDown && player.controlJump && !player.controlLeft && !player.controlRight && player.wingTime > 0) //hover extra time (+100%)
            {
                player.wingTime += 0.5f;
            }
          
        }
        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            if (player.controlDown && player.controlJump && player.wingTime > 0) //hover speeds
            {
                speed = 10; //6.5f when not hovering
                acceleration = 1.75f; // 1 when not hovering
            }
            else
            {
                base.HorizontalWingSpeeds(player, ref speed, ref acceleration);
            }
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            if (player.controlDown && player.controlJump && player.wingTime > 0 && player.velocity.Y != 0) //hover no vertical movement
            {
                ascentWhenFalling = 0f;
                ascentWhenRising = 0f;
                maxCanAscendMultiplier = 0f;
                maxAscentMultiplier = 0f;
                constantAscend = 0f;
            }
            else
            {
                ascentWhenFalling = 1f;
                ascentWhenRising = 0.15f;
                maxCanAscendMultiplier = 1f;
                maxAscentMultiplier = 1.66f;
                constantAscend = 0.15f;
            }
        }
     

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Accessory/StormWings_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
      
    }
}