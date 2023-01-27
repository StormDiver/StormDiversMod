using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.NPCs;
using StormDiversMod.Projectiles;

using Terraria.DataStructures;
using Terraria.Audio;


namespace StormDiversMod.Basefiles
{

    public class ArmourSetBonuses : ModPlayer
    {
        public bool derpJump; //Derpling armour

        public bool BloodDrop; //Hemo Armour

        public bool spaceRockOffence; //Asteroid armour with helmet 

        public bool spaceRockDefence; //Asteroid armour with mask 

        public bool mushset; //Mushroom armour

        public bool hellSoulSet; //HellSoul armour

        public bool twilightSet; //Twilight armour

        public bool skyKnightSet; //SkyKnight Armour

        public bool santankSet; //Santank armour

        public bool aridCritSet; //Arid armour

        public bool cryoSet; //Cryogenic armour


        public bool shadowflameSet; //Shadowflame armour 

        //Ints and Bools activated from this file

        public int bloodtime; //Cooldown for the orbs from the Hemo Armour set bonus     
        public int spaceStrikecooldown; //Cooldown for the Offensive Space Armour set bonus
        public int spaceBarriercooldown; //Cooldown for the Defensive Space Armour set bonus     
        public int hellblazetime; //Cooldown for the flames created from HellSoul armour set
        public int mushtime; //Cooldown for mushrooms summoned with mushroom armour
        public int hellsoultime; //Cooldown for the souls created by hell soul armour     
        public bool twilightcharged; //Activates when the player is able to teleport with the twilight armour
        public int derplinglaunchcooldown; //How long until the player can launch enemies in the air with the Derpling armour set
        public bool skysentry; //Has the Sky Knight sentry been summoned>
        public int santankcharge; //Charging up the santank missle
        public int santankmissleup; //Adds one to the charge every 10 frames
        public bool santanktrigger; //Has the player triggered the missiles
        public int cryosetcooldown; //Colldown for Cryo set bonus

        public override void ResetEffects() //Resets bools if the item is unequipped
        {
            derpJump = false;
            BloodDrop = false;
            spaceRockOffence = false;
            spaceRockDefence = false;
            hellSoulSet = false;
            mushset = false;
            twilightSet = false;
            skyKnightSet = false;
            santankSet = false;
            aridCritSet = false;
            cryoSet = false;
            shadowflameSet = false;
        }
        public override void UpdateDead()//Reset all ints and bools if dead======================
        {
            bloodtime = 0;
            spaceStrikecooldown = 0;
            spaceBarriercooldown = 0;
            hellblazetime = 0;
            mushtime = 0;
            twilightcharged = false;
            derplinglaunchcooldown = 0;
            skysentry = false;
            santankcharge = 0;
            santankmissleup = 0;
            santanktrigger = false;
            cryosetcooldown = 0;
        }

        //===============================================================================================================
     
