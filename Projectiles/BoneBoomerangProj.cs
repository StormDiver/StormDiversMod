using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using StormDiversMod.Basefiles;
using Terraria.Audio;

namespace StormDiversMod.Projectiles
{

    
    public class BoneBoomerangProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bone Boomerang");
        }
        public override void SetDefaults()
        {

            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = 14;
            Projectile.scale = 1f;
            Projectile.CloneDefaults(52);
            AIType = 52;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            DrawOffsetX = 0;
            DrawOriginOffsetY = -6;
        }


        public override void AI()
        {
            

            Projectile.width = 20;
            Projectile.height = 20;

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

           
            SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.Center);



        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
           
            SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.Center);

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {

                //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);



            }



        }
    }
    

}