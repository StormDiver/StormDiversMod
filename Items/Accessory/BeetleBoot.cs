using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;
using System.Collections.Generic;


namespace StormDiversMod.Items.Accessory
{
    [AutoloadEquip(EquipType.HandsOn)]

    public class BeetleBoot : ModItem //Now Beetle gauntlet
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beetle Gauntlet");
            Tooltip.SetDefault("Critical striking an enemy with a melee weapon causes mini beetles to burst out of them and swarm nearby enemies\nIncreases melee knockback\n12% increased melee speed\nEnables auto swing for melee weapons\nIncreases the size of melee weapons");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (ModLoader.HasMod("TRAEProject"))//DON'T FORGET THIS!!!!!!!
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = "";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        line.Text = "25% increased melee weapon size and 50% increased melee velocity";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                    {
                        line.Text = "";
                    }
                }

            }
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;

            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetModPlayer<EquipmentEffects>().beetleFist = true;
            player.autoReuseGlove = true;
            player.kbGlove = true;
            player.meleeScaleGlove = true; //keep with TRAE as it does not affect TRAE spears yet


            if (ModLoader.HasMod("TRAEProject"))//DON'T FORGET THIS!!!!!!!
            {
                player.GetModPlayer<TRAEchanges.MeleeStats>().weaponSize += 0.25f;
                player.GetModPlayer<TRAEchanges.MeleeStats>().meleeVelocity += 0.5f;
            }
            else
            {
                player.GetAttackSpeed(DamageClass.Melee) += 0.12f;

            }
        }


        public override void AddRecipes()
        {
            CreateRecipe()
          .AddIngredient(ItemID.BeetleHusk, 10)
          .AddIngredient(ItemID.PowerGlove, 1)
          .AddIngredient(ItemID.SoulofMight, 10)
          .AddTile(TileID.MythrilAnvil)

          .Register();
           

        }


    }
}