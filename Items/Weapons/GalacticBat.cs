using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Common;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Projectiles;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Terraria.GameContent.Prefixes;
using StormDiversMod.Items.Potions;
using System.Collections.Generic;
using Humanizer;
using Newtonsoft.Json.Linq;

namespace StormDiversMod.Items.Weapons
{
    public class GalacticBat : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Galactic Baseballer"); 
            //Tooltip.SetDefault("Hitting enemies charges the bat up to 10 times, only one charge can be obtained per swing
            //Once the bat is fully charged, right click to unleash a powerful swing that always crits and ignores 15 defense
            //The power swing also inflicts broken which makes enemies take 15 % extra damage for 5 seconds
            //The power swing will not need to be recharged if no enemies are hit when it is swung
            //"RULES... ARE MADE TO BE BROKEN!"'");

            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            PrefixLegacy.ItemSets.SwordsHammersAxesPicks[Item.type] = true;
            /*HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Light"),
                Color = () => new Color(255, 255, 255, 50) * 1f
            });*/
        }
        int aura = 0;
        //int batCount;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            /*foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                {
                    if (batCount >= 10)
                        line.Text = line.Text + "\n[c/ffd700:Fully Charged!]";
                    else
                        line.Text = line.Text + "\n" + batCount + "/10 Charges";
                }
            }*/
        }
        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 50;
            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            //Item.UseSound = SoundID.DD2_MonkStaffSwing;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 6;
            Item.shoot = ModContent.ProjectileType<Projectiles.GalaticBatAura>();
            aura = ModContent.ProjectileType<Projectiles.GalaticBatAura>();
            Item.shootSpeed = 1;
            Item.scale = 1f;
            Item.noMelee = true;
            Item.noUseGraphic = false;
        }
        public override void HoldItem(Player player)
        {
            //batCount = player.GetModPlayer<BatEffects>().BatCount;
            if (player.GetModPlayer<BatEffects>().BatCount >= 10)
            {
                if (Main.rand.Next(4) == 0)
                {
                    var dust = Dust.NewDustDirect(new Vector2(player.position.X, player.Center.Y + (10 * player.gravDir)), player.width, 0, 226, 0, -3 * player.gravDir);
                    dust.noGravity = true;
                    dust.scale = 0.8f;
                }
            }
        }
        public override bool AltFunctionUse(Player player)
        {
            if (player.GetModPlayer<BatEffects>().BatCount >= 10)
                return true;
            else
                return false;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            base.UseStyle(player, heldItemFrame);
        }
        float extraknockback;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                aura = ModContent.ProjectileType<Projectiles.GalaticBatAuraLarge>();
                extraknockback = 1.25f; //extra kb for alt hit
            }
            else
            {
                aura = ModContent.ProjectileType<Projectiles.GalaticBatAura>();
                extraknockback = 1;
            }
            return true;
        }
        public override void UseAnimation(Player player)
        {
            player.itemAnimationMax = Item.useAnimation;//seems to fix aura issue on first swing

            if (aura != 0 && !player.ItemAnimationActive)
            {
                Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                if (player.whoAmI == Main.myPlayer)
                {
                    Vector2 velocity = new Vector2(Math.Sign(mousePosition.X - player.Center.X), 0); // determines direction
                    int damage = (int)(player.GetTotalDamage(Item.DamageType).ApplyTo(Item.damage));
                    Projectile spawnedProj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.MountedCenter - velocity * 2, velocity * 5, aura, damage, Item.knockBack * extraknockback, Main.myPlayer,
                            Math.Sign(mousePosition.X - player.Center.X) * player.gravDir, player.itemAnimationMax, player.GetAdjustedItemScale(Item));
                    NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
                }
                return;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            if (player.altFunctionUse == 2) //Right Click
                SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash with { Volume = 1.5f, Pitch = 0f, MaxInstances = -1 }, player.Center);

            else //left Click
                SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing with { Volume = 1.5f, Pitch = 0f, MaxInstances = -1 }, player.Center);

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.MeteoriteBar, 15)
           .AddIngredient(ItemID.FallenStar, 10)
           .AddRecipeGroup("StormDiversMod:EvilMaterial", 10)
           .AddTile(TileID.Anvils)
           .Register();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //_______________________________________________________________________________________________________________
    public class BatEffects : ModPlayer
    {
        public int BatCount; //charge count
        bool batLimit;//for one time indication that limit is reached
        bool batHit; //has the bat already hit, makes only one charge added per swing
        public override void ResetEffects()
        {
        }
        public override void UpdateDead()//Reset all ints and bools if dead======================
        {
            BatCount = 0;
            batLimit = false;
            batHit = false;
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (proj.type == ModContent.ProjectileType<GalaticBatAuraLarge>()) //alt atttack always crits
            {
                modifiers.SetCrit();
            }
        }
        public override void PreUpdate()
        {
            if (Player.itemAnimation == Player.itemAnimationMax && Player.HeldItem.type == ModContent.ItemType<GalacticBat>()) //resets bool to prevent more than one charge per swing
            {
                batHit = false;
            }
            //Main.NewText("Has been hit: " + batHit);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.type == ModContent.ProjectileType<GalaticBatAura>() && target.lifeMax > 5 && !target.friendly && !batHit) //left click, add once charge per swing
            {
                if (BatCount < 10) //Count up each hit (have to have separate or text counter includes 10 and only shows charge text on 11
                    BatCount++;

                if (BatCount < 10) //count up, play sound and display count text
                {
                    CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.Gold, BatCount, false);
                    SoundEngine.PlaySound(SoundID.NPCHit53 with {Volume = 0.33f, Pitch = 0.5f, MaxInstances = 2, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, target.Center);
                    for (int i = 0; i < 10; i++) //Alt attack charged
                    {
                        Vector2 perturbedSpeed = new Vector2(0, -2f).RotatedByRandom(MathHelper.ToRadians(360));
                        var dust = Dust.NewDustDirect(Player.Center, 0, 0, 226, perturbedSpeed.X, perturbedSpeed.Y);
                        dust.noGravity = true;
                        dust.scale = 0.5f;
                    }
                }
                else if (BatCount == 10 && !batLimit) //at max charge, sound, particle, and text only once
                {
                    SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 0.75f, Pitch = 0f, MaxInstances = 0 }, target.Center);

                    for (int i = 0; i < 30; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));
                        var dust = Dust.NewDustDirect(Player.Center, 0, 0, 226, perturbedSpeed.X, perturbedSpeed.Y);
                        dust.noGravity = true;
                    }
                    CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.Gold, "RULES ARE MADE TO BE BROKEN!", false);
                    batLimit = true;
                }
                batHit = true; //only show this effect once
            }
            if (proj.type == ModContent.ProjectileType<GalaticBatAuraLarge>() && BatCount == 10 && target.lifeMax > 5 && !target.friendly) //hittign with alt attack
            {
                Player.GetModPlayer<MiscFeatures>().screenshaker = true; //very powerful >:)

                SoundEngine.PlaySound(SoundID.Item122 with { Volume = 1f, Pitch = 0f, MaxInstances = -1 }, Player.Center);
                for (int i = 0; i < 30; i++) 
                {
                    Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));
                    var dust = Dust.NewDustDirect(Player.Center, 0, 0, 226, perturbedSpeed.X, perturbedSpeed.Y);
                    dust.noGravity = true;
                }
                batLimit = false;

                BatCount = 0;
            }
            //Main.NewText(BatCount + " Whacks");
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            
        }
    }
}