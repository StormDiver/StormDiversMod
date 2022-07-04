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
using StormDiversMod.Dusts;
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

        public bool lunarBoulderDB;

        public bool superBoulderDB;
        // npc.GetGlobalNPC<NPCEffects>().boulderDB = true; in debuff.cs
        // target.AddBuff(mod.BuffType("BoulderDebuff"), 1200)
        public bool boulderDB;

        public bool sandBurn;


        public bool beetled;

        public bool nebula;


        public bool spectreDebuff;

        public bool heartDebuff;

        public bool superFrost;

        public bool bloodDebuff;

        public bool superburnDebuff;

        public bool heartStolen; //If the npc has been hit when below 50% life

        public bool hellSoulFire;

        public bool darknessDebuff;

        public bool ultraburnDebuff;

        public bool ultrafrostDebuff;

        public bool spookedDebuff;

        //All this for a speen----------------------------------------------

        public bool derplaunched; //If the npc has been launched by the Derpling armour

        public float direction; //records the direction prior to speen

        public int spintime; //how long until rotation can be reset

        //For the arid immune

        public int aridimmunetime;

        //------------------------------------------------------------------
        public override void ResetEffects(NPC npc)
        {
            boulderDB = false;
            lunarBoulderDB = false;
            superBoulderDB = false;
            sandBurn = false;
            beetled = false;
            nebula = false;
            spectreDebuff = false;
            heartDebuff = false;
            superFrost = false;
            bloodDebuff = false;
            superburnDebuff = false;
            hellSoulFire = false;
            derplaunched = false;
            darknessDebuff = false;
            ultraburnDebuff = false;
            ultrafrostDebuff = false;
            spookedDebuff = false;
        }

        public override void SetStaticDefaults()
        {

        }

        int shieldtime = 5;


        public override void AI(NPC npc)
        {
            var player = Main.LocalPlayer;

            if (NPC.ShieldStrengthTowerVortex == 0 && NPC.ShieldStrengthTowerSolar == 0 && NPC.ShieldStrengthTowerNebula == 0 && NPC.ShieldStrengthTowerStardust == 0 && shieldtime > 0)
            {
                shieldtime--;
            }
            else if (NPC.ShieldStrengthTowerVortex > 0 || NPC.ShieldStrengthTowerSolar > 0 || NPC.ShieldStrengthTowerNebula > 0 || NPC.ShieldStrengthTowerStardust > 0)
            {
                shieldtime = 5;
            }

            if (player.HeldItem.type == ModContent.ItemType<Items.Tools.ShieldKiller>() && player.itemAnimation == player.itemAnimationMax - 1)
            {
                if (shieldtime > 0)
                {
                    if (npc.type == NPCID.LunarTowerVortex)
                    {

                        for (int i = 0; i < 150; i++)
                        {
                            float speedY = -20f;

                            Vector2 perturbedSpeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(npc.Center, 0, 0, 110, perturbedSpeed.X, perturbedSpeed.Y, 150, default, 2.5f);
                            Main.dust[dust2].noGravity = true;
                        }
                    }

                    if (npc.type == NPCID.LunarTowerSolar)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            float speedY = -20f;

                            Vector2 perturbedSpeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(npc.Center, 0, 0, 174, perturbedSpeed.X, perturbedSpeed.Y, 150, default, 2.5f);
                            Main.dust[dust2].noGravity = true;
                        }
                    }
                    if (npc.type == NPCID.LunarTowerNebula)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            float speedY = -20f;

                            Vector2 perturbedSpeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(npc.Center, 0, 0, 112, perturbedSpeed.X, perturbedSpeed.Y, 150, default, 2.5f);
                            Main.dust[dust2].noGravity = true;
                        }
                    }
                    if (npc.type == NPCID.LunarTowerStardust)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            float speedY = -20f;

                            Vector2 perturbedSpeed = new Vector2(0, speedY).RotatedByRandom(MathHelper.ToRadians(360));

                            int dust2 = Dust.NewDust(npc.Center, 0, 0, 111, perturbedSpeed.X, perturbedSpeed.Y, 150, default, 2.5f);
                            Main.dust[dust2].noGravity = true;
                        }
                    }
                }
            }

            if (beetled && !npc.boss)
            {
                npc.velocity.X *= 0.92f;
                npc.velocity.Y *= 0.92f;

            }
            if (spookedDebuff && !npc.boss)
            {
                npc.velocity.X *= 0.94f;

            }
            //speen________________________________________________
            {
                if (derplaunched)
                {
                    spintime++;
                }
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
                if (spintime == 45)
                {
                    npc.rotation = 0; //reset the rotation to 0 for 1 frame
                    spintime = 0; //Allows the rotation to be recorded again

                }

            }
            //------------Arid immune
            if (aridimmunetime > 0)
            {
                //Main.NewText("PLEASE WORK::::::" + aridimmunetime, 204, 101, 22);
                aridimmunetime--;
            }

            //______________

            if (player.GetModPlayer<EquipmentEffects>().heartSteal) //For the Jar of hearts
            {
                if (!npc.SpawnedFromStatue && npc.life <= (npc.lifeMax * 0.50f) && !npc.boss && !npc.friendly && npc.lifeMax > 5 && !npc.buffImmune[(BuffType<HeartDebuff>())]) //Rolls to see the outcome when firts hit under 50% life
                {

                    if (!npc.GetGlobalNPC<NPCEffects>().heartStolen)//Makes sure this only happens once
                    {
                        if (Main.rand.Next(4) == 0) //1 in 4 chance to have the debuff applied and drop a heart
                        {
                            Item.NewItem(new EntitySource_Loot(npc), new Vector2(npc.Center.X, npc.Center.Y), new Vector2(npc.width, npc.height), ModContent.ItemType<Items.Tools.SuperHeartPickup>());


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


            }
        }
        public override void SetDefaults(NPC npc)
        {

            if (npc.boss)
            {
                npc.buffImmune[(BuffType<BeetleDebuff>())] = true;
            }

        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (bloodDebuff)
            {
                npc.lifeRegen -= 12;
                damage = 2;
            }
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
            if (boulderDB)
            {
                npc.lifeRegen -= 60;

                damage = 8;

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
            if (spectreDebuff) //unused
            {
                npc.lifeRegen -= 120;

                damage = 14;

            }

            if (superBoulderDB)
            {
                npc.lifeRegen -= 120;

                damage = 12;

            }

            if (nebula) //unused
            {
                npc.lifeRegen -= 180;
                damage = 20;
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
            if (boulderDB)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 1, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 1.5f);
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
            if (superBoulderDB)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 55, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                Lighting.AddLight(npc.position, 1f, 0.5f, 0f);
            }
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
                Lighting.AddLight(npc.position, 1f, 0.5f, 0f);
            }
            if (nebula)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 130, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                Lighting.AddLight(npc.position, 1f, 0.5f, 0.8f);
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
                Lighting.AddLight(npc.position, 0.1f, 0.2f, 0.7f);
            }


            if (beetled)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<BeetleDust>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, default, 1f);
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
            if (spectreDebuff)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 16, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, default, 1f);
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
            if (bloodDebuff)
            {
                if (Main.rand.Next(4) < 3)
                /*{
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 5, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 5f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }*/
                {
                    var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 95);
                    dust.noGravity = true;

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
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<SpookDust>(), npc.velocity.X * 1.2f, -5, 0, default, 2.5f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                }
                drawColor = new Color(255, 68, 0);

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
            if (projectile.type == ModContent.ProjectileType<Projectiles.AncientArmourProj>()) //No crit from arid explosion
            {
                crit = false;
            }

            if (npc.type == NPCType<NPCs.Boss.StormBoss>()) //75% damage from homing projectiles
            {
                if (ProjectileID.Sets.CultistIsResistantTo[projectile.type])
                {
                    damage = (damage * 3) / 4;
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
            /*
            if (crit)
            {
                if (player.GetModPlayer<EquipmentEffects>().aridCritSet == true)
                {
                    NPC.immuneTime = 10;
                }
            }*/
        }
        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {

        }

    }

}

        
