using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;
//using UnityEngine;
namespace TryToTestPhoton
{
    class PhotonServer : IPhotonPeerListener
    {
        private const string CONNECTION_STRING = "localhost:5055";
        private const string APP_NAME = "MyTestServer";

        //private static PhotonServer _instance;

        //public static PhotonServer Instance
        //{
        //    get { return _instance; }
        //}

        private PhotonPeer PhotonPeer
        {
            get;
            set;
        }

        //public void Awake()
        //{
        //    //if (Instance != null)
        //    //{
        //    //    DestroyObject(gameObject);
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
                case 1:
                    if (eventData.Parameters.ContainsKey(1))
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
                case 1:
                    if (operationResponse.Parameters.ContainsKey(1))
                    {
                        //Debug.Log("rect:" + operationResponse.Parameters[1]);
                        Console.WriteLine("rect:" + operationResponse.Parameters[1]);
                       // SendOperation2();
                    }
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
                    Debug.Log("Connected to server!");
                    // SendOperation();
                    break;
                case StatusCode.Disconnect:
                    Debug.Log("Disconnected from server!");
                    break;
                case StatusCode.TimeoutDisconnect:
                    Debug.Log("TimeoutDisconnected from server!");
                    break;
                case StatusCode.DisconnectByServer:
                    Debug.Log("Disconnect By Server from server!");
                    break;
                case StatusCode.DisconnectByServerUserLimit:
                    Debug.Log("DisconnectByServerUserLimit from server!");
                    break;
                case StatusCode.DisconnectByServerLogic:
                    Debug.Log("DisconnectByServerLogic from server!");
                    break;
                default:
                    Debug.Log("Unknown status:" + statusCode.ToString());
                    break;
            }
        }

        public void SendOperation()
        {
            PhotonPeer.OpCustom(1, new Dictionary<byte, object> { { 1, "send message" } }, false);
        }

        public void SendOperation2()
        {
            PhotonPeer.OpCustom(2, new Dictionary<byte, object> { { 1, "send message for event" } }, false);
        }

    }
}
