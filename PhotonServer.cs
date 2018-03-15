using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;
using TestPhotonLib.Common;
using TestPhotonLib.Common.CustomEventArgs;
//using UnityEngine;
namespace TryToTestPhoton
{
    class PhotonServer : IPhotonPeerListener
    {
        private const string CONNECTION_STRING = "localhost:5055";
        private const string APP_NAME = "MyTestServer";

        public static PhotonServer Instance { get; } = new PhotonServer();
        private PhotonPeer PhotonPeer
        {
            get;
            set;
        }

        public event EventHandler<LoginEventArgs> OnLoginResponse;
        public event EventHandler<ChatMessageEventArgs> OnReceiveChatMessage;

        //public void Awake()
        //{
        //    //if (Instance != null)
        //    //{
        //    //    DestroyObject(gameObject);
        //    //    return;//some problem with singletone....
        //    //}

        //    //DontDestroyOnLoad(gameObject);
        //    //Application.runInBackground = true;
        //    _instance = this;
        //}

        public void Start()
        {
            PhotonPeer = new PhotonPeer(this, ConnectionProtocol.Udp);
            Connect();
        }

        public void Update()
        {
            if (PhotonPeer != null)
                PhotonPeer.Service();
        }

        void OnApplicationQuit()
        {
            Disconnect();
        }

        private void Connect()
        {
            if (PhotonPeer != null)
                PhotonPeer.Connect(CONNECTION_STRING, APP_NAME);
        }

        private void Disconnect()
        {
            if (PhotonPeer != null)
                PhotonPeer.Disconnect();
        }

        public void DebugReturn(DebugLevel level, string message)
        {
        }

        public void OnEvent(EventData eventData)
        {
            switch (eventData.Code)
            {
                case (byte)EventCode.ChatMessage:
                    ChatMessageHandler(eventData);
                        //Debug.Log("rect event:" + eventData.Parameters[1]);
                        Console.WriteLine("rect event:" + eventData.Parameters[1]);

                    break;
                default:
                    //Debug.Log("Unknown Event: " + eventData.Code);
                    Console.WriteLine("Unknown Event: " + eventData.Code);
                    break;
            }
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            switch (operationResponse.OperationCode)
            {
                case (byte)OperationCode.Login:

                    LoginHandler(operationResponse);

                    //if (operationResponse.Parameters.ContainsKey(1))
                    //{
                    //    //Debug.Log("rect:" + operationResponse.Parameters[1]);
                    //    Console.WriteLine("rect:" + operationResponse.Parameters[1]);
                    //    SendOperation2();
                    //}
                    break;
                default:
                    //Debug.Log("Unknown OperationResponse: " + operationResponse.OperationCode);
                    Console.WriteLine("Unknown OperationResponse: " + operationResponse.OperationCode);
                    break;

            }
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Connect:
                    Console.WriteLine("Connected to server!");
                    break;
                case StatusCode.Disconnect:
                    Console.WriteLine("Disconnected from server!");
                    break;
                case StatusCode.TimeoutDisconnect:
                    Console.WriteLine("TimeoutDisconnected from server!");
                    break;
                case StatusCode.DisconnectByServer:
                    Console.WriteLine("Disconnect By Server from server!");
                    break;
                case StatusCode.DisconnectByServerUserLimit:
                    Console.WriteLine("DisconnectByServerUserLimit from server!");
                    break;
                case StatusCode.DisconnectByServerLogic:
                    Console.WriteLine("DisconnectByServerLogic from server!");
                    break;
                default:
                    Console.WriteLine("Unknown status:" + statusCode.ToString());
                    break;
            }
        }

        private void LoginHandler(OperationResponse operationResponse)
        {
            if (operationResponse.ReturnCode != 0)
            {
                ErrorCode errorCode = (ErrorCode)operationResponse.ReturnCode;
                switch (errorCode)
                {
                    case ErrorCode.OK:
                        break;
                    case ErrorCode.InvalidParameters:
                        break;
                    case ErrorCode.NameIsExist:
                        if (OnLoginResponse != null)
                            OnLoginResponse(this, new LoginEventArgs(ErrorCode.NameIsExist));
                        break;
                    case ErrorCode.RequestNotImplemented:
                        break;
                    default:
                        Console.WriteLine("Error Login returnCode:" + operationResponse.ReturnCode);
                        break;
                }
                return;
            }
            if (OnLoginResponse != null)
                OnLoginResponse(this, new LoginEventArgs(ErrorCode.OK));
        }

        private void ChatMessageHandler(EventData eventData)
        {
            string message = (string)eventData.Parameters[(byte)ParameterCode.CharacterName];
            if (OnReceiveChatMessage != null)
                OnReceiveChatMessage(this, new ChatMessageEventArgs(message));
        }

        public void SendLoginOperation(string name)
        {
            PhotonPeer.OpCustom((byte)OperationCode.Login, new Dictionary<byte, object> { { (byte)ParameterCode.CharacterName, name } }, true);

        }


        public void SendChatMessage(string message)
        {
            PhotonPeer.OpCustom((byte)OperationCode.SendChatMessage, new Dictionary<byte, object> { { (byte)ParameterCode.ChatMessage, message} }, true);
        }

        public void GetRecentChatMessage()
        {
            PhotonPeer.OpCustom((byte)OperationCode.GetRecentChatMessages, null, true);
        }

    }
}
