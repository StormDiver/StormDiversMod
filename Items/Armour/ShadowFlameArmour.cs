using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod;
using StormDiversMod.Items.Materials;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using StormDiversMod.Items.Weapons;

namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class ShadowFlameBMask : ModItem
    {


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Shadowflare Mask");
            //Tooltip.SetDefault("15% increased whip range\n10% increased whip speed\n8% increased summoner damage");
            Item.ResearchUnlockCount = 1;

            /* HeadLayer.RegisterData(Item.headSlot, new DrawLayerData()
             {
                 Texture = ModContent.Request<Texture2D>(Texture + "_Head_Glow")
             });*/
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ShadowFlameChestplate>();

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.08f;
            player.whipRangeMultiplier += 0.15f;
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.10f;

        }

        public override void ArmorSetShadows(Player player)
        {
            //player.armorEffectDrawShadow = true;
            player.armorEffectDrawOutlines = true;
            if (Main.rand.Next(2) == 0)
            {
                var dust = Dust.NewDustDirect(player.position, player.width, player.height, 65, 0, -3);
                dust.scale = 1.3f;
                dust.noGravity = true;
            }
            if (Main.rand.Next(8) == 0)
            {
                var dust = Dust.NewDustDirect(player.BottomLeft, player.width, 2, 205, 0, -4);
                dust.scale = 0.9f;
                dust.noGravity = true;
            }

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<ShadowFlameChestplate>() && legs.type == ItemType<ShadowFlameGreaves>();

        }
        public override void UpdateArmorSet(Player player)
        {
            //player.setBonus = "Whips inflict shadowflame upon enemies\nEnemies inflicted with shadowflame take extra damage when hit";
            player.setBonus = this.GetLocalization("SetBonus").Value;
            player.GetModPlayer<ArmourSetBonuses>().shadowflameSet = true;
        }

        public override void AddRecipes()
        {

            /*CreateRecipe()
            .AddIngredient(ItemID.Silk, 15)
            .AddIngredient(ModContent.ItemType<ChaosShard>(), 2)
            .AddTile(TileID.Anvils)
            .Register();*/

        }
        /* public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
         {

             Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/NightsBHelmet_Glow");

             spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                 new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
         }*/

    }

    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]

    public class ShadowFlameChestplate : ModItem
    {


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            //DisplayName.SetDefault("Shadowflare Robe");
            //Tooltip.SetDefault("10% increased whip range\nIncreases maximum minions by 1\n8% increased summoner damage");
            Item.ResearchUnlockCount = 1;
            ArmorIDs.Body.Sets.NeedsToDrawArm[Item.bodySlot] = false;

            //BodyGlowmaskPlayer.RegisterData(Item.bodySlot, () => new Color(255, 255, 255, 0) * 0.6f);
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ShadowFlameGreaves>();

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 10;
        }
        public virtual void DrawArmorColor(EquipType type, int slot, Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {

        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.08f;
            player.whipRangeMultiplier += 0.10f;
            //player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
            player.maxMinions += 1;

        }
        public override void AddRecipes()
        {

        }
        /* public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
         {
             Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/NightsChainmail_Glow");

             spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                 new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
         }*/

    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class ShadowFlameGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Shadowflare Greaves");
            //Tooltip.SetDefault("10% increased whip range\n15% increased whip speed\n8% increased summoner damage");
            Item.ResearchUnlockCount = 1;

            /* LegsLayer.RegisterData(Item.legSlot, new DrawLayerData()
             {
                 Texture = ModContent.Request<Texture2D>(Texture + "_Legs_Glow")
             });*/
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ShadowFlameBMask>();
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.08f;
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
            player.whipRangeMultiplier += 0.10f;

        }
        public override void AddRecipes()
        {

        }
        /* public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
         {
             Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/NightsGreaves_Glow");

             spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                 new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
         }*/
    }
    //__________________________________________________________________________________________________________________________
    public class Whipflamedust : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            var player = Main.player[projectile.owner];

            /*if (player.GetModPlayer<EquipmentEffects>().shadowflameSet == true)
            {
                if (projectile.aiStyle == 165)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 65, 0, -3);
                        dust.scale = 1.3f;
                        dust.noGravity = true;
                    }
                }
            }*/

        }
    }
}