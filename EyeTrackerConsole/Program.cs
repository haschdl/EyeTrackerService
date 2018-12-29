using EyeXFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobii.EyeX;

namespace EyeTrackerConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            var host = new EyeXHost();
            host.Start();
            var gazePointDataStream = host.CreateGazePointDataStream(Tobii.EyeX.Framework.GazePointDataMode.Unfiltered);
            gazePointDataStream.Next += (s,e) => Console.WriteLine("X: {0} Y:{1}", e.X, e.Y);

        }
    }
}
