using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class MineBomb : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sticky Mine");
            //Tooltip.SetDefault("For use with the Sticky Mine Detonator
            //'Be careful not to get this stuck on you'
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 10;
            Item.height = 10;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.noUseGraphic = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 0, 0, 60);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 0f;
            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.MineBombProj>();
            Item.ammo = Item.type;

            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
        }

    }
}
