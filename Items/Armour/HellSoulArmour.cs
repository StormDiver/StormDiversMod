using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)] //melee helmet
    public class HellSoulBMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Inferno Soul Mask");
            Tooltip.SetDefault("14% increased melee damage\n5% increased melee critical strike chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            Item.defense = 21;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Melee) += 0.14f;
            player.GetCritChance(DamageClass.Melee) += 5;
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
            if (Main.rand.Next(3) == 0)
            {
                var dust = Dust.NewDustDirect(player.position, player.width, player.height, 173, 0, -3);
                dust.scale = 1f;
            }

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<HellSoulChestplate>() && legs.type == ItemType<HellSoulLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "20% increased melee speed\nAttacking enemies releases damaging homing souls from them";

            player.GetModPlayer<ArmourSetBonuses>().hellSoulSet = true;
            player.GetAttackSpeed(DamageClass.Melee) += 0.2f;

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
            DisplayName.SetDefault("Inferno Soul Helmet");
            Tooltip.SetDefault("10% increased ranged damage\n8% increased ranged critical strike chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            Item.defense = 10;
        }
   
        public override void UpdateEquip(Player player)
        {
        
            player.GetDamage(DamageClass.Ranged) += 0.10f;
            player.GetCritChance(DamageClass.Ranged) += 8;
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
            if (Main.rand.Next(4) == 0)
            {
                var dust = Dust.NewDustDirect(player.position, player.width, player.height, 173);
                dust.scale = 1.4f;
            }

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<HellSoulChestplate>() && legs.type == ItemType<HellSoulLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "25% chance not to consume ammo\nAttacking enemies releases damaging homing souls from them";

            player.GetModPlayer<ArmourSetBonuses>().hellSoulSet = true;
            player.ammoCost75 = true;

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
            DisplayName.SetDefault("Inferno Soul Hood");
            Tooltip.SetDefault("12% increased magic damage\n6% increased critical strike chance\nIncreases maximum mana by 60");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
        
            player.GetDamage(DamageClass.Magic) += 0.12f;
            player.GetCritChance(DamageClass.Magic) += 6;
            player.statManaMax2 += 60;
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
            if (Main.rand.Next(4) == 0)
            {
                var dust = Dust.NewDustDirect(player.position, player.width, player.height, 173);
                dust.scale = 1.4f;
            }

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<HellSoulChestplate>() && legs.type == ItemType<HellSoulLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "18% reduced mana usage\nAttacking enemies releases damaging homing souls from them";

            player.GetModPlayer<ArmourSetBonuses>().hellSoulSet = true;
            player.manaCost -= 0.18f;
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
            DisplayName.SetDefault("Inferno Soul Crown");
            Tooltip.SetDefault("10% increased summoner damage\nIncreases maximum number of minions by 1");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            Item.defense = 2;
        }
    
        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Summon) += 0.10f;
            player.maxMinions += 1;
            if (!Main.dedServ)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.4f);
            }
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
            if (Main.rand.Next(4) == 0)
            {
                var dust = Dust.NewDustDirect(player.position, player.width, player.height, 173);
                dust.scale = 1.4f;
            }
       
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<HellSoulChestplate>() && legs.type == ItemType<HellSoulLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Increases maximum number of minions by 2\nAttacking enemies releases damaging homing souls from them";

            player.GetModPlayer<ArmourSetBonuses>().hellSoulSet = true;
            player.maxMinions += 2;
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
            DisplayName.SetDefault("Inferno Soul Breastplate");
            Tooltip.SetDefault("7% increased damage\n6% increased critical strike chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            BodyGlowmaskPlayer.RegisterData(Item.bodySlot, () => new Color(255, 255, 255, 0) * 0.8f);

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 17;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Generic) += 0.07f;
            player.GetCritChance(DamageClass.Generic) += 6;

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
            DisplayName.SetDefault("Inferno Soul Greaves");
            Tooltip.SetDefault("5% increased damage\n6% increased critical strike chance\n25% increased movement speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            Item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {

            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.GetCritChance(DamageClass.Generic) += 6;

            player.moveSpeed += 0.25f;
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