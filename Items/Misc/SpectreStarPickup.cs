using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Common;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using StormDiversMod.Projectiles;
using StormDiversMod.Buffs;



namespace StormDiversMod.Items.Misc
{
    public class SpectreStarPickup : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spectre Star");
            //Tooltip.SetDefault("Restores 200 mana and grants the Spectre Star Buff for 5 seconds");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Yellow;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.IgnoresEncumberingStone[Item.type] = true;
            ItemID.Sets.IsAPickup[Item.type] = true;
            Item.noGrabDelay = 0;
            Item.ResearchUnlockCount = 0;
        }
        public override bool GrabStyle(Player player)
        {
            if (player.manaMagnet)
            {
                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(Item.Center.X, Item.Center.Y)) * 15;
                Item.velocity = velocity;
            }
            else
            {
                Vector2 velocity = Vector2.Normalize(new Vector2(player.Center.X, player.Center.Y) - new Vector2(Item.Center.X, Item.Center.Y)) * 8;
                Item.velocity = velocity;
            }
            return true;
        }
        public override void GrabRange(Player player, ref int grabRange)
        {
            if (player.manaMagnet)
                grabRange = 342;
            else
                grabRange = 100;
        }
        int timeleft;
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            timeleft++;
            if (timeleft == 900)//last for 15 seconds in world
            {
                Item.TurnToAir();
                SoundEngine.PlaySound(SoundID.NPCDeath7, Item.Center);
                for (int i = 0; i < 20; i++) 
                {
                    int dust = Dust.NewDust(new Vector2(Item.position.X, Item.position.Y), Item.width, Item.height, 135, 0, 0, 120, default, 1.5f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 2f;

                    //Main.dust[dust].velocity *= 2.5f;
                }
            }
        }
        public override bool CanPickup(Player player)
        {
            return true;
        }
        int extratime;
        public override bool OnPickup(Player player)
        {
            int buffindex = player.FindBuffIndex(ModContent.BuffType<SpectreStarBuff>());
            if (buffindex > -1)
            {
                extratime = player.buffTime[buffindex] + 1;
            }
                player.AddBuff(ModContent.BuffType<SpectreStarBuff>(), Math.Min(600, Math.Max(0, 300 + extratime))); //extra 5 seconds per star, caps at 10
            SoundEngine.PlaySound(SoundID.Grab, player.Center);

            player.statMana += 100;
            player.ManaEffect(100);
            return false;
        }
       
        public override bool ItemSpace(Player player)
        {
            return true;
        }
       
        public override Color? GetAlpha(Color lightColor)
        {
            // Makes sure the dropped bag is always visible
            return Color.Lerp(lightColor, Color.White, 0.4f);
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