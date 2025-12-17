using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Common;
using StormDiversMod.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod.Items.Weapons
{
	public class RoseSickle : ModItem
	{
		public override void SetStaticDefaults() 
		{
            //DisplayName.SetDefault("The Crescent Rifle"); 
            //Left click to swing the scythe creating a large damaging aura that has a chance to destroy projectiles\nRight click to fire the rifle, has a strong recoil when not grounded and requires bullets as ammo
            //\n'A High Caliber Sniper Scythe, turn your enemies into dust''");
            Item.ResearchUnlockCount = 1;
            Item.staff[Item.type] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        int aura = 0;
		public override void SetDefaults() 
		{
			Item.damage = 80;
            Item.DamageType = DamageClass.Melee;
            Item.width = 75;
			Item.height = 75;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Shoot;  
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
			//Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 6;
            Item.shoot = ProjectileID.Bullet;
            aura = ModContent.ProjectileType<Projectiles.RoseAura>();
            Item.shootSpeed = 15;
            Item.scale = 1f;
            Item.noMelee = true;
            Item.ArmorPenetration = 15;
            //Item.shootsEveryUse = true;
        }
        public override void UpdateInventory(Player player)
        {
            if (!player.GetModPlayer<RoseSickleEffects>().hitlimit && player.GetModPlayer<RoseSickleEffects>().Hitcount == 1)
                Item.SetNameOverride("The Crescent Rifle - " + player.GetModPlayer<RoseSickleEffects>().Hitcount + " hit");
            else if (!player.GetModPlayer<RoseSickleEffects>().hitlimit && player.GetModPlayer<RoseSickleEffects>().Hitcount != 1)
                Item.SetNameOverride("The Crescent Rifle - " + player.GetModPlayer<RoseSickleEffects>().Hitcount + " hits");
            else
                Item.SetNameOverride("The Crescent Rifle - " + "READY!");
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {

            if (player.altFunctionUse == 2) //gun
            {
                //Item.DamageType = DamageClass.Ranged;
                aura = 0;
                Item.useAmmo = AmmoID.Bullet;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.reuseDelay = 12;
            }
            else //scythe
            {
                //Item.DamageType = DamageClass.Melee;
                Item.useStyle = ItemUseStyleID.Swing;
                aura = ModContent.ProjectileType<Projectiles.RoseAura>();
                Item.useAmmo = 0;
                Item.reuseDelay = 0;
            }
            return true;
        }
        public override void HoldItem(Player player)
        {
            if (player.GetModPlayer<RoseSickleEffects>().hitlimit == true)
            {
                if (Main.rand.Next(4) < 1)
                {
                    int dust = Dust.NewDust(player.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 12, 0, -4 * player.gravDir, 50, default, 1.25f);
                    Main.dust[dust].noGravity = true;
                }
            }
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
                    Projectile spawnedProj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.MountedCenter - velocity * 2, velocity * 5, aura, damage, Item.knockBack, Main.myPlayer,
                            Math.Sign(mousePosition.X - player.Center.X) * player.gravDir, player.itemAnimationMax, player.GetAdjustedItemScale(Item));
                    NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
                }
                return;
            }
        }
        Vector2 muzzleOffset = new Vector2(0 , 0);
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.altFunctionUse == 2)
                player.itemLocation += muzzleOffset * -1f;
        }
        int chargedshot;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);

            if (player.altFunctionUse == 2) //Right Click
            {
                if (player.GetModPlayer<RoseSickleEffects>().hitlimit == true)
                    chargedshot = 1;
                else
                    chargedshot = 0;

                if (chargedshot == 1)
                    player.GetModPlayer<MiscFeatures>().screenshaker = true; //very powerful >:)

                muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 40;
                if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                {
                    position += muzzleOffset;
                }
                if (type == ProjectileID.Bullet || chargedshot == 1)
                {
                    type = ModContent.ProjectileType<Projectiles.RoseSickleBulletProj>();
                }
                for (int i = 0; i < 1; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0)); 
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage * 2, knockback * 1.5f, player.whoAmI, 0, 0, chargedshot);
                    if (player.velocity.Y != 0 && !player.controlUp && !player.controlDown)
                    {
                        if (chargedshot == 1)
                        {
                            player.velocity.X = velocity.X * -0.75f;
                            player.velocity.Y = velocity.Y * -0.75f;
                        }
                        else
                        {
                            player.velocity.X = velocity.X * -0.5f;
                            player.velocity.Y = velocity.Y * -0.5f;
                        }
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    int dust2 = Dust.NewDust(position + muzzleOffset * 1f, 0, 0, 12, velocity.X * 0.12f, velocity.Y * 0.12f, 0);
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].scale = 1.25f;
                }
                SoundEngine.PlaySound(SoundID.Item41 with { Volume = 1f, Pitch = -0.25f }, player.Center);
                //reset charged attack
                if (chargedshot == 1)
                {
                    player.GetModPlayer<RoseSickleEffects>().Hitcount = 0;
                    player.GetModPlayer<RoseSickleEffects>().hitlimit = false;
                }
            }
            else //left Click
            {
                SoundEngine.PlaySound(SoundID.Item71, player.Center);
            }
            return false;
        }
        public override void AddRecipes()
        {
            /*CreateRecipe()
            .AddIngredient(ItemID.DeathSickle, 1)
            .AddIngredient(ItemID.SniperRifle, 1)
            .AddIngredient(ItemID.SpectreBar, 10)
           .AddTile(TileID.MythrilAnvil)
           .Register();*/
        }
    }
    //_______________________________________________________________________________________________________________
    public class RoseSickleEffects : ModPlayer
    {
        public int Hitcount; //charge count
        public bool hitlimit;//for one time indication that limit is reached
        bool hashit; //has the scythe already hit, makes only one charge added per swing
        public override void ResetEffects()
        {
        }
        public override void UpdateDead()//Reset all ints and bools if dead======================
        {
            Hitcount = 0;
            hitlimit = false;
            hashit = false;
        }
        public override void PreUpdate()
        {
            if (Player.itemAnimation == Player.itemAnimationMax && Player.HeldItem.type == ModContent.ItemType<RoseSickle>()) //resets bool to prevent more than one charge per swing
            {
                hashit = false;
            }
            //Main.NewText("Has been hit: " + hashit);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.type == ModContent.ProjectileType<RoseAura>() && target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy && !hashit) //left click, add once charge per swing
            {
                if (Hitcount < 10) //Count up each hit (have to have separate or text counter includes 10 and only shows charge text on 11
                    Hitcount++;

                if (Hitcount < 10) //count up, play sound and display count text
                {
                    CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.IndianRed, Hitcount, false);
                    SoundEngine.PlaySound(SoundID.Grass with { Volume = 2f, Pitch =0 }, Player.Center);
                    for (int i = 0; i < 15; i++)
                    {
                        var dust = Dust.NewDustDirect(Player.Center, 0, 0, 12);
                        dust.scale = 1.25f;
                        dust.velocity *= 2f;
                        dust.noGravity = true;
                    }
                }
                else if (Hitcount == 10 && !hitlimit) //at max charge, sound, particle, and text only once
                {
                    SoundEngine.PlaySound(SoundID.Grass with { Volume = 3f, Pitch = -1 }, Player.Center);

                    CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.OrangeRed, "Rose Petals Ready!", false);
                    hitlimit = true;
                    for (int i = 0; i < 25; i++)
                    {
                        var dust = Dust.NewDustDirect(Player.Center, 0, 0, 12);
                        dust.scale = 1.25f;
                        dust.velocity *= 2f;
                    }
                }
                hashit = true; //only show this effect once
            }
            /*if (proj.type == ModContent.ProjectileType<RoseSickleBulletProj>() && Hitcount == 10 && target.lifeMax > 5 && !target.friendly) //gets reset when fired
            {
                hitlimit = false;
                Hitcount = 0;
            }*/
            //Main.NewText(BatCount + " Whacks");
        }
    }
}