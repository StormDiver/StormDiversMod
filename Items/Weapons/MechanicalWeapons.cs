using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using static Humanizer.In;
using static Terraria.GameContent.Animations.Actions.NPCs;
using Humanizer;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace StormDiversMod.Items.Weapons
{
    public class MechanicalRepeater : ModItem
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mechanical Repeater");
            //Tooltip.SetDefault("Rapidly fires arrows in bursts of three\nOnly the first shot consumes ammo");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 45;
            Item.height = 24;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 6;
            Item.useAnimation = 15;
            Item.reuseDelay = 15;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            //Item.UseSound = SoundID.Item5;

            Item.damage = 40;
            //Item.crit = 4;
            Item.knockBack = 2f;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            //Item.shoot = ProjectileID.GrenadeI;
            Item.shootSpeed = 15f;

            Item.useAmmo = AmmoID.Arrow;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
          
            SoundEngine.PlaySound(SoundID.Item5, position);
            return true;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            // Because of how the game works, player.itemAnimation will be 11, 7, and finally 3. (UseAmination - 1, then - useTime until less than 0.) 
            // We can get the Clockwork Assault Riffle Effect by not consuming ammo when itemAnimation is lower than the first shot.
            return !(player.itemAnimation < Item.useAnimation - 2);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
       
    }
    //____________________________________________________________________________________
    public class MechanicalRifle : ModItem
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Super Charged Rifle");
            //Tooltip.SetDefault("Can be charged up by holding down left click
            //Fully charged fires a more powerful bullet that deals double knockback and triple damage
            //Converts musket balls to high velocity bullets
            //When fully charged it coverts them to a special variant that inflicts confused and has no damage fall off
            //Right Click to zoom out
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 22;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.reuseDelay = 20;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.channel = true;

            Item.DamageType = DamageClass.Ranged;

            //Item.UseSound = SoundID.Item5;

            Item.damage = 80;
            //Item.crit = 4;
            Item.knockBack = 3f;

            Item.shoot = ModContent.ProjectileType<Projectiles.MechanicalRifleProj>();
            //Item.shoot = ProjectileID.GrenadeI;
            Item.shootSpeed = 60f;

            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }
        public override void HoldItem(Player player)
        {
            player.scope = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[Item.shoot] < 1)
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.MechanicalRifleProj>(), damage, knockback, player.whoAmI);
            return false;
        }
    }
}