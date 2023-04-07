using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using StormDiversMod.Basefiles;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class ContestArmourBHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Cryogenic Mask");
            //Tooltip.SetDefault("Increases your max number of sentries by 1\n7% increased damage\n'Cold and Misty'");
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
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 7;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxTurrets += 1;
            player.GetDamage(DamageClass.Generic) += 0.07f;
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

            player.setBonus = "Grants immunity to all cold themed debuffs\nMakes all sentries inflict Cryoburn\nPress '" + keyName + "' to summon a cryo cloud at the cursor that rains down icicles, requires a line of sight and has a 5 second cooldown";
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[ModContent.BuffType<Buffs.SuperFrostBurn>()] = true;
            player.buffImmune[ModContent.BuffType<Buffs.UltraFrostDebuff>()] = true;

            player.GetModPlayer<ArmourSetBonuses>().cryoSet = true;

        }
        int particle = 10;
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
            if (player.GetModPlayer<ArmourSetBonuses>().cryosetcooldown >= 300)
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
          .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 8)
          .AddIngredient(ModContent.ItemType<Items.Materials.BlueCloth>(), 10)

          .AddTile(TileID.MythrilAnvil)
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
            //Tooltip.SetDefault("8% increased damage\n6% increased critical strike chance");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 8;

        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;
            player.GetCritChance(DamageClass.Generic) += 6;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 12)
         .AddIngredient(ModContent.ItemType<Items.Materials.BlueCloth>(), 15)

         .AddTile(TileID.MythrilAnvil)
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
            //Tooltip.SetDefault("Increases your max number of sentries by 1\n7% increased damage\n10% increased movement speed");
            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 7;

        }
        public override void UpdateEquip(Player player)
        {
            player.maxTurrets += 1;
            player.GetDamage(DamageClass.Generic) += 0.07f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
         .AddIngredient(ModContent.ItemType<Items.OresandBars.IceBar>(), 10)
         .AddIngredient(ModContent.ItemType<Items.Materials.BlueCloth>(), 12)

         .AddTile(TileID.MythrilAnvil)
         .Register();
        }
    }
    public class CryoSetProjs : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile) //Dust effects
        {
            var player = Main.player[projectile.owner];
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
            }

        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) //PvE
        {
           
        }
    }
}