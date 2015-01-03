#region

using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
    public class RecallTracker
    {


        public static Menu RecallM;


        private static Menu _RecallChat;
        private static MenuItem _reacallstart, _reacallabort, _reacallfinish, _reacalltimer, _reacallActive;

        private static Menu _RecallDraw;
        private static MenuItem _Recallcircle, _RecallChamp, _RecallTimer, _ActiveReacallDraw;

        private static readonly Render.Text _text = new Render.Text("", 0, 0, 24, SharpDX.Color.White);
        private static readonly Render.Text _text2 = new Render.Text("", 0, 0, 24, SharpDX.Color.White);

        private static readonly Obj_AI_Hero _hero;
        private static int _duration;
        private static float _begin;

        private static bool _Dactive;
        private static Obj_AI_Hero RecallC;

        public static void RecallTrackerRun()
        {
            RecallM = new Menu("RecallTracker", "RecallTracker");


            _RecallChat = new Menu("Reacall Chat", "Recallchat");

            _reacallstart = new MenuItem("Recallstart", "Notifiy Recall Start").SetValue(true);
            _reacallabort = new MenuItem("Recallabort", "Notifiy Recall Aborted").SetValue(true);
            _reacallfinish = new MenuItem("Recallfinish", "Notifiy Recall Finished").SetValue(true);
            _reacalltimer = new MenuItem("Recalltimer", "Notifiy Chat with Time").SetValue(true);
            _reacallActive = new MenuItem("ActiveRecall", "Active Recall Notification").SetValue(false);

            _RecallDraw = new Menu("Reacall Draw", "Reacalldraw");
            _Recallcircle = new MenuItem("Recallcircle", "Draw Circle").SetValue(new Circle(true, Color.Red));
            _RecallChamp = new MenuItem("RecallChamp", "Draw Champ Name").SetValue(false);
            _RecallTimer = new MenuItem("RecallTimer", "Draw Recall Timer").SetValue(false);
            _ActiveReacallDraw = new MenuItem("ActiveReacallDraw", "Active Recall Draw").SetValue(false);


            Menus.Menu.AddSubMenu(RecallM);
            RecallM.AddSubMenu(_RecallChat);
            RecallM.SubMenu("Recallchat").AddItem(_reacallstart);
            RecallM.SubMenu("Recallchat").AddItem(_reacallabort);
            RecallM.SubMenu("Recallchat").AddItem(_reacallfinish);
            RecallM.SubMenu("Recallchat").AddItem(_reacalltimer);
            RecallM.SubMenu("Recallchat").AddItem(_reacallActive);


            RecallM.AddSubMenu(_RecallDraw);
            RecallM.SubMenu("Reacalldraw").AddItem(_Recallcircle);
            RecallM.SubMenu("Reacalldraw").AddItem(_RecallChamp);
            RecallM.SubMenu("Reacalldraw").AddItem(_RecallTimer);
            RecallM.SubMenu("Reacalldraw").AddItem(_ActiveReacallDraw);



            Obj_AI_Base.OnTeleport += Obj_AI_Base_OnTeleport;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            DrawingP();
        }

        private static void DrawingP()
        {
            if (_Dactive && RecallM.Item("ActiveReacallDraw").GetValue<bool>())
            {
                if (RecallM.Item("Recallcircle").GetValue<Circle>().Active)
                {
                    Utility.DrawCircle(RecallC.ServerPosition, 120, RecallM.Item("Recallcircle").GetValue<Circle>().Color);
                }

                if (RecallM.Item("RecallChamp").GetValue<bool>())
                {
                    _text.Color = SharpDX.Color.Red;
                    _text.Centered = true;
                    _text.text = RecallC.ChampionName;
                    _text.X = (int)Drawing.WorldToScreen(RecallC.ServerPosition).X;
                    _text.Y = (int)Drawing.WorldToScreen(RecallC.ServerPosition).Y;
                    _text.OutLined = true;
                    _text.OnEndScene();
                }

                if (RecallM.Item("RecallTimer").GetValue<bool>())
                {
                    _text2.Color = SharpDX.Color.Red;
                    _text2.Centered = true;
                    _text2.text = Math.Round(
                        (Decimal)((_duration / 1000f) - (Game.ClockTime - _begin)), 1, MidpointRounding.AwayFromZero).ToString(); //Math coded by TheSaltyWaffle 
                    _text2.X = (int)Drawing.WorldToScreen(RecallC.ServerPosition).X;
                    _text2.Y = (int)Drawing.WorldToScreen(RecallC.ServerPosition).Y + 15;
                    _text2.OutLined = true;
                    _text2.OnEndScene();
                }

            }
        }

        // ===========================================================================================================
        //  Coded by TheSaltyWaffle in UniversalRecallTracker
        // ===========================================================================================================
        private static void Obj_AI_Base_OnTeleport(GameObject sender, GameObjectTeleportEventArgs args)
        {
            foreach (
                var enemy in
                    ObjectManager.Get<Obj_AI_Hero>().Where(o => o.IsEnemy))
            {
                Packet.S2C.Teleport.Struct decoded = Packet.S2C.Teleport.Decoded(sender, args);
                if (decoded.UnitNetworkId == enemy.NetworkId && decoded.Type == Packet.S2C.Teleport.Type.Recall)
                {

                    var recallstart = RecallM.Item("Recallstart").GetValue<bool>();
                    var recalled = RecallM.Item("Recallfinish").GetValue<bool>();
                    var recallabort = RecallM.Item("Recallabort").GetValue<bool>();


                    if (!RecallM.Item("ActiveRecall").GetValue<bool>()) return;
                    if (recallstart || recalled || recallabort)
                    {
                        switch (decoded.Status)
                        {
                            case Packet.S2C.Teleport.Status.Start:
                                if (!recallstart) return;
                                _begin = Game.ClockTime;
                        _duration = decoded.Duration;
                                RecallC = enemy;
                                _Dactive = true;

                                timechat("[<font color=\"#4EE2EC\">" + enemy.ChampionName + "</font>] <font color=\"#4CC552\">starts</font> to recall");
                                break;
                            case Packet.S2C.Teleport.Status.Finish:
                                if (!recalled) return;

                                timechat("[<font color=\"#4EE2EC\">" + enemy.ChampionName + "</font>] <font color=\"#5CB3FF\">has recalled</font>");
                             
                                _Dactive = false;
                                break;
                            case Packet.S2C.Teleport.Status.Abort:
                                if (!recallabort) return;

                                _Dactive = false;
                      
                                timechat("[<font color=\"#4EE2EC\">" + enemy.ChampionName + "</font>] <font color=\"#FF2400\">aborted</font> recalling");
                                break;
                            case Packet.S2C.Teleport.Status.Unknown:

                                break;
                        }

                    }
             
                }
            }
        }

   

        public static void timechat(string msg)
        {
            if (RecallM.Item("Recalltimer").GetValue<bool>())
                Game.PrintChat("<font color='#d8d8d8'>[" + (Utils.FormatTime(Game.ClockTime - 15)) + "]</font> " + msg); // Coded by TheSaltyWaffle 
            else
                Game.PrintChat(msg);
        }

        // ===========================================================================================================
        //  
        // ===========================================================================================================
    }
}
