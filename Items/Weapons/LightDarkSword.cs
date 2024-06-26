using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Projectiles;

namespace StormDiversMod.Items.Weapons
{
	public class LightDarkSword : ModItem
	{
		public override void SetStaticDefaults() 
		{
            //DisplayName.SetDefault("Equinox"); 
            //Tooltip.SetDefault("Left Click to fire out an essence of light that travels towards enemies at high speed\nRight click to fire out an essence of dark that surrounds enemies in darkness\n'Perfectly balanced, as all things should be
            //\nBoth swings creates a large damaging aura'");
            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
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
			//Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 6;
            Item.shoot = ModContent.ProjectileType <Projectiles.SwordLightProj>();
            aura = ModContent.ProjectileType<Projectiles.LightDarkAuraLight>();
            Item.shootSpeed = 16f;
            Item.scale = 1f;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            Item.noMelee = true;
            //Item.shootsEveryUse = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                aura = ModContent.ProjectileType<Projectiles.LightDarkAuraDark>();
            }
            else
            {
                aura = ModContent.ProjectileType<Projectiles.LightDarkAuraLight>();
            }
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


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);


            if (player.altFunctionUse == 2) //Right Click
            {
                //type = ModContent.ProjectileType<Projectiles.SwordDarkProj>();
                /*for (int i = 0; i < 1; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(6)); 
                    Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X * 0.5f, perturbedSpeed.Y * 0.5f), ModContent.ProjectileType<Projectiles.SwordDarkProj>(), damage, knockback, player.whoAmI);
                }*/
                SoundEngine.PlaySound(SoundID.Item1, player.Center);

            }
            else //left Click
            {
                //type = ModContent.ProjectileType<Projectiles.SwordLightProj>();

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(2)); 
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X * 1.75f, perturbedSpeed.Y * 1.75f), ModContent.ProjectileType<Projectiles.SwordLightProj>(), (int)(damage * 1.25f), knockback, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item9 with { Volume = 1f, Pitch = -0.5f }, player.Center);
                SoundEngine.PlaySound(SoundID.Item1, player.Center);

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
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LightDarkSword_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
  
}