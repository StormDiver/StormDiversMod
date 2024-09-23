using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;


namespace StormDiversMod.Items.Ammo
{
    public class C4Ammo : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Satchel Charge");
            //Tooltip.SetDefault("Explosives placed");
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.damage = 1000;
            Item.DamageType = DamageClass.Default;
            Item.width = 10;
            Item.height = 10;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Lime;
            Item.shootSpeed = 0f;
            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.C4Proj>();
            Item.ammo = Item.type;
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Ammo/C4Ammo_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}
