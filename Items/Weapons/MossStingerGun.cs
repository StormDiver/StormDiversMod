using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Common;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using StormDiversMod.Projectiles.Minions;
using Microsoft.CodeAnalysis;
using Humanizer;
using Microsoft.Build.Evaluation;

namespace StormDiversMod.Items.Weapons
{
    public class MossStingerGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Stinger Shooter"); 
            //Tooltip.SetDefault("Rapidly shoots out stingers
            //Stingers stick to enemies and inject a stacking debuff
            //The projectiles become less accurate the lower the remaining mana you have"); 
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
                Item.mana = 12;
            else
                Item.mana = 8;
            Item.UseSound = SoundID.Item17;
            Item.damage = 35;
            Item.crit = 8;
            Item.knockBack = 1f;
            Item.shoot = ModContent.ProjectileType<MossStingerProj>();
            Item.shootSpeed = 8;
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 4); 
        }
        float accuracy = 0; //The amount of spread
        public override void HoldItem(Player player)
        {
            //Main.NewText("" + accuracy, Color.Gold);
            accuracy = 20 - ((float)player.statMana / (float)player.statManaMax2 * 20); //0 at max mana, 12 at 0 mana
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 40;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(accuracy));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y + 8), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);

            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            //velocity = velocity.RotatedByRandom(MathHelper.ToRadians(accuracy));
        } 
    }
}