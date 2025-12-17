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
using NVorbis.Contracts;
using Terraria.GameContent.Drawing;
using Microsoft.CodeAnalysis;
using StormDiversMod.Items.Weapons;
using StormDiversMod.Items.Accessory;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StormDiversMod.Assets.Achievements;

namespace StormDiversMod.Common
{
   
    public class EquipmentEffects : ModPlayer
    {
        //Bools activated from accessories/pets/single armour pieces

        public bool goldDerpie; //The player has the Golden Derpie Pet

        public bool mrStabbyPet; //The player has the Mini Stabby pet

        public bool stormHelmet; //The player has the Storm Diver Pet

        public bool twilightPet; //The player has the Twilight Light Pet

        public bool stormBossPet; //The player has the Scandrone Pet

        public bool aridBossPet; //The player has the Ancient Husk Pet

        public bool painBossPet; //The player has the Painful pet


        public bool shroombuff; //The Player has the Ranged enhancement potion buff

        public bool flameCore; //The player has Betsy's Flame equipped

        public bool frostSpike; //The Player has the Cryo Core equipped

        public bool lunarBarrier; //The player has the Celestial Barrier equipped

        public bool primeSpin; //The player has the Mech Spikes equipped

        public bool bootFall; //The player has either heavy boots equipped

        public bool bootFallLuck; //The player has the heavy horseshoe boots equipped
       
        public bool bootFallDrill; //The player has the drill boots equipped

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

        public bool derpEyeGolem; //Player had the Jungle Eye equipped

        public bool shockDerpEye; //Player has the Storm Eye equipped

        public bool blueCuffs; //Player has insulated cuffs equipped

        public bool blueClaws; //Player has glacial claws equipped

        public bool spookyClaws; //Player has nightmare claws equipped

        public bool lunaticHood; //Player has the Luantic Hood equipped

        public bool beetleFist; //player has the Beetle gauntlet equipped

        public bool aridCritChest; //Player has the Arid Chestplate equipped

        public bool soulBoots; //Player has Soul Striders equipped;

        public bool bloodBoots; //Player has Blood Treads equipped;

        public bool woodNecklace; //Player has the wooden necklace equipped

        public bool coralEmblem; //Player has coral Emblem equipped

        public bool coralStorm; //Player has Eye of the Storm equipped

        public bool stormBossAccess; //Player has the Storm Coil equipped

        public bool aridBossAccess; //Player has the Ancient Emblem equipped

        public bool mushroomSuper; //Player has the enchnated mushroom equipped

        public bool heartpotion; //Player has taken a heart potion

        public bool superHeartpotion; //Player has taken a super heart potion

        public bool DeathCore; //Player has Prignerbringer Core equipped

        public bool SantaCore; //Player has santa Core equipped

        public bool frozenNecklace; //Player has Frost necklace equipped

        public bool desertNecklace; //Player has manible necklace equipped

        public bool deathList; //player has Repear's List equipped

        public bool shockBand; //player has the Storm Charm 

        public bool shockBandQuiver; //player has storm quiver equipped 

        //Ints and Bools activated from this file

        public bool shotflame; //Indicates whether the Betsy Flame has fired its flames or not
        public int flamecooldown; //Cooldown for betsy's flame with channeling weapons
        public bool spikespawned; //Wheter mechanical spieks ahev been spawned
        public bool falling; //Wheter the player is falling at speed
        public int stopfall; //If the player has stopped falling
        public int bearcool; //Cooldown for the Teddy Bear
        public int stomptrail; //Delay of the projectuiles of the trail when falling with the boots
        public int frosttime; //Cooldown of the frost shards from the Cryo Core
        public bool desertdustspawned; //has the Pahroh's urn creaed the oprbiting projectile?
        public int granitebufftime; //Cooldown for the granite Accessory Buff to be reapplied
        public bool granitesurge; //Makes it so the granite accessory cooldown can start and makes it so the next attack removes the buff
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
        public int coralstormdrop; //Cooldown for Eye of Storm projs with channeling weapons
        public int coralstormcount; //make Eye of the Storm shoot every other shot
        public bool stormBossProj; // wheter the projectile for the storm coil has been spawned
        public int shroomtime; //For cahnneling ranged weapons with Shroomite launcher
        public int paintime; //Cooldown for reliving pain
        public int SantaRevivedCooldown;
        public bool SantaRevived;
        public int bootdmg; //damage of the boots
        public bool bootstompjump; //if jmp boost can be done
        public int bootstompjumptime; //time for jump boost
        public bool bootsound; //sound for boot stomp
        public bool bootdrillmining; //Are the Drill boots mining?
        public int bootdrillsound; //Sound delay for boot drill

        public int ariddegrees; //degrees for the arid emblem dust
        public int ariddistance; //distance for the arid emblem dust

        public int ninelives; //how many kills with the deathlist, up to 9
        public int ninelivescooldown; //cooldown to remove a soul

        public bool shockedtarget; //has the Shock band shocked a target?
        public int shockcooldown; //10 frame cooldown for the shocked target

        //items here for the stupid GetSource_Accessory thing for projectiles

        public Item CelestialbarrierItem;
        public Item DeathCoreItem;
        public Item BloodBootsItem;
        public Item DesertJarItem;
        public Item PrimeSpinItem;
        public Item BootFallItem;
        public Item BootDrillItem;
        public Item CoralEmblemItem;
        public Item CoralStormItem;
        public Item ShroomAccessItem;
        public Item FlameCoreItem;
        public Item StormBossAccessItem;
        public Item LunaticHoodItem;
        public Item UltimateBossMaskItem;
        public Item FrostSpikeItem;
        public Item BeetleFistItem;
        public Item DesertNecklaceItem;
        public Item DeathlistItem;
        public Item ShockbandItem;