        public override void PostUpdateEquips() //Updates every frame
        {
            //Increase ints if they are below the limit and armour is equipped and not in the equip field
          
            if (BloodDrop)
            {
                if (bloodtime < 300) //HemoGolbin cooldown
                {
                    bloodtime++;
                }
                if (bloodtime >= 300)//Gives buff when cooldown is over
                {
                    Player.AddBuff(ModContent.BuffType<BloodBurstBuff>(), 2);
                }
            }
            else
            {
                Player.ClearBuff(ModContent.BuffType<BloodBurstBuff>());

                bloodtime = 0;
            }
            if (mushset && mushtime < 90) //Mushroom cooldown
            {
                mushtime++;
            }
            if (!mushset)
            {
                mushtime = 0;
            }
            if (spaceRockDefence)
            {
                if (spaceBarriercooldown < 360) //Asteroid Armour Defence
                {
                    spaceBarriercooldown++;
                }
                if (spaceBarriercooldown >= 360)
                {
                    Player.AddBuff(ModContent.BuffType<SpaceRockDefence>(), 2);

                }
            }
            else
            {
                Player.ClearBuff(ModContent.BuffType<SpaceRockDefence>());
                spaceBarriercooldown = 0;
            }

            if (spaceRockOffence)
            {
                if (spaceStrikecooldown < 180) //Asteroid Armour Offence
                {
                    spaceStrikecooldown++;
                }
                if (spaceStrikecooldown >= 180)
                {
                    Player.AddBuff(ModContent.BuffType<SpaceRockOffence>(), 2);

                }
            }
            else
            {
                Player.ClearBuff(ModContent.BuffType<SpaceRockOffence>());
                spaceStrikecooldown = 0;
            }



            //For santank set ======================================================================
            if (santankSet)
            {
                if (santankcharge <= 100 && !santanktrigger) //Charges up the rockets
                {
                    santankmissleup++;
                }
                if (santankmissleup >= 3) //Adds 1 int to the charge every n frames
                {
                    santankcharge++;
                    santankmissleup = 0;
                }
                if (StormDiversMod.ArmourSpecialHotkey.JustPressed && santankcharge >= 10 && !santanktrigger) //Activates when player presses button
                {
                    santanktrigger = true;

                }
                if (santanktrigger) //Drains the rocket charge and clears buffs
                {
                    santankcharge--;
                    Player.ClearBuff(ModContent.BuffType<SantankBuff1>());
                    Player.ClearBuff(ModContent.BuffType<SantankBuff2>());
                    Player.ClearBuff(ModContent.BuffType<SantankBuff3>());
                }
                if (!santanktrigger && santankmissleup == 0 && (santankcharge == 10 || santankcharge == 20 || santankcharge == 30 || santankcharge == 40 || santankcharge == 50 || santankcharge == 60 || santankcharge == 70 || santankcharge == 80 || santankcharge == 90 || santankcharge == 100))
                //Creates a particle and sound effect at these times, times by 3 to get excat frame
                {
                    for (int i = 0; i < 30; i++)
                    {
                        if (Player.gravDir == 1)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(Player.Center.X - (15 * Player.direction) - 4, Player.Center.Y - 8), 0, 0, 6, 0f, 0f, 50, default, 1.5f);
                            Main.dust[dustIndex].velocity *= 2;
                            Main.dust[dustIndex].noGravity = true;
                        }
                        else
                        {
                            int dustIndex = Dust.NewDust(new Vector2(Player.Center.X - (15 * Player.direction) - 4, Player.Center.Y + 8), 0, 0, 6, 0f, 0f, 50, default, 1.5f);
                            Main.dust[dustIndex].velocity *= 2;
                            Main.dust[dustIndex].noGravity = true;
                        }
                    }
                    SoundEngine.PlaySound(SoundID.Item61 with { Volume = 0.5f, Pitch = 0.5f }, Player.Center);
                }
                else if (santanktrigger && (santankcharge == 10 || santankcharge == 20 || santankcharge == 30 || santankcharge == 40 || santankcharge == 50 || santankcharge == 60 || santankcharge == 70 || santankcharge == 80 || santankcharge == 90 || santankcharge == 100))
                //Fires missles at these times
                {
                    for (int i = 0; i < 30; i++)
                    {
                        if (Player.gravDir == 1)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(Player.Center.X - (15 * Player.direction) - 4, Player.Center.Y - 8), 0, 0, 6, 0f, 0f, 50, default, 1.5f);
                            Main.dust[dustIndex].velocity *= 2;
                            Main.dust[dustIndex].noGravity = true;
                        }
                        else
                        {
                            int dustIndex = Dust.NewDust(new Vector2(Player.Center.X - (15 * Player.direction) - 4, Player.Center.Y + 8), 0, 0, 6, 0f, 0f, 50, default, 1.5f);
                            Main.dust[dustIndex].velocity *= 2;
                            Main.dust[dustIndex].noGravity = true;
                        }
                    }
                    for (int i = 0; i < 25; i++)
                    {
                        if (Player.gravDir == 1)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(Player.Center.X - (15 * Player.direction) - 4, Player.Center.Y - 8), 0, 0, 31, 0, -3, 100, default, 1f);
                            Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                            Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                            Main.dust[dustIndex].noGravity = true;
                        }
                        else
                        {
                            int dustIndex = Dust.NewDust(new Vector2(Player.Center.X - (15 * Player.direction) - 4, Player.Center.Y + 8), 0, 0, 31, 0, +3, 100, default, 1f);
                            Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                            Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                            Main.dust[dustIndex].noGravity = true;
                        }
                    }
                    SoundEngine.PlaySound(SoundID.Item92, Player.Center);


                    float speedX = 0f;
                    float speedY = -8f;
                    //int damage = 200;
                    int santankdamage = (int)Player.GetTotalDamage(DamageClass.Ranged).ApplyTo(160); //208 with santank buffs alone
                    for (int i = 0; i < 1; i++) //1 rockets per charge
                    {
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(50));
                        float scale = 1f - (Main.rand.NextFloat() * .1f);
                        perturbedSpeed = perturbedSpeed * scale;
                        if (Player.gravDir == 1)
                        {
                            Projectile.NewProjectile(null, new Vector2(Player.Center.X - (15 * Player.direction), Player.Center.Y - 6), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<SantankMissleProj>(), santankdamage, 1f, Player.whoAmI);
                        }
                        else
                        {
                            Projectile.NewProjectile(null, new Vector2(Player.Center.X - (15 * Player.direction), Player.Center.Y + 6), new Vector2(perturbedSpeed.X, -perturbedSpeed.Y), ModContent.ProjectileType<SantankMissleProj>(), santankdamage, 1f, Player.whoAmI);

                        }
                    }
                }

                if (santankcharge <= -10) //Reset trigger once all missile are fired (negative creates a recharge delay, delay is 1 = 3, so 10 = 30 frames, plus the 10 * 3 (30frame) delay for the first charge (60 frames, 1 second))
                {
                    santanktrigger = false;

                }
                //For the buffs, Tier 1 = 1-4 rockets, Tier 2 = 5-9, Tier 3 = 10
                if (santankcharge >= 10 && santankcharge < 50 && !santanktrigger)
                {
                    Player.ClearBuff(ModContent.BuffType<SantankBuff2>());
                    Player.ClearBuff(ModContent.BuffType<SantankBuff3>());

                    Player.AddBuff(ModContent.BuffType<SantankBuff1>(), 2);

                }
                else if (santankcharge >= 50 && santankcharge < 100 && !santanktrigger)
                {
                    Player.ClearBuff(ModContent.BuffType<SantankBuff1>());
                    Player.ClearBuff(ModContent.BuffType<SantankBuff3>());

                    Player.AddBuff(ModContent.BuffType<SantankBuff2>(), 2);
                }
                else if (santankcharge >= 100 && !santanktrigger)
                {
                    Player.ClearBuff(ModContent.BuffType<SantankBuff1>());
                    Player.ClearBuff(ModContent.BuffType<SantankBuff2>());

                    Player.AddBuff(ModContent.BuffType<SantankBuff3>(), 2);
                }
                else
                {
                    Player.ClearBuff(ModContent.BuffType<SantankBuff1>());
                    Player.ClearBuff(ModContent.BuffType<SantankBuff2>());
                    Player.ClearBuff(ModContent.BuffType<SantankBuff3>());

                }

            }
            if (!santankSet) //Reset values if armour is removed
            {
                santankcharge = -10;
                santankmissleup = 0;
                santanktrigger = false;
            }

            //For Twilight Armour ====================================================================

            float xWarplimit = 640;
            float yWarplimit = 400;
            if (twilightSet)
            {
             
                float distanceX = Player.Center.X - Main.MouseWorld.X;
                float distanceY = Player.Center.Y - Main.MouseWorld.Y;
                float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));

                int xcursor = (int)(Main.MouseWorld.X / 16);
                int ycursor = (int)(Main.MouseWorld.Y / 16);
                Tile tile = Main.tile[xcursor, ycursor];
                if ((tile != null && !tile.HasTile || !Main.tileSolid[tile.TileType]) && !Player.HasBuff(ModContent.BuffType<TwilightDebuff>())) //Checks if mouse is in valid postion
                {
                    if (((distanceX < -xWarplimit || distanceX > xWarplimit || distanceY < -yWarplimit || distanceY > yWarplimit) && Collision.CanHitLine(Main.MouseWorld, 1, 1, Player.position, Player.width, Player.height)) ||
                        (distanceX > -xWarplimit && distanceX < xWarplimit && distanceY > -yWarplimit && distanceY < yWarplimit)) //If there is no line of sight and cursor is past limit, don't allow teleport to prevent gettign stuck in blocks
                    {


                        twilightcharged = true; //Activates the outline effect on the armour

                        if (StormDiversMod.ArmourSpecialHotkey.JustPressed) //Activates when player presses button
                        {
                            Player.AddBuff(ModContent.BuffType<TwilightDebuff>(), 480);
                            //Player.AddBuff(BuffID.Obstructed, 10);

                            Player.grappling[0] = -1; //Remove grapple hooks
                            Player.grapCount = 0;
                            for (int p = 0; p < 1000; p++)
                            {
                                if (Main.projectile[p].active && Main.projectile[p].owner == Player.whoAmI && Main.projectile[p].aiStyle == 7)
                                {
                                    Main.projectile[p].Kill();
                                }
                            }

                            for (int i = 0; i < 30; i++) //Dust pre-teleport
                            {
                                var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 62);
                                dust.scale = 1.1f;
                                dust.velocity *= 2;
                                dust.noGravity = true;
                                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;

                            }
                            for (int i = 0; i < 30; i++)
                            {
                                var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 179);
                                dust.scale = 1.5f;
                                dust.noGravity = true;
                                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;


                            }
                            //effects to cover up teleport
                            //Player.teleportTime = 0.1f;
                            /*NPC.ResetNetOffsets();
                            Main.BlackFadeIn = 255;                   
                            Main.instantBGTransitionCounter = 10;*/

                            Main.SetCameraLerp(0.1f, 0);//Smooth camera movement, this was it :clayman:
                           
                            //X postion 
                            {
                                if (distanceX <= xWarplimit && distanceX >= -xWarplimit)
                                {
                                    Player.position.X = Main.MouseWorld.X - (Player.width / 2);
                                    //Main.NewText("Little mouse X", 0, 146, 0);
                                }
                                else
                                {
                                    if (distanceX < -xWarplimit)
                                    {
                                        Player.position.X = (Main.MouseWorld.X - (Player.width / 2)) + (distanceX + xWarplimit);
                                        //Main.NewText("Mouse it to the right", 146, 0, 0);
                                    }
                                    else if (distanceX > xWarplimit)
                                    {
                                        Player.position.X = (Main.MouseWorld.X - (Player.width / 2)) + (distanceX - xWarplimit);
                                        //Main.NewText("Mouse it to the left", 146, 0, 0);
                                    }
                                }
                            }
                            //Y postion 
                            {
                                if (distanceY <= yWarplimit && distanceY >= -yWarplimit)
                                {
                                    Player.position.Y = Main.MouseWorld.Y - (Player.height);
                                    //Main.NewText("Little mouse Y", 0, 146, 0);
                                }
                                else
                                {
                                    if (distanceY < -yWarplimit)
                                    {
                                        Player.position.Y = (Main.MouseWorld.Y - (Player.height)) + (distanceY + yWarplimit);
                                        //Main.NewText("Mouse it to the down", 0, 0, 146);
                                    }
                                    else if (distanceY > yWarplimit)
                                    {
                                        Player.position.Y = (Main.MouseWorld.Y - (Player.height)) + (distanceY - yWarplimit);
                                        //Main.NewText("Mouse it to the up", 0, 0, 146);
                                    }
                                }
                            }
                            Main.screenPosition = Main.screenLastPosition;


                            for (int i = 0; i < 30; i++) //Dust post-teleport
                            {
                                var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 62);
                                dust.scale = 1.1f;
                                dust.velocity *= 2;
                                dust.noGravity = true;
                                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;

                            }
                            for (int i = 0; i < 30; i++)
                            {
                                var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 179);
                                dust.scale = 1.5f;
                                dust.noGravity = true;
                                dust.fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;

                            }
                            SoundEngine.PlaySound(SoundID.Item8 with { Volume = 2f, Pitch = -0.5f, MaxInstances = -1 }, Player.Center);

                        }
                    }
                    else
                    {
                        twilightcharged = false;

                    }
                }

                else
                {
                    twilightcharged = false; //Removes the outline effect if the player is unable to charge
                }
            }

            //for Derpling armour

            if (derpJump)
            {
                if (derplinglaunchcooldown < 60)
                {
                    derplinglaunchcooldown++;
                }
           
                Player.jumpSpeedBoost += 4.5f;

                Player.autoJump = true;
                Player.maxFallSpeed *= 1.5f;
                //Creates the wave upon jumping
                if (Player.velocity.Y == 0 && derplinglaunchcooldown >= 60)
                {
                    Player.AddBuff(ModContent.BuffType<DerpBuff>(), 2);

                    if (Player.controlJump)
                    {

                        Player.ClearBuff(ModContent.BuffType<DerpBuff>());

                        SoundEngine.PlaySound(SoundID.NPCHit22 with { Volume = 1.5f, Pitch = -0.5f }, Player.Center);

                        for (int i = 0; i < 40; i++)
                        {
                            if (Player.gravDir == 1)
                            {
                                float speedX = Main.rand.NextFloat(-2f, 2f);
                                var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 68, speedX, -3, 130, default, 1.5f);
                                dust.noGravity = true;
                                dust.velocity *= 2;
                            }
                            else
                            {
                                float speedX = Main.rand.NextFloat(-2f, 2f);
                                var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 68, speedX, +3, 130, default, 1.5f);
                                dust.noGravity = true;
                                dust.velocity *= 2;
                            }

                        }
                        for (int i = 0; i < 20; i++)
                        {

                            var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 68, -5, 0, 130, default, 1.5f);
                            dust.noGravity = true;
                            dust.velocity *= 2;
                            var dust2 = Dust.NewDustDirect(Player.position, Player.width, Player.height, 68, 5, 0, 130, default, 1.5f);
                            dust2.noGravity = true;
                            dust2.velocity *= 2;
                        }
                        if (Player.gravDir == 1)
                        {
                            Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Right.Y - 20), new Vector2(7, 0), ModContent.ProjectileType<DerpWaveProj>(), 75, 0, Player.whoAmI);
                            Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Left.Y - 20), new Vector2(-7, 0), ModContent.ProjectileType<DerpWaveProj>(), 75, 0, Player.whoAmI);
                        }
                        else
                        {
                            Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Right.Y + 20), new Vector2(7, 0), ModContent.ProjectileType<DerpWaveProj>(), 75, 0, Player.whoAmI);
                            Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Left.Y + 20), new Vector2(-7, 0), ModContent.ProjectileType<DerpWaveProj>(), 75, 0, Player.whoAmI);
                        }
                        derplinglaunchcooldown = 0;
                    }
                }
            }
            if (!derpJump)
            {
                derplinglaunchcooldown = 0;
            }



            //For the Sky Knight set
            if (skyKnightSet)
            {
                Player.AddBuff(ModContent.BuffType<SkyKnightSentryBuff>(), 2);

                if (!skysentry)
                {
                    Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.SentryProjs.SkyKnightSentryProj>(), 0, 0, Player.whoAmI);

                    skysentry = true;

                }
            }
            if (!skyKnightSet)
            {
                skysentry = false;
                Player.ClearBuff(ModContent.BuffType<SkyKnightSentryBuff>());

            }
            //For Hellsoul Armour
            if (hellSoulSet)
            {
                if (hellblazetime < 600) //hellSoul cooldown
                {
                    hellblazetime++;
                    if (Main.rand.Next(600) < hellblazetime)
                    {
                        var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 173, 0, -4);
                        dust.scale = 1.25f;
                    }
                }
                if (hellblazetime == 599)
                {
                    SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.75f, Pitch = 0.5f }, Player.Center);

                }
                if (hellblazetime == 600)
                {
                    Player.AddBuff(ModContent.BuffType<HellSoulBuff>(), 2);

                    if (StormDiversMod.ArmourSpecialHotkey.JustPressed)
                    {
                        int hellblazedmg = 1200;

                        for (int i = 0; i < 200; i++)
                        {
                            NPC target = Main.npc[i];
                            float distanceX = Player.Center.X - target.Center.X;
                            float distanceY = Player.Center.Y - target.Center.Y;
                            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
                            bool lineOfSight = Collision.CanHitLine(target.position, target.width, target.height, Player.position, Player.width, Player.height);
                            if (!target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.CanBeChasedBy() && target.type != NPCID.TargetDummy && StormDiversMod.ArmourSpecialHotkey.JustPressed)
                            {
                                if ((distance < 500 && lineOfSight) || (distance < 200 && !lineOfSight))
                                {
                                    if (hellblazedmg > 0) //only summon projectile if enemy will take damage
                                    {
                                        Projectile.NewProjectile(null, target.Center, new Vector2(0, 0), ModContent.ProjectileType<HellSoulArmourProj>(), hellblazedmg, 0, Player.whoAmI);
                                        hellblazedmg = (hellblazedmg -= 80); //80 damage falloff per enemy, hits up to 15 enemies
                                    }
                                    //hellblazedmg = (hellblazedmg * 17) / 20; //15% damage falloff per enemy

                                }
                            }
                        }
                        Projectile.NewProjectile(null, Player.Center, new Vector2(0, 0), ModContent.ProjectileType<HellSoulArmourProj>(), 0, 0, Player.whoAmI);

                        if (!GetInstance<ConfigurationsIndividual>().NoShake)
                        {
                            Player.GetModPlayer<MiscFeatures>().screenshaker = true;
                        }

                        for (int i = 0; i < 50; i++)
                        {
                            var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 173);
                            dust.scale = 1.5f;
                            dust.velocity *= 13;

                        }
                        for (int i = 0; i < 25; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(0, -5f).RotatedByRandom(MathHelper.ToRadians(360));
                            var dust = Dust.NewDustDirect(Player.Center, 0, 0, 173, perturbedSpeed.X, perturbedSpeed.Y);
                            dust.scale = 3f;
                            dust.velocity *= 2f;

                        }
                        for (int i = 0; i < 75; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(0, -20f).RotatedByRandom(MathHelper.ToRadians(360));
                            var dust = Dust.NewDustDirect(Player.Center, 0, 0, 173, perturbedSpeed.X, perturbedSpeed.Y);
                            dust.scale = 3f;
                            dust.velocity *= 2f;

                        }
                        for (int i = 0; i < 125; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(0, -35f).RotatedByRandom(MathHelper.ToRadians(360));
                            var dust = Dust.NewDustDirect(Player.Center, 0, 0, 173, perturbedSpeed.X, perturbedSpeed.Y);
                            dust.scale = 3f;
                            dust.velocity *= 2f;

                        }
                        for (int i = 0; i < 200; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(0, -50f).RotatedByRandom(MathHelper.ToRadians(360));
                            var dust = Dust.NewDustDirect(Player.Center, 0, 0, 173, perturbedSpeed.X, perturbedSpeed.Y);
                            dust.scale = 3f;
                            dust.velocity *= 2f;

                        }
                        SoundEngine.PlaySound(SoundID.Item74 with { Volume = 2f, Pitch = 0.5f }, Player.Center);
                        SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 0.5f }, Player.Center);

                        hellblazetime = 0;
                    }
                }
            }
            if (!hellSoulSet)
            {
                hellblazetime = 0;
            }
            if (cryoSet)
            {
                if (cryosetcooldown < 300)//count up when set nonus is not active
                {
                    cryosetcooldown++;
                }
                int xcursor = (int)(Main.MouseWorld.X / 16);
                int ycursor = (int)(Main.MouseWorld.Y / 16);
                Tile tile = Main.tile[xcursor, ycursor];
                if ((tile != null && !tile.HasTile || !Main.tileSolid[tile.TileType]) && StormDiversMod.ArmourSpecialHotkey.JustPressed && cryosetcooldown >= 300 && Collision.CanHitLine(Main.MouseWorld, 1, 1, Player.position, Player.width, Player.height)) //Activate set bonus
                {
                    int cryodamage = (int)Player.GetTotalDamage(DamageClass.Summon).ApplyTo(30); //36 with cryoset buffs alone

                    Projectile.NewProjectile(null, Main.MouseWorld, new Vector2(0, 0), ModContent.ProjectileType<FrostCryoArmourProj>(), cryodamage, 0, Player.whoAmI);
                    //Kills oldest projectile when new is summoned
                    int cryoprojs = 0;
                    int oldestProjIndex = -1;
                    int oldestProjTimeLeft = 100000;
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == Player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<FrostCryoArmourProj>())
                        {
                            cryoprojs++;
                            if (Main.projectile[i].timeLeft < oldestProjTimeLeft)
                            {
                                oldestProjIndex = i;
                                oldestProjTimeLeft = Main.projectile[i].timeLeft;
                            }
                        }

                    }
                    if (cryoprojs > 1)
                    {
                        Main.projectile[oldestProjIndex].timeLeft = 1;
                    }
                    SoundEngine.PlaySound(SoundID.NPCDeath56 with { Volume = 0.4f, Pitch = -0.5f }, Player.Center);

                    for (int i = 0; i < 50; i++)
                    {
                        var dust = Dust.NewDustDirect(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 180, 0, -2);
                        dust.scale = 1.5f;
                        dust.noGravity = true;
                    }
                    int proj = Projectile.NewProjectile(null, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionFrostProj>(), 0, 0, Player.whoAmI);
                    Main.projectile[proj].scale = 1.25f;
                    int proj2 = Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionFrostProj>(), 0, 0, Player.whoAmI);
                    Main.projectile[proj2].scale = .9f;
                    cryosetcooldown = 0;
                }         
            }
            if (!cryoSet)
            {
                cryosetcooldown = 0;
            }
        }
        //=====================For attacking an enemy with anything===========================================
        public override void OnHitAnything(float x, float y, Entity victim)
        {
            //int mushdamage = 20; //Looks like you didn't deal mush damage with this 
            int mushdamage = (int)Player.GetTotalDamage(DamageClass.Ranged).ApplyTo(18); //19 with shroom buffs
            if (mushset && mushtime >= 90)
            {
                if (Main.rand.Next(2) == 0)
                {
                    Projectile.NewProjectile(null, new Vector2(victim.Center.X - 100, victim.Center.Y - 100), new Vector2(12, 12), ModContent.ProjectileType<MagicMushArmourProj>(), mushdamage, 0, Player.whoAmI);

                }
                else
                {
                    Projectile.NewProjectile(null, new Vector2(victim.Center.X + 100, victim.Center.Y - 100), new Vector2(-12, 12), ModContent.ProjectileType<MagicMushArmourProj>(), mushdamage, 0, Player.whoAmI);

                }

                mushtime = 0;
            }
            //For the SpaceArmour with the helmet (offence)
            //int offencedmg = 150;
            int offencedmg = (int)Player.GetTotalDamage(DamageClass.Generic).ApplyTo(120); //153 with asteroid buffs alone
            int offenceknb = 5;
            float offenceveloX = victim.velocity.X * 0.6f;

            if (spaceRockOffence && Player.HasBuff(ModContent.BuffType<SpaceRockOffence>()))
            {
                if (!GetInstance<ConfigurationsIndividual>().NoShake)
                {
                    Player.GetModPlayer<MiscFeatures>().screenshaker = true;
                }

                Projectile.NewProjectile(null, new Vector2(victim.Center.X - 0, victim.Center.Y - 350), new Vector2(0 + offenceveloX, 10f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned directly above and goes straight down
                Projectile.NewProjectile(null, new Vector2(victim.Center.X - 50, victim.Center.Y - 450), new Vector2(0 + offenceveloX, 10f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned slightly left and moves straight down
                Projectile.NewProjectile(null, new Vector2(victim.Center.X + 50, victim.Center.Y - 450), new Vector2(0 + offenceveloX, 10f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned slightly right and moves straight down
                Projectile.NewProjectile(null, new Vector2(victim.Center.X - 150, victim.Center.Y - 500), new Vector2(2 + offenceveloX, 10f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned to the left and moves left
                Projectile.NewProjectile(null, new Vector2(victim.Center.X + 150, victim.Center.Y - 500), new Vector2(-2 + offenceveloX, 10f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned to the right and moves right
                Projectile.NewProjectile(null, new Vector2(victim.Center.X - 200, victim.Center.Y - 450), new Vector2(4 + offenceveloX, 8f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned further to the left and moves right
                Projectile.NewProjectile(null, new Vector2(victim.Center.X + 200, victim.Center.Y - 450), new Vector2(-4 + offenceveloX, 8f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned further to the right and moves left

                for (int i = 0; i < 30; i++)
                {

                    float speedX = Main.rand.NextFloat(-5f, 5f);
                    float speedY = Main.rand.NextFloat(-5f, 5f);
                    var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 6, speedX, speedY, 130, default, 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 2;

                }
                SoundEngine.PlaySound(SoundID.Item45 with { Volume = 0.5f }, Player.Center);
                spaceStrikecooldown = 0;
                Player.ClearBuff(ModContent.BuffType<SpaceRockOffence>());

            }

            //For the Hemogoblin armour setbonus ======================

            if (Player.HeldItem.CountsAsClass(DamageClass.Melee))
            {
                if (BloodDrop)
                {

                    if (bloodtime >= 300 && !Player.dead)
                    {

                        SoundEngine.PlaySound(SoundID.NPCHit9, Player.Center);

                        float numberProjectiles = 10 + Main.rand.Next(5);

                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            float rotation = Player.itemRotation + (Player.direction == -1 ? (float)Math.PI : 0); //the direction the item points in

                            float speedX = 0f;
                            float speedY = -6f;
                            int blooddamage = (int)(Player.HeldItem.damage * 0.9f);
                            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(135));
                            float scale = 1f - (Main.rand.NextFloat() * .5f);
                            perturbedSpeed = perturbedSpeed * scale;
                            if (Player.gravDir == 1)
                            {
                                Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BloodDropProj>(), blooddamage, 1, Player.whoAmI);
                            }
                            else
                            {
                                Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(perturbedSpeed.X, -perturbedSpeed.Y), ModContent.ProjectileType<BloodDropProj>(), blooddamage, 1, Player.whoAmI);

                            }
                            bloodtime = 0;
                        }
                    }
                }
            }



            base.OnHitAnything(x, y, victim);
        }

        //=====================For taking damage from any source===========================================

        int attackdmg = 0;//This is for how much damage the player takes
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
          
            attackdmg = (int)damage; //Int for the damage taken

            //For Space Armour with Mask (Defence)
            int defencedmg = 100 + (attackdmg * 2); //Boulder damage
            if (defencedmg > 500)
            {
                defencedmg = 500;
            }
            int defenceknb = 6; //Boulder Knockback
            float defenceVeloX = Player.velocity.X * 0.25f;

            if (spaceRockDefence && Player.HasBuff(ModContent.BuffType<SpaceRockDefence>()) && damage >= 2)
            {
                if (!GetInstance<ConfigurationsIndividual>().NoShake)
                {
                    Player.GetModPlayer<MiscFeatures>().screenshaker = true;
                }

                Projectile.NewProjectile(null, new Vector2(Player.Center.X + 0, Player.Top.Y - 350), new Vector2(0 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned above and goes straight down
                Projectile.NewProjectile(null, new Vector2(Player.Center.X - 0, Player.Top.Y - 400), new Vector2(-1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned above and moves slighly left
                Projectile.NewProjectile(null, new Vector2(Player.Center.X + 0, Player.Top.Y - 400), new Vector2(1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned above and moves slightly right
                Projectile.NewProjectile(null, new Vector2(Player.Center.X - 60, Player.Top.Y - 450), new Vector2(-1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned to the left and moves  left
                Projectile.NewProjectile(null, new Vector2(Player.Center.X + 60, Player.Top.Y - 450), new Vector2(1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned to the right and moves  right
                Projectile.NewProjectile(null, new Vector2(Player.Center.X - 120, Player.Top.Y - 500), new Vector2(-1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI);  //Summoned far to the left and moves left
                Projectile.NewProjectile(null, new Vector2(Player.Center.X + 120, Player.Top.Y - 500), new Vector2(1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned far to the right and moves right

                for (int i = 0; i < 30; i++)
                {

                    float speedX = Main.rand.NextFloat(-5f, 5f);
                    float speedY = Main.rand.NextFloat(-5f, 5f);
                    var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 6, speedX, speedY, 130, default, 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 2;
                }
                SoundEngine.PlaySound(SoundID.Item45 with { Volume = 0.5f }, Player.Center);

                spaceBarriercooldown = 0;
                Player.ClearBuff(ModContent.BuffType<SpaceRockDefence>());

            }



        }
        //===================================Other hooks======================================

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) //Hitting enemies with True Melee Only
        {

            //For the Soul Fire armour setbonus with true melee
            /*if (hellSoulSet && hellblazetime == 0)
            {
                /*float numberProjectiles = 9;

                float rotation = MathHelper.ToRadians(140);
                //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    float speedX = 8f;
                    float speedY = 0f;
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles) - 0.27f));
                    Projectile.NewProjectile(target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("HellSoulArmourProj"), hellsouldmg, 0, Player.whoAmI);
                }*/

               /* int helldamagemelee = (int)Player.GetTotalDamage(DamageClass.Generic).ApplyTo(80); //89 with armour buffs

                float speedX = 0f;
                float speedY = -8f;

                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(100));

                Projectile.NewProjectile(null, new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<HellSoulArmourProj>(), helldamagemelee, 0, Player.whoAmI);

                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 173);
                    dust.scale = 1.5f;
                    dust.velocity *= 3;

                }
                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(target.position, target.width, target.height, 173);
                    dust.scale = 1.5f;
                    dust.velocity *= 3;

                }
                SoundEngine.PlaySound(SoundID.Item8, target.Center);

                hellblazetime = 30;
            }*/

            //For Arid Armour with true melee

            if (aridCritSet)
            {
                if (crit)
                {
                    target.GetGlobalNPC<NPCEffects>().aridimmunetime = 10; //target immune to explosion for 10 frames

                    Projectile.NewProjectile(null, new Vector2(target.Center.X, target.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<AncientArmourProj>(), damage, 0, Player.whoAmI);
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) //Hitting enemy with any projectile
        {
            //ShadowFlame armour
            if (shadowflameSet)
            {

                if (ProjectileID.Sets.IsAWhip[proj.type] == true)
                {
                    if (!target.buffImmune[BuffID.ShadowFlame])
                    {
                        target.AddBuff(BuffID.ShadowFlame, 180);

                        for (int i = 0; i < 40; i++)
                        {
                            var dust = Dust.NewDustDirect(target.position, target.width, target.height, 65);
                            dust.scale = 1.5f;
                            dust.velocity *= 2f;
                            dust.noGravity = true;
                        }
                    }
                }
            }
            if (cryoSet)
            {
                if (ProjectileID.Sets.SentryShot[proj.type] == true || proj.sentry)
                {
                    target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);
                    for (int i = 0; i < 10; i++)
                    {
                        var dust = Dust.NewDustDirect(target.position, target.width, target.height, 180);
                        dust.scale = 1f;
                        dust.velocity *= 2f;
                        dust.noGravity = true;
                    }
                }

            }

            //For the Soul Fire armour setbonus with projectiles ======================
            /*if (hellSoulSet && hellblazetime == 0 && proj.type != ModContent.ProjectileType<HellSoulArmourProj>())
            {

                float speedX = 0f;
                float speedY = -8f;

                int helldamageproj = (int)Player.GetTotalDamage(DamageClass.Generic).ApplyTo(80); //89 with armour buffs


                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(100)); // This defines the projectiles random spread . 10 degree spread.
                Projectile.NewProjectile(null, new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<HellSoulArmourProj>(), helldamageproj, 0, Player.whoAmI);


                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 173);
                    dust.scale = 1.5f;
                    dust.velocity *= 3;

                }
                for (int i = 0; i < 20; i++)
                {
                    var dust = Dust.NewDustDirect(target.position, target.width, target.height, 173);
                    dust.scale = 1.5f;
                    dust.velocity *= 3;

                }
                SoundEngine.PlaySound(SoundID.Item8, target.Center);

                hellblazetime = 30;
            }*/

            //Arid armour with projectiles
            if (aridCritSet)
            {
                if (crit)
                {
                    target.GetGlobalNPC<NPCEffects>().aridimmunetime = 10; //target immune to explosion for 10 frames

                    Projectile.NewProjectile(null, new Vector2(target.Center.X, target.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<AncientArmourProj>(), (int)(damage * 2f), 0, Player.whoAmI);
                }
            }

        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit) //Hit by melee only
        {

        }
        public override void OnHitByProjectile(Projectile proj, int damage, bool crit) //Hit by any projectile
        {

        }

    }

}