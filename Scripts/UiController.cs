using System;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public InputField HostnameInput;
    public InputField PortInput;

    public GameObject CommunicatorGameObject;
    private Communicator Communicator;
    
    private void Start()
    {
        Communicator = CommunicatorGameObject.GetComponent<Communicator>();
    }

    public void TryToConnect()
    {
        var hostnameValue = HostnameInput.text;
        var portValue = Int32.Parse(PortInput.text);
        
        Communicator.JoinServer(hostnameValue, portValue);
    }
}
