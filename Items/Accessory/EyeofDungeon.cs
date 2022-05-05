using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;


namespace StormDiversMod.Items.Accessory
{
   
    public class EyeofDungeon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of the Dungeon");
            Tooltip.SetDefault("Summons homing spinning bones from where you stand");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;

            Item.defense = 2;
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }


        int skulltime = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.statLife = 1;

            skulltime++;
    
            if (skulltime >=40)
            {
                
               
                int damage = 16;
                float speedX = 0f;
                float speedY = -24f;
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                float scale = 1f - (Main.rand.NextFloat() * .5f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(null, new Vector2(player.Center.X, player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.BoneAcProj>(), damage, 3f, player.whoAmI);

                
                skulltime = 0;
            }
        }
       
       
    }
}