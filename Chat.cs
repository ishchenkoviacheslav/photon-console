using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TryToTestPhoton
{
    public class Chat
    {
        private string ChatLog = "";
        private string message = "";
        public void Start()
        {
            PhotonServer.Instance.OnReceiveChatMessage += OnReceiveChatMessage;
            PhotonServer.Instance.GetRecentChatMessage();
        }
        public void OnDestroy()
        {
            PhotonServer.Instance.OnReceiveChatMessage -= OnReceiveChatMessage;

        }

        private void OnReceiveChatMessage(object sender, TestPhotonLib.Common.CustomEventArgs.ChatMessageEventArgs e)
        {
            ChatLog +=e.Message + "\r\n"+ChatLog;
        }

        public void Update()
        {

        }
        public void OnGUI()
        {
            Console.WriteLine(ChatLog);
            message = Console.ReadLine();
            if (message.Length == 0)
                return;
            PhotonServer.Instance.SendChatMessage(message);
            message = "";
        }
    }
}
