using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Weapons
{
    public class CultistLazor : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mysterious Cultist Hood");
            //Tooltip.SetDefault("Charge up and fire a damaging laser from this strange cultist hood that rapidly drains mana\nWait, What!?");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Magic;
            //Item.UseSound = SoundID.Item13;
            Item.channel = true;
            Item.damage = 85;
            Item.knockBack = 2f;
            //Item.mana = 1;
            Item.shoot = ModContent.ProjectileType <Projectiles.CultistLazorProj>();
            Item.shootSpeed = 0f;


            Item.noMelee = true;
        }
        public override bool CanUseItem(Player player) //cannot be used if the player has less than 10 mana
        {
            if (player.statMana < 10)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, -4);
        }

    }
}