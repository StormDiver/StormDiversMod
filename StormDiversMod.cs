using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using StormDiversMod.Basefiles;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace StormDiversMod
{
	public class StormDiversMod : Mod //For most important things
	{
        public override void PostSetupContent() //For boss checklist
        {
            if (ModLoader.HasMod("BossChecklist"))//DON'T FORGET THIS!!!!!!!
            {
                Mod bossChecklist = ModLoader.GetMod("BossChecklist");
                if (bossChecklist != null)
                {
                    bossChecklist.Call
                        ("AddBoss", this, "Overloaded Scandrone", ModContent.NPCType<NPCs.Boss.StormBoss>(), 12f, (Func<bool>)(() => StormWorld.stormBossDown), () => true,
                        new List<int> { ModContent.ItemType<Items.BossTrophy.StormBossTrophy>(), ModContent.ItemType<Items.BossTrophy.StormBossRelic>(), ModContent.ItemType<Items.Pets.StormBossPetItem>(),
                        ModContent.ItemType<Items.BossTrophy.StormBossBag>(), ModContent.ItemType<Items.Accessory.StormCoil>(), //vanity
                        //other
                        //ModContent.ItemType<Items.Weapons.StormKnife>(), ModContent.ItemType<Items.Weapons.StormLauncher>(), ModContent.ItemType<Items.Weapons.StormStaff>(), ModContent.ItemType<Items.Weapons.StormSentryStaff>(),
                        ModContent.ItemType<Items.Vanitysets.BossMaskStormBoss>()}, //ModContent.ItemType<Items.Tools.StormHook>(), ModContent.ItemType<Items.Accessory.StormWings>(),ItemID.TempleKey},

                        ModContent.ItemType<Items.Summons.StormBossSummoner>(), "Spawned by using a Storm Beacon once all 3 mechs have been defeated",
                        "Overloaded Scandrone returns to its home planet (and didn't die on the way home)",
                        (SpriteBatch sb, Rectangle rect, Color color) => {
                            Texture2D texture = ModContent.Request<Texture2D>("StormDiversMod/NPCs/Boss/StormBoss_Image").Value;
                            Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                            sb.Draw(texture, centered, color);
                        }
                        );
                    // Additional bosses here
                }
            }
        }

        public static ModKeybind ArmourSpecialHotkey;
        public override void Load()
        {
            ArmourSpecialHotkey = KeybindLoader.RegisterKeybind(this, "Armor Special Ability", "V");
            /*if (GetInstance<ConfigurationsGlobal>().NoDamageSpread)
            {
                //!!All credit goes to Kojo's mod called Rho's Playground!!
                On.Terraria.Main.DamageVar += (orig, damage, luck) => (int)Math.Round(damage * Main.rand.NextFloat(1, 1)); //No damage variance
            }*/
        }
        public override void Unload()
        {
            ArmourSpecialHotkey = null;
        }
        public StormDiversMod()
		{
             
		}
       
    }
    public class immunity : GlobalProjectile
    {
        /*public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileID.TerrarianBeam)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 10;
            }
        }*/
     
    }
}