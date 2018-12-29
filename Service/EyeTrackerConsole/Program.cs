using EyeXFramework;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace EyeTrackerConsole
{
    class Program

    {

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }


        public static int GetWindowsScaling()
        {
            return (int)(100 * Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth);
        }

        public static void Main(string[] args)
        {
            var host = new EyeXHost();
            host.Start();
            Console.WriteLine("Current scale:" + GetWindowsScaling());
            Console.ReadKey();
            var gazePointDataStream = host.CreateGazePointDataStream(Tobii.EyeX.Framework.GazePointDataMode.Unfiltered);
            gazePointDataStream.Next += (s,e) => Console.WriteLine("X: {0} Y:{1}", e.X, e.Y);
            Console.ReadKey();
        }
    }
}
