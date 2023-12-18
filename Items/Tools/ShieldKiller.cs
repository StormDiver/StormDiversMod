using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Tools
{
    public class ShieldKiller : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Celestial Globe");
            //Tooltip.SetDefault("Removes the shields from all active pillars");
            Item.ResearchUnlockCount = 1;


        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useTurn = false;
            Item.autoReuse = false;
                Item.shoot = ProjectileID.TowerDamageBolt;
                Item.shootSpeed = 0f;

            Item.noMelee = true; 
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, -10);
        }
        public override void UseAnimation(Player player)
        {
           
            base.UseAnimation(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (NPC.ShieldStrengthTowerVortex > 0 || NPC.ShieldStrengthTowerSolar > 0 || NPC.ShieldStrengthTowerNebula > 0 || NPC.ShieldStrengthTowerStardust > 0)
            {
                SoundEngine.PlaySound(SoundID.Item122, position);
               
                    if (NPC.ShieldStrengthTowerVortex > 0)
                    {
                        int ProjID1 = Projectile.NewProjectile(null, new Vector2(player.Center.X + (25 * player.direction), player.Center.Y - 20 * player.gravDir), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(422));
                        Main.projectile[ProjID1].extraUpdates += 5;
                    }
                    if (NPC.ShieldStrengthTowerSolar > 0)
                    {
                        int ProjID2 = Projectile.NewProjectile(null, new Vector2(player.Center.X + (25 * player.direction), player.Center.Y - 20 * player.gravDir), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(517));
                        Main.projectile[ProjID2].extraUpdates += 5;
                    }
                    if (NPC.ShieldStrengthTowerNebula > 0)
                    {
                        int ProjID3 = Projectile.NewProjectile(null, new Vector2(player.Center.X + (25 * player.direction), player.Center.Y - 20 * player.gravDir), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(507));
                        Main.projectile[ProjID3].extraUpdates += 5;
                    }
                    if (NPC.ShieldStrengthTowerStardust > 0)
                    {
                        int ProjID4 = Projectile.NewProjectile(null, new Vector2(player.Center.X + (25 * player.direction), player.Center.Y - 20 * player.gravDir), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(493));
                        Main.projectile[ProjID4].extraUpdates += 5;
                    }
               
                /*else if (player.gravDir == 0)
                {
                    if (NPC.ShieldStrengthTowerVortex > 0)
                    {
                        int ProjID1 = Projectile.NewProjectile(null, new Vector2(player.Center.X + (25 * player.direction), player.Bottom.Y + 4), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(422));
                        Main.projectile[ProjID1].extraUpdates += 5;
                    }
                    if (NPC.ShieldStrengthTowerSolar > 0)
                    {
                        int ProjID2 = Projectile.NewProjectile(null, new Vector2(player.Center.X + (25 * player.direction), player.Bottom.Y + 4), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(517));
                        Main.projectile[ProjID2].extraUpdates += 5;
                    }
                    if (NPC.ShieldStrengthTowerNebula > 0)
                    {
                        int ProjID3 = Projectile.NewProjectile(null, new Vector2(player.Center.X + (25 * player.direction), player.Bottom.Y + 4), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(507));
                        Main.projectile[ProjID3].extraUpdates += 5;
                    }
                    if (NPC.ShieldStrengthTowerStardust > 0)
                    {
                        int ProjID4 = Projectile.NewProjectile(null, new Vector2(player.Center.X + (25 * player.direction), player.Bottom.Y + 4), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(493));
                        Main.projectile[ProjID4].extraUpdates += 5;
                    }
                }*/

                //sets to 1, then the shield killer projectiles are spawned ontop of the pillars in NPCeffects
                NPC.ShieldStrengthTowerVortex = 1;

                NPC.ShieldStrengthTowerSolar = 1;
                NPC.ShieldStrengthTowerNebula = 1;
                NPC.ShieldStrengthTowerStardust = 1;

                //Projectiles for vanity

                Main.NewText("The Shields guarding the Celestial pillars have been stripped away!", 0, 204, 170);

                for (int i = 0; i < 100; i++)
                {

                    float speedX = 0f;
                    float speedY = -5f;

                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(new Vector2(player.Center.X + (25 * player.direction), player.Center.Y - 20 * player.gravDir), 0, 0, 229, perturbedSpeed.X, perturbedSpeed.Y, 200, default, 1.5f);
                    Main.dust[dust2].noGravity = true;
                }
                if (ModLoader.TryGetMod("TMLAchievements", out Mod mod))
                {
                    mod.Call("Event", "NoShield");
                }
            }
            else if (NPC.ShieldStrengthTowerVortex == 0 && NPC.ShieldStrengthTowerSolar == 0 && NPC.ShieldStrengthTowerNebula == 0 && NPC.ShieldStrengthTowerStardust == 0)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath6, position);
                for (int i = 0; i < 50; i++)
                {
                    float speedX = 0f;
                    float speedY = -5f;

                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dustIndex = Dust.NewDust(new Vector2(player.Center.X + (25 * player.direction), player.Center.Y - 18 * player.gravDir), 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }

                Main.NewText("There are no active shields...", 150, 75, 76);
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FragmentSolar, 40)
            .AddIngredient(ItemID.FragmentVortex, 40)
            .AddIngredient(ItemID.FragmentNebula, 40)
            .AddIngredient(ItemID.FragmentStardust, 40)
         .AddTile(TileID.LunarCraftingStation)
         .Register();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}