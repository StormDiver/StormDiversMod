using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using StormDiversMod.Common;
using StormDiversMod.Buffs;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.DataStructures;

namespace StormDiversMod.Projectiles
{

    public class LizardSpinnerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lihzahrd Spinner");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {

            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 3600;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.light = 0.4f;
        }
     
        float movespeed = 25f; //Speed of the proj

        Vector2 mousepos;
        bool stopmove;
        public override void AI()
        {
            Projectile.ai[1]++;

            if (Main.rand.Next(5) == 0)
            {
                Vector2 perturbedSpeed = new Vector2(0, -2.5f).RotatedByRandom(MathHelper.ToRadians(360));

                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 170, perturbedSpeed.X, perturbedSpeed.Y, 255, default, 1f);
                Main.dust[dustIndex].noGravity = true;
            }
            Projectile.rotation = Projectile.ai[1] * 0.08f;
            //movement
            if (Projectile.ai[1] == 1)
            {
                mousepos = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y); //Set position for 1 frame
            }

            if (Projectile.ai[1] > 60)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
            }
            float distance = Vector2.Distance(mousepos, Projectile.Center);

            movespeed = distance / 30;
            if (movespeed > 12)//cap speed
                movespeed = 12;
            if (!stopmove)
            {
                Vector2 moveTo = mousepos;
                Vector2 move = moveTo - Projectile.Center + new Vector2(0, 0); //Postion around cursor
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);

                if (magnitude > movespeed)
                {
                    move *= movespeed / magnitude;
                }

                Projectile.velocity = move;
            }
            var player = Main.player[Projectile.owner];

            if (Projectile.timeLeft < 60) //Destroy aniamtion
            {
                Projectile.position = Projectile.Center;
                Projectile.Center = Projectile.position;
                if (Projectile.scale > 0)
                {
                    Projectile.scale -= 0.02f;
                }
                //Projectile.velocity.X = 0;
                //Projectile.velocity.Y = 0;
            }
            /*if (player.ownedProjectileCounts[proj] >= 10 && player.controlUseItem && Projectile.ai[1] > 60)
            {
                Projectile.Kill();
            }*/
            if (player.controlUseTile && Projectile.ai[1] > 60 && player.noThrow == 0 && player.HeldItem.type == ModContent.ItemType<Items.Weapons.LizardSpinner>() || player.dead) 
            {
                if (Projectile.timeLeft > 60)
                {
                    Projectile.timeLeft = 60;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            stopmove = true;
            Projectile.velocity.X *= 0f;
            Projectile.velocity.Y *= 0f;
            //mousepos = new Vector2(target.Center.X, target.Center.Y); //Move to enemy
            Projectile.damage = (Projectile.damage * 9) / 10;

            for (int i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustDirect(target.position, target.width, target.height, 170);
                dust.scale = 1f;
                dust.noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            stopmove = true;
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74 with { Volume = 0.5f, Pitch = 0.5f }, Projectile.Center);

            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 170);
                dust.scale = 1.5f;
                
                dust.noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);

            Texture2D texture2 = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture2.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture2, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }

    }
    //____________________________________________________
    public class LizardFlameProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lihzahrd Flame");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 160;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.scale = 0.1f;
            DrawOffsetX = -35;
            DrawOriginOffsetY = -35;
            Projectile.light = 0.9f;
            Projectile.ArmorPenetration = 20;
        }
        public override bool? CanDamage()
        {
            if (Projectile.alpha < 150 && Projectile.ai[1] == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            //Projectile.rotation = Main.rand.NextFloat(0, 6.2f); //speen start
        }
        int dustoffset;
        int alphaadd; //add alpha to the trail
        int posadd = 5; //adjust trail position
        bool createtrail = true; //trail will stop being created on impact with tiles
        int trailofftime;
        public override void AI()
        {
            Projectile.rotation += 0.05f * -Projectile.direction;
            if (dustoffset > 5)
            {
                if (Main.rand.Next(30) == 0)  //dust spawn sqaure increases with hurtbox size
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X - (dustoffset / 2), Projectile.position.Y - (dustoffset / 2)), Projectile.width + dustoffset, Projectile.height + dustoffset, 174, Projectile.velocity.X * 1.5f, -5, 130, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                }
            }

            if (Projectile.scale <= 1f) //increase size until specified amount
            {
                dustoffset += 1;//makes dust expand with projectile, also used for hitbox

                Projectile.scale += 0.013f;
            }
            else//once the size has been reached begin to fade out and slow down
            {
                Projectile.alpha += 2;
                Projectile.velocity *= 0.96f;

                //begin animation
                if (Projectile.frame < 2)//stop at frame 3
                {
                    Projectile.frameCounter++;
                    if (Projectile.frameCounter >= 50)
                    {
                        Projectile.frame++;
                        Projectile.frameCounter = 0;
                    }
                }
            }
            if (Projectile.alpha > 200) //once faded enough kill projectile
            {
                Projectile.Kill();
            }
            //Trail effect(it works don't judge)
            if (Projectile.ai[1] == 0 && createtrail)
            {
                Projectile.ai[2]++;

                if (Projectile.ai[2] % 6 == 0 && Projectile.ai[2] <= 36) //summon a trail projectile every X frames
                {
                    posadd += 5; //add X times velcity to position each time
                    Vector2 velocity = Projectile.velocity * posadd;

                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(0);
                    alphaadd += 8; //Add alpha so it fades out at the same time
                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X - perturbedSpeed.X, Projectile.Center.Y - perturbedSpeed.Y), Projectile.velocity, ModContent.ProjectileType<LizardFlameProj>(), 0, 0, Projectile.owner);
                    Main.projectile[projID].ai[1] = 1;
                    Main.projectile[projID].alpha += alphaadd;
                }
            }
            if (!createtrail) //trail will stop rendering for a short time after hitting a tile to fix a visual bug
            {
                trailofftime++;
            }
            if (trailofftime > 20)
            {
                createtrail = true;
                trailofftime = 0;
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
            if (Projectile.ai[0] == 0) //
            {
                hitbox.Width = dustoffset;
                hitbox.Height = dustoffset;
                hitbox.X -= dustoffset / 2 - (Projectile.width / 2);
                hitbox.Y -= dustoffset / 2 - (Projectile.height / 2);
            }
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (Projectile.damage * 9) / 10;
            var player = Main.player[Projectile.owner];
            if (Main.rand.Next(1) == 0) // the chance
            {
                target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 300);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 300);
        }
        int reflect = 3;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            createtrail = false; //stop creating trails on the wall for a few frames
            reflect--;
            if (reflect <= 0)
            {
                Projectile.velocity *= 0;
                //Projectile.Kill();
            }
            /*else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X * 1f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 1f;
                }
            }*/
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.Chocolate;
            color.A = (Byte)Projectile.alpha;
            return color;
        }

    }
    //_____________________________________________
    public class LizardSpellProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lihzahrd Fire Orb");
        }
        public override void SetDefaults()
        {

            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            //aiType = ProjectileID.Meteor1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 1;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Main.rand.Next(1) == 0)
            {
                Dust dust;
                Vector2 position = Projectile.position;
                dust = Terraria.Dust.NewDustDirect(position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, 0, new Color(255, 255, 255), .8f);
                dust.noGravity = true;
                dust.scale = 1.5f;
            }
            Projectile.ai[1]++;

            Projectile.rotation = Projectile.ai[1] * 0.2f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            Projectile.damage = (Projectile.damage * 19) / 20;
            target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 300);

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 300);
        }
        int reflect = 5;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            reflect--;
            if (reflect <= 0)
            {
                Projectile.Kill();
            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.8f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
            }
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10 with { Volume = 1f, Pitch = -0.5f }, Projectile.Center);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 25; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 0, default, 2f);
                    Main.dust[dustIndex].noGravity = false;
                    Main.dust[dustIndex].velocity *= 2;
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
    //_________________
    public class LizardArmourProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lihzarhd Bomb");
            Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override bool? CanDamage()
        {
            if (Projectile.timeLeft > 3)
                return false;
            else
                return true;
        }
        int direction;
        int distance = 150;
        float movespeed = 25;
        public override void OnSpawn(IEntitySource source)
        {
            if (Main.rand.Next(2) == 0)
            {
                direction = -10;
            }
            else
            {
                direction = 10;
            }
            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 170, 0f, 0f, 0, new Color(255, 255, 255), 0.75f)];
                dust.noGravity = true;
            }
        }
        NPC npc;
        public override void AI()
        {
            Projectile.rotation += (float)Projectile.direction * -0.2f;

            //distance stays at 120 for 30 frames, then closes in over 90 frames, also has detonation animation for the final 30 frames; 120 timeleft
            if (distance > 0 && Projectile.timeLeft <= 90) //after 30 frames start to close for the rest of the lifetime
                distance -= 2;

            if (Projectile.timeLeft < 30) //When 30 frames are left switch dust
            {
                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 170, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }
            else
            {
                if (Main.rand.Next(2) == 0)
                {
                    Dust dust;
                    Vector2 position = Projectile.Center;
                    dust = Terraria.Dust.NewDustPerfect(position, 170, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
                    dust.noGravity = true;
                }
            }

            Player player = Main.player[Projectile.owner];
            if (player.HasMinionAttackTargetNPC) //rotate targetted npc
            {
                npc = Main.npc[player.MinionAttackTargetNPC];

                //Factors for calculations
                double deg = (((double)Projectile.ai[1]) * direction) + 90; //The degrees, you can multiply Projectile.ai[1] to make it orbit faster, may be choppy depending on the value
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = distance; //Distance away from the npc, 

                float posX = (int)(Math.Cos(rad) * dist);
                float posY = (int)(Math.Sin(rad) * dist);

                //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
                Projectile.ai[1] += 1;

                float projdistance = Vector2.Distance(npc.Center, Projectile.Center); //speed based on how far from npc
                movespeed = projdistance / 5 + 2f;
                if (movespeed > 15)
                    movespeed = 15;

                Vector2 moveTo = npc.Center;
                Vector2 move = moveTo - Projectile.Center + new Vector2(posX, posY); //Postion around npc
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > movespeed)
                {
                    move *= movespeed / magnitude;
                }
                Projectile.velocity = move;
            }
            else
            {
                Projectile.velocity *= 0.9f;
                if (Projectile.timeLeft > 30) //explode if no targets
                    Projectile.timeLeft = 30;
            }

            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 3 frames
                Projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                Projectile.position = Projectile.Center;

                Projectile.width = 160;
                Projectile.height = 160;
                Projectile.Center = Projectile.position;

                Projectile.knockBack = 3f;
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
            }

            Projectile.frameCounter++;
            if (Projectile.timeLeft > 30)
            {
                if (Projectile.frameCounter >= 4)
                {
                    Projectile.frame++;
                    Projectile.frame %= 4; // frame 0-3 normally
                    Projectile.frameCounter = 0;
                }
            }
            else
            {
                if (Projectile.frameCounter >= 3)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame < 4 || Projectile.frame > 5) //frame 4-5 when about to kaboom
                    Projectile.frame = 4;
            }
        }
        public override void OnKill(int timeLeft)
        {
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionGenericProj>(), 0, 0, Projectile.owner);
            Main.projectile[proj].scale = 1.4f;
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {
                Vector2 perturbedSpeed = new Vector2(0, -7.5f).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 170, perturbedSpeed.X, perturbedSpeed.Y);
                dust.noGravity = true;
                dust.scale = 2f;
            }
            for (int i = 0; i < 20; i++) //Grey dust circle
            {
                Vector2 perturbedSpeed = new Vector2(0, -7.5f).RotatedByRandom(MathHelper.ToRadians(360));
                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);

                //dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center, 0, 0, 31, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
                dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
            }
        }
        
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if (!player.HasMinionAttackTargetNPC)
            {
                return true;
            }
            else
            {
                if (Projectile.timeLeft > 3)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        Utils.DrawLine(Main.spriteBatch, new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(Main.npc[player.MinionAttackTargetNPC].Center.X, Main.npc[player.MinionAttackTargetNPC].Center.Y), Color.Gold, Color.Transparent, 3);
                    }
                }
            }
            return true;
        }
    }

}