using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace TryToTestPhoton
{
    class Program
    {
        static void Main(string[] args)
        {
            PhotonServer ps = new PhotonServer();
            ps.Start();
            while (true)
            {
                Thread.Sleep(1000);
                ps.Update();

            }
        }
    }
}
