using Microsoft.Xna.Framework;
using Mono.Cecil;
using SteelSeries.GameSense.DeviceZone;
using StormDiversMod.Common;
using StormDiversMod.NPCs;
using StormDiversMod.Projectiles.Minions;
using StormDiversMod.Projectiles.SentryProjs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace StormDiversMod.Items.Tools
{
    [AutoloadEquip(EquipType.Back)]

    public class Wrangler : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Remote Sentry Detonator");
            //Tooltip.SetDefault("Can be used to manually aim placed sentries, also overlcocks them and makes them fire 3 times as fast");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.shoot = 1;
            Item.sentry = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MiscFeatures>().wrangler = true;
            player.maxTurrets += 1;
        }
        public override bool CanUseItem(Player player) => true;

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-0, 0);
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            base.UseStyle(player, heldItemFrame);

            player.itemLocation += new Vector2(-14 * player.direction, -4);
        }
        public override void UseAnimation(Player player)
        {
            base.UseAnimation(player);
        }
        bool summontarget;
        public override void HoldItem(Player player)
        {
            if (player.controlUseItem)
            {
                /*if (!summontarget) //bascally summons an NPC that is immune to damage and flows the cursor and has the target set on it
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int npcIndex = NPC.NewNPC(null, (int)Math.Round(Main.MouseWorld.X), (int)Math.Round(Main.MouseWorld.Y), ModContent.NPCType<XSentryTarget>(), 0);
                        //Main.player[Main.myPlayer].MinionAttackTargetNPC = 0;
                    }
                    SoundEngine.PlaySound(SoundID.Zombie67 with { Volume = 0.5f, Pitch = 0 }, player.Center);

                    summontarget = true;
                }*/
                if (player.GetModPlayer<MiscFeatures>().wrangler == false)
                {
                    if (Main.rand.Next(5) == 0)
                    {
                        Vector2 perturbedSpeed = new Vector2(0, -1f).RotatedByRandom(MathHelper.ToRadians(360));
                        var dust = Dust.NewDustDirect(new Vector2(player.Center.X + 30 * player.direction, player.Center.Y + -6), 0, 0, 182, perturbedSpeed.X, perturbedSpeed.Y);
                        dust.noGravity = true;
                        dust.scale = 0.8f;
                    }
                    Dust dust2;
                    dust2 = Dust.NewDustPerfect(new Vector2(player.Center.X + 30 * player.direction, player.Center.Y + -4), 182, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
                    dust2.noGravity = true;
                }
                else
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        Vector2 perturbedSpeed = new Vector2(0, -0.5f).RotatedByRandom(MathHelper.ToRadians(360));
                        var dust = Dust.NewDustDirect(new Vector2((player.Center.X + 30 * player.direction) - 4, player.Center.Y + -6), 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);
                        dust.noGravity = true;
                        dust.scale = 0.9f;
                    }
                }
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Main.PlaySound(SoundID.NPCKilled, (int)position.X, (int)position.Y, 59);
            /*if (player.altFunctionUse != 2)
            {
                SoundEngine.PlaySound(SoundID.Item115 with { Volume = 0.5f, Pitch = 0.5f }, player.Center);

                for (int i = 0; i < 50; i++)
                {
                    float speedX = 0f;
                    float speedY = -5f;

                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dustIndex = Dust.NewDust(new Vector2(player.Center.X + (20 * player.direction), player.Center.Y - 16 * player.gravDir), 0, 0, 21, perturbedSpeed.X, perturbedSpeed.Y, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }
            }*/
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 15)
            .AddIngredient(ItemID.Wire, 25)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
    public class SentryControl : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        int controlrange = 600; //600 for non-vanilla sentries

        bool aurasentry;
        public override void AI(Projectile projectile)
        {
            var player = Main.player[projectile.owner];
            switch (projectile.type) //set range here
            {
                case ProjectileID.DD2LightningAuraT1 or ProjectileID.DD2LightningAuraT2 or ProjectileID.DD2LightningAuraT3
                or ProjectileID.DD2ExplosiveTrapT1 or ProjectileID.DD2ExplosiveTrapT2 or ProjectileID.DD2ExplosiveTrapT3:
                    controlrange = 750;
                    aurasentry = true;
                    break;
                case ProjectileID.DD2BallistraTowerT1 or ProjectileID.DD2BallistraTowerT2 or ProjectileID.DD2BallistraTowerT3:
                    controlrange = 900;
                    break;
                case ProjectileID.DD2FlameBurstTowerT1 or ProjectileID.DD2FlameBurstTowerT2 or ProjectileID.DD2FlameBurstTowerT3:
                    if (player.setApprenticeT2 || player.setApprenticeT3)
                        controlrange = 1350;
                    else
                        controlrange = 900;
                    break;
                case ProjectileID.HoundiusShootius or ProjectileID.SpiderHiver or ProjectileID.FrostHydra or ProjectileID.MoonlordTurret or ProjectileID.RainbowCrystal:
                    controlrange = 1000;
                    break;
            }
            //limit range here, all main modded sentires have this hardcoded into them
            if (projectile.type == ModContent.ProjectileType<SkyKnightSentryProj>())
                controlrange = 500;
            else if (projectile.type == ModContent.ProjectileType<LunaticExpertSentryProj>())
                controlrange = 800;

            if (projectile.sentry || projectile.type == ModContent.ProjectileType<SkyKnightSentryProj>() || projectile.type == ModContent.ProjectileType<LunaticExpertSentryProj>())
            {
                if (player.controlUseTile)
                {
                    //projectile.Center = player.Top;
                    //projectile.velocity.Y = 0;
                }
                if (projectile.ai[2] != 127 && !aurasentry) //my modded sentries have this value set to 127 as they have their own targetting and speed increasing code
                {
                    if (player.GetModPlayer<MiscFeatures>().sentryControl && Vector2.Distance(projectile.Center, Main.MouseWorld) <= controlrange && (Collision.CanHitLine(projectile.Center, 1, 1, Main.MouseWorld, 0, 0)))//FUNI idea
                    {
                        if (!player.GetModPlayer<MiscFeatures>().wrangler)
                            projectile.extraUpdates = 2;

                        /*for (int i = 0; i < 10; i++)
                        {
                            double deg = Main.rand.Next(0, 360); //The degrees
                            double rad = deg * (Math.PI / 180); //Convert degrees to radians
                            double dist = 50; //distance away from the player
                            float dustx = projectile.Center.X - (int)(Math.Cos(rad) * dist);
                            float dusty = projectile.Center.Y - (int)(Math.Sin(rad) * dist);
                            if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, projectile.position, projectile.width, projectile.height))//no dust unless lien of sight
                            {
                                var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 0, 0, 182, 0, 0);
                                dust.noGravity = true;
                                dust.scale = 1f;
                                dust.velocity *= 0;
                            }
                        }*/
                        if (!player.GetModPlayer<MiscFeatures>().wrangler)
                            if (Main.rand.Next(5) == 0)
                            {
                                int dust = Dust.NewDust(new Vector2(projectile.Center.X - 15, projectile.Center.Y - 2), 30, 2, 182, 0, -3, 100, default, 1f);
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].velocity *= 0.75f;
                            }
                    }
                    else
                    {
                        projectile.extraUpdates = 0;
                    }
                }
                if (aurasentry) //since they're so flat on the ground I raised the line of sight a bit
                {
                    if (player.GetModPlayer<MiscFeatures>().sentryControl && Vector2.Distance(new Vector2(projectile.Center.X, projectile.Center.Y - 5), Main.MouseWorld) <= controlrange && (Collision.CanHitLine(new Vector2(projectile.Center.X, projectile.Center.Y - 5), 1, 1, Main.MouseWorld, 0, 0)))//FUNI idea
                    {
                        if (!player.GetModPlayer<MiscFeatures>().wrangler)
                        projectile.extraUpdates = 2;
                        if (!player.GetModPlayer<MiscFeatures>().wrangler)
                            if (Main.rand.Next(5) == 0)
                            {
                                int dust = Dust.NewDust(new Vector2(projectile.Center.X - 15, projectile.Center.Y - 2), 30, 2, 182, 0, -3, 100, default, 1f);
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].velocity *= 0.75f;
                            }
                    }
                    else
                        projectile.extraUpdates = 0;
                }
            }
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor) // laser sight
        {
            var player = Main.player[projectile.owner];
            if (player.GetModPlayer<MiscFeatures>().sentryControl)
            {
                if (projectile.sentry && projectile.ai[2] != 127)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        if (!aurasentry && projectile.type != ProjectileID.MoonlordTurret) //Luanr portal sentry line is misalinged for some reason
                        {
                            if (Vector2.Distance(projectile.Center, Main.MouseWorld) <= controlrange && Collision.CanHitLine(projectile.Center, 0, 0, Main.MouseWorld, 0, 0))
                            {
                                Utils.DrawLine(Main.spriteBatch, new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), Color.DarkRed, Color.Red, 1.5f);
                                //Utils.DrawLine(Main.spriteBatch, new Vector2(player.Center.X, player.Center.Y), new Vector2(projectile.Center.X, projectile.Center.Y), Color.Yellow, Color.Transparent, 2.5f);
                            }
                        }
                        /*else if (aurasentry) //too buggy
                        {
                            if (Vector2.Distance(projectile.Center, Main.MouseWorld) <= controlrange && Collision.CanHitLine(new Vector2(projectile.Center.X, projectile.Center.Y - 5), 0, 0, Main.MouseWorld, 0, 0))
                            {
                                Utils.DrawLine(Main.spriteBatch, new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), Color.Transparent, Color.Red, 1.5f);
                                //Utils.DrawLine(Main.spriteBatch, new Vector2(player.Center.X, player.Center.Y), new Vector2(projectile.Center.X, projectile.Center.Y), Color.Yellow, Color.Transparent, 2.5f);
                            }
                        }*/
                    }
                }
            }
            return true;
        }
        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
            var player = Main.player[projectile.owner];

            if (projectile.sentry && player.GetModPlayer<MiscFeatures>().sentryControl && Vector2.Distance(projectile.Center, Main.MouseWorld) <= controlrange && (Collision.CanHitLine(projectile.Center, 1, 1, Main.MouseWorld, 0, 0)) 
                && projectile.ai[2] != 127 && !player.GetModPlayer<MiscFeatures>().wrangler)//FUNI idea
            {
                Color color = Color.OrangeRed;
                color.A = 200;
                return color;
            }
            else
            {
                return null;
            }
        }
    }
}