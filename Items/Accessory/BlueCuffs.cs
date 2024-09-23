using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Common;
using Terraria.GameContent.Creative;
using StormDiversMod.Items.Potions;


namespace StormDiversMod.Items.Accessory
{
   
    //__________________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class BlueCuffs : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Insulated Cuffs");
            //Tooltip.SetDefault("All weapons have a chance to inflict frostburn upon attacked enemies\n'Redirect the coldness into your foes'");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;

            Item.defense = 1;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().blueCuffs = true;
            //player.frostBurn = true;
        }
       
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ItemID.Shackle, 1)
         .AddIngredient(ModContent.ItemType<Items.Materials.BlueCloth>(), 5)
         .AddTile(TileID.Anvils)
         .Register();
           
        }
    }
    public class BlueCuffsEffects : ModPlayer
    {
        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            if (Player.GetModPlayer<EquipmentEffects>().blueCuffs == true == true && !projectile.noEnchantments)
            {
                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustDirect(boxPosition, boxWidth, boxHeight, 187);
                    dust.velocity *= 0.5f;
                    dust.scale = 1.5f;
                    dust.noGravity = true;
                }
            }
        }
        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (Player.GetModPlayer<EquipmentEffects>().blueCuffs == true && item.DamageType.CountsAsClass<MeleeDamageClass>() && !item.noMelee && !item.noUseGraphic)
            {
                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
                    dust.velocity *= 0.5f;
                    dust.scale = 1.5f;
                    dust.noGravity = true;
                }
            }
        }
    }
}