using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.ItemDropRules;

namespace StormDiversMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Wings)]
    public class HellSoulWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Soul Flame Wings");
            //Tooltip.SetDefault("Allows flight and slow fall\nHold UP to ascend faster");
            Item.ResearchUnlockCount = 1;
            if (!Main.dedServ)
            {
                WingsLayer.RegisterData(Item.wingSlot, new DrawLayerData()
                {
                    Texture = ModContent.Request<Texture2D>(Texture + "_Wings_Glow"),
                    Color = () => Color.White * 0.75f
                });
            }
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(160, 6.5f, 1.5f);
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;

            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.controlUp && player.controlJump && player.wingTime > 0)
            {

                if (Main.rand.Next(10) == 0)
                {
                    var dust = Dust.NewDustDirect(player.position, player.width, player.height, 173);
                    dust.scale = 2f;
                }
                player.wingTime -= 0.33f;

            }
  

        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        { 
            if (player.controlUp)
            {
                ascentWhenRising = 0.2f;
                maxAscentMultiplier = 2.3f;

            }
            else
            {
                ascentWhenRising = 0.12f;
                maxAscentMultiplier = 1.5f;
            
            }
            ascentWhenFalling = 1f;
                maxCanAscendMultiplier = 1f;
                constantAscend = 0.15f;
        }
       

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 10)
            .AddIngredient(ItemID.SoulofFlight, 20)
            .AddTile(TileID.Hellforge)
            .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Accessory/HellSoulWings_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
        /*public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }*/
    }
}