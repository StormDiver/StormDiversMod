using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using StormDiversMod;
using StormDiversMod.Items.Materials;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace StormDiversMod.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class NightsOldBHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Ancient Twilight Hood");
            //Tooltip.SetDefault("8% increased damage\n8% increased critical strike chance");
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
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;
            player.GetCritChance(DamageClass.Generic) += 8;
          
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
            //player.armorEffectDrawOutlines = true;
                 
            if (player.GetModPlayer<ArmourSetBonuses>().twilightcharged == true)
            {
                player.armorEffectDrawOutlines = true;
                if (Main.rand.Next(4) == 0)
                {
                    var dust = Dust.NewDustDirect(player.position, player.width, player.height, 62, 0, -5);
                    dust.scale = 0.8f;
                    dust.noGravity = true;
                }
            }
            else
            {
                player.armorEffectDrawOutlines = false;
            }
            player.socialShadowRocketBoots = true;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (body.type == ItemType<NightsChainmail>() || body.type == ItemType<NightsOldChainmail>()) && (legs.type == ItemType<NightsGreaves>() || legs.type == ItemType<NightsOldGreaves>());
        }
        public override void UpdateArmorSet(Player player)
        {
            var list = StormDiversMod.ArmourSpecialHotkey.GetAssignedKeys();
            string keyName = "(Not bound, set in controls)";

            if (list.Count > 0)
            {
                keyName = list[0];
            }

            //player.setBonus = "Press '" + keyName + "' to warp to the cursor's location within a limited range\nWarping has a hard 8 second cooldown\n'Teleporting is just dashing at the speed of light'";
            player.setBonus = this.GetLocalization("SetBonus1").Value + " '" + keyName + "' " + this.GetLocalization("SetBonus2").Value;

            //player.endurance += 0.1f;
            //player.blackBelt = true;
            player.GetModPlayer<ArmourSetBonuses>().twilightSet = true;
        }
        public override void AddRecipes()
        {          
            CreateRecipe()
            .AddIngredient(ItemID.Silk, 15)
            .AddIngredient(ItemID.SoulofNight, 6)
            .AddIngredient(ModContent.ItemType<ChaosShard>(), 2)
            .AddTile(TileID.DemonAltar)
            .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/NightsOldBHelmet_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }    
    }
    //___________________________________________________________________________________________________________________________
    [AutoloadEquip(EquipType.Body)]
    
    public class NightsOldChainmail : ModItem
    {     
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            //DisplayName.SetDefault("Ancient Twilight Robe");
            //Tooltip.SetDefault("8% increased damage\nIncreases player acceleration");
            Item.ResearchUnlockCount = 1;
            ArmorIDs.Body.Sets.NeedsToDrawArm[Item.bodySlot] = false;
            if (!Main.dedServ)
            {
                BodyGlowmaskPlayer.RegisterData(Item.bodySlot, () => new Color(255, 255, 255, 75) * 0.6f);
            }
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 11;
        }
        public virtual void DrawArmorColor(EquipType type, int slot, Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {

        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;

            player.runAcceleration += 0.15f;
            //player.lifeRegen += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Silk, 20)
            .AddIngredient(ItemID.SoulofNight, 9)
            .AddIngredient(ModContent.ItemType<ChaosShard>(), 3)
            .AddTile(TileID.DemonAltar)
            .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/NightsOldChainmail_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //______________________________________________________________________
    [AutoloadEquip(EquipType.Legs)]
    public class NightsOldGreaves : ModItem
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Ancient Twilight Leggings");
            //Tooltip.SetDefault("10% increased critical strike chance\nIncreases jump speed and height");
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
            Item.value = Item.sellPrice(0, 1 ,50, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 10;
            player.jumpSpeedBoost += 1.2f;
           
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ItemID.Silk, 18)
             .AddIngredient(ItemID.SoulofNight, 7)
             .AddIngredient(ModContent.ItemType<ChaosShard>(), 2)
             .AddTile(TileID.DemonAltar)
             .Register();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Armour/NightsOldGreaves_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
    //__________________________________________________________________________________________________________________________
   

}