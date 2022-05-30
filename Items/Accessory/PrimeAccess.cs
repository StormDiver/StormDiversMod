using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Accessory
{
   
    public class PrimeAccess : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Spikes");
            Tooltip.SetDefault("Six spike balls will orbit you, damaging enemies within reach\nHas a small chance to destroy almost any projectile that comes near");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 40;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Pink;

            Item.defense = 5;
            Item.accessory = true;
            Item.expert = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }


        //int skulltime = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            
            player.GetModPlayer<EquipmentEffects>().primeSpin = true;
            //skulltime++;

            /*
            if (skulltime >=20)
            {
                
               
                int damage = 20;
                float speedX = 0f;
                float speedY = -24f;
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                float scale = 1f - (Main.rand.NextFloat() * .5f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("PrimeAccessProj"), damage, 3f, player.whoAmI);
                
                skulltime = 0;
            }*/
           
        }


    }
}