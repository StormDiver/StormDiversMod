using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;


namespace StormDiversMod.Items.Accessory
{

    public class DerpEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of the Derpling");
            Tooltip.SetDefault("Greatly increases luck\nIncreases critical strike damage by 20%\n'Lucky you'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Lime;

            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EquipmentEffects>().derpEye = true;
            player.luck += 0.5f;
        }

      
        /*public class DerpHitProjs : GlobalProjectile
        {
            public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockBack, bool crit) //PvE
            {
                var player = Main.player[projectile.owner];

                if (player.GetModPlayer<EquipmentEffects>().derpEye == true)
                {
                    if (projectile.owner == Main.myPlayer && projectile.friendly && !projectile.minion && !projectile.sentry && projectile.damage > 0 && projectile.knockBack > 0)
                    {
                        if (!target.friendly && target.lifeMax > 5 && !target.boss && target.knockBackResist != 0f)
                        {

                            if (Main.rand.Next(4) == 0)
                            {
                                target.velocity.Y = (-1f * projectile.knockBack) - (target.knockBackResist * 2);
                                target.velocity.X = (0.5f * projectile.knockBack + (target.knockBackResist * 2)) * projectile.direction;
                                if (!target.HasBuff(ModContent.BuffType<Buffs.DerpDebuff>()))
                                {
                                    target.AddBuff(ModContent.BuffType < Buffs.DerpDebuff>(), 45);
                                }
                            }
                        }
                    }
                }
            }
        }
        public class DerpHitMelee : GlobalItem
        {
            public override void OnHitNPC(Item Item, Player player, NPC target, int damage, float knockBack, bool crit) //PvE
            {
                if (player.GetModPlayer<EquipmentEffects>().derpEye == true)
                {
                    if (!target.friendly && target.lifeMax > 5 && !target.boss && target.knockBackResist != 0f)
                    {
                        if (Main.rand.Next(4) == 0)
                        {
                            target.velocity.Y = (-1f * Item.knockBack) - (target.knockBackResist * 2);
                            target.velocity.X = (0.6f * Item.knockBack + (target.knockBackResist * 2)) * player.direction;
                            if (!target.HasBuff(ModContent.BuffType<Buffs.DerpDebuff>()))
                            {
                                target.AddBuff(ModContent.BuffType<Buffs.DerpDebuff>(), 45);
                            }
                        }
                    }
                }
            }
        }*/
    }
}