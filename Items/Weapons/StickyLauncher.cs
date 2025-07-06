using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using Humanizer;
using static Humanizer.In;
using System.Net.Sockets;

namespace StormDiversMod.Items.Weapons
{
    public class StickyLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spiky Bomb Launcher");
            //Tooltip.SetDefault("Fires out up to 16 Spiky Bombs that stick to surfaces, the oldest bomb will automatically explode upon firing a 17th one
            //Bombs near the cursor can be detonated by pressing right click, holding down right click while holding the weapon will detonate all bombs
            //Bombs are shot further depending on your cursor distance
            //Can be used to launch yourself into the air, also works on enemies and Town NPCs
            //Uses rockets as ammo");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            //Item.reuseDelay = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<StickyBombProj>();
            Item.useAmmo = AmmoID.Rocket;
            Item.UseSound = SoundID.Item61;
        
            Item.damage = 100;
            //Item.crit = 4;
            Item.knockBack = 3f;
            Item.shootSpeed = 10;
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than 16 bombs
            //return player.ownedProjectileCounts[Item.shoot] < 16;
            return true;
        }
        float shootvelo = 1; //Speed multiplers
        public override bool? UseItem(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {        
            shootvelo = Vector2.Distance(Main.MouseWorld, player.Center) / 500f + 0.2f; //Faster shoot speed at further distances
            if (shootvelo > 2f) //Caps the speed multipler at 2x
            {
                shootvelo = 2f;
            }
            if (shootvelo < 0.5f) // Caps low end at 0.5x
            {
                shootvelo = 0.5f;
            }
            for (int i = 0; i < 1; i++)
            {
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X * shootvelo, velocity.Y * shootvelo), ModContent.ProjectileType<StickyBombProj>(), damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item61, position);
            }
            //Kills oldest projectile when 17th is summoned
            int spikyprojs = 0;
            int oldestProjIndex = -1;
            int oldestProjTimeLeft = 100000;
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<StickyBombProj>())
                {
                    spikyprojs++;
                    if (Main.projectile[i].timeLeft < oldestProjTimeLeft)
                    {
                        oldestProjIndex = i;
                        oldestProjTimeLeft = Main.projectile[i].timeLeft;
                    }
                }
            }
            if (spikyprojs > 16)
            {
                Main.projectile[oldestProjIndex].timeLeft = 3;
            }
            if (spikyprojs >= 16)
                CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.Red, "MAX", false);
            else
                CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.White, spikyprojs, false);

            return false;
        }
    }
}
 