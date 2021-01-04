using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Communicator : MonoBehaviour
{
    private UdpClient _udpClient;
    private IPEndPoint _serverEndpoint;
    public int Id;
    
    void Start()
    {
        _udpClient = new UdpClient();
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
        if (result.StartsWith("id:"))
        {
            var resultParts = result.Split(':');
            Id = Int32.Parse(resultParts[1]);
        }
        Debug.Log(result);
        _udpClient.BeginReceive(ReceiveCallback, null);
    }

    public void SendHelloPackage()
    {
        var bytes = Encoding.ASCII.GetBytes("hello");
        _udpClient.BeginSend(bytes, bytes.Length, SendCallback, null);
    }

    private void SendCallback(IAsyncResult ar)
    {
        _udpClient.EndSend(ar);
    }
}
