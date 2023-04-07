using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Accessory
{
   
    //__________________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.HandsOn)]
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
         .AddIngredient(ModContent.ItemType<Items.Materials.BlueCloth>(), 10)
         .AddTile(TileID.Anvils)
         .Register();
           
        }
    }
    public class BlueCuffsProjs : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile) //Dust effects
        {
            var player = Main.player[projectile.owner];
            if (projectile.aiStyle != 20)
            {
                if (player.GetModPlayer<EquipmentEffects>().blueCuffs == true)
                {
                    if (projectile.owner == Main.myPlayer && projectile.friendly && !projectile.minion && !projectile.sentry && projectile.damage >= 1)
                    {

                        if (Main.rand.Next(4) < 2)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 187, 0f, 0f, 100, default, 1f);
                            Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                            Main.dust[dustIndex].noGravity = true;
                        }
                    }
                }
            }
            
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) //PvE

            var player = Main.player[projectile.owner];

            if (player.GetModPlayer<EquipmentEffects>().blueCuffs == true)
            {
                if (projectile.owner == Main.myPlayer && projectile.friendly)
                {
                    target.AddBuff(BuffID.Frostburn, 120);
                }
            }

        }
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                var player = Main.player[projectile.owner];

                if (player.GetModPlayer<EquipmentEffects>().blueCuffs == true)
                {
                    if (projectile.owner == Main.myPlayer && projectile.friendly)
                    {
                        target.AddBuff(BuffID.Frostburn, 120);
                        //target.AddBuff(mod.BuffType("SuperFrostBurn"), 600);
                    }
                }
            }
        }
    }
    public class BlueCuffsMelee : GlobalItem
    {
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {

            if (player.GetModPlayer<EquipmentEffects>().blueCuffs == true)
            {
                if (Main.rand.Next(4) == 0)
                {
                    target.AddBuff(BuffID.Frostburn, 180);
                }
            }

        }
        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (player.GetModPlayer<EquipmentEffects>().blueCuffs == true)
            {
                if (Main.rand.Next(4) == 0)
                {
                    target.AddBuff(BuffID.Frostburn, 180);
                }
            }
        }
        public override void MeleeEffects(Item Item, Player player, Rectangle hitbox) //Dust Effects
        {
            if (player.GetModPlayer<EquipmentEffects>().blueCuffs == true)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }
        }          
        
    }
}