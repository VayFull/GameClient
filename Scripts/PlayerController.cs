using System.Text;
using UnityEngine;

namespace GameClient.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterController _characterController;
        private Communicator Communicator;
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            Communicator = GameObject.FindWithTag("GameController").GetComponent<Communicator>();
        }
    
        void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                _characterController.Move(Vector3.forward * 0.1f);
                var currentPosition = transform.position;
                SendCurrentPosition(currentPosition);
            }

            if (Input.GetKey(KeyCode.S))
            {
                _characterController.Move(Vector3.back * 0.1f);
                var currentPosition = transform.position;
                SendCurrentPosition(currentPosition);
            }
        
            if (Input.GetKey(KeyCode.A))
            {
                _characterController.Move(Vector3.left * 0.1f);
                var currentPosition = transform.position;
                SendCurrentPosition(currentPosition);
            }

            if (Input.GetKey(KeyCode.D))
            {
                _characterController.Move(Vector3.right * 0.1f);
                var currentPosition = transform.position;
                SendCurrentPosition(currentPosition);
            }
        }

        private void SendCurrentPosition(Vector3 currentPosition)
        {
            var bytes = Encoding.ASCII.GetBytes($"pos={Communicator.Id}&pos?{currentPosition.x}:{currentPosition.y}:{currentPosition.z}");
            Communicator.Send(bytes);
        }
    }
}