        public bool ultimateBossMask; //player has Painbringer mask equipped
        public override void ResetEffects() //Resets bools if the item is unequipped
        {
            goldDerpie = false;
            mrStabbyPet = false;
            stormHelmet = false;
            twilightPet = false;
            stormBossPet = false;
            aridBossPet = false;
            painBossPet = false;

            shroombuff = false;
            flameCore = false;
            frostSpike = false;
            lunarBarrier = false;
            primeSpin = false;
            bootFall = false;
            bootFallLuck = false;
            bootFallDrill = false;

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
            derpEyeGolem = false;
            shockDerpEye = false;
            blueCuffs = false;
            blueClaws = false;
            spookyClaws = false;
            lunaticHood = false;
            beetleFist = false;
            aridCritChest = false;
            soulBoots = false;
            bloodBoots = false;
            woodNecklace = false;
            coralEmblem = false;
            coralStorm = false;
            stormBossAccess = false;
            aridBossAccess = false;
            mushroomSuper = false;
            heartpotion = false;
            superHeartpotion = false;
            DeathCore = false;
            frozenNecklace = false;
            desertNecklace = false;
            SantaCore = false;
            deathList = false;
            shockBand = false;
            shockBandQuiver = false;
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
            coralstormdrop = 0;
            stormBossProj = false;
            shotflame = false;
            flamecooldown = 0;
            paintime = 0;
            ultimateBossMask = false;
            SantaRevived = false;
            SantaRevivedCooldown = 0;
            bootsound = false;
            ninelives = 0;
            ninelivescooldown = 0;
            shockedtarget = false;
            shockcooldown = 0;
        }
        public override void PreUpdate()
        {
            base.PreUpdate();
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

           //Doesn't work, displayed ammo damage increases like normal, but actual damage doesn't
            /*if (Player.HasBuff(BuffType<ShroomiteBuff>()))//If the player has the shroomite potion then 10% increase ammo damage
            {
                if (item.CountsAsClass(DamageClass.Ranged) && item.ammo != 0)
                {
                    damage *= 10.1f;
                    if (item.damage <= 10)
                    {
                        damage.Flat += 1;
                    }

                }
            }*/
            //for Enchanted Mushroom
            if (item.ammo == 0)
            {
                if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff1>()))
                {
                    damage.Flat += 1;
                }
                else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff2>()))
                {
                    damage.Flat += 3;
                }
                else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff3>()))
                {
                    damage.Flat += 5;
                }
                /*else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff4>()))
                {
                    damage.Flat += 6;
                }*/

            }
        }

        public override void ModifyZoom(ref float zoom)
        {
            if (Player.controlUseTile && Player.noThrow == 0)
            {
                if (Player.HeldItem.type == ModContent.ItemType<BloodyRifle>())
                {
                    if (Player.scope)
                        zoom = 0.5f;
                    else
                        zoom = 0.33f;
                }
                if (Player.HeldItem.type == ModContent.ItemType<MechanicalRifle>())
                {
                    if (Player.scope)
                        zoom = 0.66f;
                    else
                        zoom = 0.4f;
                }
                if (Player.HeldItem.type == ModContent.ItemType<ShroomiteSharpshooter>())
                {
                    if (Player.scope)
                        zoom = 0.75f;
                    else
                        zoom = 0.5f;
                }
            }
        }
        float painincrease;
        public override void PostUpdateEquips() //Updates every frame
        {
            /*if (Player.sitting.isSitting || Player.controlDown)
            {
                Player.hasFloatingTube = false;
                Player.cFloatingTube = 0;
                Player.canFloatInWater = false;
            }*/
            //Reduces ints if they are above 0 and not in the equip field
            if (beetleFist)
            {
                if (beetlecooldown > 0)
                {
                    beetlecooldown--;
                }
            }
            else
                beetlecooldown = 0;
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

            if (graniteBuff && !Player.HasBuff(ModContent.BuffType<GraniteAccessBuff>())) //don't increase timer if buff is active
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
            if (shockcooldown > 0)
                shockcooldown--;
            if (!shockBand && !shockBandQuiver && !shockDerpEye)
                shockedtarget = false;

            if (bearcool > 0)
            {
                bearcool--;
            }
            if (paintime > 0)
            {
                //Player.lifeRegen = 0;
                //Player.lifeRegenCount = 0;
                paintime--;
            }
            else
            {
                Player.ClearBuff(ModContent.BuffType<PainlessDebuff>());
                if (DeathCore)
                {
                    Player.AddBuff(ModContent.BuffType<PainBuff>(), 2);

                    if (Main.rand.Next(10) == 0)
                    {
                        int xprojpos = Main.rand.Next(-40, 40);
                        int yprojpos = Main.rand.Next(-40, 40);
                        int proj = Projectile.NewProjectile(Player.GetSource_Accessory(DeathCoreItem), new Vector2(Player.Center.X + xprojpos, Player.Center.Y - yprojpos), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.PainCoreProj>(), 0, 0, Main.myPlayer);
                    }
                }
            }
            if (DeathCore)
            {
                Player.GetDamage(DamageClass.Generic) += painincrease / 100 / 4;
                painincrease = (100 - ((float)(Player.statLife) / (float)(Player.statLifeMax2)) * 100);
                //Main.NewText(painincrease / 4, Color.Red);
            }
            //======================================================================================Accessories/other======================================================================================
            //luck
            //Main.NewText("Your coin luck is: " + Player.coinLuck + " Your regular luck is: " + Player.luck, 204, 101, 22);

            if (derpEye)
                Player.luck += 0.3f;
            if (derpEyeGolem)
                Player.luck += 0.3f;
            if (shockDerpEye)
                Player.luck += 0.3f;
            if (bootFallLuck)
                Player.luck += 0.05f;

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
                //code in npceffects
                /*if (Main.LocalPlayer.HasBuff(BuffID.ManaSickness))
                {                   
                        Player.manaCost *= 0f;

                    Dust dust;
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Vector2 position = Main.LocalPlayer.position;
                    dust = Main.dust[Terraria.Dust.NewDust(position, Player.width, Player.height, 15, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                    dust.noGravity = true;
                }*/
            }
            //For Soul/Blood Striders============================================================
            var tilePos = Player.Bottom.ToTileCoordinates16();
            var tileposgrav = Player.Top.ToTileCoordinates16();
            if (soulBoots || bloodBoots)
            {
                //int xboottilepos = (int)(Player.position.X + (float)(Player.width / 2)) / 16;
                //int yboottilepos = (int)(Player.Bottom.Y / 16);

                //SPEEDS!

                if (Framing.GetTileSafely(tilePos.X, tilePos.Y).TileType == TileID.Asphalt || (Player.gravDir == -1 && Framing.GetTileSafely(tileposgrav.X, tileposgrav.Y - 1).TileType == TileID.Asphalt))//When on asphalt 
                {
                    if (soulBoots)
                    {
                        Player.maxRunSpeed += 1.7f;
                        Player.runAcceleration *= 1.5f;
                        Player.runSlowdown = 0.2f;
                    }
                    else if (bloodBoots)
                    {
                        Player.maxRunSpeed += 0.8f;
                        Player.runAcceleration *= 1.2f;
                        Player.runSlowdown = 0.15f;
                    }

                }
                else if (Framing.GetTileSafely(tilePos.X, tilePos.Y).TileType != TileID.Asphalt && Player.velocity.Y == 0)//When not on asphalt 
                {
                    if (soulBoots)
                    {
                        Player.maxRunSpeed += 6.2f;
                        Player.runAcceleration *= 2;
                        Player.runSlowdown = 0.5f;
                    }
                    else if (bloodBoots)
                    {
                        Player.maxRunSpeed += 4.2f;
                        Player.runAcceleration *= 1.3f;
                        Player.runSlowdown = 0.35f;
                    }
                }
                else if (Player.velocity.Y != 0)//When in the air
                {
                    if (soulBoots)
                    {
                        Player.maxRunSpeed += 4f;
                        Player.runAcceleration *= 1.4f;
                        Player.runSlowdown = 0.25f;
                    }
                    else if (bloodBoots)
                    {
                        Player.maxRunSpeed += 3.2f;
                        Player.runAcceleration *= 1.1f;
                        Player.runSlowdown = 0.2f;
                    }
                }

                /*if (soulBoots)
                {
                    Player.rocketBoots = 1;
                    if (Main.dayTime)
                    {
                        Player.vanityRocketBoots = 2;
                    }
                    else
                    {
                        Player.vanityRocketBoots = 1;
                        Player.socialShadowRocketBoots = true;
                    }
                }*/

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
                                Projectile.NewProjectile(Player.GetSource_Accessory(BloodBootsItem), new Vector2(Player.Center.X, Player.Bottom.Y - 4), new Vector2(0, 0), ModContent.ProjectileType<BloodBootProj>(), 20, 0, Player.whoAmI);

                                SoundEngine.PlaySound(SoundID.Run, Player.Center);

                                soundDelay = 0;
                            }
                            else
                            {
                                Projectile.NewProjectile(Player.GetSource_Accessory(BloodBootsItem), new Vector2(Player.Center.X, Player.Top.Y - 4), new Vector2(0, 0), ModContent.ProjectileType<BloodBootProj>(), 20, 0, Player.whoAmI);

                                SoundEngine.PlaySound(SoundID.Run, Player.Center);
                                soundDelay = 0;
                            }
                        }
                    }
                }
            }
            //For Spooky Core======================
            if (spooked || spookyClaws)
            {
                float distancehealth = 300 + ((Player.statLifeMax2 - Player.statLife) / 2);
                if (distancehealth > 600)
                {
                    distancehealth = 600;
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
                //Vector2 spawnPos = Projectile.Center + Main.rand.NextVector2CircularEdge(Range, Range);
                for (int i = 0; i < 8; i++)
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
                    Vector2 velocity = Vector2.Normalize(new Vector2(Player.Center.X, Player.Center.Y) - new Vector2(dustx, dusty)) * 10;
                    if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, Player.Center, 1, 1))//no dust unless line of sight
                    {
                        var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 200, velocity.X, velocity.Y);
                        dust.noGravity = true;
                        dust.velocity *= 1f;
                        dust.scale = 1.5f;
                        dust.fadeIn = 0.5f;
                    }
                }
                for (int i = 0; i < 200; i++)
                {
                    NPC target = Main.npc[i];
                    var player = Main.LocalPlayer;

                    if (Vector2.Distance(Player.Center, target.Center) <= distancehealth && !target.friendly && target.lifeMax > 5 && !target.dontTakeDamage && target.active && target.type != NPCID.TargetDummy && Collision.CanHit(Player.Center, 0, 0, target.Center, 0, 0))
                    {
                        if (!target.buffImmune[(BuffType<SpookedDebuff>())])
                        {
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
                        Projectile.NewProjectile(Player.GetSource_Accessory(DesertJarItem), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.DesertJarProj>(), 40, 0, Player.whoAmI);
                        dropdust = 0;

                        //Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 13);
                    }
                }
                if (!desertdustspawned) //spawn the 2 orbiting orojs
                {
                    Projectile.NewProjectile(Player.GetSource_Accessory(DesertJarItem), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<DesertJarProj2>(), 40, 0f, Player.whoAmI, 0, 0);
                    Projectile.NewProjectile(Player.GetSource_Accessory(DesertJarItem), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<DesertJarProj2>(), 40, 0f, Player.whoAmI, 0, 180);

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
                    if (Player == Main.LocalPlayer)
                    {
                        Projectile.NewProjectile(Player.GetSource_Accessory(PrimeSpinItem), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 0);
                        Projectile.NewProjectile(Player.GetSource_Accessory(PrimeSpinItem), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 60);

                        Projectile.NewProjectile(Player.GetSource_Accessory(PrimeSpinItem), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 120);
                        Projectile.NewProjectile(Player.GetSource_Accessory(PrimeSpinItem), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 180);

                        Projectile.NewProjectile(Player.GetSource_Accessory(PrimeSpinItem), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 240);
                        Projectile.NewProjectile(Player.GetSource_Accessory(PrimeSpinItem), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<PrimeAccessProj>(), 80, 0f, Player.whoAmI, 0, 300);

                        spikespawned = true;
                    }
                }
            }
            if (!primeSpin)//reset bool
            {
                spikespawned = false;
            }

            //For the Heavy Boots===========================
            if (bootFallLuck)
            {

            }
            if (bootFall && !bootFallDrill)
            {
                //Player.rocketBoots = 1;            
                //Player.vanityRocketBoots = 1;
                if ((Player.controlDown) && !Player.controlJump && Player.velocity.Y != 0 && !Player.mount.Active)
                {
                    Player.ignoreWater = true;
                    Player.extraFall += 10;

                    Player.grappling[0] = -1; //Remove grapple hooks
                    Player.grapCount = 0;
                    for (int p = 0; p < 1000; p++)
                    {
                        if (Main.projectile[p].active && Main.projectile[p].owner == Player.whoAmI && Main.projectile[p].aiStyle == 7)
                        {
                            Main.projectile[p].Kill();
                        }
                    }
                    //SoundEngine.PlaySound(SoundID.Item, (int)Player.Center.X, (int)Player.Center.Y, 15, 2, -0.5f);
                    Player.gravity += 1.2f;
                    Player.maxFallSpeed *= 1.5f;

                    Player.runAcceleration = 0.25f;
                    if ((Player.velocity.Y > 8 && Player.gravDir == 1) || (Player.velocity.Y < -8 && Player.gravDir == -1))
                    {
                        if (!falling)
                        {
                            Player.velocity.X *= 0.5f;
                            Projectile.NewProjectile(Player.GetSource_Accessory(BootFallItem), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 5), ModContent.ProjectileType<StompBootProj2>(), 10, 6, Player.whoAmI);

                        }
                        //immunity is in miscfeatures.cs
                        falling = true;
                        Player.noKnockback = true;

                        if (bootdmg < 110) //(10-120 damage)
                            bootdmg += 2;

                        //Main.NewText("The damage is: " + bootdmg, 204, 101, 22);
                    }

                }
                //For impacting the ground at speed
                if (Player.velocity.Y == 0 && falling && (Player.controlDown))
                {
                    //if (!GetInstance<ConfigurationsIndividual>().NoShake)
                    {
                        if (bootdmg >= 30)
                            Player.GetModPlayer<MiscFeatures>().screenshaker = true;
                    }

                    for (int i = 0; i < 30; i++)
                    {

                        int dustIndex = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 31, 0f, 0f, 100, default, 1f);
                        Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].noGravity = true;
                    }
                    for (int i = 0; i < (int) (bootdmg / 10); i++)
                    {
                        int goreIndex = Gore.NewGore(Player.GetSource_Accessory(BootFallItem), new Vector2(Player.position.X - 4, Player.Bottom.Y - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 2f;
                        Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 2f;
                        goreIndex = Gore.NewGore(Player.GetSource_Accessory(BootFallItem), new Vector2(Player.position.X - 4, Player.Bottom.Y - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 2f;
                        Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 2f;
                        goreIndex = Gore.NewGore(Player.GetSource_Accessory(BootFallItem), new Vector2(Player.position.X - 4, Player.Bottom.Y - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 2f;
                        Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 2f;
                        goreIndex = Gore.NewGore(Player.GetSource_Accessory(BootFallItem), new Vector2(Player.position.X - 4, Player.Bottom.Y - 4f), default(Vector2), Main.rand.Next(61, 64), 1f);
                        Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 2f;
                        Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 2f;
                    }
                    //Main.NewText(" " + (int) (bootdmg / 10), Color.Red);
                    Projectile.NewProjectile(Player.GetSource_Accessory(BootFallItem), new Vector2(Player.Center.X, Player.Right.Y + 2 * Player.gravDir), new Vector2(5, 0), ModContent.ProjectileType<StompBootProj>(), bootdmg + 10, 12f, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_Accessory(BootFallItem), new Vector2(Player.Center.X, Player.Left.Y + 2 * Player.gravDir), new Vector2(-5, 0), ModContent.ProjectileType<StompBootProj>(), bootdmg + 10, 12f, Player.whoAmI);

                    bootstompjump = true;
                    bootstompjumptime = 15; //jump boost, 15 frame buffer after landing

                    SoundEngine.PlaySound(SoundID.Item14, Player.Center);
                    bootdmg = 0;
                    falling = false;
                }
                if ((!Player.controlDown) || Player.controlJump || Player.mount.Active || Player.controlUp) //cancels stomp
                {
                    falling = false;
                    bootdmg = 0;
                }

                //Main.NewText("Stomp Time: " + bootstompjumptime, 204, 101, 22);
                //Main.NewText("Stomp Active: " + bootstompjump, 204, 101, 22);

                //For the bounce

                if (bootstompjumptime > 0 && (Player.velocity.Y >= 0 || !Player.controlJump)) // As player is on floor reduce time for jump to be activated
                {
                    bootstompjumptime--;
                }

                if (bootstompjumptime <= 0 || Player.mount.Active) //if timer runs out remove the super jump
                {
                    bootstompjumptime = 0;
                    bootstompjump = false;
                }
                if (bootstompjump && Player.controlJump) //Super jump
                {
                    if (Player.velocity.Y == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item150 with { Pitch = -0.5f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Player.Center);
                    }
                    //achievement get
                    ModContent.GetInstance<AchievementStompBounce>().AchStompCondition.Complete();

                    Player.frogLegJumpBoost = true;
                    Player.jumpHeight += 8;
                    Player.jumpSpeedBoost += 1.3f;

                    Player.maxRunSpeed += 1.5f;
                    Player.runAcceleration *= 1.5f;

                    if (Player.velocity.Y < 0)
                    {
                        int dustIndex = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 31, 0f, 0f, 100, default, 1f);
                        Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                        Main.dust[dustIndex].noGravity = true;
                    }
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
            if (bootFallDrill)
            {
                //Player.rocketBoots = 1;            
                //Player.vanityRocketBoots = 1;
                if ((Player.controlDown) && !Player.controlJump && Player.velocity.Y != 0 && !Player.mount.Active)
                {
                    Player.ignoreWater = true;
                    Player.wet = false;
                    Player.extraFall += 10;

                    Player.grappling[0] = -1; //Remove grapple hooks
                    Player.grapCount = 0;
                    for (int p = 0; p < 1000; p++)
                    {
                        if (Main.projectile[p].active && Main.projectile[p].owner == Player.whoAmI && Main.projectile[p].aiStyle == 7)
                        {
                            Main.projectile[p].Kill();
                        }
                    }
                    if (!bootdrillmining)
                    {
                        Player.maxFallSpeed *= 1.5f;
                        Player.gravity += 1.2f;
                    }
                    else
                    {
                        if (!Player.wet)
                        Player.maxFallSpeed *= 0.33f;//slower falling when drilling
                        Player.moveSpeed /= 2;
                        Player.maxRunSpeed -= 1;
                    }
                    Player.runAcceleration = 0.25f;
                    if ((Player.velocity.Y > 0.1f && Player.gravDir == 1) || (Player.velocity.Y < -0.1f && Player.gravDir == -1))
                    {
                        if (!falling)
                        {
                            SoundEngine.PlaySound(SoundID.Item23 with { Volume = 1f, MaxInstances = 5 }, Player.Center);

                            Player.velocity.X *= 0.5f;
                            Projectile.NewProjectile(Player.GetSource_Accessory(BootDrillItem), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 5), ModContent.ProjectileType<StompBootDrillProj>(), 10, 6, Player.whoAmI);
                        }
                        //immunity is in miscfeatures.cs
                        falling = true;
                        Player.noKnockback = true;

                        //if (bootdmg < 110) //(10-120 damage)
                        //    bootdmg += 2;

                        //Main.NewText("The damage is: " + bootdmg, 204, 101, 22);
                    }
                    if (falling)
                    {
                        bootdrillsound++;
                        if (bootdrillsound > 30)
                        {
                            SoundEngine.PlaySound(SoundID.Item22 with { Volume = 1f, MaxInstances = 1 }, Player.Center);
                            bootdrillsound = 0;
                        }
                    }
                }
                if (Player.velocity.Y == 0 && falling && (Player.controlDown))
                {
                    //SoundEngine.PlaySound(SoundID.Item14, Player.Center);
                    //bootdmg = 0;
                    falling = false;
                }
                if ((!Player.controlDown) || Player.controlJump || Player.mount.Active || Player.velocity.Y == 0) //cancels stomp
                {
                    bootdrillmining = false;
                    falling = false;
                    //bootdmg = 0;
                }
            }
            //If boots are unequipped then cancel the bool
            if (!bootFall && !bootFallDrill)
            {
                falling = false;
                bootstompjump = false;
                bootstompjumptime = 0;
            }
            //For Coral Emblem
            if (coralEmblem)
            {
                if (Player.HeldItem.damage >= 1) //If the player is holding a weapon and usetime cooldown is above 1
                {
                    coraldrop++;

                    if ((!Player.channel && (Player.itemAnimation == Player.itemAnimationMax)  || (Player.HeldItem.channel && Player.channel)) && coraldrop >= 60 && Player.itemAnimation != 0)
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
                            int projID = Projectile.NewProjectile(Player.GetSource_Accessory(CoralEmblemItem), new Vector2(vector2_1.X, vector2_1.Y), new Vector2(SpeedX, SpeedY), ModContent.ProjectileType<OceanSpellProj>(), 25, 0.5f, Player.whoAmI, 0.0f, (float)Main.rand.Next(5));

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
            //For Riptide Emblem
            if (coralStorm)
            {
                //Main.NewText("WTF " + Player.itemAnimation, Color.Orange);
                if (Player.HeldItem.damage >= 1 && Player.HeldItem.pick <= 1 && Player.HeldItem.axe <= 1 && Player.HeldItem.hammer <= 1) //If the player is holding a weapon and usetime cooldown is above 1
                {
                    coralstormdrop++;

                    if ((Player.channel && coralstormdrop >= Player.HeldItem.useTime) || (!Player.channel && Player.itemAnimation == (Player.itemAnimationMax) && Player.itemAnimation != 0)) //If the player is holding a weapon and has just swung, item animation coutns down
                    {
                        coralstormcount++;
                        if (coralstormcount == 2)
                        {
                            Vector2 velocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(Player.Center.X, Player.Center.Y)) * 8;
                            int type = ModContent.ProjectileType<CoralBoneProj>();
                            //int damage = (int)Player.GetTotalDamage(DamageClass.Generic).ApplyTo(20);
                            int damage = (int)Player.GetTotalDamage(DamageClass.Generic).ApplyTo((int)(Player.HeldItem.damage * 0.5f));

                            int projID = Projectile.NewProjectile(Player.GetSource_Accessory(CoralStormItem), Player.Center, velocity, type, damage, 2f, Player.whoAmI);

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

                            coralstormcount = 0;
                        }
                        coralstormdrop = 0;
                    }
                }
            }
            else
            {
                coralstormdrop = 0;
                coralstormcount = 0;
            }
            // For the Shroomite Launcher Accessory
            if (shroomaccess)
            {
                shroomtime++;
                //Channeling weapons fire every time half the usetime is met with a counter
                if (((Player.channel && shroomtime >= Player.HeldItem.useTime / 2) || (!Player.channel && Player.itemAnimation == Player.itemAnimationMax)) && Player.HeldItem.CountsAsClass(DamageClass.Ranged) 
                    && Player.HeldItem.useAmmo == AmmoID.Bullet && Player.itemAnimation != 0) //If the player is holding a ranged weapon and weapon just fired (useaniamtion counts down)
                {
                    shroomshotCount++;
                    //Main.NewText("Pls work " + shroomtime + " | " + shroomshotCount, 0, 204, 170); //Inital Scale

                    shroomtime = 0;

                    if (shroomshotCount >= 5) //Every 5 shots fires a rocket
                    {
                        shroomshotCount = 0; //Resets the shot count
                        float rotation = Player.itemRotation + (Player.direction == -1 ? (float)Math.PI : 0); //the direction the item points in
                        float velocity = 20f;
                        int type = ModContent.ProjectileType<ShroomSetRocketProj>();
                        int damage = (int)Player.GetTotalDamage(DamageClass.Ranged).ApplyTo((int)(Player.HeldItem.damage * 2f));
                        Projectile.NewProjectile(Player.GetSource_Accessory(ShroomAccessItem), Player.Center, new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * velocity, type, damage, 2f, Player.whoAmI);

                        SoundEngine.PlaySound(SoundID.Item92, Player.Center);
                    }
                }
            }
            else
            {
                shroomtime = 0;
            }
            //For betsy's Flame ======================
            if (flameCore)
            {
                if (flamecooldown > 0)
                {
                    flamecooldown--;
                }
                shotflame = false;

                //Player.runAcceleration += 0.25f;
                for (int i = 0; i < 200; i++)
                {
                    NPC target = Main.npc[i];

                    if (Vector2.Distance(Player.Center, target.Center) <= 600 && !target.friendly && target.lifeMax > 5 && !target.dontTakeDamage && target.active && target.type != NPCID.TargetDummy && Collision.CanHit(Player.Center, 0, 0, target.Center, 0, 0))
                    {
                        if ((Player.itemAnimation == 1 && Player.HeldItem.damage >= 1 && Player.HeldItem.ammo == 0) || (Player.HeldItem.channel && Player.channel && flamecooldown <= 0)) //weapon is in use
                        {
                            if (Main.rand.Next(3) == 0 && !shotflame)
                            {
                                for (int j = 0; j < 20; j++)
                                {
                                    var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 244);

                                    dust.noGravity = true;
                                }

                                SoundEngine.PlaySound(SoundID.Item34, Player.Center);

                                float numberProjectiles = 2 + Main.rand.Next(2);

                                for (int k = 0; k < numberProjectiles; k++)
                                {
                                    float rotation = Player.itemRotation + (Player.direction == -1 ? (float)Math.PI : 0); //the direction the item points in


                                    float speedX = 0f;
                                    float speedY = -4f;
                                    int damage = (int)(Player.HeldItem.damage * 0.7f);
                                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(90));
                                    float scale = 1f - (Main.rand.NextFloat() * .5f);
                                    perturbedSpeed = perturbedSpeed * scale;
                                    Projectile.NewProjectile(Player.GetSource_Accessory(FlameCoreItem), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y * Player.gravDir), ModContent.ProjectileType<BetsyFlameProj>(), damage, 1, Player.whoAmI);
                                }
                                shotflame = true;
                            }
                            flamecooldown = Player.HeldItem.useTime; //cooldown for channeling weapons
                        }
                    }
                }
            }
            //For wooden necklace=======================
            if (woodNecklace)
            {
                if (((Player.ZoneForest || Player.ZoneHallow) && Player.ZoneOverworldHeight) || (frozenNecklace && Player.ZoneSnow) || (desertNecklace && Player.ZoneDesert))
                {
                    Player.AddBuff(ModContent.BuffType<WoodenBuff>(), 2);
                    Player.lifeRegen += 1;
                }
            }

            //For Frozen Pendant======================
            if (frozenNecklace)
            {
                Player.buffImmune[BuffID.Chilled] = true;
                if (Player.ZoneSnow || (woodNecklace && (Player.ZoneForest || Player.ZoneHallow) && Player.ZoneOverworldHeight) || (desertNecklace && Player.ZoneDesert))
                {
                    Player.AddBuff(ModContent.BuffType<WoodenBlizzardBuff>(), 2);

                    //circle of dust
                    for (int i = 0; i < 6; i++)
                    {
                        double deg = Main.rand.Next(0, 360); //The degrees
                        double rad = deg * (Math.PI / 180); //Convert degrees to radians
                        double dist = 150; //Distance away from the player
                        float dustx = Player.Center.X - (int)(Math.Cos(rad) * dist);
                        float dusty = Player.Center.Y - (int)(Math.Sin(rad) * dist);
                        if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, Player.position, Player.width, Player.height))//no dust unless line of sight
                        {
                            var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 156, 0, 0);
                            dust.noGravity = true;
                            dust.scale = 1.25f;
                        }
                        Vector2 velocity = Vector2.Normalize(new Vector2(Player.Center.X, Player.Center.Y) - new Vector2(dustx, dusty)) * 10;
                        if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, Player.Center, 1, 1))//no dust unless line of sight
                        {
                            var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 156, velocity.X, velocity.Y);
                            dust.noGravity = true;
                            dust.velocity *= 1f;
                            dust.scale = 1f;
                            dust.fadeIn = 0.5f;
                        }
                    }
                    for (int i = 0; i < 200; i++)
                    {
                        NPC target = Main.npc[i];
                        var player = Main.LocalPlayer;

                        if (Vector2.Distance(Player.Center, target.Center) <= 150 && !target.friendly && target.lifeMax > 5 && !target.dontTakeDamage && target.active && target.type != NPCID.TargetDummy && Collision.CanHit(Player.Center, 0, 0, target.Center, 0, 0))
                        {

                            target.AddBuff(BuffType<BlizzardDebuff>(), 2);
                        }

                    }
                }
            }

            //For mandible necklace=======================
            if (desertNecklace)
            {
                Player.buffImmune[BuffID.WindPushed] = true;
                Player.buffImmune[BuffID.Suffocation] = true;

                if (Player.ZoneDesert || (frozenNecklace && Player.ZoneSnow) || (woodNecklace && (Player.ZoneForest || Player.ZoneHallow) && Player.ZoneOverworldHeight))
                {
                    Player.AddBuff(ModContent.BuffType<WoodenDesertBuff>(), 2);
                }
            }

            //achievement
            if ((Player.HasBuff(ModContent.BuffType<WoodenBuff>()) && Player.HasBuff(ModContent.BuffType<WoodenBlizzardBuff>()) ||
                (Player.HasBuff(ModContent.BuffType<WoodenBuff>()) && Player.HasBuff(ModContent.BuffType<WoodenDesertBuff>())) ||
                (Player.HasBuff(ModContent.BuffType<WoodenBlizzardBuff>()) && Player.HasBuff(ModContent.BuffType<WoodenDesertBuff>()))))
            {
                //achievement get
                ModContent.GetInstance<AchievementPendants>().AchPendantsCondition.Complete();
            }
            if (!graniteBuff)//If the player removes the accessory the buff is gone
            {
                Player.ClearBuff(ModContent.BuffType<GraniteAccessBuff>());
            }
            //For the Celestial Barrier Projectile
            if (lunarBarrier)
            {
                //Item thepain = ModContent.ItemType<Celestialshield>();
                if (!celestialspin)
                {
                    Projectile.NewProjectile(Player.GetSource_Accessory(CelestialbarrierItem), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<CelestialShieldProj>(), 0, 0, Player.whoAmI);
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
                    Projectile.NewProjectile(Player.GetSource_Accessory(StormBossAccessItem), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<StormAccessProj>(), 75, 1, Player.whoAmI);
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
                if (!lunaticsentry)
                {
                    if (Player == Main.LocalPlayer)
                    {
                        Projectile.NewProjectile(Player.GetSource_Accessory(LunaticHoodItem), new Vector2(Player.Center.X - 80, Player.Center.Y - 40), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.SentryProjs.LunaticExpertSentryProj>(), 0, 0, Player.whoAmI);
                        Projectile.NewProjectile(Player.GetSource_Accessory(LunaticHoodItem), new Vector2(Player.Center.X + 80, Player.Center.Y - 40), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.SentryProjs.LunaticExpertSentryProj>(), 0, 0, Player.whoAmI);
                    }
                    lunaticsentry = true;
                }
            }
            if (!lunaticHood)
            {
                lunaticsentry = false;
            }
            if (mushroomSuper)
            {
                if (Player.statLife >= Player.statLifeMax2 * .75f)
                {
                    Player.ClearBuff(ModContent.BuffType<Buffs.MushBuff1>());
                    Player.ClearBuff(ModContent.BuffType<Buffs.MushBuff2>());
                    Player.ClearBuff(ModContent.BuffType<Buffs.MushBuff3>());

                    //Player.AddBuff(ModContent.BuffType<Buffs.MushBuff1>(), 2);
                }
                else if (Player.statLife >= Player.statLifeMax2 * .5f && Player.statLife < Player.statLifeMax2 * .75f)
                {
                    Player.ClearBuff(ModContent.BuffType<Buffs.MushBuff2>());
                    Player.ClearBuff(ModContent.BuffType<Buffs.MushBuff3>());
                    //Player.ClearBuff(ModContent.BuffType<Buffs.MushBuff4>());

                    Player.AddBuff(ModContent.BuffType<Buffs.MushBuff1>(), 2);
                }
                else if (Player.statLife >= Player.statLifeMax2 * .25f && Player.statLife < Player.statLifeMax2 * .5f)
                {
                    Player.ClearBuff(ModContent.BuffType<Buffs.MushBuff1>());
                    Player.ClearBuff(ModContent.BuffType<Buffs.MushBuff2>());
                    //Player.ClearBuff(ModContent.BuffType<Buffs.MushBuff4>());

                    Player.AddBuff(ModContent.BuffType<Buffs.MushBuff2>(), 2);
                }
                else
                {
                    Player.ClearBuff(ModContent.BuffType<Buffs.MushBuff1>());
                    Player.ClearBuff(ModContent.BuffType<Buffs.MushBuff2>());

                    Player.AddBuff(ModContent.BuffType<Buffs.MushBuff3>(), 2);
                }
            }
            //Ancient Emblem
            if (aridBossAccess)
            {
                ariddistance -= 1;
                if (ariddistance <= 0) //rest when reached edge
                    ariddistance = 60;
                for (int i = 0; i < 3; i++)
                {
                    ariddegrees += 4;
                    double rad = ariddegrees * (Math.PI / 180); //Convert degrees to radians
                    double dist = 60; //Distance away from the cursor

                    float dustpositionX = Main.MouseWorld.X - (int)(Math.Cos(rad) * dist);
                    float dustpositionY = Main.MouseWorld.Y - (int)(Math.Sin(rad) * dist);

                    float dustpositionX2 = Main.MouseWorld.X - (int)(Math.Cos(rad) * ariddistance);
                    float dustpositionY2 = Main.MouseWorld.Y - (int)(Math.Sin(rad) * ariddistance);

                    for (int j = 0; j < 5; j++)
                    {
                        if (Collision.CanHitLine(new Vector2(Player.Center.X, Player.Center.Y), 0, 0, Main.MouseWorld, 1, 1))//no dust unless line of sight
                        {
                            if (Collision.CanHitLine(new Vector2(dustpositionX, dustpositionY), 0, 0, Main.MouseWorld, 1, 1))
                            {
                                int dust = Dust.NewDust(new Vector2(dustpositionX, dustpositionY), 1, 1, 138, 0, 0, 138, default, 1f);

                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].velocity *= 0.2f;
                            }
                        }

                    }
                    for (int j = 0; j < 2; j++)
                    {
                        if (Collision.CanHitLine(new Vector2(Player.Center.X, Player.Center.Y), 0, 0, Main.MouseWorld, 1, 1))//no dust unless line of sight
                        {
                            if (Collision.CanHitLine(new Vector2(dustpositionX2, dustpositionY2), 0, 0, Main.MouseWorld, 1, 1))
                            {
                                int dust = Dust.NewDust(new Vector2(dustpositionX2, dustpositionY2), 1, 1, 138, 0, 0, 138, default, 1f);

                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].velocity *= 0.2f;
                            }
                        }

                    }
                    ariddegrees += 120; //for dust on other side
                }
                for (int i = 0; i < 5; i++)
                {
                    /*double deg = Main.rand.Next(0, 360); //The degrees
                    double rad = deg * (Math.PI / 180); //Convert degrees to radians
                    double dist = 60; //Distance away from the cursor
                    float dustx = Main.MouseWorld.X - 2 - (int)(Math.Cos(rad) * dist);
                    float dusty = Main.MouseWorld.Y - 2 - (int)(Math.Sin(rad) * dist);
                    if (Player == Main.LocalPlayer)
                    {
                        if (Collision.CanHitLine(new Vector2(Player.Center.X, Player.Center.Y), 0, 0, Main.MouseWorld, 1, 1))//no dust unless line of sight
                        {
                            if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, Main.MouseWorld, 1, 1))//no dust unless line of sight
                            {
                                var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 138, 0, 0);
                                dust.noGravity = true;
                                dust.velocity *= 0;
                                dust.scale = 1f;

                            }
                            Vector2 velocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X - 2, Main.MouseWorld.Y - 2) - new Vector2(dustx, dusty)) * 10;
                            if (Collision.CanHitLine(new Vector2(dustx, dusty), 0, 0, Main.MouseWorld, 1, 1))//no dust unless line of sight
                            {
                                var dust = Dust.NewDustDirect(new Vector2(dustx, dusty), 1, 1, 138, velocity.X, velocity.Y);
                                dust.noGravity = true;
                                dust.velocity *= 0.5f;
                                dust.scale = 0.75f;
                            }
                        }
                    }*/
                }
                for (int i = 0; i < 200; i++)
                {
                    NPC target = Main.npc[i];
                    if (Player == Main.LocalPlayer)
                    {
                        if (Collision.CanHitLine(new Vector2(Player.Center.X, Player.Center.Y), 0, 0, Main.MouseWorld, 1, 1))//no dust unless line of sight
                        {
                            if (Vector2.Distance(Main.MouseWorld, target.Center) <= 60 && !target.friendly && target.lifeMax > 5 && !target.dontTakeDamage && target.active && target.type != NPCID.TargetDummy && Collision.CanHit(Main.MouseWorld, 0, 0, target.Center, 0, 0))
                            {
                                if (!target.buffImmune[(BuffType<AridCoreDebuff>())])
                                {
                                    target.AddBuff(ModContent.BuffType<AridCoreDebuff>(), 90); //1.5 seconds
                                }
                            }
                        }
                    }
                }
            }
            if (Player.armor[0].type == ModContent.ItemType<Items.Vanitysets.BossMaskUltimateBoss>() || Player.armor[10].type == ModContent.ItemType<Items.Vanitysets.BossMaskUltimateBoss>())
            {
                if (ultimateBossMask == false)
                {
                    //Main.NewText("work!!!!!!!!!!!!!!!!!!!!!!!!!", 220, 63, 139);
                    Projectile.NewProjectile(Player.GetSource_Accessory(UltimateBossMaskItem), new Vector2(Player.Center.X - 30, Player.Center.Y - 5), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.PainbringerMaskProj>(), 0, 0, Player.whoAmI, 0, 0);

                    Projectile.NewProjectile(Player.GetSource_Accessory(UltimateBossMaskItem), new Vector2(Player.Center.X - 20, Player.Center.Y - 30), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.PainbringerMaskProj>(), 0, 0, Player.whoAmI, 0, 1);
                    Projectile.NewProjectile(Player.GetSource_Accessory(UltimateBossMaskItem), new Vector2(Player.Center.X + 20, Player.Center.Y - 30), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.PainbringerMaskProj>(), 0, 0, Player.whoAmI, 0, 2);
                    Projectile.NewProjectile(Player.GetSource_Accessory(UltimateBossMaskItem), new Vector2(Player.Center.X + 30, Player.Center.Y - 5), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.PainbringerMaskProj>(), 0, 0, Player.whoAmI, 0, 3);
                }
                ultimateBossMask = true;
            }
            else
                ultimateBossMask = false;

            //for Santank Core
            //if (SantaCore)
            {
                if (SantaRevived) //add buff
                {
                    Player.AddBuff(ModContent.BuffType<SantaReviveBuff>(), Player.immuneTime);

                    for (int i = 0; i < Player.buffType.Length; i++) //put again here because :ech:
                    {
                        if (Main.debuff[Player.buffType[i]] == true && BuffID.Sets.NurseCannotRemoveDebuff[Player.buffType[i]] == false)
                            Player.DelBuff(i);
                    }
                    Player.ClearBuff(BuffID.PotionSickness);
                    Player.potionDelay = 0;
                }

                if (SantaRevivedCooldown > 0)
                {
                    SantaRevivedCooldown--;
                    Player.AddBuff(ModContent.BuffType<SantaReviveDebuff>(), SantaRevivedCooldown);

                }

                if (SantaRevived && Player.immuneTime == 0)//once immune time has ran out, choose action
                {
                    if (Player.statLife < Player.statLifeMax2 / 3) //less than 33% health, die
                    {
                        Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " had a heart attack and died"), 99999, 0, false);
                    }
                    else //more, survive with debuff cooldown
                    {
                        for (int i = 0; i < 30; i++) //Grey dust circle7
                        {
                            Vector2 perturbedSpeed = new Vector2(0, -3f).RotatedByRandom(MathHelper.ToRadians(360));
                            var dust = Dust.NewDustDirect(Player.Center, 0, 0, 31, perturbedSpeed.X, perturbedSpeed.Y);
                            dust.noGravity = true;
                            dust.scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                            dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                        }
                        SantaRevivedCooldown = 18000; // 5 minutes (300 seconds)
                        Player.AddBuff(ModContent.BuffType<SantaReviveDebuff>(), 18000);
                        SantaRevived = false;
                    }
                }
            }
            if (SantaRevivedCooldown == 0)
            {
                Player.ClearBuff(ModContent.BuffType<SantaReviveDebuff>());
            }
            if (deathList)
            {
                if (ninelivescooldown > 0)
                {
                    ninelivescooldown--;
                }
                if (ninelivescooldown == 0)
                {
                    if (ninelives > 0) //once cooldown is met remove 1 soul
                    {
                        ninelives--;
                        ninelivescooldown = 90; //  1.5 seconds per soul
                    }
                }
                //Code for dropping in NPCEffects
                //Code for picking up in the item

                Player.GetDamage(DamageClass.Generic) += 0.02f * ninelives; //increase damage from 2-18%
                Player.GetCritChance(DamageClass.Generic) += 1 * ninelives; //increase crit from 1-9%

                if (Player.HeldItem.type == ModContent.ItemType<Items.Weapons.TheSickle>() || Player.HeldItem.type == ModContent.ItemType<Items.Weapons.TheScythe>())//extra for sickle/scythe
                {
                    Player.GetDamage(DamageClass.Melee) += 0.01f * ninelives; 
                    Player.GetCritChance(DamageClass.Melee) += 1 * ninelives;
                }
                //Main.NewText("" + ninelives + " " + ninelivescooldown, 204, 101, 22);

                if (ninelives == 9)
                {
                    //achievement get
                    ModContent.GetInstance<AchievementNineLives>().AchNineLivesCondition.Complete();
                }
            }
            else
            {
                ninelivescooldown = 0;
                ninelives = 0;
            }
        }
        //=====================For attacking an enemy with anything===========================================
        public override void OnHitAnything(float x, float y, Entity victim)
        {
            base.OnHitAnything(x, y, victim);
        }

        //=====================For taking damage from any source===========================================

        int attackdmg = 0;//This is for how much damage the player takes
        public override void OnHurt(Player.HurtInfo info)
        {
            Player.ClearBuff(ModContent.BuffType<HeartBarrierBuff>()); //Removes buff on hit

            attackdmg = info.Damage; //Int for the damage taken

            //triggers the granite accessory buff for 5 seconds, and it cannot be refreshed until the 10 second timer hjas ran out
            if (graniteBuff && !Player.HasBuff(ModContent.BuffType<GraniteAccessBuff>()) && granitebufftime == 600 && attackdmg > 1)
            {
                Player.AddBuff(ModContent.BuffType<GraniteAccessBuff>(), 180);
                SoundEngine.PlaySound(SoundID.NPCHit41 with { Volume = 1f, Pitch = -0.3f }, Player.Center);
                for (int i = 0; i < 25; i++)
                {
                    var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 187);
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                    dust.velocity *= 2;
                }
                granitebufftime = 0; //Activates the 10 second cooldown
            }

            //Grant buff for celestial barrier based on incoming damage======================
            if (lunarBarrier)
            {
                if (((attackdmg >= 100 && Main.masterMode) || (attackdmg >= 80 && Main.expertMode && !Main.masterMode) || (attackdmg >= 60 && !Main.expertMode)) && attackdmg < Player.statLife)
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
                if (frosttime >= 360 && attackdmg > 1)
                {

                    SoundEngine.PlaySound(SoundID.NPCDeath56 with { Volume = 0.2f, Pitch = -0.5f }, Player.Center);
                    float numberProjectiles = 10 + Main.rand.Next(4);
                    for (int i = 0; i < numberProjectiles; i++)
                    {


                        float speedX = 0f;
                        float speedY = -9f;
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(150));
                        float scale = 1f - (Main.rand.NextFloat() * .5f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(Player.GetSource_Accessory(FrostSpikeItem), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<FrostAccessProj>(), frostdamage, 3f, Player.whoAmI);

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
            //Vanity Mask sound
            if (!GetInstance<ConfigurationsIndividual>().NoPain)
            {
                if (Player.armor[0].type == ModContent.ItemType<Items.Vanitysets.ThePainMask>() || Player.armor[10].type == ModContent.ItemType<Items.Vanitysets.ThePainMask>())
                {
                    if (Player.Male)
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSound") with { Volume = 1.5f, MaxInstances = -1 }, Player.Center);
                    else
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ThePainSoundFemale") with { Volume = 1.5f, MaxInstances = -1 }, Player.Center);

                    //CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.DeepPink, "ThePain!", false);

                }
                if (Player.armor[0].type == ModContent.ItemType<Items.Vanitysets.TheClaymanMask>() || Player.armor[10].type == ModContent.ItemType<Items.Vanitysets.TheClaymanMask>())
                {
                    if (Player.Male)
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ClayManSound") with { Volume = 1.5f, MaxInstances = -1 }, Player.Center);
                    else
                        SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ClayManSoundFemale") with { Volume = 1.5f, MaxInstances = -1 }, Player.Center);

                    //CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.PeachPuff, "Clayman!", false);

                }
            }
            if (Player.armor[0].type == ModContent.ItemType<Items.Vanitysets.GnomedHat>() || Player.armor[10].type == ModContent.ItemType<Items.Vanitysets.GnomedHat>())
            {
                SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/Gnomed") with { Volume = 1.5f, MaxInstances = -1 }, Player.Center);
                //achievement get
                ModContent.GetInstance<AchievementGnomed>().AchGnomeCondition.Complete();
                //CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.Red, "Gnomed!", false);
            }
        }
        //Prevent Death
        String Suffertext;
        String Revivetext;

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (paintime == 0 && DeathCore && damage <= 9999) //Save you from death once
            {
                for (int j = 0; j < Player.MaxBuffs; j++)
                {
                    for (int i = 0; i < Player.buffType.Length; i++)
                    {
                        if (Main.debuff[Player.buffType[i]] == true && BuffID.Sets.NurseCannotRemoveDebuff[Player.buffType[i]] == false)
                            Player.DelBuff(i);
                    }
                    Player.ClearBuff(BuffID.PotionSickness);
                    Player.potionDelay = 0;
                }

                Suffertext = "Live to suffer another day!";
                CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 12, 4), Color.IndianRed, Suffertext, true);
                int proj = Projectile.NewProjectile(Player.GetSource_Accessory(DeathCoreItem), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.ExplosionPainNofaceProj>(), 0, 0, Main.myPlayer);
                Main.projectile[proj].scale = 2.5f;

                float numberProjectiles = 20;
                float rotation = MathHelper.ToRadians(180);
                //position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                for (int j = 0; j < numberProjectiles; j++)
                {
                    float speedX = 0f;
                    float speedY = 7f;
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, j / (numberProjectiles - 1)));
                    Projectile.NewProjectile(Player.GetSource_Accessory(DeathCoreItem), new Vector2(Player.Center.X, Player.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<PainStaffProj>(), 2000, 0, Main.myPlayer, 0, 0, 1);
                }
                SoundEngine.PlaySound(SoundID.Item109 with { Volume = 1f, Pitch = 0f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Player.Center);

                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Suffertext), Color.IndianRed);
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText(Suffertext, Color.IndianRed);
                }

                for (int i = 0; i < 100; i++)
                {
                    float speedY = -8f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(Player.Center, 0, 0, 115, dustspeed.X, dustspeed.Y, 50, default, 1.5f);
                    Main.dust[dust2].noGravity = true;
                }
                for (int i = 0; i < 6; i++)
                {
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.StellarTune, new ParticleOrchestraSettings
                    {
                        PositionInWorld = new Vector2(Player.Center.X + Main.rand.Next(-Player.width / 3, Player.width / 3), Player.Center.Y + Main.rand.Next(-Player.height / 3, Player.height / 3)),
                    }, Player.whoAmI);
                    
                }
                Player.HealEffect(Player.statLifeMax2, true);                               
                Player.statLife = Player.statLifeMax2; //restore life
                Player.immune = true;
                Player.immuneTime = 300;
                Player.ClearBuff(ModContent.BuffType<PainBuff>()); //Buff will go away on its own after 1 frame, allows it to satck with Pain boss
                Player.AddBuff(ModContent.BuffType<PainlessDebuff>(), 7200);

                paintime = 7200; // 2 minutes (120 seconds)
                return false;
            }
            
            if (SantaCore && !SantaRevived && damage < Player.statLifeMax2 && !Player.HasBuff(ModContent.BuffType<SantaReviveDebuff>()) && ((paintime > 0 && DeathCore) || !DeathCore)) //Save you from death once
            {
                for (int j = 0; j < Player.MaxBuffs; j++)
                {
                    for (int i = 0; i < Player.buffType.Length; i++)
                    {
                        if (Main.debuff[Player.buffType[i]] == true && BuffID.Sets.NurseCannotRemoveDebuff[Player.buffType[i]] == false)
                            Player.DelBuff(i);
                    }
                }
                SoundEngine.PlaySound(SoundID.Item93 with { Volume = 1f, Pitch = 0f, MaxInstances = -1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Player.Center);
                //Player.Hurt(PlayerDeathReason.ByCustomReason(Player.name + " got a shock"), 0, 0, false);

                for (int i = 0; i < 100; i++)
                {
                    float speedY = -4f;

                    Vector2 dustspeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                    int dust2 = Dust.NewDust(Player.Center, 0, 0, 133, dustspeed.X, dustspeed.Y, 100, default, 1f);
                }
                PlayerDeathReason.ByCustomReason(Player.name + " was shocked back to life");
                Revivetext = Player.name + " was shocked back to life";

                if (Main.netMode == 2) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Revivetext), new Color(189, 180, 21));
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText(Revivetext, 189, 180, 21);
                }

                //Player.HealEffect(1, true);

                //Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " died of a heart attack"), 99999, 0, false);
                Player.immune = true;
                Player.immuneTime = 600;
                Player.statLife = 1; //restore life
                //Player.statLife = 1; //restore life

                SantaRevived = true;
                return false;
            }

            if (Player.armor[0].type == ModContent.ItemType<Items.Vanitysets.UltimateFearMask>() || Player.armor[10].type == ModContent.ItemType<Items.Vanitysets.UltimateFearMask>())
            {
                playSound = false;

                SoundEngine.PlaySound(SoundID.ScaryScream with { Volume = 1.5f, MaxInstances = 1 }, Player.Center);
            }
            //playSound = false;
            
            return true;
        }
        //===================================Other hooks======================================
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
           
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //Insulated Cuffs
            if (blueCuffs)
            {
                if (Main.rand.Next(3) == 0)
                {
                    target.AddBuff(BuffID.Frostburn, 180);
                }
            }
            //Glacial Claws
            if (blueClaws)
            {
                if (Main.rand.Next(3) == 0)
                {
                    target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);
                }
            }
            //Nightmare Claws
            if (spookyClaws)
            {
                target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 450);
            }
            if (SantaCore)
            {
                target.AddBuff(BuffID.Electrified, 180);

            }
            //Storm Charm
            if (shockBand || shockBandQuiver || shockDerpEye)
            {
                if (hit.Crit & !target.friendly && target.lifeMax > 5 && shockcooldown == 0)
                {
                    target.GetGlobalNPC<NPCEffects>().shockbandtime = 10;// makes sure the enemy that summons the proj can't get hit by it
                    for (int i = 0; i < Main.maxNPCs; i++) //Shoots sand at one enemy
                    {
                        NPC npctarget = Main.npc[i];

                        if (Vector2.Distance(target.Center, npctarget.Center) <= 500f && !npctarget.friendly && npctarget.active && !npctarget.dontTakeDamage && npctarget.lifeMax > 5 && npctarget.type != NPCID.TargetDummy
                            && Collision.CanHit(target.Center, 0, 0, npctarget.Center, 0, 0) && npctarget.GetGlobalNPC<NPCEffects>().shockbandtime == 0 && !shockedtarget)
                        {
                            SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 0.5f, Pitch = -0.1f, MaxInstances = 0 }, target.Center);
                            Vector2 velocity = Vector2.Normalize(new Vector2(npctarget.Center.X, npctarget.Center.Y) - new Vector2(target.Center.X, target.Center.Y)) * 10;

                            float extradmg = 0;
                            if (shockBand && shockBandQuiver && shockDerpEye)
                                extradmg += 0.3f;
                            //2 of 3, equipped, 1 extra taget and -15% falloff
                            else if ((shockBand && shockBandQuiver) || (shockBand && shockDerpEye) || (shockDerpEye && shockBandQuiver))
                                extradmg += 0.15f;

                            int ProjID = Projectile.NewProjectile(Player.GetSource_Accessory(ShockbandItem), new Vector2(target.Center.X, target.Center.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.ShockBandProj>(), (int)(damageDone * 0.4f), 0, Main.myPlayer);
                            shockedtarget = true;
                            shockcooldown = 10;
                            for (int j = 0; j < 20; j++)
                            {
                                Vector2 dustspeed = new Vector2(0, 2).RotatedByRandom(MathHelper.ToRadians(360));
                                int dust2 = Dust.NewDust(target.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 100, default, 1f);
                                Main.dust[dust2].noGravity = true;
                            }
                            for (int j = 0; j < 20; j++)
                            {
                                Vector2 dustspeed = new Vector2(0, 2).RotatedByRandom(MathHelper.ToRadians(360));
                                int dust2 = Dust.NewDust(Player.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 100, default, 1f);
                                Main.dust[dust2].noGravity = true;
                            }
                        }
                    }
                }
                else
                    shockedtarget = false;
            }
            //for the Beetle Gauntlet
            if (beetleFist && beetlecooldown <= 0)
            {
                if (!Player.dead && hit.Crit)
                {
                    SoundEngine.PlaySound(SoundID.Zombie50 with { Volume = 2f, Pitch = -0.5f, MaxInstances= 0 }, target.Center);

                    float numberProjectiles = 2 + Main.rand.Next(2);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        float speedX = 0f;
                        float speedY = -12f;
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(135));
                        float scale = 1f - (Main.rand.NextFloat() * .5f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(Player.GetSource_Accessory(BeetleFistItem), new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BeetleGloveProj>(), 30, 1, Player.whoAmI);

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
                    beetlecooldown = 5;
                }
            }
            if (Player.HasBuff(ModContent.BuffType<WoodenDesertBuff>()))
            {
                float speedX = 0f;
                float speedY = -8f;
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(120));
                float scale = 1f - (Main.rand.NextFloat() * .5f);
                perturbedSpeed = perturbedSpeed * scale;
                if (Main.rand.Next(2) == 0)
                {
                    int ProjID = Projectile.NewProjectile(Player.GetSource_Accessory(DesertNecklaceItem), new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<DesertSparkProj>(), item.damage / 2, 1, Player.whoAmI);

                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 dustspeed = new Vector2(0, 0.75f).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(target.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 1f);
                        Main.dust[dust2].noGravity = true;
                    }
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            /*if (Player.armor[0].type == ModContent.ItemType<Items.Vanitysets.TheClaymanMask>() || Player.armor[10].type == ModContent.ItemType<Items.Vanitysets.TheClaymanMask>())
            {
                SoundEngine.PlaySound(new SoundStyle("StormDiversMod/Assets/Sounds/ClayManSound") with { Volume = 1.5f, MaxInstances = 5, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Player.Center);
            }*/
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
            //Insulated Cuffs
            if (blueCuffs)
            {
                if (Main.rand.Next(3) == 0)
                {
                    target.AddBuff(BuffID.Frostburn, 120);
                }
            }
            //Glacial Claws
            if (blueClaws && (proj.CountsAsClass(DamageClass.Melee) || ProjectileID.Sets.IsAWhip[proj.type] == true) && proj.owner == Main.myPlayer)
            {
                if (Main.rand.Next(3) == 0)
                {
                    target.AddBuff(ModContent.BuffType<SuperFrostBurn>(), 300);
                }
            }
            //Nightmare Claws
            if (spookyClaws && (proj.CountsAsClass(DamageClass.Melee) || ProjectileID.Sets.IsAWhip[proj.type] == true) && proj.owner == Main.myPlayer)
            {
                target.AddBuff(ModContent.BuffType<UltraBurnDebuff>(), 450);
            }
            if (SantaCore)
            {
                target.AddBuff(BuffID.Electrified, 180);

            }
            //Storm Charm
            if (shockBand || shockBandQuiver || shockDerpEye)
            {
                if (hit.Crit & !target.friendly && target.lifeMax > 5 && shockcooldown == 0)
                {
                    target.GetGlobalNPC<NPCEffects>().shockbandtime = 10;// makes sure the enemy that summons the proj can't get hit by it
                    for (int i = 0; i < Main.maxNPCs; i++) //Shoots sand at one enemy
                    {
                        NPC npctarget = Main.npc[i];

                        if (Vector2.Distance(target.Center, npctarget.Center) <= 500f && !npctarget.friendly && npctarget.active && !npctarget.dontTakeDamage && npctarget.lifeMax > 5 && npctarget.type != NPCID.TargetDummy
                            && Collision.CanHit(target.Center, 0, 0, npctarget.Center, 0, 0) && npctarget.GetGlobalNPC<NPCEffects>().shockbandtime == 0 && !shockedtarget)
                        {
                            SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 0.5f, Pitch = -0.1f, MaxInstances = 0 }, target.Center);
                            Vector2 velocity = Vector2.Normalize(new Vector2(npctarget.Center.X, npctarget.Center.Y) - new Vector2(target.Center.X, target.Center.Y)) * 10;

                            float extradmg = 0;
                            if (shockBand && shockBandQuiver && shockDerpEye)
                                extradmg += 0.3f;
                            //2 of 3, equipped, 1 extra taget and -15% falloff
                            else if ((shockBand && shockBandQuiver) || (shockBand && shockDerpEye) || (shockDerpEye && shockBandQuiver))
                                extradmg += 0.15f;

                            //Main.NewText("" + extradmg, Color.Teal);
                            int ProjID = Projectile.NewProjectile(Player.GetSource_Accessory(ShockbandItem), new Vector2(target.Center.X, target.Center.Y), new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<Projectiles.ShockBandProj>(), (int)(damageDone * (0.4f + extradmg)), 0, Main.myPlayer);
                            Main.projectile[ProjID].ai[2] = 0;
                            Main.projectile[ProjID].timeLeft = 120;
                            Main.projectile[ProjID].ArmorPenetration = 20;
                            Main.projectile[ProjID].extraUpdates = 10;
                            shockedtarget = true;
                            shockcooldown = 10;
                            for (int j = 0; j < 20; j++)
                            {
                                Vector2 dustspeed = new Vector2(0, 2).RotatedByRandom(MathHelper.ToRadians(360));
                                int dust2 = Dust.NewDust(target.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 100, default, 1f);
                                Main.dust[dust2].noGravity = true;
                            }
                            for (int j = 0; j < 20; j++)
                            {
                                Vector2 dustspeed = new Vector2(0, 2).RotatedByRandom(MathHelper.ToRadians(360));
                                int dust2 = Dust.NewDust(Player.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 100, default, 1f);
                                Main.dust[dust2].noGravity = true;
                            }
                        }
                    }
                }
                else
                    shockedtarget = false;
            }
            //for the Beetle Gauntlet
            if (beetleFist && beetlecooldown <= 0)
            {            
                if (!Player.dead && proj.CountsAsClass(DamageClass.Melee) && hit.Crit && proj.type != ModContent.ProjectileType<BeetleGloveProj>())
                {
                    SoundEngine.PlaySound(SoundID.Zombie50 with { Volume = 2f, Pitch = -0.5f, MaxInstances = 0 }, target.Center);

                    float numberProjectiles = 2 + Main.rand.Next(2);

                    for (int i = 0; i < numberProjectiles; i++)
                    {


                        float speedX = 0f;
                        float speedY = -12f;
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(135));
                        float scale = 1f - (Main.rand.NextFloat() * .5f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(Player.GetSource_Accessory(BeetleFistItem), new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<BeetleGloveProj>(), 30, 1, Player.whoAmI);

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
                    beetlecooldown = 5;
                }
                
            }
            if (Player.HasBuff(ModContent.BuffType<WoodenDesertBuff>()))
            {
                float speedX = 0f;
                float speedY = -8f;
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(120));
                float scale = 1f - (Main.rand.NextFloat() * .5f);
                perturbedSpeed = perturbedSpeed * scale;
                if (Main.rand.Next(2) == 0)
                {

                    int ProjID = Projectile.NewProjectile(Player.GetSource_Accessory(DesertNecklaceItem), new Vector2(target.Center.X, target.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ModContent.ProjectileType<DesertSparkProj>(), proj.damage / 2, 1, Player.whoAmI);

                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 dustspeed = new Vector2(0, 0.75f).RotatedByRandom(MathHelper.ToRadians(360));

                        int dust2 = Dust.NewDust(target.Center, 0, 0, 226, dustspeed.X, dustspeed.Y, 229, default, 1f);
                        Main.dust[dust2].noGravity = true;
                    }
                }
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            //For vanity sound, remove hurt sound here, play sound in hurt hook
            if (!GetInstance<ConfigurationsIndividual>().NoPain)
            {
                if (Player.armor[0].type == ModContent.ItemType<Items.Vanitysets.ThePainMask>() || Player.armor[10].type == ModContent.ItemType<Items.Vanitysets.ThePainMask>() ||
                Player.armor[0].type == ModContent.ItemType<Items.Vanitysets.TheClaymanMask>() || Player.armor[10].type == ModContent.ItemType<Items.Vanitysets.TheClaymanMask>() ||
                Player.armor[0].type == ModContent.ItemType<Items.Vanitysets.GnomedHat>() || Player.armor[10].type == ModContent.ItemType<Items.Vanitysets.GnomedHat>())
                {
                    modifiers.DisableSound();
                }
                
            }

            if (Player.HasBuff(ModContent.BuffType<WoodenBuff>()))
            {
                modifiers.FinalDamage.Flat -= 4;
            }
            //for Enchanted Mushroom
            if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff1>()))
            {
                modifiers.FinalDamage.Flat -= 1;
            }
            else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff2>()))
            {
                modifiers.FinalDamage.Flat -= 3;

            }
            else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff3>()))
            {
                modifiers.FinalDamage.Flat -= 5;

            }
            /*else if (Player.HasBuff(ModContent.BuffType<Buffs.MushBuff4>()))
            {
                modifiers.FinalDamage.Flat -= 6;

            }*/
            /*if (damage >= Player.statLife && Player.statLife > 1)
            {
                damage = Player.statLife - 1;
                SoundEngine.PlaySound(SoundID.Item, (int)Player.position.X, (int)Player.position.Y, 122);

            }*/         

        }
        
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
           /* if (proj.type == ModContent.ProjectileType<Projectiles.StickyBombProj>())
            {
                proj.damage = 10;
               
            }*/
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
        int xline = 0;
        int yline = 0;
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (!Main.dedServ)
            {
                if (shroomaccess)
                {
                    if (Player.HeldItem.CountsAsClass(DamageClass.Ranged) && Player.HeldItem.useAmmo == AmmoID.Bullet && (Player.controlUseTile) && Player.noThrow == 0)
                    {
                        if (Player.HeldItem.type == ModContent.ItemType<TommyGun>())
                        {
                            if (Player.controlUp && !Player.controlDown) //up 
                            {
                                xline = 1000 * Player.direction;
                                yline = -1000;
                            }

                            else if (Player.controlDown && !Player.controlUp )//down 
                            {
                                xline = 1000 * Player.direction;
                                yline = 1000;
                            }
                            
                            else //straight
                            {
                                xline = 1500 * Player.direction;
                                yline = 0;
                            }
                            Utils.DrawLine(Main.spriteBatch, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(Player.Center.X + xline, Player.Center.Y + yline), Color.DeepSkyBlue, Color.Transparent, 2f);

                        }
                        else
                        {
                            Vector2 velocity = Vector2.Normalize(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - new Vector2(Player.Center.X, Player.Center.Y)) * 2000;
                            Utils.DrawLine(Main.spriteBatch, new Vector2(Player.Center.X, Player.Center.Y), new Vector2(Player.Center.X + velocity.X, Player.Center.Y + velocity.Y), Color.DeepSkyBlue, Color.Transparent, 2f);
                        }
                    }
                }
            }
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }
    }

}