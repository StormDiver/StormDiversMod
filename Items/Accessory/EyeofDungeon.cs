using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace StormDiversMod.Items.Accessory
{

    public class EyeofDungeon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eye of the Dungeon");
            //Tooltip.SetDefault("Summons homing spinning bones when near enemies");
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;

            Item.defense = 2;
            Item.accessory = true;
            

        }
        int skulltime = 0;
        //Item DungeonEye;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //DungeonEye = Item;
            skulltime++;
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
               
                if (Vector2.Distance(player.Center, target.Center) <= 250 && !target.friendly && target.lifeMax > 5 && !target.dontTakeDamage && target.active && target.type != NPCID.TargetDummy && Collision.CanHit(player.Center, 0, 0, target.Center, 0, 0))
                {
                    if (skulltime >= 45)
                    {
                        int damage = 16;
                        float speedX = 0f;
                        float speedY = -5f;
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
                        float scale = 1f - (Main.rand.NextFloat() * .5f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(player.GetSource_Accessory(Item), new Vector2(player.Center.X, player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.BoneAcProj>(), damage, 1.5f, player.whoAmI);

                        skulltime = 0;
                    }
                }
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Items/Accessory/EyeofDungeon_Glow");

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}