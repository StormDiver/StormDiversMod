using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;
using System.Collections.Generic;
using StormDiversMod.Projectiles.ToolsProjs;
using rail;
using Humanizer;

namespace StormDiversMod.Items.Tools
{

    public class Oilcan : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Oil Can");
            //Tooltip.SetDefault("Left click to fire out a blob of oil that inflicts the oiled debuff onto enemies and town NPCs
           // Right click to set nearby oiled enemies ablaze, consumes 3 cans
           //'Cover your enemies in oil'
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 22;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 0, 0, 40);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.damage = 0;
            Item.DamageType = DamageClass.Generic;

            Item.shoot = ModContent.ProjectileType<OilCanProj>();
           
            //Item.UseSound = SoundID.Item86;

            //Item.crit = 0;
            Item.knockBack = 0f;

            Item.shootSpeed = 7.5f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(6, 0);
        }
        public override bool AltFunctionUse(Player player)
        {
                return true;
        }
        public override bool CanUseItem(Player player)
        {

            if (player.altFunctionUse == 2)
            {
                Item.consumable = false;

                Item.useStyle = ItemUseStyleID.HoldUp;
                Item.useTime = 30;
                Item.useAnimation = 30;
            }
            else
            {
                Item.consumable = true;

                Item.useStyle = ItemUseStyleID.Shoot;
                Item.useTime = 15;
                Item.useAnimation = 15;
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2) //Right Click
            {
                if (player.CountItem(ModContent.ItemType<Oilcan>(), 9999) >= 3)
                {
                    SoundEngine.PlaySound(SoundID.Item100 with { Volume = 1f, Pitch = -2 }, player.Center);

                    for (int i = 0; i < 10; i++) //Orange particles
                    {
                        var dust = Dust.NewDustDirect(new Vector2(player.Center.X + (42 * player.direction), player.Center.Y - 36 * player.gravDir), 0, 0, 174, 0, 0);
                        dust.noGravity = true;
                        dust.scale = 1.5f;

                    }
                    for (int i = 0; i < 175; i++) //Orange particles
                    {
                        Vector2 perturbedSpeed = new Vector2(0, -40f).RotatedByRandom(MathHelper.ToRadians(360));

                        var dust = Dust.NewDustDirect(new Vector2(player.Center.X + (42 * player.direction), player.Center.Y - 36 * player.gravDir), 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                        dust.noGravity = true;
                        dust.scale = 2f;

                    }
                    for (int i = 0; i < 200; i++)//for town npcs
                    {
                        NPC target = Main.npc[i];

                        float npcdistance = Vector2.Distance(target.Center, new Vector2(player.Center.X + (42 * player.direction), player.Center.Y - 36 * player.gravDir));

                        if (npcdistance <= 400 && target.HasBuff(BuffID.Oiled) && target.active && target.lifeMax >= 5 && !target.dontTakeDamage && Collision.CanHitLine(target.position, target.width, target.height, new Vector2(player.Center.X + (42 * player.direction), player.Center.Y - 36), 0, 0))
                        {
                            target.AddBuff(BuffID.OnFire, Main.rand.Next(600, 901)); //10-15 seconds

                            for (int j = 0; j < 15; j++) //Orange particles
                            {
                                Vector2 perturbedSpeed = new Vector2(0, -6f).RotatedByRandom(MathHelper.ToRadians(360));

                                var dust = Dust.NewDustDirect(new Vector2(target.Center.X, target.Center.Y), 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y);
                                dust.noGravity = true;
                                dust.scale = 2f;
                            }
                        }
                    }
                    for (int j = 0; j < 3; j++)
                    {
                        player.ConsumeItem(ModContent.ItemType<Oilcan>(), false, false);
                    }
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath6, position);

                    for (int i = 0; i < 10; i++) //grey particles
                    {
                        var dust = Dust.NewDustDirect(new Vector2(player.Center.X + (42 * player.direction), player.Center.Y - 36 * player.gravDir), 0, 0, 31, 0, 0);
                        dust.noGravity = true;
                        dust.scale = 1.5f;

                    }
                }
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item86 with { Volume = 1f, Pitch = 0 }, player.Center);

                Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
                if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                {
                    position += muzzleOffset;
                }
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y - 12 * player.gravDir), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            
        }
    }
}