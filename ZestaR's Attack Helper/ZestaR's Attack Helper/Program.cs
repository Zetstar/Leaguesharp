#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
#endregion

namespace ZestaR_s_Attack_Helper
{
    class Program
    {
        public static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }
        public static Orbwalking.Orbwalker Orbwalker;
        public static Menu Menu;
        public const string VA = "1.0.0";


        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        public static void Game_OnGameLoad(EventArgs args)
        {

            Menu = new Menu("ZestaR's Attack Helper", "ZestaR's Attack Helper", true);
            Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(Menu.SubMenu("Orbwalker"));

            Menu.AddSubMenu(new Menu("Orbwalker Disable", "Disable"));
            Menu.SubMenu("Disable").AddItem(new MenuItem("DisMove", "Disable Movements").SetValue(false));
            Menu.SubMenu("Disable").AddItem(new MenuItem("DisAtt", "Disable Attack").SetValue(false));

            Menu.AddSubMenu(new Menu("AA Reset", "AA Reset")); // reset script made by SamuraiGilgamesh
            Menu.SubMenu("AA Reset").AddItem(new MenuItem("ChampActive", "AA Reset for Champions").SetValue(new KeyBind(32, KeyBindType.Press)));
            Menu.SubMenu("AA Reset").AddItem(new MenuItem("ChampEnabled", "AA Reset for Champions [Toggle]").SetValue(new KeyBind("Y".ToCharArray()[0], KeyBindType.Toggle, true)));
            Menu.SubMenu("AA Reset").AddItem(new MenuItem("TowerActive", "AA Reset for Towers").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));
            Menu.SubMenu("AA Reset").AddItem(new MenuItem("TowerEnabled", "AA Reset for Towers [Toggle]").SetValue(new KeyBind("H".ToCharArray()[0], KeyBindType.Toggle, true)));

            Menu.AddSubMenu(new Menu("Drawings", "Drawing"));
            Menu.SubMenu("Drawing").AddItem(new MenuItem("Myrange", "My AA Range").SetValue(new Circle(true, Color.Lime)));
            Menu.SubMenu("Drawing")
                .AddItem(new MenuItem("EnemyAAoff", "Enemy AA Range").SetValue(new Circle(true, Color.Lime)));
            Menu.SubMenu("Drawing")
                .AddItem(new MenuItem("EnemyAAon", "Enemy AA Range").SetValue(new Circle(true, Color.Red)));
            Menu.AddToMainMenu();

