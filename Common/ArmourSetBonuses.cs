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
using Terraria.GameContent.Drawing;
using Terraria.Map;

namespace StormDiversMod.Common
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

        public bool snowfallSet; //Snowfall armour

        public bool LizardSet; //Lihzarhd Commander armour

        public bool graniteSet; //Granite Armour

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
        public int cryosetcooldown; //Cooldown for Cryo set bonus
        public int lizardsetcooldown; //Cooldown for the lizard bombs
        public int granitesetcooldown; //cooldown for granite Set
        public int snowfalllimit;
        //public bool granite

        //items here for the stupid GetSource_Fromthis thing for projectiles
        public string SetBonus_Santank;
        public string SetBonus_Derp;
        public string SetBonus_SkyKnight;
        public string SetBonus_HellSoul;
        public string SetBonus_Cryo;
        public string SetBonus_Mushroom;
        public string SetBonus_SpaceRockOffence;
        public string SetBonus_SpaceRockDefence;
        public string SetBonus_Blood;
        public string SetBonus_Arid;
        public string SetBonus_Lizard;

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
            snowfallSet = false;
            LizardSet = false;
            graniteSet = false;
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
            granitesetcooldown = 0;
        }

        //===============================================================================================================

        public override void PostUpdateEquips() //Updates every frame
        {
            //Increase ints if they are below the limit and armour is equipped and not in the equip field
          
            if (BloodDrop)
            {
                if (bloodtime < 180) //HemoGolbin cooldown
                {
                    bloodtime++;
                }
                if (bloodtime >= 180)//Gives buff when cooldown is over
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

            if (LizardSet)
            {
                if (lizardsetcooldown < 60)
                    lizardsetcooldown++;
            }
            else
                lizardsetcooldown = 0;
            //For hemoglobin set ========================================================
            /*if (BloodDrop)
            {
                bloodtime++;
                //Channeling weaposn fire every time half the usetime is met with a counter
                if (((Player.channel && bloodtime >= Player.HeldItem.useAnimation) || (!Player.channel && Player.itemAnimation == Player.itemAnimationMax && Player.HeldItem.useTime > 1)) && (Player.HeldItem.CountsAsClass(DamageClass.Melee) || Player.HeldItem.CountsAsClass(DamageClass.MeleeNoSpeed))) //If the player is holding a ranged weapon and usetime cooldown is above 1
                {
                    bloodtime = 0;

                    int aura = ModContent.ProjectileType<Projectiles.BloodAura>();

                    Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        Vector2 velocity = new Vector2(Math.Sign(mousePosition.X - Player.Center.X), 0); // determines direction
                        int damage = (int)(Player.GetTotalDamage(Player.HeldItem.DamageType).ApplyTo(Player.HeldItem.damage));
                        Projectile spawnedProj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(SetBonus_Blood), Player.MountedCenter - velocity * 2, velocity * 5, aura, damage / 2, 0.1f, Main.myPlayer,
                                Math.Sign(mousePosition.X - Player.Center.X) * Player.gravDir, Player.itemAnimationMax, 1);
                        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, Player.whoAmI);
                    }
                    Player.HeldItem.useTurn = false;
                    SoundEngine.PlaySound(SoundID.NPCHit13, Player.Center);
                }
            }
            else
            {
                bloodtime = 0;
            }*/
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
                    if (santankcharge >= 100)
                    {
                        if (ModLoader.TryGetMod("TMLAchievements", out Mod mod))
                        {
                            mod.Call("Event", "Santanked");
                        }
                    }
                    santanktrigger = true;

                }
                if (santanktrigger) //Drains the rocket charge and clears buffs
                {
                    santankcharge--;
                    Player.ClearBuff(ModContent.BuffType<SantankBuff1>());
                    Player.ClearBuff(ModContent.BuffType<SantankBuff2>());
                    Player.ClearBuff(ModContent.BuffType<SantankBuff3>());
                }
                if (!santanktrigger && santankmissleup == 0 && santankcharge % 10 == 0 && santankcharge != 0) //every 10 (30 frames)
                //Creates a particle and sound effect at these times, times by 3 to get excat frame
                {
                    for (int i = 0; i < 30; i++)
                    {
                        int dustIndex = Dust.NewDust(new Vector2(Player.Center.X - (15 * Player.direction) - 4, Player.Center.Y - 8 * Player.gravDir), 0, 0, 6, 0f, 0f, 50, default, 1.5f);
                        Main.dust[dustIndex].velocity *= 2;
                        Main.dust[dustIndex].noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.Item61 with { Volume = 0.5f, Pitch = 0.5f }, Player.Center);
                }
                else if (santanktrigger && santankcharge % 10 == 0 && santankcharge > 0)
                //Fires missles at these times
                {
                    for (int i = 0; i < 30; i++)
                    {
                        int dustIndex = Dust.NewDust(new Vector2(Player.Center.X - (15 * Player.direction) - 4, Player.Center.Y - 8 * Player.gravDir), 0, 0, 6, 0f, 0f, 50, default, 1.5f);
                        Main.dust[dustIndex].velocity *= 2;
                        Main.dust[dustIndex].noGravity = true;
                    }
                    for (int i = 0; i < 25; i++)
                    {
                        int dustIndex = Dust.NewDust(new Vector2(Player.Center.X - (15 * Player.direction) - 4, Player.Center.Y - 8 * Player.gravDir), 0, 0, 31, 0, -3 * Player.gravDir, 100, default, 1f);
                        Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].noGravity = true;
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

                        Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Santank), new Vector2(Player.Center.X - (15 * Player.direction), Player.Center.Y - 6 * Player.gravDir), new Vector2(perturbedSpeed.X, perturbedSpeed.Y * Player.gravDir), ModContent.ProjectileType<SantankMissleProj>(), santankdamage, 1f, Player.whoAmI);
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

            //float xWarplimit = 640;
            //float yWarplimit = 400; (old code, keep to be safe)
            Vector2 warplocation;
            int warplimit = 640;
            if (twilightSet)
            {           
                float distanceX = Player.Center.X - Main.MouseWorld.X;
                float distanceY = Player.Center.Y - Main.MouseWorld.Y;

                if (Vector2.Distance(Player.Center, Main.MouseWorld) <= warplimit) //if close, just teleport to mouse <40 tile radius
                {
                    warplocation.X = Main.MouseWorld.X;
                    warplocation.Y = Main.MouseWorld.Y;
                }
                else // if further, find maximum distance and teleport to it
                {
                    Vector2 velocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(Player.Center.X, Player.Center.Y));
                    Vector2 perturbedSpeed = new Vector2(velocity.X * warplimit, velocity.Y * warplimit).RotatedBy(0);
                    warplocation.X = Player.Center.X + perturbedSpeed.X;
                    warplocation.Y = Player.Center.Y + perturbedSpeed.Y;
                }

                int xcursor = (int)(warplocation.X / 16);
                int ycursor = (int)(warplocation.Y / 16);
                Tile tile = Main.tile[xcursor, ycursor]; // don't teleport if the teleport location would be in solid tiles

                if ((!tile.HasTile || !Main.tileSolid[tile.TileType]) && !Player.HasBuff(ModContent.BuffType<TwilightDebuff>())) //Checks if mouse is in valid postion
                {
                    //if (((distanceX < -xWarplimit || distanceX > xWarplimit || distanceY < -yWarplimit || distanceY > yWarplimit) && Collision.CanHitLine(Main.MouseWorld, 1, 1, Player.position, Player.width, Player.height)) ||
                        //(distanceX > -xWarplimit && distanceX < xWarplimit && distanceY > -yWarplimit && distanceY < yWarplimit)) //If there is no line of sight and cursor is past limit, don't allow teleport to prevent gettign stuck in blocks
                    {
                        twilightcharged = true; //Activates the outline effect on the armour

                        if (StormDiversMod.ArmourSpecialHotkey.JustPressed) //Activates when player presses button
                        {
                            if (ModLoader.TryGetMod("TMLAchievements", out Mod mod))
                            {
                                mod.Call("Event", "TwilightWarp");
                            }

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
                            Main.SetCameraLerp(0.1f, 0);//Smooth camera movement, this was it :clayman:

                            Player.position = warplocation;

                            //X postion //Old code
                            /*{
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
                            }*/
                            //warp line effects
                            Dust.QuickDustLine(Player.Center, Player.oldPosition + new Vector2(Player.width / 2, Player.height/ 2), 50, Color.Purple); //centre to centre
                            Dust.QuickDustLine(new Vector2(Player.Center.X, Player.Top.Y), Player.oldPosition + new Vector2(Player.width / 2, Player.height ), 50, Color.Purple); //top to bottom
                            Dust.QuickDustLine(new Vector2(Player.Center.X, Player.Bottom.Y), Player.oldPosition + new Vector2(Player.width / 2, 0), 50, Color.Purple); //bottom to top
                            Dust.QuickDustLine(new Vector2(Player.Left.X, Player.Center.Y), Player.oldPosition + new Vector2(Player.width, Player.height / 2), 50, Color.Purple); //left to right
                            Dust.QuickDustLine(new Vector2(Player.Right.X, Player.Center.Y), Player.oldPosition + new Vector2(0, Player.height / 2), 50, Color.Purple); //right to left

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
                    /*else
                    {
                        twilightcharged = false;
                    }*/
                }
                else
                {
                    twilightcharged = false; //Removes the outline effect if the player is unable to charge
                }
            }
            //for Derpling armour

            if (derpJump)
            {
                if (derplinglaunchcooldown < 45)
                {
                    derplinglaunchcooldown++;
                }
           
                Player.jumpSpeedBoost += 4.5f;

                Player.autoJump = true;
                if (Player.controlDown && !Player.controlJump && Player.velocity.Y != 0 && !Player.mount.Active)
                {
                    Player.gravity += 1.25f;
                    Player.maxFallSpeed *= 1.6f;

                    var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 68, 0, 0, 130, default, 1f);
                    dust.noGravity = true;
                    dust.velocity *= 2;

                }
                //Creates the wave upon jumping
                if (Player.velocity.Y == 0 && derplinglaunchcooldown >= 45 && !Player.mount.Active)
                {
                    Player.AddBuff(ModContent.BuffType<DerpBuff>(), 2);

                    if (Player.controlJump)
                    {

                        Player.ClearBuff(ModContent.BuffType<DerpBuff>());

                        SoundEngine.PlaySound(SoundID.NPCHit22 with { Volume = 1.5f, Pitch = -0.5f }, Player.Center);

                        for (int i = 0; i < 40; i++)
                        {
                            float speedX = Main.rand.NextFloat(-2f, 2f);
                            var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 68, speedX, -3 * Player.gravDir, 130, default, 1.5f);
                            dust.noGravity = true;
                            dust.velocity *= 2;
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
                        Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Derp), new Vector2(Player.Center.X, Player.Right.Y - 20 * Player.gravDir), new Vector2(7, 0), ModContent.ProjectileType<DerpWaveProj>(), 120, 0, Player.whoAmI);
                        Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Derp), new Vector2(Player.Center.X, Player.Left.Y - 20 * Player.gravDir), new Vector2(-7, 0), ModContent.ProjectileType<DerpWaveProj>(), 120, 0, Player.whoAmI);
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
                    if (Player == Main.LocalPlayer)
                    {
                        Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SkyKnight), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.SentryProjs.SkyKnightSentryProj>(), 0, 0, Player.whoAmI);
                        skysentry = true;
                    }
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
                            bool lineOfSight = Collision.CanHitLine(target.position, target.width, target.height, Player.position, Player.width, Player.height);
                            if (!target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.CanBeChasedBy() && target.type != NPCID.TargetDummy && StormDiversMod.ArmourSpecialHotkey.JustPressed)
                            {
                                if ((Vector2.Distance(Player.Center, target.Center) <= 500 && lineOfSight) || (Vector2.Distance(Player.Center, target.Center) <= 200 && !lineOfSight))
                                {
                                    if (hellblazedmg > 0) //only summon projectile if enemy will take damage
                                    {
                                        Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_HellSoul), target.Center, new Vector2(0, 0), ModContent.ProjectileType<HellSoulArmourProj>(), hellblazedmg, 0, Player.whoAmI);
                                        hellblazedmg = (hellblazedmg -= 80); //80 damage falloff per enemy, hits up to 15 enemies
                                        //Dust.QuickDustLine(Player.Center, target.Center, 50, Color.MediumPurple); //centre to centre
                                    }
                                    //hellblazedmg = (hellblazedmg * 17) / 20; //15% damage falloff per enemy
                                }
                            }
                        }
                        Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_HellSoul), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<HellSoulArmourProj>(), 0, 0, Player.whoAmI);

                        //if (!GetInstance<ConfigurationsIndividual>().NoShake)
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
                if (cryosetcooldown < 120)//count up when set nonus is not active
                {
                    cryosetcooldown++;
                }
                Vector2 cryocloudpos; // position for cloud

                if (Vector2.Distance(Player.Center, Main.MouseWorld) <= 500)
                {
                    cryocloudpos.X = Main.MouseWorld.X;
                    cryocloudpos.Y = Main.MouseWorld.Y;
                }
                else
                {
                    Vector2 velocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(Player.Center.X, Player.Center.Y));
                    Vector2 perturbedSpeed = new Vector2(velocity.X * 500, velocity.Y * 500).RotatedBy(0);
                    cryocloudpos.X = Player.Center.X + perturbedSpeed.X;
                    cryocloudpos.Y = Player.Center.Y + perturbedSpeed.Y;
                }

                int xcursor = (int)(cryocloudpos.X / 16);
                int ycursor = (int)(cryocloudpos.Y / 16);

                Tile tile = Main.tile[xcursor, ycursor];
                if ((tile != null && !tile.HasTile || !Main.tileSolid[tile.TileType]) && StormDiversMod.ArmourSpecialHotkey.JustPressed && cryosetcooldown >= 120 && Collision.CanHitLine(Main.MouseWorld, 1, 1, Player.position, Player.width, Player.height)) //Activate set bonus
                {
                    if (Player.statMana >= 75)
                    {
                        //int cryodamage = (int)Player.GetTotalDamage(DamageClass.Magic).ApplyTo(25); //31 with cryoset buffs alone
                        
                        Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Cryo), new Vector2(cryocloudpos.X, cryocloudpos.Y), new Vector2(0, 0), ModContent.ProjectileType<FrostCryoArmourProj>(), 25, 0, Player.whoAmI);
                        Dust.QuickDustLine(Player.Center, cryocloudpos, 50, Color.LightSkyBlue); //centre to centre

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
                        int proj = Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Cryo), new Vector2(cryocloudpos.X, cryocloudpos.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionFrostProj>(), 0, 0, Player.whoAmI);
                        Main.projectile[proj].scale = 1.25f;
                        int proj2 = Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Cryo), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<ExplosionFrostProj>(), 0, 0, Player.whoAmI);
                        Main.projectile[proj2].scale = .9f;

                        Player.statMana -= 75;
                        Player.manaRegenDelay = 120;
                        Player.manaRegenCount = 0;
                        Player.manaRegen = 0;
                        cryosetcooldown = 0;
                    }
                }
            }
            if (!cryoSet)
            {
                cryosetcooldown = 0;
            }

            if (snowfallSet)
            {
                if (Player.velocity.Y == 0)
                    snowfalllimit = 120;

                if (Player.controlJump && Player.velocity.Y > 0 && snowfalllimit > 0)
                {
                    if (snowfalllimit > 45)
                    {
                        var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 16);
                        dust.scale = 1f;
                        dust.noGravity = true;
                    }
                    else
                    {
                        var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 31); //when low on glide, chnage particles
                        dust.scale = 1f;
                        dust.noGravity = true;
                    }
                    if (snowfalllimit == 1 || (snowfalllimit == 2 && Player.controlUp)) //final puff
                    {
                        for (int i = 0; i < 40; i++) //Grey dust circle7
                        {
                            Vector2 perturbedSpeed = new Vector2(0, -3f).RotatedByRandom(MathHelper.ToRadians(360));
                            var dust = Dust.NewDustDirect(Player.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);
                            dust.noGravity = true;
                            dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                            dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                        }
                        SoundEngine.PlaySound(SoundID.NPCDeath6 with { Volume = 0.5f, Pitch = 0.35f, MaxInstances = 0 } , Player.Center);

                    }

                    if (Player.controlUp)
                    {
                        snowfalllimit -= 2;

                        Player.gravity = 0.1f;
                        Player.maxFallSpeed *= 0.1f;
                    }
                    else
                    {
                        snowfalllimit--;

                        Player.gravity = 0.25f;
                        Player.maxFallSpeed *= 0.25f;
                    }
                    Player.runAcceleration += 0.2f;
                    Player.fallStart = (int)Player.tileTargetY;
                    Player.slowFall = false;

                    //Main.NewText("flight limit = " + snowfalllimit, 0, 146, 0);
                }
            }
            else
                snowfalllimit = 0;
            if (graniteSet)
            {
                /*if (StormDiversMod.ArmourSpecialHotkey.JustPressed && granitetoggle == false)
                {
                    granitetoggle = true;
                }
                else if (StormDiversMod.ArmourSpecialHotkey.JustPressed && granitetoggle == true)
                {
                    granitetoggle = false;
                }*/
                if (StormDiversMod.ArmourSpecialHotkey.JustPressed && granitesetcooldown <= 0)
                {
                    Player.AddBuff(ModContent.BuffType<GraniteBuff>(), 300);
                    //Player.GetAttackSpeed(DamageClass.Melee) += 0.2f;
                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(0, -4f).RotatedByRandom(MathHelper.ToRadians(360));
                        var dust = Dust.NewDustDirect(Player.Center, 0, 0, 229, perturbedSpeed.X, perturbedSpeed.Y);
                        dust.scale = 1.5f;
                        dust.noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.Item122, Player.Center);
                    granitesetcooldown = 1500;

                }

                if (StormDiversMod.ArmourSpecialHotkey.JustPressed && granitesetcooldown > 0 && !Player.HasBuff(ModContent.BuffType<GraniteBuff>()))
                {
                    SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 0.25f, Pitch = 0.75f, MaxInstances = 0 }, Player.Center);
                    for (int i = 0; i < 3; i++)
                    {
                        var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 187);
                        dust.scale = 1f;
                        dust.velocity *= 1.5f;
                        //dust.noGravity = true;
                    }
                }
                if (granitesetcooldown > 0) //only count down when set is equipped
                {
                    granitesetcooldown--;
                }
            }
            else
            {
                Player.ClearBuff(ModContent.BuffType<GraniteBuff>());
            }

            if (!Player.HasBuff(ModContent.BuffType<GraniteBuff>()) && granitesetcooldown > 0) //once buff is removed add remaining duration to debuff
            {
                Player.AddBuff(ModContent.BuffType<GraniteDebuff>(), granitesetcooldown);
            }
           
            if (Player.HasBuff(ModContent.BuffType<GraniteBuff>()))
            {
                Player.jumpSpeed *= 0.85f;
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
                    Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Mushroom), new Vector2(victim.Center.X - 100, victim.Center.Y - 100), new Vector2(12, 12), ModContent.ProjectileType<MagicMushArmourProj>(), mushdamage, 0, Player.whoAmI);

                }
                else
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Mushroom), new Vector2(victim.Center.X + 100, victim.Center.Y - 100), new Vector2(-12, 12), ModContent.ProjectileType<MagicMushArmourProj>(), mushdamage, 0, Player.whoAmI);

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
                //if (!GetInstance<ConfigurationsIndividual>().NoShake)
                {
                    Player.GetModPlayer<MiscFeatures>().screenshaker = true;
                }

                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockOffence), new Vector2(victim.Center.X - 0, victim.Center.Y - 350), new Vector2(0 + offenceveloX, 10f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned directly above and goes straight down
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockOffence), new Vector2(victim.Center.X - 50, victim.Center.Y - 450), new Vector2(0 + offenceveloX, 10f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned slightly left and moves straight down
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockOffence), new Vector2(victim.Center.X + 50, victim.Center.Y - 450), new Vector2(0 + offenceveloX, 10f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned slightly right and moves straight down
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockOffence), new Vector2(victim.Center.X - 150, victim.Center.Y - 500), new Vector2(2 + offenceveloX, 10f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned to the left and moves left
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockOffence), new Vector2(victim.Center.X + 150, victim.Center.Y - 500), new Vector2(-2 + offenceveloX, 10f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned to the right and moves right
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockOffence), new Vector2(victim.Center.X - 200, victim.Center.Y - 450), new Vector2(4 + offenceveloX, 8f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned further to the left and moves right
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockOffence), new Vector2(victim.Center.X + 200, victim.Center.Y - 450), new Vector2(-4 + offenceveloX, 8f), ModContent.ProjectileType<SpaceArmourProj>(), offencedmg, offenceknb, Player.whoAmI); //Summoned further to the right and moves left

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
            base.OnHitAnything(x, y, victim);
        }

        //=====================For taking damage from any source===========================================

        int attackdmg = 0;//This is for how much damage the player takes
        public override void OnHurt(Player.HurtInfo info)
        {
            attackdmg = (int)info.Damage; //Int for the damage taken

            //For Space Armour with Mask (Defence)
            int defencedmg = 100 + (attackdmg * 2); //Boulder damage
            if (defencedmg > 500)
            {
                defencedmg = 500;
            }
            int defenceknb = 6; //Boulder Knockback
            float defenceVeloX = Player.velocity.X * 0.25f;

            if (spaceRockDefence && Player.HasBuff(ModContent.BuffType<SpaceRockDefence>()) && attackdmg >= 2)
            {
                //if (!GetInstance<ConfigurationsIndividual>().NoShake)
                {
                    Player.GetModPlayer<MiscFeatures>().screenshaker = true;
                }

                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockDefence), new Vector2(Player.Center.X + 0, Player.Top.Y - 350), new Vector2(0 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned above and goes straight down
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockDefence), new Vector2(Player.Center.X - 0, Player.Top.Y - 400), new Vector2(-1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned above and moves slighly left
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockDefence), new Vector2(Player.Center.X + 0, Player.Top.Y - 400), new Vector2(1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned above and moves slightly right
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockDefence), new Vector2(Player.Center.X - 60, Player.Top.Y - 450), new Vector2(-1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned to the left and moves  left
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockDefence), new Vector2(Player.Center.X + 60, Player.Top.Y - 450), new Vector2(1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned to the right and moves  right
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockDefence), new Vector2(Player.Center.X - 120, Player.Top.Y - 500), new Vector2(-1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI);  //Summoned far to the left and moves left
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_SpaceRockDefence), new Vector2(Player.Center.X + 120, Player.Top.Y - 500), new Vector2(1 + defenceVeloX, 8), ModContent.ProjectileType<SpaceArmourProj>(), defencedmg, defenceknb, Player.whoAmI); //Summoned far to the right and moves right

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
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
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

             Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_HellSoul), new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<HellSoulArmourProj>(), helldamagemelee, 0, Player.whoAmI);

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

            //For the Hemoglobin armour setbonus true melee ======================
            if (BloodDrop)
            {
                if (bloodtime >= 180 && !Player.dead && target.life > 5 && !target.friendly)
                {
                    SoundEngine.PlaySound(SoundID.NPCHit9, Player.Center);

                    float numberProjectiles = 6 + Main.rand.Next(3);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        float rotation = Player.itemRotation + (Player.direction == -1 ? (float)Math.PI : 0); //the direction the item points in

                        float speedX = 0f;
                        float speedY = -6f;
                        int blooddamage = (int)(Player.HeldItem.damage / 2);
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(200));
                        float scale = 1f - (Main.rand.NextFloat() * .5f);
                        perturbedSpeed = perturbedSpeed * scale;

                        Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Blood), new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y * Player.gravDir), ModContent.ProjectileType<BloodArmourDropProj>(), blooddamage, 1, Player.whoAmI, 0, 0, 1);
                        bloodtime = 0;
                    }
                }
            }

            //For Arid Armour with true melee

            if (aridCritSet)
            {
                if (hit.Crit & !target.friendly && target.lifeMax > 5)
                {
                    target.GetGlobalNPC<NPCEffects>().aridimmunetime = 10; //target immune to explosion for 10 frames

                    Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Arid), new Vector2(target.Center.X, target.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<AncientArmourProj>(), (int)(damageDone), 0, Player.whoAmI);
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //ShadowFlame armour
            if (shadowflameSet)
            {
                if (ProjectileID.Sets.IsAWhip[proj.type] == true && proj.owner == Main.myPlayer)
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
                if ((ProjectileID.Sets.SentryShot[proj.type] == true || proj.sentry) && proj.owner == Main.myPlayer)
                {
                    target.AddBuff(BuffID.Frostburn2, 300);
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
                Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_HellSoul), new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<HellSoulArmourProj>(), helldamageproj, 0, Player.whoAmI);


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

            //For the Hemoglobin armour setbonus projs ======================
            if (BloodDrop)
            {
                if ((proj.DamageType == DamageClass.Melee || proj.DamageType == DamageClass.MeleeNoSpeed) && proj.type != ModContent.ProjectileType<BloodArmourDropProj>())
                {
                    if (bloodtime >= 180 && !Player.dead && target.life > 5 && !target.friendly)
                    {
                        SoundEngine.PlaySound(SoundID.NPCHit9, Player.Center);

                        float numberProjectiles = 6 + Main.rand.Next(3);

                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            float speedX = 0f;
                            float speedY = -6f * Player.gravDir;
                            int blooddamage = (int)(Player.HeldItem.damage / 2);
                            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(150));
                            float scale = 1f - (Main.rand.NextFloat() * .5f);
                            perturbedSpeed = perturbedSpeed * scale;

                            Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Blood), new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y * Player.gravDir), ModContent.ProjectileType<BloodArmourDropProj>(), blooddamage, 1, Player.whoAmI, 0, 0, 1);
                            bloodtime = 0;
                        }
                    }
                }
            }

            //Arid armour with projectiles
            if (aridCritSet)
            {
                if (hit.Crit && !target.friendly && target.lifeMax > 5)
                {
                    target.GetGlobalNPC<NPCEffects>().aridimmunetime = 10; //target immune to explosion for 10 frames

                    Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Arid), new Vector2(target.Center.X, target.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<AncientArmourProj>(), (int)(damageDone), 0, Player.whoAmI);
                }
            }
            if (LizardSet)
            {
                int lizarddamage = (int)Player.GetTotalDamage(DamageClass.Summon).ApplyTo(160); //224 with armour buffs alone

                if (ProjectileID.Sets.IsAWhip[proj.type] == true && proj.owner == Main.myPlayer && !target.friendly && target.lifeMax > 5 && target.CanBeChasedBy())
                {
                    if (lizardsetcooldown >= 30)
                    {
                        Projectile.NewProjectile(Player.GetSource_FromThis(SetBonus_Lizard), new Vector2(target.Center.X, target.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.LizardArmourProj>(), lizarddamage, 0, Main.myPlayer, 0, Main.rand.Next(0, 359));
                        SoundEngine.PlaySound(SoundID.Item61 with { Volume = 0.5f, Pitch = -0.25f }, Player.Center);

                        for (int i = 0; i < 30; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(0, -4f).RotatedByRandom(MathHelper.ToRadians(360));
                            var dust = Dust.NewDustDirect(target.Center, 0, 0, 170, perturbedSpeed.X, perturbedSpeed.Y);
                            dust.noGravity = true;
                            dust.scale = 1.5f;
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(0, -4f).RotatedByRandom(MathHelper.ToRadians(360));
                            var dust = Dust.NewDustDirect(Player.Center, 0, 0, 170, perturbedSpeed.X, perturbedSpeed.Y);
                            dust.noGravity = true;
                            dust.scale = 1.5f;
                        }

                        lizardsetcooldown = 0;
                    }
                }
            }
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            base.OnHitByNPC(npc, hurtInfo);
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            base.OnHitByProjectile(proj, hurtInfo);
        }
    }
}