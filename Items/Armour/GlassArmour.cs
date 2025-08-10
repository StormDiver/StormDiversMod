using Microsoft.Xna.Framework;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using StormDiversMod.Items.Materials;
using StormDiversMod.Items.Tools;
using StormDiversMod.Items.Weapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class GlassBHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Glass helmet");
            //Tooltip.SetDefault("6% increased damage");
            Item.ResearchUnlockCount = 1;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;
            Item.defense = 1;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.06f;
        }
        public override void ArmorSetShadows(Player player)
        {
            /*if (Main.rand.Next(30) == 0)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(player.Center.X + Main.rand.Next(-player.width / 2, player.width / 2), player.Center.Y + Main.rand.Next(-player.height / 2, player.height / 2)),

                }, player.whoAmI);
            }*/
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<GlassChestplate>() && legs.type == ItemType<GlassGreaves>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "33% increased movement speed";
            player.GetModPlayer<ArmourSetBonuses>().glassSet = true;
            player.moveSpeed += 0.33f;
            //player.setBonus = this.GetLocalization("SetBonus").Value;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Glass, 15)
           .AddTile(TileID.Furnaces)
           .Register();

            CreateRecipe()
            .AddIngredient(ModContent.ItemType<GlassArmourShard>(), 1)
            .AddIngredient(ItemID.Glass, 8)
            .AddIngredient(ItemID.Gel, 8)
            .Register();
        }
    }
    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    public class GlassChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Glass Chestplate");
            //Tooltip.SetDefault("8% increased damage");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;
            Item.defense = 1;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Glass, 25)
           .AddTile(TileID.Furnaces)
           .Register();

            CreateRecipe()
            .AddIngredient(ModContent.ItemType<GlassArmourShard>(), 1)
            .AddIngredient(ItemID.Glass, 13)
            .AddIngredient(ItemID.Gel, 13)
            .Register();
        }
    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class GlassGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Glass Greaves");
            //Tooltip.SetDefault("6% increased damage");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;
            Item.defense = 1;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.06f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.Glass, 20)
           .AddTile(TileID.Furnaces)
           .Register();

            CreateRecipe()
            .AddIngredient(ModContent.ItemType<GlassArmourShard>(), 1)
            .AddIngredient(ItemID.Glass, 10)
            .AddIngredient(ItemID.Gel, 10)
            .Register();
        }
    }
    //__________________________________________________________________________________________________________________________
}