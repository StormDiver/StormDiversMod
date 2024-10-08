using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod;
using Terraria.GameContent.Creative;
using System.Threading;
using static Terraria.ModLoader.PlayerDrawLayer;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class GraniteBMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Granite Mask");
            //Tooltip.SetDefault("2% increased melee damage and critical strike chance");
            Item.ResearchUnlockCount = 1;
            HeadLayer.RegisterData(Item.headSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Head_Glow")
            });
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.02f;
            player.GetCritChance(DamageClass.Melee) += 2;

        }

        public override void ArmorSetShadows(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<GraniteBuff>()))
            {

                player.armorEffectDrawOutlines = true;
            }
            else
            {
                player.armorEffectDrawOutlines = false;

            }

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<GraniteChestplate>() && legs.type == ItemType<GraniteGreaves>();

        }

        public override void UpdateArmorSet(Player player)
        {
            var list = StormDiversMod.ArmourSpecialHotkey.GetAssignedKeys();
            string keyName = "(Not bound, set in controls)";

            if (list.Count > 0)
            {
                keyName = list[0];
            }

            //player.setBonus = "Press '" + keyName + "' grants the Granite Barrier buff which grants 50% damage resistance, immunity to knockback, and a thorns effect for 5 seconds
            //\nThe Granite barrier reduces movement speed and jump height\nHas a 20 second cooldown between uses";
            player.setBonus = this.GetLocalization("SetBonus1").Value + " '" + keyName + "' " + this.GetLocalization("SetBonus2").Value;

            player.GetModPlayer<ArmourSetBonuses>().graniteSet = true;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("StormDiversMod:GoldBars", 12)
            .AddIngredient(ModContent.ItemType<Items.Materials.GraniteCore>(), 2)
            .AddTile(TileID.Anvils)
            .Register();


        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/GraniteBMask_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }

    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    
    public class GraniteChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Granite Chestplate");
            //Tooltip.SetDefault("4% increased melee damage\n10% increased melee speed");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.04f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.1f;           


        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("StormDiversMod:GoldBars", 18)
            .AddIngredient(ModContent.ItemType<Items.Materials.GraniteCore>(), 3)
            .AddTile(TileID.Anvils)
            .Register();

        }
     
    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class GraniteGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Granite Greaves");
            //Tooltip.SetDefault("2% increased melee damage and critical strike chance");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.02f;
            player.GetCritChance(DamageClass.Melee) += 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddRecipeGroup("StormDiversMod:GoldBars", 15)
             .AddIngredient(ModContent.ItemType<Items.Materials.GraniteCore>(), 2)
             .AddTile(TileID.Anvils)
             .Register();


        }
    }
    //__________________________________________________________________________________________________________________________
   

}