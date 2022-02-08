using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Weapons
{
	
    public class BoneBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Femurang");
            Tooltip.SetDefault("3 can be thrown out at a time\n'What, you thought this was Humerus?'");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 46;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
    
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.crit = 0;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 32;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;  
            Item.value = Item.sellPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.knockBack = 5f;
            Item.shoot = ModContent.ProjectileType < Projectiles.BoneBoomerangProj>();
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.noUseGraphic = true;

        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 3;

        }
       
        
        
    }
    
}