using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Tools
{
    public class FastDrill : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Drill");
            Tooltip.SetDefault("'Speeds up your mining experience'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            
            Item.damage = 5;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 22;
        
            Item.useTime = 5;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.knockBack = 1f;
           
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.FastDrillProj>();
            Item.shootSpeed = 25f;
            Item.pick = 55;
            Item.tileBoost = -1;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true; 
            Item.channel = true; 
            Item.autoReuse = true;
            
            
        }
       
    }
   
}