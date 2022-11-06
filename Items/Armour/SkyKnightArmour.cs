using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class SkyKnightBMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Star Warrior Visage");
            Tooltip.SetDefault("7% increased damage");
            //ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
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
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.07f;

       
        }

        public override void ArmorSetShadows(Player player)
        {
            //player.armorEffectDrawShadow = true;

            player.armorEffectDrawOutlines = true;

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<SkyKnightChest>() && legs.type == ItemType<SkyKnightGreaves>();

        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Summons a floating star above you that launches mini homing stars at enemies";


            player.GetModPlayer<ArmourSetBonuses>().skyKnightSet = true;

        }

        public override void AddRecipes()
        {

            CreateRecipe()
           .AddIngredient(ItemID.MeteoriteBar, 12)
           .AddRecipeGroup("StormDiversMod:EvilMaterial", 10)
           .AddIngredient(ItemID.FallenStar, 5)
           .AddTile(TileID.Anvils)
           .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/SkyKnightBMask_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }

    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    
    public class SkyKnightChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Star Warrior Platemail");
            Tooltip.SetDefault("Increases your max number of sentries by 1\n4% increased damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            if (!Main.dedServ)
            {
                BodyGlowmaskPlayer.RegisterData(Item.bodySlot, () => new Color(255, 255, 255, 75) * 0.6f);
            }

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.04f;
            player.maxTurrets += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.MeteoriteBar, 18)
           .AddRecipeGroup("StormDiversMod:EvilMaterial", 15)
           .AddIngredient(ItemID.FallenStar, 8)
           .AddTile(TileID.Anvils)
           .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/SkyKnightChest_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class SkyKnightGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Star Warrior Greaves");
            Tooltip.SetDefault("Increases your max number of sentries by 1\n4% increased damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            LegsLayer.RegisterData(Item.legSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Legs_Glow")
            });
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1 ,0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {

            player.maxTurrets += 1;

            player.GetDamage(DamageClass.Generic) += 0.04f;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.MeteoriteBar, 15)
           .AddRecipeGroup("StormDiversMod:EvilMaterial", 12)
           .AddIngredient(ItemID.FallenStar, 7)
           .AddTile(TileID.Anvils)
           .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/SkyKnightGreaves_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //__________________________________________________________________________________________________________________________
   

}