            Menu.SubMenu("Drawing").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(false)); // from SKOKarma
            Menu.SubMenu("Drawing").AddItem(new MenuItem("CircleQuality", "Circles Quality").SetValue(new Slider(100, 100, 10)));
            Menu.SubMenu("Drawing").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;

            AAreset.Champlist();

            Game.PrintChat("<font color=\"#4EE2EC\">ZestaR's Attack Helper</font> - " + VA + " v [Beta]");
  

        }




        private static void Drawing_OnDraw(EventArgs args)
        {


            if (Menu.Item("CircleLag").GetValue<bool>())
            {

                if (Menu.Item("Myrange").GetValue<Circle>().Active && Menu.Item("EnemyAAon").GetValue<Circle>().Active)
                    Utility.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), Menu.Item("Myrange").GetValue<Circle>().Color,
                            Menu.Item("CircleThickness").GetValue<Slider>().Value,
                            Menu.Item("CircleQuality").GetValue<Slider>().Value);

                foreach (var target in ObjectManager.Get<Obj_AI_Hero>().Where(target => target.IsValidTarget(2000)))
                {

                    if (Menu.Item("EnemyAAoff").GetValue<Circle>().Active &&
                        Menu.Item("EnemyAAon").GetValue<Circle>().Active)
                    {
                        Utility.DrawCircle(target.Position, target.AttackRange + target.BoundingRadius,
                            Player.Distance(target) > target.AttackRange + target.BoundingRadius
                                ? Menu.Item("EnemyAAoff").GetValue<Circle>().Color
                                : Menu.Item("EnemyAAon").GetValue<Circle>().Color,
                            Menu.Item("CircleThickness").GetValue<Slider>().Value,
                            Menu.Item("CircleQuality").GetValue<Slider>().Value);
                        // Game.PrintChat("AA" + target.AttackRange + target.BoundingRadius);
                    }
                    //   Drawing.DrawCircle(target.Position, target.AttackRange + target.BoundingRadius, ObjectManager.Player.Distance(target) < target.AttackRange + target.BoundingRadius ? Color.Red : Color.LimeGreen);

                    if (Menu.Item("EnemyAAoff").GetValue<Circle>().Active &&
                        !Menu.Item("EnemyAAon").GetValue<Circle>().Active)
                    {
                        Utility.DrawCircle(target.Position, target.AttackRange + target.BoundingRadius,
                            Menu.Item("EnemyAAoff").GetValue<Circle>().Color,
                            Menu.Item("CircleThickness").GetValue<Slider>().Value,
                            Menu.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                    if (!Menu.Item("EnemyAAoff").GetValue<Circle>().Active &&
                        Menu.Item("EnemyAAon").GetValue<Circle>().Active)
                    {
                        Utility.DrawCircle(target.Position, target.AttackRange + target.BoundingRadius,
                            Player.Distance(target) > target.AttackRange + target.BoundingRadius
                                ? Color.Empty
                                : Menu.Item("EnemyAAon").GetValue<Circle>().Color,
                            Menu.Item("CircleThickness").GetValue<Slider>().Value,
                            Menu.Item("CircleQuality").GetValue<Slider>().Value);
                    }


                }
            }
            else
            {
                if (Menu.Item("Myrange").GetValue<Circle>().Active && Menu.Item("EnemyAAon").GetValue<Circle>().Active)
                    Utility.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), Menu.Item("Myrange").GetValue<Circle>().Color);

                foreach (var target in ObjectManager.Get<Obj_AI_Hero>().Where(target => target.IsValidTarget(2000)))
                {

                    if (Menu.Item("EnemyAAoff").GetValue<Circle>().Active &&
                        Menu.Item("EnemyAAon").GetValue<Circle>().Active)
                    {
                        Utility.DrawCircle(target.Position, target.AttackRange + target.BoundingRadius,
                            Player.Distance(target) > target.AttackRange + target.BoundingRadius
                                ? Menu.Item("EnemyAAoff").GetValue<Circle>().Color
                                : Menu.Item("EnemyAAon").GetValue<Circle>().Color);
                    }

                    if (Menu.Item("EnemyAAoff").GetValue<Circle>().Active &&
                        !Menu.Item("EnemyAAon").GetValue<Circle>().Active)
                    {
                        Utility.DrawCircle(target.Position, target.AttackRange + target.BoundingRadius,
                            Menu.Item("EnemyAAoff").GetValue<Circle>().Color);
                    }
                    if (!Menu.Item("EnemyAAoff").GetValue<Circle>().Active &&
                        Menu.Item("EnemyAAon").GetValue<Circle>().Active)
                    {
                        Utility.DrawCircle(target.Position, target.AttackRange + target.BoundingRadius,
                            Player.Distance(target) > target.AttackRange + target.BoundingRadius
                                ? Color.Empty
                                : Menu.Item("EnemyAAon").GetValue<Circle>().Color);
                    }


                }
            }
        }

        private static void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (unit.IsMe && target.IsValidTarget())
            {
                var championName = Player.ChampionName;
                if (target.Type == GameObjectType.obj_AI_Hero && (Menu.Item("ChampEnabled").GetValue<KeyBind>().Active || (Menu.Item("ChampActive").GetValue<KeyBind>().Active)))
                {
                    if (AAreset.Special == true)
                    {
                        AAreset.Reset.Cast();
                        Utility.DelayAction.Add(200,
                            () => ObjectManager.Player.IssueOrder(GameObjectOrder.AttackUnit, target));

                        //ObjectManager.Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    }
                    else if (championName == "Tristana")
                    {
                        var eTarget = TargetSelector.GetTarget(703, TargetSelector.DamageType.Physical);
                        if (AAreset.Reset.IsReady() && eTarget.IsValidTarget())
                            AAreset.Reset.CastOnUnit(eTarget);
                    }
                    else
                    {
                        AAreset.Reset.Cast();
                    }

                }
                else if (target.Type == GameObjectType.obj_AI_Turret && (Menu.Item("TowerEnabled").GetValue<KeyBind>().Active || (Menu.Item("TowerActive").GetValue<KeyBind>().Active)))
                {
                    AAreset.Reset.Cast();
                }
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead) return;

            if (Menu.Item("DisMove").GetValue<bool>())
            {
                Orbwalker.SetMovement(false);
            }
            else
            {
                Orbwalker.SetMovement(true);
            }


            if (Menu.Item("DisAtt").GetValue<bool>())
            {
                Orbwalker.SetAttack(false);
            }
            else
            {
                Orbwalker.SetAttack(true);
            }


        }
    }
}
