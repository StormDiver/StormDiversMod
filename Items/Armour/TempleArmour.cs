using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod;
using Terraria.GameContent.Creative;
using System.Collections.Generic;


namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class TempleBMask : ModItem
    { 
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Lihzarhd Temple Mask");
            //Tooltip.SetDefault("10% increased whip range
            //10 % increased whip speed
            //        15 % increased summoner damage
            //        Increases maximum minions by 1");
            Item.ResearchUnlockCount = 1;
            HeadLayer.RegisterData(Item.headSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Head_Glow")
            });
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (!NPC.downedPlantBoss)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        if (!GetInstance<ConfigurationsGlobal>().NoScaryCurse)
                            line.Text = line.Text + "\n[c/A14F12:Inflicted with a strange curse, pick up at your own risk!]";
                        else
                            line.Text = line.Text + "\n[c/A14F12:Inflicted with a strange curse, equip at your own risk!]";
                    }
                }
            }
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.whipRangeMultiplier += 0.10f;
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.10f;
            player.GetDamage(DamageClass.Summon) += 0.15f;
            player.maxMinions += 1;

            if (!Main.dedServ)
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.2f);
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<TempleChest>() && legs.type == ItemType<TempleLegs>();
        }

        public override void UpdateArmorSet(Player player)
        {
            //player.setBonus = "Striking an enemy with a whip summons a Lihzahrd drone which orbits the tagged enemy\nThe Drone closes in on the enemy, and explodes\nThe drone will also explode if the enemy is killed or loses the tag";
            player.setBonus = this.GetLocalization("SetBonus").Value;
            player.GetModPlayer<ArmourSetBonuses>().LizardSet = true;
            player.GetModPlayer<ArmourSetBonuses>().SetBonus_Lizard = "true";
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarTabletFragment, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
           Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/TempleBMask_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
  
    public class TempleChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Lihzarhd Temple Chestplate");
            //Tooltip.SetDefault("15% increased whip range
            //15 % increased whip speed
            //        15 % increased summoner damage
            //        Enables autoswing for whips");
            Item.ResearchUnlockCount = 1;
            if (!Main.dedServ)
            {
                BodyGlowmaskPlayer.RegisterData(Item.bodySlot, () => new Color(255, 255, 255, 75) * 0.6f);
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (!NPC.downedPlantBoss)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        if (!GetInstance<ConfigurationsGlobal>().NoScaryCurse)
                            line.Text = line.Text + "\n[c/A14F12:Inflicted with a strange curse, pick up at your own risk!]";
                        else
                            line.Text = line.Text + "\n[c/A14F12:Inflicted with a strange curse, equip at your own risk!]";
                    }
                }
            }
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 15;
        }
        public override void UpdateEquip(Player player)
        {
            player.whipRangeMultiplier += 0.15f;
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
            player.GetDamage(DamageClass.Summon) += 0.15f;

            if (player.HeldItem.DamageType == DamageClass.SummonMeleeSpeed)
            player.autoReuseGlove = true;

            if (!Main.dedServ)
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.2f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarTabletFragment, 16)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/TempleChest_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class TempleLegs : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Lihzarhd Temple Greaves");
            //Tooltip.SetDefault("15% increased whip range
            //10 % increased whip speed
            //10 % increased summoner damage
            // Increases maximum minions by 1");
            Item.ResearchUnlockCount = 1;
            LegsLayer.RegisterData(Item.legSlot, new DrawLayerData()
            {
                Texture = ModContent.Request<Texture2D>(Texture + "_Legs_Glow")
            });
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (!NPC.downedPlantBoss)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                    {
                        if (!GetInstance<ConfigurationsGlobal>().NoScaryCurse)
                            line.Text = line.Text + "\n[c/A14F12:Inflicted with a strange curse, pick up at your own risk!]";
                        else
                            line.Text = line.Text + "\n[c/A14F12:Inflicted with a strange curse, equip at your own risk!]";
                    }
                }
            }
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5 ,0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {
            player.whipRangeMultiplier += 0.15f;
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.10f;
            player.GetDamage(DamageClass.Summon) += 0.10f;
            player.maxMinions += 1;

            if (!Main.dedServ)
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.2f);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarTabletFragment, 14)
            .AddTile(TileID.MythrilAnvil)
            .Register();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/TempleLegs_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //__________________________________________________________________________________________________________________________
   

}