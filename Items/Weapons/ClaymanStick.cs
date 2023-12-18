using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;
using Terraria.DataStructures;
using static Terraria.Main.CurrentFrameFlags;
using StormDiversMod.Items.Vanitysets;
using Terraria.GameContent.Drawing;
using StormDiversMod.Basefiles;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.NPCs;
using NVorbis.Contracts;
using Microsoft.CodeAnalysis;
using StormDiversMod.Items.Weapons;
using System.Security.Policy;

namespace StormDiversMod.Items.Weapons
{
    public class ClaymanStick : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stick of Judgment");
            //Tooltip.SetDefault("//Left click to judge enemies in front of you
            //Right click to judge a specfic point dealing much more damage
            //'Everyone will be judged'");
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;

        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Magic;
            if (ModLoader.HasMod("TRAEProject"))
            {
                Item.mana = 30;
            }
            else
            {
                Item.mana = 20;
            }

            Item.damage = 80;
            //Item.crit = 4;
            Item.knockBack = 0f;

            Item.shoot = ModContent.ProjectileType<ClaymanProj>();

            Item.shootSpeed = 5f;
            
            //Item.useAmmo = AmmoID.Arrow;

            Item.noMelee = true; //Does the weapon itself inflict damage?
        }
        /* public override Vector2? HoldoutOffset()
         {
             return new Vector2(5, 0);
         }*/

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation += new Vector2(-15 * player.direction, -4);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {

            if (player.altFunctionUse == 2)
            {
               
            }
            else
            {
               
            }
            return true;
        }
        int extraheight;

        int xlimit;
        int ylimittop;
        int ylimitbottom;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            xlimit = 600;
            ylimittop = -250;
            ylimitbottom = 200;
            if (player.mount.Active)
                extraheight = -6;
            else
                extraheight = 0;
            if (player.altFunctionUse == 2) //Right Click
            {
                bool lineOfSight = Collision.CanHitLine(Main.MouseWorld, 0, 0, player.position, player.width, player.height);
                if (lineOfSight)
                {
                    Projectile.NewProjectile(source, Main.MouseWorld, new Vector2(0, 0), ModContent.ProjectileType<ClaymanProj>(), (int)(damage * 2.5f), 0, player.whoAmI, 1);
                    if (!GetInstance<ConfigurationsIndividual>().NoPain)
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ClayManSound") with { Volume = 1.5f, MaxInstances = 5, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, player.Center);
                    else
                        SoundEngine.PlaySound(SoundID.Item42 with { Volume = 1f, Pitch = 0.25f }, player.Center);

                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.Excalibur, new ParticleOrchestraSettings
                    {
                        PositionInWorld = Main.MouseWorld,

                    }, player.whoAmI);
                    CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.PeachPuff, "Clayman!", false);
                }
            }
            else
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC target = Main.npc[i];

                    bool lineOfSight = Collision.CanHitLine(target.position, target.width, target.height, player.position, player.width, player.height);
                    float distanceX = target.position.X + ((float)target.width * 0.5f) - player.Center.X;
                    float distanceY = target.position.Y + ((float)target.height * 0.5f) - player.Center.Y;

                    if (!target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.CanBeChasedBy() && target.type != NPCID.TargetDummy)
                    {
                        if (((distanceX >= -xlimit && distanceX <= -30 && player.direction == -1) || (distanceX >= 30 && distanceX <= xlimit && player.direction == 1)) && (distanceY >= ylimittop && distanceY <= ylimitbottom) && lineOfSight)
                        {
                            if (damage > 15) //only summon projectile if enemy will take 15> damage
                            {
                                Projectile.NewProjectile(source, target.Center, new Vector2(0, 0), ModContent.ProjectileType<ClaymanProj>(), damage, 0, player.whoAmI);
                                damage = (damage * 17) / 20; //15% damage falloff per enemy
                            }
                            //hellblazedmg = (hellblazedmg * 17) / 20; //15% damage falloff per enemy
                        }
                    }
                }
                /*ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(player.Center.X + xlimit * player.direction, player.Center.Y + ylimittop * player.gravDir + extraheight),

                }, player.whoAmI);
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(player.Center.X + xlimit * player.direction, player.Center.Y + ylimitbottom * player.gravDir + extraheight),

                }, player.whoAmI);
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(player.Center.X + 30 * player.direction, player.Center.Y + ylimittop * player.gravDir + extraheight),

                }, player.whoAmI);
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
                {
                    PositionInWorld = new Vector2(player.Center.X + 30 * player.direction, player.Center.Y + ylimitbottom * player.gravDir + extraheight),

                }, player.whoAmI);*/
                if (!GetInstance<ConfigurationsIndividual>().NoPain)
                    SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ClayManSound") with { Volume = 1.5f, MaxInstances = 5, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, player.Center);
                else
                    SoundEngine.PlaySound(SoundID.Item42 with { Volume = 1f, Pitch = 0.25f }, player.Center);
                CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 12, 4), Color.PeachPuff, "Clayman!", false);
            }
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(player.Center.X + 27 * player.direction, player.Center.Y - 38 * player.gravDir + extraheight),

            }, player.whoAmI);
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(player.Center.X + 38 * player.direction, player.Center.Y - 38 * player.gravDir + extraheight),

            }, player.whoAmI);
            return false;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<TheClaymanMask>(), 1)
            .AddIngredient(ItemID.SpookyWood, 50)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
      
    }
}