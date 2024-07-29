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
using StormDiversMod.Basefiles;
using System.Collections.Generic;
using Terraria.GameContent;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Items.Vanitysets;

namespace StormDiversMod.Items.Weapons
{
    public class PainStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Staff of Judgment");
            //Tooltip.SetDefault("Summons multiple Judgment skulls in various patterns that skeek out enemies\nRight click while holding to cycle damage types");
            Item.staff[Item.type] = true;

            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<PainSword>();
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 200;

            Item.DamageType = DamageClass.Generic;
            Item.width = 40;
            Item.height = 50;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 65, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item42;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.scale = 1f;
            Item.knockBack = 20;
            Item.shoot = ModContent.ProjectileType<PainStaffProj>();
            Item.shootSpeed = 30f;
            Item.noMelee = true;
            Item.crit = 11;
        }
        public override bool WeaponPrefix()
        {
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(1) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 72, 0f, 0f, 100, default, 1.5f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        float posY;
        int damagetype = 0;
        string classtext = "Generic Pain";
        public override bool ConsumeItem(Player player) => false;

        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player) //Right click in inventory
        {
            damagetype++;

            if (damagetype > 4)
                damagetype = 0;
            if (!Main.autoPause)
            {
                //CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.DeepPink, classtext, true);
            }
            SoundEngine.PlaySound(SoundID.Item73, player.Center);
            for (int i = 0; i < 30; i++) //Pink particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -10f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(player.Center, 0, 0, 72, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 1.5f;

            }
            //Main.NewText("pls: " + dummytype, 175, 17, 96);
        }
        public override void UpdateInventory(Player player) //cycle damage types
        {
            if (damagetype == 1)
            {
                Item.DamageType = DamageClass.Melee;
                classtext = "Melee Pain";
            }
            else if (damagetype == 2)
            {
                Item.DamageType = DamageClass.Ranged;
                classtext = "Ranged Pain";
            }
            else if (damagetype == 3)
            {
                Item.DamageType = DamageClass.Magic;
                classtext = "Magic Pain";
            }
            else if (damagetype == 4)
            {
                Item.DamageType = DamageClass.Summon;
                classtext = "Summoner Pain";
            }
            else
            {
                Item.DamageType = DamageClass.Generic;
                classtext = "Generic Pain";
            }
            base.UpdateInventory(player);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (!StormWorld.ultimateBossDown)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "You haven't experienced pain yet, why do you have this?";
                    }
                }

                if (line.Mod == "Terraria" && line.Name == "ItemName")
                {
                    line.Text = line.Text + " (" + classtext + ")";
                }
            }
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) //swing
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.ArmorPenetration = 0;
                Item.noMelee = false;
                Item.UseSound = SoundID.DD2_MonkStaffSwing;
            }
            else //shoot
            {
                //aura = 0;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.ArmorPenetration = 0;
                Item.noMelee = true;
                Item.UseSound = SoundID.Item42;
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2) //Right Click
            {
                //Main.NewText("Hello? " + damagetype, 0, 204, 170); //Inital Scale

                /*damagetype++;

                if (damagetype > 4)
                    damagetype = 0;*/

                /*if (damagetype == 1)
                {
                    Item.DamageType = DamageClass.Melee;
                    classtext = "Melee Pain";
                }
                else if (damagetype == 2)
                {
                    Item.DamageType = DamageClass.Ranged;
                    classtext = "Ranged Pain";
                }
                else if (damagetype == 3)
                {
                    Item.DamageType = DamageClass.Magic;
                    classtext = "Magic Pain";
                }
                else if (damagetype == 4)
                {
                    Item.DamageType = DamageClass.Summon;
                    classtext = "Summoner Pain";
                }
                else
                {
                    Item.DamageType = DamageClass.Generic;
                    classtext = "Generic Pain";
                }*/

                /*CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.DeepPink, classtext, true);
                SoundEngine.PlaySound(SoundID.Item73, player.Center);
                for (int i = 0; i < 30; i++) //Pink particles
                {
                    Vector2 perturbedSpeed = new Vector2(0, -10f).RotatedByRandom(MathHelper.ToRadians(360));

                    var dust = Dust.NewDustDirect(player.Center, 0, 0, 72, perturbedSpeed.X, perturbedSpeed.Y);
                    dust.noGravity = true;
                    dust.scale = 1.5f;

                }*/
            }
            else //left Click
            {
                if (!GetInstance<ConfigurationsIndividual>().NoShake)
                {
                    player.GetModPlayer<MiscFeatures>().screenshaker = true;
                }
                int shoottype = Main.rand.Next(0, 4);
                int numberProjectiles = 3 + Main.rand.Next(3);
                //SoundEngine.PlaySound(SoundID.ScaryScream with{ Volume = 0.5f, Pitch = 0.5f, PitchVariance = 0.1f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew}, player.Center);

                for (int index = 0; index < numberProjectiles; ++index)
                {
                    if (shoottype == 0)
                    {
                        Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(150) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                        vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-600, 600); //Spawn Spread
                        vector2_1.Y -= (float)(70 * index);
                        float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                        float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                        if ((double)num13 < 0.0) num13 *= -1f;
                        if ((double)num13 < 20.0) num13 = 20f;
                        float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                        float num15 = Item.shootSpeed / num14;
                        float num16 = num12 * num15;
                        float num17 = num13 * num15;
                        float SpeedX = num16 + (float)Main.rand.Next(-10, 10) * 0.05f;  //this defines the projectile X position speed and randomnes
                        float SpeedY = num17 + (float)Main.rand.Next(-10, 10) * 0.05f;  //this defines the projectile Y position speed and randomnes
                        int projID = Projectile.NewProjectile(source, new Vector2(vector2_1.X, vector2_1.Y), new Vector2(SpeedX, SpeedY), type, (int)(damage), 0.5f, player.whoAmI, 0.0f, (float)Main.rand.Next(5));
                        Main.projectile[projID].DamageType = Item.DamageType;

                    }
                    if (shoottype == 1)
                    {
                        float posX = position.X + Main.rand.NextFloat(100f, -100f);
                       
                        posY = position.Y + Main.rand.NextFloat(10f, -100f) * player.gravDir;
                        
                        int projID = Projectile.NewProjectile(source, new Vector2(posX, posY), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                        Main.projectile[projID].DamageType = Item.DamageType;

                    }
                    if (shoottype == 2)
                    {
                        Vector2 perturbedSpeed2 = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(360)); // 
                        int projID = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(perturbedSpeed2.X * 0.5f, perturbedSpeed2.Y * 0.5f), type, damage, knockback, player.whoAmI);
                        Main.projectile[projID].DamageType = Item.DamageType;

                    }
                    if (shoottype == 3)
                    {
                        double deg = Main.rand.Next(0, 360); //The degrees
                        double rad = deg * (Math.PI / 180); //Convert degrees to radians
                        double dist = 500; //Distance away from the cursor

                        position.X = Main.MouseWorld.X - (int)(Math.Cos(rad) * dist);
                        position.Y = Main.MouseWorld.Y - (int)(Math.Sin(rad) * dist);
                        float projspeed = 20;
                        Vector2 projvelocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(position.X, position.Y)) * projspeed;
                        int projID = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(projvelocity.X, projvelocity.Y), type, damage, knockback, player.whoAmI, 1, 0);
                        Main.projectile[projID].tileCollide = false;
                        Main.projectile[projID].DamageType = Item.DamageType;
                    }
                }
            }
            return false;
        }
      
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void PostUpdate()
        {
            // Spawn some light and dust when dropped in the world
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.4f);
            }
            if (Item.timeSinceItemSpawned % 12 == 0)
            {
                Vector2 center = Item.Center + new Vector2(0f, Item.height * -0.1f);

                // This creates a randomly rotated vector of length 1, which gets it's components multiplied by the parameters
                Vector2 direction = Main.rand.NextVector2CircularEdge(Item.width * 0.6f, Item.height * 0.6f);
                float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

                Dust dust = Dust.NewDustPerfect(center + direction * distance, DustID.GoldFlame, velocity);
                dust.scale = 0.5f;
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.alpha = 0;
            }
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            // Draw the periodic glow effect behind the item when dropped in the world (hence PreDrawInWorld)
            Texture2D texture = TextureAssets.Item[Item.type].Value;

            Rectangle frame;

            if (Main.itemAnimations[Item.type] != null)
            {
                // In case this item is animated, this picks the correct frame
                frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
            }
            else
            {
                frame = texture.Frame();
            }

            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new Vector2(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
            Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;

            for (float i = 0f; i < 1f; i += 0.25f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(90, 70, 255, 50), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(140, 120, 255, 77), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }
    //____________________________________________________________________________
    public class PainSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Pain Giver");
            //Tooltip.SetDefault("You went through unimaginable pain to get this, now you can inflict that pain upon your enemies");
            Item.staff[Item.type] = true;

            Item.ResearchUnlockCount = 1;

            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<PainStaff>();
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 200;

            Item.DamageType = DamageClass.Generic;
            Item.width = 40;
            Item.height = 50;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;  
            Item.value = Item.sellPrice(0, 65, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.scale = 1f;
            Item.knockBack = 20;
            Item.shoot = ModContent.ProjectileType<PainProj>();
            Item.shootSpeed = 30f;
            Item.noMelee = true;
            Item.crit = 11;

        }
        public override bool WeaponPrefix()
        {
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(1) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 72, 0f, 0f, 100, default, 1.5f);
                Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dustIndex].noGravity = true;
            }
        }
        float posY;
        int damagetype = 0;
        string classtext = "Generic Pain";
        public override bool ConsumeItem(Player player) => false;

        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player) //Right click in inventory
        {
            damagetype++;

            if (damagetype > 4)
                damagetype = 0;
            if (!Main.autoPause)
            {
                //CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.DeepPink, classtext, true);
            }
            SoundEngine.PlaySound(SoundID.Item73, player.Center);
            for (int i = 0; i < 30; i++) //Pink particles
            {
                Vector2 perturbedSpeed = new Vector2(0, -10f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(player.Center, 0, 0, 72, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 1.5f;

            }
            //Main.NewText("pls: " + dummytype, 175, 17, 96);
        }
        public override void UpdateInventory(Player player) //cycle damage types
        {
            if (damagetype == 1)
            {
                Item.DamageType = DamageClass.Melee;
                classtext = "Melee Pain";
            }
            else if (damagetype == 2)
            {
                Item.DamageType = DamageClass.Ranged;
                classtext = "Ranged Pain";
            }
            else if (damagetype == 3)
            {
                Item.DamageType = DamageClass.Magic;
                classtext = "Magic Pain";
            }
            else if (damagetype == 4)
            {
                Item.DamageType = DamageClass.Summon;
                classtext = "Summoner Pain";
            }
            else
            {
                Item.DamageType = DamageClass.Generic;
                classtext = "Generic Pain";
            }
            base.UpdateInventory(player);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (!StormWorld.ultimateBossDown)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "You haven't experienced pain yet, why do you have this?";
                    }
                }

                if (line.Mod == "Terraria" && line.Name == "ItemName")
                {
                    line.Text = line.Text + " (" + classtext + ")";
                }
            }
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) //swing
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.ArmorPenetration = 0;
                Item.noMelee = false;
                Item.UseSound = SoundID.DD2_MonkStaffSwing;
            }
            else //shoot
            {
                //aura = 0;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.ArmorPenetration = 0;
                Item.noMelee = true;
                Item.UseSound = SoundID.Item1;
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2) //Right Click
            {
               
            }
            else //left Click
            {
                if (!GetInstance<ConfigurationsIndividual>().NoShake)
                {
                    player.GetModPlayer<MiscFeatures>().screenshaker = true;
                }
                int shoottype = Main.rand.Next(0, 4);
                int numberProjectiles = 3 + Main.rand.Next(3);
                //SoundEngine.PlaySound(SoundID.ScaryScream with{ Volume = 0.5f, Pitch = 0.5f, PitchVariance = 0.1f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew}, player.Center);
                if (!GetInstance<ConfigurationsIndividual>().NoPain)
                {
                    if (player.Male)
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSound") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, player.Center);
                    else
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSoundFemale") with { Volume = 1.5f, MaxInstances = 12, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, player.Center);
                }

                for (int index = 0; index < numberProjectiles; ++index)
                {
                    if (shoottype == 0)
                    {
                        Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(150) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                        vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-600, 600); //Spawn Spread
                        vector2_1.Y -= (float)(70 * index);
                        float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                        float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                        if ((double)num13 < 0.0) num13 *= -1f;
                        if ((double)num13 < 20.0) num13 = 20f;
                        float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                        float num15 = Item.shootSpeed / num14;
                        float num16 = num12 * num15;
                        float num17 = num13 * num15;
                        float SpeedX = num16 + (float)Main.rand.Next(-10, 10) * 0.05f;  //this defines the projectile X position speed and randomnes
                        float SpeedY = num17 + (float)Main.rand.Next(-10, 10) * 0.05f;  //this defines the projectile Y position speed and randomnes
                        int projID = Projectile.NewProjectile(source, new Vector2(vector2_1.X, vector2_1.Y), new Vector2(SpeedX, SpeedY), type, (int)(damage), 0.5f, player.whoAmI, 0.0f, (float)Main.rand.Next(5));
                        Main.projectile[projID].DamageType = Item.DamageType;

                    }
                    if (shoottype == 1)
                    {
                        float posX = position.X + Main.rand.NextFloat(100f, -100f);
                        
                        posY = position.Y + Main.rand.NextFloat(10f, -100f) * player.gravDir;
                       
                        int projID = Projectile.NewProjectile(source, new Vector2(posX, posY), new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                        Main.projectile[projID].DamageType = Item.DamageType;

                    }
                    if (shoottype == 2)
                    {
                        Vector2 perturbedSpeed2 = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(360)); // 
                        int projID = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(perturbedSpeed2.X * 0.5f, perturbedSpeed2.Y * 0.5f), type, damage, knockback, player.whoAmI);
                        Main.projectile[projID].DamageType = Item.DamageType;

                    }
                    if (shoottype == 3)
                    {
                        double deg = Main.rand.Next(0, 360); //The degrees
                        double rad = deg * (Math.PI / 180); //Convert degrees to radians
                        double dist = 500; //Distance away from the cursor


                        position.X = Main.MouseWorld.X - (int)(Math.Cos(rad) * dist);
                        position.Y = Main.MouseWorld.Y - (int)(Math.Sin(rad) * dist);
                        float projspeed = 20;
                        Vector2 projvelocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(position.X, position.Y)) * projspeed;
                        int projID = Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(projvelocity.X, projvelocity.Y), type, damage, knockback, player.whoAmI, 1, 0);
                        Main.projectile[projID].tileCollide = false;
                        Main.projectile[projID].DamageType = Item.DamageType;

                    }
                }
            }
            return false;
        }
        /*public override void AddRecipes()
        {
         CreateRecipe()
         .AddIngredient(ModContent.ItemType<Items.Vanitysets.ThePainMask>(), 50)
         .AddIngredient(ItemID.LunarOre, 1)
         .AddTile(TileID.LunarCraftingStation)
         .Register();
        }*/
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void PostUpdate()
        {
            // Spawn some light and dust when dropped in the world
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.4f);
            }
            if (Item.timeSinceItemSpawned % 12 == 0)
            {
                Vector2 center = Item.Center + new Vector2(0f, Item.height * -0.1f);

                // This creates a randomly rotated vector of length 1, which gets it's components multiplied by the parameters
                Vector2 direction = Main.rand.NextVector2CircularEdge(Item.width * 0.6f, Item.height * 0.6f);
                float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

                Dust dust = Dust.NewDustPerfect(center + direction * distance, DustID.GoldFlame, velocity);
                dust.scale = 0.5f;
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.alpha = 0;
            }
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            // Draw the periodic glow effect behind the item when dropped in the world (hence PreDrawInWorld)
            Texture2D texture = TextureAssets.Item[Item.type].Value;

            Rectangle frame;

            if (Main.itemAnimations[Item.type] != null)
            {
                // In case this item is animated, this picks the correct frame
                frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
            }
            else
            {
                frame = texture.Frame();
            }

            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new Vector2(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
            Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;

            for (float i = 0f; i < 1f; i += 0.25f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(90, 70, 255, 50), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(140, 120, 255, 77), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }
}