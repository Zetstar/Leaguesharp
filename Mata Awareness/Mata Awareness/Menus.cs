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
using Color = System.Drawing.Color;

#endregion

namespace Mata_Awareness
{
    public class Menus
    {
        public static Menu Menu;

     
        public static void Open()
        {
            Menu = new Menu("Mata Awareness", "Mata Awareness", true);


            Ranges.RangesMenu();
            ExtraInfo.ExtraInfoRun();
            RecallTracker.RecallTrackerRun();
            Menu.AddToMainMenu();


        }

    }
}
