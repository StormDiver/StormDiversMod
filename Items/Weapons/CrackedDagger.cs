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
using StormDiversMod.Common;
using Terraria.GameContent.Drawing;
using StormDiversMod.Items.Materials;

namespace StormDiversMod.Items.Weapons
{
    public class CrackedDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shattered Dagger");
            //Tooltip.SetDefault("Summons dagger shards around cursor that travel inwards each use\nHas a limited range and requries a line of sight");
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;

        }
        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useTurn = false;
            //Item.channel = true;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item8 with {Volume = 1f, Pitch = 0.2f};

            Item.damage = 20;
            //Item.crit = 4;
            Item.knockBack = 0.1f;

            Item.shoot = ModContent.ProjectileType<SoulsProj>();
            
            Item.shootSpeed = 1f;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 30;
            }
            else
            {
                Item.mana = 20;
            }

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override void PostUpdate()
        {
            if (!Main.dedServ)
            {
                Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.2f * Main.essScale);
            }
            if (Item.lavaWet && Item.velocity.Y > -2f)
                Item.velocity.Y -= 0.25f;
        }
        public override bool CanUseItem(Player player)
        {
            if (Collision.CanHitLine(Main.MouseWorld, 1, 1, player.Center, 0, 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        Vector2 projvelocity;
        
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //position = Main.MouseWorld;

            //For the radius
            double deg = Main.rand.Next(0, 360); //The degrees

            for (int i = 0; i < 3; i++)
            {
                deg += 120;
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 150; //Distance away from the cursor

                float projspeed = 8;

                if (Vector2.Distance(player.Center, Main.MouseWorld) <= 500)
                {
                    position.X = Main.MouseWorld.X - (int)(Math.Cos(rad) * dist);
                    position.Y = Main.MouseWorld.Y - (int)(Math.Sin(rad) * dist);
                    projvelocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(position.X, position.Y)) * projspeed;
                }
                else
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X * 500, velocity.Y * 500).RotatedBy(0);
                    position.X = player.Center.X + perturbedSpeed.X - (int)(Math.Cos(rad) * dist);
                    position.Y = player.Center.Y + perturbedSpeed.Y - (int)(Math.Sin(rad) * dist);
                    projvelocity = Vector2.Normalize(new Vector2(player.Center.X + perturbedSpeed.X, player.Center.Y + perturbedSpeed.Y) - new Vector2(position.X, position.Y)) * projspeed;
                }
                //For the direction

                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(projvelocity.X, projvelocity.Y), ModContent.ProjectileType<CrackedDaggerProj>(), damage, knockback, player.whoAmI);
            }
                Vector2 dustoffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 33;
                for (int i = 0; i < 10; i++)
                {
                    var dust = Dust.NewDustDirect((player.Center - new Vector2(15, 15)) + dustoffset, 30, 30, 248, velocity.X * 10, velocity.Y * 10, 100, default, 1);
                    dust.noGravity = true;
                    dust.velocity *= 0;
                }
            
            return false;
        }
       
        /*public override Vector2? HoldoutOffset()
        {
            return new Vector2(6, 0);
        }*/

    }
   
}