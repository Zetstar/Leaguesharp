#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;

#endregion

namespace Mata_Awareness
{
    public class ExtraInfo
    {
        private static Menu Extrainfo;
        private static Menu _time;
      
        private static MenuItem _clockActive;
        private static MenuItem _clockX;
        private static MenuItem _clockY;
     //   private static MenuItem _clockfont;

        private static Menu _ping;
        private static MenuItem _pingX;
        private static MenuItem _pingY;
   //     private static MenuItem _pingfont;
        private static MenuItem _pingActive;

        public static String Time, Ping;


        private static readonly Render.Text _text = new Render.Text("", 0, 0, 24, SharpDX.Color.White);

        public static void ExtraInfoRun()
        {

            Extrainfo = new Menu("Extra Info", "Extrainfo");

            _time = new Menu("Realtime Clock", "Clock");
            
            _clockX = new MenuItem("ClockX", "Clock X Position").SetValue(new Slider(0, 50, -50));
            _clockY = new MenuItem("ClockY", "Clock Y Position").SetValue(new Slider(0, 50, -50));
        //    _clockfont = new MenuItem("Clockfont", "Clock Font Size").SetValue(new Slider(12, 24, 6));
            _clockActive = new MenuItem("ClockActive", "Show Clock").SetValue(false);

            _ping = new Menu("Latency", "Ping");
            _pingX = new MenuItem("PingX", "Ping X Position").SetValue(new Slider(0, 50, -50));
            _pingY = new MenuItem("PingY", "Ping Y Position").SetValue(new Slider(0, 50, -50));
        //    _pingfont = new MenuItem("Pingfont", "Ping Font Size").SetValue(new Slider(12, 24, 6));
            _pingActive = new MenuItem("PingActive", "Show Ping").SetValue(false);

            Menus.Menu.AddSubMenu(Extrainfo);
            Extrainfo.AddSubMenu(_time);
            Extrainfo.SubMenu("Clock").AddItem(_clockX);
            Extrainfo.SubMenu("Clock").AddItem(_clockY);
       //     Extrainfo.SubMenu("Clock").AddItem(_clockfont);
            Extrainfo.SubMenu("Clock").AddItem(_clockActive);
            Extrainfo.AddSubMenu(_ping);
            Extrainfo.SubMenu("Ping").AddItem(_pingX);
            Extrainfo.SubMenu("Ping").AddItem(_pingY);
          //  Extrainfo.SubMenu("Ping").AddItem(_pingfont);
            Extrainfo.SubMenu("Ping").AddItem(_pingActive);
            Drawing.OnDraw += Drawing_OnDraw;
           // DrawingClock();
        }

        public static void DrawingClock()  //coded by daYMAN007
        {

           // float StartX = Drawing.Width - 100f, StartY = Drawing.Height / 16f;
            if (Extrainfo.Item("ClockActive").GetValue<bool>())
            {
                var clockX1 = Extrainfo.Item("ClockX").GetValue<Slider>().Value;
                var clockY1 = Extrainfo.Item("ClockY").GetValue<Slider>().Value;
           //     var clockfont = (Extrainfo.Item("Clockfont").GetValue<Slider>().Value)*2;
   
               
                Time = DateTime.Now.ToString("hh:mm tt", new CultureInfo("en-US"));


              
                _text.Color = SharpDX.Color.White;
             //   _text.TextFontDescription.Height = clockfont;
              //  test2 = clockfont;
                _text.text = Time;
                
                _text.X = (int) (Drawing.Width - 100f) + clockX1;
                _text.Y = (int) (Drawing.Height / 16f) + clockY1;
                _text.OutLined = true;
                _text.OnEndScene(); 
            }

        }

        public static void DrawingPing()
        {

            if (Extrainfo.Item("PingActive").GetValue<bool>())
            {
                var pingX1 = Extrainfo.Item("PingX").GetValue<Slider>().Value;
                var pingY1 = Extrainfo.Item("PingY").GetValue<Slider>().Value;
             //   int pingfont = (Extrainfo.Item("Pingfont").GetValue<Slider>().Value) * 2;

                Ping = Game.Ping.ToString();
                _text.Color = SharpDX.Color.Lime;

             //   _text.TextFontDescription.Height = pingfont;
                _text.text = Ping + " (ms)";

                _text.X = (int)(Drawing.Width - 90f) + pingX1;
                _text.Y = (int)((Drawing.Height / 16f) + 20f) + pingY1;
                _text.OutLined = true;
                _text.OnEndScene(); 
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
        //    if (ObjectManager.Player.IsDead) return;
            DrawingClock();
            DrawingPing();
        }
    }
}
