using Microsoft.Xna.Framework;

using Terraria;

using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using StormDiversMod.Buffs;
using StormDiversMod.Basefiles;
using StormDiversMod.Items.Weapons;

using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using StormDiversMod.Items.Ammo;
using StormDiversMod.Items.Accessory;
using StormDiversMod.Items.Tools;
using StormDiversMod.Items.Vanitysets;
using Terraria.Audio;

namespace StormDiversMod.Basefiles
{
    public class NPCEffects : GlobalNPC
    {

        public override bool InstancePerEntity => true;

        // npc.GetGlobalNPC<NPCEffects>().boulderDB = true; in debuff.cs
        // target.AddBuff(mod.BuffType("BoulderDebuff"), 1200)

        public bool lunarBoulderDB; //Lunar Bouldered

        public bool sandBurn; //Forbidden Burn 

        public bool beetled; //beetle Swarm

        public bool heartDebuff; //Stolen Heart
         
        public bool superFrost; //Cryoburn

        public bool superburnDebuff; //Blazing fire

        public bool hellSoulFire; //Soulburn
         
        public bool darknessDebuff; //Escence of darkness

        public bool ultraburnDebuff; //Ultra Burn

        public bool ultrafrostDebuff; //Ultra freeze

        public bool spookedDebuff; //Spooked

        //All this for a speen----------------------------------------------

        public bool derplaunched; //If the npc has been launched by the Derpling armour

        public float direction; //records the direction prior to speen

        public int spintime; //how long until rotation can be reset

        //For projectile immunity immune

        public int aridimmunetime; //prevent arid armour explosion from hitting triggered enemy

        public int forbiddenimmunetime; //Prevent forbidden sand from bitten targetted enemy


        //For Heart Emblem

        public bool heartStolen; //If the npc has dropped below 50% life

        //Whip tags
        public bool WhiptagBlood; //Bloody Whip
        int bloodwhipcooldown;

        public bool WhiptagForbidden; //Forbidden Whip
        int forbiddenwhipcooldown;

        public bool WhiptagSpaceRock; //Asteroid Whip
        int spacerockwhipcooldown;

        //------------------------------------------------------------------
        public override void ResetEffects(NPC npc)
        {
            lunarBoulderDB = false;
            sandBurn = false;
            beetled = false;
            heartDebuff = false;
            superFrost = false;
            superburnDebuff = false;
            hellSoulFire = false;
            derplaunched = false;
            darknessDebuff = false;
            ultraburnDebuff = false;
            ultrafrostDebuff = false;
            spookedDebuff = false;
            WhiptagBlood = false;
            WhiptagForbidden = false;
            WhiptagSpaceRock = false;
        }

