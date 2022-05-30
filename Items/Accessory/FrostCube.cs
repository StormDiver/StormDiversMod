using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;


namespace StormDiversMod.Items.Accessory
{

    public class FrostCube : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen Queen's Core");
            Tooltip.SetDefault("Increases your max number of minions and sentries by 1\nIncreases whip range by 15%");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;

            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Yellow;

            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            
                //player.GetModPlayer<EquipmentEffects>().frostCube = true;
            
            player.maxMinions += 1;
            player.maxTurrets += 1;
            //player.GetDamage(DamageClass.Summon) += 0.1f;
            player.whipRangeMultiplier *= 1.15f;
            //player.whipUseTimeMultiplier *= 0.9f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

    }
    public class FrostCubeProjs : GlobalProjectile
    {
        /*public override bool InstancePerEntity => true;
       
        public override void AI(Projectile projectile)
        {
            var player = Main.player[projectile.owner];
            if (projectile.minion && projectile.friendly)
            {
                if (player.GetModPlayer<EquipmentEffects>().frostCube == true)
                {



                    if (Main.rand.Next(3) == 0)
                    {
                        Dust dust;
                        // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                        Vector2 position = projectile.position;
                        dust = Terraria.Dust.NewDustDirect(position, projectile.width, projectile.height, 16, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                        dust.noGravity = true;
                    }

                    //projectile.extraUpdates = (int)1f;


                }
                
            
            }
            else
            {
                if (projectile.type != ProjectileID.DeadlySphere &&
                   projectile.type != ProjectileID.Spazmamini)
                {
                    projectile.extraUpdates = 0;
                }
            }
        }*/
    }
}