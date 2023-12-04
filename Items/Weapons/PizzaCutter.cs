using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Weapons
{
    public class PizzaCutter : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Motorized Pizza Cutter");
            //Tooltip.SetDefault("'Summons larger spinning pizza cutter blades when attacking enemies 
            //The larger blade ignores 15 enemy defense and slowly homes towards nearby enemies
             //'What kind of pizza was this designed for?''");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.IsDrill[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 40;
            Item.height = 22;

            Item.useTime = 4;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 2, 50, 0);
                     Item.rare = ItemRarityID.Pink;
            Item.knockBack = 1.5f;
           
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.PizzaCutterProj>();
            Item.shootSpeed = 50f;
            //Item.pick = 180;
            Item.axe = 18;

            Item.tileBoost = 0;
            Item.UseSound = SoundID.Item23;
            Item.noMelee = true;
            Item.noUseGraphic = true; 
            Item.channel = true; 
            Item.autoReuse = true;
        }    
    }
    
}