        public override void SetStaticDefaults()
        {

        }
        public override void AI(NPC npc)
        {
            //Debuff immunities
            if (npc.boss)
            {
                npc.buffImmune[(BuffType<BeetleDebuff>())] = true;
            }
            if (npc.buffImmune[BuffID.Frostburn] == true) //all enemies immune to frost burn are immune to the Cryoburn and Ultra Freeze
            {
                npc.buffImmune[BuffType<SuperFrostBurn>()] = true; //Cryoburn
                npc.buffImmune[BuffType<UltraFrostDebuff>()] = true; //Ultra Freeze
            }
            if (npc.buffImmune[BuffID.OnFire] == true) //all enemies immune to on fire are immune to the fire debuffs
            {
                npc.buffImmune[BuffType<SuperBurnDebuff>()] = true; //Blazing Fire
                npc.buffImmune[BuffType<HellSoulFireDebuff>()] = true; //Soul Fire
                npc.buffImmune[BuffType<UltraBurnDebuff>()] = true; //Ultra Burn
            }
            //All underground desert and sandstorm enemies are immune to Forbidden burn
            if (npc.type == NPCID.SandSlime || npc.type == NPCID.Antlion || npc.type == NPCID.WalkingAntlion || npc.type == NPCID.GiantWalkingAntlion || npc.type == NPCID.FlyingAntlion || npc.type == NPCID.GiantFlyingAntlion || 
                npc.type == NPCID.LarvaeAntlion || npc.type == NPCID.DesertBeast || npc.type == NPCID.DesertScorpionWalk || npc.type == NPCID.DesertScorpionWall || npc.type == NPCID.DesertLamiaDark || npc.type == NPCID.DesertLamiaLight || 
                npc.type == NPCID.DesertDjinn ||  npc.type == NPCID.DesertGhoul || npc.type == NPCID.DesertGhoulCorruption || npc.type == NPCID.DesertGhoulCrimson || npc.type == NPCID.DesertGhoulHallow ||
                npc.type == NPCID.TombCrawlerHead || npc.type == NPCID.TombCrawlerBody || npc.type == NPCID.TombCrawlerTail || npc.type == NPCID.DuneSplicerHead || npc.type == NPCID.DuneSplicerBody || npc.type == NPCID.DuneSplicerTail ||
                npc.type == NPCID.SandElemental || npc.type == NPCID.SandShark || npc.type == NPCID.SandsharkCorrupt || npc.type == NPCID.SandsharkCrimson || npc.type == NPCID.SandsharkHallow || npc.type == NPCID.Tumbleweed
                )
            {
                npc.buffImmune[BuffType<AridSandDebuff>()] = true;
            }
            
            //slowdown enemies
            if (beetled && !npc.boss)
            {
                npc.velocity.X *= 0.92f;
                npc.velocity.Y *= 0.92f;

            }
            if (spookedDebuff && !npc.boss)
            {
                npc.velocity.X *= 0.96f;

            }
            if (ultrafrostDebuff && !npc.boss)
            {
                npc.velocity.X *= 0.93f;

            }
            //summon projectiles for shield killer
            var player = Main.LocalPlayer;         

            if (player.HeldItem.type == ModContent.ItemType<Items.Tools.ShieldKiller>() && player.itemAnimation == player.itemAnimationMax - 1)
            {

                if (npc.type == NPCID.LunarTowerVortex)
                {
                    Projectile.NewProjectile(null, new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(422));
                }

                if (npc.type == NPCID.LunarTowerSolar)
                {
                    Projectile.NewProjectile(null, new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(517));
                }
                if (npc.type == NPCID.LunarTowerNebula)
                {
                    Projectile.NewProjectile(null, new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(507));
                }
                if (npc.type == NPCID.LunarTowerStardust)
                {
                    Projectile.NewProjectile(null, new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0, 0), ProjectileID.TowerDamageBolt, 0, 0, Main.myPlayer, NPC.FindFirstNPC(493));
                }

            }
        
            //speen________________________________________________
            {
                if (derplaunched)
                {
                    spintime++;

                    if (spintime == 0)
                    {
                        if (npc.velocity.X > 0)
                        {
                            direction = 1;
                        }
                        else
                        {
                            direction = -1;
                        }

                    }
                    if (spintime > 0 && spintime < 45) //begins the rotation 
                    {
                        npc.rotation += (0.14f * -direction); //Speen speed and direction
                    }
                    if (npc.velocity.Y == 0)
                    {
                        spintime = 44;
                        npc.rotation = 0; //reset the rotation to 0 for 1 frame

                    }

                    if (spintime >= 44)
                    {
                        npc.rotation = 0; //reset the rotation to 0 for 1 frame
                        spintime = 0; //Allows the rotation to be recorded again

                    }
                }

            }
            if (!npc.friendly)
            {
                //------------Projectile immune
                if (aridimmunetime > 0)
                {
                    //Main.NewText("PLEASE WORK::::::" + aridimmunetime, 204, 101, 22);
                    aridimmunetime--;
                }
                if (forbiddenimmunetime > 0)
                {
                    forbiddenimmunetime--;
                }
            }
            //______________

