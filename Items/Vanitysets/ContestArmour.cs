using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using StormDiversMod.Basefiles;


namespace StormDiversMod.Items.Vanitysets
{
    [AutoloadEquip(EquipType.Head)]
    public class ContestArmourBHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Cryogenic Mask");
            Tooltip.SetDefault("Cold and Misty");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeadLayer.RegisterData(Item.headSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Head_Glow")
            });
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;

        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<ContestArmourChestplate>() && legs.type == ItemType<ContestArmourLeggings>();
        }
        int particle = 10;
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
            player.armorEffectDrawOutlines = true;
            {
                particle--;
                if (particle <= 0)
                {
                    int dustIndex = Dust.NewDust(new Vector2(player.position.X + 1f, player.position.Y), player.width, player.height, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;

                    particle = 10;
                }

            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 6)
          .AddIngredient(ModContent.ItemType<Items.Materials.BlueCloth>(), 12)

          .AddTile(TileID.MythrilAnvil)
          .Register();

         
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Vanitysets/ContestArmourBHelmet_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

    }
    //_________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    public class ContestArmourChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Cryogenic Chestplate");
            //Tooltip.SetDefault("Concept by Storm Diver");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 10)
         .AddIngredient(ModContent.ItemType<Items.Materials.BlueCloth>(), 18)
         .AddTile(TileID.MythrilAnvil)
         .Register();
        }
    }
    //_________________________________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class ContestArmourLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Cryogenic Greaves");
            //Tooltip.SetDefault("Concept by Storm Diver");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
        .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 8)
        .AddIngredient(ModContent.ItemType<Items.Materials.BlueCloth>(), 15)

        .AddTile(TileID.MythrilAnvil)
        .Register();
        }
    }

}