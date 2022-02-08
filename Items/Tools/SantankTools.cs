using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace StormDiversMod.Items.Tools
{
    public class SantankDrill : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Santa's Drill");
            Tooltip.SetDefault("'For use on those who have been naughty'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 22;

            Item.useTime = 2;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 9, 0, 0);
                     Item.rare = ItemRarityID.Yellow;
            Item.knockBack = 3f;
           
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.SantankDrillProj>();
            Item.shootSpeed = 40f;
            Item.pick = 205;
            Item.tileBoost = 0;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true; 
            Item.channel = true; 
            Item.autoReuse = true;
            Item.tileBoost = 0;


        }



        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<Items.Materials.SantankScrap>(), 20)
          .AddTile(TileID.MythrilAnvil)
          .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Tools/SantankDrill_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    public class SantankSaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Santa's Chainsaw");
            Tooltip.SetDefault("'For use on those who have been naughty'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {

            Item.damage = 65;
            Item.DamageType = DamageClass.Melee;
            Item.width = 60;
            Item.height = 20;
            Item.useTime = 2;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 9, 0, 0);
                     Item.rare = ItemRarityID.Yellow;
            Item.knockBack = 5f;
          
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType < Projectiles.SantankSawProj>();
            Item.shootSpeed = 55f;
            Item.axe = 27;
            Item.tileBoost = 0;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.tileBoost = 0;


        }


        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.SantankScrap>(), 20)
           .AddTile(TileID.MythrilAnvil)
           .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Tools/SantankDrill_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    public class SantankJackham : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Santa's Jackhammer");
            Tooltip.SetDefault("'For use on those who have been naughty'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {

            Item.damage = 55;
            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 20;
            Item.useTime = 3;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 9, 0, 0);
                     Item.rare = ItemRarityID.Yellow;
            Item.knockBack = 7f;

            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.SantankJackhamProj>();
            Item.shootSpeed = 40f;
            Item.hammer = 100;
            Item.tileBoost = 0;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.tileBoost = 0;


        }



        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ModContent.ItemType<Items.Materials.SantankScrap>(), 20)
         .AddTile(TileID.MythrilAnvil)
         .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Tools/SantankDrill_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}