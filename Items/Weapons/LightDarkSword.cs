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

namespace StormDiversMod.Items.Weapons
{
	public class LightDarkSword : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 44));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            //DisplayName.SetDefault("Equinox"); 
            //Tooltip.SetDefault("Left Click to fire out an essence of light that travels towards enemies at high speed\nRight click to fire out an essence of dark that surrounds enemies in darkness\n'Perfectly balanced, as all things should be
            //\nBoth swings creates a large damaging aura'");
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
        
		public override void SetDefaults() 
		{
			Item.damage = 50;

            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
			Item.height = 50;
			Item.useTime = 20;
			Item.useAnimation = 20;

            Item.useStyle = ItemUseStyleID.Swing; 
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
			//Item.UseSound = SoundID.DD2_MonkStaffSwing;
			Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 6;
            Item.shoot = ModContent.ProjectileType <Projectiles.SwordLightProj>();
            aura = ModContent.ProjectileType<Projectiles.LightDarkAuraLight>();
            Item.shootSpeed = 16f;
            Item.scale = 1f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            //Item.shootsEveryUse = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            base.UseStyle(player, heldItemFrame);
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                aura = ModContent.ProjectileType<Projectiles.LightDarkAuraDark>();
            else
                aura = ModContent.ProjectileType<Projectiles.LightDarkAuraLight>();
            return true;
        }

        int dusttype;
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (player.altFunctionUse == 2)
            {
                dusttype = 54;
            }
            else
            {
                dusttype = 66;
            }
            if (Main.rand.Next(4) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dusttype, 0f, 0f, 100, default, 1.5f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
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
        //double degrees;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            if (player.altFunctionUse == 2) //Right Click
            {
                SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash with { Volume = 1.5f, Pitch = 0f, MaxInstances = -1 }, player.Center);
                //blade sprite
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.SwordLightDarkBlade>(), damage, Item.knockBack, Main.myPlayer,
                                          1 * player.direction, player.itemAnimationMax, 1); //0: direction, 1: animation time, 2: light or dark
            }
            else //left Click
            {
                //type = ModContent.ProjectileType<Projectiles.SwordLightProj>();
                /*if (player.direction == -1)
                degrees = Main.rand.Next(90, 171); //The degrees
                else
                degrees = Main.rand.Next(10, 91); //The degrees

                double rad = degrees * (Math.PI / 180); //Convert degrees to radians
                double dist = Main.rand.Next(100, 100); //Distance away from the player

                position.X = player.Center.X - (int)(Math.Cos(rad) * dist);
                position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist);
                //Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0)); 
                Vector2 velocitynew = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(player.Center.X, player.Center.Y)) * Item.shootSpeed;

                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocitynew.X, velocitynew.Y), ModContent.ProjectileType<Projectiles.SwordLightProj>(), (int)(damage * 1.25f), knockback, player.whoAmI);*/

                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.SwordLightProjspin>(), (int)(damage * 1.25f), knockback, player.whoAmI);

                SoundEngine.PlaySound(SoundID.Item9 with { Volume = 1f, Pitch = 0 }, player.Center);
                SoundEngine.PlaySound(SoundID.Item1, player.Center);

                //blade sprite
               Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.SwordLightDarkBlade>(), damage, Item.knockBack, Main.myPlayer,
                                            1 * player.direction, player.itemAnimationMax, 0); //0: direction, 1: animation time, 2: light or dark
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LightShard, 1)
            .AddIngredient(ItemID.DarkShard, 1)
            .AddIngredient(ItemID.SoulofLight, 8)
            .AddIngredient(ItemID.SoulofNight, 8)
           .AddRecipeGroup("StormDiversMod:HighHMBars", 15)
           .AddTile(TileID.MythrilAnvil)
           .Register();
           
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        /*public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LightDarkSword_Glow");

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Item.height * 0.5f);
            Rectangle rectangle = texture.Frame(1, 44);

            Main.EntitySpriteDraw(texture, Item.Center - Main.screenPosition, new Rectangle(0, Main.itemFrame[44] * (texture.Height / 44), texture.Width, texture.Height / Main.projFrames[Projectile.type]),
                Color.White, 0, drawOrigin, Item.scale, Item.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }*/
    }
  
}