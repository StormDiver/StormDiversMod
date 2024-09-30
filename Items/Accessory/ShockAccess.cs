using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Common;
using Terraria.GameContent.Creative;
using Humanizer;
using System.Threading.Channels;
using System;
using Microsoft.Xna.Framework.Graphics;


namespace StormDiversMod.Items.Accessory
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class ShockBand : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shock Charm");
            //Tooltip.SetDefault("Crits do lightning arc thingy");
            Item.ResearchUnlockCount = 1;
            if (!Main.dedServ)
            {
                HandOnGlowmaskPlayer.RegisterData(Item.handOnSlot, () => new Color(255, 255, 255, 75) * 0.6f);
            }
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().shockBand = true;
            player.GetModPlayer<EquipmentEffects>().ShockbandItem = Item;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Accessory/ShockBand_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    public class ShockQuiver : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shock Quiver");
            //Tooltip.SetDefault("Increases arrow damage by 10% and greatly increases arrow speed
            //20 % chance to not consume arrows
            //Critical hits cause lightning to arc to nearby enemies");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Pink;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.GetModPlayer<EquipmentEffects>().shockBand = true;
            player.GetModPlayer<EquipmentEffects>().ShockbandItem = Item;
            player.GetModPlayer<EquipmentEffects>().shockBandQuiver = true;

            player.magicQuiver = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<ShockBand>(), 1)
         .AddIngredient(ItemID.MagicQuiver, 1)
         .AddTile(TileID.TinkerersWorkbench)
         .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Accessory/ShockQuiver_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    public class ShockEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eye of the Storm");
            //Tooltip.SetDefault("Greatly increases luck\nIncreases critical strike damage by 15%\
            //Critical hits cause lightning to arc to nearby enemies");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 9, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.GetModPlayer<EquipmentEffects>().shockBand = true;
            player.GetModPlayer<EquipmentEffects>().ShockbandItem = Item;
            player.GetModPlayer<EquipmentEffects>().shockDerpEye = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<ShockBand>(), 1)
         .AddIngredient(ModContent.ItemType<DerpEye>(), 1)
         .AddTile(TileID.TinkerersWorkbench)
         .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Accessory/ShockEye_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}