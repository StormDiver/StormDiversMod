using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.UI;
using System.Diagnostics.Metrics;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)] //melee helmet
    public class HellSoulBMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Infernal Soul Mask");
            //Tooltip.SetDefault("15% increased melee critical strike chance
            //\n15 % increased melee speed ");
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
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 15;

            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;

            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }

        public override void ArmorSetShadows(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<Buffs.HellSoulBuff>()))
            {
                player.armorEffectDrawOutlines = true;

                if (Main.rand.Next(3) == 0)
                {
                    var dust = Dust.NewDustDirect(player.position, player.width, player.height, 173, 0, -3);
                    dust.scale = 1f;
                }
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<HellSoulChestplate>() && legs.type == ItemType<HellSoulLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var list = StormDiversMod.ArmourSpecialHotkey.GetAssignedKeys();
            string keyName = "(Not bound, set in controls)";

            if (list.Count > 0)
            {
                keyName = list[0];
            }

            //player.setBonus = "Charges up a powerful Infernal Storm over 10 seconds, press '" + keyName + "' once charged to unleash the storm upon up to 15 nearby enemies";
            player.setBonus = this.GetLocalization("SetBonus1").Value + " '" + keyName + "' " + this.GetLocalization("SetBonus2").Value;

            player.GetModPlayer<ArmourSetBonuses>().hellSoulSet = true;
            player.GetModPlayer<ArmourSetBonuses>().SetBonus_HellSoul = "true";
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 12)
            .AddTile(TileID.Hellforge)
            .Register();

           
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/HellSoulBMask_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }

    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Head)] //Ranged helmet
    public class HellSoulBHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Infernal Soul Helmet");
            //Tooltip.SetDefault("5% increased ranged damage\n15% increased ranged critical strike chance");
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
            Item.value = Item.sellPrice(0, 5, 0, 0);
                     Item.rare = ItemRarityID.LightPurple;
            Item.defense = 7;
        }
   
        public override void UpdateEquip(Player player)
        {
        
            player.GetDamage(DamageClass.Ranged) += 0.5f;
            player.GetCritChance(DamageClass.Ranged) += 15;
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }

        public override void ArmorSetShadows(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<Buffs.HellSoulBuff>()))
            {
                player.armorEffectDrawOutlines = true;
                if (Main.rand.Next(4) == 0)
                {
                    var dust = Dust.NewDustDirect(player.position, player.width, player.height, 173);
                    dust.scale = 1.4f;
                }
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<HellSoulChestplate>() && legs.type == ItemType<HellSoulLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var list = StormDiversMod.ArmourSpecialHotkey.GetAssignedKeys();
            string keyName = "(Not bound, set in controls)";

            if (list.Count > 0)
            {
                keyName = list[0];
            }

            //player.setBonus = "Charges up a powerful Infernal Storm over 10 seconds, press '" + keyName + "' once charged to unleash the storm upon up to 15 nearby enemies";
            player.setBonus = this.GetLocalization("SetBonus1").Value + " '" + keyName + "' " + this.GetLocalization("SetBonus2").Value;

            player.GetModPlayer<ArmourSetBonuses>().hellSoulSet = true;
            player.GetModPlayer<ArmourSetBonuses>().SetBonus_HellSoul = "true";
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 12)
            .AddTile(TileID.Hellforge)
            .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/HellSoulBHelmet_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }

    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Head)] //magic helmet
    public class HellSoulBHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Infernal Soul Hood");
            //Tooltip.SetDefault("5% increased magic damage\n15% increased magic critical strike chance\nIncreases maximum mana by 80");
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
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
        
            player.GetDamage(DamageClass.Magic) += 0.05f;
            player.GetCritChance(DamageClass.Magic) += 15;
            player.statManaMax2 += 80;
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }

        public override void ArmorSetShadows(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<Buffs.HellSoulBuff>()))
            {
                player.armorEffectDrawOutlines = true;
                if (Main.rand.Next(4) == 0)
                {
                    var dust = Dust.NewDustDirect(player.position, player.width, player.height, 173);
                    dust.scale = 1.4f;
                }
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<HellSoulChestplate>() && legs.type == ItemType<HellSoulLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var list = StormDiversMod.ArmourSpecialHotkey.GetAssignedKeys();
            string keyName = "(Not bound, set in controls)";

            if (list.Count > 0)
            {
                keyName = list[0];
            }

            //player.setBonus = "Charges up a powerful Infernal Storm over 10 seconds, press '" + keyName + "' once charged to unleash the storm upon up to 15 nearby enemies";
            player.setBonus = this.GetLocalization("SetBonus1").Value + " '" + keyName + "' " + this.GetLocalization("SetBonus2").Value;

            player.GetModPlayer<ArmourSetBonuses>().hellSoulSet = true;
            player.GetModPlayer<ArmourSetBonuses>().SetBonus_HellSoul = "true";
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 12)
           .AddTile(TileID.Hellforge)
           .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/HellSoulBHood_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Head)] //summoner helmet
    public class HellSoulBCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Infernal Soul Crown");
            //Tooltip.SetDefault("Increases maximum number of minions by 2");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
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
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 1;
        }
   
        public override void UpdateEquip(Player player)
        {
            //player.GetDamage(DamageClass.Summon) += 0.15f;
            player.maxMinions += 2;
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }

        public override void ArmorSetShadows(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<Buffs.HellSoulBuff>()))
            {
                player.armorEffectDrawOutlines = true;
                if (Main.rand.Next(4) == 0)
                {
                    var dust = Dust.NewDustDirect(player.position, player.width, player.height, 173);
                    dust.scale = 1.4f;
                }
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<HellSoulChestplate>() && legs.type == ItemType<HellSoulLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var list = StormDiversMod.ArmourSpecialHotkey.GetAssignedKeys();
            string keyName = "(Not bound, set in controls)";

            if (list.Count > 0)
            {
                keyName = list[0];
            }

            //player.setBonus = "Increases maximum number of minions by 2\nCharges up a powerful Infernal Storm over 10 seconds, press '" + keyName + "' once charged to unleash the storm upon up to 15 nearby enemies";
            player.setBonus = this.GetLocalization("SetBonus1").Value + " '" + keyName + "' " + this.GetLocalization("SetBonus2").Value;

            player.GetModPlayer<ArmourSetBonuses>().hellSoulSet = true;
            player.GetModPlayer<ArmourSetBonuses>().SetBonus_HellSoul = "true";
            player.maxMinions += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 12)
           .AddTile(TileID.Hellforge)
           .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/HellSoulBCrown_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
       
    }
    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    
    public class HellSoulChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Infernal Soul Breastplate");
            //Tooltip.SetDefault("10% increased critical strike chance");
            Item.ResearchUnlockCount = 1;
            if (!Main.dedServ)
            {
                BodyGlowmaskPlayer.RegisterData(Item.bodySlot, () => new Color(255, 255, 255, 75) * 0.6f);
            }
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 10;

            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 18)
            .AddTile(TileID.Hellforge)
            .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/HellSoulChestplate_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class HellSoulLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Infernal Soul Greaves");
            //Tooltip.SetDefault("10% increased damage");
            Item.ResearchUnlockCount = 1;
            LegsLayer.RegisterData(Item.legSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Legs_Glow")
            });
        }
       
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f;

            player.moveSpeed += 0.15f;

            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.SoulFire>(), 15)
            .AddTile(TileID.Hellforge)
            .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/HellSoulLeggings_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f ),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

    }
    
}