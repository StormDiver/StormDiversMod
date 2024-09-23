using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using StormDiversMod.Common;

using Terraria;

using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Accessory
{
    //[AutoloadEquip(EquipType.Shoes)]
  
    public class ShroomAccessory : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroomite Launcher Attachment");
            //Tooltip.SetDefault("Makes all guns fire out mini shroomite rockets\nCreates a laser sight line when firing or when holding right click while holding a gun");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;

            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Yellow;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().ShroomAccessItem = Item;
            player.GetModPlayer<EquipmentEffects>().shroomaccess = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ItemID.ShroomiteBar, 20)
            .AddIngredient(ItemID.RocketI, 250)
            .AddIngredient(ItemID.SoulofSight, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Accessory/ShroomAccessory_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }

    public class ShroomAccessorySnipe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shroomite Launcher Scope");
            //Tooltip.SetDefault("Makes all guns fire out mini shroomite rockets\nCreates a laser sight line when firing or when holding right click while holding a gun
            //\nIncreases view range for guns (Right click to zoom out)\n10% increased ranged damage and critical strike chance");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;

            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.HeldItem.CountsAsClass(DamageClass.Ranged) && player.HeldItem.useAmmo == AmmoID.Bullet)
            {
                player.scope = true;
            }
            player.GetModPlayer<EquipmentEffects>().ShroomAccessItem = Item;

            player.GetModPlayer<EquipmentEffects>().shroomaccess = true;

            player.GetDamage(DamageClass.Ranged) += 0.1f;

            player.GetCritChance(DamageClass.Ranged) += 10;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<ShroomAccessory>(), 1)
            .AddIngredient(ItemID.SniperScope, 1)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();


        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Accessory/ShroomAccessorySnipe_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}