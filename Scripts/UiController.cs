using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameClient.Scripts
{
    public class UiController : MonoBehaviour
    {
        public InputField HostnameInput;
        public InputField PortInput;

        public GameObject CommunicatorGameObject;

        public GameObject MainMenu;

        public GameObject GameMenuGameObject;

        public Text ErrorMessageText;

        private void Start()
        {
            if (ErrorMessageText != null)
                ErrorMessageText.text = String.IsNullOrEmpty(SceneData.ErrorMessage) 
                    ? "Data" 
                    : SceneData.ErrorMessage;

            if (HostnameInput != null && PortInput != null)
            {
                if (!String.IsNullOrEmpty(SceneData.Hostname))
                {
                    HostnameInput.text = SceneData.Hostname;
                }

                if (SceneData.Port != 0)
                {
                    PortInput.text = SceneData.Port.ToString();
                }
            }
        }

        public void TryToConnect()
        {
            var hostnameValue = HostnameInput.text;
            var portValue = Int32.Parse(PortInput.text);

            /*var hostnameValue = "127.0.0.1";
            var portValue = 12000;*/

            SceneData.Hostname = hostnameValue;
            SceneData.Port = portValue;

            SceneManager.LoadScene("Game");
        }

        public void Resume()
        {
            GameMenuGameObject.SetActive(false);
        }

        public void Disconnect()
        {
            var communicator = CommunicatorGameObject.GetComponent<Communicator>();
            communicator.DisconnectFromServer();
        }
    }
}