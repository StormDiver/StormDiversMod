using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Basefiles;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Projectiles.Petprojs;

namespace StormDiversMod.Items.Pets
{

    public class DerplingVine : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mysterious Vine");
            //Tooltip.SetDefault("Seems to be infused with some strange energy");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;

            Item.width = 16;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 7, 50, 0);
           
            
            Item.shoot = ProjectileType<GoldDerpie>();
            Item.buffType = BuffType<GoldDerpieBuff>();
            Item.rare = ItemRarityID.Yellow;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
        
            player.AddBuff(Item.buffType, 2); // The item applies the buff, the buff spawns the projectile

            return false;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
        
    }

    public class StabbyKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stabby Knife");
            //Tooltip.SetDefault("Summons a Mini Stabby Pet");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;

            Item.width = 16;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);

            Item.shoot = ProjectileType<MrStabbyPetProj>();
            Item.buffType = BuffType<MrStabbyPetBuff>();
            Item.rare = ItemRarityID.Pink;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2); // The item applies the buff, the buff spawns the projectile

            return false;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }

    public class StormLightItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Suspicious Looking Helmet");
            //Tooltip.SetDefault("Summons something unthinkable");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;
            Item.width = 24;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);

            Item.shoot = ProjectileType<StormLightProj>();
            Item.buffType = BuffType<StormLightBuff>();
            Item.rare = ItemRarityID.Red;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {


            player.AddBuff(Item.buffType, 2); // The item applies the buff, the buff spawns the projectile

            return false;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {

            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Pets/StormLightItem_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

    }
    public class TwilightPetItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Strange Twilight Hood");
            //Tooltip.SetDefault("Summons a mysterious figure to light your way");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;


            Item.width = 24;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 3, 0, 0);

            Item.shoot = ProjectileType<TwilightPetProj>();
            Item.buffType = BuffType<TwilightPetBuff>();
            Item.rare = ItemRarityID.Orange;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {


            player.AddBuff(Item.buffType, 2); // The item applies the buff, the buff spawns the projectile

            return false;

        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {

            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Pets/TwilightPetItem_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

    }
    //boss master pets

    public class StormBossPetItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mysterious Device");
            //Tooltip.SetDefault("Summons a Baby Overloaded Scandrone\n'Scanning complete!'");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;
            Item.width = 24;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);

            Item.shoot = ProjectileType<StormBossPetProj>();
            Item.buffType = BuffType<StormBossPetBuff>();
            Item.rare = ItemRarityID.Red;
            Item.master = true;
        }


        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {

            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            player.AddBuff(Item.buffType, 2); // The item applies the buff, the buff spawns the projectile

            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Pets/StormBossPetItem_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

    }

    public class AridBossPetItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glowing Horns");
            //Tooltip.SetDefault("Summons a liberated Husk that follows you");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;
            Item.width = 24;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);

            Item.shoot = ProjectileType<AridBossPetProj>();
            Item.buffType = BuffType<AridBossPetBuff>();
            Item.rare = ItemRarityID.Orange;
            Item.master = true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {

            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            player.AddBuff(Item.buffType, 2); // The item applies the buff, the buff spawns the projectile

            return false;
        }      
    }
    public class UltimateBossPetItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("TheCute");
            //Tooltip.SetDefault("Summons something cute and painless");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;
            Item.width = 24;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);

            Item.shoot = ProjectileType<UltimateBossPetProj>();
            Item.buffType = BuffType<UltimateBossPetBuff>();
            Item.rare = ItemRarityID.Red;
            Item.master = true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {

            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            player.AddBuff(Item.buffType, 2); // The item applies the buff, the buff spawns the projectile

            return false;
        }
    }
}

