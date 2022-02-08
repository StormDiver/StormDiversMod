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

namespace StormDiversMod.Items.Accessory
{

    [AutoloadEquip(EquipType.Wings)]
    public class HellSoulWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HellSoul Wings");
            Tooltip.SetDefault("Allows flight and slow fall\nHold UP to ascend faster");
            //Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 6));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(160, 8, 1);

            WingsLayer.RegisterData(Item.wingSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Wings_Glow")
            });
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;

            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

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

            }
            


        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        { 
            if (player.controlUp)
            {
                ascentWhenRising = 0.25f;
                maxAscentMultiplier = 2.5f;

            }
            else
            {
                ascentWhenRising = 0.15f;
                maxAscentMultiplier = 1.75f;
            
            }
            ascentWhenFalling = 1f;
                maxCanAscendMultiplier = 1f;
                constantAscend = 0.15f;
        }
       

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 12)
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
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}