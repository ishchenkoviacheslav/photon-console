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
            PhotonServer ps = PhotonServer.Instance;
            ps.Start();

            Login login = new Login();


            for (int i = 0; true; i++)
            {
                Thread.Sleep(1000);
                ps.Update();
                if(i%10==0)
                {
                    i = 0;
                    login.Start();
                    login.OnGUI();
                }
                else
                {
                    login.Start();
                    login.OnGUI("ivanov");
                }

            }
        }
    }
}