            if (player.GetModPlayer<EquipmentEffects>().heartSteal) //For the Jar of hearts
            {
                if (!npc.SpawnedFromStatue && npc.life <= (npc.lifeMax * 0.50f) && !npc.friendly && npc.lifeMax > 5 && !npc.buffImmune[(BuffType<HeartDebuff>())]) //Rolls to see the outcome when firts hit under 50% life
                {

                    if (!npc.GetGlobalNPC<NPCEffects>().heartStolen)//Makes sure this only happens once
                    {
                        if (!npc.boss) //non bosses
                        {
                            if (Main.rand.Next(4) == 0) //1 in 4 chance to have the debuff applied and drop a heart
                            {
                                if (Main.netMode == 0)
                                {
                                    Item.NewItem(new EntitySource_Loot(npc), new Vector2(npc.Center.X, npc.Center.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Tools.SuperHeartPickup>());
                                }
                                else
                                {
                                    Main.LocalPlayer.statLife += 20;
                                    Main.LocalPlayer.HealEffect(20, true);
                                }
                                //Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("TESTER."), new Color(224, 141, 255));

                                SoundEngine.PlaySound(SoundID.NPCDeath7, npc.Center);
                                for (int i = 0; i < 15; i++)
                                {
                                    var dust = Dust.NewDustDirect(new Vector2(npc.Center.X, npc.Center.Y), 5, 5, 72);
                                    //dust.noGravity = true;
                                }
                                npc.AddBuff(ModContent.BuffType<HeartDebuff>(), 3600);
                                npc.GetGlobalNPC<NPCEffects>().heartStolen = true; //prevents more hearts from being dropped

                            }
                            else //Otherwise it just prevents the roll from happening again
                            {
                                npc.GetGlobalNPC<NPCEffects>().heartStolen = true;
                            }
                        }
                        else //for bosses
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (Main.netMode == 0)
                                {
                                    Item.NewItem(new EntitySource_Loot(npc), new Vector2(npc.Center.X, npc.Center.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Tools.SuperHeartPickup>());
                                }
                                else
                                {
                                    Main.LocalPlayer.statLife += 20;
                                    Main.LocalPlayer.HealEffect(20, true);
                                }
                            }
                            SoundEngine.PlaySound(SoundID.NPCDeath7, npc.Center);
                            for (int i = 0; i < 15; i++)
                            {
                                var dust = Dust.NewDustDirect(new Vector2(npc.Center.X, npc.Center.Y), 5, 5, 72);
                                //dust.noGravity = true;
                            }
                            npc.AddBuff(ModContent.BuffType<HeartDebuff>(), 3600);
                            npc.GetGlobalNPC<NPCEffects>().heartStolen = true;

                        }
                    }
                }
            }
            /*float distanceX = player.Center.X - npc.Center.X;
            float distanceY = player.Center.Y - npc.Center.Y;
            float distance = (float)System.Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
            bool lineOfSight = Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height);
            if (Main.LocalPlayer.HasBuff(BuffType<BloodBuff>()) && !npc.friendly && npc.lifeMax > 5) //If the player has taken a blood potion and the NPC is within a certian radius of the player
            {

                if (distance < 140 && lineOfSight)
                {

                    npc.AddBuff(mod.BuffType("BloodDebuff"), 2);
                }
            }*/

