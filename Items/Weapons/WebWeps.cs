using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;

namespace StormDiversMod.Items.Weapons
{
    public class WebStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Web Staff");
            //Tooltip.SetDefault("Fires out a blob of web that sticks to surfaces");
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;

        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 34;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 15, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 15;
            }
            else
            {
                Item.mana = 10;
            }
            //Item.UseSound = SoundID.Item8;

            Item.damage = 17;
         
            Item.knockBack = 4f;

            Item.shoot = ModContent.ProjectileType<WebProj>();

            Item.shootSpeed = 10f;
   
            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            
            
                SoundEngine.PlaySound(SoundID.NPCDeath9, player.Center);

                {


                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(0));
                Projectile.NewProjectile(source, new Vector2(position.X, position.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, damage, knockback, player.whoAmI);


            }

           
            return false;
        }
        //Also generates in web covered chests
       
    }
    //________________________________
    public class WebWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spider Whip");
            //Tooltip.SetDefault("3 summon tag damage\nYour summons will focus struck enemies\nThe targetted enemy has a chance to be slowed down when hit by summons");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.WhipProjs.WebWhipProj>(), 12, 0.5f, 3.5f, 30);
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 15, 0);
            Item.rare = ItemRarityID.Blue;
            Item.DamageType = DamageClass.SummonMeleeSpeed;

            Item.noMelee = true;
        }
        public override bool MeleePrefix()
        {
            return true;
        }   

    }

}