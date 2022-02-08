using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class StickyBomb : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiky Bomb");
            Tooltip.SetDefault("Very sticky and explosive\nFor use with the Spiky Bomb Launcher\n'What makes a good Demoman?'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 2, 50);
            Item.rare = ItemRarityID.Yellow;

            //Item.melee = true;
            Item.DamageType = DamageClass.Ranged;
            //Item.magic = true;
            //Item.summon = true;
            //Item.thrown = true;

            Item.damage = 60;

            Item.knockBack = 1f;
            Item.consumable = true;


            Item.shoot = ModContent.ProjectileType<Projectiles.StickyBombProj>();
            Item.shootSpeed = 3f;
            Item.ammo = Item.type;
        }
       

      
    }
}
