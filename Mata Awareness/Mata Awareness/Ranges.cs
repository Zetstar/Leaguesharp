#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace Mata_Awareness
{
    public class Ranges
    {
        public static Menu RangesM;

        private static Obj_AI_Hero Player = ObjectManager.Player;
        private static Menu _turretrange;
        private static MenuItem _turretrangeAlly, _turretrangeEnemy, _turretrangeMax, _turretrangeQu, _turretrangeThick, _turretrangeActive;

        private static Menu _shoprange;
        private static MenuItem _shoprangeColor, _shoprangeMax, _shoprangeQu, _shoprangeThick, _shoprangeActive;

        private static Menu _aaRange;
        private static MenuItem _myaaRange, _enemyaarangeOn, _enemyaarangeOff, _aaQu, _aaThick, _aarangeActive;

        public static void RangesMenu()
        {
            RangesM = new Menu("Ranges", "Ranges");


            _turretrange = new Menu("Turret Range", "Turretrange");
            _turretrangeAlly = new MenuItem("AllyTurret", "Ally Turret Range").SetValue(new Circle(true, Color.Lime));
            _turretrangeEnemy = new MenuItem("EnemyTurret", "Enemy Turret Range").SetValue(new Circle(true, Color.Red));
            _turretrangeMax = new MenuItem("MaxRange", "Max Range").SetValue(new Slider(2000, 8000, 100));
            _turretrangeQu = new MenuItem("Thickness", "Circle Thickness [Default = 5]").SetValue(new Slider(5, 20, 1));
            _turretrangeThick = new MenuItem("Quality", "Circle Quality [Default = 30]").SetValue(new Slider(30, 100, 1));
            _turretrangeActive = new MenuItem("ActiveTurret", "Active Turret Range").SetValue(false);

            _shoprange = new Menu("Shop Range", "shoprange");
            _shoprangeColor = new MenuItem("Shoprange", "Shop Range").SetValue(new Circle(true, Color.DodgerBlue));
            _shoprangeMax = new MenuItem("ShopMaxRange", "Max Range").SetValue(new Slider(2000, 8000, 100));
            _shoprangeQu = new MenuItem("SThickness", "Circle Thickness [Default = 5]").SetValue(new Slider(5, 20, 1));
            _shoprangeThick = new MenuItem("SQuality", "Circle Quality [Default = 30]").SetValue(new Slider(30, 100, 1));
            _shoprangeActive = new MenuItem("ActiveShop", "Active Shop Range").SetValue(false);

            _aaRange = new Menu("AA Ranges", "aaranges");
            _myaaRange = new MenuItem("Myrange", "My AA Range").SetValue(new Circle(true, Color.Lime));
            _enemyaarangeOn = new MenuItem("EnemyAAon", "Enemy AA out Range").SetValue(new Circle(true, Color.Lime));
            _enemyaarangeOff = new MenuItem("EnemyAAoff", "Enemy AA in Range").SetValue(new Circle(true, Color.Red));
            _aaQu = new MenuItem("AAThickness", "Circle Thickness [Default = 5]").SetValue(new Slider(5, 20, 1));
            _aaThick = new MenuItem("AAQuality", "Circle Quality [Default = 30]").SetValue(new Slider(30, 100, 1));
            _aarangeActive = new MenuItem("AAactive", "Active Show AA Ranges").SetValue(false);

            Menus.Menu.AddSubMenu(RangesM);
            RangesM.AddSubMenu(_turretrange);
            RangesM.SubMenu("Turretrange").AddItem(_turretrangeAlly);
            RangesM.SubMenu("Turretrange").AddItem(_turretrangeEnemy);
            RangesM.SubMenu("Turretrange").AddItem(_turretrangeMax);
            RangesM.SubMenu("Turretrange").AddItem(_turretrangeQu);
            RangesM.SubMenu("Turretrange").AddItem(_turretrangeThick);
            RangesM.SubMenu("Turretrange").AddItem(_turretrangeActive);

            RangesM.AddSubMenu(_shoprange);
            RangesM.SubMenu("shoprange").AddItem(_shoprangeColor);
            RangesM.SubMenu("shoprange").AddItem(_shoprangeMax);
            RangesM.SubMenu("shoprange").AddItem(_shoprangeQu);
            RangesM.SubMenu("shoprange").AddItem(_shoprangeThick);
            RangesM.SubMenu("shoprange").AddItem(_shoprangeActive);

             RangesM.AddSubMenu(_aaRange);
            RangesM.SubMenu("aaranges").AddItem(_myaaRange);
            RangesM.SubMenu("aaranges").AddItem(_enemyaarangeOn);
            RangesM.SubMenu("aaranges").AddItem(_enemyaarangeOff);
            RangesM.SubMenu("aaranges").AddItem(_aaQu);
            RangesM.SubMenu("aaranges").AddItem(_aaThick);
            RangesM.SubMenu("aaranges").AddItem(_aarangeActive);


            Drawing.OnDraw += Drawing_OnDraw;

        }

        public static void DrawingAA()
        {
            var aaActive = RangesM.Item("AAactive").GetValue<bool>();

            if (!aaActive) return;

            var MyAARange = RangesM.Item("Myrange").GetValue<Circle>();
            var EnemyAAOn = RangesM.Item("EnemyAAon").GetValue<Circle>();
            var EnemyAAOff = RangesM.Item("EnemyAAoff").GetValue<Circle>();
            var AAThick = RangesM.Item("AAThickness").GetValue<Slider>().Value;
            var AAQual = RangesM.Item("AAQuality").GetValue<Slider>().Value;
            


            if (MyAARange.Active)
                Utility.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), MyAARange.Color, AAThick,
                    AAQual);

            foreach (var target in ObjectManager.Get<Obj_AI_Hero>().Where(target => target.IsValidTarget(2000))) // coded by Trees
            {
                var realAARange = target.AttackRange + target.BoundingRadius;

                if (EnemyAAOn.Active && EnemyAAOff.Active)
                {
                    Utility.DrawCircle(target.Position, realAARange, Player.Distance(target) >= realAARange ? EnemyAAOn.Color : EnemyAAOff.Color, AAThick, AAQual);
                }
                else if (EnemyAAOn.Active)
                {
                    Utility.DrawCircle(target.Position, realAARange, EnemyAAOn.Color, AAThick, AAQual);
                }
                else if (EnemyAAOff.Active)
                {
                    Utility.DrawCircle(target.Position, realAARange, Player.Distance(target) >= realAARange ? Color.Empty : EnemyAAOff.Color, AAThick, AAQual);
                }


            }

        }

        public static void DrawingTurret()
        {
            var ActiveTcheck = RangesM.Item("ActiveTurret").GetValue<bool>();
            var AllyTurret = RangesM.Item("AllyTurret").GetValue<Circle>().Active;
            var EnemyTurret = RangesM.Item("EnemyTurret").GetValue<Circle>().Active;

            if (!ActiveTcheck || (!AllyTurret && !EnemyTurret)) return;

            var CThick = RangesM.Item("Thickness").GetValue<Slider>().Value;
            var CQuality = RangesM.Item("Quality").GetValue<Slider>().Value;
            var TurretDistance = RangesM.Item("MaxRange").GetValue<Slider>().Value;
  
            foreach (Obj_AI_Turret turret in ObjectManager.Get<Obj_AI_Turret>().Where(turret => turret.IsVisible && !turret.IsDead && turret.IsValid && ObjectManager.Player.ServerPosition.Distance(turret.ServerPosition) < TurretDistance && (turret.IsAlly && AllyTurret || turret.IsEnemy && EnemyTurret)))
            {
                Utility.DrawCircle(turret.Position, 900f, RangesM.Item(turret.IsAlly ? "AllyTurret" : "EnemyTurret").GetValue<Circle>().Color, CThick, CQuality);
            }

        }

        public static void DrawingShop()
        {
            var ActiveShop = RangesM.Item("ActiveShop").GetValue<bool>();
            var ShopRange = RangesM.Item("Shoprange").GetValue<Circle>().Active;
            var Map = Utility.Map.GetMap();

            if (!ActiveShop || !ShopRange || Map == null) return;

            var SThick = RangesM.Item("SThickness").GetValue<Slider>().Value;
            var SQuality = RangesM.Item("SQuality").GetValue<Slider>().Value;
            var shopRangeD = 1050;
            var ShopP = new Vector3();

                foreach (
                     var shop in
                         ObjectManager.Get<Obj_Building>().Where(shop => shop.IsAlly && shop.IsValid && ObjectManager.Player.Team == shop.Team && ObjectManager.Player.ServerPosition.Distance(shop.Position) < RangesM.Item("ShopMaxRange").GetValue<Slider>().Value))
                {
                    if (Map.Type == Utility.Map.MapType.SummonersRift && shop.Name.Contains("__Spawn_T"))
                    {
                        ShopP = new Vector3(shop.Position.X, shop.Position.Y, shop.Position.Z - 100f);
                        Utility.DrawCircle(ShopP, shopRangeD, RangesM.Item("Shoprange").GetValue<Circle>().Color, SThick, SQuality);
                    }

                    else if (Map.Type != Utility.Map.MapType.SummonersRift && shop.Name.Contains("rderShop"))
                    {
                  
                        switch (Map.Type)
                        {
                            case Utility.Map.MapType.TwistedTreeline:
                                shopRangeD = 1250;
                                ShopP = new Vector3(shop.Position.X, shop.Position.Y, shop.Position.Z - 100f);
                                break;
                            case Utility.Map.MapType.HowlingAbyss:
                                shopRangeD = 1250;
                                ShopP = new Vector3(shop.Position.X, shop.Position.Y, shop.Position.Z - 100f);
                                break;
                            case Utility.Map.MapType.CrystalScar:
                                shopRangeD = 1200;
                                ShopP = new Vector3(shop.Position.X, shop.Position.Y, shop.Position.Z - 200f);
                                break;
                        }

                        Utility.DrawCircle(ShopP, shopRangeD, RangesM.Item("Shoprange").GetValue<Circle>().Color, SThick, SQuality);
                    }
                        
                } 
            
          
               
            
        }



        private static void Drawing_OnDraw(EventArgs args)
        {
            if (ObjectManager.Player.IsDead) return;
            DrawingTurret();
            DrawingShop();
            DrawingAA();
        }
    }
}
