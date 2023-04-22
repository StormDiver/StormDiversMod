using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Basefiles;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using StormDiversMod.Items.Vanitysets;

namespace StormDiversMod.Items.Weapons
{
    public class StormKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lightning Knife");
            //Tooltip.SetDefault("Summons a powerful lightning bolt onto struck enemies");

            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<StormLauncher>();

        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.knockBack = 2.5f;
            Item.shoot = ModContent.ProjectileType <Projectiles.StormKnifeProj>();
            Item.shootSpeed = 14f;
            Item.noMelee = true;
            Item.noUseGraphic = true;

        }
      
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
         
            return true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/StormKnife_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
        //Drop rate in NPCEffects/ Luantic Cultist treasure bag
    }
    //________________________________________________________________________________
    public class StormLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Overloaded Lightning Launcher");
            //Tooltip.SetDefault("Fires out overloaded grenades that explode into a lightning blast\nUses regular rockets as ammo");
            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<StormStaff>();

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (!GetInstance<ConfigurationsGlobal>().StormBossSkipsPlant)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = "Uses regular rockets as ammo, purchase more from the Demolitionist"; //If not set as a plantera alt you need to get rockets somehow
                    }
                }
            }
        }
        public override void SetDefaults()
        {

            Item.width = 40;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 33;
            Item.useAnimation = 33;
            //Item.reuseDelay = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ProjectileID.RocketI;
            //Item.useAmmo = ItemType<Ammo.ProtoGrenade>();
            Item.useAmmo = AmmoID.Rocket;
            
            Item.UseSound = SoundID.Item92;

            Item.damage = 34;
            //Item.crit = 4;
            Item.knockBack = 3f;
            Item.shootSpeed = 12f;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
        int aiTime = 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 35f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(3));
            if (type == ProjectileID.RocketI || type == ProjectileID.RocketII)//ai 0
            {
                aiTime = 0;
            }
            else if (type == ProjectileID.RocketIII || type == ProjectileID.RocketIV)//ai 1
            {
                aiTime = 1;
            }
            else if (type == ProjectileID.MiniNukeGrenadeI || type == ProjectileID.MiniNukeGrenadeII || type == ProjectileID.ClusterRocketI || type == ProjectileID.ClusterRocketII) //ai 2 (Doesn't work)               
            {
                aiTime = 2;
            }
            Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.StormGrenadeProj>(), damage, knockback, player.whoAmI, 0, aiTime);


            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/StormLauncher_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

    }
    //__________________________________________________________________________________________________________
    public class StormStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lightning Scepter");
            //Tooltip.SetDefault("Rapidly fires out piercing lightning bolts");
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<StormSentryStaff>();

        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 15;
            }
            else
            {
                Item.mana = 10;
            }
            //Item.UseSound = SoundID.Item20;

            Item.damage = 40;
            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.StormLightningProj>();

            Item.shootSpeed = 7f;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        /* public override Vector2? HoldoutOffset()
         {
             return new Vector2(5, 0);
         }*/
        
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            float numberProjectiles = 3;
            float rotation = MathHelper.ToRadians(3);

            SoundEngine.PlaySound(SoundID.NPCHit53 with {Volume = 0.5f, Pitch = -0.5f, MaxInstances = 0}, player.Center);

            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 35f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            for (int j = 0; j < numberProjectiles; j++)
            {
               

                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles - 1)));

                float ai = Main.rand.Next(100);
                int projID = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y),
                    ModContent.ProjectileType<Projectiles.StormLightningProj>(), damage, .5f, player.whoAmI, perturbedSpeed.ToRotation(), ai);

                Main.projectile[projID].DamageType = DamageClass.Magic;
            }
          

            for (int i = 0; i < 10; i++)
            {
                float speedY = -2f;

                Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                int dust2 = Dust.NewDust(player.Center + muzzleOffset * 1.6f, 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 1.5f);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].scale = 1f;

            }


            return false;

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/StormStaff_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

    }
    public class StormSentryStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Overloaded Staff");
            //Tooltip.SetDefault("Summons a Scandrone sentry that attacks enemies for you\nRight click to target a specific enemy");
            Item.staff[Item.type] = true;
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;


            Item.ResearchUnlockCount = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<StormKnife>();

        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 10;
            //Item.UseSound = SoundID.Item122;

            Item.damage = 40;

            Item.knockBack = 1f;

            Item.shoot = ModContent.ProjectileType<Projectiles.SentryProjs.StormSentryProj>();

            Item.shootSpeed = 1f;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
       
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 0.5f, Pitch= -0.5f, MaxInstances = 0 }, player.Center);

            int index = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(0, 0), type, damage, knockback, player.whoAmI);
            Main.projectile[index].originalDamage = Item.damage;


            return false;


        }
       
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/StormSentryStaff_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}