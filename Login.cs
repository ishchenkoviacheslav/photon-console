using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestPhotonLib.Common;

namespace TryToTestPhoton
{
    public class Login
    {
        public void Start()
        {
            PhotonServer.Instance.OnLoginResponse += OnLoginHandler;
        }
        private string Error { get; set; }
        private string CharacterName {get;set;}
        public void OnGUI(string name = "petrov")
        {
            //click..
            Error = "";
            PhotonServer.Instance.SendLoginOperation(name);

            Console.WriteLine("Info from OnGUI");
        }
        private void OnLoginHandler(Object o, LoginEventArgs e)
        {
            if(e.Error != ErrorCode.OK)
            {
                Error = "ErrorCode not OK: " + e.Error.ToString();
                return;
            }
            PhotonServer.Instance.OnLoginResponse -= OnLoginHandler;

            //some new scene in unity
            Console.WriteLine("New scene in console))");
        }
    }
}
