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
   
    public class EquipmentEffects : ModPlayer
    {
        //Bools activated from accessories/pets/single armour pieces

        public bool goldDerpie; //The player has the Golden Derpie Pet

        public bool stormHelmet; //The player has the Storm Diver Pet

        public bool twilightPet; //The player has the Twilight Light Pet

        public bool stormBossPet; //The player has the Scandrone Pet

        public bool shroombuff; //The Player has the Ranged enhancement potion buff

        public bool flameCore; //The player has Betsy's Flame equipped

        public bool frostSpike; //The Player has the Cryo Core equipped

        public bool lunarBarrier; //The player has the Celestial Barrier equipped

        public bool primeSpin; //The player has the Mech Spikes equipped

        public bool bootFall; //The player has either heavy boots equipped

        public bool frostCube; //Player has the Summoners Core equipped

        public bool spooked; //Player has the Spooky Core equipped

        public bool SpectreSkull; //Player has the Spectre Skull equipped

        public bool desertJar; //Player has the Pharoh's Urn equipped

        public bool frostJar; //Player has the Frozen Urn equipped, just used to change the desert projs into frost

        public bool graniteBuff; //Player has the granite accessory equipped

        public bool shroomaccess; //Player has the Shroomite Accessory equipped

        public bool heartSteal; //Player has the Heart emblem equipped

        public bool mushChestplate; //Player has Shroom chestplate equipped

        public bool derpEye; //Player had the Derpling Eye equipped

        public bool blueCuffs; //Player has insulated cuffs equipped

        public bool lunaticHood; //Player has the Luantic Hood equipped

        public bool beetleFist; //player has the Beetle gauntlet equipped

        public bool aridCritChest; //Player has the Arid Chestplate equipped

        public bool soulBoots; //Player has Soul Striders equipped;

        public bool bloodBoots; //Player has Blood Treads equipped;

        public bool woodNecklace; //Player has the wooden necklace equipped

        public bool coralEmblem; //Player has coral Emblem equipped

        public bool stormBossAccess; //Player has the Storm Coil equipped

        //Ints and Bools activated from this file

        public bool shotflame; //Indicates whether the SPooky Core has fired its flames or not
        public bool spikespawned; //Wheter mechanical spieks ahev been spawned
        public bool falling; //Wheter the player is falling at speed
        public int stopfall; //If the player has stopped falling
        public int bearcool; //Cooldown for the Teddy Bear
        public int stomptrail; //Delay of the projectuiles of the trail when falling with the boots
        public int frosttime; //Cooldown of the frost shards from the Cryo Core
        public bool desertdustspawned; //has the Pahroh's urn creaed the oprbiting projectile?
        public int granitebufftime; //Cooldown for the granite Accessory Buff to be reapplied
        public bool granitesurge; //Makes it so the granite accessory cooldown can start and makes it so the next attack removes the buff
        public int flamecooldown; //Cooldown for betsy's flame with channeling weapons
        public int shroomshotCount = 0; //Count show many times the player has fired with the shroomite access
        public bool shotrocket; //Wheter the shroomite rocket has been fired or not
   
        public bool derpfalling; //Falling at speed with derp leg
        public int stopderpfall; //Player has stoppd falling with derp leg
    
        public bool celestialspin; //Has the spinning projectile fo the celestial shell been summoned?
       
        public bool lunaticsentry; //Has the lunatic sentry been summoned?
        public int beetlecooldown; //Cooldown until more beetles can be summoned
        public int soundDelay; //Cooldown for boot sound
        public int dropdust; //Cooldown for urn dust
        public int coraldrop; //Cooldown for coral emblem drops
        public bool stormBossProj; // wheter the projectile for the storm coil has been spawned

        public override void ResetEffects() //Resets bools if the item is unequipped
        {
            goldDerpie = false;
            stormHelmet = false;
            twilightPet = false;
            stormBossPet = false;
            shroombuff = false;
            flameCore = false;
            frostSpike = false;
            lunarBarrier = false;
            primeSpin = false;
            bootFall = false;
            frostCube = false;
            spooked = false;     
            SpectreSkull = false;
            desertJar = false;
            frostJar = false;
            graniteBuff = false;   
            shroomaccess = false;
            heartSteal = false;
            mushChestplate = false;
            derpEye = false;
            blueCuffs = false;
            lunaticHood = false;
            beetleFist = false;
            aridCritChest = false;
            soulBoots = false;
            bloodBoots = false;
            woodNecklace = false;
            coralEmblem = false;
            stormBossAccess = false;
        }
        public override void UpdateDead()//Reset all ints and bools if dead======================
        {
            bearcool = 0;
            frosttime = 0;
            falling = false;
            desertdustspawned = false;
            granitebufftime = 0;
            granitesurge = false;
            shroomshotCount = 0;
            shotrocket = false;
            celestialspin = false;  
            lunaticsentry = false;
            spikespawned = false;
            dropdust = 0;
            coraldrop = 0;
            stormBossProj = false;
            flamecooldown = 0;
        }
   
        //===============================================================================================================
       
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (mushChestplate)
            {
                if (item.CountsAsClass(DamageClass.Ranged) && item.ammo == 0)
                {
                    damage.Flat += 1;
                }
            }
            if (Player.HasBuff(BuffType<ShroomiteBuff>()))//If the player has the shroomite potion then 10% increase ammo damage
            {
                if (item.CountsAsClass(DamageClass.Ranged) && item.ammo != 0)
                {
                    damage *= 1.1f;
                    if (item.damage <= 10)
                    {
                        damage.Flat += 1;
                    }

                }
            }
            //for Enchanted Mushroom
            if (item.ammo == 0)
            {
                if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff1>()))
                {

                    damage.Flat += 1;

                }
                else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff2>()))
                {

                    damage.Flat += 2;

                }
                else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff3>()))
                {

                    damage.Flat += 4;

                }
                else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff4>()))
                {

                    damage.Flat += 6;

                }
            }
        }
       
        public override void PostUpdateEquips() //Updates every frame
        {
            //Reduces ints if they are above 0 and not in the equip field

            if (frostSpike)
            {
                if (frosttime < 360)
                {
                    frosttime++;
                }
            }
            else
            {
                frosttime = 0;
            }

            if (graniteBuff)
            {
                if (granitebufftime < 600)
                {
                    granitebufftime++;
                }
            }
            else
            {
                granitebufftime = 0;
            }

            if (bearcool > 0)
            {
                bearcool--;
            }
            //======================================================================================Accessories/other======================================================================================
            if (beetleFist && Player.HeldItem.CountsAsClass(DamageClass.Melee))
            {
                if (Main.rand.Next(6) == 0)
                {
                    int dust = Dust.NewDust(new Vector2(Player.position.X - 10, Player.position.Y - 5), Player.width + 20, Player.height + 10, 186);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.5f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    Main.dust[dust].fadeIn = 0.5f;

                }
            }
            //For Spectre Skull/Flower
            if (SpectreSkull)
            {
                if (Main.LocalPlayer.HasBuff(BuffID.ManaSickness))
                {

                    Player.manaCost *= 0f;

                    Dust dust;
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Vector2 position = Main.LocalPlayer.position;
                    dust = Main.dust[Terraria.Dust.NewDust(position, Player.width, Player.height, 15, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                    dust.noGravity = true;
                }
            }
            //For Soul/Blood Striders============================================================
            var tilePos = Player.Bottom.ToTileCoordinates16();
            if (soulBoots || bloodBoots)
            {

                //int xboottilepos = (int)(Player.position.X + (float)(Player.width / 2)) / 16;
                //int yboottilepos = (int)(Player.Bottom.Y / 16);

                //SPEEDS!
                if (Framing.GetTileSafely(tilePos.X, tilePos.Y).TileType == TileID.Asphalt)//When on asphalt 
                {
                    if (soulBoots)
                    {
                        Player.maxRunSpeed += 2.1f;
                        Player.runAcceleration *= 2f;
                        Player.runSlowdown = 0.2f;
                    }
                    else if (bloodBoots)
                    {
                        Player.maxRunSpeed += 1f;
                        Player.runAcceleration *= 1.5f;
                        Player.runSlowdown = 0.15f;
                    }
                }
                else if (Framing.GetTileSafely(tilePos.X, tilePos.Y).TileType != TileID.Asphalt && Player.velocity.Y == 0)//When not on asphalt 
                {
                    if (soulBoots)
                    {
                        Player.maxRunSpeed += 7f;
                        Player.runAcceleration *= 3f;
                        Player.runSlowdown = 0.5f;
                    }
                    else if (bloodBoots)
                    {
                        Player.maxRunSpeed += 5f;
                        Player.runAcceleration *= 1.75f;
                        Player.runSlowdown = 0.35f;
                    }
                }
                else if (Player.velocity.Y != 0)//When in the air
                {
                    if (soulBoots)
                    {
                        Player.maxRunSpeed += 4.5f;
                        Player.runAcceleration *= 2f;
                        Player.runSlowdown = 0.25f;
                    }
                    else if (bloodBoots)
                    {
                        Player.maxRunSpeed += 3.5f;
                        Player.runAcceleration *= 1.5f;
                        Player.runSlowdown = 0.2f;
                    }
                }
                if (soulBoots)
                {
                    Player.vanityRocketBoots = 2;
                    Player.rocketBoots = 1;
                    //Player.socialShadowRocketBoots = true;
                }
                
                if (Player.moveSpeed > 1)
                {
                    Player.moveSpeed = 1;

                }
                //dusts and projs
                if (soulBoots)
                {
                    if ((Player.velocity.X > 5 || Player.velocity.X < -5) && (Player.velocity.Y == 0) && (Player.controlLeft || Player.controlRight) && !Player.mount.Active)
                    {

                        if (Main.dayTime)
                        {
                            if (Player.gravDir == 1)
                            {
                                Dust dust;
                                dust = Dust.NewDustDirect(new Vector2(Player.Center.X - 5, Player.Bottom.Y - 6), 10, 0, 58, 0, -1);
                                //dust.noGravity = true;
                                dust.scale = 1.25f;
                            }
                            else
                            {
                                Dust dust;
                                dust = Dust.NewDustDirect(new Vector2(Player.Center.X - 5, Player.Top.Y - 6), 10, 0, 58, 0, +1);
                                //dust.noGravity = true;
                                dust.scale = 1.25f;
                            }
                        }
                        else
                        {
                            if (Player.gravDir == 1)
                            {
                                Dust dust;
                                dust = Dust.NewDustDirect(new Vector2(Player.Center.X - 5, Player.Bottom.Y - 6), 10, 0, 27, 0, -1);
                                //dust.noGravity = true;
                                dust.scale = 1.25f;
                            }
                            else
                            {
                                Dust dust;
                                dust = Dust.NewDustDirect(new Vector2(Player.Center.X - 5, Player.Top.Y - 6), 10, 0, 27, 0, +1);
                                //dust.noGravity = true;
                                dust.scale = 1.25f;
                            }
                        }
                        if (Player.gravDir == 1)
                        {

                            int dustSmoke = Dust.NewDust(new Vector2(Player.Center.X - 4, Player.Bottom.Y - 5), 5, 5, 31, 0f, -2f, 0, default, 1f);
                            Main.dust[dustSmoke].scale = 0.5f + (float)Main.rand.Next(5) * 0.1f;
                            Main.dust[dustSmoke].fadeIn = 3f + (float)Main.rand.Next(5) * 0.1f;
                            Main.dust[dustSmoke].noGravity = true;
                            Main.dust[dustSmoke].velocity *= 0.1f;
                        }
                        else
                        {

                            int dustSmoke = Dust.NewDust(new Vector2(Player.Center.X - 4, Player.Top.Y - 5), 5, 5, 31, 0f, +2f, 0, default, 1f);
                            Main.dust[dustSmoke].scale = 0.5f + (float)Main.rand.Next(5) * 0.1f;
                            Main.dust[dustSmoke].fadeIn = 3f + (float)Main.rand.Next(5) * 0.1f;
                            Main.dust[dustSmoke].noGravity = true;
                            Main.dust[dustSmoke].velocity *= 0.1f;
                        }
                        if (!bloodBoots)//If wearing blood boots sound comes from that one (as blood drops are tied to sound effect)
                        {
                            soundDelay++;

                            if (soundDelay >= 6)
                            {

                                SoundEngine.PlaySound(SoundID.Run, Player.Center);
                                soundDelay = 0;
                            }
                        }
                    }

                }
                if (bloodBoots)
                {
                    if ((Player.velocity.X > 4 || Player.velocity.X < -4) && (Player.velocity.Y == 0) && (Player.controlLeft || Player.controlRight) && !Player.mount.Active)
                    {
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Player.gravDir == 1)
                            {

                                Dust dust;
                                dust = Dust.NewDustDirect(new Vector2(Player.Center.X - 5, Player.Bottom.Y - 6), 10, 0, 5, 0, -1);
                                //dust.noGravity = true;
                                dust.scale = 1.25f;
                            }
                            else
                            {
                                Dust dust;
                                dust = Dust.NewDustDirect(new Vector2(Player.Center.X - 5, Player.Top.Y - 6), 10, 0, 5, 0, +1);
                                //dust.noGravity = true;
                                dust.scale = 1.25f;
                            }

                        }
                        if (!soulBoots)//If wearing soul boots dust comes from that one
                        {
                            if (Player.gravDir == 1)
                            {
                                int dustSmoke = Dust.NewDust(new Vector2(Player.Center.X - 4, Player.Bottom.Y - 5), 5, 5, 31, 0f, -2f, 0, default, 1f);
                                Main.dust[dustSmoke].scale = 0.5f + (float)Main.rand.Next(5) * 0.1f;
                                Main.dust[dustSmoke].fadeIn = 2f + (float)Main.rand.Next(5) * 0.1f;
                                Main.dust[dustSmoke].noGravity = true;
                                Main.dust[dustSmoke].velocity *= 0.1f;
                            }
                            else
                            {
                                int dustSmoke = Dust.NewDust(new Vector2(Player.Center.X - 4, Player.Top.Y - 5), 5, 5, 31, 0f, +2f, 0, default, 1f);
                                Main.dust[dustSmoke].scale = 0.5f + (float)Main.rand.Next(5) * 0.1f;
                                Main.dust[dustSmoke].fadeIn = 2f + (float)Main.rand.Next(5) * 0.1f;
                                Main.dust[dustSmoke].noGravity = true;
                                Main.dust[dustSmoke].velocity *= 0.1f;
                            }
                        }
                        soundDelay++;

                        if (soundDelay >= 6) //always drop blood, even with soul boots equipped
                        {
                            if (Player.gravDir == 1)
                            {
                                Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Bottom.Y - 4), new Vector2(0, 0), ModContent.ProjectileType<BloodBootProj>(), 20, 0, Player.whoAmI);
                                SoundEngine.PlaySound(SoundID.Run, Player.Center);
                                soundDelay = 0;
                            }
                            else
                            {
                                Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Top.Y - 4), new Vector2(0, 0), ModContent.ProjectileType<BloodBootProj>(), 20, 0, Player.whoAmI);
                                SoundEngine.PlaySound(SoundID.Run, Player.Center);
                                soundDelay = 0;
                            }
                        }
                    }
                }
            }
            //For Spooky Core======================
            if (spooked)
            {
                float distancehealth = 500 + ((Player.statLifeMax2 - Player.statLife) / 2);
                if (distancehealth > 1000)
                {
                    distancehealth = 1000;
                }
                if (Main.rand.Next(5) == 0)
                {
                    if (Player.gravDir == 1)
                    {
                        var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 200, 0, -2);
                        dust.noGravity = true;
                        dust.fadeIn = 0.5f;
                    }
                    else
                    {
                        var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 200, 0, +2);
                        dust.noGravity = true;
                        dust.fadeIn = 0.5f;
                    }
                }
                //circle of dust
                for (int i = 0; i < 10; i++)
                {
                    double deg = Main.rand.Next(0, 360); //The degrees
                    double rad = deg * (Math.PI / 180); //Convert degrees to radians
                    double dist = distancehealth; //Distance away from the player
                    float dustx = Player.Center.X - (int)(Math.Cos(rad) * dist);
                    float dusty = Player.Center.Y - (int)(Math.Sin(rad) * dist);
                    if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, Player.position, Player.width, Player.height))//no dust unless line of sight
                    {
                        var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 200, 0, 0);
                        dust.noGravity = true;
                        dust.scale = 2;
                    }
                }
                for (int i = 0; i < 200; i++)
                {
                    NPC target = Main.npc[i];
                    var player = Main.LocalPlayer;

                    float shootToX = target.position.X + (float)target.width * 0.5f - Player.Center.X;
                    float shootToY = target.position.Y + (float)target.height * 0.5f - Player.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));


                    /*float distanceX = Player.Center.X - target.Center.X;
                    float distanceY = Player.Center.Y - target.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));*/
                    if (distance < distancehealth && !target.friendly && target.lifeMax > 5 && !target.dontTakeDamage && target.active && target.type != NPCID.TargetDummy && Collision.CanHit(Player.Center, 0, 0, target.Center, 0, 0))
                    {
                        if (!target.buffImmune[(BuffType<SpookedDebuff>())])
                        {
                            /*
                            distance = 1.6f / distance;

                            //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                            shootToX *= distance * (10f + (distancehealth / 50));
                            shootToY *= distance * (10f + (distancehealth / 50));
                            if (Main.rand.Next(4) == 0)
                            {
                                Vector2 perturbedSpeed = new Vector2(shootToX, shootToY).RotatedByRandom(MathHelper.ToRadians(8));

                                var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 259, perturbedSpeed.X, perturbedSpeed.Y);
                                dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                                dust.noGravity = true;
                                dust.scale = 0.75f;
                            }
                            */

                            target.AddBuff(ModContent.BuffType<SpookedDebuff>(), 2);
                        }
                    }

                }

            }
            //for Pharoh Urn
            if (desertJar)
            {
                if ((Player.velocity.X > 3.5f || Player.velocity.X < -3.5f) || (Player.velocity.Y > 4f || Player.velocity.Y < -4f))
                {

                    dropdust++;
                    if (dropdust >= 3)
                    {

                        //float speedX = 0f;
                        //float speedY = 0f;
                        //Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(180));
                        //float scale = 1f - (Main.rand.NextFloat() * .5f);
                        //perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.DesertJarProj>(), 40, 0, Player.whoAmI);
                        dropdust = 0;

                        //Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 13);
                    }
                }
                if (!desertdustspawned) //spawn the 2 orbiting orojs
                {
                    Projectile.NewProjectile(null, Player.Center, new Vector2(0, 0), ModContent.ProjectileType<DesertJarProj2>(), 40, 0f, Player.whoAmI, 0, 0);
                    Projectile.NewProjectile(null, Player.Center, new Vector2(0, 0), ModContent.ProjectileType<DesertJarProj2>(), 40, 0f, Player.whoAmI, 0, 180);

                    desertdustspawned = true;
                }


            }
            if (!desertJar)//reset bool
            {
                desertdustspawned = false;
            }

            //For the Mechanical Spikes===========================
            if (primeSpin)
            {

                if (!spikespawned) 
                {

                    Projectile.NewProjectile(null, Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 0);
                    Projectile.NewProjectile(null, Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 60);

                    Projectile.NewProjectile(null, Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 120);
                    Projectile.NewProjectile(null, Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 180);

                    Projectile.NewProjectile(null, Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 240);
                    Projectile.NewProjectile(null, Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 300);

                    spikespawned = true;

                }


            }
            if (!primeSpin)//reset bool
            {
                spikespawned = false;
            }


            //For the Heavy Boots===========================
            if (bootFall)
            {
                Player.rocketBoots = 1;

                if (Player.controlDown && !Player.controlJump && Player.velocity.Y != 0 && !Player.mount.Active)
                {

                    //SoundEngine.PlaySound(SoundID.Item, (int)Player.Center.X, (int)Player.Center.Y, 15, 2, -0.5f);
                    Player.gravity += 4;
                    Player.maxFallSpeed *= 1.4f;
                    
                    Player.runAcceleration = 0.25f;
                    if ((Player.velocity.Y > 12 && Player.gravDir == 1) || (Player.velocity.Y < -12 && Player.gravDir == -1))
                    {
                        if (!falling)
                        {
                            Player.velocity.X *= 0.5f;
                            Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 5), ModContent.ProjectileType<StompBootProj2>(), 60, 6, Player.whoAmI);
                        }
                        //immunity is in miscfeatures.cs
                        falling = true;
                        Player.noKnockback = true;
                    
                    }

                }               
                //For impacting the ground at speed
                if (Player.velocity.Y == 0 && falling && Player.controlDown)
                {
                    if (!GetInstance<ConfigurationsIndividual>().NoShake)
                    {
                        Player.GetModPlayer<MiscFeatures>().screenshaker = true;
                    }

                    for (int i = 0; i < 30; i++)
                    {

                        int dustIndex = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 31, 0f, 0f, 100, default, 1f);
                        Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].noGravity = true;
                    }
                    if (Player.gravDir == 1)
                    {
                        Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Right.Y + 2), new Vector2(5, 0), ModContent.ProjectileType<StompBootProj>(), 40, 12f, Player.whoAmI);
                        Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Left.Y + 2), new Vector2(-5, 0), ModContent.ProjectileType<StompBootProj>(), 40, 12f, Player.whoAmI);
                    }
                    else
                    {
                        Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Right.Y - 2), new Vector2(5, 0), ModContent.ProjectileType<StompBootProj>(), 40, 12f, Player.whoAmI);
                        Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Left.Y - 2), new Vector2(-5, 0), ModContent.ProjectileType<StompBootProj>(), 40, 12f, Player.whoAmI);
                    }
                    SoundEngine.PlaySound(SoundID.Item14, Player.Center);
                    falling = false;

                }
                if (!Player.controlDown || Player.controlJump || Player.mount.Active) //cancels stomp
                {
                    falling = false;
                }
                //If the player slows down too much then the stomp bool is cancelled
                /*if ((Player.velocity.Y <= 2 && Player.gravDir == 1) || (Player.velocity.Y >= -2 && Player.gravDir == -1))
                {

                    stopfall++;
                }
                else
                {
                    stopfall = 0;
                }
                if (stopfall > 1)
                {
                    falling = false;
                }*/

            }
            //If boots are unequipped then cancel the bool
            if (!bootFall)
            {
                falling = false;
            }
            //For Coral Emblem
            if (coralEmblem)
            {
                if (Player.HeldItem.damage >= 1) //If the player is holding a weapon and usetime cooldown is above 1
                {
                    coraldrop++;

                    if ((Player.itemAnimation == 1 || Player.HeldItem.channel && Player.channel) && coraldrop >= 60)
                    {
                        for (int index = 0; index < 1; ++index)
                        {
                            Vector2 vector2_1 = new Vector2((float)((double)Player.position.X + (double)Player.width * 0.5 + (double)(Main.rand.Next(50) * -Player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)Player.position.X)), (float)((double)Player.position.Y + (double)Player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                            vector2_1.X = (float)(((double)vector2_1.X + (double)Player.Center.X) / 2.0) + (float)Main.rand.Next(-50, 51); //Spawn Spread
                            vector2_1.Y -= (float)(100 * index);
                            float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                            float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                            if ((double)num13 < 0.0) num13 *= -1f;
                            if ((double)num13 < 20.0) num13 = 20f;
                            float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                            float num15 = 10 / num14;
                            float num16 = num12 * num15;
                            float num17 = num13 * num15;
                            float SpeedX = num16 + (float)Main.rand.Next(-2, 2) * 0.05f;  //this defines the projectile X position speed and randomnes
                            float SpeedY = num17 + (float)Main.rand.Next(-2, 2) * 0.05f;  //this defines the projectile Y position speed and randomnes
                            int projID = Projectile.NewProjectile(null, new Vector2(vector2_1.X, vector2_1.Y), new Vector2(SpeedX, SpeedY), ModContent.ProjectileType<OceanSpellProj>(), 25, 0.5f, Player.whoAmI, 0.0f, (float)Main.rand.Next(5));

                            Main.projectile[projID].aiStyle = 0;
                            Main.projectile[projID].DamageType = DamageClass.Generic;

                            SoundEngine.PlaySound(SoundID.Item21, Player.Center);
                            for (int i = 0; i < 35; i++)
                            {

                                var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 33, 0, -3);
                                dust.velocity *= 2;
                                //dust.noGravity = true;
                                dust.scale = 1.5f;
                            }
                        }
                        coraldrop = 0;
                    }
                }
            }
            else
            {
                coraldrop = 0;
            }
            // For the Shroomite Launcher Accessory
            if (shroomaccess)
            {
                if (Player.itemTime > 1 && Player.HeldItem.CountsAsClass(DamageClass.Ranged) && Player.HeldItem.useAmmo == AmmoID.Bullet) //If the player is holding a ranged weapon and usetime cooldown is above 1
                {


                    if (!shotrocket) //If the rocket hasn't already been fired this use then it it fire it
                    {
                        shroomshotCount++;
                        if (shroomshotCount >= 5) //Every 5 shots fires a rocket
                        {

                            shroomshotCount = 0; //Resets the shot count
                            float rotation = Player.itemRotation + (Player.direction == -1 ? (float)Math.PI : 0); //the direction the item points in
                            float velocity = 13f;
                            int type = ModContent.ProjectileType<ShroomSetRocketProj>();
                            int damage = (int)(Player.HeldItem.damage * 2f);
                            Projectile.NewProjectile(null, Player.Center, new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * velocity, type, damage, 2f, Player.whoAmI);

                            SoundEngine.PlaySound(SoundID.Item92, Player.Center);
                        }

                    }
                    shotrocket = true; //This prevents a rocket from spawning every frame
                }
                else
                {
                    shotrocket = false; //Once the usetime is back to 0 this bool can be set to false again
                }
            }
            //For betsy's Flame ======================
            if (flameCore)
            {
                if (flamecooldown > 0)
                {
                    flamecooldown--;
                }
                Player.runAcceleration += 0.25f;

                if ((Player.itemAnimation == 1 && Player.HeldItem.damage >= 1 && Player.HeldItem.ammo == 0 && !shotflame) || (Player.HeldItem.channel && Player.channel && flamecooldown <= 0)) //weapon is in use
                {

                    if (Main.rand.Next(3) == 0)
                    {
                        for (int i = 0; i < 20; i++)
                        {

                            var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 244);

                            dust.noGravity = true;

                        }

                        SoundEngine.PlaySound(SoundID.Item34, Player.Center);

                        float numberProjectiles = 2 + Main.rand.Next(2);

                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            float rotation = Player.itemRotation + (Player.direction == -1 ? (float)Math.PI : 0); //the direction the item points in


                            float speedX = 0f;
                            float speedY = -10f;
                            int damage = (int)(Player.HeldItem.damage * 0.7f);
                            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(90));
                            float scale = 1f - (Main.rand.NextFloat() * .5f);
                            perturbedSpeed = perturbedSpeed * scale;

                            if (Player.gravDir == 1)
                            {
                                Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BetsyFlameProj>(), damage, 1, Player.whoAmI);
                            }
                            else
                            {
                                Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(perturbedSpeed.X, -perturbedSpeed.Y), ModContent.ProjectileType<BetsyFlameProj>(), damage, 1, Player.whoAmI);

                            }

                        }

                    }
                    flamecooldown = Player.HeldItem.useTime; //cooldown for channeling weapons
                    shotflame = true;
                }
                else
                {
                    shotflame = false;
                }

            }
            //For wooden necklace=======================
            if (woodNecklace)
            {
                if (Player.ZoneForest || Player.ZoneHallow && Player.ZoneOverworldHeight)
                {

                    Player.AddBuff(ModContent.BuffType<WoodenBuff>(), 2);

                }
            }


            if (!graniteBuff)//If the player removes the accessory the buff is gone
            {
                Player.ClearBuff(ModContent.BuffType<GraniteAccessBuff>());
            }
            //For the Celestial Barrier Projectile
            if (lunarBarrier)
            {
                if (!celestialspin)
                {
                    Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<CelestialShieldProj>(), 0, 0, Player.whoAmI);

                    celestialspin = true;
                }
            }
            if (!lunarBarrier)
            {
                celestialspin = false;
            }
            //For the Storm Coil projectile
            if (stormBossAccess)
            {
                if (!stormBossProj)
                {
                    Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<StormAccessProj>(), 75, 1, Player.whoAmI);
                    for (int i = 0; i < 30; i++)
                    {
                        float speedY = -3f;

                        Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(Player.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 0.75f);
                        Main.dust[dust2].noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.Item122 with { Volume = 0.5f }, Player.Center);

                    stormBossProj = true;
                }
            }
            if (!stormBossAccess)
            {
                stormBossProj = false;
            }

            //For the Lunatic Cultist accessory
            if (lunaticHood)
            {
                //Player.AddBuff(ModContent.BuffType<SkyKnightSentryBuff"), 2);

                if (!lunaticsentry)
                {
                    Projectile.NewProjectile(null, new Vector2(Player.Center.X - 80, Player.Center.Y - 40), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.SentryProjs.LunaticExpertSentryProj>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(null, new Vector2(Player.Center.X + 80, Player.Center.Y - 40), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.SentryProjs.LunaticExpertSentryProj>(), 0, 0, Player.whoAmI);

                    lunaticsentry = true;

                }
            }
            if (!lunaticHood)
            {
                lunaticsentry = false;
                //Player.ClearBuff(ModContent.BuffType<SkyKnightSentryBuff"));

            }

        }
        //=====================For attacking an enemy with anything===========================================
        public override void OnHitAnything(float x, float y, Entity victim)
        {
           
            //For the Desert urn
            /*if (desertJar)
            {
                
                if (desertdusttime < 1 && !Player.dead)
                {

                    SoundEngine.PlaySound(SoundID.Item20, Player.Center);


                    float numberProjectiles = 8 + Main.rand.Next(0);
                    float rotation = MathHelper.ToRadians(180);
                    //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        float speedX = -1.5f;
                        float speedY = 0f;
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles)));
                        if (!frostJar)
                        {
                            Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<DesertSpellProj>(), 30, 0, Player.whoAmI);
                        }
                        else
                        {
                            Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<Frostthrowerproj>(), 30, 0, Player.whoAmI);

                        }
                        desertdusttime = 240;

                    }
                }
            }*/
           
            base.OnHitAnything(x, y, victim);
        }

        //=====================For taking damage from any source===========================================

        int attackdmg = 0;//This is for how much damage the player takes
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter) //When you take damage for whatever reason
        {
            Player.ClearBuff(ModContent.BuffType<HeartBarrierBuff>()); //Removes buff on hit


            attackdmg = (int)damage; //Int for the damage taken

            //triggers the granite accessory buff for 5 seconds, and it cannot be refreshed until the 10 second timer hjas ran out
            if (graniteBuff && !Player.HasBuff(ModContent.BuffType<GraniteAccessBuff>()) && granitebufftime == 600 && damage > 1)
            {
                Player.AddBuff(ModContent.BuffType<GraniteAccessBuff>(), 240);
                SoundEngine.PlaySound(SoundID.NPCHit41 with { Volume = 1f, Pitch = -0.3f }, Player.Center);
                for (int i = 0; i < 25; i++)
                {

                    var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 65);
                    dust.noGravity = true;
                    dust.scale = 2f;
                    dust.velocity *= 2;
                }
                granitebufftime = 0; //Activates the 10 second cooldown
            }
         
            //Grant buff for celestial barrier based on incoming damage======================
            if (lunarBarrier)
            {


                if (((attackdmg >= 75 && Main.expertMode) || (attackdmg >= 50 && !Main.expertMode)) && attackdmg < Player.statLife)
                {


                    if (!Main.LocalPlayer.HasBuff(ModContent.BuffType<CelestialBuff>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item122, Player.Center);
                        Player.AddBuff(ModContent.BuffType<CelestialBuff>(), (int)(attackdmg * 4f));

                    }

                }
                /*if (attackdmg >= 60 && !Main.expertMode && !Player.HasBuff(ModContent.BuffType<CelestialBuff>()))
                {
                    if (!Main.LocalPlayer.HasBuff(ModContent.BuffType<CelestialBuff>()))
                    {
                        SoundEngine.PlaySound(SoundID.Item122, Player.Center);
                        Player.AddBuff(ModContent.BuffType<CelestialBuff>(), (int)(attackdmg * 4f));
                    }
                }*/
            }
            //Creates the shards for the frost core when hit ======================
            int frostdamage = (int)((attackdmg * .75f));
            if (frostdamage > 150)
            {
                frostdamage = 150;
            }
            if (frostSpike)
            {
                if (frosttime >= 360 && damage > 1)
                {
                    
                    SoundEngine.PlaySound(SoundID.NPCDeath56 with { Volume = 0.2f, Pitch = -0.5f}, Player.Center);
                    float numberProjectiles = 10 + Main.rand.Next(4);
                    for (int i = 0; i < numberProjectiles; i++)
                    {


                        float speedX = 0f;
                        float speedY = -9f;
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(150));
                        float scale = 1f - (Main.rand.NextFloat() * .5f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(null, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<FrostAccessProj>(), frostdamage, 3f, Player.whoAmI);

                    }
                    for (int i = 0; i < 30; i++)
                    {

                        Dust dust;
                        // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                        Vector2 position = Main.LocalPlayer.position;
                        dust = Main.dust[Terraria.Dust.NewDust(position, Player.width, Player.height, 92, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                        dust.noGravity = true;
                    }
                    frosttime = 0;
                    if (attackdmg < Player.statLife)
                    {
                        Player.AddBuff(ModContent.BuffType<FrozenBuff>(), 360);
                    }
                }
            }

        }
        //===================================Other hooks======================================

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) //Hitting enemies with True Melee Only
        { 
          
            //for the Beetle Gauntlet
            if (beetleFist)
            {
                if (!Player.dead && crit)
                {

                    SoundEngine.PlaySound(SoundID.Zombie50 with { Volume = 2f, Pitch = -0.5f }, target.Center);

                    float numberProjectiles = 3 + Main.rand.Next(3);

                    for (int i = 0; i < numberProjectiles; i++)
                    {


                        float speedX = 0f;
                        float speedY = -12f;
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(135));
                        float scale = 1f - (Main.rand.NextFloat() * .5f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(null, new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BeetleGloveProj>(), 45, 1, Player.whoAmI);

                    }
                    for (int i = 0; i < 25; i++)
                    {

                        var dust = Dust.NewDustDirect(target.position, target.width, target.height, 186);
                        dust.velocity *= 2;
                        dust.noGravity = true;
                        dust.scale = 1.5f;

                    }
                    for (int i = 0; i < 25; i++)
                    {

                        var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 186);
                        dust.velocity *= 2;
                        dust.noGravity = true;
                        dust.scale = 1;

                    }


                }

            }
         
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) //Hitting enemy with any projectile
        {     
          
            /*if (heartSteal) //For the Jar of hearts
            {
                if (!target.SpawnedFromStatue && target.life <= (target.lifeMax * 0.50f) && !target.boss && !target.friendly && target.lifeMax > 5 && !target.buffImmune[(BuffType<HeartDebuff>())]) //Rolls to see the outcome when firts hit under 50% life
                {

                    if (!target.GetGlobalNPC<NPCEffects>().heartStolen)//Makes sure this only happens once
                    {
                        if (Main.rand.Next(4) == 0) //1 in 4 chance to have the debuff applied and drop a heart
                        {
                            Item.NewItem(new EntitySource_Loot(target), new Vector2(target.Center.X, target.Center.Y), new Vector2(target.width, target.height), ModContent.ItemType<Items.Tools.SuperHeartPickup>());


                            SoundEngine.PlaySound(SoundID.NPCDeath7, target.Center);
                            for (int i = 0; i < 15; i++)
                            {
                                var dust = Dust.NewDustDirect(new Vector2(target.Center.X, target.Center.Y), 5, 5, 72);
                                //dust.noGravity = true;
                            }
                            target.AddBuff(ModContent.BuffType<HeartDebuff>(), 3600);
                            target.GetGlobalNPC<NPCEffects>().heartStolen = true; //prevents more hearts from being dropped

                        }
                        else //Otherwise it just prevents the roll from happening again
                        {
                            target.GetGlobalNPC<NPCEffects>().heartStolen = true;
                        }
                    }
                }
            }*/                 

            //for the Beetle Gauntlet
            if (beetleFist)
            {            
                if (!Player.dead && proj.CountsAsClass(DamageClass.Melee) && crit && proj.type != ModContent.ProjectileType<BeetleGloveProj>())
                {
                    SoundEngine.PlaySound(SoundID.Zombie50 with { Volume = 2f, Pitch = -0.5f }, target.Center);

                    float numberProjectiles = 3 + Main.rand.Next(3);

                    for (int i = 0; i < numberProjectiles; i++)
                    {


                        float speedX = 0f;
                        float speedY = -12f;
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(135));
                        float scale = 1f - (Main.rand.NextFloat() * .5f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(null, new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BeetleGloveProj>(), 45, 1, Player.whoAmI);

                    }
                    for (int i = 0; i < 25; i++)
                    {

                        var dust = Dust.NewDustDirect(target.position, target.width, target.height, 186);
                        dust.velocity *= 2;
                        dust.noGravity = true;
                        dust.scale = 1.5f;

                    }
                    for (int i = 0; i < 25; i++)
                    {

                        var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 186);
                        dust.velocity *= 2;
                        dust.noGravity = true;
                        dust.scale = 1;

                    }


                }
                
            }
        
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (woodNecklace && Player.ZoneForest)
            {
                damage -= 4;
            }
            //for Enchanted Mushroom
            if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff1>()))
            {
                damage -= 1;
            }
            else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff2>()))
            {
                damage -= 2;

            }
            else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff3>()))
            {
                damage -= 4;

            }
            else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff4>()))
            {
                damage -= 6;

            }
            /*if (damage >= Player.statLife && Player.statLife > 1)
            {
                damage = Player.statLife - 1;
                SoundEngine.PlaySound(SoundID.Item, (int)Player.position.X, (int)Player.position.Y, 122);

            }*/
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
        }
        public override void OnHitByNPC(NPC npc, int damage, bool crit) //Hit by melee only
        {

        }
        public override void OnHitByProjectile(Projectile proj, int damage, bool crit) //Hit by any projectile
        {

        }
     
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            /*
            {
                if (Player.HasBuff(BuffType<ShroomiteBuff>()) && Main.rand.Next(100) <= 75)//If the player has the shroomite potion then 50% chance not to consume ammo
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            */
            return true;
        }
       
    }

}