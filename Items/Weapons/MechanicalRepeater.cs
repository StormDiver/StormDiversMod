using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
    public class MechanicalRepeater : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Repeater");
            Tooltip.SetDefault("Rapidly fires arrows in bursts of three\nOnly the first shot consumes ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            Item.reuseDelay = 19;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;

            //Item.UseSound = SoundID.Item5;

            Item.damage = 32;
            //Item.crit = 4;
            Item.knockBack = 2f;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            //Item.shoot = ProjectileID.GrenadeI;
            Item.shootSpeed = 10f;

            Item.useAmmo = AmmoID.Arrow;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
          
            SoundEngine.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 5);
            return true;
        }
        public override bool CanConsumeAmmo(Player player)
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
}