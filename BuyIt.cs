using BuyIt.Utils;
using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BuyIt
{

        public class BuyIt : BaseSettingsPlugin<BuyItSettings>
    {
        private Stopwatch Timer { get; } = new Stopwatch();
        private Random Random { get; } = new Random();
        public static BuyIt Controller { get; set; }

        private RectangleF Gamewindow;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hwnd, StringBuilder ss, int count);
        public override bool Initialise()
        {
            Controller = this;
            Gamewindow = GameController.Window.GetWindowRectangle();
            
            Timer.Start();
            return true;
        }
        private bool isPOEActive()
        {

            if (ActiveWindowTitle().IndexOf("Path of Exile", 0, StringComparison.CurrentCultureIgnoreCase) == -1)
            {
                if (Settings.DebugMode)
                    LogMessage("(ClickIt) Path of exile window not active");
                return false;
            }
            return true;
        }
        private bool isTujenWindowOpen()
        {

            if (GameController.IngameState.IngameUi.HaggleWindow.Address != 0)
            {
                return true;
            }
            return false;
        }

        public override Job Tick()
        {
            if (Timer.ElapsedMilliseconds < Settings.WaitTimeInMs.Value - 10 + Random.Next(0, 20))
                return null;
            Timer.Restart();
            PerformAction();
            return null;
        }

        private static string ActiveWindowTitle()
        {
            const int nChar = 256;
            StringBuilder ss = new StringBuilder(nChar);
            IntPtr handle = IntPtr.Zero;
            handle = GetForegroundWindow();
            if (GetWindowText(handle, ss, nChar) > 0) return ss.ToString();
            else return "";
        }

        private enum ArtifactValues
        {
            /*
             * These are values from expedition league, slightly adjusted by me.
             * https://i.redd.it/qx9lhpiseyf71.png
             */
            LesserBlackScythe = 1/55,
            GreaterBlackScythe = 1/95,
            GrandBlackScythe = 1/100,
            ExceptionalBlackScythe = 1/7
        }

        private void PerformAction()
        {
            try
            {
                if (!Input.GetKeyState(Settings.ClickLabelKey.Value))
                    return;
                if (!isPOEActive())
                    return;
                if (!isTujenWindowOpen())
                    return;
                Element haggleWindow = GameController.IngameState.IngameUi.HaggleWindow;

            }
            catch (Exception e)
            {
                LogError(e.ToString());
            }
        }
    }
}
