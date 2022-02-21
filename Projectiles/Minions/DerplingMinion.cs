using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Items.Pets;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.Creative;


namespace StormDiversMod.Projectiles.Minions
{
    public class DerplingMinionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Derpling Minion");
            Description.SetDefault("A buffed baby Derpling will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<DerplingMinionProj>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
    

    //CCOPY SLIME MINION AI
    public class DerplingMinionProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Derpling Minion");
            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[Projectile.type] = 6;
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            // These below are needed for a minion
            // Denotes that this Projectile is a pet or minion
            Main.projPet[Projectile.type] = true;
            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

            
        }

        public sealed override void SetDefaults()
        {
            // Makes the minion go through tiles freely
            Projectile.tileCollide = false;
            // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.friendly = true;
            // Only determines the damage type
            Projectile.minion = true;
            // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.minionSlots = 1f;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.penetrate = -1;
            //Projectile.DamageType = DamageClass.Summon;
            Projectile.CloneDefaults(266);
            AIType = 266;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<DerplingMinionBuff>());
            }
            if (player.HasBuff(BuffType<DerplingMinionBuff>()))
            {
                Projectile.timeLeft = 2;
            }
            Projectile.width = 32;
            Projectile.height = 24;
            Projectile.Opacity = 1;
            DrawOffsetX = -1;
            DrawOriginOffsetY = -6;

            //Projectile.extraUpdates = 1;
            if (!Projectile.tileCollide)
            {
  
                Projectile.extraUpdates = 1;
            }
            else
            {
                Projectile.extraUpdates = 0;
            }
            /*Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;*/
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 2; i++)
                {


                    var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68);
                }
            }
            //NPC.immuneTime = 10;
        }
        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                if (!Main.dedServ)
                {
                    Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                    SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 22);
                    for (int i = 0; i < 15; i++)
                    {


                        var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68);
                    }
                }
            }
        }

    }
}