using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace StormDiversMod.Items.Weapons
{
    public class LizardSpinner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lihzahrd Sawblade");
            Tooltip.SetDefault("Throw out spinning sawblades that linger in place\nLimit of 6 sawblades at a time, right click to remove all thrown sawblades");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {

            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 38;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2.5f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<Projectiles.LizardSpinnerProj>();
         
            Item.autoReuse = true;
            Item.noMelee = true;

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (!NPC.downedPlantBoss)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = line.Text + "\nYou shouldn't have this yet >:("; 
                    }
                }
            }
        }
        
        /*public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 10;

        }*/

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            SoundEngine.PlaySound(SoundID.Item1 with { Volume = 2f, Pitch = 0.25f }, position);
            int spinnyprojs = 0;
            int oldestProjIndex = -1;
            int oldestProjTimeLeft = 100000;
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<Projectiles.LizardSpinnerProj>())
                {
                    spinnyprojs++;
                    if (Main.projectile[i].timeLeft < oldestProjTimeLeft)
                    {
                        oldestProjIndex = i;
                        oldestProjTimeLeft = Main.projectile[i].timeLeft;
                    }
                }
            }
            if (spinnyprojs > 5)
            {
                Main.projectile[oldestProjIndex].timeLeft = 10;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarTabletFragment, 16)
            .AddTile(TileID.LihzahrdAltar)
            .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LizardSpinner_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //___________________________________________________________________________________

    public class LizardFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lihzahrd Flamer");
            Tooltip.SetDefault("Fires out a stream of super heated flames that ricohet off tiles and are unaffected by water\nUses gel for ammo\nIgnores 15 points of enemy defense");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (!NPC.downedPlantBoss)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = line.Text + "\nYou shouldn't have this yet >:("; 
                    }
                }
            }
        }
        public override void SetDefaults()
        {

            Item.width = 40;
            Item.height = 24;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 3;
            Item.useAnimation = 12;
        
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.UseSound = SoundID.Item34;

            Item.damage = 60;
            Item.knockBack = 0.5f;
            Item.shoot = ModContent.ProjectileType<Projectiles.LizardFlameProj>();

            Item.shootSpeed = 3f;

            Item.useAmmo = AmmoID.Gel;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(3, 0);
        }
        public override void HoldItem(Player player)
        {
        }
        bool candamage;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 40;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            if (candamage)
            {
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X + player.velocity.X / 10, velocity.Y + player.velocity.Y / 10), type, damage, knockback, player.whoAmI, 0, 1);
                candamage = false;
            }
            else
            {
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(velocity.X + player.velocity.X / 10, velocity.Y + player.velocity.Y / 10), type, damage, knockback, player.whoAmI, 0, 0);
                candamage = true;

            }
            /*if (!NPC.downedPlantBoss)
            {
                for (int i = 0; i < 10; i++)
                {

                    int dustIndex = Dust.NewDust(new Vector2(player.position.X + 1f, player.position.Y), player.width, player.height, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                }

                player.statLife = 1;
                player.statMana = 0;
                player.velocity.X = 0;
                player.statDefense = 0;
                player.endurance = 0;
                player.lifeRegen = 0;
                    Main.NewText("Defeat the Plantera you cheater >:(", 100, 100, 100);
                    Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 16);

               
                return false;
           
            }
            else
            {
                
                return true;
            }*/
            return false;
         }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {

            return !(player.itemAnimation < Item.useAnimation - 2);

        }
        public override void AddRecipes()
        {
           CreateRecipe()
           .AddIngredient(ItemID.LunarTabletFragment, 16)
           .AddTile(TileID.LihzahrdAltar)
           .Register();
            
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LizardFlame_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //___________________________________________________________________________________
    public class LizardSpell : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lihzahrd Flameburst Tome");
            Tooltip.SetDefault("Summons a burst of ricocheting fireballs");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (!NPC.downedPlantBoss)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\nYou shouldn't have this yet >:(";
                    }
                }
            }
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 18;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.UseSound = SoundID.Item20;

            Item.damage = 55;

            Item.knockBack = 4f;

            Item.shoot = ModContent.ProjectileType<Projectiles.LizardSpellProj>();
            //Item.shoot = ProjectileID.BallofFire;

            Item.shootSpeed = 10f;


            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 6, 0, 0, 120, default, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 2.5f;
            }
            int numberProjectiles = 2; //This defines how many projectiles to shot.
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(14));
                int projID = Projectile.NewProjectile(source, position, new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.LizardSpellProj>(), damage, knockback, player.whoAmI);

               // Main.projectile[projID].penetrate = 3;
                //Main.projectile[projID].usesLocalNPCImmunity = true;
                //Main.projectile[projID].localNPCHitCooldown = 10;
                //Main.projectile[projID].timeLeft = 300;
            }
            return true;



        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarTabletFragment, 16)
            .AddTile(TileID.LihzahrdAltar)
            .Register();

        }
        //Drop rate in NPCEffects/ Luantic Cultist treasure bag
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LizardSpell_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //___________________________________________________________________________________
    public class LizardMinion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lihzahrd Guardian Staff");
            Tooltip.SetDefault("Summons a mini Temple Guardian to fight for you");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            HeldItemLayer.RegisterData(Item.type, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Glow"),
                Color = () => new Color(255, 255, 255, 50) * 0.7f
            });
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.autoReuse = true;

            Item.damage = 85;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item43;


            Item.mana = 10;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;

            Item.UseSound = SoundID.Item45;

            Item.buffType = ModContent.BuffType<Projectiles.Minions.LizardMinionBuff>();
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.LizardMinionProj>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (!NPC.downedPlantBoss)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = line.Text + "\nYou shouldn't have this yet >:("; 
                    }
                }
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(8, 8);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            player.AddBuff(Item.buffType, 2);

            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);

            return false;



        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.LunarTabletFragment, 16)
           .AddTile(TileID.LihzahrdAltar)
           .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Weapons/LizardMinion_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}