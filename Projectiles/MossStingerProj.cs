using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent;
using StormDiversMod.Buffs;
using Terraria.Enums;
using System.Collections.Generic;

namespace StormDiversMod.Projectiles
{
    public class MossStingerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stinger");
            ProjectileID.Sets.DontAttachHideToAlpha[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.arrow = true;
            Projectile.tileCollide = true;
            Projectile.knockBack = 0f;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.extraUpdates = 1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ArmorPenetration = 5;
        }
        //bool sticktarget;
        int stickTimer;
        int sticktime = 300;
        public int TargetWhoAmI
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public bool sticktarget
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }
        Vector2 projoffest;

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

            if (Main.rand.Next(2) == 0 && !sticktarget) // the chance
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 85, 0f, 0f, 0, new Color(255, 255, 255), 0.8f);
                dust.noGravity = true;

            }
            if (sticktarget)
            {
                Projectile.penetrate = 1;
                //Projectile.alpha = 255;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                stickTimer += 1;
                int npcTarget = TargetWhoAmI;
                if (stickTimer >= sticktime || npcTarget < 0 || npcTarget >= 200) 
                {
                    Projectile.Kill();
                }
                else if (Main.npc[npcTarget].active && !Main.npc[npcTarget].dontTakeDamage)
                {
                    // If the target is active and can take damage
                    // Set the projectile's position relative to the target's center
                    Projectile.Center = Main.npc[npcTarget].Center + projoffest - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.npc[npcTarget].gfxOffY;
                }
                else
                { // Otherwise, kill the projectile
                    Projectile.Kill();
                }
            }
        }
        private const int Maxstingers = 15; //15 max stingers, reach in exactly 2 seconds
        private readonly Point[] stickingstingers = new Point[Maxstingers]; 
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 85, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);
            }
            if (!sticktarget)//attach to enemy
            {
                target.AddBuff(ModContent.BuffType<StungDebuff>(), sticktime);
                Projectile.timeLeft = sticktime;
                Projectile.damage = 0; 
                projoffest = new Vector2(Main.rand.NextFloat(-target.width / 6, target.width / 6), Main.rand.NextFloat(-target.height / 6, target.height / 6));
                Projectile.extraUpdates = 0;
                TargetWhoAmI = target.whoAmI; // Set the target whoAmI
                Projectile.velocity = (target.Center - Projectile.Center) * 0.75f; // Change velocity based on delta center of targets (difference between entity centers)
                Projectile.netUpdate = true; // netUpdate this stinger
                sticktarget = true; // we are sticking to a target
            }                
            Projectile.KillOldestJavelin(Projectile.whoAmI, Type, target.whoAmI, stickingstingers);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {

                var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 85);
                dust.noGravity = true;
            }
            if (!sticktarget)
            SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (sticktarget)
            {
                int npcIndex = TargetWhoAmI;
                if (npcIndex >= 0 && npcIndex < 200 && Main.npc[npcIndex].active)
                {
                    if (Main.npc[npcIndex].behindTiles) //this works I guess
                    {
                        Projectile.hide = true;
                        behindNPCsAndTiles.Add(index);
                    }
                    else
                    {
                        Projectile.hide = false;
                        behindNPCsAndTiles.Add(index);
                    }
                    return;
                }
            }
            // Since we aren't attached, add to this list
            behindNPCsAndTiles.Add(index);
        }
    }
}
