using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Common;
using StormDiversMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using StormDiversMod.Projectiles;


namespace StormDiversMod.Items.Potions
{
    public class AFlaskFrost : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flask of Frost");
            //Tooltip.SetDefault("Melee and Whip attacks inflict enemies with Frostburn");
            Item.ResearchUnlockCount = 20;
            ItemID.Sets.DrinkParticleColors[Type] = [
                new Color(224, 236, 240),
                new Color(144, 195, 232),
                new Color(40, 152, 240)
            ];
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.buffType = BuffType<Buffs.FlaskFrostImbue>(); //Specify an existing buff to be applied when used.
            Item.buffTime = 72000; //20 minutes
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<AFlaskFrost>(), 1);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.IceBlock, 5);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();

        }
    }
    public class AFlaskExplosive : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flask of Explosives");
            //Tooltip.SetDefault("Melee and Whip attacks cause a small explosion");
            Item.ResearchUnlockCount = 20;
            ItemID.Sets.DrinkParticleColors[Type] = [
                new Color(0, 3, 23),
                new Color(23, 0, 22),
                new Color(212, 49, 27)
            ];
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.buffType = BuffType<Buffs.FlaskExplosiveImbue>(); //Specify an existing buff to be applied when used.
            Item.buffTime = 72000; //20 minutes
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<AFlaskExplosive>(), 1);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.ExplosivePowder, 5);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
    public class FlaskEffects : ModPlayer
    {
        public bool flaskFrost;
        public bool flaskExplosive;

        public override void ResetEffects()
        {
            flaskFrost = false;
            flaskExplosive = false;
        }
        public string FlaskExplosive;

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //flaskexplode = true;
            if (item.DamageType.CountsAsClass<MeleeDamageClass>())
            {
                if (flaskFrost)
                {
                    target.AddBuff(BuffID.Frostburn, 60 * Main.rand.Next(3, 7));
                }
                if (flaskExplosive)
                {
                    if (!target.friendly && target.lifeMax > 5 && !item.GetGlobalItem<FlaskExplosivesItem>().exploded)
                    {
                        target.GetGlobalNPC<NPCEffects>().flaskexplosivetime = 10; //target immune to explosion for 10 frames
                        Projectile.NewProjectile(Player.GetSource_FromThis(FlaskExplosive), new Vector2(target.Center.X, target.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<FlaskExplosionProj>(), (int)(damageDone / 2), 0, Player.whoAmI);
                        item.GetGlobalItem<FlaskExplosivesItem>().exploded = true;
                    }
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if ((proj.DamageType.CountsAsClass<MeleeDamageClass>() || ProjectileID.Sets.IsAWhip[proj.type]) && !proj.noEnchantments)
            {
                if (flaskFrost)
                {
                    target.AddBuff(BuffID.Frostburn, 60 * Main.rand.Next(3, 7));
                }
                if (flaskExplosive)
                {
                    if (!target.friendly && target.lifeMax > 5 && proj.type != ModContent.ProjectileType<FlaskExplosionProj>() && !proj.GetGlobalProjectile<FlaskExplosivesProj>().exploded)
                    {
                        target.GetGlobalNPC<NPCEffects>().flaskexplosivetime = 10; //target immune to explosion for 10 frames
                        Projectile.NewProjectile(Player.GetSource_FromThis(FlaskExplosive), new Vector2(target.Center.X, target.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<FlaskExplosionProj>(), (int)(damageDone / 2), 0, Player.whoAmI);
                        proj.GetGlobalProjectile<FlaskExplosivesProj>().exploded = true;
                    }
                }
            }
        }
        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (item.DamageType.CountsAsClass<MeleeDamageClass>() && !item.noMelee && !item.noUseGraphic)
            {
                if (flaskFrost)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
                        dust.velocity *= 0.5f;
                        dust.scale = 1.5f;
                        dust.noGravity = true;
                    }
                }
                if (flaskExplosive && !item.GetGlobalItem<FlaskExplosivesItem>().exploded)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 174);
                        dust.velocity *= 0.5f;
                        dust.scale = 1.5f;
                        dust.noGravity = true;

                        Dust dust2 = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 31);
                        dust2.velocity *= 0.5f;
                        dust2.scale = 1.5f;
                        dust2.noGravity = true;
                    }
                }
            }
        }
        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            if ((projectile.DamageType.CountsAsClass<MeleeDamageClass>() || ProjectileID.Sets.IsAWhip[projectile.type]) && !projectile.noEnchantments)
            {
                if (flaskFrost == true)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust dust = Dust.NewDustDirect(boxPosition, boxWidth, boxHeight, 187);
                        dust.velocity *= 0.5f;
                        dust.scale = 1.5f;
                        dust.noGravity = true;
                    }
                }
                if (flaskExplosive == true && !projectile.GetGlobalProjectile<FlaskExplosivesProj>().exploded)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust dust = Dust.NewDustDirect(boxPosition, boxWidth, boxHeight, 174);
                        dust.velocity *= 0.5f;
                        dust.scale = 1.5f;
                        dust.noGravity = true;

                        Dust dust2 = Dust.NewDustDirect(boxPosition, boxWidth, boxHeight, 31);
                        dust2.velocity *= 0.5f;
                        dust2.scale = 1.5f;
                        dust2.noGravity = true;
                    }
                }
            }
        }
    }
    //Make sure explosion only occurs on first hit
    public class FlaskExplosivesItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public bool exploded;

        public override void HoldItem(Item item, Player player) //reset the bool each swing
        {
            if (!player.channel && player.itemAnimation == (player.itemAnimationMax - 1) && player.HeldItem.useTime > 1)
                exploded = false;
        }
    }
    public class FlaskExplosivesProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool exploded;
        int explodetime;
        public override void AI(Projectile projectile)
        {
            if (explodetime > 0) //for channeling projectiles
                explodetime--;
            //Main.NewText("tester" + explodetime, Color.Green);
            var player = Main.player[projectile.owner];
            if (player.channel && explodetime == 0)
            {
                explodetime = player.HeldItem.useAnimation;
                exploded = false;
            }
            base.AI(projectile);
        }
    }
}