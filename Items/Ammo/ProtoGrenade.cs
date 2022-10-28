using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Ammo
{
    public class ProtoGrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prototype Grenade");
            Tooltip.SetDefault("For use with certain launchers");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;

        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 6);
            Item.rare = ItemRarityID.Orange;

            //Item.melee = true;
            Item.DamageType = DamageClass.Ranged;

            //Item.magic = true;
            //Item.summon = true;
            //Item.thrown = true;

            Item.damage = 30;

            Item.knockBack = 1f;
            Item.consumable = true;


            Item.shoot = ModContent.ProjectileType<Projectiles.AmmoProjs.ProtoGrenadeProj>();
            Item.shootSpeed = 3f;
            Item.ammo = Item.type;
        }
       

        
       
    }
}