            //COVER YOURSELF IN OIL
            /*if (npc.HasBuff(BuffID.Oiled) && Main.raining && !npc.boss)
            {
                npc.velocity.Y = -10;
            }*/
            if (bloodwhipcooldown < 60)
            {
                bloodwhipcooldown++;
            }
            if (forbiddenwhipcooldown < 60)
            {
                forbiddenwhipcooldown++;
            }
            if (spacerockwhipcooldown < 60)
            {
                spacerockwhipcooldown++;
            }
        }
        public override void SetDefaults(NPC npc)
        {

          

        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
           
            if (heartDebuff)
            {
                npc.lifeRegen -= 50;

                damage = 5;

            }
            if (superburnDebuff)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 80;
                    damage = 10;
                }
                else
                {
                    npc.lifeRegen -= 30;
                    damage = 5;
                }
            }
            if (sandBurn)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 100;
                    damage = 12;
                }
                else
                {
                    npc.lifeRegen -= 50;
                    damage = 5;
                }
            }
            if (superFrost)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 100;
                    damage = 12;
                }
                else
                {
                    npc.lifeRegen -= 50;
                    damage = 5;
                }
            }
            if (darknessDebuff)
            {
                npc.lifeRegen -= 60;

                damage = 6;

            }
            if (hellSoulFire)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 130;
                    damage = 15;
                }
                else
                {
                    npc.lifeRegen -= 80;
                    damage = 10;
                }
            }
       
            if (ultraburnDebuff)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 150;
                    damage = 15;
                }
                else
                {
                    npc.lifeRegen -= 100;
                    damage = 10;
                }
            }
            if (ultrafrostDebuff)
            {
                if (npc.HasBuff(BuffID.Oiled))
                {
                    npc.lifeRegen -= 150;
                    damage = 15;
                }
                else
                {
                    npc.lifeRegen -= 100;
                    damage = 10;
                }
            }
            if (spookedDebuff)
            {
                npc.lifeRegen -= 500;
                damage = 250;

            }
            if (lunarBoulderDB)
            {
                npc.lifeRegen -= 300;

                damage = 30;

            }
        }
        int particle = 0;
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
          
           
            if (lunarBoulderDB)
            {
                int choice = Main.rand.Next(4);
                if (choice == 0)
                {
                    particle = 244;
                }
                else if (choice == 1)
                {
                    particle = 110;
                }
                else if (choice == 2)
                {
                    particle = 111; ;
                }
                else if (choice == 3)
                {
                    particle = 112;
                }
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, particle, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                if (!Main.dedServ)
                {
                    Lighting.AddLight(npc.position, 1f, 0.5f, 0f);
                }
            }           
            if (sandBurn)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 10, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 1.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                if (!Main.dedServ)
                {
                    Lighting.AddLight(npc.position, 0.1f, 0.2f, 0.7f);
                }
            }
          
            if (beetled)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 186, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

            }
            if (heartDebuff)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 72, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

            }
            if (superFrost)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 135, npc.velocity.X * 1.2f, npc.velocity.Y * 1.2f, 130, default, 2f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity

                    int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 135, npc.velocity.X, npc.velocity.Y, 130, default, 1f);
                    Main.dust[dust].velocity *= 0.5f;
                }

            }           
            if (superburnDebuff)
            {

                int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, npc.velocity.X * 1.2f, npc.velocity.Y * 1.2f, 0, default, 2f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity

                int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, npc.velocity.X, npc.velocity.Y, 0, default, 1f);
                Main.dust[dust].velocity *= 0.5f;


            }
            if (hellSoulFire)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173, npc.velocity.X * 1.2f, npc.velocity.Y * 1.2f, 0, default, 2f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity


                }

            }
            if (derplaunched)
            {

                var dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.Center.Y + npc.height / 2), npc.width, 0, 68, 0, 2, 130, default, 1f);
                dust.noGravity = true;



            }
            if (darknessDebuff)
            {

                int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 54, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, default, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1f;
                Main.dust[dust].velocity.Y -= 0.5f;
                if (Main.rand.NextBool(4))
                {
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].scale *= 0.5f;
                }

            }
            if (ultraburnDebuff)
            {

                int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, npc.velocity.X * 1.2f, npc.velocity.Y * 1.2f, 0, default, 2.5f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity

                int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, npc.velocity.X, -1, 0, default, 1.5f);
                Main.dust[dust2].noGravity = true; //this make so the dust has no gravity


            }
            if (ultrafrostDebuff)
            {

                int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 135, npc.velocity.X * 1.2f, npc.velocity.Y * 1.2f, 0, default, 2.5f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity

                int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 135, npc.velocity.X, -1, 0, default, 1.5f);
                Main.dust[dust2].noGravity = true; //this make so the dust has no gravity


            }
            if (spookedDebuff)
            {
                if (Main.rand.Next(5) == 0)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 200, npc.velocity.X * 1.2f, -3, 0, default, 1f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                    Main.dust[dust].fadeIn = 0.5f; //this make so the dust has no gravity

                }
                drawColor = new Color(255, 68, 0);

            }
            if (WhiptagBlood)
            {
                if (Main.rand.Next(4) < 1)
                {
                    int dust = Dust.NewDust(npc.Center - new Vector2(10f, 10f), 20, 20, 115, 0, 4, 50, default, 1.2f);
                    Main.dust[dust].noGravity = true;

                }

            }
            if (WhiptagForbidden)
            {
                if (Main.rand.Next(4) < 2)
                {
                    int dust = Dust.NewDust(npc.Center - new Vector2(5f, 5f), 10, 10, 10, 0, 0, 100, default, 1.25f);
                    Main.dust[dust].noGravity = true;

                }

            }
            if (WhiptagSpaceRock)
            {
                if (Main.rand.Next(4) < 1)
                {
                    int dust = Dust.NewDust(npc.Center - new Vector2(10f, 10f), 20, 20, 0, 0, -4, 100, default, 1f);
                    Main.dust[dust].noGravity = true;

                }

            }

        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            var player = Main.player[projectile.owner];

            if (player.GetModPlayer<ArmourSetBonuses>().shadowflameSet == true)
            {
                if (npc.HasBuff(BuffID.ShadowFlame))
                {
                    damage += 5;
                }
            }

            if (projectile.type == ModContent.ProjectileType<Projectiles.AncientArmourProj>()) //No crit from arid explosion
            {
                crit = false;
            }
            if (projectile.type == ModContent.ProjectileType<Projectiles.HellSoulArmourProj>()) //No crit from hellsoul explosion
            {
                if (npc.aiStyle == 6) //Worms take reduced damage
                {
                    damage = (int)(damage * 0.5f);
                }
                crit = false;
            }
            if (npc.type == NPCType<NPCs.DerpMimic>()) //Takes 666 less damage
            {
                damage -= 666;

            }
            if (npc.type == NPCType<NPCs.Boss.StormBoss>()) //75% damage from homing projectiles
            {
                if (ProjectileID.Sets.CultistIsResistantTo[projectile.type])
                {
                    damage = (damage * 3) / 4;
                }
            }

            if (projectile.GetGlobalProjectile<Projectiles.SelenianReflect>().reflected) //update damage to be the same as if it hit the player
            {
                if (Main.masterMode)
                {
                    damage *= 6;
                }
                else if (Main.expertMode && !Main.masterMode)
                {
                    damage *= 4;
                }
                else
                {
                    damage *= 2;

                }
            }
            //Whips
            if (!projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type] == true || projectile.sentry))
            {
                //Blood Whip
                if (WhiptagBlood)
                {
                    //damage += 5; //tag damage
                    if (Main.rand.Next(100) <= 4 && !crit) //tag crit
                    {
                        crit = true;
                    }
                    if (player.HasMinionAttackTargetNPC && npc == Main.npc[player.MinionAttackTargetNPC]) //summon projectile
                    {
                        if (bloodwhipcooldown >= 60)
                        {

                            Projectile.NewProjectile(null, new Vector2(npc.Center.X, npc.Center.Y - (40 - (npc.width / 2))), new Vector2(0, 0), ModContent.ProjectileType<Projectiles.WhipProjs.BloodWhipProj2>(), 15, 0, Main.myPlayer, 0 , Main.rand.Next (0, 359));
                            bloodwhipcooldown = 0;
                        }
                       
                    }
                }
                //Forbidden whip
                if (WhiptagForbidden)
                {
                    //damage += 4; //tag damage
                    if (Main.rand.Next(100) <= 6 && !crit) //tag crit
                    {
                        crit = true;
                    }

                    if (player.HasMinionAttackTargetNPC && npc == Main.npc[player.MinionAttackTargetNPC])
                    {
                        npc.GetGlobalNPC<NPCEffects>().forbiddenimmunetime = 20;// makes sure the enemy that summons the sand can't get hit by it

                        for (int i = 0; i < Main.maxNPCs; i++) //Shoots sand at one enemy
                        {
                            NPC target = Main.npc[i];

                            target.TargetClosest(true);

                            //Getting the shooting trajectory
                            float shootToX = target.position.X + (float)target.width * 0.5f - npc.Center.X;
                            float shootToY = target.position.Y + (float)target.height * 0.5f - npc.Center.Y;
                            float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                            if (distance < 400f && distance > 15f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy && Collision.CanHit(npc.Center, 0, 0, target.Center, 0, 0))
                            {
                                if (forbiddenwhipcooldown >= 10)
                                {
                                    //Dividing the factor of 2f which is the desired velocity by distance
                                    distance = 1.6f / distance;

                                    //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                                    shootToX *= distance * 2f;
                                    shootToY *= distance * 2f;

                                    int ProjID = Projectile.NewProjectile(null, new Vector2(npc.Center.X, npc.Center.Y), new Vector2(shootToX, shootToY), ModContent.ProjectileType<Projectiles.WhipProjs.DesertWhipProj2>(), 10, 0, Main.myPlayer);

                                    forbiddenwhipcooldown = 0;
                                }
                            }
                        }
                    }
                }
                if (WhiptagSpaceRock)
                {
                    damage += 5; //tag damage
                    if (Main.rand.Next(100) <= 15 && !crit) //tag crit
                    {
                        crit = true;
                    }

                    if (player.HasMinionAttackTargetNPC && npc == Main.npc[player.MinionAttackTargetNPC]) //summon projectile
                    {
                        if (spacerockwhipcooldown >= 25)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Projectile.NewProjectile(null, new Vector2(npc.Center.X + 200, npc.Center.Y - 300), new Vector2(-15, 23f), ModContent.ProjectileType<Projectiles.WhipProjs.SpaceRockWhipProj2>(), 60, 0, Main.myPlayer);
                            }
                            else
                            {
                                Projectile.NewProjectile(null, new Vector2(npc.Center.X - 200, npc.Center.Y - 300), new Vector2(15, 23f), ModContent.ProjectileType<Projectiles.WhipProjs.SpaceRockWhipProj2>(), 60, 0, Main.myPlayer);
                            }
                            spacerockwhipcooldown = 0;
                        }

                    }
                }
            }

            if (crit) //crit damage increases
            {
                if (player.GetModPlayer<EquipmentEffects>().aridCritChest == true)
                {
                    damage = (int)(damage * 1.1f);
                }
                if (player.GetModPlayer<EquipmentEffects>().derpEye == true)
                {
                    damage = (int)(damage * 1.15f);
                }
                if (player.GetModPlayer<EquipmentEffects>().derpEyeGolem == true)
                {
                    damage = (int)(damage * 1.15f);
                }
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (player.GetModPlayer<ArmourSetBonuses>().shadowflameSet == true)
            {
                if (npc.HasBuff(BuffID.ShadowFlame))
                {
                    damage += 5;
                }
            }
            if (npc.type == NPCType<NPCs.DerpMimic>()) //Takes 666 less damage
            {
                 damage -= 666;

            }
            if (crit)
            {
                if (player.GetModPlayer<EquipmentEffects>().aridCritChest == true)
                {
                    damage = (int)(damage * 1.1f);
                }
                if (player.GetModPlayer<EquipmentEffects>().derpEye == true)
                {
                    damage = (int)(damage * 1.2f);
                }

            }

        }
        
        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {

        }
        //Ditto, but from player projectiles
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            var player = Main.player[projectile.owner];
          
        }
        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {

        }

    }

}

        
