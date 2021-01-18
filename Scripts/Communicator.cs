using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace GameClient.Scripts
{
    public class Communicator : MonoBehaviour
    {
        private UdpClient _udpClient;
        private IPEndPoint _serverEndpoint;
        public int Id;
        public GameObject Menu;
        public GameObject Player;
        public GameObject OtherPlayer;
        private GameObject PlayerGameObject;
        private bool _isConnected = false;
        private bool _createNewPlayer = false;
        private bool _isChangedPosition = false;
        private bool _needToDelete = false;
        private KeyValuePair<int, Vector3> _changedPositionKeyValue;
        private int _newIdInt = 0;
        private Dictionary<int, GameObject> playerDictionary = new Dictionary<int, GameObject>();
        private Dictionary<int, Vector3> positionDictionary = new Dictionary<int, Vector3>();
        private int _needToChangeId = 0;
        private int _needToDeleteId = 0;

        void Start()
        {
            _udpClient = new UdpClient();
        }

        private void Update()
        {
            if (_isConnected)
            {
                Menu.SetActive(false);
                PlayerGameObject = Instantiate(Player, new Vector3(0, 2, 0), Quaternion.identity);
                
                _isConnected = false;
                var playerController = PlayerGameObject.GetComponent<PlayerController>();
                playerController.GameMenu = GameObject.FindWithTag("GameMenu");
                playerController.GameMenu.SetActive(false);
                

                foreach (var otherPlayer in positionDictionary)
                {
                    var otherPlayerObject = Instantiate(OtherPlayer, otherPlayer.Value, Quaternion.identity);
                    playerDictionary[otherPlayer.Key] = otherPlayerObject;
                }
                positionDictionary.Clear();
            }

            if (_createNewPlayer)
            {
                var newPlayer = Instantiate(OtherPlayer, new Vector3(0, 2, 0), Quaternion.identity);
                playerDictionary[_newIdInt] = newPlayer;
                _createNewPlayer = false;
                _newIdInt = 0;
            }

            if (_isChangedPosition)
            {
                var changedObject = playerDictionary[_changedPositionKeyValue.Key];
                changedObject.transform.position = _changedPositionKeyValue.Value;
                playerDictionary[_needToChangeId].transform.position = _changedPositionKeyValue.Value;
                _isChangedPosition = false;
            }

            if (_needToDelete)
            {
                var playerToDelete = playerDictionary[_needToDeleteId];
                Destroy(playerToDelete);
                playerDictionary.Remove(_needToDeleteId);
                _needToDeleteId = 0;
                _needToDelete = false;
            }
        }

        public void JoinServer(string hostname, int port)
        {
            _serverEndpoint = new IPEndPoint(IPAddress.Parse(hostname), port);
            _udpClient.Connect(hostname, port);
            _udpClient.BeginReceive(ReceiveCallback, null);

            SendHelloPackage();
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var receivedBytes = _udpClient.EndReceive(ar, ref _serverEndpoint);
            var result = Encoding.ASCII.GetString(receivedBytes);
            Debug.Log(result);
            if (result.StartsWith("id="))
            {
                var resultParts = result.Split('*');
                var newClientId = resultParts[0].Split('=')[1];
                Debug.Log(result);
                if (!result.EndsWith("*"))
                {
                    var otherClients = resultParts[1];
                    if (otherClients.Contains("&"))
                    {
                        var clientPositions = resultParts[1].Split('&');

                        foreach (var clientPosition in clientPositions)
                        {
                            var dividedClientPosition = clientPosition.Split('?');
                            var otherPlayerPositions = dividedClientPosition[1].Split(':');
                            var otherPlayerPosition = new Vector3(float.Parse(otherPlayerPositions[0]),
                                float.Parse(otherPlayerPositions[1]), float.Parse(otherPlayerPositions[2]));
                            positionDictionary[int.Parse(dividedClientPosition[0])] = otherPlayerPosition;
                        }
                    }
                    else
                    {
                        var dividedClientPosition = otherClients.Split('?');
                        var otherPlayerPositions = dividedClientPosition[1].Split(':');
                        var otherPlayerPosition = new Vector3(float.Parse(otherPlayerPositions[0]),
                            float.Parse(otherPlayerPositions[1]), float.Parse(otherPlayerPositions[2]));
                        
                        positionDictionary[int.Parse(dividedClientPosition[0])] = otherPlayerPosition;
                    }
                    
                }
                
                Id = Int32.Parse(newClientId);
                _isConnected = true;
            }

            if (result.StartsWith("new:"))
            {
                var newId = result.Split(':')[1];
                _newIdInt = int.Parse(newId);
                _createNewPlayer = true;
            }

            if (result.StartsWith("pos="))
            {
                var parsedResult = result.Split('&');
                var positions = parsedResult[1].Split('?')[1].Split(':');
                var position = new Vector3(float.Parse(positions[0]), float.Parse(positions[1]),
                    float.Parse(positions[2]));

                var needToChangeId = int.Parse(parsedResult[0].Split('=')[1]);
                _isChangedPosition = true;
                _needToChangeId = needToChangeId;
                _changedPositionKeyValue = new KeyValuePair<int, Vector3>(needToChangeId, position);
            }

            if (result.StartsWith("disconnect:"))
            {
                var disconnectedId = int.Parse(result.Split(':')[1]);
                _needToDeleteId = disconnectedId;
                _needToDelete = true;
            }

            Debug.Log(result);
            _udpClient.BeginReceive(ReceiveCallback, null);
        }

        public void SendHelloPackage()
        {
            var bytes = Encoding.ASCII.GetBytes("hello");
            _udpClient.BeginSend(bytes, bytes.Length, SendCallback, null);
            Debug.Log("Hellopackagesent");
        }

        public void Send(byte[] bytes)
        {
            _udpClient.BeginSend(bytes, bytes.Length, SendCallback, null);
        }

        private void SendCallback(IAsyncResult ar)
        {
            _udpClient.EndSend(ar);
        }

        public void DisconnectFromServer()
        {
            var bytes = Encoding.ASCII.GetBytes($"disconnect:{Id}");
            Send(bytes);
            foreach (var otherPlayer in playerDictionary.Values)
                Destroy(otherPlayer);
            
            playerDictionary.Clear();
            positionDictionary.Clear();
            Destroy(PlayerGameObject);
        }
    }
}