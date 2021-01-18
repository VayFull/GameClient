using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameClient.Scripts
{
    public class UiController : MonoBehaviour
    {
        public InputField HostnameInput;
        public InputField PortInput;

        public GameObject CommunicatorGameObject;
        private Communicator Communicator;

        public GameObject MainMenu;

        public GameObject GameMenuGameObject;

        private void Start()
        {
            Communicator = CommunicatorGameObject.GetComponent<Communicator>();
        }

        public void TryToConnect()
        {
            /*var hostnameValue = HostnameInput.text;
            var portValue = Int32.Parse(PortInput.text);*/
            
            var hostnameValue = "127.0.0.1";
            var portValue = 12000;

            Communicator.JoinServer(hostnameValue, portValue);
        }

        public void Resume()
        {
            GameMenuGameObject.SetActive(false);
        }

        public void Disconnect()
        {
            Communicator.DisconnectFromServer();
            MainMenu.SetActive(true);
        }
    }
}
