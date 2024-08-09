using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using StormDiversMod.Basefiles;
using System;
using Newtonsoft.Json;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class ContestArmourBHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Cryogenic Mask");
            //Tooltip.SetDefault("10% increased magic and summon damage
            //Increases maximum mana by 40
              //'Cold and Misty'");
            Item.ResearchUnlockCount = 1;
            HeadLayer.RegisterData(Item.headSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Head_Glow")
            });
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
        }
        public override void UpdateEquip(Player player)
        {
            //player.maxTurrets += 1;
            //player.GetDamage(DamageClass.Generic) += 0.07f;
            player.GetDamage(DamageClass.Magic) += 0.1f;
            player.GetDamage(DamageClass.Summon) += 0.1f;

            player.statManaMax2 += 40;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<ContestArmourChestplate>() && legs.type == ItemType<ContestArmourLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            var list = StormDiversMod.ArmourSpecialHotkey.GetAssignedKeys();
            string keyName = "(Not bound, set in controls)";

            if (list.Count > 0)
            {
                keyName = list[0];
            }

            //player.setBonus = "Grants immunity to all cold themed debuffs Press " + keyName + "' to summon a cryo cloud at the cursor that rains down icicles requires a line of sight and consumes 75 mana";
            player.setBonus = this.GetLocalization("SetBonus1").Value + " '" + keyName + "' " + this.GetLocalization("SetBonus2").Value;

            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[ModContent.BuffType<Buffs.SuperFrostBurn>()] = true;
            player.buffImmune[ModContent.BuffType<Buffs.UltraFrostDebuff>()] = true;

            player.GetModPlayer<ArmourSetBonuses>().cryoSet = true;
            player.GetModPlayer<ArmourSetBonuses>().SetBonus_Cryo = "true";

        }
        int particle = 10;
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
            if (player.GetModPlayer<ArmourSetBonuses>().cryosetcooldown >= 120 && player.statMana >= 100)
            {
                player.armorEffectDrawOutlines = true;
            }
            else
            {
                player.armorEffectDrawOutlines = false;

            }
            {
                particle--;
                if (particle <= 0)
                {
                    int dustIndex = Dust.NewDust(new Vector2(player.position.X + 1f, player.position.Y), player.width, player.height, 31, 0f, 0f, 100, default, 1f);
                    Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;

                    particle = 10;
                }

            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          //.AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 8)
          .AddIngredient(ModContent.ItemType<Items.Materials.BlueCloth>(), 10)
          .AddIngredient(ItemID.IceBlock, 50)
          .AddIngredient(ItemID.Bone, 30)
          .AddTile(TileID.Anvils)
          .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/ContestArmourBHelmet_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

    }
    //_________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    public class ContestArmourChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Cryogenic Chestplate");
            //Tooltip.SetDefault("Increases maximum minions by 1\nIncreases Mana regeneration");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;

        }
        public override void UpdateEquip(Player player)
        {
            //player.GetDamage(DamageClass.Magic) += 0.06f;
            //player.GetCritChance(DamageClass.Magic) += 4;
            player.maxMinions += 1;
            player.manaRegenBuff = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          //.AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 8)
          .AddIngredient(ModContent.ItemType<Items.Materials.BlueCloth>(), 15)
          .AddIngredient(ItemID.IceBlock, 100)
          .AddIngredient(ItemID.Bone, 60)
          .AddTile(TileID.Anvils)
          .Register();
        }
    }
    //_________________________________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class ContestArmourLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Cryogenic Greaves");
            //Tooltip.SetDefault("Increases your max number of sentries by 1\n
            //Increases maximum mana by 20");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;

        }
        public override void UpdateEquip(Player player)
        {
            //player.maxTurrets += 1;
            //player.GetDamage(DamageClass.Generic) += 0.07f;
            //player.GetCritChance(DamageClass.Magic) += 6;
            player.statManaMax2 += 20;
            player.maxTurrets += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
          //.AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 8)
          .AddIngredient(ModContent.ItemType<Items.Materials.BlueCloth>(), 12)
          .AddIngredient(ItemID.IceBlock, 75)
          .AddIngredient(ItemID.Bone, 40)
          .AddTile(TileID.Anvils)
          .Register();
        }
    }
    public class CryoSetProjs : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile) //Dust effects
        {
            /*var player = Main.player[projectile.owner];
            if (ProjectileID.Sets.SentryShot[projectile.type] == true && projectile.owner == Main.myPlayer)
            {
                if (player.GetModPlayer<ArmourSetBonuses>().cryoSet == true)
                {

                    if (Main.rand.Next(4) < 2)
                    {
                        int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 187, 0f, 0f, 100, default, 1f);
                        Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].noGravity = true;
                    }

                }
            }*/

        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) //PvE
        {
           
        }
    }
}