using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using StormDiversMod.Common;

namespace StormDiversMod.Items.Weapons
{
    public class VortexiaWeapon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vortexia");
            //Tooltip.SetDefault("Summons a damaging vortex that summons smaller homing vortexes\n'Relic of a lost era'");
            Item.staff[Item.type] = true;

            Item.ResearchUnlockCount = 1;

            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            //Item.channel = true;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item117;

            Item.damage = 55;
            //Item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<VortexiaProj>();
            
            Item.shootSpeed = 14f;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 22;
            }
            else
            {
                Item.mana = 14;
            }
            Item.noMelee = true; //Does the weapon itself inflict damage?
            
        }
      
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 55f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);
            for (int i = 0; i < 20; i++)
            {

                var dust = Dust.NewDustDirect((player.Center - new Vector2(20, 20)) + muzzleOffset, 40, 40, 229, velocity.X * 10, velocity.Y * 10, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0;
            }
            return false;

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        /*public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }*/
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/VortexiaWeapon_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    
}