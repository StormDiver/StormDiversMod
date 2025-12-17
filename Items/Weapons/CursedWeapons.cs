using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Projectiles;
using Terraria.GameContent.Creative;
using static System.Net.Mime.MediaTypeNames;
using StormDiversMod.Common;
using Terraria.Audio;
using Humanizer;
using Steamworks;

namespace StormDiversMod.Items.Weapons
{
    public class CursedSkullMinion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cursed Skull Staff");
            //Tooltip.SetDefault("Summons a Cursed Skull minion to fight for you
            //The minion will fly next to enemies and will fire cursed bone projectiles at them that bounce and pierce");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.knockBack = 2f;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<Projectiles.Minions.CursedSkullMinionBuff>();
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.CursedSkullMinionProj>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);

            // Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position.
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/CursedSkullMinion_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //_____________________________________________________
    public class CursedSpearGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cursed Spear Shotgun"); 
            //Left click to use as a fast spear that inflicts frostburn and ignores 10 defense
            //Right cick to fire as highly accurate shotgun
            //'Whose idea was it to strap a shotgun to a spear ?'");
            Item.ResearchUnlockCount = 1;
            Item.staff[Item.type] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.damage = 32;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            //Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 2;
            Item.shoot = ModContent.ProjectileType<CursedSpearGunProj>();
            Item.shootSpeed = 10;
            Item.scale = 1f;
            Item.noMelee = true;
            //Item.shootsEveryUse = true;
        }
        public override void UpdateInventory(Player player)
        {
            if (!player.GetModPlayer<CursedShotgunEffects>().bulletlimit && player.GetModPlayer<CursedShotgunEffects>().Bulletcount == 1)
                Item.SetNameOverride("Cursed Spear Shotgun - " + player.GetModPlayer<CursedShotgunEffects>().Bulletcount + " hit");
            else if (!player.GetModPlayer<CursedShotgunEffects>().bulletlimit && player.GetModPlayer<CursedShotgunEffects>().Bulletcount != 1)
                Item.SetNameOverride("Cursed Spear Shotgun - " + player.GetModPlayer<CursedShotgunEffects>().Bulletcount + " hits");
            else
                Item.SetNameOverride("Cursed Spear Shotgun - " + "READY!");
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {

            if (player.altFunctionUse == 2) //gun
            {
                Item.useAmmo = AmmoID.Bullet;
                Item.useStyle = ItemUseStyleID.Shoot;
                //Item.DamageType = DamageClass.Ranged;
                Item.reuseDelay = 20;
                Item.noUseGraphic = false;
                return true;
            }
            else //spear
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.shoot = ModContent.ProjectileType<CursedSpearGunProj>();
                Item.useAmmo = 0;
                //Item.DamageType = DamageClass.Melee;
                Item.reuseDelay = 0;
                Item.noUseGraphic = true;

                return player.ownedProjectileCounts[Item.shoot] < 1;
            }
        }
        public override void HoldItem(Player player)
        {
            if (player.GetModPlayer<CursedShotgunEffects>().bulletlimit == true)
            {
                if (Main.rand.Next(5) < 1)
                {
                    int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 68, 0, 1, 50, default, 1.25f);
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                }
                player.armorEffectDrawOutlines = true;
            }
        }
        Vector2 muzzleOffset = new Vector2(0, 0);
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.altFunctionUse == 2)
                player.itemLocation += muzzleOffset * -1.2f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);

            if (player.altFunctionUse == 2) //Right Click
            {
                muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 20;
                if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                {
                    position += muzzleOffset;
                }
                if (type == ProjectileID.Bullet)
                {
                    type = ModContent.ProjectileType<Projectiles.CursedSpearBulletProj>();
                }
                for (int i = 0; i < 3; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(5));
                    float scale = 1f - (Main.rand.NextFloat() * .3f);
                    perturbedSpeed = perturbedSpeed * scale;
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y - (2 * player.gravDir)), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage / 2, knockback * 1.5f, player.whoAmI);
                }
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y - (2 * player.gravDir)), new Vector2(velocity.X, velocity.Y), type, damage / 2, knockback * 1.5f, player.whoAmI);

                SoundEngine.PlaySound(SoundID.Item36 with { Volume = 0.66f, Pitch = -0.25f }, player.Center);
                return false;

            }
            else //left Click
            {
                SoundEngine.PlaySound(SoundID.Item1, player.Center);
                return true;
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/CursedSpearGun_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //_______________________________________________________________________________________________________________
    public class CursedShotgunEffects : ModPlayer
    {
        public int Bulletcount; //charge count
        public bool bulletlimit;//for one time indication that limit is reached
        public override void ResetEffects()
        {
        }
        public override void UpdateDead()//Reset all ints and bools if dead======================
        {
            Bulletcount = 0;
            bulletlimit = false;
        }
        public override void PreUpdate()
        {
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.type == ModContent.ProjectileType<CursedSpearBulletProj>() && target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy) //bullet
            {
                if (Bulletcount < 12) //Count up each hit 
                    Bulletcount++;

                if (Bulletcount < 12) //count up, play sound and display count text
                {
                    //CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.LightBlue, Bulletcount, false);
                    SoundEngine.PlaySound(SoundID.Item8 with { Volume = 1f, Pitch = 0 }, Player.Center);
                    for (int i = 0; i < 10; i++)
                    {
                        var dust = Dust.NewDustDirect(Player.Center, 0, 0, 68);
                        dust.noGravity = true;
                        dust.velocity *= 2f;
                        dust.fadeIn = 1f;
                    }
                }
                else if (Bulletcount == 12 && !bulletlimit) //at max charge, sound, particle, and text only once
                {
                    for (int i = 0; i < 20; i++)
                    {
                        var dust = Dust.NewDustDirect(Player.Center, 0, 0, 68);
                        dust.noGravity = true;
                        dust.velocity *= 3f;
                        dust.fadeIn = 1f;
                    }
                    SoundEngine.PlaySound(SoundID.Item8 with { Volume = 3f, Pitch = 0.4f }, Player.Center);

                    CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.Aqua, "Spear Enhanced!", false);
                    bulletlimit = true;
                }
                //batHit = true; //only show this effect once
            }
            if (proj.type == ModContent.ProjectileType<CursedSpearGunProj>() && bulletlimit && target.lifeMax > 5 && !target.friendly) //gets reset when it hits an enemy
            {
                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(target.Center, 0, 0, 68);
                    dust.noGravity = true;
                    dust.velocity *= 2f;
                    dust.fadeIn = 1f;
                }
                for (int i = 0; i < 20; i++)
                {
                    float speedY = -3f;
                    Vector2 perturbedSpeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                    var dust = Dust.NewDustDirect(target.Center, 0, 0, 68, perturbedSpeed.X, perturbedSpeed.Y);
                    dust.noGravity = true;
                    dust.velocity *= 2f;
                    dust.fadeIn = 1f;
                }
                SoundEngine.PlaySound(SoundID.Item74, target.Center);
                target.AddBuff(BuffID.OnFire3, 300);
                bulletlimit = false;
                Bulletcount = 0;
            }
            //Main.NewText(BatCount + " Whacks");
        }
    }
    public class CursedShotgunEffectsHit : GlobalNPC
    {
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            var player = Main.player[projectile.owner];
            if (player.GetModPlayer<CursedShotgunEffects>().bulletlimit == true && projectile.type == ModContent.ProjectileType<CursedSpearGunProj>()) //only increase damage for first hit
            {
                modifiers.FinalDamage *= 2.5f; //deal x2.5 damage with visual-only explosion effect
            }
        }
    }
}