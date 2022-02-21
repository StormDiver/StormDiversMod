using StormDiversMod.Basefiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Items.Pets
{

    public class DerplingVine : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mysterious Vine");
            Tooltip.SetDefault("Seems to be infused with some strange energy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.width = 16;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 7, 50, 0);
           
            
            Item.shoot = ProjectileType<GoldDerpie>();
            Item.buffType = BuffType<GoldDerpieBuff>();
            Item.rare = ItemRarityID.Yellow;
        }


        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
        
    }

    public class GoldDerpieBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Golden Derpie");
            Description.SetDefault("So shiny and bouncy!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<StormPlayer>().goldDerpie = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<GoldDerpie>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {

                Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), new Vector2(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2)), new Vector2(0, 0), ProjectileType<GoldDerpie>(), 0, 0, player.whoAmI);

            }
        }
    }
    public class GoldDerpie : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Golden Derpie");
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EyeSpring);
            AIType = ProjectileID.EyeSpring;
            Projectile.width = 34;
           Projectile.height = 24;
            DrawOffsetX = -0;
            DrawOriginOffsetY = -8;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            player.eyeSpring = false; 
            return true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            StormPlayer modPlayer = player.GetModPlayer<StormPlayer>();
            if (player.dead)
            {
                modPlayer.goldDerpie = false;
            }
            if (modPlayer.goldDerpie)
            {
                Projectile.timeLeft = 2;
            }
            /* if (dustime >= 5 && !Projectile.tileCollide)

             {
                 int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 19);
                 Main.dust[dust].velocity *= -1f;
                 dustime = 0;
             }*/
            if (!Main.dedServ)
            {
                if (Main.rand.Next(15) == 0)
                {
                    {
                        var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 124);

                        dust.scale = 0.5f;
                        dust.velocity *= 0;

                    }
                }
            }

        }
    }
}

