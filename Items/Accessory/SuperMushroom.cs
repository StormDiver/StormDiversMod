using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Basefiles;
using System.Collections.Generic;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
    public class SuperMushroom : ModItem
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Enchanted Mushroom");
            Tooltip.SetDefault("Increases damage dealt and reduces damage taken when losing health");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            /*foreach (TooltipLine line in tooltips)
            {
                if (line.mod == "Terraria" && line.Name == "Defense")
                {
                    line.text = line.text + " and % increased damage";
                }

            }*/
        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.5f * Main.essScale);
            }
        }
        public override void SetDefaults()
        {
       
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            
            Item.accessory = true;
            Item.maxStack = 1;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }
        //int dustchance;
        public override void UpdateEquip(Player player)
        {
            /*if (Main.rand.Next(dustchance) == 0)
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 113, player.velocity.X, player.velocity.Y, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
                Main.playerDrawDust.Add(dust);
            }*/
        }
        public override void UpdateAccessory(Player player, bool hideVisual) //Maybe add buffs for the levels
        {
            player.GetModPlayer<EquipmentEffects>().mushroomSuper = true;
        }
    }
}