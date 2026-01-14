using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using StormDiversMod.Buffs;
using StormDiversMod.Common;
using StormDiversMod.NPCs;
using StormDiversMod.NPCs.Boss;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace StormDiversMod.Projectiles
{
    public class VoidSuitcaseProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Void Suitcase");
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            //Projectile.CloneDefaults(509);
            // aiType = 509;
            //Projectile.aiStyle = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = false;
            //Projectile.hide = true; //prevents pre draw from running
            //Projectile.alpha = 255; //use instead

            Projectile.ownerHitCheck = true; //so you can't hit enemies through walls
            Projectile.DamageType = DamageClass.Magic;
            DrawOffsetX = 0;
            DrawOriginOffsetY = 0;
            //Projectile.ContinuouslyUpdateDamage = true;

        }
        public override bool? CanDamage() => true;

        bool maxcharge; //is the max charged reached?
        //float projsize = 0; //increase size
        float soundpitch; //increase pitch of charge sound
        float extravel = 1f; //extra velocity
        float extradamage = 1f; //extra damage
        float manachance; //chance not to consume mana
        bool deathtimer; //start death timer for proj
        int deathtime = 30; // time until proj is removed

        public override void OnSpawn(IEntitySource source)
        {
            var player = Main.player[Projectile.owner];
            //Projectile.damage = player.HeldItem.damage; //starts off at 80, but then ranged buffs are applied after charge, otherwise additon damage is worse at higher charge levels
        }
        public override void AI()
        {
            var player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI; //appear in front of player but behind hand
            if (!deathtimer)
            {
                player.maxRunSpeed = 0;
                Projectile.frameCounter++; //make projectile animate at first
                if (Projectile.frameCounter >= 5)
                {
                    Projectile.frame++;
                    if (Projectile.frame == 5)
                        Projectile.frame = 4;

                    Projectile.frameCounter = 0;
                }
                if (!maxcharge) //charge up, minimum charge is 60, frames = projectile pull in radius
                {
                    Projectile.ai[1]++; //projectile size
                    //projsize += 1f;
                    if (Projectile.ai[1] > 60) // only increase at minimum charge
                    {
                        extravel += 0.005f; //extra 60% speed at max charge
                        extradamage += 0.0075f; //extra 90% damage at max charge
                    }
                }
                if (Projectile.ai[1] >= 180)//Charge up time is 60 + 120 frames
                {
                    maxcharge = true;
                }
                if (!deathtimer && Projectile.frame >= 4)
                {
                    if (Projectile.soundDelay <= 0 && !maxcharge) //Charge up sound and effect
                    {
                        soundpitch += 0.1f;

                        SoundEngine.PlaySound(SoundID.Item130 with { Volume = 2f, Pitch = soundpitch, MaxInstances = 0 }, base.Projectile.position);

                        Projectile.soundDelay = 12;
                    }
                    for (int i = 0; i < 1; i++)
                    {
                        int dust2 = Dust.NewDust(Projectile.Center + new Vector2(-3, -3) + Projectile.velocity * Main.rand.Next(6, 10) * 0.2f, 0, 0, 272, player.velocity.X, player.velocity.Y, 80, default(Color), 0.75f);
                        Main.dust[dust2].noGravity = true;
                        //Main.dust[dust2].velocity *= 0.2f;
                        //Main.dust[dust2].velocity.Y = (float)(-Main.rand.Next(7, 13)) * 0.15f;
                    }
                }
                //Main.NewText("Size " + projsize + " Extra damage = " + extravel, 0, 204, 170); //Inital Scale

                Vector2 muzzleOffset = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 15f; // Position of end of barrel

                if (Collision.CanHit(Projectile.position, 0, 0, Projectile.position + muzzleOffset, 0, 0))
                {
                    Projectile.position += muzzleOffset;
                }
                //=========================mana=====================
                manachance = player.manaCost *= 100;

                player.manaRegenDelay = 60; //prevent mana from regenerating while laser is being fired
                if (player.statMana > 0)
                {
                    if (Main.rand.Next(100) <= manachance)
                    {
                        if (ModLoader.HasMod("TRAEProject"))//bool if TRAE
                        {
                            player.statMana -= 2;
                        }
                        else
                            player.statMana -= 1;
                    }
                }
                if (player.statMana <= 0 || player.dead) //If the player runs out of mana kill the projectile
                {
                    //Projectile.Kill();
                    deathtimer = true;
                }
            }
            //=============================================================================================Code for movement of weapon projectile====================================================================================================================================================
            Vector2 vector13 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                if ((Main.player[Projectile.owner].channel || Projectile.ai[1] < 60) && Projectile.ai[1] < 180) //if released and past 60 frames or if 180 frames is reached
                {
                    float num171 = Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].shootSpeed * Projectile.scale;
                    Vector2 vector14 = vector13;
                    float num172 = (float)Main.mouseX + Main.screenPosition.X - vector14.X;
                    float num173 = (float)Main.mouseY + Main.screenPosition.Y - vector14.Y;
                    if (Main.player[Projectile.owner].gravDir == -1f)
                    {
                        num173 = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector14.Y;
                    }
                    float num174 = (float)Math.Sqrt(num172 * num172 + num173 * num173);
                    num174 = (float)Math.Sqrt(num172 * num172 + num173 * num173);
                    num174 = num171 / num174;
                    num172 *= num174;
                    num173 *= num174;
                    if (num172 != base.Projectile.velocity.X || num173 != base.Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }
                    base.Projectile.velocity.X = num172;
                    base.Projectile.velocity.Y = num173;
                }
                else
                {
                    //Projectile.Kill();
                    deathtimer = true;
                }
            }
            Projectile.spriteDirection = Projectile.direction;
            player.ChangeDir(Projectile.direction);
            //Main.player[Projectile.owner].heldProj = player.whoAmI; //<-- causes issues
            player.SetDummyItemTime(2);
            base.Projectile.position.X = vector13.X - (float)(base.Projectile.width / 2);
            base.Projectile.position.Y = vector13.Y - (float)(base.Projectile.height / 2);
            Projectile.rotation = (float)(Math.Atan2(base.Projectile.velocity.Y, base.Projectile.velocity.X) + 1.5700000524520874);
            if (player.direction == 1)
            {
                player.itemRotation = (float)Math.Atan2(base.Projectile.velocity.Y * (float)Projectile.direction, base.Projectile.velocity.X * (float)Projectile.direction);
            }
            else
            {
                player.itemRotation = (float)Math.Atan2(base.Projectile.velocity.Y * (float)Projectile.direction, base.Projectile.velocity.X * (float)Projectile.direction);
            }
            //base.Projectile.velocity.X *= 1f + (float)Main.rand.Next(-3, 4) * 0.01f;        
            //================================================================================================================================================================================================================================================
            if (deathtimer) //60 frames to close and remove projectile
            {
                deathtime--;
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 6)
                {
                    Projectile.frame--;
                    if (Projectile.frame == -1)
                        Projectile.frame= 0;
                    Projectile.frameCounter = 0;
                }
            }
            if (deathtime <= 0)
                Projectile.Kill();

            //var player = Main.player[Projectile.owner];
            if (deathtimer && deathtime == 29)
            {
                for (int i = 0; i < 1000; i++) //begind death of any exisiting void proj
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<VoidProj>())
                    {
                        Main.projectile[i].timeLeft = 2;
                    }
                }
                if (maxcharge) //Different sound at max charge
                {
                    player.GetModPlayer<MiscFeatures>().screenshaker = true;
                    SoundEngine.PlaySound(SoundID.Item38 with { Volume = 1f, Pitch = 0f }, player.Center);
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Item36 with { Volume = 0.5f, Pitch = 0f }, player.Center);
                }
                //To fire the projs
                if (Collision.CanHit(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, new Vector2(player.Center.X, player.Center.Y), 0, 0))
                {
                    int projToShoot = ModContent.ProjectileType<Projectiles.VoidProj>();

                    int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y - 2), new Vector2((Projectile.velocity.X * 0.5f * extravel), (float)(Projectile.velocity.Y * 0.5f * extravel)),
                        projToShoot, (int)(Projectile.damage * extradamage), Projectile.knockBack, Projectile.owner);

                    Main.projectile[projID].ai[1] = Projectile.ai[1];

                    if (maxcharge) //extra dust for maxcharge
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            float speedY = -1.5f;

                            Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y - 2), Projectile.width, Projectile.height, 272, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, default, 1.5f);
                            Main.dust[dust2].noGravity = true;
                        }
                    }
                }
                Projectile.damage = 0;
            }
        }
        public override void OnKill(int timeLeft)
        {
            //all occurs at 59 frames left for 1 second of closing
        }
        float thescale = 0.1f;
        public override void PostDraw(Color lightColor) //draw samll void on end of barrel
        {
            if (thescale < 0.75f)
            thescale += 0.01f;
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/VoidProj");
            lightColor.A = 200;
            Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
            //for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (!deathtimer && Projectile.frame >= 4)
                {
                    Vector2 drawPos = new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.velocity.X * 1.2f, Projectile.position.Y - Main.screenPosition.Y + Projectile.velocity.Y - 1.2f) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Color.MediumPurple * 0.4f;
                    Main.EntitySpriteDraw(texture, drawPos, null, color, 0, drawOrigin, Projectile.scale * (thescale + 0.005f * Projectile.ai[1]) + Main.rand.NextFloat( -0.03f, 0.04f), SpriteEffects.None, 0);
                }
            }
        }
    }
    //________________________________________________________________________________________________________________________
    public class VoidProj : ModProjectile //weapon charges up, longer charge more damage and larger size
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Test");
            //Main.projFrames[Projectile.type] = 4;
        } 
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.scale = 1f;
            Projectile.light = 0.8f;
            Projectile.ArmorPenetration = 5;
            Projectile.knockBack = 0;
            Projectile.alpha = 0;
            Projectile.timeLeft = 999;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        Vector2 mousepos;
        public override void OnSpawn(IEntitySource source)
        {
            mousepos = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y); //Set position for 1 frame
        }
        int maindistance;
        bool timeleftspawn;
        bool slowdown;
        public override void AI()
        {
            if (Projectile.ai[2] < 5)//prevent it getting stuck if shot from a slope
            Projectile.ai[2]++;
            //int projID = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.velocity,
            //            ModContent.ProjectileType<Projectiles.VoidProj2>(), 0, 0, Projectile.owner);
            //Main.projectile[projID].scale = 0.35f + (Projectile.ai[1] / 60) + Main.rand.NextFloat(-0.01f, 0.02f);
            //Projectile.timeLeft = 100;
            Projectile.scale = 0.5f + (Projectile.ai[1] / 50) + Main.rand.NextFloat(-0.05f, 0.06f);
            if (!timeleftspawn) //putting this in onspawn doesnt work for stupid reasons
            {
                Projectile.timeLeft = 180 + (int)Projectile.ai[1] * 3;
                //Main.NewText("Timeleft= " + Projectile.timeLeft, 0, 204, 170); //Inital Scale
                timeleftspawn = true;
            }

            maindistance = (int)Projectile.ai[1]; //determines size
            Projectile.rotation += 0.50f * -Projectile.direction;
            for (int i = 0; i < maindistance / 25; i++)
            {
                double deg = Main.rand.Next(0, 360); //The degrees
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                float dustx = Projectile.Center.X - (int)(Math.Cos(rad) * maindistance);
                float dusty = Projectile.Center.Y - (int)(Math.Sin(rad) * maindistance);
                if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, Projectile.position, Projectile.width, Projectile.height))//no dust unless line of sight
                {
                    var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 272, 0, 0);
                    dust.noGravity = true;
                    dust.scale = 1;
                }
                if (Main.rand.Next(3) == 0)
                {
                    double dist2 = maindistance / 2; //Distance away from the player
                    float dustx2 = Projectile.Center.X - (int)(Math.Cos(rad) * dist2);
                    float dusty2 = Projectile.Center.Y - (int)(Math.Sin(rad) * dist2);
                    if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, Projectile.position, Projectile.width, Projectile.height))//no dust unless line of sight
                    {
                        var dust = Dust.NewDustDirect(new Vector2(dustx2, dusty2), 1, 1, 272, 0, 0);
                        dust.noGravity = true;
                        dust.scale = 1;
                    }
                }
                Vector2 velocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(dustx, dusty)) * maindistance / 12;
                if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, Projectile.Center, 1, 1))//no dust unless line of sight
                {
                    var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 272, velocity.X, velocity.Y);
                    dust.noGravity = true;
                    dust.velocity *= 1f;
                    dust.scale = 1f;
                    dust.fadeIn = 0.5f;
                }
            }
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if (!target.boss && !target.friendly && target.active && target.lifeMax > 1 && target.type != ModContent.NPCType<DerpMimic>() && !target.dontTakeDamage)
                {
                    float distance = Vector2.Distance(target.Center, Projectile.Center);
                    if (distance < maindistance + 20 && distance > 20 && Collision.CanHitLine(Projectile.Center, 0, 0, target.Center, 1, 1))
                    {
                        float movespeed = 1 * (target.knockBackResist * 4); //less KB resistance = more pull
                        if (movespeed > 3)//cap pull in speed
                            movespeed = 3;
                        //Main.NewText(movespeed, Color.Red);
                        Vector2 moveTo = Projectile.Center;
                        Vector2 move = moveTo - target.Center + new Vector2(0, 0); //Postion around cursor
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);

                        if (magnitude > movespeed)
                        {
                            move *= movespeed / magnitude;
                        }
                        target.velocity = move;
                    }
                }
            }
            /*if (Vector2.Distance(Projectile.Center, mousepos) <= 10)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
            }*/
            var player = Main.player[Projectile.owner];

           // if (player.controlUseTile)
           //     Projectile.Kill();

            if (Projectile.timeLeft <= 2)
            {
                Projectile.timeLeft = 2;
                Projectile.ai[1] -= 4;
                if (Projectile.ai[1] <= 0)
                    Projectile.Kill();
            }
            if (Projectile.soundDelay <= 0) //Charge up sound and effect
            {
                if (Projectile.timeLeft > 2)
                SoundEngine.PlaySound(SoundID.Item165 with { Volume = 0.75f, Pitch = 0.5f, MaxInstances = 0 }, base.Projectile.position);
                else
                    SoundEngine.PlaySound(SoundID.Item165 with { Volume = 1f, Pitch = -0f, MaxInstances = 0 }, base.Projectile.position);

                Projectile.soundDelay = 15;
            }
            if (slowdown)
                Projectile.velocity *= 0.9f;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
            hitbox.Width = maindistance;
            hitbox.Height = maindistance;
            hitbox.X -= maindistance / 2 - (Projectile.width / 2);
            hitbox.Y -= maindistance / 2 - (Projectile.height / 2);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //Projectile.Kill();
            if (target.lifeMax > 5)
            slowdown = true;
            //Projectile.velocity *= 0.1f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[2] >= 5)
            Projectile.velocity *= 0;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 100; i++) 
            {
                Vector2 perturbedSpeed = new Vector2(0, Main.rand.NextFloat(-4f, -3f)).RotatedByRandom(MathHelper.ToRadians(360));

                var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, 272, perturbedSpeed.X, perturbedSpeed.Y);
                dust.scale = 1f;

            }
            SoundEngine.PlaySound(SoundID.Item73 with { Volume = 1f, Pitch = -0.5f, MaxInstances = 0 }, base.Projectile.position);

        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.Purple;
            color.A = 200;
            return color;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Projectiles/VoidProj2");
            lightColor.A = 200;
            Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
            //for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = new Vector2 (Projectile.position.X - Main.screenPosition.X - (maindistance / 2) - (Projectile.width / 3), Projectile.position.Y - Main.screenPosition.Y - (maindistance / 2) - (Projectile.height / 3)) + drawOrigin - new Vector2(maindistance / 50, maindistance / 50);
                Color color = Color.MediumPurple * 0.4f;
                Main.EntitySpriteDraw(texture, drawPos, null, color, 0, drawOrigin, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            }

            return true;
        }
    }
    public class VoidProj2 : ModProjectile //test proj
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Test");
            //Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.scale = 1f;
            Projectile.light = 0.8f;
            Projectile.ArmorPenetration = 5;
            Projectile.knockBack = 0;
            Projectile.tileCollide = true;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            //Projectile.rotation = Main.rand.NextFloat(0, 6.2f); //speen start
        }
        public override void AI()
        {
            Projectile.rotation += 0.50f * -Projectile.direction;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) //expands the hurt box, but hitbox size remains the same
        {
          
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.knockBackResist != 0 && !target.boss)
            {
                float distance = Vector2.Distance(target.Center, Projectile.Center);
                float movespeed = distance / 30 * (target.knockBackResist * 2); //less KB resistance = more pull
                if (movespeed > 3)
                    movespeed = 3;
                Main.NewText(movespeed, Color.Red);
                Vector2 moveTo = Projectile.Center;
                Vector2 move = moveTo - target.Center + new Vector2(0, 0); //Postion around cursor
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);

                if (magnitude > movespeed)
                {
                    move *= movespeed / magnitude;
                }
                target.velocity = move;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Projectile.Kill();
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            Color color = Color.Purple;
            color.A = 150;
            return color;
        }
    }
   
}
