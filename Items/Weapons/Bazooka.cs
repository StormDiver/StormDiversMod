using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Projectiles;
using static Terraria.ModLoader.ModContent;


namespace StormDiversMod.Items.Weapons
{
  
    public class Bazooka : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scavenger's Bazooka");
            //Tooltip.SetDefault("//Hold down left click to load up to 6 missiles, release to fire them in a rapid barrage
            //Each fired missile in a barrage does slightly more damage than the last
            //Missiles have a slight spread
            //Uses rockets as ammo
            //'Crudely made, but effective enough'
            //");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            //Item.useAmmo = ItemType<Ammo.BazookaRocket>();
            Item.useAmmo = AmmoID.Rocket;
            Item.shoot = ModContent.ProjectileType<Projectiles.BazookaProj>();
            Item.damage = 90;

            //each fired missile in a barrage gains an extra 6% bonus damage over the last
            //130 every 0.334 seconds (~390dps) + 6%   (~413)
            //260 every 0.667 seconds (~390dps) + 9%   (~425)
            //390 every 1     seconds (~390dps) + 12%  (~436)
            //520 every 1.334 seconds (~390dps) + 15%  (~448)
            //650 every 1.667 seconds (~390dps) + 18%  (~460)
            //780 every 2     seconds (~390dps) + 21%  (~471)


            //OLD=
            //10 + (15 x (rockets)) + (5 x rockets) = total shoot time, divide by 60 for seconds, damage = rockets x 140, divide total damage by total shoot time in seconds, (with rocket Is)
            //140 every 0.5   seconds ( 280dps)
            //280 every 0.833 seconds (~336dps)
            //420 every 1.166 seconds (~360dps)
            //560 every 1.5   seconds (~373dps)
            //700 every 1.833 seconds (~381dps)
            //840 every 2.166 seconds (~387dps)

            Item.DamageType = DamageClass.Ranged;

            Item.width = 30;
            Item.height = 20;
            Item.reuseDelay = 0;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.knockBack = 6f;
            Item.useTurn = false;
            //Item.shoot = ModContent.ProjectileType<Projectiles.BazookaProj>();
            Item.shootSpeed = 35f;
            //Item.UseSound = SoundID.Item12;
            Item.noMelee = true;
            //Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-34, -5);
        }
        int ammotime;
        public override void HoldItem(Player player)
        {
            if (player.channel)
                ammotime++;
            else
                ammotime = 0;
            base.HoldItem(player);
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (ammotime <= 1) //don't consume ammo when first fired, let projectile do it
                return false;
            else
                return true;
            //return Main.rand.NextFloat() >= 0f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[Item.shoot] < 1)
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.BazookaProj>(), damage, knockback, player.whoAmI);
            return false;
        }
       
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
           
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ItemID.RocketLauncher, 1)
             .AddIngredient(ItemID.IllegalGunParts, 1)
             .AddRecipeGroup(RecipeGroupID.IronBar, 25)
             //.AddIngredient(ItemID.TrashCan, 1)
            .AddIngredient(ItemID.ExplosivePowder, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
   

}